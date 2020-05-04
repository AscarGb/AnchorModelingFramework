using System;
using System.Collections.Generic;
using System.Text;

namespace AnchorModeling.Attributes
{
    public class AnchorTypeAttribute : Attribute
    {
        public string TypeName;
        public AnchorTypeAttribute(string typeName)
        {
            TypeName = typeName;
        }
    }
}