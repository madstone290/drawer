using Drawer.Domain.Models.Locations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Infrastructure.EntityTypeConfigurations.Locations
{
    public class ZoneTypeConfiguration : IEntityTypeConfiguration<ZoneType>
    {
        public void Configure(EntityTypeBuilder<ZoneType> builder)
        {
        }
    }
}
