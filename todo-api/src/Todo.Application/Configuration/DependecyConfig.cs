using Todo.Domain.Models;
using Todo.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Todo.Infra.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Todo.Application.Notifications;
using Todo.Domain.Contracts.Repository;
using ScottBrady91.AspNetCore.Identity;
using Todo.Application.Contracts.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Todo.Application.Configuration;

public static class DependecyConfig
{
    public static void ResolveDependecies(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddSingleton(_ => builder.Configuration);
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAssignmentRepository, AssignmentRepository>();
        services.AddScoped<IAssignmentListRepository, AssignmentListRepository>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAssignmentService, AssignmentService>();
        services.AddScoped<IAssignmentListService, AssignmentListService>();

        services.AddScoped<INotificator, Notificator>();
        services.AddScoped<IPasswordHasher<User>, Argon2PasswordHasher<User>>();
    }
}