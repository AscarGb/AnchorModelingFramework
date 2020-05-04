using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnchorModeling.Common
{
    public class ForeignKeySet
    {
        public PropertyDefinition PropertyOfEntity { get; set; }
        public PropertyDefinition ForeignKeyProperty { get; set; }
        public PropertyDefinition ForeignKeyIdProperty { get; set; }
    }
}