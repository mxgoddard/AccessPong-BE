using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AccessPong.Events.Helper;
using AccessPong.Events.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AccessPong.Controllers
{
    [Route("api")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> _logger;
        private readonly IHelper _helper;

        public ValuesController(ILogger<ValuesController> logger, IHelper helper)
        {
            _logger = logger;
            _helper = helper;
        }

        // GET api/home
        [HttpGet]
        public IActionResult GetApi()
        {
            return Ok($"{DateTime.UtcNow}: This is the /api/ endpoint.");
        }

        // GET api/home
        [HttpGet("home")]
        public IActionResult GetHome()
        {
            return Ok($"{DateTime.UtcNow}: This is the /api/home endpoint.");
        }

        // GET api/league
        [HttpGet("league")]
        public IActionResult GetLeague()
        {
            var leagueJson = _helper.GetLeague();

            return Content(leagueJson, "application/json");
        }

        // GET api/rules
        [HttpGet("rules")]
        public IActionResult GetRules()
        {
            var rulesJson = _helper.GetRules();

            return Content(rulesJson, "application/json");
        }
    }
}
