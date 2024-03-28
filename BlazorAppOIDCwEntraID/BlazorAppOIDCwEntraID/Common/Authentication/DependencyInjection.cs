namespace BlazorAppOIDCwEntraID.Common.Authentication;

using BlazorAppOIDCwEntraID.Client.Common;
using BlazorAppOIDCwEntraID.Common.Authentication.CookieRefresher;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

public static class DependencyInjection
{
    public const string SchemeName = "MicrosoftOidc";

    /// <summary>
    /// Responsible for adding the authentication services to the application.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public static IServiceCollection AddAppAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        // Get and validate the authentication options
        var options = configuration.GetSection(AuthenticationOptions.Section).Get<AuthenticationOptions>();
        var (isValied, errorMessage) = options.IsValid();
        if (!isValied || options is null)
        {
            throw new InvalidOperationException(errorMessage);
        }

        // Get a logger
        var logger = services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();

        services.AddAuthentication(SchemeName)
               .AddOpenIdConnect(SchemeName, configureOptions =>
               {
                   configureOptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                   configureOptions.Authority = $"https://login.microsoftonline.com/{options.TenantId}/v2.0/";
                   configureOptions.ClientId = options.ClientId;
                   configureOptions.ClientSecret = options.ClientSecret;
                   configureOptions.ResponseType = OpenIdConnectResponseType.Code;
                   configureOptions.MapInboundClaims = false;
                   configureOptions.UsePkce = true;
                   configureOptions.Events = new()
                   {
                       OnRemoteFailure = context =>
                       {
                           context.HandleResponse(); // TODO: Some stupid preflight request is causing errors. I can't figure out how to fix it.
                           return Task.CompletedTask;
                       }
                   };
               })
               .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
               {
                   options.AccessDeniedPath = RouteConstants.AccessDeniedPath;
                   options.LogoutPath = RouteConstants.LogoutPath;
                   options.LoginPath = RouteConstants.LoginPath;
               });
        
        services.AddCookieRefresher(CookieAuthenticationDefaults.AuthenticationScheme, SchemeName);

        services.AddCascadingAuthenticationState();

        services.AddScoped<AuthenticationStateProvider, PersistingAuthenticationStateProvider>();

        return services;
    }

    internal static IEndpointConventionBuilder MapLoginAndLogout(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/");

        group.MapGet(RouteConstants.LoginPath, (string? returnUrl) => TypedResults.Challenge(CreateAuthenticationProperties(returnUrl))).AllowAnonymous();

        group.MapPost(RouteConstants.LogoutPath, ([FromForm] string? returnUrl) => TypedResults.SignOut(CreateAuthenticationProperties(returnUrl), [CookieAuthenticationDefaults.AuthenticationScheme, SchemeName]));

        return group;
    }

    private static AuthenticationProperties CreateAuthenticationProperties(string? returnUrl, HttpContext? httpContext = null)
    {
        var basePath = "/";
        if (httpContext != null)
        {
            basePath = httpContext.Request.PathBase.HasValue ? httpContext.Request.PathBase.Value : "/";
        }

        if (string.IsNullOrEmpty(returnUrl))
        {
            returnUrl = basePath;
        }
        else
        {
            if (Uri.TryCreate(returnUrl, UriKind.Absolute, out var uri))
            {
                returnUrl = uri.PathAndQuery;
            }
            else if (!returnUrl.StartsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                returnUrl = $"{basePath}{returnUrl}";
            }
            else
            {
                returnUrl = basePath;
            }
        }

        return new AuthenticationProperties { RedirectUri = returnUrl };
    }
}
