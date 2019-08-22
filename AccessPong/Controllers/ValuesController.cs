using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            return Ok($"{DateTime.UtcNow}: This is the /api/ endpoint");
        }

        // GET api/home
        [HttpGet("home")]
        public IActionResult GetHome()
        {
            return Ok($"{DateTime.UtcNow}: This is the /api/home endpoint");
        }

        // GET api/league
        [HttpGet("league")]
        public IActionResult GetLeague()
        {
            return Ok($"{DateTime.UtcNow}: This is the /api/league endpoint");
        }

        // GET api/fixtures
        [HttpGet("fixtures")]
        public IActionResult GetFixtures()
        {
            return Ok($"{DateTime.UtcNow}: This is the /api/fixtures endpoint");
        }

    }
}
