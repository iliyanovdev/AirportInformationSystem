using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using AirportInformationSystem.Data;
using AirportInformationSystem.Data.Models;
using AirportInformationSystem.Data.Seed;
using AirportInformationSystem.Services.Interfaces;
using AirportInformationSystem.Services;

namespace AirportInformationSystem.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // 1. Database configuration
            string connectionString = builder.Configuration.GetConnectionString("SQLServer")!;
            builder.Services.AddDbContext<AirportInformationSystemDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // 2. Identity configuration with ApplicationUser and roles (Guid)
            builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                ConfigureIdentity(builder, options);
            })
            .AddEntityFrameworkStores<AirportInformationSystemDbContext>()
            .AddDefaultTokenProviders();

            // 3. Adding scopes
            builder.Services.AddScoped<IFlightService, FlightService>();
            builder.Services.AddScoped<ITicketService, TicketService>();
            builder.Services.AddScoped<ICityService, CityService>();
            builder.Services.AddScoped<IFlightRouteService, FlightRouteService>();
            builder.Services.AddScoped<ICompanyService, CompanyService>();
            builder.Services.AddScoped<IAirplaneTypeService, AirplaneTypeService>();

            // 4. Add support for MVC pattern (Controllers and Views)
            builder.Services.AddControllersWithViews();

            // 5. Add support for Razor Pages (used by Identity)
            builder.Services.AddRazorPages();

            // 6. Build the application
            WebApplication app = builder.Build();

            // 7. Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // 8. Enable authentication and authorization middleware
            app.UseAuthentication();
            app.UseAuthorization();

            // 9. Configure areas routing
            app.MapControllerRoute(
                name: "Areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            // 10. Configure default routing
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            // 11. Seed the database with initial data
            using (IServiceScope scope = app.Services.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;
                AirportInformationSystemDbContext dbContext = services.GetRequiredService<AirportInformationSystemDbContext>();
                await AirportInformationSystemDbSeeder.SeedDynamicAsync(dbContext);
                await AirportInformationSystemDbSeeder.SeedRolesAndAdminUserAsync(services);
                await AirportInformationSystemDbSeeder.SeedUsersAndTicketsAsync(services);
            }

            app.Run();
        }

        private static void ConfigureIdentity(WebApplicationBuilder builder, IdentityOptions options)
        {
            // Password settings
            options.Password.RequireDigit =
                builder.Configuration.GetValue<bool>("Identity:Password:RequireDigits");
            options.Password.RequireLowercase =
                builder.Configuration.GetValue<bool>("Identity:Password:RequireLowercase");
            options.Password.RequireUppercase =
                builder.Configuration.GetValue<bool>("Identity:Password:RequireUppercase");
            options.Password.RequireNonAlphanumeric =
                builder.Configuration.GetValue<bool>("Identity:Password:RequireNonAlphanumeric");
            options.Password.RequiredLength =
                builder.Configuration.GetValue<int>("Identity:Password:RequiredLength");

            // SignIn settings
            options.SignIn.RequireConfirmedAccount =
                builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedAccount");
            options.SignIn.RequireConfirmedEmail =
                builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedEmail");
            options.SignIn.RequireConfirmedPhoneNumber =
                builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedPhoneNumber");

            // User settings
            options.User.RequireUniqueEmail =
                builder.Configuration.GetValue<bool>("Identity:User:RequireUniqueEmail");
        }
    }
}
