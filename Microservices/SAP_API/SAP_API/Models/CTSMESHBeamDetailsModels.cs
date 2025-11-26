using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAP_API.Modelss
{
    [Table("OESCTSMESHBeamDetails")]
    public class CTSMESHBeamDetailsModels
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
        public int? MeshWidth { get; set; }
        public int? MeshDepth { get; set; }
        public int? MeshSlope { get; set; }
        public string MeshProduct { get; set; }
        public string MeshShapeCode { get; set; }
        public int? MeshTotalLinks { get; set; }
        public int? MeshSpan { get; set; }
        public int? MeshMemberQty { get; set; }
        public bool MeshCapping { get; set; }
        public string MeshCPProduct { get; set; }
        public int? A { get; set; }
        public int? B { get; set; }
        public int? C { get; set; }
        public int? D { get; set; }
        public int? E { get; set; }
        public int? P { get; set; }
        public int? Q { get; set; }
        public int? HOOK { get; set; }
        public int? LEG { get; set; }
        public decimal? MeshTotalWT { get; set; }
        public string Remarks { get; set; }
        public int? MWLength { get; set; }
        public string MWBOM { get; set; }
        public string CWBOM { get; set; }
        public string SplitNotes { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; }
    }

    public class CTSShapeBeamDetailsModels
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
        public int? MeshWidth { get; set; }
        public int? MeshDepth { get; set; }
        public int? MeshSlope { get; set; }
        public string MeshProduct { get; set; }
        public string MeshShapeCode { get; set; }
        public int? MeshTotalLinks { get; set; }
        public int? MeshSpan { get; set; }
        public int? MeshMemberQty { get; set; }
        public bool MeshCapping { get; set; }
        public string MeshCPProduct { get; set; }
        public int? A { get; set; }
        public int? B { get; set; }
        public int? C { get; set; }
        public int? D { get; set; }
        public int? E { get; set; }
        public int? P { get; set; }
        public int? Q { get; set; }
        public int? HOOK { get; set; }
        public int? LEG { get; set; }
        public decimal? MeshTotalWT { get; set; }
        public string Remarks { get; set; }
        public int? MWLength { get; set; }
        public string MWBOM { get; set; }
        public string CWBOM { get; set; }
        public string SplitNotes { get; set; }

        public string MeshShapeParameters { get; set; }
        public string MeshEditParameters { get; set; }
        public string MeshShapeParamTypes { get; set; }
        public string MeshShapeMinValues { get; set; }
        public string MeshShapeMaxValues { get; set; }
        public string MeshShapeWireTypes { get; set; }

        public decimal? ProdMWDia { get; set; }
        public int? ProdMWSpacing { get; set; }
        public decimal? ProdCWDia { get; set; }
        public int? ProdCWSpacing { get; set; }
        public decimal? ProdMass { get; set; }
        public int? ProdMinFactor { get; set; }
        public int? ProdMaxFactor { get; set; }
        public string ProdTwinInd { get; set; }
    }
}