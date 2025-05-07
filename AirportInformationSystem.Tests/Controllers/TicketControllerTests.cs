using AirportInformationSystem.Data.Models;
using AirportInformationSystem.Services.Interfaces;
using AirportInformationSystem.Web.Controllers;
using AirportInformationSystem.Web.ViewModels.Ticket;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System.Security.Claims;

namespace AirportInformationSystem.Tests.Controllers
{
    [TestFixture]
    public class TicketControllerTests
    {
        private Mock<ITicketService> _ticketServiceMock = null!;
        private Mock<UserManager<ApplicationUser>> _userManagerMock = null!;
#pragma warning disable NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method
        private TicketController _controller = null!;
#pragma warning restore NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method

        // Initializes mocks and the controller before each test
        [SetUp]
        public void Setup()
        {
            _ticketServiceMock = new Mock<ITicketService>();

            var store = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null!, null!, null!, null!, null!, null!, null!, null!);

            _controller = new TicketController(_ticketServiceMock.Object, _userManagerMock.Object);

            var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
            _controller.TempData = tempData;
        }

        // Tests if PurchasedTickets action returns the correct view with ticket list for a valid user
        [Test]
        public async Task PurchasedTickets_WithValidUser_ReturnsViewWithTickets()
        {
            // Arrange
            var user = new ApplicationUser { Id = Guid.NewGuid() };

            _userManagerMock.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            _ticketServiceMock.Setup(s => s.GetUserTicketsAsync(user.Id)).ReturnsAsync(new List<TicketInfoViewModel>());

            // Act
            var result = await _controller.PurchasedTickets();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOf<List<TicketInfoViewModel>>(viewResult.Model);
        }

        // Tests if PurchasedTickets action redirects to Login when the user is not found (null)
        [Test]
        public async Task PurchasedTickets_WithNullUser_RedirectsToLogin()
        {
            // Arrange
            _userManagerMock.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync((ApplicationUser?)null);

            // Act
            var result = await _controller.PurchasedTickets();

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = (RedirectToActionResult)result;
            Assert.That(redirectResult.ActionName, Is.EqualTo("Login"));
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Account"));
        }
    }
}
