using System.ComponentModel.DataAnnotations.Schema;

namespace ShapeCodeService.Models
{
    public class Shapegroup
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int? SG_IDENT { get;set; }
        public string SG_CODE { get; set; }
        public string SG_DESC { get; set; }
        public string SG_CON_DIM { get; set; }
        public string SG_CON_STCUR { get; set; }
        public string SG_CON_BEND { get; set; }
        public string SG_CON_COU { get; set; }
        public bool SG_ARCHIVED { get; set; }

    }
}
