using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SWIFTTAP.Domain.Statistics;

namespace SWIFTTAP.Infrastructure.Database.Configurations.Statistics;
internal class VcfDownloadStatisticConfiguration : IEntityTypeConfiguration<VcfDownloadStatistic>
{
    public void Configure(EntityTypeBuilder<VcfDownloadStatistic> builder)
    {
        builder.ToTable("VcfDownloadStatistics", Schema.Statistics);

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Card)
               .WithMany(x => x.VcfDownloadStatistics)
               .HasForeignKey(x => x.CardId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
