using Logistics_Transportation.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logistics_Transportation.Configurations
{
    public class LicenceCategoriesConfiguration : IEntityTypeConfiguration<LicenceCategories>
    {
        public void Configure(EntityTypeBuilder<LicenceCategories> builder)
        {
            builder.HasKey(lc => lc.Id);
            builder.Property(lc => lc.Id).ValueGeneratedOnAdd();

            builder.HasIndex(g => g.Name).IsUnique();
        }
    }
}
