using System.ComponentModel.DataAnnotations;

namespace Logistics_Transportation.Models
{
    public class Loader
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Passport { get; set; }
        public int Age { get; set; }

        public ICollection<TripLoaders> triploaders { get; set; }
    }
}
