using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Todo.Application.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Todo.Application;

public static class DependecyInjection
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration,
        WebApplicationBuilder builder)
    {
        services.AddCorsConfig();
        services.ResolveDependecies(builder);
        services.AddAuthConfiguration(configuration);
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.Configure<ApiBehaviorOptions>(o => o.SuppressModelStateInvalidFilter = true);
    }
}