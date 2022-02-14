using FlightPlanner.Models;
using FlightPlanner.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("admin-api")]
    [ApiController]
    public class AdminAPIController : ControllerBase
    {
        private static readonly object _lock = new object();

        [HttpGet, Authorize]
        [Route("flights/{id}")]
        public IActionResult GetFlight(int id)
        {
            return FlightStorage.FetchFlight(id) == null ? NotFound() : Ok(FlightStorage.FetchFlight(id));
        }

        [HttpDelete, Authorize]
        [Route("flights/{id}")]
        public IActionResult DeleteFlight(int id)
        {
            lock (_lock)
            {
                FlightStorage.DeleteFlight(id);
                return Ok();
            }
        }

        [HttpPut, Authorize]
        [Route("flights")]
        public IActionResult AddFlight(AddFlightRequest request)
        {
            lock (_lock)
            {
                if (FlightStorage.CheckForDuplicate(request))
                {
                    return Conflict();
                }

                if (!FlightStorage.CheckIfRequestIsValid(request))
                {
                    return BadRequest();
                }

                return Created("", FlightStorage.AddFlight(request));
            }
        }
    }
}
