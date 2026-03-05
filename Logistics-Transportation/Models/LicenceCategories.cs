namespace Logistics_Transportation.Models
{
    public class LicenceCategories
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Driver> drivers { get; set; }
        public ICollection<Car> cars { get; set; }
        
    }
}
