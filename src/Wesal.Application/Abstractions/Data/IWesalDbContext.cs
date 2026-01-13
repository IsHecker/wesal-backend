using Microsoft.EntityFrameworkCore;
using Wesal.Domain.Entities.Alimonies;
using Wesal.Domain.Entities.AlimonyPaymentDues;
using Wesal.Domain.Entities.Children;
using Wesal.Domain.Entities.CourtCases;
using Wesal.Domain.Entities.Custodies;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.Notifications;
using Wesal.Domain.Entities.Parents;
using Wesal.Domain.Entities.Payments;
using Wesal.Domain.Entities.Visitations;
using Wesal.Domain.Entities.VisitationSchedules;

namespace Wesal.Application.Abstractions.Data;

public interface IWesalDbContext
{
    DbSet<Parent> Parents { get; init; }
    DbSet<Family> Families { get; init; }
    DbSet<Child> Children { get; init; }
    DbSet<CourtCase> CourtCases { get; init; }
    DbSet<Custody> Custodies { get; init; }
    DbSet<Visitation> Visitations { get; init; }
    DbSet<VisitationSchedule> VisitationSchedules { get; init; }
    DbSet<Alimony> Alimonies { get; init; }
    DbSet<AlimonyPaymentDue> AlimonyPaymentDues { get; init; }
    DbSet<Payment> Payments { get; init; }
    DbSet<Notification> Notifications { get; init; }
}