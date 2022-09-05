using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Infrastructure.Data.Audit
{
    public class AuditEvent
    {
        public long Id { get; private set; } 

        public DateTime DateTime { get; private set; }

        /// <summary>
        /// Insert/Update/Delete
        /// </summary>
        public string EventType { get; private set; } = null!;

        public string EntityType { get; private set; } = null!;

        public string EntityAuditId { get; private set; } = null!;

        public string UserId { get; private set; } = null!;

        public string? Payload { get; private set; }

        public static AuditEvent InsertEvent(Type entityType, object entityAuditId, string userId, string? payload = null)
        {
            return new AuditEvent("Insert", entityType.Name, entityAuditId.ToString()!, userId, payload);
        }

        public static AuditEvent UpdateEvent(Type entityType, object entityAuditId, string userId, string? payload = null)
        {
            return new AuditEvent("Update", entityType.Name, entityAuditId.ToString()!, userId, payload);
        }

        public static AuditEvent DeleteEvent(Type entityType, object entityAuditId, string userId, string? payload = null)
        {
            return new AuditEvent("Delete", entityType.Name, entityAuditId.ToString()!, userId, payload);
        }

        private AuditEvent() { }
        public AuditEvent(string eventType, string entityType, string entityAuditId, string userId, string? payload)
        {
            DateTime = DateTime.UtcNow;
            EventType = eventType;
            EntityType = entityType;
            EntityAuditId = entityAuditId;
            UserId = userId;
            Payload = payload;
        }

        
    }
}
