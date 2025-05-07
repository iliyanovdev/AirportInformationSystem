namespace AirportInformationSystem.Web.ViewModels.Flight.Admin
{
    public class ConfirmToggleFlightStatusViewModel
    {
        public string? FilterDepartureCityName { get; set; }

        public string? FilterArrivalCityName { get; set; }

        public DateTime? FilterFlightDate { get; set; }


        public Guid FlightId { get; set; }

        public string FlightCode { get; set; } = null!;

        public string CompanyName { get; set; } = null!;

        public string DepartureCity { get; set; } = null!;

        public string ArrivalCity { get; set; } = null!;
        
        public DateTime FlightDate { get; set; }

        public TimeSpan DepartureTime { get; set; }

        public TimeSpan Duration { get; set; }

        public TimeSpan ArrivalTime { get; set; }

        public string AirplaneModel { get; set; } = null!;

        public int AvailableSeats { get; set; }

        public int TotalSeats { get; set; }

        public decimal Price { get; set; }

        public bool IsCancelled { get; set; }
    }
}
