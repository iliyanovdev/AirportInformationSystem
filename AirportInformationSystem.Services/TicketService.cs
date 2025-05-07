using AirportInformationSystem.Data;
using AirportInformationSystem.Data.Models;
using AirportInformationSystem.Services.Interfaces;
using AirportInformationSystem.Web.ViewModels.Ticket;
using Microsoft.EntityFrameworkCore;

namespace AirportInformationSystem.Services
{
    public class TicketService : ITicketService
    {
        private readonly AirportInformationSystemDbContext _context;

        public TicketService(AirportInformationSystemDbContext context)
        {
            _context = context;
        }

        public async Task<bool> BuyTicketAsync(Guid flightId, Guid userId)
        {
            Flight? flight = await _context.Flights
                .Include(f => f.Tickets)
                .Include(f => f.FlightRoute)
                    .ThenInclude(fr => fr.AirplaneType)
                .FirstOrDefaultAsync(f => f.Id == flightId && !f.IsCancelled);

            if (flight == null)
            {
                return false;
            }

            bool alreadyHasTicket = flight.Tickets.Any(t => t.UserId == userId);
            if (alreadyHasTicket)
            {
                return false;
            }

            if (flight.FlightRoute.AirplaneType.Capacity <= flight.Tickets.Count(t => t.FlightId == flightId))
            {
                return false;
            }

            Ticket ticket = new Ticket
            {
                FlightId = flightId,
                UserId = userId,
                PurchaseDate = DateTime.UtcNow,
                Price = flight.FlightRoute.Price
            };

            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ConfirmBuyTicketViewModel?> GetConfirmBuyTicketViewModelAsync(Guid flightId)
        {
            Flight? flight = await _context.Flights
                .Include(f => f.FlightRoute)
                .Include(f => f.FlightRoute.Company)
                .Include(f => f.FlightRoute.DepartureCity)
                .Include(f => f.FlightRoute.ArrivalCity)
                .FirstOrDefaultAsync(f => f.Id == flightId && !f.IsCancelled);

            if (flight == null) return null;

            return new ConfirmBuyTicketViewModel
            {
                FlightId = flight.Id,
                DepartureCity = flight.FlightRoute.DepartureCity.Name,
                ArrivalCity = flight.FlightRoute.ArrivalCity.Name,
                DepartureTime = flight.FlightRoute.DepartureTime,
                Duration = flight.FlightRoute.FlightDuration,
                ArrivalTime = flight.FlightRoute.DepartureTime.Add(flight.FlightRoute.FlightDuration),
                CompanyName = flight.FlightRoute.Company.Name,
                Price = flight.FlightRoute.Price,
                Date = flight.FlightDate
            };
        }

        public Task<List<TicketInfoViewModel>> GetUserTicketsAsync(Guid userId)
        {
            return _context.Tickets
                .Where(t => t.UserId == userId)
                .Include(t => t.Flight)
                    .ThenInclude(f => f.FlightRoute)
                .OrderBy(t => t.Flight.FlightDate < DateTime.Today)
                .ThenByDescending(t => t.Flight.FlightDate)
                .Select(t => new TicketInfoViewModel
                {
                    DepartureCity = t.Flight.FlightRoute.DepartureCity.Name,
                    ArrivalCity = t.Flight.FlightRoute.ArrivalCity.Name,
                    FlightDate = t.Flight.FlightDate,
                    DepartureTime = t.Flight.FlightRoute.DepartureTime,
                    Duration = t.Flight.FlightRoute.FlightDuration,
                    ArrivalTime = t.Flight.FlightRoute.DepartureTime.Add(t.Flight.FlightRoute.FlightDuration),
                    CompanyName = t.Flight.FlightRoute.Company.Name,
                    Price = t.Price,
                    PurchaseDate = t.PurchaseDate
                })
                .ToListAsync();
        }
    }
}
