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
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
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
        }
    }
}
