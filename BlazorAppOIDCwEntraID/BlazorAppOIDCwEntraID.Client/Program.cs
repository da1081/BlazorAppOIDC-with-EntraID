using BlazorAppOIDCwEntraID.Client.Common.Authentication;
using BlazorAppOIDCwEntraID.Client.Common.Authorization;
using BlazorAppOIDCwEntraID.Client.WeatherForecaster;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Add the client dependencies for authentication
builder.Services.AddClientAuthentication();

// Add the dependencies for authorization (Policies) to the client
builder.Services.AddAppAuthorization();

// Lets add a simple HttpClient with the base address of the server. The Authorization will be included in every request using a cookie.
builder.Services.AddScoped<HttpClient>(clinet => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// For demonstration purposes, we will add the WeatherForecaster service to the client
builder.Services.AddClientWeatherForecaster();

await builder.Build().RunAsync();
