using AnchorModeling.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WikiConvensionExampleModels
{
    public class AnotherEntity
    {
        [Key]
        public int Id { get; set; }

        [Temporary]
        [Column(TypeName = "numeric(15, 3)")]
        public decimal HistoricalProperty { get; set; }
    }
}