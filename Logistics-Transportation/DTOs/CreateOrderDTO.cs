namespace Logistics_Transportation.DTOs
{
    public class CreateOrderDTO
    {
        public string PickAppAddress { get; set; }
        public string DeliveryAddress { get; set; }
        public string Description { get; set; }
        public double CargoWeight { get; set; }
        public double CargoVolume { get; set; }
    }
}
