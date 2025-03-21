using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SWIFTTAP.Domain.Cards;

namespace SWIFTTAP.Infrastructure.Database.Configurations.Cards;
internal class LogoConfiguration : IEntityTypeConfiguration<Logo>
{
    public void Configure(EntityTypeBuilder<Logo> builder)
    {
        builder.ToTable("Logos", Schema.Cards);

        builder.HasKey(i => i.CardId);

        builder.Property(x => x.ContentType).HasMaxLength(32);
        builder.Property(x => x.Content).HasColumnType("bytea");

        builder.HasOne(x => x.Card)
               .WithOne(x => x.Logo)
               .HasForeignKey<Logo>(x => x.CardId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}