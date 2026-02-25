using Wesal.Application.Messaging;

namespace Wesal.Application.Complaints.CreateComplaint;

public record struct CreateComplaintCommand(
    Guid ParentId,
    Guid FamilyId,
    Guid? DocumentId,
    string Type,
    string Description) : ICommand<Guid>;