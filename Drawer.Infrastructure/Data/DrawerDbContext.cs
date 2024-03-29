﻿using Drawer.Application.Services;
using Drawer.Domain.Models;
using Drawer.Domain.Models.Authentication;
using Drawer.Domain.Models.Inventory;
using Drawer.Domain.Models.Organization;
using Drawer.Domain.Models.UserInformation;
using Drawer.Infrastructure.Data.Audit;
using Drawer.Infrastructure.Services.Organization;
using Drawer.Infrastructure.Services.UserInformation;
using MediatR;
using Microsoft.AspNetCore.Identity;
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
        /// <summary>
        /// 회사 리소스필터링을 위한 회사ID를 가져온다
        /// </summary>
        private readonly ICompanyIdProvider _companyIdProvider;
        
        /// <summary>
        /// 데이터 감사를 위한 사용자ID를 가져온다.
        /// </summary>
        private readonly IUserIdProvider _userIdProvider;

        public DrawerDbContext(DbContextOptions options,
            ICompanyIdProvider companyIdProvider,
            IUserIdProvider userIdProvider) : base(options)
        {
            _companyIdProvider = companyIdProvider;
            _userIdProvider = userIdProvider;
        }

        public DbSet<IdentityUser> IdentityUsers => base.Users;

        public DbSet<AuditEvent> AuditEvents { get; set; } = default!;

        public DbSet<RefreshToken> RefreshTokens { get; set; } = default!;

        public new DbSet<User> Users { get; set; } = default!;
        public DbSet<Company> Companies { get; set; } = default!;
        public DbSet<CompanyMember> CompanyMembers { get; set; } = default!;
        public DbSet<CompanyJoinRequest> CompanyJoinRequests { get; set; } = default!;

        public DbSet<Item> Items { get; set; } = default!;
        public DbSet<Location> Locations { get; set; } = default!;
        public DbSet<LocationGroup> LocationGroups { get; set; } = default!;
        public DbSet<InventoryItem> InventoryItems { get; set; } = default!;
        public DbSet<Receipt> Receipts { get; set; } = default!;
        public DbSet<Issue> Issues { get; set; } = default!;
        public DbSet<Layout> Layouts { get; set; } = default!;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(DrawerDbContext).Assembly);


            builder.AddQueryFilterToAllEntitiesAssignableFrom<ICompanyResource>(e => e.CompanyId == _companyIdProvider.GetCompanyId());
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ApplyCompanyIdToCompanyResource();
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
            if (!companyId.HasValue)
                throw new Exception("회사 리소스 식별을 위한 회사Id를 찾을 수 없습니다");
            foreach (var entry in addedEntries)
            {
                entry.Entity.CompanyId = companyId.Value;
            }
        }

        /// <summary>
        /// 감사 추적을 적용한다.
        /// </summary>
        void ApplyAuditTrail()
        {
            var entries = ChangeTracker.Entries<IAuditable>()
                .ToList();

            foreach (var entry in entries)
            {
                string? eventType = null;
                if (entry.State == EntityState.Added)
                    eventType = "Insert";
                else if (entry.State == EntityState.Modified)
                    eventType = "Update";
                else if (entry.State == EntityState.Deleted)
                    eventType = "Delete";

                if (eventType != null)
                {
                    var entity = entry.Entity;
                    var userId = _userIdProvider.GetUserId() ?? throw new Exception("유효하지 않는 사용자Id입니다");
                    var auditEvent = new AuditEvent(eventType, entity.GetType().Name, entity.AuditId.ToString(), userId, null);
                    AuditEvents.Add(auditEvent);
                }
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
