using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoreMath.Core.Entities;

namespace MoreMath.Infrastructure.DbConfigurations;

public class CategoryConfigure : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasIndex(x => x.CategoryName).IsUnique();
        builder.Property(x => x.CategoryName).IsRequired();
    }
}
