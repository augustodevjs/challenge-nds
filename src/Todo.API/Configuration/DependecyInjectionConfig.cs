using Todo.Domain.Models;
using Microsoft.AspNetCore.Identity;
using ScottBrady91.AspNetCore.Identity;
using Todo.Application.Contracts;
using Todo.Application.Notifications;
using Todo.Application.Services;
using Todo.Domain.Contracts.Repository;
using Todo.Infra.Data.Repositories;

namespace Todo.API.Configuration;

public static class DependecyInjectionConfig
{
    public static void ResolveDependecies(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddSingleton(_ => builder.Configuration);
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAssignmentListRepository, AssignmentListRepository>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAssignmentListService, AssignmentListService>();

        services.AddScoped<INotificator, Notificator>();
        services.AddScoped<IPasswordHasher<User>, Argon2PasswordHasher<User>>();
    }
}