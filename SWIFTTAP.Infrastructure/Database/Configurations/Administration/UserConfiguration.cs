using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SWIFTTAP.Domain.Administration;
using SWIFTTAP.Domain.Cards;

namespace SWIFTTAP.Infrastructure.Database.Configurations.Administration;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users", Schema.Administration); // Określenie tabeli w schemacie

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        // Relacja 1:1 między User a UserKeys
        builder.HasOne(x => x.UserKeys)
               .WithOne(x => x.User)
               .HasForeignKey<UserKeys>(x => x.UserId)
               .OnDelete(DeleteBehavior.Cascade); // Usunięcie User powoduje usunięcie UserKeys

        // Możesz dodać inne konfiguracje dla właściwości User, np. UserName, Email itd.
    }
}
