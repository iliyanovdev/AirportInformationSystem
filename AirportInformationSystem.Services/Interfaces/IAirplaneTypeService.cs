using Microsoft.AspNetCore.Mvc.Rendering;

namespace AirportInformationSystem.Services.Interfaces
{
    public interface IAirplaneTypeService
    {
        Task<List<SelectListItem>> GetAirplaneTypeIdDropdownAsync();
    }
}
