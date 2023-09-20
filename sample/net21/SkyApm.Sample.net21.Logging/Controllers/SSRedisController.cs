using System;
using Microsoft.AspNetCore.Mvc;
using ServiceStack.Redis;
using System.Collections.Generic;
using ServiceStack.Script;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace SkyApm.Sample.net21.Logging.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SSRedisController : ControllerBase
    {
        private static PooledRedisClientManager _redisClientManager = null;

        private readonly ILogger<SSRedisController> _logger;

        private const string _redisKey = "SkyApm-DevTest-SSRedis";

        public SSRedisController(ILogger<SSRedisController> logger)
        {
            _logger = logger;
            if(_redisClientManager == null)
                _redisClientManager = new PooledRedisClientManager(4,"AAAaaa1234@172.16.112.171:6379");
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var all = DiagnosticListener.AllListeners;

            _logger.LogInformation("ssredis get all");
            var vals = _redisClientManager.GetClient().GetValues<string>(new List<string>(){$"{_redisKey}_1", $"{_redisKey}_2",$"{_redisKey}_3"});
            return vals;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            _logger.LogInformation($"ssredis get {id}");
            return _redisClientManager.GetClient().GetValue($"{_redisKey}_{id}");
        }

        // POST api/values
        [HttpPost("{id}/{value}")]
        public void Post(int id, string value)
        {
            _logger.LogInformation($"ssredis set {id} with value: {value}");
            _redisClientManager.GetClient().SetValue($"{_redisKey}_{id}", value);
        }

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _logger.LogInformation($"ssredis delete {id}");
            _redisClientManager.GetClient().Remove($"{_redisKey}_{id}");
        }
    }
}
