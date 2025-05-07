using Microsoft.EntityFrameworkCore;
using AirportInformationSystem.Data;
using AirportInformationSystem.Services.Interfaces;
using AirportInformationSystem.Web.ViewModels.Flight;
using AirportInformationSystem.Web.ViewModels.Flight.Admin;
using AirportInformationSystem.Data.Models;
using Microsoft.AspNetCore.Routing;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AirportInformationSystem.Services
{
    public class FlightService : IFlightService
    {
        private readonly AirportInformationSystemDbContext _context;

        public FlightService(AirportInformationSystemDbContext context)
        {
            _context = context;
        }

        public async Task<FlightSearchViewModel> SearchFlightsAsync(FlightSearchViewModel flightSearchViewModel, Guid? userId)
        {
            var userTickets = new HashSet<Guid>();

            if (userId.HasValue)
            {
                userTickets = _context.Tickets
                    .Where(t => t.UserId == userId.Value)
                    .Select(t => t.FlightId)
                    .ToHashSet();
            }

            List<FlightResultViewModel> outboundFlights = await _context.Flights
                .Where(f => !f.IsCancelled &&
                        !f.FlightRoute.IsCancelled &&
                        f.FlightDate == flightSearchViewModel.DepartureDate &&
                        f.FlightRoute.DepartureCity.Name.ToLower() == flightSearchViewModel.DepartureCityName.ToLower() &&
                        f.FlightRoute.ArrivalCity.Name.ToLower() == flightSearchViewModel.ArrivalCityName.ToLower())
                .OrderBy(f => f.FlightRoute.DepartureTime)
                .ThenBy(f => f.FlightRoute.Price)
                .Select(f => new FlightResultViewModel
                {
                    FlightId = f.Id ,
                    DepartureCity = f.FlightRoute.DepartureCity.Name,
                    ArrivalCity = f.FlightRoute.ArrivalCity.Name,
                    CompanyName = f.FlightRoute.Company.Name,
                    Price = f.FlightRoute.Price,
                    DepartureTime = f.FlightRoute.DepartureTime,
                    Duration = f.FlightRoute.FlightDuration,
                    ArrivalTime = f.FlightRoute.DepartureTime.Add(f.FlightRoute.FlightDuration),
                    AvailableSeats = f.FlightRoute.AirplaneType.Capacity - f.Tickets.Count(t => t.FlightId == f.Id),
                    UserAlreadyHasTicket = userTickets.Contains(f.Id)
                })
                .ToListAsync();

            List<FlightResultViewModel>? returnFlights = null;

            if (flightSearchViewModel.IsRoundTrip && flightSearchViewModel.ReturnDate.HasValue)
            {
                returnFlights = await _context.Flights
                    .Where(f => !f.IsCancelled &&
                            !f.FlightRoute.IsCancelled &&
                            f.FlightDate == flightSearchViewModel.ReturnDate &&
                            f.FlightRoute.DepartureCity.Name.ToLower() == flightSearchViewModel.ArrivalCityName.ToLower() &&
                            f.FlightRoute.ArrivalCity.Name.ToLower() == flightSearchViewModel.DepartureCityName.ToLower())
                    .OrderBy(f => f.FlightRoute.DepartureTime)
                    .ThenBy(f => f.FlightRoute.Price)
                    .Select(f => new FlightResultViewModel
                    {
                        FlightId = f.Id,
                        DepartureCity = f.FlightRoute.DepartureCity.Name,
                        ArrivalCity = f.FlightRoute.ArrivalCity.Name,
                        CompanyName = f.FlightRoute.Company.Name,
                        Price = f.FlightRoute.Price,
                        DepartureTime = f.FlightRoute.DepartureTime,
                        Duration = f.FlightRoute.FlightDuration,
                        ArrivalTime = f.FlightRoute.DepartureTime.Add(f.FlightRoute.FlightDuration),
                        AvailableSeats = f.FlightRoute.AirplaneType.Capacity - f.Tickets.Count(t => t.FlightId == f.Id),
                        UserAlreadyHasTicket = userTickets.Contains(f.Id)
                    })
                    .ToListAsync();
            }

            flightSearchViewModel.OutboundFlights = outboundFlights;
            flightSearchViewModel.ReturnFlights = returnFlights;

            return flightSearchViewModel;
        }

        public async Task<IEnumerable<FlightAdminResultViewModel>> GetFilteredFlightsAsync(FlightAdminFilterViewModel flightAdminFilterViewModel)
        {
            var filteredFlights = await _context.Flights
                .Include(f => f.FlightRoute)
                .Include(f => f.FlightRoute.Company)
                .Include(f => f.FlightRoute.DepartureCity)
                .Include(f => f.FlightRoute.ArrivalCity)
                .Where(f => !f.FlightRoute.IsCancelled &&
                            f.FlightRoute.DepartureCity.Name.ToLower() == flightAdminFilterViewModel.DepartureCityName.ToLower() &&
                            f.FlightRoute.ArrivalCity.Name.ToLower() == flightAdminFilterViewModel.ArrivalCityName.ToLower() &&
                            f.FlightDate.Date == flightAdminFilterViewModel.FlightDate)
                .OrderBy(f => f.FlightRoute.Company.Name)
                .ThenBy(f => f.FlightRoute.DepartureTime)
                .Select(f => new FlightAdminResultViewModel
                {
                    FlightId = f.Id,
                    DepartureCity = f.FlightRoute.DepartureCity.Name,
                    ArrivalCity = f.FlightRoute.ArrivalCity.Name,
                    CompanyName = f.FlightRoute.Company.Name,
                    FlightDate = f.FlightDate,
                    DepartureTime = f.FlightRoute.DepartureTime,
                    Duration = f.FlightRoute.FlightDuration,
                    IsCancelled = f.IsCancelled
                })
                .ToListAsync();

            return filteredFlights;
        }

        public async Task<ConfirmToggleFlightStatusViewModel?> GetConfirmToogleFlightStatusViewModelAsync(Guid flightId)
        {
            Flight? flight =  await _context.Flights
                .Include(f => f.FlightRoute)
                .Include(f => f.FlightRoute.Company)
                .Include(f => f.FlightRoute.DepartureCity)
                .Include(f => f.FlightRoute.ArrivalCity)
                .Include(f => f.FlightRoute.AirplaneType)
                .Include(f => f.Tickets)
                .FirstOrDefaultAsync(f => f.Id == flightId);

            if (flight == null) return null;

            var viewModel = new ConfirmToggleFlightStatusViewModel
            {
                FlightId = flight.Id,
                FlightCode = flight.FlightCode,
                CompanyName = flight.FlightRoute.Company.Name,
                DepartureCity = flight.FlightRoute.DepartureCity.Name,
                ArrivalCity = flight.FlightRoute.ArrivalCity.Name,
                FlightDate = flight.FlightDate,
                DepartureTime = flight.FlightRoute.DepartureTime,
                Duration = flight.FlightRoute.FlightDuration,
                ArrivalTime = flight.FlightRoute.DepartureTime.Add(flight.FlightRoute.FlightDuration),
                AirplaneModel = flight.FlightRoute.AirplaneType.Model,
                AvailableSeats = flight.FlightRoute.AirplaneType.Capacity - flight.Tickets.Count(t => t.FlightId == flight.Id),
                TotalSeats = flight.FlightRoute.AirplaneType.Capacity,
                Price = flight.FlightRoute.Price,
                IsCancelled = flight.IsCancelled
            };

            return viewModel;
        }

        public async Task ToggleFlightCancellationAsync(Guid flightId)
        {
            Flight? flight = await _context.Flights.FindAsync(flightId);

            if (flight != null)
            {
                flight.IsCancelled = !flight.IsCancelled;
                _context.Flights.Update(flight);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<CreateFlightViewModel?> GetCreateFlightViewModelAsync(int routeId)
        {
            var route = await _context.FlightRoutes
            .Include(r => r.Company)
            .Include(r => r.DepartureCity).ThenInclude(c => c.Country)
            .Include(r => r.ArrivalCity).ThenInclude(c => c.Country)
            .FirstOrDefaultAsync(r => r.Id == routeId);

            if (route == null) return null;

            return new CreateFlightViewModel
            {
                FlightRouteId = route.Id,
                DepartureCityName = route.DepartureCity.Name,
                ArrivalCityName = route.ArrivalCity.Name,
                DepartureCountryName = route.DepartureCity.Country.Name,
                ArrivalCountryName = route.ArrivalCity.Country.Name,
                CompanyName = route.Company.Name,
                DepartureTime = route.DepartureTime,
                FlightDuration = route.FlightDuration,
                ArrivalTime = route.DepartureTime.Add(route.FlightDuration),
                Price = route.Price,
                FlightDate = DateTime.Today
            };
        }

        public async Task<bool> CreateFlightAsync(CreateFlightInputModel inputModel)
        {
            var route = await _context.FlightRoutes
               .Include(fr => fr.Company)
               .FirstOrDefaultAsync(fr => fr.Id == inputModel.FlightRouteId);

            if (route == null) return false;

            string flightCode = $"{route.Company.Prefix}-{inputModel.FlightRouteId}-{inputModel.FlightDate:yyyyMMdd}";

            var exists = await _context.Flights
                .AnyAsync(f => f.FlightRouteId == inputModel.FlightRouteId && f.FlightDate == inputModel.FlightDate);

            if (exists) return false;

            var flight = new Flight
            {
                FlightRouteId = inputModel.FlightRouteId,
                FlightDate = inputModel.FlightDate.Date,
                FlightCode = flightCode,
                IsCancelled = false
            };

            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
