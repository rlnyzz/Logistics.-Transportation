using Logistics_Transportation.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logistics_Transportation.Configurations
{
    public class TripLoadersConfiguration : IEntityTypeConfiguration<TripLoaders>
    {
        public void Configure(EntityTypeBuilder<TripLoaders> builder)
        {
            builder.HasKey(tl => tl.Id);
            builder.Property(tl => tl.Id).ValueGeneratedOnAdd();

            builder
                .HasOne(tl => tl.trip)
                .WithMany(t => t.triploaders)
                .HasForeignKey(t => t.TripId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder
                .HasOne(tl => tl.loader)
                .WithMany(l => l.triploaders)
                .HasForeignKey(tl => tl.LoaderId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }
    }
}
