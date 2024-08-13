using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoreMath.Core.Entities;

namespace MoreMath.Infrastructure.Configurations;

public class ArticleConfigure : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.HasMany(p => p.Authors).WithMany(p => p.Articles);
    }
}
