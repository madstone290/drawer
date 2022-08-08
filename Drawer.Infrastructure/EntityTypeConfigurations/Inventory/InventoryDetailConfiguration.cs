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
    public class InventoryDetailConfiguration : IEntityTypeConfiguration<InventoryItem>
    {
        public void Configure(EntityTypeBuilder<InventoryItem> builder)
        {
            builder.HasOne<Item>()
              .WithMany()
              .HasForeignKey(x => x.ItemId)
              .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Location>()
              .WithMany()
              .HasForeignKey(x => x.LocationId)
              .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
