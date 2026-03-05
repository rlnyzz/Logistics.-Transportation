using Logistics_Transportation.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logistics_Transportation.Configurations
{
    public class DriverConfiguration : IEntityTypeConfiguration<Driver>
    {
        public void Configure(EntityTypeBuilder<Driver> builder)
        {
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Id).ValueGeneratedOnAdd();

            builder.Property(f => f.Name).IsRequired();
            builder.HasIndex(f => f.Passport).IsUnique();
            builder.Property(f => f.Age).IsRequired();
            builder.Property(f => f.Rate).IsRequired();
            
            builder
                .HasOne(d => d.LicenceCategories)
                .WithMany(lc => lc.drivers)
                .HasForeignKey(d => d.CategoryLicenceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
