using Microsoft.AspNetCore.Components.Authorization;
using Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Services.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ITokenStorage _tokenService;

        public CustomAuthStateProvider(ITokenStorage cookieManager)
        {
            _tokenService = cookieManager;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = _tokenService.GetToken();

            var identity = string.IsNullOrWhiteSpace(token)
                ? new ClaimsIdentity()
                : GetClaimsIdentity(token);

            var user = new ClaimsPrincipal(identity);

            return Task.FromResult(new AuthenticationState(user));
        }

        public void MarkUserAsAuthenticated(string token)
        {
            _tokenService.SetToken(token);
            var identity = GetClaimsIdentity(token);
            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void MarkUserAsLoggedOut()
        {
            _tokenService.RemoveToken();
            var user = new ClaimsPrincipal(new ClaimsIdentity());

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
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
