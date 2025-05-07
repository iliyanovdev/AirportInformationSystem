namespace AirportInformationSystem.Web.ViewModels.FlightRoute.Admin
{
    public class FlightRouteListingViewModel
    {
        public int FlightRouteId { get; set; }

        public string DepartureCityName { get; set; } = null!;

        public string ArrivalCityName { get; set; } = null!;

        public string CompanyName { get; set; } = null!;

        public TimeSpan DepartureTime { get; set; }

        public TimeSpan Duration { get; set; }

        public decimal Price { get; set; }
    }
}
