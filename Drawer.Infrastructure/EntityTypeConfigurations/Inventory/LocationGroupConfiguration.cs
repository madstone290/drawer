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
    public class LocationGropuConfiguration : IEntityTypeConfiguration<LocationGroup>
    {
        public void Configure(EntityTypeBuilder<LocationGroup> builder)
        {
            builder.HasIndex(x => new
            {
                x.Name,
                x.CompanyId,
            }).IsUnique();

            builder.Property(x => x.ParentGroupId).IsRequired(false);
            builder.HasOne(x => x.ParentGroup)
              .WithMany()
              .HasForeignKey(x => x.ParentGroupId)
              .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.RootGroupIdDBValue).IsRequired(false);
            builder.HasOne<LocationGroup>()
                .WithMany()
                .HasForeignKey(x => x.RootGroupIdDBValue)
                .OnDelete(DeleteBehavior.Restrict);
                
        }
    }
}
