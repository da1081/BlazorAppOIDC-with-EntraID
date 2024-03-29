namespace BlazorAppOIDCwEntraID.Client.Common.Authorization;

public static class DependencyInjection
{
    /// <summary>
    /// This should be used both in the client and the server to add the authorization services and policies.
    /// </summary>
    public static IServiceCollection AddAppAuthorization(this IServiceCollection services)
    {
        services.AddAuthorizationCore(options =>
        {
            options.AddPolicy(Policies.IsCoolUser, Policies.IsCoolUserPolicy); // This is to demonstrate that you are authorized.
            options.AddPolicy(Policies.PineappleOnPizza, Policies.PineappleOnPizzaPolicy); // This is to demonstrate that you are NOT authorized.
        }); // We will add more policies later...

        return services;
    }
}
