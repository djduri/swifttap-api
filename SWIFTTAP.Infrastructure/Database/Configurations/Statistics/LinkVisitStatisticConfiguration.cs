using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SWIFTTAP.Domain.Statistics;

namespace SWIFTTAP.Infrastructure.Database.Configurations.Statistics;
internal class LinkVisitStatisticConfiguration : IEntityTypeConfiguration<LinkVisitStatistic>
{
    public void Configure(EntityTypeBuilder<LinkVisitStatistic> builder)
    {
        builder.ToTable("LinkVisitStatistics", Schema.Statistics);

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Link)
               .WithMany(x => x.LinkVisitStatistics)
               .HasForeignKey(x => x.LinkId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
