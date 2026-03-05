using System.ComponentModel.DataAnnotations;

namespace Logistics_Transportation.Models
{
    public class Driver
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Passport { get; set; }
        public int Age { get; set; }
        public int Rate { get; set; }
        public int CategoryLicenceId { get; set; }
        public LicenceCategories LicenceCategories { get; set; }

        public ICollection<Trip> trips { get; set; }
    }
}
