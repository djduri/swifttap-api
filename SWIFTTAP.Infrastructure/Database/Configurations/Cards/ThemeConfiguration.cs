using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SWIFTTAP.Domain.Cards;

namespace SWIFTTAP.Infrastructure.Database.Configurations.Cards;
internal class ThemeConfiguration : IEntityTypeConfiguration<Theme>
{
    public void Configure(EntityTypeBuilder<Theme> builder)
    {
        builder.ToTable("Themes", Schema.Cards);

        builder.HasKey(i => i.CardId);

        builder.Property(x => x.Name).HasMaxLength(100);
        builder.Property(x => x.PrimaryColor).HasMaxLength(25);
        builder.Property(x => x.SecondaryColor).HasMaxLength(25);
        builder.Property(x => x.TextDark).HasMaxLength(25);
        builder.Property(x => x.TextLight).HasMaxLength(25);
        builder.Property(x => x.Background).HasMaxLength(25);
        builder.Property(x => x.TextOnButtons).HasMaxLength(25);
        builder.Property(x => x.LinkBackgroundColor).HasMaxLength(25);
        builder.Property(x => x.LinkTextColor).HasMaxLength(25);

        builder.Property(x => x.HasSharpEdges).IsRequired();

        builder.HasOne(x => x.Card)
               .WithOne(x => x.Theme)
               .HasForeignKey<Theme>(x => x.CardId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
