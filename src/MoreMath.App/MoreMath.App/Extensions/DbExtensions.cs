using Microsoft.EntityFrameworkCore;
using MoreMath.App.Data;

namespace MoreMath.App.Extensions;

public static class DbExtensions
{
    public static WebApplication ApplyMigrations(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<WebAppDbContext>();
            db.Database.Migrate();
        }
        return app;
    }
}
