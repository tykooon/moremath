using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MoreMath.App.Data
{
    public class WebAppDbContext(DbContextOptions<WebAppDbContext> options):
        IdentityDbContext<ApplicationUser>(options)
    {
    }
}
