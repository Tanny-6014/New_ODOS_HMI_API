using System.ComponentModel.DataAnnotations.Schema;

namespace ShapeCodeService.Models
{
    public class Shapesurchage
    {
        public int?     IDENTITYNO          {get; set; }
        public int?     INTSHAPECODE       { get; set; }
        public string?  CHRSHAPECODE       { get; set;}
        public decimal? BARDIA             { get; set; }
        public decimal? INVLEN             { get; set; }
        public string? CHRSURCHARGE        { get; set; }
        public int?     INTSURCHARGECODE   { get; set; }
        public int?    INTCONDITION       { get; set; }
        public string? CHRCONDITION        { get; set; }
        public string?   Updated_By      { get; set; }
        public DateTime Updated_Date     { get; set; }
    }
}
