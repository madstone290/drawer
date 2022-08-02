using Drawer.Domain.Models;
using Drawer.Domain.Models.Authentication;
using Drawer.Domain.Models.Items;
using Drawer.Domain.Models.Locations;
using Drawer.Domain.Models.Organization;
using Drawer.Domain.Models.UserInformation;
using Drawer.Infrastructure.Services.Organization;
using Drawer.Infrastructure.Services.UserInformation;
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
    public class DrawerDbContext : IdentityDbContext
    {
        private readonly ICompanyIdProvider _companyIdProvider;
        private readonly IUserIdProvider _userIdProvider;

        public DrawerDbContext(DbContextOptions options,
            ICompanyIdProvider companyIdProvider,
            IUserIdProvider userIdProvider) : base(options)
        {
            _companyIdProvider = companyIdProvider;
            _userIdProvider = userIdProvider;
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; } = default!;

        public DbSet<UserInfo> UserInfos { get; set; } = default!;

        public DbSet<Company> Companies { get; set; } = default!;
        public DbSet<CompanyMember> CompanyMembers { get; set; } = default!;

        public DbSet<Workplace> WorkPlaces { get; set; } = default!;
        public DbSet<Zone> Zones { get; set; } = default!;
        public DbSet<Spot> Spots { get; set; } = default!;

        public DbSet<Item> Items { get; set; } = default!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(DrawerDbContext).Assembly);


            builder.AddQueryFilterToAllEntitiesAssignableFrom<ICompanyResource>(e => e.CompanyId == _companyIdProvider.GetCompanyId());
            builder.AddQueryFilterToAllEntitiesAssignableFrom<ISoftDelete>(e => e.IsDeleted == false);
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ApplyCompanyIdToCompanyResource();
            ApplySoftDelete();
            ApplyAuditTrail();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            ApplyCompanyIdToCompanyResource();
            ApplySoftDelete();
            ApplyAuditTrail();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        /// <summary>
        /// 회사자원에 식별자를 적용한다.
        /// </summary>
        void ApplyCompanyIdToCompanyResource()
        {
            var addedEntries = ChangeTracker.Entries<ICompanyResource>()
                .Where(p => p.State == EntityState.Added);

            if (!addedEntries.Any())
                return;

            var companyId = _companyIdProvider.GetCompanyId();
            if (companyId == null)
                throw new Exception("회사 리소스 식별을 위한 회사Id를 찾을 수 없습니다");
            foreach (var entry in addedEntries)
            {
                entry.Entity.CompanyId = companyId;
            }
        }

        /// <summary>
        /// 감사 추적을 적용한다.
        /// </summary>
        void ApplyAuditTrail()
        {
            var addedEntries = ChangeTracker.Entries<IAuditable>()
                .Where(p => p.State == EntityState.Added);

            var modifiedEntries = ChangeTracker.Entries<IAuditable>()
              .Where(p => p.State == EntityState.Modified);


            var now = DateTime.UtcNow;
            foreach (var entry in addedEntries)
            {
                entry.Entity.Created = now;
                entry.Entity.CreatedBy = _userIdProvider.GetUserId()
                    ?? throw new Exception("유효하지 않는 사용자Id입니다");
            }

            foreach (var entry in modifiedEntries)
            {
                entry.Entity.LastModified = now;
                entry.Entity.LastModifiedBy = _userIdProvider.GetUserId()
                    ?? throw new Exception("유효하지 않는 사용자Id입니다");
            }
        }

        /// <summary>
        /// 소프트 삭제를 적용한다.
        /// </summary>
        void ApplySoftDelete()
        {
            var deleteEntries = ChangeTracker.Entries<ISoftDelete>()
              .Where(p => p.State == EntityState.Deleted);

            foreach (var entry in deleteEntries)
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsDeleted = true;
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
