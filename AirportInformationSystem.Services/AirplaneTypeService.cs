using AirportInformationSystem.Data;
using AirportInformationSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AirportInformationSystem.Services
{
    public class AirplaneTypeService:IAirplaneTypeService
    {
        private readonly AirportInformationSystemDbContext _context;

        public AirplaneTypeService(AirportInformationSystemDbContext context)
        {
            _context = context;
        }

        public async Task<List<SelectListItem>> GetAirplaneTypeIdDropdownAsync()
        {
            List<SelectListItem> cities = await _context.AirplaneTypes
                .OrderBy(c => c.Model)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Model
                })
                .ToListAsync();

            return cities;
        }
    }
}
