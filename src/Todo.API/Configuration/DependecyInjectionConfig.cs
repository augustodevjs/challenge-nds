using Todo.Core.Interfaces;
using Todo.Infra.Repository;
using Todo.Infra.Interfaces;
using Todo.Services.Services;
using Todo.Core.Notifications;
using Todo.Services.Interfaces;

namespace Todo.API.Configuration;

public static class DependecyInjectionConfig
{
    public static void ResolveDependecies(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddSingleton(_ => builder.Configuration);
        
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<INotificator, Notificator>();
        services.AddScoped<IUserRepository, UserRepository>();
    }
}