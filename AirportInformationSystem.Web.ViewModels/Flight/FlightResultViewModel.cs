namespace AirportInformationSystem.Web.ViewModels.Flight
{
    public class FlightResultViewModel
    {
        public Guid FlightId { get; set; }

        public string DepartureCity { get; set; } = null!;

        public string ArrivalCity { get; set; } = null!;

        public string CompanyName { get; set; } = null!;

        public decimal Price { get; set; }

        public TimeSpan DepartureTime { get; set; }

        public TimeSpan Duration { get; set; }

        public TimeSpan ArrivalTime { get; set; }

        public int AvailableSeats { get; set; }

        public bool UserAlreadyHasTicket { get; set; }
    }
}
