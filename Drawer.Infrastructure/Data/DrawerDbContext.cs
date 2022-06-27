using Drawer.Domain.Models;
using Drawer.Domain.Models.Authentication;
using Drawer.Domain.Models.Locations;
using Drawer.Domain.Models.Organization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Infrastructure.Data
{
    public class DrawerDbContext : IdentityDbContext<User>
    {
        public DrawerDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; } = default!;

        public DbSet<Company> Companies { get; set; } = default!;
        public DbSet<CompanyMember> CompanyMembers { get; set; } = default!;

        public DbSet<WorkPlace> WorkPlaces { get; set; } = default!;
        public DbSet<WorkPlaceZone> WorkPlaceZones { get; set; } = default!;
        public DbSet<WorkPlaceZoneType> WorkPlaceZoneTypes { get; set; } = default!;
        public DbSet<Position> Positions { get; set; } = default!;



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(DrawerDbContext).Assembly);
        }

        public override int SaveChanges()
        {
            AuditTrail();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            AuditTrail();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            AuditTrail();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AuditTrail();
            return base.SaveChangesAsync(cancellationToken);
        }

        void AuditTrail()
        {
            var addedAuditedEntities = ChangeTracker.Entries<IAuditable>()
                .Where(p => p.State == EntityState.Added)
                .Select(p => p.Entity);

            var modifiedAuditedEntities = ChangeTracker.Entries<IAuditable>()
              .Where(p => p.State == EntityState.Modified)
              .Select(p => p.Entity);


            var now = DateTime.UtcNow;
            foreach (var added in addedAuditedEntities)
            {
                added.Created = now;
                added.CreatedBy = "test";
            }

            foreach (var modified in modifiedAuditedEntities)
            {
                modified.LastModified = now;
                modified.LastModifiedBy = "test";
            }
        }
    }
}
