using System.ComponentModel.DataAnnotations;

namespace AnchorModelingExample.Models
{
    public class MotherBoard
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
