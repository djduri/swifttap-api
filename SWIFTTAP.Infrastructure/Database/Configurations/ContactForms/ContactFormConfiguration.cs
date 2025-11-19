using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SWIFTTAP.Domain.ContactForms;

namespace SWIFTTAP.Infrastructure.Database.Configurations.ContactForms;

public class ContactFormConfiguration : IEntityTypeConfiguration<ContactForm>
{
    public void Configure(EntityTypeBuilder<ContactForm> builder)
    {
        builder.ToTable("ContactForms", Schema.ContactForms);

        builder.HasKey(x => x.Id);

        builder.Property(cf => cf.Name)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(cf => cf.Email)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(cf => cf.Phone)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(cf => cf.Company)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(cf => cf.Quantity)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(cf => cf.Message)
            .HasMaxLength(1000);
    }
}
