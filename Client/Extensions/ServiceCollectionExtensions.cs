using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Refit;
using Services.ExternalApi;
using Services.Interfaces;
using Services.Services;

namespace Client.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
          .AddCookie(options =>
          {
              options.ExpireTimeSpan = TimeSpan.FromHours(8);
              options.SlidingExpiration = true;
              options.AccessDeniedPath = "/Forbidden/";
          });

        services.AddTransient<AuthHttpClientHandler>();
        services.AddScoped<ITokenStorage, TokenStorage>();

        var settings = configuration.GetSection(TaskTrackerSettings.ConfigurationSection)
            .Get<TaskTrackerSettings>();

        var types = new[] { typeof(IUserService), typeof(IIdentityApi) };

        foreach(var type in types)
        {
            services.AddRefitClient(type)
                .ConfigureHttpClient(httpClient => 
                    httpClient.BaseAddress = new Uri(settings!.BaseAddress))
                .AddHttpMessageHandler<AuthHttpClientHandler>();
        }

        services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
        services.AddScoped<ProtectedSessionStorage>();
        services.AddHttpContextAccessor();
        services.AddMemoryCache();
        //services.AddAuthorizationCore();

        return services;
    }
}
