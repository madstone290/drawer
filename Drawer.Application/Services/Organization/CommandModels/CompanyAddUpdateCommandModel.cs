﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Organization.CommandModels
{
    public class CompanyAddUpdateCommandModel
    {
        public string Name { get; set; } = null!;
        public string? PhoneNumber { get; set; }
    }
}
