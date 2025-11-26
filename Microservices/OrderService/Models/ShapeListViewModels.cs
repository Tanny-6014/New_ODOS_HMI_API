using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderService.Models
{
    [Table("OESShapeReference")]
    public class ShapeListViewModels
    {
        [Key]
        public string shapeCode { get; set; }

        public string shapeCategory { get; set; }

        public string shapeParameters { get; set; }

        public string shapeLengthFormula { get; set; }

        public string shapeParaValidator { get; set; }

        public string shapeTransportValidator { get; set; }

        public string shapeStatus { get; set; }

        public string CouplerParameters { get; set; }

        public DateTime?  shapeCreated { get; set; }
    }

    public class ShapeConfViewModels
    {
        [Key]
        public string shapeCode { get; set; }
        public string shapeCategory { get; set; }
        public string shapeParameters { get; set; }
        public string shapeParaX { get; set; }
        public string shapeParaY { get; set; }
        public string shapeLengthFormula { get; set; }
        public string shapeParaValidator { get; set; }
        public string shapeTransportValidator { get; set; }
        public string shapeParType { get; set; }
        public string shapeDefaultValue { get; set; }
        public string shapeHeightCheck { get; set; }
        public string shapeAutoCalcFormula1 { get; set; }
        public string shapeAutoCalcFormula2 { get; set; }
        public string shapeAutoCalcFormula3 { get; set; }
        public byte[] shapeImage { get; set; }
        public DateTime? shapeCreated { get; set; }
    }
}