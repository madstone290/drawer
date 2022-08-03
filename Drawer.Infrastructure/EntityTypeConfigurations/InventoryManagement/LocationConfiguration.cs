using Drawer.Domain.Models.InventoryManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Infrastructure.EntityTypeConfigurations.InventoryManagement
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.Property(x => x.UpperLocationId).IsRequired(false);

            builder.HasOne(x => x.UpperLocation)
              .WithMany()
              .HasForeignKey(x => x.UpperLocationId)
              .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
