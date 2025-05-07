namespace AirportInformationSystem.Web.ViewModels.Flight.Admin
{
    public class FlightAdminResultViewModel
    {
        public Guid FlightId { get; set; }

        public string CompanyName { get; set; } = null!;

        public string DepartureCity { get; set; } = null!;

        public string ArrivalCity { get; set; } = null!;

        public DateTime FlightDate { get; set; }

        public TimeSpan DepartureTime { get; set; }

        public TimeSpan Duration { get; set; }

        public bool IsCancelled { get; set; }
    }
}
