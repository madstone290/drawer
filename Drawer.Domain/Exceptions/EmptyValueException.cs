using Drawer.Domain.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Domain.Exceptions
{
    public class EmptyValueException : DomainException
    {
        public EmptyValueException(string argName) : base(CreateMessage(argName))
        {
        }

        private static string? CreateMessage(string argName)
        {
            return $"{argName} is empty";
        }
    }
}
