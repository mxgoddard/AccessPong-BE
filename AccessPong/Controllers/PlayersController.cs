using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccessPong.Events.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AccessPong.Controllers
{
    [Route("api/players")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly ILogger<ValuesController> _logger;
        private readonly IHelper _helper;

        public PlayersController(ILogger<ValuesController> logger, IHelper helper)
        {
            _logger = logger;
            _helper = helper;
        }

        // GET api/players
        [HttpGet()]
        public IActionResult GetPlayers()
        {
            var playersJson = _helper.GetPlayers();

            return Content(playersJson, "application/json");
        }
    }
}
