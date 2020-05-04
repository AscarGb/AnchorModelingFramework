using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AnchorModeling.Entities
{
   public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public DateTime SysTime { get; set; }
        public string User { get; set; }
        public int SourceId { get; set; }

        [ForeignKey(nameof(SourceId))]
        public Source Source { get; set; }
    }
}
