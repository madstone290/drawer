﻿using Drawer.Domain.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Authentication.Repos
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        Task<IList<RefreshToken>> FindByUserIdAsync(string userId);
    }
}
