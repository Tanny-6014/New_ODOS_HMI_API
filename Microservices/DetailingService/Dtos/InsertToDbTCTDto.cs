using DetailingService.Repositories;

namespace DetailingService.Dtos
{
    public class InsertToDbTCTDto
    {
        public int SEDetailingID { get; set; }
        public int StructureMarkId { get; set; }
        public int CABProductMarkID { get; set; }
        public int ProductCodeID { get; set; }
        public int Quantity { get; set; }
        public int PinSizeID { get; set; }
        public string Status { get; set; }
        public string DDStatus { get; set; } // Added for CAB Shape Parameter Validation Data Discrepancy CR
        public int UserID { get; set; } // Added for CAB Shape Parameter Validation Data Discrepancy CR #Activity Task
        public string Activity { get; set; }// Added for CAB Shape Parameter Validation Data Discrepancy CR #Activity Task

        public int Diameter { get; set; }
        public string ShapeCode { get; set; }
        public string DescScript { get; set; }
        public string CABProductMarkName { get; set; }

        public List<ShapeParameter> ShapeParametersList { get; set; }
        public List<Accessory> accList { get; set; }

        public CABItem CABProductItem { get; set; }

        //public string CABProductItem { get; set; }
        public int MemberQty { get; set; }
        public int PinSize { get; set; }
        public double InvoiceLength { get; set; }
        public double ProductionLength { get; set; }
        public double InvoiceWeight { get; set; }
        public double ProductionWeight { get; set; }
        public string Grade { get; set; }
        public int ShapeTransHeaderID { get; set; }
        public int GroupMarkId { get; set; }
        public string ShapeGroup { get; set; }
        public int EndCount { get; set; }
        public string ImagePath { get; set; }
        public string CustomerRemarks { get; set; }
        public string PageNumber { get; set; }
        public double EnvLength { get; set; }
        public double EnvWidth { get; set; }
        public double EnvHeight { get; set; }
        public string ShapeImage { get; set; }
        public int NoOfBends { get; set; }
        public int TransportModeId { get; set; }
        public string BVBS { get; set; }
        public string CreatedBy { get; set; }
        public string ProduceIndicator { get; set; }

        //public string ProduceIndicator { get; set; }
        public string BarMark { get; set; }
        public string CommercialDesc { get; set; }

        public string ShapeType { get; set; }
        public Accessory accItem { get; set; }
        //public string accItem { get; set; }

        public bool IsReadOnly { get; set; }
        public int intSEDetailingId { get; set; }
        public string ipAddress { get; set; }

        public string ACCProductNameforCAB { get; set; }


        public string Coupler1 { get; set; }
        public string Coupler2 { get; set; }
        public string Thread1 { get; set; }
        public string Thread2 { get; set; }
        public string Coupler1Type { get; set; }
        public string Coupler1Standard { get; set; }
        public string Thread1Type { get; set; }
        public string Thread1Standard { get; set; }
        public string Coupler2Type { get; set; }
        public string Coupler2Standard { get; set; }
        public string Thread2Type { get; set; }
        public string Thread2Standard { get; set; }

        //Locknut
        public string Locknut1 { get; set; }
        public string Locknut2 { get; set; }

        //BBS No
        public string BBSNo { get; set; }

        //VPN User
        public bool IsVPNUsers { get; set; }

        //New parameter for cube integration
        public string ProduceInd { get; set; }
        public ShapeCode Shape { get; set; }
        public bool IsVariableBar { get; set; }

    }
}
