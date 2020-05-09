using AnchorModeling.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WikiConvensionExampleModels
{
    public class SomeEntity
    {
        [Key]
        public int Id { get; set; }

        [Temporary]
        public string HistoricalProperty { get; set; }

        public string Property { get; set; }

        public int? FKId { get; set; }
        public int? FKId2 { get; set; }

        [Temporary]
        [ForeignKey(nameof(FKId))]
        public AnotherEntity HistoricaAnotherEntityProperty { get; set; }

        [ForeignKey(nameof(FKId2))]
        public AnotherEntity AnotherEntityProperty { get; set; }
    }
}