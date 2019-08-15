using AccessPong.Service.Event;
using System;
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
            var tempTest = new TemporaryTest();
            int result = tempTest.AddOne(0);
            string endPointMessage = $"{result}: Health endpoint hit at {DateTime.UtcNow}";
            return Ok(endPointMessage);
        }            
    }
}
