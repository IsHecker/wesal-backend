using Wesal.Application.Messaging;

namespace Wesal.Application.Alimonies.DeleteAlimony
{
    public record struct DeleteAlimonyCommand(Guid CourtId, Guid AlimonyId) : ICommand;
}