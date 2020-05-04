using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AnchorModeling.QueryExtensions
{
    public static class AttributeExtensions
    {
        public static string GetFirstAttributeConstructorStringArgument(this PropertyInfo property, Type attrbuteType) =>
               property.GetCustomAttributesData().First(a => a.AttributeType.Equals(attrbuteType)).ConstructorArguments[0].Value.ToString();

    }
}
