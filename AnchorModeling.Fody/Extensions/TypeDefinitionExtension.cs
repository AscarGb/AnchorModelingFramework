using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnchorModeling.Extensions
{
    public static class TypeDefinitionExtension
    {
        public static bool IsAssignableFromClass(this TypeDefinition baseType, TypeDefinition checking)
        {
            if (baseType.Name.Equals(checking.Name, StringComparison.Ordinal))
            {
                return true;
            }
            else if (checking.BaseType != null)
            {
                return baseType.IsAssignableFromClass(checking.BaseType.Resolve());
            }

            return false;
        }
    }
}
