using FlightPlanner.Models.UserModels;
using FlightPlanner.Storage;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CustomerAPIController : ControllerBase
    {
        [HttpGet]
        [Route("airports")]
        public IActionResult GetAirport(string search)
        {
            return Ok(FlightStorage.SearchAirports(search));
        }

        [HttpPost]
        [Route("flights/search")]
        public IActionResult SearchFlights(SearchFlightRequest req)
        {
            return FlightStorage.SearchFlights(req) == null ? BadRequest() : Ok(FlightStorage.SearchFlights(req));
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult FindFlightById(int id)
        {
            return FlightStorage.FetchFlight(id) == null ? NotFound() : Ok(FlightStorage.FetchFlight(id));
        }
    }
}
