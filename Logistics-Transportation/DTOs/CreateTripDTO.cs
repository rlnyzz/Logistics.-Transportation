namespace Logistics_Transportation.DTOs
{
    public class CreateTripDTO
    {
        public int OrderId { get; set; }
        public int DriverId { get; set; }
        public int CarId { get; set; }
        public decimal FinalePrice { get; set; }
        public int FinalTimeMinutes { get; set; }
    }
}
