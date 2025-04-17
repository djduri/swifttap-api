using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SWIFTTAP.Domain.Administration;

namespace SWIFTTAP.Infrastructure.Database.Configurations.Administration;
internal sealed class DeletedUserConfiguration : IEntityTypeConfiguration<DeletedUser>
{
    public void Configure(EntityTypeBuilder<DeletedUser> builder)
    {
        builder.ToTable("DeletedUser", Schema.Administration); // Określenie tabeli w schemacie

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Reason).HasMaxLength(255).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(255).IsRequired();
    }
}

