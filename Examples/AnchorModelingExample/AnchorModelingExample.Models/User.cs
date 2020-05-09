using AnchorModeling.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnchorModelingExample.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int? HomeComputerId { get; set; }

        [ForeignKey(nameof(HomeComputerId))]
        [Temporary]
        public Computer HomeComputer { get; set; }

        public int? WorkComputerId { get; set; }

        [ForeignKey(nameof(WorkComputerId))]
        [Temporary]
        public Computer WorkComputer { get; set; }
    }
}
