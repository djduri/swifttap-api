using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SWIFTTAP.Domain.Cards;

namespace SWIFTTAP.Infrastructure.Database.Configurations.Cards;

internal class CardConfiguration : IEntityTypeConfiguration<Card>
{
    public void Configure(EntityTypeBuilder<Card> builder)
    {
        builder.ToTable("Cards", Schema.Cards);

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.UniqueName).IsUnique();      // Unikalność dla UniqueName

        builder.Property(x => x.UniqueName).HasMaxLength(100).IsRequired();

        builder.Property(x => x.Description).HasMaxLength(500);

        builder.Property(x => x.HasLogo).IsRequired();

        builder.Property(x => x.Guid).IsRequired();

        builder.Property(x => x.Email).HasMaxLength(255);
        builder.Property(x => x.PhoneNumber).HasMaxLength(25);

        // Relacja 1:1 między Card i User (powiązanie przez UserId w Card)
        builder.HasOne(x => x.User)
               .WithOne(x => x.Card)
               .HasForeignKey<Card>(x => x.UserId) // Klucz obcy w Card
               .OnDelete(DeleteBehavior.Cascade) // Usuwanie użytkownika powoduje usunięcie karty
               .IsRequired();

        // Relacja 1:1 między Card a Theme
        builder.HasOne(x => x.Theme)
               .WithOne(x => x.Card)
               .HasForeignKey<Theme>(x => x.CardId)
               .OnDelete(DeleteBehavior.Cascade) // Usunięcie Card powoduje usunięcie Theme
               .IsRequired();

        // Relacja 1:1 między Card a Logo
        builder.HasOne(x => x.Logo)
               .WithOne(x => x.Card)
               .HasForeignKey<Logo>(x => x.CardId)
               .OnDelete(DeleteBehavior.Cascade) // Usunięcie Card powoduje usunięcie Logo
               .IsRequired();

        // Relacja 1 do wielu między Card a Linkami
        builder.HasMany(x => x.Links)
               .WithOne(x => x.Card)
               .HasForeignKey(x => x.CardId)
               .OnDelete(DeleteBehavior.Cascade); // Usunięcie Card powoduje usunięcie Linków
    }
}
