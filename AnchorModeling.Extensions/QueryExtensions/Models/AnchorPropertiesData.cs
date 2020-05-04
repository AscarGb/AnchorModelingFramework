using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AnchorModeling.QueryExtensions
{
    public class AnchorPropertiesData
    {       
        public List<AnchorSetData> AnchorProperties { get; set; }
        public DbContext DbContext { get; set; }
    }
}
