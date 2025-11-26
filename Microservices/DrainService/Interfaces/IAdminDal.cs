using DrainService.Dtos;
using DrainService.Repositories;



namespace DrainService.Interfaces
{
    public interface IAdminDal
    {


        #region GM Listing
        //public SqlDataReader GetGMId(AdminInfo obj);
        #endregion


        #region Role Mgmt 
        //// added for ship to party
        //public System.Data.DataSet GetProject_new(AdminInfo obj);
        //public System.Data.DataSet GetProductType_new(AdminInfo obj);
        //public System.Data.DataSet GetContract_new(AdminInfo obj);
        //// added for ship to party
        //public System.Data.DataSet GetRoleUser(AdminInfo obj);
        //public System.Data.DataSet GetRole();
        //public System.Data.DataSet GetForm();
        //public int InsertRole(AdminInfo obj);
        //public System.Data.DataSet GetRoleAccessDetails(AdminInfo obj);
        //public System.Data.DataSet GetStatusDetails();
        //public System.Data.DataSet GetProductType();
        //public System.Data.DataSet GetSapMaterial();
        //// 'Added For Core Cage Development'
        //public System.Data.DataSet GetSapMaterial_CoreCage();
        //// ' end 
        //public System.Data.DataSet GetMaterialType();
        //public System.Data.DataSet GetProductCode();
        //public System.Data.DataSet GetStrutureElementType();
        //public System.Data.DataSet GetMasterMenu(AdminInfo obj);
        //public SqlDataReader InsertProductCode(AdminInfo obj);
        //public int InsertStructureElement(AdminInfo obj);
        //public System.Data.DataSet GetStructureElement();
        //public System.Data.DataSet GetProductDetails(AdminInfo obj);
        //public System.Data.DataSet GetBomMasterHeader();
        //public int InsertBomMasterHeader(AdminInfo obj);
        //public System.Data.DataSet GetBOM(AdminInfo obj);
        //public int InsertProductBOMDetails(AdminInfo obj);
        //public int DeleteProductBOMDetails(AdminInfo obj);
        //public string[] ValidateUser(AdminInfo obj);
        //public System.Data.DataSet GetRawMaterial();
        //public System.Data.DataSet GetWireType(AdminInfo obj);
        //public System.Data.DataSet GetCustomer();
        //public System.Data.DataSet GetContractDetail();
        //public int InsertContractDetail(AdminInfo obj);
        //public System.Data.DataSet GetBOMHeader();
        //public System.Data.DataSet GetBomIndicator();
        //public System.Data.DataSet GetTwinIndicator();
        //public System.Data.DataSet GetSAPProject(AdminInfo obj);
        //public SqlDataReader GetProductMainSelect(AdminInfo obj);
        //public SqlDataReader GetProductCrossSelect(AdminInfo obj);
        //public System.Data.DataSet GetProductTypeL2();
        //public System.Data.DataSet GetSapContractNo(AdminInfo obj);
        //public System.Data.DataSet GetProductCodeWithoutRawMaterial(AdminInfo obj);
        //public System.Data.DataSet GetStandardCode();
        //public System.Data.DataSet GetGrade();
        //public string[] GetSapContractDesc(AdminInfo obj);
        //public System.Data.DataSet GetShape();
        //public System.Data.DataSet CopySAPContractNo(AdminInfo obj);
        //public System.Data.DataSet GetProductCodeSearch(AdminInfo obj);
        //public System.Data.DataSet GetProductApprovalRights();
        //public System.Data.DataSet GetValidUsers(AdminInfo obj);
        //public System.Data.DataSet GetSAPMaterialStructureElement();
        #endregion

        #region SAP Material Details
        //public System.Data.DataSet GetSAPMaterialDetails(AdminInfo obj);
        //public int UpdateSAPMaterialDetails(AdminInfo obj);
        //public System.Data.DataSet GetSAPContractDetails(AdminInfo obj);
        //public System.Data.DataSet GetSAPProjectDetails(AdminInfo obj);
        #endregion

        #region cab product code master
        //public System.Data.DataSet GetCabProductCodeMasterDetails();
        //public int InsertCabProductCodeMasterDetails(AdminInfo obj);
        //public int DeleteCabProductCodeMasterDetails(AdminInfo obj);
        //public System.Data.DataSet GetCabProductCodeSAPMaterial();
        //public System.Data.DataSet GetGradeTypes();
        //public int FindMaterialID(string matcode);   // added for difot
        #endregion

        #region Status Master
        //public System.Data.DataSet GetWBSStatus();
        //public int InsertWBSStatus(AdminInfo obj);
        #endregion

        #region Consignment
        //public int InsertWBSConsignment(AdminInfo obj);
        //public System.Data.DataSet GetWBSConsignment();
        #endregion

        #region Project Master
        //public System.Data.DataSet GetProjectType();
        //public System.Data.DataSet GetMarketSector();
        //public System.Data.DataSet GetSales();
        //public System.Data.DataSet GetProjectInCharge();
        //public System.Data.DataSet GetSAPTransport();
        //public System.Data.DataSet GetProjectMaster(AdminInfo obj);
        //public SqlDataReader GetProjectDetails(AdminInfo obj);
        //public int InsertWBSProject(AdminInfo obj);
        //public System.Data.DataSet GetContract(AdminInfo obj);
        //public System.Data.DataSet GetSAPProject2(AdminInfo obj);
        #endregion

        #region Detailing Explorer Menu
        // public System.Data.DataSet GetDetailExploreMenu();

        #endregion

        #region Drain
        public  Task<IEnumerable<Get_ParameterSetDropdown>> GetParameterSetAsync(int projectId);

        List<GetProjectParamDrainLapDto>GetProjectParamDrainLap(AdminInfo obj);

        public int InsertProjectParamDrainLap(InsertProjectParamDrainLapDto obj,out string errorMessage);
       
        public int DeleteProductParamDrainLap(AdminInfo obj);

        Task<IEnumerable<getDrainProductCodeDto>> GetDrainProductCode();

        public int InsertProjectParamDrainOH(AdminInfo obj);
        public int DeleteProductParamDrainOH(AdminInfo obj);
        public System.Data.DataSet GetProjectParamDrainOH(AdminInfo obj);
        
        List<DrainProductCodeDetailsDto>GetDrainParameterDetails(AdminInfo obj);

        List<GetDrainWidthWMDto> GetDrainDepthWidth(AdminInfo obj);
        
        List<DrainWidthDepthDto>GetDrainWidthDepth(AdminInfo obj);
        
        Task<IEnumerable<GetDrainWidthWMDto>> GetDrainWidthWM(int tntParamSetNumber);
        
        List<GetProjectParamDrainDepthDto>GetProjectParamDrainDepth(AdminInfo obj);

        public int InsertProjectParamDrainDepth(GetProjectParamDrainDepthDto obj,out string errorMessage);
        public int DeleteProductParamDrainDepth(AdminInfo obj);
        List<GetDrainProductTypeDto>GetDrainProductType(AdminInfo obj);
        List<GetDrainProductTypeDto>GetDrainProductType();

        List<GetProjectParamDrainWMDto>GetProjectParamDrainWM(AdminInfo obj);
        public int InsertProjectParamDrainWM(InsertProjectParamDrainWMDto obj,out string errorMessage);
        public int DeleteProductParamDrainWM(AdminInfo obj);
        List<getDrainProductCodeDto>GetLapProductCodeWM(AdminInfo obj);

        List<GetProjectParamDrainLayerDto>GetProjectParamDrainLayer();
       
        List<GetProjectParamDrainLayerDto>GetProjectParamDrainMaxDepth(AdminInfo obj);
        
        List<ProjectParamDrainShapeDto> GetProjectParamDrainShapeforLayer(AdminInfo obj);
        
        List<GetProjectParamDrainParamDto>GetProjectParamDrainParamDetails(AdminInfo obj);

        List<DrainShapeCodeDto> GetDrainShapeCode(AdminInfo obj);
        public int InsertProjectDrainParamDetails(InsertProjectDrainParamDetailsDto insertProjectDrainParam,out string errorMessage);
        public int GetExistenceDrainDepthLap(AdminInfo obj);
        //public System.Data.DataSet GetShapeDetails(AdminInfo obj);----later to add
        #endregion

        #region Project Parameter

        //public System.Data.DataSet GetTransport();
        //public System.Data.DataSet GetCapCLinkProjectParameter(AdminInfo obj);
        public List<GetProjectParameterDto>GetProjectParameter(AdminInfo obj);
        public List<GetParameterSetDto>GetParameterSet(AdminInfo obj);
        public int[] InsertProjectParameter(InsertProjectParameterDto obj,out string errorMessage);
        //public int InsertProjectParamCage(AdminInfo obj);
        //public int InsertProjectParamOH(AdminInfo obj);
        //public int InsertProjectParamLap(AdminInfo obj);
        //public int[] InsertProjectParameterOHHeader(AdminInfo obj);
        //public System.Data.DataSet GetProjectParamCage(AdminInfo obj);
        //public System.Data.DataSet GetProductCodeParam(AdminInfo obj);
        //public System.Data.DataSet GetProductTypeParam(AdminInfo obj);
        //public int DeleteProductParamCage(AdminInfo obj);
        //public int DeleteProductParamOH(AdminInfo obj);
        //public int DeleteProductParamLap(AdminInfo obj);
        //public System.Data.DataSet GetProjectParameterType(AdminInfo obj);
        //public System.Data.DataSet GetCappingParamSet(AdminInfo obj);
        //public System.Data.DataSet GetCappingProductCodeParam(AdminInfo obj);
        //public System.Data.DataSet GetParamSelect(AdminInfo obj);
        //public System.Data.DataSet GetParamStandard();
        //public System.Data.DataSet GetProjectParamOH(AdminInfo obj);
        //public System.Data.DataSet GetConsignment();
        //public System.Data.DataSet GetParamMeshSelect(AdminInfo obj);
        //public System.Data.DataSet GetProductCode(AdminInfo obj);
        //public System.Data.DataSet GetStrutureElementTypeMesh();
        //public System.Data.DataSet GetProductTypeParamter(AdminInfo obj);
        //public System.Data.DataSet GetCapClinkParameterSet(AdminInfo obj);
        #endregion

        #region Copy GroupMarking
        //public System.Data.DataSet CopyParamGet_Get(AdminInfo obj);
        //public  System.Data.DataSet GetCopyGroupmarking(AdminInfo obj);
        //public  System.Data.DataSet GetGroupmarkingRevQuan(AdminInfo obj);
        //public  System.Data.DataSet GetCopyStructureMarking(AdminInfo obj);
        //public  System.Data.DataSet GetGroupMarkingName(AdminInfo obj);
        //public  System.Data.DataSet GetGroupMarkingParameterSet(AdminInfo obj);
        //public  System.Data.DataSet InsertCopyGroupMarking(AdminInfo obj);
        //public  System.Data.DataSet GetCopyGroupmarkingRevision(AdminInfo obj);
        #endregion

        #region Product BOM Master
        //public SqlDataReader GetWireDiameter(AdminInfo obj);
        //public  SqlDataReader GetWireLength(AdminInfo obj);
        #endregion

        #region Shape Code Master
        //public System.Data.DataSet GetShapeCode(AdminInfo obj);
        //public  int InsertShapeCode(AdminInfo obj);
        List<GetShapeParamDetailsDto>GetShapeParamDetails(AdminInfo obj);
        //public  int InsUpdShapeParamDetails(AdminInfo obj);
        //public  int DeleteShapeParamDetails(AdminInfo obj);
        //public  int InsUpdShapeHeaderDetails(AdminInfo obj);
        //public  System.Data.DataSet GetShapeCodeDetails(AdminInfo obj);
        #endregion

        #region Shape Pop Up
        //public int InsertShapeDetails(AdminInfo obj);
        #endregion

        #region Preferred WireLength Master
        public System.Data.DataSet GetPreferredWireLength(AdminInfo obj);
       
        public int InsUpdPreferredWireLength(AdminInfo obj);
       
        public int DeletePreferredWireLength(DrainService.Repositories.AdminInfo obj);
        #endregion

        #region Reports
        //public System.Data.DataSet GetProductTypeForReport(AdminInfo obj);
        //public  System.Data.DataSet GetReportBeamProductDetailsHeader(AdminInfo obj);
        //public  System.Data.DataSet GetReportBeamProductDetailsDetails(AdminInfo obj);
        //public  System.Data.DataSet GetReportColumnProductDetails(AdminInfo obj);
        //public  System.Data.DataSet GetReportMeshUnitTypeListHeader(AdminInfo obj);
        //public  System.Data.DataSet GetReportMeshUnitTypeListDetails(AdminInfo obj);
        //public  System.Data.DataSet GetReportBeamProductDetails(AdminInfo obj);
        //public  System.Data.DataSet GetReportDrainProductDetails(AdminInfo obj);
        //public  System.Data.DataSet GetReportMeshUnitTypeList(AdminInfo obj);
        //public  System.Data.DataSet GetReportMeshProductMarkingList(AdminInfo obj);
        //public  System.Data.DataSet GetReportBeamBOM(AdminInfo obj);
        //public  System.Data.DataSet GetReportColumnBOM(AdminInfo obj);
        //public  System.Data.DataSet GetReportMeshBOM(AdminInfo obj);
        //public  System.Data.DataSet GetReportDrainBOM(AdminInfo obj);
        //public  System.Data.DataSet GetGroupMarkingForProject(AdminInfo obj);
        //public  System.Data.DataSet GetReportWbsPostingDetailsBeam(AdminInfo obj);
        //// Public MustOverride Function GetReportBeamPostingList(ByVal obj As AdminInfo) As DataSet
        //public  System.Data.DataSet GetReportCABProductDetails(AdminInfo obj);
        //public  System.Data.DataSet GetBBSPostingDetailsForGrid(AdminInfo obj);
        //public  System.Data.DataSet GetReportWBSPostedBeam(AdminInfo obj);
        //public  System.Data.DataSet GetReportWBSPostedSlab(AdminInfo obj);
        //public  System.Data.DataSet GetReportWBSPostedColumn(AdminInfo obj);
        //public  System.Data.DataSet GetReportWbsPostingDetailsColumn(AdminInfo obj);
        //public  System.Data.DataSet GetReportWbsPostingDetailsSlab(AdminInfo obj);
        //public  System.Data.DataSet GetReportWbsPostingDetailsPrecage(AdminInfo obj);
        //public  System.Data.DataSet GetReportWbsPostingDetailsCAB(AdminInfo obj);
        //public  System.Data.DataSet GetReportPrecageProductDetails(AdminInfo obj);
        //public  System.Data.DataSet GetReportBPCProductDetails(AdminInfo obj);
        //public  System.Data.DataSet GetReportWbsPostingDetailsBPC(AdminInfo obj);
        //public  System.Data.DataSet GetReportWBSPostedPrecage(AdminInfo obj);
        //public  System.Data.DataSet GetReportProductDetails(AdminInfo obj);
        //public  System.Data.DataSet GetReportPostedParentMsh(AdminInfo obj);
        //public  System.Data.DataSet GetReportBPCSummary(AdminInfo obj);
        //public  System.Data.DataSet GetBPCSummaryExportExcel(AdminInfo obj);
        //public  System.Data.DataSet GetReportWbsPostingDetailsDrain(AdminInfo obj);
        //public  System.Data.DataSet GetReportMeshSummaryDetails(AdminInfo obj);
        //public  System.Data.DataSet GetReportPostedDrain(AdminInfo obj);
        //public  System.Data.DataSet GetDestGMRevQuanParam(AdminInfo obj);
        //public  System.Data.DataSet GetReportWbsPostedDetailsCAB(AdminInfo obj);
        //public  System.Data.DataSet GetProjectWiseGroupMarking(AdminInfo obj);
        #endregion

        #region tree caching
        //public System.Data.DataSet GetCustomerTree();
        //public System.Data.DataSet GetContractTree();
        //public System.Data.DataSet GetProjectTree();
        //public System.Data.DataSet GetCustomerStartLetters();
        //public System.Data.DataSet GetProjectTree1();
        #endregion

        #region CarrrierWireMaster
        //public System.Data.DataSet GetCarrierWireMaster(AdminInfo obj);
        //public  int saveCarrierWireMaster(AdminInfo obj);
        #endregion

        #region Wire Spec
        //public int saveWireSpecDetails(AdminInfo obj);
        //public  System.Data.DataSet GetWireSpecDetails();
        //public  int DeleteWireSpecDetails(AdminInfo obj);
        #endregion

        #region GetRoundSettings
        //public System.Data.DataSet GetRoundSettings();
        //public  int saveRoundSettings(AdminInfo obj);
        //public  int DeleteRoundSettings(AdminInfo obj);
        #endregion

        #region Structure Dimension
        // Public MustOverride Function GetDimensionDetails(ByVal obj As AdminInfo) As DataSet
        //public int saveStructureDimension(AdminInfo obj);--vidya
        #endregion

        #region TransactionDelete
        //public int DeleteTransCustomerName(AdminInfo obj);
        //public  int DeleteTransContract(AdminInfo obj);
        //public  int DeleteTransProject(AdminInfo obj);
        //public  int DeleteTransGroupMarkId(AdminInfo obj);
        #endregion

        #region NDS Maintenance
        //public System.Data.DataSet GetAllProjects(AdminInfo obj);
        //public  System.Data.DataSet GetAllContracts(AdminInfo obj);
        #endregion

        #region WBS Estimation Master
        //public int InsertWBSEstimationMaster(NDSBLL.AdminInfo obj);
        //public  System.Data.DataSet GetWBSEstimationMaster(AdminInfo obj);
        //public  System.Data.DataSet GetShapePopUpDetails(AdminInfo obj);
        #endregion

        #region Copy GM
        //public int GroupMarkExists(NDSBLL.AdminInfo obj);

        #endregion

        // Get Minimum and Maximum Values for prc Envelops
        //public System.Data.DataSet GetMinMaxPrcEnvelopValues(NDSBLL.AdminInfo obj);


    }

}

