using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SkyApm.Diagnostics.Logging;
using SkyApm.Sample.Backend.Models;

namespace SkyApm.Sample.Backend.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly ISkyApmLogger<ValuesController> _skyApmLogger;

        public ValuesController(ISkyApmLogger<ValuesController> skyApmLogger)
        {
            _skyApmLogger = skyApmLogger;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            _skyApmLogger.Information("aaaaa");
            return new List<string> {"value1", "value2"};
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet("ignore")]
        public string Ignore()
        {
            return "ignore";
        }

        [HttpGet("StopPropagation")]
        public string StopPropagation()
        {
            return "stop propagation";
        }

        [HttpPost("postin")]
        public string PostIn([FromBody]PostModel model)
        {
            return model.Name;
        }
    } 
}