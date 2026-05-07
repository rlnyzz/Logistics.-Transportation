using Microsoft.AspNetCore.Mvc;
using System.Text;
using Logistics_Transportation.DTOs;
using System.Text.Json;

namespace Logistics_Transportation.Controllers
{
    [ApiController]
    [Route("api/geo")]
    public class GeoController : ControllerBase
    {
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string q)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "KabluchkoffApp/1.0");
            var url = $"https://nominatim.openstreetmap.org/search?format=json&q={Uri.EscapeDataString(q)}&limit=1";
            var response = await client.GetStringAsync(url);
            return Content(response, "application/json");
        }

        [HttpPost("route")]
        public async Task<IActionResult> GetRoute([FromBody] RouteRequestDto dto)
        {
            using var client = new HttpClient();
            var apiKey = "eyJvcmciOiI1YjNjZTM1OTc4NTExMTAwMDFjZjYyNDgiLCJpZCI6IjZhOThmZWI0MmUwODdkY2FiNjdkNjQxZDgxMTg3N2Y5NDgyZGY1MWQxOTkyZDlhMjUyMzk5ZTlmIiwiaCI6Im11cm11cjY0In0=";

            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", apiKey);

            var body = JsonSerializer.Serialize(new { coordinates = dto.Coordinates, radiuses = new[] { -1, -1 } });
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(
                "https://api.openrouteservice.org/v2/directions/driving-car/geojson",
                content
            );

            var result = await response.Content.ReadAsStringAsync();
            return Content(result, "application/json");
        }
    }
}
