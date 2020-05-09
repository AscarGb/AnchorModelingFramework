using AnchorModeling.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnchorModelingExample.Models
{
    public class Computer
    {
        [Key]
        public int Id { get; set; }

        public int MotherBoardId { get; set; }
        [Temporary]
        [ForeignKey(nameof(MotherBoardId))]
        public MotherBoard MotherBoard { get; set; }

        public int ProcessorId { get; set; }
        [Temporary]
        [ForeignKey(nameof(ProcessorId))]
        public Processor Processor { get; set; }

        public int RAM1Id { get; set; }
        [Temporary]
        [ForeignKey(nameof(RAM1Id))]
        public RAM RAM1 { get; set; }

        public int RAM2Id { get; set; }
        [Temporary]
        [ForeignKey(nameof(RAM2Id))]
        public RAM RAM2 { get; set; }

        public int SoundCardId { get; set; }
        [Temporary]
        [ForeignKey(nameof(SoundCardId))]
        public SoundCard SoundCard { get; set; }

        public int VideoCardId { get; set; }
        [Temporary]
        [ForeignKey(nameof(VideoCardId))]
        public VideoCard VideoCard { get; set; }
    }
}
