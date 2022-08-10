using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Authentication.CommandModels
{
    public class RefreshCommandModel
    {
        public RefreshCommandModel() { }
        public RefreshCommandModel(string email, string refreshToken)
        {
            Email = email;
            RefreshToken = refreshToken;
        }

        public string Email { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
    }
}
