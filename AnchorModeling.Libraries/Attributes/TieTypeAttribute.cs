using System;
using System.Collections.Generic;
using System.Text;

namespace AnchorModeling.Attributes
{
    public class TieTypeAttribute : Attribute
    {
        public string TypeName;
        public TieTypeAttribute(string typeName)
        {
            TypeName = typeName;
        }
    }
}