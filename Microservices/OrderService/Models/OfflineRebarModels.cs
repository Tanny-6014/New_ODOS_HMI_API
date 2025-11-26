using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    [Table("OESOfflRebar")]
    public class OfflineRebarDBModels
    {
        [Key, Column(Order = 1)]
        public int BackupID { get; set; }
        [Key, Column(Order = 2)]
        public int POID { get; set; }
        [Key, Column(Order = 3)]
        public int BBSID { get; set; }
        [Key, Column(Order = 4)]
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

        public int? A { get; set; }

        public int? B { get; set; }

        public int? C { get; set; }

        public int? D { get; set; }

        public int? E { get; set; }

        public int? F { get; set; }

        public int? G { get; set; }

        public int? H { get; set; }

        public int? I { get; set; }

        public int? J { get; set; }

        public int? K { get; set; }

        public int? L { get; set; }

        public int? M { get; set; }

        public int? N { get; set; }

        public int? O { get; set; }

        public int? P { get; set; }

        public int? Q { get; set; }

        public int? R { get; set; }

        public int? S { get; set; }
        public int? T { get; set; }
        public int? U { get; set; }
        public int? V { get; set; }
        public int? W { get; set; }

        public int? X { get; set; }
        public int? Y { get; set; }
        public int? Z { get; set; }

        public int? A2 { get; set; }

        public int? B2 { get; set; }

        public int? C2 { get; set; }

        public int? D2 { get; set; }

        public int? E2 { get; set; }

        public int? F2 { get; set; }

        public int? G2 { get; set; }

        public int? H2 { get; set; }

        public int? I2 { get; set; }

        public int? J2 { get; set; }

        public int? K2 { get; set; }

        public int? L2 { get; set; }

        public int? M2 { get; set; }

        public int? N2 { get; set; }

        public int? O2 { get; set; }

        public int? P2 { get; set; }

        public int? Q2 { get; set; }

        public int? R2 { get; set; }

        public int? S2 { get; set; }
        public int? T2 { get; set; }
        public int? U2 { get; set; }
        public int? V2 { get; set; }
        public int? W2 { get; set; }

        public int? X2 { get; set; }
        public int? Y2 { get; set; }
        public int? Z2 { get; set; }

        public int? BarLength { get; set; }
        public int? BarLength2 { get; set; }

        public decimal? BarWeight { get; set; }

        public string Remarks { get; set; }

        public byte? shapeTransport { get; set; }
        //null ; 0 -- normal 1 -- Low Bed 2 -- Low Bed wirh police escort

        public int? PinSize { get; set; }

        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateBy { get; set; }
    }

    public class OfflineRebarModels
    {
        [Key, Column(Order = 1)]
        public int BackupID { get; set; }
        [Key, Column(Order = 2)]
        public int POID { get; set; }
        [Key, Column(Order = 3)]
        public int BBSID { get; set; }
        [Key, Column(Order = 4)]
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
    }

}