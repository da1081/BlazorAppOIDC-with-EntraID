using BlazorAppOIDCwEntraID.Client.WeatherForecaster;
using BlazorAppOIDCwEntraID.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAppOIDCwEntraID.WeatherForecaster;

public static class DependencyInjection
{
    public static IServiceCollection AddServerWeatherForecaster(this IServiceCollection services)
    {
        services.AddScoped<IWeatherForecaster, ServerWeatherForecaster>();
        return services;
    }

    public static void MapWeatherForecasterEndpoint(this WebApplication app)
    {
        app.MapGet("/weather-forecast", ([FromServices] IWeatherForecaster WeatherForecaster) =>
        {
            return WeatherForecaster.GetWeatherForecastAsync();
        }).RequireAuthorization();
    }
}
