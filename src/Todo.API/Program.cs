using Microsoft.EntityFrameworkCore;
using Todo.API.Configuration;
using Todo.Infra.Context;

var builder = WebApplication.CreateBuilder(args);

var serverVersion = new MySqlServerVersion(new Version(10, 4, 27));
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.ResolveDependecies();
builder.Services.AddDbContext<TodoDbContext>(options => options.UseMySql(connectionString, serverVersion));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
