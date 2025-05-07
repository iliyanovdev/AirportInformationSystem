using AirportInformationSystem.Data.Models;
using AirportInformationSystem.Services.Interfaces;
using AirportInformationSystem.Web.ViewModels.Ticket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AirportInformationSystem.Web.Controllers
{
    [Authorize]
    public class TicketController : Controller
    {
        private readonly ITicketService _ticketService;
        private readonly UserManager<ApplicationUser> _userManager;

        public TicketController(ITicketService ticketService, UserManager<ApplicationUser> userManager)
        {
            _ticketService = ticketService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> PurchasedTickets()
        {
            ApplicationUser? user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("Login", "Account");
            }

            List<TicketInfoViewModel> tickets = await _ticketService.GetUserTicketsAsync(user.Id);
            return View(tickets);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmBuy(Guid flightId, string? returnUrl)
        {
            ConfirmBuyTicketViewModel? flight = await _ticketService.GetConfirmBuyTicketViewModelAsync(flightId);

            if (flight == null)
            {
                return RedirectToAction("Search", "Flight");
            }

            ViewBag.ReturnUrl = returnUrl ?? Url.Action("Search", "Flight");

            return View(flight);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmBuyPost(Guid flightId, string? returnUrl)
        {
            ApplicationUser? user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("Login", "Account");
            }

            bool isPurchaseSuccessful = await _ticketService.BuyTicketAsync(flightId, user.Id);

            if (!isPurchaseSuccessful)
            {
                TempData["ErrorMessage"] = "You already have a ticket for this flight or there are no available seats.";
                return Redirect(GetSafeRedirectUrl(returnUrl));
            }

            TempData["SuccessMessage"] = "You have successfully purchased a ticket.";
            return Redirect(GetSafeRedirectUrl(returnUrl));
        }

        private string GetSafeRedirectUrl(string? returnUrl)
        {
            var defaultUrl = Url.Action("Search", "Flight") ?? "/";
            return string.IsNullOrWhiteSpace(returnUrl) ? defaultUrl : returnUrl;
        }
    }
}
