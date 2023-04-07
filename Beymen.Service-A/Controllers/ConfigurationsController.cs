using Beymen.ConfigurationLibrary;
using ConfigurationLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Beymen.Service_A.Controllers
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
        // SERVICE-A test için yazılmıştır. Veritabanında SiteName keyini test etmek için yazılmıştır.
        [HttpGet("get-service-a")]
        public IActionResult GetConfiguration()
        {
            return Ok("SiteName: "+_configurationReader.GetValue<string>("SiteName"));
        }
    }
}
