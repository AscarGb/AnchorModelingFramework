using AnchorModeling.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AnchorModeling.QueryExtensions
{
   public class SharedAnchorModel
    {
        [Column(Names.A_Id)]
        public int AnchorId { get; set; }

        [Column(Names.TransactionIdFKName)]
        public int TransactionId { get; set; }

        [Column(Names.CloseTransactionIdFKName)]
        public int CloseTransactionId { get; set; }

        [Column(Names.When)]
        public DateTime ApplicationTime { get; set; }

        public Transaction Transaction { get; set; }

        public Transaction CloseTransaction { get; set; }
    }
}
