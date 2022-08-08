using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Infrastructure.Utils
{
    public static class ExpressionUtil
    {
        public static Expression<Func<TEntity, bool>> GenericIdEqualExpression<TEntity, TId>(
            Expression<Func<TEntity, TId>> idExpression,TId otherIdValue)
        {
            var memberExpression = (MemberExpression)idExpression.Body;
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var property = Expression.Property(parameter, memberExpression.Member.Name);
            var equal = Expression.Equal(property, Expression.Constant(otherIdValue));
            var lambda = Expression.Lambda<Func<TEntity, bool>>(equal, parameter);
            return lambda;
        }
    }
}
