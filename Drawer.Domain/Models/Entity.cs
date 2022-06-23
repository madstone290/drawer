using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Domain.Models
{
    /// <summary>
    /// 엔티티.
    /// long Type을 Id로 사용한다.
    /// </summary>
    public class Entity : Entity<long>
    {
    }

    /// <summary>
    /// 엔티티
    /// </summary>
    /// <typeparam name="TId">Id타입</typeparam>
    public class Entity<TId>
    {
        public TId Id { get; protected set; } = default!;
    }

}
