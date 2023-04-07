using Beymen.ConfigurationLibrary;
using ConfigurationLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Beymen.Service_B.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ConfigurationsController : ControllerBase
    {
        private readonly IConfigurationReader _configurationReader;
        public ConfigurationsController(IConfigurationReader configurationReader)
        {
            _configurationReader = configurationReader;
        }

        // GET: api/Configuration
        // SERVICE-B test için yazılmıştır. Veritabanında IsBasketEnabled keyini test etmek için yazılmıştır.
        [HttpGet("get-service-b")]
        public IActionResult GetConfiguration()
        {
            return Ok("IsBasketEnabled: "+_configurationReader.GetValue<bool>("IsBasketEnabled"));
        }
    }
}
