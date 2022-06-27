using Drawer.Domain.Models;
using Drawer.Domain.Models.Authentication;
using Drawer.Domain.Models.Locations;
using Drawer.Domain.Models.Organization;
using Drawer.Infrastructure.Services.Organization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Infrastructure.Data
{
    public class DrawerDbContext : IdentityDbContext<User>
    {
        private readonly ICompanyIdProvider _companyIdProvider;

        public DrawerDbContext(DbContextOptions options, ICompanyIdProvider companyIdProvider) : base(options)
        {
            _companyIdProvider = companyIdProvider;
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; } = default!;

        public DbSet<Company> Companies { get; set; } = default!;
        public DbSet<CompanyMember> CompanyMembers { get; set; } = default!;

        public DbSet<WorkPlace> WorkPlaces { get; set; } = default!;
        public DbSet<WorkPlaceZone> WorkPlaceZones { get; set; } = default!;
        public DbSet<WorkPlaceZoneType> WorkPlaceZoneTypes { get; set; } = default!;
        public DbSet<Position> Positions { get; set; } = default!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

  

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(DrawerDbContext).Assembly);

            ApplyGlobalFilters<ICompanyResource>(builder, x => x.CompanyId == _companyIdProvider.GetCompanyId());
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

        /// <summary>
        /// 인터페이스에 대한 글로벌 필터를 적용한다 
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="modelBuilder"></param>
        /// <param name="expression"></param>
        static void ApplyGlobalFilters<TInterface>(ModelBuilder modelBuilder, Expression<Func<TInterface, bool>> expression)
        {
            var entities = modelBuilder.Model
                .GetEntityTypes()
                .Where(e => e.ClrType.GetInterface(typeof(TInterface).Name) != null)
                .Select(e => e.ClrType);

            foreach (var entity in entities)
            {
                var newParam = Expression.Parameter(entity);
                var newbody = ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), newParam, expression.Body);
                var expressions = Expression.Lambda(newbody, newParam);

                modelBuilder.Entity(entity).HasQueryFilter(expressions);
            }
        }


       
    }
}
