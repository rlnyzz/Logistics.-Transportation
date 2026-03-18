using Logistics_Transportation.Models;

namespace Logistics_Transportation.DTOs
{
    public class CreateDriverDTO
    {
        public string Name { get; set; }
        public string Passport { get; set; }
        public int Age { get; set; }
        public int Rate { get; set; }
        public string LicenceCategories { get; set; }
    }
}
