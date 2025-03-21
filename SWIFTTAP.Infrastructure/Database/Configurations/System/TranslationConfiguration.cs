using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SWIFTTAP.Domain.System;

namespace SWIFTTAP.Infrastructure.Database.Configurations.System;
internal class TranslationConfiguration : IEntityTypeConfiguration<Translation>
{
    public void Configure(EntityTypeBuilder<Translation> builder)
    {
        builder.ToTable("Translations", Schema.System);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Language).IsRequired();
        builder.Property(x => x.Content).HasDefaultValue(null);

        builder.HasIndex(x => x.Name).IsUnique();
    }
}

