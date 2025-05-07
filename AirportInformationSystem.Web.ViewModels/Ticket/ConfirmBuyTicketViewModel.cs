namespace AirportInformationSystem.Web.ViewModels.Ticket
{
    public class ConfirmBuyTicketViewModel
    {
        public Guid FlightId { get; set; }

        public string DepartureCity { get; set; } = null!;

        public string ArrivalCity { get; set; } = null!;

        public TimeSpan DepartureTime { get; set; }

        public TimeSpan Duration { get; set; }

        public TimeSpan ArrivalTime { get; set; }

        public string CompanyName { get; set; } = null!;

        public decimal Price { get; set; }

        public DateTime Date { get; set; }
    }
}
