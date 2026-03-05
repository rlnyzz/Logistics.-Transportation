using Microsoft.AspNetCore.Mvc;

namespace Logistics_Transportation.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        [HttpGet("all")]
        public IActionResult GetUsers()
        {
            return Ok(new List<string> { "User1", "User2" });
        }
    }
}
