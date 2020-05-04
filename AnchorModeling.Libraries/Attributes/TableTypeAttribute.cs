using System;
using System.Collections.Generic;
using System.Text;

namespace AnchorModeling.Attributes
{
    public class TableTypeAttribute : Attribute
    {
        public string TypeName;
        public TableTypeAttribute(string typeName)
        {
            TypeName = typeName;
        }
    }
}