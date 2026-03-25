using Logistics_Transportation.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Logistics_Transportation.Controllers
{
    [ApiController]
    [Route("api/ML")]
    public class MLController : ControllerBase
    {
        [HttpPost]
        public IActionResult GetML([FromBody] MLPredictDTO dto)
        {
            //Load sample data
            var sampleData = new MLModel.ModelInput()
            {
                Description = dto.Description
            };

            //Load model and predict output
            var result = MLModel.Predict(sampleData);

            return Ok(new
            {
                RecommendedCarType = result.PredictedLabel,
                Score = result.Score
            });
        }
    }
}
