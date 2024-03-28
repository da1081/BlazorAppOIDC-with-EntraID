namespace BlazorAppOIDCwEntraID.Client.Common.Authentication;

using Microsoft.AspNetCore.Components.Authorization;

public static class DependencyInjection
{
    internal static IServiceCollection AddClientAuthentication(this IServiceCollection services)
    {
        services.AddCascadingAuthenticationState();
        services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

        return services;
    }
}
