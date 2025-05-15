using AntDesign;
using Blazored.LocalStorage;
using Client.Authentication;
using Client.Configuration;
using Client.Constants;
using Client.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Plk.Blazor.DragDrop;
using Refit;
using Services.ExternalApi;

namespace Client.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthenticationAndAuthorization(configuration);
        services.AddExternalApis(configuration);
        services.AddMemoryCache();
        services.AddHttpContextAccessor();
        services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
        services.AddAntDesign();
        services.AddBlazorDragDrop();
        services.AddBlazoredLocalStorage();
        services.AddScoped<DeleteConfirmationService>();
        services.AddSignalR();

        services.Configure<CommentsHubOptions>(configuration
            .GetSection("CommentsHub"));

        LocaleProvider.DefaultLanguage = "en-US";
        LocaleProvider.SetLocale("en-US");

        return services;
    }

    private static IServiceCollection AddAuthenticationAndAuthorization(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<AuthHttpClientHandler>();

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, op =>
            {
                op.Cookie.Name = "jwtToken";
                op.LoginPath = "/login";
                op.LogoutPath = "/logout";
                op.AccessDeniedPath = "/access-denied";
                op.ReturnUrlParameter = "returnUrl";
                op.SlidingExpiration = true;
                op.ExpireTimeSpan = TimeSpan.FromDays(1);
                op.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Для Azure повинно бути на Secure
                op.Cookie.SameSite = SameSiteMode.Strict; // Як потрібно
            });
                
        services.AddAuthorization();

        services.AddCascadingAuthenticationState();

        return services;
    }

    private static IServiceCollection AddExternalApis(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<TaskTrackerSettings>()
            .BindConfiguration(TaskTrackerSettings.ConfigurationSection)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var settings = configuration.GetSection(TaskTrackerSettings.ConfigurationSection)
            .Get<TaskTrackerSettings>();

        var baseInterface = typeof(IApi);

        var types = typeof(IApi).Assembly
            .GetTypes()
            .Where(t => t.IsInterface && baseInterface.IsAssignableFrom(t) && t != baseInterface);

        foreach (var type in types)
        {
            services.AddRefitClient(type)
                .ConfigureHttpClient(httpClient =>
                    httpClient.BaseAddress = new Uri(Environment
                        .GetEnvironmentVariable(EnvironmentKeys.ApiUrl) ?? settings!.BaseAddress))
                .AddHttpMessageHandler<AuthHttpClientHandler>();
        }

        return services;
    }
}
