using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoreMath.Core.Entities;

namespace MoreMath.Infrastructure.DbConfigurations;

public class TagConfigure : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasIndex(x => x.TagName).IsUnique();
        builder.Property(x => x.TagName).IsRequired();
    }
}
