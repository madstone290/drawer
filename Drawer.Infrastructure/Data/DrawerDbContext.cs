using Drawer.Domain.Models;
using Drawer.Domain.Models.Authentication;
using Drawer.Domain.Models.Locations;
using Drawer.Domain.Models.Organization;
using Drawer.Infrastructure.Services.Organization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(DrawerDbContext).Assembly);

            // 글로벌 쿼리필터 설정
            var allTypes = typeof(ICompanyResource).Assembly.GetTypes();
            var companyEntities = allTypes
                .Where(type => typeof(ICompanyResource).IsAssignableFrom(type) 
                        && type.IsClass && !type.IsAbstract);

            foreach (var companyEntity in companyEntities)
            {
                var companyId = _companyIdProvider.GetCompanyId() ?? string.Empty;
                var lambda = GenerateCompanyIdQueryFilterLambdaExpression(companyEntity, companyId);
                builder.Entity(companyEntity)
                    .HasQueryFilter(lambda);
            }

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


        private LambdaExpression GenerateSoftDeleteQueryFilterLambdaExpression(Type type)
        {
            //// we should generate:  e => e.IsDeleted == false
            //// or: e => !e.IsDeleted

            //// e =>
            //var parameter = Expression.Parameter(type, "e");

            //// false
            //var falseConstant = Expression.Constant(false);

            //// e.IsDeleted
            //var propertyAccess = Expression.PropertyOrField(parameter, nameof(ICanBeSoftDeleted.IsDeleted));

            //// e.IsDeleted == false
            //var equalExpression = Expression.Equal(propertyAccess, falseConstant);

            //// e => e.IsDeleted == false
            //var lambda = Expression.Lambda(equalExpression, parameter);

            //return lambda;
            return null;
        }

        /// <summary>
        /// 회사ID쿼리필터 람다표현식을 생성한다.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        private LambdaExpression GenerateCompanyIdQueryFilterLambdaExpression(Type type, string companyId)
        {
            // we should generate:  e => e.CompanyId == companyId
   
            // e =>
            var parameter = Expression.Parameter(type, "e");

            // companyId
            var companyIdConstant = Expression.Constant(companyId);

            // e.CompanyId
            var propertyAccess = Expression.PropertyOrField(parameter, nameof(ICompanyResource.CompanyId));

            // e.CompanyId == companyId
            var equalExpression = Expression.Equal(propertyAccess, companyIdConstant);

            // e => e.CompanyId == companyId
            var lambda = Expression.Lambda(equalExpression, parameter);

            return lambda;
        }


    }
}
