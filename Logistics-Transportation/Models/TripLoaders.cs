namespace Logistics_Transportation.Models
{
    public class TripLoaders
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public Trip trip { get; set; }
        public int LoaderId { get; set; }
        public Loader loader { get; set; }
    }
}
