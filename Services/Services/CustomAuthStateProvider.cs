using Domain.Responses;
using Microsoft.AspNetCore.Components.Authorization;
using Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Services.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ITokenStorage _tokenService;
        private ClaimsPrincipal _user;

        public CustomAuthStateProvider(ITokenStorage cookieManager)
        {
            _tokenService = cookieManager;
            _user = new ClaimsPrincipal(new ClaimsIdentity());
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(new AuthenticationState(_user));
        }

        public void MarkUserAsAuthenticated(string token)
        {
            _tokenService.SetToken(token);
            var identity = GetClaimsIdentity(token);
            _user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_user)));
        }

        public void MarkUserAsLoggedOut()
        {
            _tokenService.RemoveToken();
            _user = new ClaimsPrincipal(new ClaimsIdentity());

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_user)));
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
