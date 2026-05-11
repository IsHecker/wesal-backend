using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.SchoolReports;
using Wesal.Domain.Entities.Schools;
using Wesal.Domain.Results;

namespace Wesal.Application.Schools.DeleteSchool;

internal sealed class DeleteSchoolCommandHandler(
    ISchoolRepository schoolRepository,
    IChildRepository childRepository,
    IRepository<SchoolReport> schoolReportRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<DeleteSchoolCommand>
{
    public async Task<Result> Handle(DeleteSchoolCommand request, CancellationToken cancellationToken)
    {
        var school = await schoolRepository.GetByIdAsync(request.SchoolId, cancellationToken);
        if (school is null)
            return SchoolErrors.NotFound(request.SchoolId);

        // Check if referenced by children
        var isChildExist = await childRepository.Query().AnyAsync(c => c.SchoolId == request.SchoolId, cancellationToken);
        if (isChildExist)
            return SchoolErrors.HasReferences;

        // Check if referenced by school reports
        var isReportExist = await schoolReportRepository.Query().AnyAsync(r => r.SchoolId == request.SchoolId, cancellationToken);
        if (isReportExist)
            return SchoolErrors.HasReferences;

        schoolRepository.Delete(school);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}