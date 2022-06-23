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
        }
    }
}
