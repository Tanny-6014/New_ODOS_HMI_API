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
    public class ShapeCodeListModels
    {
        [Key]
        public string shapeCode { get; set; }
        public string shapeCategory { get; set; }
    }

    public class ShapeImageListModels
    {
        [Key]
        public string shapeCode { get; set; }
        public byte[] shapeImage { get; set; }
    }
}
