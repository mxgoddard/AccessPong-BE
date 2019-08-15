using AccessPong.Service.Event;
using System;
using Microsoft.AspNetCore.Mvc;

namespace AccessPong.Service.Controllers
{
    [Route("api")]
    public class Endpoints : ControllerBase
    {
        // Next match endpoint
        [HttpGet("next")]
        public IActionResult GetNextMatch()
        {
            return Ok($"{DateTime.UtcNow}: Next match endpoint hit");
        }

        // Next match endpoint
        [HttpGet("league")]
        public IActionResult GetLeague()
        {
            return Ok($"{DateTime.UtcNow}: Get league endpoint hit");
        }

        // Next match endpoint
        [HttpGet("fixtures")]
        public IActionResult GetFixtures()
        {
            return Ok($"{DateTime.UtcNow}: Get fixtures endpoint hit");
        }
    }
}
