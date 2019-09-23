using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccessPong.Events.Helper;
using AccessPong.Events.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AccessPong.Controllers
{
    [Route("api/fixtures")]
    [ApiController]
    public class FixturesController : ControllerBase
    {
        private readonly ILogger<ValuesController> _logger;
        private readonly IHelper _helper;

        public FixturesController(ILogger<ValuesController> logger, IHelper helper)
        {
            _logger = logger;
            _helper = helper;
        }

        // GET api/fixtures
        [HttpGet()]
        public IActionResult GetFixtures()
        {
            var fixturesJson = _helper.GetFixtures();

            return Content(fixturesJson, "application/json");
        }

        [HttpGet("{id}")]
        public IActionResult GetTest(int id)
        {
            Console.WriteLine(id);
            var fixturesJson = _helper.GetFixtures();

            return Content(fixturesJson, "application/json");
        }

        // GET api/fixtures/generate
        [HttpGet("generate")]
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
        [HttpGet("next")]
        public IActionResult GetNextFixture()
        {
            var fixtureJson = _helper.GetNextGame();

            return Content(fixtureJson, "application/json");
        }

        // POST api/fixtures/update
        [HttpPost("update")]
        public IActionResult UpdateFixture([FromBody]FixtureUpdate data)
        {
            bool success = _helper.FinishMatch(data.FixtureId, data.WinnerId, data.LoserId);

            if (success)
            {
                return Ok($"{DateTime.UtcNow}: Fixture and player information updated.");
            }

            return NotFound($"{DateTime.UtcNow}: Fixture and player information failed to update.");
        }
    }
}
