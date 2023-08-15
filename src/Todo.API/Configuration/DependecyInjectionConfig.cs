using Todo.Core.Interfaces;
using Todo.Core.Notifications;

namespace Todo.API.Configuration;

public static class DependecyInjectionConfig
{
    public static void ResolveDependecies(this IServiceCollection services)
    {
        services.AddScoped<INotificator, Notificator>();
    }
}