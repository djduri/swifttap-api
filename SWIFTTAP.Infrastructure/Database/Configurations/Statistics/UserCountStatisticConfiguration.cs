using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SWIFTTAP.Domain.Statistics;

namespace SWIFTTAP.Infrastructure.Database.Configurations.Statistics;
internal class UserCountStatisticConfiguration : IEntityTypeConfiguration<UserCountStatistic>
{
    public void Configure(EntityTypeBuilder<UserCountStatistic> builder)
    {
        builder.ToTable("UserCountStatistics", Schema.Statistics);

        builder.HasKey(x => x.Id);
    }
}
