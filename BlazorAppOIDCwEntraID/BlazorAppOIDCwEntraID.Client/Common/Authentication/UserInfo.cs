namespace BlazorAppOIDCwEntraID.Client.Common.Authentication;
using System.Security.Claims;

// Add properties to this class and update the server and client AuthenticationStateProviders
// to expose more information about the authenticated user to the client.
public sealed class UserInfo
{
    public const string UserIdClaimType = "sub";
    public const string NameClaimType = "name";
    public const string RoleClaimType = "groups"; // or whatever your role claim type is, this is just the default for Azure AD / Entra ID
    
    public required string UserId { get; init; }
    public required string Name { get; init; }
    public List<string> Roles { get; init; } = [];

    public static UserInfo FromClaimsPrincipal(ClaimsPrincipal principal) => new()
    {
        UserId = GetRequiredClaim(principal, UserIdClaimType),
        Name = GetRequiredClaim(principal, NameClaimType),
        Roles = principal.FindAll(RoleClaimType).Select(claim => claim.Value).ToList(),
    };

    public ClaimsPrincipal ToClaimsPrincipal() => new(new ClaimsIdentity(
            claims: [
                new(UserIdClaimType, UserId),
                new(NameClaimType, Name),
                .. Roles.Select(role => new Claim(RoleClaimType, role))
                ],
            authenticationType: nameof(UserInfo),
            nameType: NameClaimType,
            roleType: RoleClaimType));

    private static string GetRequiredClaim(ClaimsPrincipal principal, string claimType) => principal.FindFirst(claimType)?.Value
            ?? throw new InvalidOperationException($"Could not find required '{claimType}' claim.");
}
