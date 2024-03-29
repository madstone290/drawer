﻿using Drawer.Application.Config;
using Drawer.Shared.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Exceptions
{
    public class EntityNotFoundException : AppException
    {
        public EntityNotFoundException(string message, object? tag = null) : base(message, tag, ErrorCodes.ENTITY_NOT_FOUND)
        {
        }
    }
    

    /// <summary>
    /// 엔티티를 찾을 수 없는 경우 발생하는 예외
    /// </summary>
    public class EntityNotFoundException<TEntity> : AppException
    {




        public EntityNotFoundException(object id) : base(CreateMessage(id))
        {
        }
        static string CreateMessage(object id)
        {
            return $"{typeof(TEntity).Name}.{id} 엔티티를 찾을 수 없습니다";
        }
    }
}
