using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.ComponentModel.DataAnnotations;

namespace Logistics_Transportation.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public User user { get; set; }
        public  string PickAppAddress { get; set; }
        public string DeliveryAddress { get; set; }
        public string Description { get; set; }
        public int CargoWeight { get; set; }
        public int CargoVolume { get; set; }
        public DateTime RegistrationDateOrder { get; set; }
        public Trip trip { get; set; }
    }
}
