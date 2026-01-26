using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Data;
using Wesal.Domain.Entities.Alimonies;
using Wesal.Domain.Entities.Children;
using Wesal.Domain.Entities.Complaints;
using Wesal.Domain.Entities.CourtCases;
using Wesal.Domain.Entities.CourtStaffs;
using Wesal.Domain.Entities.Custodies;
using Wesal.Domain.Entities.CustodyRequests;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Entities.Notifications;
using Wesal.Domain.Entities.ObligationAlerts;
using Wesal.Domain.Entities.Parents;
using Wesal.Domain.Entities.PaymentDues;
using Wesal.Domain.Entities.Payments;
using Wesal.Domain.Entities.SchoolReports;
using Wesal.Domain.Entities.Schools;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Entities.VisitationLocations;
using Wesal.Domain.Entities.Visitations;
using Wesal.Domain.Entities.VisitationSchedules;
using Wesal.Domain.Entities.VisitCenterStaffs;
using Wesal.Infrastructure.Extensions;

namespace Wesal.Infrastructure.Database;

internal sealed class WesalDbContext(DbContextOptions<WesalDbContext> options)
   : DbContext(options), IUnitOfWork, IWesalDbContext
{
   public DbSet<User> Users { get; init; }
   public DbSet<FamilyCourt> FamilyCourts { get; init; }
   public DbSet<CourtStaff> CourtStaffs { get; init; }
   public DbSet<VisitCenterStaff> VisitCenterStaffs { get; init; }
   public DbSet<Parent> Parents { get; init; }
   public DbSet<School> Schools { get; init; }
   public DbSet<SchoolReport> SchoolReports { get; init; }
   public DbSet<Family> Families { get; init; }
   public DbSet<Child> Children { get; init; }
   public DbSet<CourtCase> CourtCases { get; init; }
   public DbSet<Custody> Custodies { get; init; }
   public DbSet<CustodyRequest> CustodyRequests { get; init; }
   public DbSet<Visitation> Visitations { get; init; }
   public DbSet<VisitationSchedule> VisitationSchedules { get; init; }
   public DbSet<VisitationLocation> VisitationLocations { get; init; }
   public DbSet<Alimony> Alimonies { get; init; }
   public DbSet<PaymentDue> PaymentsDue { get; init; }
   public DbSet<Payment> Payments { get; init; }
   public DbSet<Complaint> Complaints { get; init; }
   public DbSet<ObligationAlert> ObligationAlerts { get; init; }
   public DbSet<Notification> Notifications { get; init; }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      modelBuilder.StoreAllEnumsAsNames();

      modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
   }
}