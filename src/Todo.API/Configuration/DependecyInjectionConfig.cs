using Todo.API.Token;
using Todo.Core.Interfaces;
using Todo.Core.Notifications;

namespace Todo.API.Configuration;

public static class DependecyInjectionConfig
{
    public static void ResolveDependecies(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddScoped<INotificator, Notificator>();
        services.AddSingleton(_ => builder.Configuration);
        services.AddScoped<ITokenGenerator, TokenGenerator>();
    }
}