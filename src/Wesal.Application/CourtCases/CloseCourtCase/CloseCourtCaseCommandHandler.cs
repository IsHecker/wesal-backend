using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.CourtCases;
using Wesal.Domain.Results;

namespace Wesal.Application.CourtCases.CloseCourtCase;

internal sealed class CloseCourtCaseCommandHandler(
    ICourtCaseRepository courtCaseRepository,
    IVisitationRepository visitationRepository,
    IPaymentDueRepository paymentDueRepository,
    IVisitationScheduleRepository scheduleRepository,
    IAlimonyRepository alimonyRepository)
    : ICommandHandler<CloseCourtCaseCommand>
{
    public async Task<Result> Handle(
        CloseCourtCaseCommand request,
        CancellationToken cancellationToken)
    {
        var courtCase = await courtCaseRepository.GetByIdAsync(request.CourtCaseId, cancellationToken);
        if (courtCase is null)
            return CourtCaseErrors.NotFound(request.CourtCaseId);

        if (courtCase.CourtId != request.CourtId)
            return CourtCaseErrors.Unauthorized;

        var closeResult = courtCase.Close(request.ClosureNotes);
        if (closeResult.IsFailure)
            return closeResult.Error;

        courtCaseRepository.Update(courtCase);

        var alimony = await alimonyRepository.GetByCourtCaseIdAsync(courtCase.Id, cancellationToken);
        var schedule = await scheduleRepository.GetByCourtCaseIdAsync(courtCase.Id, cancellationToken);

        if (alimony is not null)
        {
            alimony.Stop();
            alimonyRepository.Update(alimony);
        }

        if (schedule is not null)
        {
            schedule.Stop();
            scheduleRepository.Update(schedule);
        }

        await visitationRepository.DeleteScheduledByCourtCaseIdAsync(courtCase.Id, cancellationToken);
        await paymentDueRepository.DeleteUnpaidByCourtCaseIdAsync(courtCase.Id, cancellationToken);

        return Result.Success;
    }
}