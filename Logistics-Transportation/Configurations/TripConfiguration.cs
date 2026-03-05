using Logistics_Transportation.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logistics_Transportation.Configurations
{
    public class TripConfiguration : IEntityTypeConfiguration<Trip>
    {
        public void Configure(EntityTypeBuilder<Trip> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd();

            builder
                .HasOne(t => t.order)
                .WithOne(o => o.trip)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(t => t.driver)
                .WithMany(d => d.trips)
                .HasForeignKey(t => t.DriverId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(t => t.car)
                .WithMany(c => c.trips)
                .HasForeignKey(t => t.CarId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(t => t.FinalePrice).IsRequired();

            builder.Property(t => t.FinaleTimeMinutes).IsRequired();
        }
    }
}
