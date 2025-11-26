using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrderService.Dtos
{
    public class VarBarDetailDto
    {
 
            [Key, Column(Order = 1)]
            public string CustomerCode { get; set; }
            [Key, Column(Order = 2)]
            public string ProjectCode { get; set; }
            [Key, Column(Order = 3)]
            public int JobID { get; set; }
            [Key, Column(Order = 4)]
            public int BBSID { get; set; }
            [Key, Column(Order = 5)]
            public int BarID { get; set; }

            public double? BarSort { get; set; }

            public bool? Cancelled { get; set; }

            public bool? BarCAB { get; set; }

            public bool? BarSTD { get; set; }

            public string ElementMark { get; set; }

            public string BarMark { get; set; }

            public string BarType { get; set; }

            public Int16? BarSize { get; set; }

            public int? BarMemberQty { get; set; }

            public int? BarEachQty { get; set; }

            public int? BarTotalQty { get; set; }

            public string BarShapeCode { get; set; }

            public string A { get; set; }

            public string B { get; set; }

            public string C { get; set; }

            public string D { get; set; }

            public string E { get; set; }

            public string F { get; set; }

            public string G { get; set; }

            public string H { get; set; }

            public string I { get; set; }

            public string J { get; set; }

            public string K { get; set; }

            public string L { get; set; }

            public string M { get; set; }

            public string N { get; set; }

            public string O { get; set; }

            public string P { get; set; }

            public string Q { get; set; }

            public string R { get; set; }

            public string S { get; set; }
            public string T { get; set; }
            public string U { get; set; }
            public string V { get; set; }
            public string W { get; set; }

            public string X { get; set; }
            public string Y { get; set; }
            public string Z { get; set; }

            public string BarLength { get; set; }
            public decimal? BarWeight { get; set; }
            public string Remarks { get; set; }

            public string shapeParameters { get; set; }
            public string shapeLengthFormula { get; set; }
            public string shapeParaValidator { get; set; }
            public string shapeTransportValidator { get; set; }
            public string shapeParType { get; set; }
            public string shapeDefaultValue { get; set; }
            public string shapeHeightCheck { get; set; }
            //new added 2022-05-19
            public string shapeAutoCalcFormula1 { get; set; }
            public string shapeAutoCalcFormula2 { get; set; }
            public string shapeAutoCalcFormula3 { get; set; }
            //end of new added 2022-05-19
            public byte? shapeTransport { get; set; }
            //null ; 0 -- normal 1 -- Low Bed 2 -- Low Bed wirh police escort

            public int? PinSize { get; set; }

            public string TeklaGUID { get; set; }
            public string PartGUID { get; set; }
            public DateTime? UpdateDate { get; set; }

            public string UpdateBy { get; set; }
        }
    
}
