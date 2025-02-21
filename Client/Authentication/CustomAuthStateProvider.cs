using Microsoft.AspNetCore.Components.Authorization;
using Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Client.Authentication
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ITokenStorage _tokenService;

        public CustomAuthStateProvider(ITokenStorage cookieManager)
        {
            _tokenService = cookieManager;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _tokenService.GetToken();

            var identity = string.IsNullOrWhiteSpace(token)
                ? new ClaimsIdentity()
                : GetClaimsIdentity(token);

            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }

        private static ClaimsIdentity GetClaimsIdentity(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var claims = jwtToken.Claims;

            return new ClaimsIdentity(claims, "jwt");
        }
    }
}
