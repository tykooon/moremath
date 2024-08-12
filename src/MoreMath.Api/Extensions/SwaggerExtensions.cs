using Microsoft.OpenApi.Models;

namespace MoreMath.Api.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerService(this IServiceCollection services)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddSwaggerGen(
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "MoreMath.Api", Version = "v1" });
                options.CustomSchemaIds(type => type.FullName);
            });
        return services;
    }

    public static WebApplication UseSwaggerForDevelopment(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        return app;

    }
}
