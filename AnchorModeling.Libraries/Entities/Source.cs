using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnchorModeling.Entities
{
    public class Source
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
