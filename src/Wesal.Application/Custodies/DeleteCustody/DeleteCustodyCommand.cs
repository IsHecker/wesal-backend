using Wesal.Application.Messaging;

namespace Wesal.Application.Custodies.DeleteCustody;

public record struct DeleteCustodyCommand(Guid CourtId, Guid StaffId, Guid CustodyId) : ICommand;