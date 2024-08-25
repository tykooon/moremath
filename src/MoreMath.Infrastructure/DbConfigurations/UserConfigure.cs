using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoreMath.Core.Entities;

namespace MoreMath.Infrastructure.DbConfigurations;

public class UserConfigure : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(x => x.Username).IsUnique();
        builder.Property(x => x.Username).IsRequired();
    }
}
