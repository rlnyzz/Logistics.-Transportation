using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Logistics_Transportation.Models
{
    public class User : IdentityUser
    {
        public ICollection<Order> orders { get; set; }
    }
}
