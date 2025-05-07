using AirportInformationSystem.Web.ViewModels.Flight;
using AirportInformationSystem.Web.ViewModels.Flight.Admin;

namespace AirportInformationSystem.Services.Interfaces
{
    public interface IFlightService
    {
        Task<FlightSearchViewModel> SearchFlightsAsync(FlightSearchViewModel flightSearchViewModel, Guid? userId);

        Task<IEnumerable<FlightAdminResultViewModel>> GetFilteredFlightsAsync(FlightAdminFilterViewModel flightAdminFilterViewModel);

        Task<ConfirmToggleFlightStatusViewModel?> GetConfirmToogleFlightStatusViewModelAsync(Guid flightId);

        Task ToggleFlightCancellationAsync(Guid flightId);

        Task<CreateFlightViewModel?> GetCreateFlightViewModelAsync(int routeId);

        Task<bool> CreateFlightAsync(CreateFlightInputModel inputModel);
    }
}
