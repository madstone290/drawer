﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Contract.UserInformation
{
    public record UpdatePasswordRequest(string CurrentPassword, string NewPassword);
}
