using Microsoft.EntityFrameworkCore;
using MoreMath.Infrastructure;

namespace MoreMath.Api.Extensions;

public static class DbExtensions
{
    public static WebApplication ApplyMigrations(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.Migrate();
        }

        return app;
    }
}
