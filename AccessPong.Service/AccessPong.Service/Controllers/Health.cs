using Microsoft.AspNetCore.Mvc;

namespace AccessPong.Service.Controllers
{
    [Route("api/health")]
    public class Health : ControllerBase
    {    
        // Return a positive health message of the service
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("The Service is healthy and working properly.");
        }            
    }
}
