using System.ComponentModel.DataAnnotations;

namespace AnchorModelingExample.Models
{
    public class SoundCard
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
