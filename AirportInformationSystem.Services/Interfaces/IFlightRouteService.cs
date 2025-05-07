using AirportInformationSystem.Web.ViewModels.Flight.Admin;
using AirportInformationSystem.Web.ViewModels.FlightRoute.Admin;

namespace AirportInformationSystem.Services.Interfaces
{
    public interface IFlightRouteService
    {
        Task<IEnumerable<Web.ViewModels.FlightRoute.Admin.FlightRouteAdminResultViewModel>> GetFilteredFlightRoutesAsync(FlightRouteAdminFilterViewModel flightRouteAdminFilterViewModel);

        Task<ConfirmToggleFlightRouteStatusViewModel?> GetConfirmToggleFlightRouteStatusViewModelAsync(int flightRouteId);

        Task ToggleFlightRouteCancellationAsync(int flightRouteId);

        Task CreateFlightRouteAsync(FlightRouteCreateViewModel flightRouteCreateViewModel);

        Task<List<FlightRouteListingViewModel>> SearchRoutesAsync(string? fromCity, string? toCity, string? company);
    }
}
