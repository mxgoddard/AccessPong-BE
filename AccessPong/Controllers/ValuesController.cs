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
            return Ok($"{DateTime.UtcNow}: This is the /api/league endpoint.");
        }

        // GET api/fixtures
        [HttpGet("fixtures")]
        public IActionResult GetFixtures()
        {
            var fixturesJson = _helper.GetFixtures();

            return Content(fixturesJson, "application/json");
        }

        // GET api/fixtures/generate
        [HttpGet("fixtures/generate")]
        public IActionResult GenerateFixturesEndpoint()
        {
            bool IS_ADMIN = true;

            if (!IS_ADMIN)
            {
                return Unauthorized($"{DateTime.UtcNow}: Fixtures failed to generate.");
            }

            bool success = _helper.GenerateFixtures();

            if (!success)
            {
                return NotFound($"{DateTime.UtcNow}: Fixtures failed to generate.");
            }

            return Ok($"{DateTime.UtcNow}: Fixtures successfully generated.");
        }

        // GET api/fixtures/next
        [HttpGet("fixtures/next")]
        public IActionResult GetNextFixture()
        {
            var fixtureJson = _helper.GetNextGame();

            return Content(fixtureJson, "application/json");
        }

        // POST api/fixtures/update
        [HttpPost("fixtures/update")]
        public IActionResult UpdateFixture([FromBody]FixtureUpdate data)
        {
            var fixtureJson = _helper.UpdateFixture(data.FixtureId, data.WinnerId);

            return Content(fixtureJson, "application/json");
        }
    }
}
