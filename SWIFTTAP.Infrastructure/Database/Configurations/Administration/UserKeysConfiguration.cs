using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SWIFTTAP.Domain.Administration;

namespace SWIFTTAP.Infrastructure.Database.Configurations.Administration;

internal class UserKeysConfiguration : IEntityTypeConfiguration<UserKeys>
{
    public void Configure(EntityTypeBuilder<UserKeys> builder)
    {
        builder.ToTable("UserKeys", Schema.Administration);

        builder.Property(x => x.AuthTwoFactorCode).HasMaxLength(6);
        builder.Property(x => x.AuthTwoFactorKey).HasMaxLength(54);
        builder.Property(x => x.PreAuthenticationToken).HasMaxLength(64);

        builder.HasKey(x => x.UserId);
    }
}


