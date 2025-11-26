using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OrderService.Models
{
    [Table("OESShapeReference")]
    public class ShapeModels
    {
        [Required(ErrorMessage = "Shape Code Required") ]
        [StringLength(3, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Key]
        [Display(Name = "Shape Code")]
        public string shapeCode { get; set; }

        [Required(ErrorMessage = "Shape Category Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string shapeCategory { get; set; }

        [Required(ErrorMessage = "Parameter Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string shapeParameters { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string shapeLengthFormula { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string shapeParaValidator { get; set; }
        //Validator format: "0,1,0,1,1,2,3,4;8A"
        // 0 -- No dalidation
        // 1 -- Bending Minimum Length valisation
        // 2 -- Hook minimum length validation
        // 3 -- Hook minimum height validation
        // 4 -- Height validation
        // 5 -- Angle validation 1-89
        // 6 -- Angle validaion 91-179
        // 7 -- Angle validation 1-179
        // 8 -- Segment Length Less another parameter 8A
        // 9 -- 9DB1:Length to calculate angle D using hypotenuse B and 1-asin;2-asin+90;3-acos;4-acos+90;5,6,7,8-dia;9-great than 90 bending<br>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string shapeTransportValidator { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public byte[] shapeImage { get; set; }

        public string shapeStatus { get; set; }
        public string CouplerParameters { get; set; }
        public DateTime? shapeCreated { get; set; }
    }
}