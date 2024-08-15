using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoreMath.Application.Contracts;

namespace MoreMath.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastucture(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlServer(configuration["Database:Development:ConnectionString"]));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}
