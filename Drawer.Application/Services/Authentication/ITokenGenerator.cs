using Drawer.Domain.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Authentication
{
    public interface ITokenGenerator
    {
        string GenenateAccessToken(User user);

        string GenerateRefreshToken(User user);
    }
}
