using Microsoft.AspNetCore.Identity;

namespace MoreMath.App.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public int AppUserId { get; set; }
    }

}
