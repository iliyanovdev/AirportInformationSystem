using AirportInformationSystem.Data;
using AirportInformationSystem.Data.Models;
using AirportInformationSystem.Services;
using AirportInformationSystem.Web.ViewModels.Flight;
using Microsoft.EntityFrameworkCore;

namespace AirportInformationSystem.Tests.Services
{
    [TestFixture]
    public class FlightServiceTests
    {
        private AirportInformationSystemDbContext _context = null!;
        private FlightService _flightService = null!;

        // Initializes the in-memory database and FlightService before each test
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AirportInformationSystemDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // нова база всеки път
                .Options;

            _context = new AirportInformationSystemDbContext(options);
            _flightService = new FlightService(_context);

            SeedTestData();
        }

        // Seeds test data into the in-memory database
        private void SeedTestData()
        {
            City sofia = new City { Id = Guid.NewGuid(), Name = "Sofia", Country = new Country { Name = "Bulgaria" } };
            City berlin = new City { Id = Guid.NewGuid(), Name = "Berlin", Country = new Country { Name = "Germany" } };
            AirplaneType airplane = new AirplaneType { Id = Guid.NewGuid(), Model = "A320", Capacity = 150 };
            Company company = new Company { Id = Guid.NewGuid(), Name = "TestAir", Prefix = "TA" };

            FlightRoute route = new FlightRoute
            {
                Id = 1,
                DepartureCity = sofia,
                ArrivalCity = berlin,
                AirplaneType = airplane,
                Company = company,
                DepartureTime = new TimeSpan(10, 0, 0),
                FlightDuration = new TimeSpan(2, 30, 0),
                Price = 200,
                IsCancelled = false
            };

            Flight flight = new Flight
            {
                Id = Guid.NewGuid(),
                FlightCode = "TA123",
                FlightRoute = route,
                FlightDate = DateTime.Today.AddDays(1),
                IsCancelled = false
            };

            _context.Cities.AddRange(sofia, berlin);
            _context.AirplaneTypes.Add(airplane);
            _context.Companies.Add(company);
            _context.FlightRoutes.Add(route);
            _context.Flights.Add(flight);
            _context.SaveChanges();
        }

        // Tests if searching flights with valid cities and date returns correct flight
        [Test]
        public async Task SearchFlightsAsync_WithValidCitiesAndDate_ReturnsMatchingFlights()
        {
            // Arrange
            var model = new FlightSearchViewModel
            {
                DepartureCityName = "Sofia",
                ArrivalCityName = "Berlin",
                DepartureDate = DateTime.Today.AddDays(1),
                IsRoundTrip = false
            };

            // Act
            var result = await _flightService.SearchFlightsAsync(model, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.OutboundFlights.Count, Is.EqualTo(1));
            Assert.That(result.OutboundFlights[0].CompanyName, Is.EqualTo("TestAir"));
            Assert.That(result.OutboundFlights[0].DepartureCity, Is.EqualTo("Sofia"));
            Assert.That(result.OutboundFlights[0].ArrivalCity, Is.EqualTo("Berlin"));
        }

        // Tests if cancelled flights are excluded from the search results
        [Test]
        public async Task SearchFlightsAsync_WithCancelledFlight_ReturnsNoResults()
        {
            // Arrange
            var route = _context.FlightRoutes.First();
            var cancelledFlight = new Flight
            {
                Id = Guid.NewGuid(),
                FlightCode = "TA-CANCEL",
                FlightRouteId = route.Id,
                FlightDate = DateTime.Today.AddDays(2),
                IsCancelled = true
            };

            _context.Flights.Add(cancelledFlight);
            await _context.SaveChangesAsync();

            var model = new FlightSearchViewModel
            {
                DepartureCityName = route.DepartureCity.Name,
                ArrivalCityName = route.ArrivalCity.Name,
                DepartureDate = cancelledFlight.FlightDate,
                IsRoundTrip = false
            };

            // Act
            var result = await _flightService.SearchFlightsAsync(model, null);

            // Assert
            Assert.That(result.OutboundFlights.Count, Is.EqualTo(0));
        }

        // Tests if return flights are correctly retrieved when round-trip option is selected
        [Test]
        public async Task SearchFlightsAsync_WithRoundTripSelected_ReturnsReturnFlights()
        {
            // Arrange
            var departureCity = _context.Cities.First(c => c.Name == "Sofia");
            var arrivalCity = _context.Cities.First(c => c.Name == "Berlin");
            var airplane = _context.AirplaneTypes.First();
            var company = _context.Companies.First();

            var returnRoute = new FlightRoute
            {
                Id = 99,
                DepartureCity = arrivalCity, // Berlin
                ArrivalCity = departureCity, // Sofia
                AirplaneType = airplane,
                Company = company,
                DepartureTime = new TimeSpan(16, 0, 0),
                FlightDuration = new TimeSpan(2, 30, 0),
                Price = 180,
                IsCancelled = false
            };

            var returnFlight = new Flight
            {
                Id = Guid.NewGuid(),
                FlightCode = "TA-RETURN",
                FlightRoute = returnRoute,
                FlightDate = DateTime.Today.AddDays(3),
                IsCancelled = false
            };

            _context.FlightRoutes.Add(returnRoute);
            _context.Flights.Add(returnFlight);
            await _context.SaveChangesAsync();

            var model = new FlightSearchViewModel
            {
                DepartureCityName = "Sofia",
                ArrivalCityName = "Berlin",
                DepartureDate = DateTime.Today.AddDays(1),
                ReturnDate = DateTime.Today.AddDays(3),
                IsRoundTrip = true
            };

            // Act
            var result = await _flightService.SearchFlightsAsync(model, null);

            // Assert
            Assert.IsNotNull(result.ReturnFlights);
            Assert.That(result.ReturnFlights!.Count, Is.EqualTo(1));
            Assert.That(result.ReturnFlights.First().DepartureCity, Is.EqualTo("Berlin"));
            Assert.That(result.ReturnFlights.First().ArrivalCity, Is.EqualTo("Sofia"));
        }

        // Tests if search returns no results when there are no flights on the selected date
        [Test]
        public async Task SearchFlightsAsync_WhenNoFlightsOnDate_ReturnsEmpty()
        {
            // Arrange
            var model = new FlightSearchViewModel
            {
                DepartureCityName = "Sofia",
                ArrivalCityName = "Berlin",
                DepartureDate = DateTime.Today.AddYears(1),
                IsRoundTrip = false
            };

            // Act
            var result = await _flightService.SearchFlightsAsync(model, null);

            // Assert
            Assert.That(result.OutboundFlights.Count, Is.EqualTo(0));
        }

        // Tests if the system correctly marks that the user already has a ticket for the flight
        [Test]
        public async Task SearchFlightsAsync_UserHasTicket_UserAlreadyHasTicketSetToTrue()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var flight = _context.Flights.First();

            var ticket = new Ticket
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                FlightId = flight.Id,
                PurchaseDate = DateTime.Now,
                Price = flight.FlightRoute.Price
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            var model = new FlightSearchViewModel
            {
                DepartureCityName = flight.FlightRoute.DepartureCity.Name,
                ArrivalCityName = flight.FlightRoute.ArrivalCity.Name,
                DepartureDate = flight.FlightDate,
                IsRoundTrip = false
            };

            // Act
            var result = await _flightService.SearchFlightsAsync(model, userId);

            // Assert
            Assert.That(result.OutboundFlights.First().UserAlreadyHasTicket, Is.True);
        }

        // Disposes the database context after each test
        [TearDown]
        public async Task TearDown()
        {
            await _context.DisposeAsync();
        }
    }
}
