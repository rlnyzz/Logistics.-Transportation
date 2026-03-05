using System.ComponentModel.DataAnnotations;

namespace Logistics_Transportation.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }
        public string CarMake { get; set; }
        public string  CarModel { get; set; }
        public string TypeOfCar { get; set; }
        public string CarNumber { get; set; }
        public decimal CargoCapacityT { get; set; }
        public decimal TrunkVolumeL {  get; set; }
        public decimal FuelConsumption { get; set; }
        public int LicenceCategoriesId { get; set; }
        public LicenceCategories LicenceCategories { get; set; }
        public ICollection<Trip> trips { get; set; }
    }
}
