using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoreMath.Application.Contracts;
using MoreMath.Application.Contracts.Services;
using MoreMath.Infrastructure.Services;


namespace MoreMath.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastucture(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(opt =>
            opt.UseMySql(
                configuration.GetConnectionString("MainDb:Development:MariaDb"),
                new MariaDbServerVersion(new Version(10, 6, 18)))
            );
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<ITagService, TagService>();
        return services;
    }
}
