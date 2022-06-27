using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Infrastructure.Data
{
    public static class DbContextExtensions
    {
        /// <summary>
        /// PostgresSql에서 truncate를 실행한다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbSet"></param>
        /// <returns></returns>
        public static string Truncate<T>(this DbSet<T> dbSet) where T : class
        {
            string table = AnnotationHelper.PostgresSqlTableName(dbSet);
            string cmd = $"TRUNCATE TABLE {table} CASCADE";

            var context = dbSet.GetService<ICurrentDbContext>().Context;
            context.Database.ExecuteSqlRaw(cmd);
            return cmd;
        }
    }
}
