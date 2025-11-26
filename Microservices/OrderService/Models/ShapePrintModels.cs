using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderService.Models
{
    [Table("OESShapePrint")]
    public class ShapePrintModels
    {
        [Key]
        public string ShapeCode { get; set; }
        public int TopCrop { get; set; }
        public int BottomCrop { get; set; }
    }
}
