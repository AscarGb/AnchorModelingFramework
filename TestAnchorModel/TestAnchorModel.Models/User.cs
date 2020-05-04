using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AnchorModeling.Attributes;

namespace TestAnchorModel.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
     
        public string Name { get; set; }

        [Temporary]
        public string Email { get; set; }

        public DateTime BirthDate { get; set; }

        [Temporary]
        [Column(TypeName = "numeric(15, 3)")]
        public decimal Height { get; set; }

        //FK section
        public int? BookId { get; set; }

        public int? CarId { get; set; }

        [Temporary]
        [ForeignKey(nameof(BookId))]
        public Book Book { get; set; }

        [Temporary]
        [ForeignKey(nameof(CarId))]
        public Car Car { get; set; }
    }
}
