using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrustedPartners.Attributes;
using TrustedPartners.DataAccessLayer;
using Microsoft.Extensions.Configuration;

namespace TrustedPartners.Controllers
{
    //The following attribute enables ApiKey validation
    [ApiKey]
    [ApiController]
    [Route("[controller]")]
    public class TrustedPartnersController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        
        private readonly ILogger<TrustedPartnersController> _logger;
        public TrustedPartnersController(ILogger<TrustedPartnersController> logger)
        {
            _logger = logger;
        }
        
        //The AGENT_CODE who has the highest sum of ADVANCE_AMOUNT in the first quarter of the specific year sent in the parameter

        [HttpGet("HighestAgent/{year}")]
        public ActionResult<string> HighestAgent(int year)
        {
            var da = new DataAccess();
            return da.HighestAgent(year);
        }

        // GET api/WeatherForecast/GetListOfAgents/
        [HttpGet("GetListOfAgents")]
        public ActionResult<string> GetListOfAgents([FromQuery] string[] agents, [FromQuery] int number)
        {
            var da = new DataAccess();
            return da.GetListOfAgents(agents, number);
        }

        // GET api/GetTopAgents/
        [HttpGet("{number}/{year}")]
        public ActionResult<string> Get(int number, int year)
        {
            var da = new DataAccess();
            return da.GetTopAgents(number, year);
        }
    }
}
