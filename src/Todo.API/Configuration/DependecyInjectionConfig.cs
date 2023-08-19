using Todo.Domain.Models;
using Todo.Core.Interfaces;
using Todo.Infra.Interfaces;
using Todo.Infra.Repository;
using Todo.Services.Services;
using Todo.Core.Notifications;
using Todo.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using ScottBrady91.AspNetCore.Identity;

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