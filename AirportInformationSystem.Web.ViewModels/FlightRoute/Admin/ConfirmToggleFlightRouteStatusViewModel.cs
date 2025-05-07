namespace AirportInformationSystem.Web.ViewModels.FlightRoute.Admin
{
    public class ConfirmToggleFlightRouteStatusViewModel
    {
        public string? FilterDepartureCityName { get; set; }

        public string? FilterArrivalCityName { get; set; }


        public int FlightRouteId { get; set; }

        public string CompanyName { get; set; } = null!;

        public string DepartureCityName { get; set; } = null!;

        public string ArrivalCityName { get; set; } = null!;

        public TimeSpan DepartureTime { get; set; }

        public TimeSpan Duration { get; set; }

        public TimeSpan ArrivalTime { get; set; }

        public string AirplaneModel { get; set; } = null!;

        public decimal Price { get; set; }

        public bool IsCancelled { get; set; }

        public int NumberOfFlights { get; set; }

        public int NumberOfActiveFlights { get; set; }
    }
}
