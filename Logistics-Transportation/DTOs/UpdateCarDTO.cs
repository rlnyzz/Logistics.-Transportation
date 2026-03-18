namespace Logistics_Transportation.DTOs
{
    public class UpdateCarDTO
    {
        public string CarMake { get; set; }
        public string CarModel { get; set; }
        public string TypeOfCar { get; set; }
        public string CarNumber { get; set; }
        public decimal CargoCapacityT { get; set; }
        public decimal TrunkVolumeL { get; set; }
        public decimal FuelConsumption { get; set; }
        public string LicenceCategories { get; set; }
    }
}
