using Drawer.Application.Services.Authentication;
using Drawer.Domain.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Infrastructure.Authentication
{
    public class TokenGenerator : ITokenGenerator
    {
        public string GenenateAccessToken(User user)
        {
            return "sdfkjlejh";
        }

        public string GenerateRefreshToken(User user)
        {
            return "sdfkjlejh";
        }
    }
}
