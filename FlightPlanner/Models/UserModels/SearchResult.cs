namespace FlightPlanner.Models.UserModels
{
    public class SearchResult
    {
        public Flight[] Items { get; set; } = new Flight[0];
        public int Page { get; set; } = 0;
        public int TotalItems { get; set; } = 0;
    }
}
