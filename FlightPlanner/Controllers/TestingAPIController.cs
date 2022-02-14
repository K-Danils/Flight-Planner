using FlightPlanner.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanner.Controllers
{
    [Route("testing-api/")]
    [ApiController]
    public class TestingAPIController : ControllerBase
    {
        [HttpPost]
        [Route("clear/")]
        public IActionResult Clear()
        {
            FlightStorage.ClearFlights();
            return Ok();
        }
    }
}
