using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Data;
using Wesal.Infrastructure.Extensions;

namespace Wesal.Infrastructure.Database;

public sealed class WesalDbContext : DbContext, IUnitOfWork, IWesalDbContext
{
   public WesalDbContext(DbContextOptions<WesalDbContext> options) : base(options) { }


   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      modelBuilder.StoreAllEnumsAsNames();

      modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
   }
}