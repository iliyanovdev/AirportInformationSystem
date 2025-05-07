using AirportInformationSystem.Data;
using AirportInformationSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AirportInformationSystem.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly AirportInformationSystemDbContext _context;

        public CompanyService(AirportInformationSystemDbContext context)
        {
            _context = context;
        }

        public async Task<List<SelectListItem>> GetCompanyIdDropdownAsync()
        {
            List<SelectListItem> companies = await _context.Companies
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToListAsync();

            return companies;
        }

        public async Task<List<SelectListItem>> GetCompanyNameDropdownAsync()
        {
            List<SelectListItem> companies = await _context.Companies
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem
                {
                    Value = c.Name,
                    Text = c.Name
                })
                .ToListAsync();

            return companies;
        }
    }
}
