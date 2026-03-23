using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Logistics_Transportation.Controllers
{
    [ApiController]
    [Route("api/information")]
    public class InfoController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllInfo()
        {
            return Ok("Общая информация доступная всем");
        }
    }
}
