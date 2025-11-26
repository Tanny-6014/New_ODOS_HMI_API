using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OrderService.Models
{
    [Table("OESCTSMESHShape")]
    public class CTSMESHShapeModels
    {
        [Required(ErrorMessage = "Shape Code Required") ]
        [StringLength(3, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Key]
        [Display(Name = "Shape Code")]
        public string MeshShapeCode { get; set; }

        [Required(ErrorMessage = "Shape Category Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MeshShapeCategory { get; set; }

        [Required(ErrorMessage = "Parameter Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MeshShapeParameters { get; set; }
        public string MeshEditParameters { get; set; }

        public string MeshShapeParamTypes { get; set; }
        public string MeshShapeMinValues { get; set; }
        public string MeshShapeMaxValues { get; set; }
        public string MeshShapeWireTypes { get; set; }
        public bool MeshCreepMO1 { get; set; }
        public bool MeshCreepCO1 { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public byte[] MeshShapeImage { get; set; }

        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
    }
}