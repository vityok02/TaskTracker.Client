using AntDesign;
using Blazored.LocalStorage;
using Client.Authentication;
using Client.Configuration;
using Client.Constants;
using Client.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Plk.Blazor.DragDrop;
using Refit;
using Services.ExternalApi;
using Services.Interfaces;
using System.Text;

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
        services.AddScoped<DeleteStateConfirmationService>();
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

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(op =>
        {
            op.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true
            };
            op.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var cache = context.HttpContext.RequestServices
                    .GetService<ITokenStorage>();

                    context.Token = cache!.GetToken().Result
                        ?? string.Empty;

                    return Task.CompletedTask;
                }
            };
        });

        services.AddAuthorizationCore();

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
