﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Contract.Locations
{
    public record CreateWorkPlaceResponse(long Id, string Name, string? Description);
}