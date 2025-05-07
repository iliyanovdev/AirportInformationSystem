using AirportInformationSystem.Data;
using AirportInformationSystem.Data.Models;
using AirportInformationSystem.Services;
using Microsoft.EntityFrameworkCore;

namespace AirportInformationSystem.Tests.Services
{
    [TestFixture]
    public class TicketServiceTests
    {
        private AirportInformationSystemDbContext _context = null!;
        private TicketService _ticketService = null!;

        // Initializes the in-memory database and TicketService before each test
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AirportInformationSystemDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AirportInformationSystemDbContext(options);
            _ticketService = new TicketService(_context);

            SeedData();
        }

        // Seeds test data into the in-memory database
        private void SeedData()
        {
            var cityA = new City { Id = Guid.NewGuid(), Name = "Sofia", Country = new Country { Name = "Bulgaria" } };
            var cityB = new City { Id = Guid.NewGuid(), Name = "Berlin", Country = new Country { Name = "Germany" } };
            var airplane = new AirplaneType { Id = Guid.NewGuid(), Model = "Boeing 737", Capacity = 1 };
            var company = new Company { Id = Guid.NewGuid(), Name = "AirTest", Prefix = "AT" };

            var route = new FlightRoute
            {
                Id = 1,
                DepartureCity = cityA,
                ArrivalCity = cityB,
                AirplaneType = airplane,
                Company = company,
                DepartureTime = new TimeSpan(8, 0, 0),
                FlightDuration = new TimeSpan(2, 0, 0),
                Price = 150,
                IsCancelled = false
            };

            var flight = new Flight
            {
                Id = Guid.NewGuid(),
                FlightCode = "AT123",
                FlightRoute = route,
                FlightDate = DateTime.Today.AddDays(1),
                IsCancelled = false
            };

            _context.Cities.AddRange(cityA, cityB);
            _context.AirplaneTypes.Add(airplane);
            _context.Companies.Add(company);
            _context.FlightRoutes.Add(route);
            _context.Flights.Add(flight);
            _context.SaveChanges();
        }

        // Tests if a ticket is successfully created for a valid user and flight
        [Test]
        public async Task BuyTicketAsync_WithValidData_AddsTicketAndReturnsTrue()
        {
            var flightId = _context.Flights.First().Id;
            var userId = Guid.NewGuid();

            var result = await _ticketService.BuyTicketAsync(flightId, userId);

            Assert.IsTrue(result);
            Assert.That(_context.Tickets.Any(t => t.FlightId == flightId && t.UserId == userId));
        }

        // Tests that a user cannot buy a ticket for a flight they already have a ticket for
        [Test]
        public async Task BuyTicketAsync_WhenUserAlreadyHasTicket_ReturnsFalse()
        {
            var flight = _context.Flights.Include(f => f.Tickets).First();
            var userId = Guid.NewGuid();

            _context.Tickets.Add(new Ticket
            {
                FlightId = flight.Id,
                UserId = userId,
                PurchaseDate = DateTime.Now,
                Price = 100
            });
            await _context.SaveChangesAsync();

            var result = await _ticketService.BuyTicketAsync(flight.Id, userId);

            Assert.IsFalse(result);
        }

        // Tests that no ticket is issued if the flight is already full
        [Test]
        public async Task BuyTicketAsync_WhenFlightIsFull_ReturnsFalse()
        {
            var flight = _context.Flights.Include(f => f.FlightRoute).ThenInclude(r => r.AirplaneType).First();
            var fullUserId = Guid.NewGuid();

            _context.Tickets.Add(new Ticket
            {
                FlightId = flight.Id,
                UserId = fullUserId,
                PurchaseDate = DateTime.Now,
                Price = 100
            });
            await _context.SaveChangesAsync();

            var anotherUserId = Guid.NewGuid();
            var result = await _ticketService.BuyTicketAsync(flight.Id, anotherUserId);

            Assert.IsFalse(result);
        }

        // Tests that no ticket is issued for a cancelled flight
        [Test]
        public async Task BuyTicketAsync_WhenFlightIsCancelled_ReturnsFalse()
        {
            var flight = _context.Flights.First();
            flight.IsCancelled = true;
            await _context.SaveChangesAsync();

            var result = await _ticketService.BuyTicketAsync(flight.Id, Guid.NewGuid());

            Assert.IsFalse(result);
        }

        // Disposes the database context after each test
        [TearDown]
        public async Task TearDown()
        {
            await _context.DisposeAsync();
        }
    }
}
