using AnchorModeling.Attributes;
using System.ComponentModel.DataAnnotations;


namespace TestAnchorModel.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [KnotValue]
        public string Name { get; set; }
        public string Author { get; set; }
    }
}