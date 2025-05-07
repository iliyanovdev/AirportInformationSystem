using AirportInformationSystem.Data.Models;
using AirportInformationSystem.Services.Interfaces;
using AirportInformationSystem.Web.Controllers;
using AirportInformationSystem.Web.ViewModels.Flight;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using System.Security.Claims;

namespace AirportInformationSystem.Tests.Controllers
{
    [TestFixture]
    public class FlightControllerTests
    {
        private Mock<IFlightService> _flightServiceMock = null!;
        private Mock<ICityService> _cityServiceMock = null!;
        private Mock<UserManager<ApplicationUser>> _userManagerMock = null!;
#pragma warning disable NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method
        private FlightController _controller;
#pragma warning restore NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method

        // Initializes mocks and controller before each test
        [SetUp]
        public void Setup()
        {
            _flightServiceMock = new Mock<IFlightService>();
            _cityServiceMock = new Mock<ICityService>();

            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);

            _controller = new FlightController(_flightServiceMock.Object, _cityServiceMock.Object, _userManagerMock.Object);
        }

        // Tests if the Search action calls the flight service with valid input and returns a ViewResult
        [Test]
        public async Task Search_WithValidInput_CallsSearchFlightsAndReturnsView()
        {
            // Arrange
            var user = new ApplicationUser { Id = Guid.NewGuid() };

            _userManagerMock.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(user);

            _cityServiceMock.Setup(c => c.GetCityNameDropdownAsync())
                .ReturnsAsync(new List<SelectListItem>());

            _flightServiceMock.Setup(f => f.SearchFlightsAsync(It.IsAny<FlightSearchViewModel>(), user.Id))
                .ReturnsAsync((FlightSearchViewModel model, Guid? userId) => model);

            // Act
            IActionResult result = await _controller.Search("Sofia", "Berlin", DateTime.Today.AddDays(1), false, null);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOf<FlightSearchViewModel>(viewResult.Model);

            _flightServiceMock.Verify(f => f.SearchFlightsAsync(It.IsAny<FlightSearchViewModel>(), user.Id), Times.Once);
            _cityServiceMock.Verify(c => c.GetCityNameDropdownAsync(), Times.Once);
        }

        // Tests if the Search action adds a model error when departure and arrival cities are the same
        [Test]
        public async Task Search_WithSameDepartureAndArrivalCities_AddsModelError()
        {
            // Arrange
            _cityServiceMock.Setup(c => c.GetCityNameDropdownAsync())
                .ReturnsAsync(new List<SelectListItem>());

            // Act
            var result = await _controller.Search("Sofia", "Sofia", DateTime.Today.AddDays(1));

            // Assert
            Assert.IsFalse(_controller.ModelState.IsValid);
            Assert.That(_controller.ModelState[nameof(FlightSearchViewModel.ArrivalCityName)]!.Errors.Count, Is.EqualTo(1));

            _flightServiceMock.Verify(f => f.SearchFlightsAsync(It.IsAny<FlightSearchViewModel>(), It.IsAny<Guid?>()), Times.Never);
        }

        // Tests if the Search action adds a model error when round-trip is selected but return date is missing
        [Test]
        public async Task Search_WithRoundTripButNoReturnDate_AddsModelError()
        {
            // Arrange
            _cityServiceMock.Setup(c => c.GetCityNameDropdownAsync())
                .ReturnsAsync(new List<SelectListItem>());

            // Act
            var result = await _controller.Search("Sofia", "Berlin", DateTime.Today.AddDays(1), true, null);

            // Assert
            Assert.IsFalse(_controller.ModelState.IsValid);
            Assert.That(_controller.ModelState[nameof(FlightSearchViewModel.ReturnDate)]!.Errors.Count, Is.EqualTo(1));
            _flightServiceMock.Verify(f => f.SearchFlightsAsync(It.IsAny<FlightSearchViewModel>(), It.IsAny<Guid?>()), Times.Never);
        }

        // Tests if the Search action adds a model error when return date is before departure date
        [Test]
        public async Task Search_WithReturnDateBeforeDeparture_AddsModelError()
        {
            // Arrange
            _cityServiceMock.Setup(c => c.GetCityNameDropdownAsync())
                .ReturnsAsync(new List<SelectListItem>());

            // Act
            var result = await _controller.Search("Sofia", "Berlin", DateTime.Today.AddDays(3), true, DateTime.Today.AddDays(1));

            // Assert
            Assert.IsFalse(_controller.ModelState.IsValid);
            Assert.That(_controller.ModelState[nameof(FlightSearchViewModel.ReturnDate)]!.Errors.Count, Is.EqualTo(1));
            _flightServiceMock.Verify(f => f.SearchFlightsAsync(It.IsAny<FlightSearchViewModel>(), It.IsAny<Guid?>()), Times.Never);
        }
    }
}
