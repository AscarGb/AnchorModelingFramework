using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnchorModeling.Models
{
    public class AuditEntry
    {
        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
            Entity = Entry.Entity;
        }

        public readonly object Entity;
        public EntityState EntityState { get; set; }
        public EntityEntry Entry { get; }
        public string TableName { get; set; }
        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();

        //prop name - id prop name (ClassName : ClassNameId(int) )
        public Dictionary<string, string> FKIds { get; } =
            new Dictionary<string, string>();

        public HashSet<string> FKsProps = new HashSet<string>(StringComparer.Ordinal);

        public List<PropertyEntry> TemporaryProperties { get; } = new List<PropertyEntry>();

        public bool HasTemporaryProperties => TemporaryProperties.Any();

        public int PkValue { get; set; }
    }
}
