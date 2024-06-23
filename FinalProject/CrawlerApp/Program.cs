using Application.Common.Models.Order;
using Application.Common.Models.Product;
using Application.Common.Models.SeleniumLog;
using Application.Features.OrderEvents.Commands.Add;
using Application.Features.Orders.Commands.Update;
using Application.Features.Products.Commands.Add;
using Domain.Enums;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Text;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

class Program
{
    private static int requestedAmount;
    private static ProductCrawlType productCrawlType;
    private static string email;

    private static readonly HttpClient client = new HttpClient();

    static async Task Main(string[] args)
    {
        Console.WriteLine("Selenium Crawler");

        var orderHubConnection = await InitializeOrderHubConnectionAsync();
        var logHubConnection = await InitializeLogHubConnectionAsync();

        orderHubConnection.On<OrderAddDto>("NewOrderAdded", async orderAddDto =>
        {
            InitializeWebDriver(out IWebDriver driver, out string baseUrl);

            requestedAmount = orderAddDto.RequestedAmount;
            productCrawlType = orderAddDto.ProductCrawlType;
            email = orderAddDto.Email;

            Console.WriteLine($"Received Order - Requested Amount: {requestedAmount}, Product Crawl Type: {productCrawlType}, Email: {email}");
            orderAddDto.Id = Guid.NewGuid();

            var response = await AddOrderAsync(orderAddDto);

            if (response.IsSuccessStatusCode)
            {
                await AddOrderEventAsync(orderAddDto.Id, email, OrderStatus.CrawlingStarted);
                
                bool isSuccess = await StartCrawlingAsync(driver, logHubConnection, baseUrl, requestedAmount, productCrawlType, orderAddDto.Id, email);

                if (isSuccess) await AddOrderEventAsync(orderAddDto.Id, email, OrderStatus.CrawlingCompleted);

            }
            driver.Quit();


        });
        await Task.Delay(Timeout.Infinite);
    }

    static async Task<HubConnection> InitializeOrderHubConnectionAsync()
    {
        var orderHubConnection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7010/Hubs/OrderHub")
            .WithAutomaticReconnect()
            .Build();

        await orderHubConnection.StartAsync();
        Console.WriteLine("OrderHub connection started");

        return orderHubConnection;
    }

    static async Task<HubConnection> InitializeLogHubConnectionAsync()
    {
        var logHubConnection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7010/Hubs/SeleniumLogHub")
            .WithAutomaticReconnect()
            .Build();

        await logHubConnection.StartAsync();
        Console.WriteLine("LogHub connection started");

        return logHubConnection;
    }

    static void InitializeWebDriver(out IWebDriver driver, out string baseUrl)
    {
        new DriverManager().SetUpDriver(new ChromeConfig());
        driver = new ChromeDriver();
        baseUrl = "http://192.168.1.112:5500/";
    }

    static async Task<bool> StartCrawlingAsync(IWebDriver driver, HubConnection logHubConnection, string baseUrl,
        int requestedAmount, ProductCrawlType productCrawlType, Guid orderId, string orderEmail)
    {
        int pageNumber = 1;
        int totalProductsFetched = 0;
        var allProductDtos = new List<ProductDto>();

        try
        {
            await LogAsync(logHubConnection, "Bot started");
            await AddOrderEventAsync(orderId, orderEmail, OrderStatus.BotStarted);

            while (totalProductsFetched < requestedAmount)
            {
                string nextPageUrl = $"{baseUrl}/{pageNumber}.html";
                var (newProductDtos, productsFetched) = await NavigateAndCrawlPageAsync(driver, logHubConnection, nextPageUrl, productCrawlType, orderId);
                allProductDtos.AddRange(newProductDtos);
                totalProductsFetched += productsFetched;

                if (totalProductsFetched >= requestedAmount || !HasNextPage(driver))
                {
                    break;
                }

                pageNumber++;
            }

            if (allProductDtos.Count > requestedAmount)
            {
                allProductDtos = allProductDtos.Take(requestedAmount).ToList();
            }

            await UpdateOrderAsync(orderId, allProductDtos.Count);
            await AddProductsToDatabase(allProductDtos, orderId);

            return true;
        }
        catch (Exception ex)
        {
            await LogAsync(logHubConnection, $"Error: {ex.Message}");
            await AddOrderEventAsync(orderId, orderEmail, OrderStatus.CrawlingFailed);
            return false;
        }
    }
    static async Task LogAsync(HubConnection logHubConnection, string message)
    {
        await logHubConnection.InvokeAsync("SendLogNotificationAsync", new SeleniumLogDto(message));
    }

    static async Task<(List<ProductDto>, int)> NavigateAndCrawlPageAsync(IWebDriver driver, HubConnection logHubConnection,
        string url, ProductCrawlType productCrawlType, Guid orderId)
    {
        await NavigateToPageAsync(driver, logHubConnection, url);

        var productElements = driver.FindElements(By.CssSelector("div.col.mb-5"));
        var imageElements = driver.FindElements(By.TagName("img"));

        var productDtos = productElements.Select((productElement, index) =>
            ConvertToProductDto(productElement, imageElements[index])
        ).ToList();

        var filteredProductDtos = FilterProducts(productDtos, productCrawlType);

        await LogAsync(logHubConnection, $"Page crawled. Found {filteredProductDtos.Count} products.");


        return (filteredProductDtos, filteredProductDtos.Count);
    }

    static async Task NavigateToPageAsync(IWebDriver driver, HubConnection logHubConnection, string url)
    {
        driver.Navigate().GoToUrl(url);
        await LogAsync(logHubConnection, $"Navigated to {url}");
    }

    static bool HasNextPage(IWebDriver driver)
    {
        try
        {
            string nextLinkSelector = ".page-item .page-link.next-page";
            driver.FindElement(By.CssSelector(nextLinkSelector));
            return true;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }

    static ProductDto ConvertToProductDto(IWebElement productElement, IWebElement imageElement)
    {
        var productName = productElement.FindElement(By.CssSelector("h5.product-name")).Text;

        var priceElement = productElement.FindElement(By.CssSelector("span.price"));
        var priceText = priceElement.Text;

        var salePriceElement = productElement.FindElements(By.CssSelector("span.sale-price"))
            .FirstOrDefault();
        var salePriceText = salePriceElement?.Text;

        var imageSrc = imageElement.GetAttribute("src");

        decimal price = ParseDecimal(priceText);
        decimal salePrice = salePriceElement != null ? ParseDecimal(salePriceText) : price;

        return new ProductDto
        {
            Name = productName,
            Price = price,
            IsOnSale = salePriceElement != null,
            SalePrice = salePrice,
            Picture = imageSrc
        };
    }

    static List<ProductDto> FilterProducts(List<ProductDto> productDtos, ProductCrawlType productCrawlType)
    {
        return productCrawlType switch
        {
            ProductCrawlType.All => productDtos,
            ProductCrawlType.OnDiscount => productDtos.Where(p => p.IsOnSale).ToList(),
            ProductCrawlType.NonDiscount => productDtos.Where(p => !p.IsOnSale).ToList(),
        };
    }

    static decimal ParseDecimal(string value)
    {
        if (string.IsNullOrEmpty(value)) return 0;
        decimal result;
        value = value.Replace("$", "").Replace(",", "").Trim();
        return decimal.TryParse(value, out result) ? result : 0;
    }

    static async Task AddProductsToDatabase(List<ProductDto> productDtos, Guid orderId)
    {
        foreach (var productDto in productDtos)
        {
            var productAddCommand = new ProductAddCommand
            {
                OrderId = orderId,
                Name = productDto.Name,
                Picture = productDto.Picture,
                IsOnSale = productDto.IsOnSale,
                Price = productDto.Price,
                SalePrice = productDto.SalePrice
            };
            
            var json = JsonConvert.SerializeObject(productAddCommand);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await client.PostAsync("https://localhost:7010/api/Products/Add", content);
        }
    }

    static async Task AddOrderEventAsync(Guid orderId, string email, OrderStatus status)
    {
        var json = JsonConvert.SerializeObject(new OrderEventAddCommand(orderId, email, status));
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        await client.PostAsync("https://localhost:7010/api/OrderEvents/Add", content);
    }

    static async Task UpdateOrderAsync(Guid orderId, int productCount)
    {
        var updateJson = JsonConvert.SerializeObject(new OrderUpdateCommand(orderId, productCount));
        var updateContent = new StringContent(updateJson, Encoding.UTF8, "application/json");

        await client.PostAsync("https://localhost:7010/api/Orders/UpdateById", updateContent);
    }

    static async Task<HttpResponseMessage> AddOrderAsync(OrderAddDto orderAddDto)
    {
        var json = JsonConvert.SerializeObject(orderAddDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync("https://localhost:7010/api/Orders/Add", content);
        return response;
    }
}
