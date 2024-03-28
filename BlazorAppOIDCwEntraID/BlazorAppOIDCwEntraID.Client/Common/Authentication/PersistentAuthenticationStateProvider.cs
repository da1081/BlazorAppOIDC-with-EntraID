namespace BlazorAppOIDCwEntraID.Client.Common.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;

internal sealed class PersistentAuthenticationStateProvider : AuthenticationStateProvider
{
    private static readonly Task<AuthenticationState> DefaultUnauthenticatedTask =
        Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

    private readonly Task<AuthenticationState> authenticationStateTask = DefaultUnauthenticatedTask;

    public PersistentAuthenticationStateProvider(PersistentComponentState state)
    {
        if (!state.TryTakeFromJson<UserInfo>(nameof(UserInfo), out var userInfo) || userInfo is null)
        {
            return;
        }

        authenticationStateTask = Task.FromResult(new AuthenticationState(userInfo.ToClaimsPrincipal()));
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync() => authenticationStateTask;
}
