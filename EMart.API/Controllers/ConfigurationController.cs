using EMart.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace EMart.API.Controllers
{
    [Route("[controller]")]
    public class ConfigurationController : BaseAPIController
    {
        private readonly IConfigurationsService _configurationsService;
        public ConfigurationController(IConfigurationsService configurationsService)
        {
            _configurationsService = configurationsService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _configurationsService.GetAll();
            return Ok(result);
        }
    }
}
