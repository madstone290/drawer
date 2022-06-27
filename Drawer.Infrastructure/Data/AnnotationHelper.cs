using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Infrastructure.Data
{
    public class AnnotationHelper
    { 
        public static string PostgresSqlTableName<T>(DbSet<T> dbSet) where T : class
        {
            var schema = dbSet.EntityType.FindAnnotation("Relational:Schema")?.Value
                ?? "public";

            var tableName = dbSet.EntityType.GetAnnotation("Relational:TableName").Value!.ToString();

            return $"{schema}.\"{tableName}\"";
        }
    }
}
