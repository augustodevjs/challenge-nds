using Todo.Domain.Models;
using Todo.Infra.Repository;
using Todo.Services.Services;
using Microsoft.AspNetCore.Identity;
using ScottBrady91.AspNetCore.Identity;
using Todo.Infra.Contracts;
using Todo.Services.Contracts;
using Todo.Services.Notifications;

namespace Todo.API.Configuration;

public static class DependecyInjectionConfig
{
    public static void ResolveDependecies(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddSingleton(_ => builder.Configuration);
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddScoped<INotificator, Notificator>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAssignmentListRepository, AssignmentListRepository>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAssignmentListService, AssignmentListService>();

        builder.Services.AddScoped<IPasswordHasher<User>, Argon2PasswordHasher<User>>();
    }
}