using System.Net.Http.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using FinalProject.Domain.Dtos;
using FinalProject.Domain.Entities;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

Console.WriteLine("Selenium Crawler");


new DriverManager().SetUpDriver(new ChromeConfig());

IWebDriver driver = new ChromeDriver();
var hubConnection = new HubConnectionBuilder()
    .WithUrl($"https://localhost:7296/Hubs/SeleniumLogHub")
    .WithAutomaticReconnect()
    .Build();

await hubConnection.StartAsync();


try
{
    await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog("Bot started."));

    string baseUrl = "https://finalproject.dotnet.gg/";
    driver.Navigate().GoToUrl(baseUrl);
    await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog("Navigated to website"));

    OrderAddDto orderAddDto = new OrderAddDto();

    int requestedAmount;
    bool validAmount = false;

    while (!validAmount)
    {
        Console.WriteLine("How many products do you want to crawl?");

        string requestedAmountStr = Console.ReadLine();

        if (int.TryParse(requestedAmountStr, out requestedAmount))
        {
            orderAddDto.RequestedAmount = requestedAmount;
            validAmount = true;
        }
        else
        {
            Console.WriteLine("Not a valid number. Please try again.");
        }
    }

    string choice = string.Empty;
    bool validChoice = false;

    while (!validChoice)
    {
        Console.WriteLine("Which products do you want to crawl?");
        Console.WriteLine("A) All");
        Console.WriteLine("B) On Sale");
        Console.WriteLine("C) Normal");

        choice = Console.ReadLine();

        switch (choice.ToUpper())
        {
            case "A":
                orderAddDto.ProductCrawlType = ProductCrawlType.All;
                validChoice = true;
                break;
            case "B":
                orderAddDto.ProductCrawlType = ProductCrawlType.OnSale;
                validChoice = true;
                break;
            case "C":
                orderAddDto.ProductCrawlType = ProductCrawlType.Normal;
                validChoice = true;
                break;
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                break;
        }
    }

    var httpClient = new HttpClient();

    string json = JsonConvert.SerializeObject(orderAddDto);

    await httpClient.PostAsync("https://localhost:7296/api/Orders", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));


    //Console.ReadLine();

    int pageNumber = 2;
    List<Product> products = new List<Product>();


    while (true)
    {
        string nextPageUrl = $"{baseUrl}?currentPage={pageNumber}";

        driver.Navigate().GoToUrl(nextPageUrl);

        pageNumber++;

        IList<IWebElement> productElements = driver.FindElements(By.CssSelector("div.col.mb-5"));

        IList<IWebElement> images = driver.FindElements(By.TagName("img"));


        for (int i = 0; i < productElements.Count; i++)
        {
            IWebElement product = productElements[i];
            IWebElement img = images[i];

            Product p = new Product();
            p.Id = Guid.NewGuid();
            p.Picture = img.GetAttribute("src");
            
            string[] parts = product.Text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
           
            if (product.Text.Contains("Sale"))
            {
                p.IsOnSale = true;
                p.Name = parts[1].Trim();
                p.Price = decimal.Parse(parts[2].Replace("$", "").Split()[0]);
                p.SalePrice = decimal.Parse(parts[2].Replace("$", "").Split()[1]);
            }else
            {
                p.Name = parts[0].Trim();
                p.Price = decimal.Parse(parts[1].Replace("$", ""));
                p.IsOnSale = false;
            }
                    
            products.Add(p);
        }

        try
        {
            string nextLinkSelector = $".page-item .page-link.next-page";
            IWebElement nextLink = driver.FindElement(By.CssSelector(nextLinkSelector));
        }
        catch (NoSuchElementException)
        {
            break;
        }
    }

    Console.WriteLine("bitti");

    await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog("Bot stopped."));


}
catch (Exception exception)
{
    driver.Quit();
}

Console.WriteLine("bitti");

SeleniumLogDto CreateLog(string message) => new SeleniumLogDto(message);