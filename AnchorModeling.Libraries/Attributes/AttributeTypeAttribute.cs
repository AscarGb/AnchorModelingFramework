using System;
using System.Collections.Generic;
using System.Text;

namespace AnchorModeling.Attributes
{
    public class AttributeTypeAttribute : Attribute
    {
        public string TypeName;
        public AttributeTypeAttribute(string typeName)
        {
            TypeName = typeName;
        }
    }
}