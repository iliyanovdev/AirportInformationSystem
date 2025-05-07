using AirportInformationSystem.Data;
using AirportInformationSystem.Data.Models;
using AirportInformationSystem.Services.Interfaces;
using AirportInformationSystem.Web.ViewModels.FlightRoute.Admin;
using Microsoft.EntityFrameworkCore;

namespace AirportInformationSystem.Services
{
    public class FlightRouteService : IFlightRouteService
    {
        private readonly AirportInformationSystemDbContext _context;

        public FlightRouteService(AirportInformationSystemDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FlightRouteAdminResultViewModel>> GetFilteredFlightRoutesAsync(FlightRouteAdminFilterViewModel flightRouteAdminFilterViewModel)
        {
            var filteredFlightRoutes = await _context.FlightRoutes
                .Include(fr => fr.DepartureCity)
                .Include(fr => fr.ArrivalCity)
                .Include(fr => fr.Company)
                .Include(fr => fr.AirplaneType)
                .Where(fr => fr.DepartureCity.Name.ToLower() == flightRouteAdminFilterViewModel.DepartureCityName.ToLower() &&
                        fr.ArrivalCity.Name.ToLower() == flightRouteAdminFilterViewModel.ArrivalCityName.ToLower())
                .OrderBy(fr => fr.Company.Name)
                .ThenBy(fr => fr.DepartureTime)
                .Select(fr => new FlightRouteAdminResultViewModel()
                {
                    FlightRouteId = fr.Id,
                    CompanyName = fr.Company.Name,
                    DepartureCityName = fr.DepartureCity.Name,
                    ArrivalCityName = fr.ArrivalCity.Name,
                    DepartureTime = fr.DepartureTime,
                    Duration = fr.FlightDuration,
                    IsCancelled = fr.IsCancelled
                })
                .ToListAsync();

            return filteredFlightRoutes;

        }
        public async Task<ConfirmToggleFlightRouteStatusViewModel?> GetConfirmToggleFlightRouteStatusViewModelAsync(int flightRouteId)
        {
            FlightRoute? flightRoute = await _context.FlightRoutes
                .Include(fr => fr.DepartureCity)
                .Include(fr => fr.ArrivalCity)
                .Include(fr => fr.Company)
                .Include(fr => fr.AirplaneType)
                .Include(fr => fr.Flights)
                .FirstOrDefaultAsync(fr => fr.Id == flightRouteId);

            if (flightRoute == null) return null;

            var viewModel = new ConfirmToggleFlightRouteStatusViewModel
            {
                FlightRouteId = flightRoute.Id,
                CompanyName = flightRoute.Company.Name,
                DepartureCityName = flightRoute.DepartureCity.Name,
                ArrivalCityName = flightRoute.ArrivalCity.Name,
                DepartureTime = flightRoute.DepartureTime,
                Duration = flightRoute.FlightDuration,
                ArrivalTime = flightRoute.DepartureTime.Add(flightRoute.FlightDuration),
                AirplaneModel = flightRoute.AirplaneType.Model,
                Price = flightRoute.Price,
                IsCancelled = flightRoute.IsCancelled,
                NumberOfFlights = flightRoute.Flights.Count(),
                NumberOfActiveFlights = flightRoute.Flights.Count(f => !f.IsCancelled)
            };

            return viewModel;
        }

        public async Task ToggleFlightRouteCancellationAsync(int flightRouteId)
        {
            FlightRoute? flightRoute = await _context.FlightRoutes.FindAsync(flightRouteId);

            if (flightRoute != null)
            {
                flightRoute.IsCancelled = !flightRoute.IsCancelled;
                _context.FlightRoutes.Update(flightRoute);
                await _context.SaveChangesAsync();
            }
        }

        public async Task CreateFlightRouteAsync(FlightRouteCreateViewModel flightRouteCreateViewModel)
        {
            FlightRoute newRoute = new FlightRoute()
            {
                DepartureCityId = flightRouteCreateViewModel.DepartureCityId,
                ArrivalCityId = flightRouteCreateViewModel.ArrivalCityId,
                CompanyId = flightRouteCreateViewModel.CompanyId,
                AirplaneTypeId = flightRouteCreateViewModel.AirplaneTypeId,
                DepartureTime = flightRouteCreateViewModel.DepartureTime,
                FlightDuration = flightRouteCreateViewModel.FlightDuration,
                Price = flightRouteCreateViewModel.Price,
                IsCancelled = false
            };

            await _context.FlightRoutes.AddAsync(newRoute);
            await _context.SaveChangesAsync();
        }

        public async Task<List<FlightRouteListingViewModel>> SearchRoutesAsync(string? departureCityName, string? arrivalCityName, string? companyName)
        {
            var query = _context.FlightRoutes
           .Include(fr => fr.DepartureCity)
           .Include(fr => fr.ArrivalCity)
           .Include(fr => fr.Company)
           .Where(fr => !fr.IsCancelled)
           .AsQueryable();

            if (!string.IsNullOrWhiteSpace(departureCityName))
                query = query.Where(fr => fr.DepartureCity.Name.ToLower() == departureCityName);

            if (!string.IsNullOrWhiteSpace(arrivalCityName))
                query = query.Where(fr => fr.ArrivalCity.Name.ToLower() == arrivalCityName);

            if (!string.IsNullOrWhiteSpace(companyName))
                query = query.Where(fr => fr.Company.Name.ToLower() == companyName);

            return await query
                .OrderBy(fr => fr.DepartureCity.Name)
                .ThenBy(fr => fr.ArrivalCity.Name)
                .ThenBy(fr => fr.Company.Name)
                .Select(fr => new FlightRouteListingViewModel
                {
                    FlightRouteId = fr.Id,
                    DepartureCityName = fr.DepartureCity.Name,
                    ArrivalCityName = fr.ArrivalCity.Name,
                    CompanyName = fr.Company.Name,
                    DepartureTime = fr.DepartureTime,
                    Duration = fr.FlightDuration,
                    Price = fr.Price
                }).ToListAsync();
        }
    }
}
