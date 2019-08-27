using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AccessPong.Events.Helper;
using Microsoft.AspNetCore.Mvc;

namespace AccessPong.Controllers
{
    [Route("api")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
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
            return Ok($"{DateTime.UtcNow}: This is the /api/fixtures endpoint.");
        }

        // GET api/fixtures/generate
        [HttpGet("fixtures/generate")]
        public IActionResult GenerateFixturesEndpoint()
        {
            Helper _helper = new Helper();
            bool success = _helper.GenerateFixtures();

            if (!success)
            {
                return NotFound($"{DateTime.UtcNow}: Fixtures failed to generate.");
            }

            return Ok($"{DateTime.UtcNow}: Fixtures successfully generated.");
        }

    }
}
