using Drawer.Domain.Models.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Infrastructure.EntityTypeConfigurations.Inventory
{
    internal class LayoutConfiguration : IEntityTypeConfiguration<Layout>
    {
        public void Configure(EntityTypeBuilder<Layout> builder)
        {
            builder.HasOne<LocationGroup>()
                .WithMany()
                .HasForeignKey(x => x.LocationGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.OwnsMany(x => x.Items)
                .Ignore(x=> x.ConnectedLocations);
                
        }
    }
}
