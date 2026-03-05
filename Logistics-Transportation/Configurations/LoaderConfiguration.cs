using Logistics_Transportation.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logistics_Transportation.Configurations
{
    public class LoaderConfiguration : IEntityTypeConfiguration<Loader>
    {
        public void Configure(EntityTypeBuilder<Loader> builder)
        {
            builder.HasKey(l => l.Id);
            builder.Property(l => l.Id).ValueGeneratedOnAdd();

            builder.Property(l => l.Name).IsRequired();
            builder.HasIndex(l => l.Passport).IsUnique();
            builder.Property(l => l.Age).IsRequired();
        }
    }
}
