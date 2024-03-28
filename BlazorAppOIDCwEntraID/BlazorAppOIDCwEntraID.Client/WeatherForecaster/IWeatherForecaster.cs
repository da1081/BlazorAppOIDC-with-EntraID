namespace BlazorAppOIDCwEntraID.Client.WeatherForecaster;

public interface IWeatherForecaster
{
    Task<IEnumerable<WeatherForecast>> GetWeatherForecastAsync();
}
