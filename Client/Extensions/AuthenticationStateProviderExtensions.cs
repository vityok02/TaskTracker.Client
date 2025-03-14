using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Client.Extensions;

public static class AuthenticationStateProviderExtensions
{
    public static async Task<Guid?> GetUserIdAsync(
        this AuthenticationStateProvider authStateProvider)
    {
        var authState = await authStateProvider
            .GetAuthenticationStateAsync();

        var user = authState.User;

        if (user.Identity?.IsAuthenticated == true)
        {
            var subClaim = user
                .FindFirstValue(JwtRegisteredClaimNames.Sub);

            return Guid.TryParse(subClaim, out Guid userId)
                ? userId
                : null;
        }

        return null;
    }
}
