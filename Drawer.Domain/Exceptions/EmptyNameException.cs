using Drawer.Domain.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Domain.Exceptions
{
    public class EmptyNameException : DomainException
    {
        public EmptyNameException() : base(Messages.EmptyName)
        {

        }

        public EmptyNameException(string? message) : base(message)
        {
        }
    }
}
