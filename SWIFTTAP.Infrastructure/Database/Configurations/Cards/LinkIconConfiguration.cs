using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SWIFTTAP.Domain.Cards;

namespace SWIFTTAP.Infrastructure.Database.Configurations.Cards;

internal class LinkIconConfiguration : IEntityTypeConfiguration<LinkIcon>
{
    public void Configure(EntityTypeBuilder<LinkIcon> builder)
    {
        builder.ToTable("LinkIcons", Schema.Cards);

        builder.HasKey(i => i.LinkId);

        builder.Property(x => x.ContentType).HasMaxLength(32);
        builder.Property(x => x.Content).HasColumnType("bytea");

        builder.HasOne(x => x.Link)
               .WithOne(x => x.LinkIcon)
               .HasForeignKey<LinkIcon>(x => x.LinkId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
