using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    [Table("OESPRCCTSMESHDetails")]
    public class PRCCTSMESHDetailsModels
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
        public int MeshID { get; set; }
        public int MeshSort { get; set; }
        public string MeshMark { get; set; }
        public string MeshProduct { get; set; }
        public int? MeshMainLen { get; set; }
        public int? MeshCrossLen { get; set; }
        public int? MeshMO1 { get; set; }
        public int? MeshMO2 { get; set; }
        public int? MeshCO1 { get; set; }
        public int? MeshCO2 { get; set; }
        public int? MeshMemberQty { get; set; }
        public string MeshShapeCode { get; set; }
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
        public int? HOOK { get; set; }
        public decimal? MeshTotalWT { get; set; }
        public string Remarks { get; set; }
        public string MWBOM { get; set; }
        public string CWBOM { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; }
    }

    public class PRCCTSShapeDetailsModels
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
        public int MeshID { get; set; }
        public int MeshSort { get; set; }
        public string MeshMark { get; set; }
        public string MeshProduct { get; set; }
        public int? MeshMainLen { get; set; }
        public int? MeshCrossLen { get; set; }
        public int? MeshMO1 { get; set; }
        public int? MeshMO2 { get; set; }
        public int? MeshCO1 { get; set; }
        public int? MeshCO2 { get; set; }
        public int? MeshMemberQty { get; set; }
        public string MeshShapeCode { get; set; }
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
        public int? HOOK { get; set; }
        public decimal? MeshTotalWT { get; set; }
        public string Remarks { get; set; }
        public string MWBOM { get; set; }
        public string CWBOM { get; set; }

        public string MeshShapeParameters { get; set; }
        public string MeshEditParameters { get; set; }
        public string MeshShapeParamTypes { get; set; }
        public string MeshShapeMinValues { get; set; }
        public string MeshShapeMaxValues { get; set; }
        public string MeshShapeWireTypes { get; set; }

        public bool MeshCreepMO1 { get; set; }
        public bool MeshCreepCO1 { get; set; }

        public decimal? ProdMWDia { get; set; }
        public int? ProdMWSpacing { get; set; }
        public decimal? ProdCWDia { get; set; }
        public int? ProdCWSpacing { get; set; }
        public decimal? ProdMass { get; set; }
        public string ProdTwinInd { get; set; }
    }
}