namespace AirportInformationSystem.Web.ViewModels.Ticket
{
    public class TicketInfoViewModel
    {
        public string DepartureCity { get; set; } = null!;

        public string ArrivalCity { get; set; } = null!;

        public DateTime FlightDate { get; set; }

        public TimeSpan DepartureTime { get; set; }

        public TimeSpan Duration { get; set; }

        public TimeSpan ArrivalTime { get; set; }

        public string CompanyName { get; set; } = null!;

        public decimal Price { get; set; }

        public DateTime PurchaseDate { get; set; }
    }
}
