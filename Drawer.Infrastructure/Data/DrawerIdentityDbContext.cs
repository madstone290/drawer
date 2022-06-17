using Drawer.Domain.Models.Authentication;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Infrastructure.Data
{
    public class DrawerIdentityDbContext : IdentityDbContext<User>
    {
        /// <summary>
        /// AspNetCoreIdentity테이블 식별용 접두사
        /// </summary>
        private const string TABLE_PREFIX = "__Identity__";

        public DrawerIdentityDbContext(DbContextOptions options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var entity in builder.Model.GetEntityTypes())
            {
                var table = entity.GetTableName();
                if (table != null)
                    entity.SetTableName(TABLE_PREFIX + table);
            }
        }
    }
}
