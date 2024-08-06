using Messaging.Application.Interfaces;
using Messaging.External.Proxies;
using Messaging.External.Proxies.User;
using Messaging.External.SignalR;
using Microsoft.AspNetCore.SignalR;
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

            // SignalR
            services.AddSignalR();
            services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
            services.AddSingleton<INotificationService, NotificationService>();


            return services;
        }
    }
}
