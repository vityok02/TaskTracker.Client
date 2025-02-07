﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Refit;
using Services.ExternalApi;
using Services.Interfaces;
using System.Text;

namespace Client.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApi(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthenticationAndAuthorization(configuration);

        services.AddExternalApis(configuration);

        services.AddMemoryCache();
        services.AddHttpContextAccessor();

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
                    var cache = context.HttpContext.RequestServices.GetService<ITokenStorage>();

                    context.Token = cache!.GetToken() ?? string.Empty;

                    return Task.CompletedTask;
                }
            };
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

        var types = new[] { typeof(IUserApi), typeof(IIdentityApi) };

        foreach (var type in types)
        {
            services.AddRefitClient(type)
                .ConfigureHttpClient(httpClient =>
                    httpClient.BaseAddress = new Uri(settings!.BaseAddress))
                .AddHttpMessageHandler<AuthHttpClientHandler>();
        }

        return services;
    }
}
