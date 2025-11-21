using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SWIFTTAP.Domain.Statistics;

namespace SWIFTTAP.Infrastructure.Database.Configurations.Statistics;

internal class CardVisitGeoStatisticConfiguration : IEntityTypeConfiguration<CardVisitGeoStatistic>
{
    public void Configure(EntityTypeBuilder<CardVisitGeoStatistic> builder)
    {
        builder.ToTable("CardVisitGeoStatistics", Schema.Statistics);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.City).HasMaxLength(256).IsRequired();
        builder.Property(x => x.CountryCode).HasMaxLength(2).IsRequired();

        builder.HasOne(x => x.Card)
               .WithMany(x => x.CardVisitGeoStatistics)
               .HasForeignKey(x => x.CardId)
               .OnDelete(DeleteBehavior.Cascade);

        // Indeksy
        builder.HasIndex(x => x.CardId); // przyspiesza filtrowanie po CardId
        builder.HasIndex(x => x.Date);   // przyspiesza filtrowanie po zakresie dat
        builder.HasIndex(x => new { x.City, x.CountryCode }); // przyspiesza grupowanie po mieście i kraju
        builder.HasIndex(x => new { x.CardId, x.Date }); // dla kombinacji CardId + Date (najczęstsze zapytania)
    }
}
