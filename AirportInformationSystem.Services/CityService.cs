using AirportInformationSystem.Data;
using AirportInformationSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AirportInformationSystem.Services
{
    public class CityService : ICityService
    {
        private readonly AirportInformationSystemDbContext _context;

        public CityService(AirportInformationSystemDbContext context)
        {
            _context = context;
        }

        public async Task<List<SelectListItem>> GetCityNameDropdownAsync()
        {
            List<SelectListItem> cities = await _context.Cities
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem
                {
                    Value = c.Name,
                    Text = c.Name
                })
                .ToListAsync();

            return cities;
        }

        public async Task<List<SelectListItem>> GetCityIdDropdownAsync()
        {
            List<SelectListItem> cities = await _context.Cities
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToListAsync();

            return cities;
        }
    }
}
