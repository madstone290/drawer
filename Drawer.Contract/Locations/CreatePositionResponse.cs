﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Contract.Locations
{
    public record CreatePositionResponse(long Id, long ZoneId, string Name);
}
