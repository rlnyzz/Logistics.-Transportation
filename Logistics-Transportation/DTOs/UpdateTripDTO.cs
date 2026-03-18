namespace Logistics_Transportation.DTOs
{
    public class UpdateTripDTO
    {
        public int DriverId { get; set; }
        public int CarId { get; set; }
        public decimal FinalePrice { get; set; }
        public int FinalTimeMinutes { get; set; }
    }
}
