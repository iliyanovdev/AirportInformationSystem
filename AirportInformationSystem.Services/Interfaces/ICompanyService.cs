using Microsoft.AspNetCore.Mvc.Rendering;

namespace AirportInformationSystem.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<List<SelectListItem>> GetCompanyIdDropdownAsync();
        Task<List<SelectListItem>> GetCompanyNameDropdownAsync();
    }
}
