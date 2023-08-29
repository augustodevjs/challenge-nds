using Todo.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Todo.Infra.Data;

public static class DependecyInjection
{
    public static void AddInfraData(this IServiceCollection services, string? connectionString)
    {
        var serverVersion = new MySqlServerVersion(new Version(10, 4, 27));
        services.AddDbContext<TodoDbContext>(options => options.UseMySql(connectionString, serverVersion));
    }
}