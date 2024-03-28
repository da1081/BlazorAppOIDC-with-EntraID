namespace BlazorAppOIDCwEntraID.Common.Authentication;

public class AuthenticationOptions
{
    public const string Section = "Authentication";

    public required string TenantId { get; set; }
    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
}

public static class AuthenticationOptionsExtensions
{
    public static (bool isValid, string errorMessage) IsValid(this AuthenticationOptions? options)
    {
        if (options is null)
        {
            return (false, "AuthenticationOptions is required");
        }

        if (string.IsNullOrWhiteSpace(options.TenantId))
        {
            return (false, "TenantId is required");
        }

        if (string.IsNullOrWhiteSpace(options.ClientId))
        {
            return (false, "ClientId is required");
        }

        if (string.IsNullOrWhiteSpace(options.ClientSecret))
        {
            return (false, "ClientSecret is required");
        }

        return (true, string.Empty);
    }
}
