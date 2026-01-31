using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.CourtCases;
using Wesal.Domain.Entities.CourtStaffs;
using Wesal.Domain.Entities.Documents;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Results;

namespace Wesal.Application.CourtCases.CreateCourtCase;

internal sealed class CreateCourtCaseHandler(
    ICourtCaseRepository courtCaseRepository,
    ICourtStaffRepository staffRepository,
    IFamilyRepository familyRepository,
    IRepository<Document> documentRepository)
    : ICommandHandler<CreateCourtCaseCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        CreateCourtCaseCommand request,
        CancellationToken cancellationToken)
    {
        var staff = await staffRepository.GetByIdAsync(request.StaffId, cancellationToken);
        if (staff is null)
            return CourtStaffErrors.NotFound(request.StaffId);

        var family = await familyRepository.GetByIdAsync(request.FamilyId, cancellationToken);
        if (family is null)
            return FamilyErrors.NotFound(request.FamilyId);

        if (await courtCaseRepository.ExistsByCaseNumberAsync(request.CaseNumber, cancellationToken))
            return CourtCaseErrors.CaseNumberAlreadyExists(request.CaseNumber);

        if (request.DocumentId is not null)
        {
            var isExist = await documentRepository.ExistsAsync(request.DocumentId.Value, cancellationToken);
            if (!isExist)
                return DocumentErrors.NotFound(request.DocumentId.Value);
        }

        var courtCase = CourtCase.Create(
            staff.CourtId,
            request.FamilyId,
            request.CaseNumber,
            request.DecisionSummary,
            request.DocumentId);

        await courtCaseRepository.AddAsync(courtCase, cancellationToken);

        return courtCase.Id;
    }
}