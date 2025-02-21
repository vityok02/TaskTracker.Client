using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Infrastructure;
using Services.Interfaces;
using Services.Interfaces.Components;
using Services.Services;
using Services.Services.Components;

namespace Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICookieManager, CookieManager>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<ITokenStorage, TokenStorage>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IUserService, UserService>();
        services.AddValidatorsFromAssembly(AssemblyReference.Assembly, includeInternalTypes: true);

        return services;
    }
}
