using AnchorModeling.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnchorModeling.Common.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Temporary]
        public string Name { get; set; }

        [Temporary]
        public string Email { get; set; }       

        public DateTime BirthDate { get; set; }

        [Temporary]
        [Column(TypeName = "numeric(15, 3)")]
        public decimal Height { get; set; }

        //FK section
        public int? BookId { get; set; }

        public int? BookId2 { get; set; }

        [ForeignKey(nameof(BookId))]
        public Book Book1 { get; set; }

        [Temporary]
        [ForeignKey(nameof(BookId2))]
        public Book Book2 { get; set; }
    }
}