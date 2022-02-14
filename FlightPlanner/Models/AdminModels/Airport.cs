using System.Text.Json.Serialization;

namespace FlightPlanner.Models
{
    public class Airport
    {
        [JsonPropertyName("airport")]
        public string AirportName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }
}
