using Blazored.LocalStorage;
using Blazored.Toast;
using Blazored.Toast.Services;
using FinalProject.Domain.Services;
using FinalProject.Wasm.Services;
using FinalProject.Wasm;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
//builder.Configuration.AddJsonFile("apsettings.json");


//var titanicFluteApiUrl = builder.Configuration.GetConnectionString("TitanicFlute");

//var apiUrl = builder.Configuration.GetSection("ApiUrl").Value!;

//var signalRUrl = builder.Configuration.GetSection("SignalRUrl").Value!;

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var titanicFluteApiUrl = "https://youtu.be/X2WH8mHJnhM";
var apiUrl = "https://localhost:7296/api/";
var signalRUrl = "https://localhost:7296/Hubs";

//var uriString = "https://localhost:7296/api/";

// if (uriString == null)
// {
//     throw new ArgumentNullException("uriString");
// }

// var uri = new Uri(uriString);



builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiUrl) });

builder.Services.AddBlazoredToast();

builder.Services.AddScoped<IToasterService, BlazoredToastService>();

builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});

//builder.Services.AddSingleton(typeof(LoggerBase));

builder.Services.AddSingleton<IUrlHelperService>(new UrlHelperService(titanicFluteApiUrl, signalRUrl));

builder.Services.AddBlazoredLocalStorage(config =>
{
    config.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    config.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    config.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
    config.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    config.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    config.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
    config.JsonSerializerOptions.WriteIndented = false;
});

await builder.Build().RunAsync();