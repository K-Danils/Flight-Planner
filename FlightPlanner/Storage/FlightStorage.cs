using FlightPlanner.Models;
using FlightPlanner.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlightPlanner.Storage
{
    public static class FlightStorage
    {
        private static List<Flight> _flights = new List<Flight>();
        private static int _id;
        private static readonly object _lock = new object();

        public static Flight AddFlight(AddFlightRequest request)
        {
            lock (_lock)
            {
                var flight = new Flight
                {
                    Id = ++_id,
                    From = request.From,
                    To = request.To,
                    Carrier = request.Carrier,
                    DepartureTime = request.DepartureTime,
                    ArrivalTime = request.ArrivalTime
                };
                _flights.Add(flight);

                return flight;
            }
            
        }

        public static SearchResult SearchFlights(SearchFlightRequest req)
        {
            lock (_lock)
            {
                if (CheckIfSearchTermsAreInvalid(req))
                {
                    return null;
                }

                return new SearchResult { Items = _flights.ToArray(), Page = 0, TotalItems = _flights.Count };
            }
        }

        public static bool CheckIfSearchTermsAreInvalid(SearchFlightRequest req)
        {
            lock (_lock)
            {
                return
                    req == null ||
                    String.IsNullOrEmpty(req.From) ||
                    String.IsNullOrEmpty(req.To) ||
                    String.IsNullOrEmpty(req.DepartureDate) ||
                    req.From == req.To;
            }
        }

        public static void ClearFlights()
        {
            lock (_lock)
            {
                _id = 0;
                _flights.Clear();
            }
        }

        public static Flight FetchFlight(int id)
        {
            lock (_lock)
            {
                return _flights.SingleOrDefault(flight => flight.Id == id);
            }
        }

        public static Airport[] SearchAirports(string phrase)
        {
            lock (_lock)
            {
                var foundAirports = new List<Airport>();
                phrase = phrase.ToLower().Trim();
                foreach (var flight in _flights)
                {
                    if (flight.From.AirportName.ToLower().Contains(phrase))
                    {
                        foundAirports.Add(flight.From);
                        continue;
                    }

                    if (flight.From.City.ToLower().Contains(phrase))
                    {
                        foundAirports.Add(flight.From);
                        continue;
                    }

                    if (flight.From.Country.ToLower().Contains(phrase))
                    {
                        foundAirports.Add(flight.From);
                        continue;
                    }

                    if (flight.To.AirportName.ToLower().Contains(phrase))
                    {
                        foundAirports.Add(flight.From);
                        continue;
                    }

                    if (flight.To.City.ToLower().Contains(phrase))
                    {
                        foundAirports.Add(flight.From);
                        continue;
                    }

                    if (flight.To.Country.ToLower().Contains(phrase))
                    {
                        foundAirports.Add(flight.From);
                        continue;
                    }
                }

                return foundAirports.ToArray();
            }
        }

        public static void DeleteFlight(int id)
        {
            lock (_lock)
            {
                _flights.Remove(FetchFlight(id));
            }
        }

        public static bool CheckForDuplicate(AddFlightRequest request)
        {
            lock (_lock)
            {
                return
                    _flights.Any(flight =>
                    flight.From.AirportName.ToLower().Trim() == request.From.AirportName.ToLower().Trim() &&
                    flight.To.AirportName.ToLower().Trim() == request.To.AirportName.ToLower().Trim() &&
                    flight.Carrier.ToLower().Trim() == request.Carrier.ToLower().Trim() &&
                    flight.DepartureTime == request.DepartureTime &&
                    flight.ArrivalTime == request.ArrivalTime);
            }
        }

        public static bool CheckIfRequestIsValid(AddFlightRequest request)
        {
            lock (_lock)
            {
                return
                    !CheckForNullValues(request) &&
                    !CheckForSameAirports(request) &&
                    CheckForCorrectDates(request);
            }
        }

        private static bool CheckForNullValues(AddFlightRequest request)
        {
            lock (_lock)
            {
                return
                    request == null ||
                    request.From == null ||
                    request.To == null ||
                    String.IsNullOrEmpty(request.From.AirportName) ||
                    String.IsNullOrEmpty(request.From.City) ||
                    String.IsNullOrEmpty(request.From.Country) ||
                    String.IsNullOrEmpty(request.To.AirportName) ||
                    String.IsNullOrEmpty(request.To.City) ||
                    String.IsNullOrEmpty(request.To.Country) ||
                    String.IsNullOrEmpty(request.Carrier) ||
                    String.IsNullOrEmpty(request.DepartureTime) ||
                    String.IsNullOrEmpty(request.ArrivalTime);
            }
        }

        private static bool CheckForSameAirports(AddFlightRequest request)
        {
            lock (_lock)
            {
                return
                    request.From.AirportName.ToLower().Trim() == request.To.AirportName.ToLower().Trim();
            }
        }

        private static bool CheckForCorrectDates(AddFlightRequest request)
        {
            lock (_lock)
            {
                var tempArrival = new DateTime();
                var tempDeparture = new DateTime();
                if (DateTime.TryParse(request.DepartureTime, out tempDeparture) && DateTime.TryParse(request.ArrivalTime, out tempArrival))
                {
                    return tempArrival > tempDeparture;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
