using System.Security.Cryptography.X509Certificates;

namespace Logistics_Transportation.Models
{
    public class Trip
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order order { get; set; }
        public int DriverId { get; set; }
        public Driver driver { get; set; }
        public int CarId { get; set; }
        public Car car { get; set; }
        public decimal FinalePrice { get; set; }
        public int FinaleTimeMinutes { get; set; }

        public ICollection<TripLoaders> triploaders { get; set; }
    }
}
