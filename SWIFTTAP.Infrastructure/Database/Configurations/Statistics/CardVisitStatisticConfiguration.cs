using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SWIFTTAP.Domain.Statistics;

namespace SWIFTTAP.Infrastructure.Database.Configurations.Statistics;
internal class CardVisitStatisticConfiguration : IEntityTypeConfiguration<CardVisitStatistic>
{
    public void Configure(EntityTypeBuilder<CardVisitStatistic> builder)
    {
        builder.ToTable("CardVisitStatistics", Schema.Statistics);

        builder.HasKey(x => x.Id);        

        builder.HasOne(x => x.Card)
               .WithMany(x => x.CardVisitStatistics)
               .HasForeignKey(x => x.CardId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}