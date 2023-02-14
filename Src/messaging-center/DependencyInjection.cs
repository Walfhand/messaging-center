using messaging_center.Impl;
using messaging_center.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace messaging_center
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMessagingCenter(this IServiceCollection services)
        {
            services.AddSingleton<IMessagingCenter, MessagingCenter>();
            return services;
        }
    }
}
