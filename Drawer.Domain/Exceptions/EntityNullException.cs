using Drawer.Domain.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Domain.Exceptions
{
    public class EntityNullException<TEntity> : DomainException
    {
        public EntityNullException(string argName) : base(CreateMessage(argName))
        {
        }

        static string CreateMessage(string argName)
        {
            return $"{typeof(Type).Name}.{argName} is null";
        }
    }
}
