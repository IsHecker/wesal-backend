using Microsoft.EntityFrameworkCore;
using Wesal.Domain.Entities.Alimonies;
using Wesal.Domain.Entities.Children;
using Wesal.Domain.Entities.CourtCases;
using Wesal.Domain.Entities.CourtStaffs;
using Wesal.Domain.Entities.Custodies;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Entities.Notifications;
using Wesal.Domain.Entities.ObligationAlerts;
using Wesal.Domain.Entities.Parents;
using Wesal.Domain.Entities.PaymentDues;
using Wesal.Domain.Entities.Payments;
using Wesal.Domain.Entities.Schools;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Entities.VisitationLocations;
using Wesal.Domain.Entities.Visitations;
using Wesal.Domain.Entities.VisitationSchedules;

namespace Wesal.Application.Abstractions.Data;

public interface IWesalDbContext
{
    DbSet<User> Users { get; init; }
    DbSet<FamilyCourt> FamilyCourts { get; init; }
    DbSet<CourtStaff> CourtStaffs { get; init; }
    DbSet<Parent> Parents { get; init; }
    DbSet<School> Schools { get; init; }
    DbSet<Family> Families { get; init; }
    DbSet<Child> Children { get; init; }
    DbSet<CourtCase> CourtCases { get; init; }
    DbSet<Custody> Custodies { get; init; }
    DbSet<Visitation> Visitations { get; init; }
    DbSet<VisitationSchedule> VisitationSchedules { get; init; }
    DbSet<VisitationLocation> VisitationLocations { get; init; }
    DbSet<Alimony> Alimonies { get; init; }
    DbSet<PaymentDue> PaymentsDue { get; init; }
    DbSet<Payment> Payments { get; init; }
    DbSet<ObligationAlert> ObligationAlerts { get; init; }
    DbSet<Notification> Notifications { get; init; }
}