namespace BlazorAppOIDCwEntraID.Client.Common.Authorization;

using BlazorAppOIDCwEntraID.Client.Common.Authentication;
using Microsoft.AspNetCore.Authorization;

public static class Policies
{
    /// <summary>
    /// You should make sure that you have this policy when testing this solution.
    /// </summary>
    public const string IsCoolUser = "IsCoolUser";

    /// <summary>
    /// We use this policy to demonstrate what happens when the user is NOT authorized.
    /// </summary>
    public const string PineappleOnPizza = "PineappleOnPizza";

    public static AuthorizationPolicy IsCoolUserPolicy => new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .RequireClaim(UserInfo.UserIdClaimType, "PSyr6xTErUzMG45VxAUbpSC5PrlscAF0IK2KrsRrdpJ") // TODO: Make sure you are authorized by this policy. You can hardcode your userid here.
        .Build();

    public static AuthorizationPolicy PineappleOnPizzaPolicy => new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .RequireClaim("WantsPineappleOnPizzaClaimType", "wtf?!") // TODO: Make sure you are NOT authorized by this policy.
        .Build();

    // TODO: You can configure more policies here...
}
