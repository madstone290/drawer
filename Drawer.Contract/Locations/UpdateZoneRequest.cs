﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Contract.Locations
{
    public record UpdateZoneRequest(string Name, long? ZoneTypeId);
}