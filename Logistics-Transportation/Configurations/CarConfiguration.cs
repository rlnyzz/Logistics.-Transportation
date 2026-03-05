using Logistics_Transportation.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logistics_Transportation.Configurations
{
    public class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.Property(d => d.CarMake).IsRequired();
            builder.Property(d => d.CarModel).IsRequired();
            builder.Property(d => d.TypeOfCar).IsRequired();
            builder.HasIndex(d => d.CarNumber).IsUnique();
            builder.Property(d => d.CargoCapacityT).IsRequired();
            builder.Property(d => d.TrunkVolumeL).IsRequired();
            builder.Property(d => d.FuelConsumption).IsRequired();

            builder
                .HasOne(c => c.LicenceCategories)
                .WithMany(lc => lc.cars)
                .HasForeignKey(c => c.LicenceCategoriesId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
