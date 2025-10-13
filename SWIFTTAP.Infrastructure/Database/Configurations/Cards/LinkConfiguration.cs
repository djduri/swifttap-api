using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SWIFTTAP.Domain.Cards;

namespace SWIFTTAP.Infrastructure.Database.Configurations.Cards;
internal class LinkConfiguration : IEntityTypeConfiguration<Link>
{
    public void Configure(EntityTypeBuilder<Link> builder)
    {
        builder.ToTable("Links", Schema.Cards);

        builder.HasKey(i => i.Id);

        builder.Property(x => x.Type)
            .HasMaxLength(8)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Url)
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(x => x.LinkKind).HasDefaultValue(LinkKind.Default).IsRequired();

        builder.HasOne(x => x.Card)
            .WithMany(x => x.Links) // jeśli User ma kolekcję Linków
            .HasForeignKey(x => x.CardId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
