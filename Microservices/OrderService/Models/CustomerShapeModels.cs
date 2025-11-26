using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    [Table("OESCustomShapes")]
    public class CustomerShapeModels
    {
        [Key, Column(Order = 1)]
        [Display(Name = "Customer Code")]
        public string CustomerCode { get; set; }

        [Display(Name = "Project Code")]
        [Key, Column(Order = 2)]
        public string ProjectCode { get; set; }

        [Key, Column(Order = 3)]
        public string shapeCode { get; set; }

        public string shapeCategory { get; set; }

        public string shapeParameters { get; set; }

        public string shapeLengthFormula { get; set; }

        public byte[] shapeImage { get; set; }

        public DateTime? shapeCreated { get; set; }
        public string CreatedBy { get; set; }
        public string ShapeStatus { get; set; }
        public string ReplacedBy { get; set; }
    }
}