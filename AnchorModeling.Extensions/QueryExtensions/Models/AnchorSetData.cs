using System;
using System.Linq;
using System.Reflection;

namespace AnchorModeling.QueryExtensions
{
    public class AnchorSetData : ICloneable
    {
        public bool IsTie { get; set; }
        public bool IsTemporary { get; set; }
        public IQueryable Query { get; set; }
        public Type TableType { get; set; }
        public PropertyInfo AttributeProperty { get; set; }
        public PropertyInfo ContextEntityProperty { get; set; }
        
        public object Clone() => MemberwiseClone();
    }
}