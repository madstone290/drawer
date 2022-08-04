using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Infrastructure.Data.Audit
{
    public class AuditEvent
    {
        public Guid Id { get; private set; } 

        public DateTime DateTime { get; private set; }

        /// <summary>
        /// Insert/Update/Delete
        /// </summary>
        public string EventType { get; private set; }

        public string EntityType { get; private set; } 

        public string EntityAuditId { get; private set; }

        public string UserId { get; private set; }

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
            Id = Guid.NewGuid();
            DateTime = DateTime.UtcNow;
            EventType = eventType;
            EntityType = entityType;
            EntityAuditId = entityAuditId;
            UserId = userId;
            Payload = payload;
        }

        
    }
}
