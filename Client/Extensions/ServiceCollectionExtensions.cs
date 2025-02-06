using Microsoft.AspNetCore.Authentication.Cookies;
using Refit;
using Services.ExternalApi;

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

        var settings = configuration.GetSection(TaskTrackerSettings.ConfigurationSection)
            .Get<TaskTrackerSettings>();

        var types = new[] { typeof(IUserApi), typeof(IIdentityApi) };

        foreach(var type in types)
        {
            services.AddRefitClient(type)
                .ConfigureHttpClient(httpClient => 
                    httpClient.BaseAddress = new Uri(settings!.BaseAddress))
                .AddHttpMessageHandler<AuthHttpClientHandler>();
        }

        services.AddMemoryCache();
        services.AddHttpContextAccessor();
        //services.AddAuthorizationCore();
        services.AddCascadingAuthenticationState();

        return services;
    }
}
