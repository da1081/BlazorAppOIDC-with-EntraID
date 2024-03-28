using System.Net.Http.Json;

namespace BlazorAppOIDCwEntraID.Client.WeatherForecaster;

public static class DependencyInjection
{
    public static IServiceCollection AddClientWeatherForecaster(this IServiceCollection services)
    {
        services.AddScoped<IWeatherForecaster, ClientWeatherForecaster>();
        return services;
    }
}

public class ClientWeatherForecaster(HttpClient httpClient) : IWeatherForecaster
{
    private readonly HttpClient httpClient = httpClient;

    public async Task<IEnumerable<WeatherForecast>> GetWeatherForecastAsync() => await this.httpClient.GetFromJsonAsync<WeatherForecast[]>("/weather-forecast")
            ?? throw new Exception("The weather stopped weathering??");
}
