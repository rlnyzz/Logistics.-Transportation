using Logistics_Transportation.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logistics_Transportation.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).ValueGeneratedOnAdd();

            builder
                .HasOne(o => o.user)
                .WithMany(u => u.orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.PickAppAddress)
                .IsRequired();

            builder.Property(p => p.DeliveryAddress)
                .IsRequired();

            builder.Property(p => p.Description)
                .IsRequired();

            builder.Property(p => p.CargoWeight)
                .IsRequired();

            builder.Property(p => p.CargoVolume)
                .IsRequired();

            builder.Property(p => p.RegistrationDateOrder)
                .HasDefaultValueSql("NOW()");

        }
    }
}
