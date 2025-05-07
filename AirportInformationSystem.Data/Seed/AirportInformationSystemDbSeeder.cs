using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;

using AirportInformationSystem.Data.Models;
using static AirportInformationSystem.Common.ApplicationConstants;
using static AirportInformationSystem.Common.EntityValidationConstants.Flight;

namespace AirportInformationSystem.Data.Seed
{
    public static class AirportInformationSystemDbSeeder
    {
        public static async Task SeedDynamicAsync(AirportInformationSystemDbContext context)
        {
            if (!context.Countries.Any() && !context.Cities.Any() &&
                !context.Companies.Any() && !context.AirplaneTypes.Any())
            {
                var countries = new List<Country>()
                {
                    new Country { Id = Guid.NewGuid(), Name = "United States" },
                    new Country { Id = Guid.NewGuid(), Name = "Canada" },
                    new Country { Id = Guid.NewGuid(), Name = "Mexico" },
                    new Country { Id = Guid.NewGuid(), Name = "United Kingdom" },
                    new Country { Id = Guid.NewGuid(), Name = "Germany" },
                    new Country { Id = Guid.NewGuid(), Name = "France" },
                    new Country { Id = Guid.NewGuid(), Name = "Bulgaria" },
                    new Country { Id = Guid.NewGuid(), Name = "Spain" },
                    new Country { Id = Guid.NewGuid(), Name = "Brazil" },
                    new Country { Id = Guid.NewGuid(), Name = "India" },
                    new Country { Id = Guid.NewGuid(), Name = "Italy" },
                    new Country { Id = Guid.NewGuid(), Name = "Australia" },
                    new Country { Id = Guid.NewGuid(), Name = "Japan" }
                };

                await context.Countries.AddRangeAsync(countries);
                await context.SaveChangesAsync();

                var cities = new List<City>()
                {
                    // United States
                    new City { Id = Guid.NewGuid(), Name = "New York", CountryId = countries.First(c => c.Name == "United States").Id },
                    new City { Id = Guid.NewGuid(), Name = "Los Angeles", CountryId = countries.First(c => c.Name == "United States").Id },
                    new City { Id = Guid.NewGuid(), Name = "Chicago", CountryId = countries.First(c => c.Name == "United States").Id },

                    // Canada
                    new City { Id = Guid.NewGuid(), Name = "Toronto", CountryId = countries.First(c => c.Name == "Canada").Id },
                    new City { Id = Guid.NewGuid(), Name = "Montreal", CountryId = countries.First(c => c.Name == "Canada").Id },

                    // Mexico
                    new City { Id = Guid.NewGuid(), Name = "Mexico City", CountryId = countries.First(c => c.Name == "Mexico").Id },
                    new City { Id = Guid.NewGuid(), Name = "Monterrey", CountryId = countries.First(c => c.Name == "Mexico").Id },

                    // United Kingdom
                    new City { Id = Guid.NewGuid(), Name = "London", CountryId = countries.First(c => c.Name == "United Kingdom").Id },
                    new City { Id = Guid.NewGuid(), Name = "Manchester", CountryId = countries.First(c => c.Name == "United Kingdom").Id },
                    new City { Id = Guid.NewGuid(), Name = "Edinburgh", CountryId = countries.First(c => c.Name == "United Kingdom").Id },

                    // Germany
                    new City { Id = Guid.NewGuid(), Name = "Berlin", CountryId = countries.First(c => c.Name == "Germany").Id },
                    new City { Id = Guid.NewGuid(), Name = "Munich", CountryId = countries.First(c => c.Name == "Germany").Id },
                    new City { Id = Guid.NewGuid(), Name = "Frankfurt", CountryId = countries.First(c => c.Name == "Germany").Id },

                    // France
                    new City { Id = Guid.NewGuid(), Name = "Paris", CountryId = countries.First(c => c.Name == "France").Id },
                    new City { Id = Guid.NewGuid(), Name = "Lyon", CountryId = countries.First(c => c.Name == "France").Id },
                    new City { Id = Guid.NewGuid(), Name = "Marseille", CountryId = countries.First(c => c.Name == "France").Id },

                    // Bulgaria
                    new City { Id = Guid.NewGuid(), Name = "Sofia", CountryId = countries.First(c => c.Name == "Bulgaria").Id },
                    new City { Id = Guid.NewGuid(), Name = "Plovdiv", CountryId = countries.First(c => c.Name == "Bulgaria").Id },
                    new City { Id = Guid.NewGuid(), Name = "Varna", CountryId = countries.First(c => c.Name == "Bulgaria").Id },

                    // Spain
                    new City { Id = Guid.NewGuid(), Name = "Madrid", CountryId = countries.First(c => c.Name == "Spain").Id },
                    new City { Id = Guid.NewGuid(), Name = "Barcelona", CountryId = countries.First(c => c.Name == "Spain").Id },
                    new City { Id = Guid.NewGuid(), Name = "Valencia", CountryId = countries.First(c => c.Name == "Spain").Id },

                    // Brazil
                    new City { Id = Guid.NewGuid(), Name = "Sao Paulo", CountryId = countries.First(c => c.Name == "Brazil").Id },
                    new City { Id = Guid.NewGuid(), Name = "Rio de Janeiro", CountryId = countries.First(c => c.Name == "Brazil").Id },

                    // India
                    new City { Id = Guid.NewGuid(), Name = "Delhi", CountryId = countries.First(c => c.Name == "India").Id },
                    new City { Id = Guid.NewGuid(), Name = "Mumbai", CountryId = countries.First(c => c.Name == "India").Id },

                    // Italy
                    new City { Id = Guid.NewGuid(), Name = "Rome", CountryId = countries.First(c => c.Name == "Italy").Id },
                    new City { Id = Guid.NewGuid(), Name = "Milan", CountryId = countries.First(c => c.Name == "Italy").Id },
                    new City { Id = Guid.NewGuid(), Name = "Naples", CountryId = countries.First(c => c.Name == "Italy").Id },

                    // Australia
                    new City { Id = Guid.NewGuid(), Name = "Sydney", CountryId = countries.First(c => c.Name == "Australia").Id },
                    new City { Id = Guid.NewGuid(), Name = "Melbourne", CountryId = countries.First(c => c.Name == "Australia").Id },

                    // Japan
                    new City { Id = Guid.NewGuid(), Name = "Tokyo", CountryId = countries.First(c => c.Name == "Japan").Id },
                    new City { Id = Guid.NewGuid(), Name = "Osaka", CountryId = countries.First(c => c.Name == "Japan").Id },
                };

                await context.Cities.AddRangeAsync(cities);
                await context.SaveChangesAsync();

                var companies = new List<Company>
                {
                    new Company { Id = Guid.NewGuid(), Name = "SkyJet", Prefix = "SJ" },
                    new Company { Id = Guid.NewGuid(), Name = "FlyFast", Prefix = "FF" },
                    new Company { Id = Guid.NewGuid(), Name = "AirBalkan", Prefix = "AB" },
                    new Company { Id = Guid.NewGuid(), Name = "JetStream", Prefix = "JS" },
                    new Company { Id = Guid.NewGuid(), Name = "WingsAir", Prefix = "WA" },
                    new Company { Id = Guid.NewGuid(), Name = "BlueHorizon", Prefix = "BH" },
                    new Company { Id = Guid.NewGuid(), Name = "GlobalFly", Prefix = "GF" },
                    new Company { Id = Guid.NewGuid(), Name = "Skyline Airways", Prefix = "SA" },
                    new Company { Id = Guid.NewGuid(), Name = "JetWorld", Prefix = "JW" },
                    new Company { Id = Guid.NewGuid(), Name = "AeroWings", Prefix = "AW" }
                };

                await context.Companies.AddRangeAsync(companies);
                await context.SaveChangesAsync();

                var airplaneTypes = new List<AirplaneType>
                {
                    new AirplaneType { Id = Guid.NewGuid(), Model = "Airbus A320", Capacity = 180 },
                    new AirplaneType { Id = Guid.NewGuid(), Model = "Airbus A321", Capacity = 220 },
                    new AirplaneType { Id = Guid.NewGuid(), Model = "Airbus A330", Capacity = 277 },
                    new AirplaneType { Id = Guid.NewGuid(), Model = "Airbus A350", Capacity = 300 },
                    new AirplaneType { Id = Guid.NewGuid(), Model = "Boeing 737-800", Capacity = 189 },
                    new AirplaneType { Id = Guid.NewGuid(), Model = "Boeing 737 MAX 8", Capacity = 178 },
                    new AirplaneType { Id = Guid.NewGuid(), Model = "Boeing 747-400", Capacity = 416 },
                    new AirplaneType { Id = Guid.NewGuid(), Model = "Boeing 767-300", Capacity = 218 },
                    new AirplaneType { Id = Guid.NewGuid(), Model = "Boeing 777-300", Capacity = 396 },
                    new AirplaneType { Id = Guid.NewGuid(), Model = "Boeing 787-9 Dreamliner", Capacity = 296 },
                    new AirplaneType { Id = Guid.NewGuid(), Model = "Embraer E190", Capacity = 100 },
                    new AirplaneType { Id = Guid.NewGuid(), Model = "Embraer E195", Capacity = 120 },
                    new AirplaneType { Id = Guid.NewGuid(), Model = "Bombardier CRJ900", Capacity = 90 },
                    new AirplaneType { Id = Guid.NewGuid(), Model = "ATR 72-600", Capacity = 70 },
                    new AirplaneType { Id = Guid.NewGuid(), Model = "McDonnell Douglas MD-80", Capacity = 155 }
                };

                await context.AirplaneTypes.AddRangeAsync(airplaneTypes);
                await context.SaveChangesAsync();
            }

            var random = new Random();

            if (!context.FlightRoutes.Any())
            {
                var cities = await context.Cities.ToListAsync();
                var companies = await context.Companies.ToListAsync();
                var airplaneTypes = await context.AirplaneTypes.ToListAsync();

                var flightRoutes = new List<FlightRoute>();

                foreach (City departureCity in cities)
                {
                    foreach (City arrivalCity in cities)
                    {
                        if (departureCity.Id == arrivalCity.Id)
                            continue;

                        int numberOfFlightRoutes = random.Next(2, 5);

                        for (int i = 0; i < numberOfFlightRoutes; i++)
                        {
                            Company company = companies[random.Next(companies.Count)];
                            AirplaneType airplaneType = airplaneTypes[random.Next(airplaneTypes.Count)];

                            TimeSpan departureTime = new TimeSpan(random.Next(0, 24), random.Next(0, 60), 0);
                            TimeSpan duration = TimeSpan.FromMinutes(random.Next(40, 720));

                            decimal baseRatePerMinute = 1.2m;
                            int durationMinutes = (int)duration.TotalMinutes;
                            decimal price = Math.Round(durationMinutes * baseRatePerMinute + random.Next(20, 80), 2);

                            flightRoutes.Add(new FlightRoute
                            {
                                CompanyId = company.Id,
                                AirplaneTypeId = airplaneType.Id,
                                DepartureCityId = departureCity.Id,
                                ArrivalCityId = arrivalCity.Id,
                                DepartureTime = departureTime,
                                FlightDuration = duration,
                                Price = price,
                                IsCancelled = false
                            });
                        }
                    }
                }
                await context.FlightRoutes.AddRangeAsync(flightRoutes);
                await context.SaveChangesAsync();
            }

            if (!context.Flights.Any())
            {
                var insertedRoutes = await context.FlightRoutes
                .Include(fr => fr.Company)
                .ToListAsync();

                var flights = new List<Flight>();

                foreach (FlightRoute flightRoute in insertedRoutes)
                {
                    Company company = flightRoute.Company;
                    for (int dayCounter = 0; dayCounter < 90; dayCounter++)
                    {
                        DateTime date = DateTime.Today.AddDays(dayCounter);

                        // example: JA-1732-20250426
                        string flightCode = $"{company.Prefix}-{flightRoute.Id}-{date.ToString(FlightCodeDateFormat)}";

                        flights.Add(new Flight
                        {
                            Id = Guid.NewGuid(),
                            FlightCode = flightCode,
                            FlightRouteId = flightRoute.Id,
                            FlightDate = date,
                            IsCancelled = false
                        });
                    }
                }
                await context.Flights.AddRangeAsync(flights);
                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedRolesAndAdminUserAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = { AdminRoleName, ClientRoleName };

            foreach (string roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                }
            }

            string adminEmail = "admin@airport.com";
            string adminUserName = "admin";
            string adminPassword = "admin123";

            ApplicationUser? admin = await userManager.FindByNameAsync(adminUserName);
            if (admin == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = adminUserName,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                IdentityResult result = await userManager.CreateAsync(newAdmin, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, AdminRoleName);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error creating admin: {error.Description}");
                    }
                }
            }
        }

        public static async Task SeedUsersAndTicketsAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var dbContext = serviceProvider.GetRequiredService<AirportInformationSystemDbContext>();

            if (!await roleManager.RoleExistsAsync(ClientRoleName))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(ClientRoleName));
            }

            var clientsToCreate = new[]
            {
                new { UserName = "stamo", Email = "stamo@gmail.com" },
                new { UserName = "pesho", Email = "pesho@gmail.com" },
                new { UserName = "mitko", Email = "mitko@gmail.com" },
                new { UserName = "maria", Email = "maria@gmail.com" },
                new { UserName = "vlado", Email = "vlado@gmail.com" },
            };

            foreach (var client in clientsToCreate)
            {
                ApplicationUser? existing = await userManager.FindByNameAsync(client.UserName);
                if (existing == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = client.UserName,
                        Email = client.Email,
                        EmailConfirmed = true
                    };

                    IdentityResult result = await userManager.CreateAsync(user, $"{client.UserName}123");
                    
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, ClientRoleName);
                    }
                }
            }

            var clientUserNames = clientsToCreate.Select(c => c.UserName).ToList();

            var clients = await userManager.Users
                .Where(u => clientUserNames.Contains(u.UserName!))
                .ToListAsync();

            var clientUserIds = clients.Select(c => c.Id).ToList();

            bool anyTicketsExist = await dbContext.Tickets
                .AnyAsync(t => clientUserIds.Contains(t.UserId));


            if (anyTicketsExist)
            {
                return;
            }

            var allActiveFlights = await dbContext.Flights
                .Include(f => f.FlightRoute)
                .ThenInclude(fr => fr.AirplaneType)
                .Where(f => !f.IsCancelled)
                .ToListAsync();

            var random = new Random();
            var tickets = new List<Ticket>();

            foreach (var client in clients)
            {
                int ticketCount = random.Next(5, 10);

                var selectedFlights = allActiveFlights.OrderBy(x => random.Next()).Take(ticketCount).ToList();

                foreach (var flight in selectedFlights)
                {
                    tickets.Add(new Ticket
                    {
                        Id = Guid.NewGuid(),
                        UserId = client.Id,
                        FlightId = flight.Id,
                        PurchaseDate = DateTime.Today.AddDays(-random.Next(1, 30)),
                        Price = flight.FlightRoute.Price
                    });
                }
            }

            await dbContext.Tickets.AddRangeAsync(tickets);
            await dbContext.SaveChangesAsync();
        }
    }
}
