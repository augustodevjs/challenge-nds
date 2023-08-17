using Microsoft.OpenApi.Models;

namespace Todo.API.Configuration;

public static class SwaggerConfiguration
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "Todo API",
                Version = "v1",
                Contact = new OpenApiContact
                {
                    Name = "João Augusto",
                    Email = "jaugusto.dev@gmail.com",
                    Url = new Uri("https://github.com/augustodevjs")
                },
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description =
                    @"JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below Example: 'Bearer 1231asd'",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });
    }
}