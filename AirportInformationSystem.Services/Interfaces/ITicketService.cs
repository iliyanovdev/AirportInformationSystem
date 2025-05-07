using AirportInformationSystem.Data.Models;
using AirportInformationSystem.Web.ViewModels.Ticket;

namespace AirportInformationSystem.Services.Interfaces
{
    public interface ITicketService
    {
        Task<bool> BuyTicketAsync(Guid flightId, Guid userId);
        Task<ConfirmBuyTicketViewModel?> GetConfirmBuyTicketViewModelAsync(Guid flightId);
        Task<List<TicketInfoViewModel>> GetUserTicketsAsync(Guid userId);
    }
}
