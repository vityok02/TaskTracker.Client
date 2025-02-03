using Refit;
using Services.Interfaces;

namespace Client.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection(TaskTrackerSettings.ConfigurationSection)
            .Get<TaskTrackerSettings>();

        var types = new[] { typeof(IUserService), typeof(IIdentityService) };

        foreach(var type in types)
        {
            services.AddRefitClient(type)
                .ConfigureHttpClient(httpClient => httpClient.BaseAddress = new Uri(settings!.BaseAddress));
        }

        return services;
    }
}
