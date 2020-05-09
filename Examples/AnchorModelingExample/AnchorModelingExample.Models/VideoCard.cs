using System.ComponentModel.DataAnnotations;

namespace AnchorModelingExample.Models
{
    public class VideoCard
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Bytes { get; set; }
    }
}
