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
    public class InventoryDetailConfiguration : IEntityTypeConfiguration<InventoryDetail>
    {
        public void Configure(EntityTypeBuilder<InventoryDetail> builder)
        {
            builder.HasOne(x => x.Item)
              .WithMany()
              .HasForeignKey(x => x.ItemId)
              .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Location)
              .WithMany()
              .HasForeignKey(x => x.LocationId)
              .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
