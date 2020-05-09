using AnchorModeling.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace TestAnchorModel.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }

        public DateTime Year { get;set;}

        [KnotValue]
        public string Name { get; set; }      
    }
}