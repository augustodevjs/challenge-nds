using Todo.API.Token;
using Todo.Core.Interfaces;
using Todo.Core.Notifications;
using Todo.Infra.Interfaces;
using Todo.Infra.Repository;
using Todo.Services.Interfaces;
using Todo.Services.Services;

namespace Todo.API.Configuration;

public static class DependecyInjectionConfig
{
    public static void ResolveDependecies(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<INotificator, Notificator>();
        services.AddSingleton(_ => builder.Configuration);
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITokenGenerator, TokenGenerator>();
    }
}