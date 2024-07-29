using Messaging.Application.Interfaces;
using Messaging.External.Proxies;
using Messaging.External.Proxies.User;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Messaging.External
{
    public static class DependencyInjectionService
    {
        public static IServiceCollection AddExternal(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<ApiUrls>(opts => configuration.GetSection("ApiUrls").Bind(opts));

            // Proxies
            services.AddHttpClient<IUserServiceProxy, UserServiceProxy>();

            return services;
        }
    }
}
