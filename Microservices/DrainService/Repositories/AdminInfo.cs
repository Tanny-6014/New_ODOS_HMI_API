namespace DrainService.Repositories
{
    public class AdminInfo
    {
        public string RoleName { get; set; }
        public bool bitApproveAccess { get; set; }
        public string FormId { get; set; }
        public string ReadAccess { get; set; }
        public string WriteAccess { get; set; }
        public Int32 RoleId { get; set; }
        public int UserId { get; set; }
        public int StructElemId { get; set; }
        public Int32 StatusId { get; set; }
        public string EMailId { get; set; }
        public string USerName { get; set; }

        // -------- Product Bom Master--------'
        public int BomMasterHeaderId { get; set; }
        public int intProductCodeId { get; set; }
        public int ProductBOMId { get; set; }
        public string BomType { get; set; }
        public string MeshType { get; set; }
        public char WireType { get; set; }
        public string LineNo { get; set; }
        public string StartPosition { get; set; }
        public string NoOfPitch { get; set; }


        // ----START:-Core Carpet flag-----
        public bool CoreCage { get; set; }

        // -------END:- Core Cage Flag ----


        public decimal CopDiameter { get; set; }

        public string CouplerType { get; set; }

        public string Coupler { get; set; }

        public string Thread { get; set; }

        public decimal CouplerLength_old { get; set; }

        public decimal CouplerLength_new { get; set; }

        public string DiaPrefix { get; set; }

        public string ShapeGroupDescription { get; set; }

        public string Code { get; set; }

        public string ShapeGroupDescription_New { get; set; }

        public string Code_new { get; set; }

        public int norm { get; set; }

        public int norm_new { get; set; }

        public int us { get; set; }

        public int us_new { get; set; }

        public string Grade { get; set; }

        public string Grade_new { get; set; }

        public int barDes { get; set; }

        public int barDes_new { get; set; }

        public int pinStirrups { get; set; }

        public int pinStirrups_new { get; set; }

        public int pinMainBars { get; set; }

        public int pinMainBars_new { get; set; }

        public int pinHooks { get; set; }

        public int pinHooks_new { get; set; }

        public int smLong { get; set; }

        public int smLong_new { get; set; }

        public int smTrans { get; set; }

        public int smTrans_new { get; set; }

        public string BBS_No { get; set; }

        public string vchShapeCode { get; set; }

        public int WBSGroupId { get; set; }

        public int WBSCollaspeId { get; set; }

        public int WBSEstimationDays { get; set; }

        public int WBSLocationTo { get; set; }

        public int WBSLocationFrom { get; set; }

        public string WBSStoreyTo { get; set; }

        public string DrainDepthWidth { get; set; }

        public string WBSStoreyFrom { get; set; }

        public int WBSUOM { get; set; }

        public int WBSCHDays { get; set; }

        public int WBSSSBDays { get; set; }

        public int WBSSMDays { get; set; }

        public int WBSSPDays { get; set; }

        public int WBSBufferHours { get; set; }

        public int WBSStandByDays { get; set; }

        public int WBSCycleDays { get; set; }

        public int WBSManDays { get; set; }

        public int WBSProductionDays { get; set; }

        public int WBSStructureElement { get; set; }

        public int WBSProductId { get; set; }

        public int SummaryReport { get; set; }

        public string Search { get; set; }

        public string GMName { get; set; }

        public int GroupRevNo { get; set; }

        public string Sequence { get; set; }

        public string CriticalIndicator { get; set; }

        public int WBSTypeId { get; set; }

        public string WBS1 { get; set; }

        public string WBS2 { get; set; }

        public int StructureElementTypeId { get; set; }

        public int MeshLapId { get; set; }

        public int OddCO2 { get; set; }

        public int OddCO1 { get; set; }


        public int OddMO2 { get; set; }

        public int OddMO1 { get; set; }

        public int EvenCO2 { get; set; }

        public int EvenCO1 { get; set; }

        public int EvenMO2 { get; set; }

        public int EvenMO1 { get; set; }

        public string consignmentType { get; set; }

        public DateTime datCreatedUpdatedDate { get; set; }

        public int ParamOHProduct { get; set; }

        public int ParamOHId { get; set; }

        public string strGroupMarkId { get; set; }

        public string strWBSElementID { get; set; }

        public int ProductMarkingId { get; set; }

        public int PostHeaderId { get; set; }


        public string strSapMaterial { get; set; }

        public string strMWSpace { get; set; }

        public string strCWSpace { get; set; }

        public string ConversionFactor { get; set; }

        public int MWLap { get; set; }

        public int CWLap { get; set; }

        public int ShapeTransHeaderId { get; set; }

        public int ShapeId { get; set; }

        public string ShapeDescription { get; set; }

        public string ParamValues { get; set; }

        public string AttributeDesc { get; set; }

        public string AttributeValue { get; set; }

        public int StdCCLProductID { get; set; }

        public int ReftParamSetNumber { get; set; }

        public int Leg { get; set; }

        public int Hook { get; set; }

        public int ShapeDetailsId { get; set; }

        public string StandardType { get; set; }

        public string StandardCode { get; set; }

        public string CopyFrom { get; set; }

        public byte OHIndicator { get; set; }


        public byte StructureMarkingLevel { get; set; }

        public int ParamCageId { get; set; }

        public string ParameterType { get; set; }

        public string CLMaterial { get; set; }

        public string CLStandard { get; set; }

        // Added by Siddhi for MESH Shape code creation 
        public string TCXPath { get; set; }
        // End
        public string CPStatus { get; set; }

        public int Diameter { get; set; }

        public int CPProductId { get; set; }

        public int Hook1 { get; set; }

        public int Hook2 { get; set; }

        public int Leg1 { get; set; }

        public int Length { get; set; }

        public int Mo1 { get; set; }

        public int Mo2 { get; set; }

        public int Co1 { get; set; }

        public int Co2 { get; set; }

        public int DestCreatedUId { get; set; }

        public int DestStatusId { get; set; }

        public string DestRemarks { get; set; }

        public int DestParamSetNumber { get; set; }

        public string DestGroupMarkingName { get; set; }

        public int DestProductTypeId { get; set; }

        public int DestStructureElementTypeId { get; set; }

        public int DestProjectId { get; set; }

        public int DestGroupRevNo { get; set; }

        public string SourceStructureMarking { get; set; }

        public string SourceGroupMarking { get; set; }

        public int GroupMarkingId { get; set; }

        public int CopyStructureElement { get; set; }

        public int ParameterSetNo { get; set; }

        public int ParameterSet { get; set; }

        public string StandardCP { get; set; }

        public int Gap2 { get; set; }

        public int Gap1 { get; set; }

        public int RightCover { get; set; }

        public int LeftCover { get; set; }

        public int BottomCover { get; set; }

        public int TopCover { get; set; }

        public int TransportModeId { get; set; }

        public string CustomerName { get; set; }

        public string StructureElement { get; set; }

        public string ProductTypeL2 { get; set; }

        public bool boolRawMaterial { get; set; }

        public string StartDt { get; set; }

        public string EndDt { get; set; }

        public int ContractId { get; set; }

        public string ContractNo { get; set; }

        public string ContractDesc { get; set; }

        public int Customer { get; set; }

        public string SapContractNo { get; set; }

        //public Hashtable hashTwinIndicator { get; set; }

        //public Hashtable hashStaggeredIndicator { get; set; }

        public string OH1 { get; set; }

        public string OH2 { get; set; }

        public string FormName { get; set; }

        public string WireSpace { get; set; }

        public string WireLength { get; set; }

        public string WireDiameter { get; set; }

        public string RawMaterial { get; set; }

        public string RawVar { get; set; }

        public string WireSpec { get; set; }

        public string RepFrom { get; set; }

        public string RepTo { get; set; }

        public string Rep { get; set; }

        public bool TwinWire { get; set; }

        public string Remarks { get; set; }

        public string ElementId { get; set; }

        public string ElementType { get; set; }

        public string ElementDesc { get; set; }

        //public Hashtable hashUser { get; set; }

        public string GenerateBOMStatus { get; set; }

        public string ProductCodeId { get; set; }

        public string ProductCode { get; set; }

        public string ProductDesc { get; set; }

        public int ProductType { get; set; }

        public string MWProduct { get; set; }

        public string CWProduct { get; set; }

        public string MWMaterialType { get; set; }

        public string CWMaterialType { get; set; }

        public string MWDiameter { get; set; }

        public string CWDiameter { get; set; }

        public string MWMaterialAbbr { get; set; }

        public string CWMaterialAbbr { get; set; }

        public int MWSpace { get; set; }

        public int CWSpace { get; set; }

        public string MWGrade { get; set; }

        public string CWGrade { get; set; }

        public string MWLength { get; set; }

        public string CWLength { get; set; }

        public string MWMaxBendLen { get; set; }

        public string CWMaxBendLen { get; set; }

        public string MWWeightRun { get; set; }

        public string CWWeightRun { get; set; }

        public string WeightSqm { get; set; }

        public string TwinInd { get; set; }

        public bool StaggeredInd { get; set; }

        public bool BendInd { get; set; }

        public string BomIndicator { get; set; }

        public int MinLink { get; set; }

        public int MaxLink { get; set; }

        public int SapMaterial { get; set; }

        public string LoginId { get; set; }

        public string StatusCode { get; set; }

        public string Status { get; set; }

        public string StatusDesc { get; set; }

        // 'Consignment Master
        public string strConsignmentType { get; set; }

        public string strConsignmentDesc { get; set; }

        public int intConsignmentId { get; set; }

        // 'cab product code master
        public string strGradeType { get; set; }

        public string strCouplerType { get; set; }

        public byte bitCouplerIndicator { get; set; }

        public int intFGSAPMaterialID { get; set; }

        public int intRMSAPMaterialID { get; set; }

        public int intCabProductCodeID { get; set; }

        // 'Project Master
        public int ProjectId { get; set; }

        public string ProjectCode { get; set; }

        public string ProjectAbbr { get; set; }

        public string ProjectName { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Address3 { get; set; }

        public int ProjectTypeId { get; set; }

        public int MarketSectorId { get; set; }

        public string ContactPerson { get; set; }

        public string Contact { get; set; }

        public string DrawingReceived { get; set; }

        public int SalesIncharge1 { get; set; }

        public int SalesIncharge2 { get; set; }

        public int SalesIncharge3 { get; set; }

        public int ProjectIncharge1 { get; set; }

        public int ProjectIncharge2 { get; set; }

        public int ProjectIncharge3 { get; set; }

        public string PlanDeliveryDate { get; set; }

        public string StartDate { get; set; }

        public string ExpiryDate { get; set; }

        public int SAPProjectId { get; set; }


        public string SAPProjectName { get; set; }

        public int SAPTransportId { get; set; }

        //public Hashtable hashProject { get; set; }

        //public Hashtable hashProject2 { get; set; }

        public int CustomerId { get; set; }

        public string SAPMaterialCode { get; set; }

        public int MaterialCodeId { get; set; }

        public bool Accessories { get; set; }

        public bool RM { get; set; }

        public bool Stock { get; set; }

        public bool FG { get; set; }

        public string SAPWeight { get; set; }

        public string SAPLength { get; set; }

        public string SAPWidth { get; set; }

        public string SAPHeight { get; set; }


        // -----------Drain----------- 

        public int DrainLapId { get; set; }

        public int OuterCover { get; set; }

        public int InnerCover { get; set; }

        public int Lap { get; set; }

        public int Type { get; set; }


        public int Width { get; set; }


        public int Adjust { get; set; }

        public int Channel { get; set; }

        public int SlabThick { get; set; }

        public int MaxDepth1 { get; set; }

        public int MaxDepth2 { get; set; }

        public int MaxDepth3 { get; set; }

        public int MaxDepth4 { get; set; }

        public int MaxDepth5 { get; set; }

        public int Layer { get; set; }

        public int Qty { get; set; }

        public string LeftWallThick { get; set; }

        public string LeftWallFactor { get; set; }

        public string RightWallThick { get; set; }

        public string RightWallFactor { get; set; }

        public string BaseThick { get; set; }

        public int LayerId { get; set; }

        public int WMId { get; set; }

        public int DepthId { get; set; }

        public int ParamAValue { get; set; }

        public byte DrainDetail { get; set; }

        public int DetailId { get; set; }

        public string PrdType { get; set; }

        public string PrdTypeDesc { get; set; }

        public int PrdTypeId { get; set; }

        public string Description { get; set; }

        public byte Confirm { get; set; }
        public char ParamName { get; set; }
        public char AngleType { get; set; }
        public int ParamSeq { get; set; }
        public string MWShape { get; set; }

        public string CWShape { get; set; }

        public int AngleDir { get; set; }

        public int BendSeq1 { get; set; }

        public int BendSeq2 { get; set; }

        public int MinLen { get; set; }

        public int MaxLen { get; set; }

        public int ConstValue { get; set; }

        /// Shape header
        public string MeshGroup { get; set; }

        public string BendingGroup { get; set; }

        public string MWBendingGroup { get; set; }

        public string CWBendingGroup { get; set; }

        public string Image { get; set; }

        public string ImagePath { get; set; }

        public int NoOfSegments { get; set; }

        public int NoOfParameters { get; set; }

        public int NoOfBends { get; set; }

        public int NoOfCuts { get; set; }

        public string NoOfRoll { get; set; }

        public string ShapeType { get; set; }

        public string BendPosition { get; set; }

        public string CheckLength { get; set; }

        public string MWTemplate { get; set; }

        public string CWTemplate { get; set; }

        public bool BendType { get; set; }

        public string MOCO { get; set; }

        public bool BendIndicator { get; set; }

        public bool BendSeqInd { get; set; }

        public bool CreepMO1 { get; set; }

        public bool CreepCO1 { get; set; }

        public bool VerifyInd { get; set; }

        public int MWShapeId { get; set; }

        public int CWShapeId { get; set; }

        public int CarrierWireId { get; set; }

        public decimal MWMinlen { get; set; }

        public decimal MWMaxLen { get; set; }

        public int MWStep { get; set; }

        public decimal CWMinLen { get; set; }

        public decimal CWMaxLen { get; set; }

        public int CWStep { get; set; }

        public int PrefMeshId { get; set; }

        public int DimensionId { get; set; }

        public string DimensionDesc { get; set; }

        public decimal MinSize { get; set; }

        public decimal MaxSize { get; set; }

        // ''' Carrier Wire Master
        public int intCAWSpace { get; set; }

        // start carrier wire master
        public string MinLength { get; set; }

        public string MaxLength { get; set; }

        public int PrdTypeL2Id { get; set; }

        public int RoundId { get; set; }

        public int CAWSequence { get; set; }

        public int StructureElementId { get; set; }

        public string CAWSpace { get; set; }

        public string MinDia { get; set; }

        public string MaxDia { get; set; }

        public int RoundLength { get; set; }

        // end carrier wire master

        public int WireSpecId { get; set; }

        public string WireSpecDesc { get; set; }

        public string fromDateProperty { get; set; }

        public string toDateProperty { get; set; }

        public string userNameList { get; set; }

        public string TonnageReportType { get; set; }

    }
}
