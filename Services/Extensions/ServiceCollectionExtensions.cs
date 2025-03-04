using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.ApiServices;
using Services.Infrastructure;
using Services.Interfaces;
using Services.Interfaces.ApiServices;
using Services.Services;

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
        services.AddScoped<ITaskService, TaskService>();
        services.AddValidatorsFromAssembly(AssemblyReference.Assembly, includeInternalTypes: true);

        return services;
    }
}
