using Microsoft.OpenApi.Models;
using MoreMath.Api.Common;

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
                options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Name = ApiConstants.APIKEY_HEADERNAME,
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "Authorization by x-api-key inside request's header",
                    Scheme = "ApiKeyScheme"
                });
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
