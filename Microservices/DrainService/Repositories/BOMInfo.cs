using System.Diagnostics;
using System;

namespace DrainService.Repositories
{
    public class BOMInfo
    {
        [DebuggerNonUserCode]
        //public BOMInfo();

        public string BOMPDFPath { get; set; }
        public string BOMType { get; set; }
        public int CO1 { get; set; }
        public int CO2 { get; set; }
        public string COH1 { get; set; }
        public string COH2 { get; set; }
        public int DetailingBOMDetailId { get; set; }
        public string FormName { get; set; }
        public string Left { get; set; }
        public string LineNo { get; set; }
        public string LoginId { get; set; }
        public int MO1 { get; set; }
        public int MO2 { get; set; }
        public string MOH1 { get; set; }
        public string MOH2 { get; set; }
        public string NoOfPitch { get; set; }
        public string OverHang { get; set; }
        public string ParametersForCWSpace { get; set; }
        public string ParametersForMWSpace { get; set; }
        public int ProductMarkingId { get; set; }
        public string RawMaterial { get; set; }
        public string RawVar { get; set; }
        public string Rep { get; set; }
        public string RepFrom { get; set; }
        public string RepTo { get; set; }
        public string Right { get; set; }
        public string Shape { get; set; }
        public int ShapeId { get; set; }
        public string StartPosition { get; set; }
        public string StructureElement { get; set; }
        public int StructureMarkingId { get; set; }
        public string TotalSpace { get; set; }
        public bool TwinWire { get; set; }
        public int UserId { get; set; }
        public string WireDiameter { get; set; }
        public string WireForCW { get; set; }
        public string WireForMW { get; set; }
        public string WireLength { get; set; }
        public string WireSpace { get; set; }
        public string WireSpec { get; set; }
        public string WireType { get; set; }
    }
}
