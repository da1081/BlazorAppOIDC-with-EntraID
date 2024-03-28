using BlazorAppOIDCwEntraID.Common.Authentication;
using BlazorAppOIDCwEntraID.Components;
using BlazorAppOIDCwEntraID.Client.Common.Authorization;
using BlazorAppOIDCwEntraID.WeatherForecaster;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// Add the Server-Side Blazor dependencies for authentication
builder.Services.AddAppAuthentication(builder.Configuration);

// Add the dependencies for authorization (Policies) to the Server-Side Blazor
builder.Services.AddAppAuthorization();

// For demonstration purposes, we will add the WeatherForecaster service to the server
builder.Services.AddServerWeatherForecaster();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorAppOIDCwEntraID.Client._Imports).Assembly);

// We map the login and logout endpoints to the app
app.MapLoginAndLogout();

// Demonstrate a weather service that can be accessed by both the client and the server
app.MapWeatherForecasterEndpoint();

app.Run();
