using Microsoft.AspNetCore.Mvc.Rendering;

namespace AirportInformationSystem.Services.Interfaces
{
    public interface ICityService
    {
        Task<List<SelectListItem>> GetCityNameDropdownAsync(); // Value = Name
        Task<List<SelectListItem>> GetCityIdDropdownAsync(); // Value = Id
    }
}
