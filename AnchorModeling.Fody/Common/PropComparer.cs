using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnchorModeling.Common
{
    internal class PropComparer : IEqualityComparer<PropertyDefinition>
    {
        public bool Equals(PropertyDefinition x, PropertyDefinition y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return x.Name == y.Name;
        }

        public int GetHashCode(PropertyDefinition obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
