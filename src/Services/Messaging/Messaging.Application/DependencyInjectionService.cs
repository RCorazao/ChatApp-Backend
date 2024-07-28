using AutoMapper;
using Messaging.Application.Interfaces;
using Messaging.Application.Mapper;
using Messaging.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Messaging.Application
{
    public static class DependencyInjectionService
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var mapper = new MapperConfiguration(config =>
            {
                config.AddProfile(new MapperProfile());
            });
            services.AddSingleton(mapper.CreateMapper());

            services.AddHttpContextAccessor();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IChatService, ChatService>();

            return services;
        }
    }
}
