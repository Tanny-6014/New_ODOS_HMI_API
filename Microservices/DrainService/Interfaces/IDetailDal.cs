using System.Data;
using System.Data.SqlClient;
using DrainService.Dtos;
using DrainService.Repositories;

namespace DrainService.Interfaces
{
    public interface IDetailDal
    {


        //// ' Detail Creation
        public List<GetHeaderInfoDto>GetHeaderInfo(DetailInfo obj);
        //public  System.Data.DataSet GetParameterInfo(DetailInfo obj);
        //public  System.Data.DataSet GetStructureMarking(DetailInfo obj);
        //public  System.Data.DataSet GetProductMarking(DetailInfo obj);
        //public  System.Data.DataSet ParameterInfoByPrjID_Get(DetailInfo obj);
        //public  System.Data.DataSet GetConsignmentType(DetailInfo obj);
        //public  System.Data.DataSet GetWBSElements(DetailInfo obj);
        //public  System.Data.DataSet GetProductTypeByStructElement(DetailInfo obj);
        //public  System.Data.DataSet GetProductCABDetail(DetailInfo obj);
        //public  System.Data.DataSet GetProductAccessoriesDetail(DetailInfo obj);
        //public  System.Data.DataSet GetGroupMark(DetailInfo obj);
        //public  int UpdateStructureMarking(DetailInfo obj);
        //public  int DelStructureMarking(DetailInfo obj);
        //public  int InsertStructureMarking(DetailInfo obj);
        //public  int UpdateBeamCageCAB(DetailInfo obj);
        //public  int DelBeamCageCAB(DetailInfo obj);
        //public  int InsertBeamCageCAB(DetailInfo obj);
        //public  int UpdateBeamCageAccess(DetailInfo obj);
        //public  int InsertBeamCageAccess(DetailInfo obj);
        //public  int DelBeamCageAccess(DetailInfo obj);
        //public  int InsertGroupMarking(DetailInfo obj);
        public  int InsertShapeValFrmTC(DetailInfo obj);
        //public  System.Data.DataSet GetShapeParamByShapeTrans(DetailInfo obj);

        //public  System.Data.DataSet GetWBSList(DetailInfo obj);
        //public  int GetGroupMarkIsExist(DetailInfo obj);

        //public  int InsertWbsDetailing(DetailInfo obj);
        //public  int DeleteWbsDetailing(DetailInfo obj);
        //public  string[] GetGroupMarkingByID(DetailInfo obj);
        //public  int InsertProductMarking(DetailInfo obj);
        public  int GetRoundOffValue(DetailInfo obj);
        public List<Get_ParameterSet> DrainParamInfo_Get(DetailInfo obj);
        public  System.Data.DataSet GetPageInfoFromGroupMarkID(DetailInfo obj);
        //public  System.Data.DataSet GetTcPrmFrmPrdCodeMaster(DetailInfo obj);
        //public  System.Data.DataSet GetTCPrmFrmShapeParm(DetailInfo obj);
        //public  System.Data.DataSet GetTCPrmFrmParameterSet(DetailInfo obj);
        public System.Data.DataSet GetTcTransChTCMaster(DetailInfo obj);

        //public  System.Data.DataSet GetBeamProduct(DetailInfo obj);
        //public  System.Data.DataSet GetCappingShape(DetailInfo obj);
        //public  System.Data.DataSet GetCappingProduct(DetailInfo obj);
        //public  System.Data.DataSet GetBeamShape(DetailInfo obj);
        public List<GetWBSElementByIdDto>GetWBSElementByGroupMarkId(DetailInfo obj);
        //public  int CappingProductSelectbyPrdCode_Get(DetailInfo obj);
        public  int GetStructElementIsExist(int GroupMarkingId, string StructureElementName);

        //public  System.Data.DataSet GetParentProductMarkid(DetailInfo obj);
        //public  System.Data.DataSet GetParentStructureMarkid(DetailInfo obj);
        //public  System.Data.DataSet GetProductType();

        //// 'Drawing & version
        //public  System.Data.DataSet GetGroupMarkingDrawingReference(DetailInfo obj);
        //public  int InsertGroupMarkingDrawingHistory(DetailInfo obj);

        //public  System.Data.DataSet GetWBSDrawingReference(DetailInfo obj);
        //public  int InsertWBSDrawingHistory(DetailInfo obj);

        //public  System.Data.DataSet GetStructureMarkingDrawingReference(DetailInfo obj);
        //public  int InsertStructureMarkingDrawingHistory(DetailInfo obj);
        //public  System.Data.DataSet GetSAPMaterialId(DetailInfo obj);
        //public  int GetCreepLength(DetailInfo obj);
        //public  System.Data.DataSet GetCABProductCodeID(DetailInfo obj);
        public List<GetCABDetailsDto> GetCABDetails(DetailInfo obj);
        //public  int DelCABProductMarkingDetails(DetailInfo obj);
        //public  System.Data.DataSet GetAccProductMarkingDetails(DetailInfo obj);
        //public  System.Data.DataSet GetAccSAPMaterialDetails(DetailInfo obj);
        //public  int InsertAccessories(DetailInfo obj);
        //public  System.Data.DataSet GetAccSAPDetails(DetailInfo obj);
        //public  int InsertBeamPRCDetails(DetailInfo obj);
        //public  System.Data.DataSet GetPRCDetails(DetailInfo obj);
        //public  int InsertColumnPRCDetails(DetailInfo obj);
        //public  int BCDProductMarkingDetails_Del(DetailInfo obj);
        //public  System.Data.DataSet GetTCPrmFrmBeamParm(DetailInfo obj);
        //public  int GetGroupMarkNameIsExistnew(DetailInfo obj);
        //public  int InsUpdWBSDetails(DetailInfo obj);




        ///// <summary>
        /////     ''' 
        /////     ''' </summary>
        /////     ''' <param name="obj"></param>
        /////     ''' <returns></returns>
        /////     ''' <remarks></remarks>
        /////     '''
        //public  System.Data.DataSet SWParameterInfoByPrjID_Get(DetailInfo obj);

        //public  System.Data.DataSet SWStructureMarking_Get(DetailInfo obj);
        //public  System.Data.DataSet SWProductMarking_Get(DetailInfo obj);
        //public  int SWStructureMarking_InsUpd(DetailInfo obj);
        //public  int SWProductMarking_InsUpd(DetailInfo obj);

        //// 'Drawing & version
        //public  System.Data.DataSet GetMeshWBSDrawingReference(DetailInfo obj);
        //public  int InsertMeshWBSDrawingHistory(DetailInfo obj);

        //public  System.Data.DataSet GetMeshStructureMarkingDrawingReference(DetailInfo obj);
        //public  int InsertMeshStructureMarkingDrawingHistory(DetailInfo obj);
        //public  System.Data.DataSet SWShapeCode_Get_Mesh(DetailInfo obj);
        //public  int DelSWStructureMarking(DetailInfo obj);
        //public  int DelSWProductMarking(DetailInfo obj);
        //public  System.Data.DataSet SWProjectTypeID_Get(DetailInfo obj);
        //public  System.Data.DataSet SW_MWCW_Lap_Get(DetailInfo obj);
        //public  System.Data.DataSet SWPCMSpacing_Get(DetailInfo obj);
        //public  System.Data.DataSet SWPrefMeshMaster_Get(DetailInfo obj);
        //public  System.Data.DataSet SWParam_Get(DetailInfo obj);
        //public  System.Data.DataSet PinSize_Get(DetailInfo obj);
        //public  System.Data.DataSet SWShapeParam_Get(DetailInfo obj);
        //public  System.Data.DataSet SWProjectParamOH_Get(DetailInfo obj);
        //public  System.Data.DataSet SWProjectParamLap_Get(DetailInfo obj);
        //public  System.Data.DataSet SWProductCode_Mesh_Get(DetailInfo obj);
        //public  System.Data.DataSet SWProjectOH_Get(DetailInfo obj);
        //public  int SWShapeIdByCode_Get(DetailInfo obj);
        //public  int InsertSlabPRCDetails(DetailInfo obj);
        //public  SqlDataReader GetShapeGroup(DetailInfo obj);
        //public  System.Data.DataSet SWTransportMode_Get(DetailInfo obj);
        //public  int SWProductMarkingDetails_Del(DetailInfo obj);
        //public  System.Data.DataSet GetAutoCABdata(DetailInfo obj);
        //public  System.Data.DataSet ImportSlabMachineCheck_Get();
        //public  System.Data.DataSet SWStructureProduct_Get(DetailInfo obj);

        //// Public MustOverride Function GetXMLResultFor3D(ByVal obj As DetailInfo) As DataSet

        //// export for SLAB
        //// Public MustOverride Function GetSLABExportDetails(ByVal obj As DetailInfo) As DataSet




        //// 'Column Cage
        //public  int DelStructureMarkingColumn(DetailInfo obj);
        //public  int InsertStructureMarkingColumn(DetailInfo obj);
        //public  System.Data.DataSet GetStructureMarkingColumn(DetailInfo obj);
        //public  int UpdateStructureMarkingColumn(DetailInfo obj);
        //public  int InsertProductMarkingColumn(DetailInfo obj);
        //public  System.Data.DataSet GetProductMarkingColumn(DetailInfo obj);
        //public  System.Data.DataSet GetWBSListColumn(DetailInfo obj);
        //public  System.Data.DataSet ParameterInfoByPrjIDColumn_Get(DetailInfo obj);
        //public  System.Data.DataSet GetProductTypeColumn(DetailInfo obj);
        //public  System.Data.DataSet GetShapeColumn();
        //public  System.Data.DataSet GetShapeCLink();
        //public  System.Data.DataSet GetProductTypeCLink();
        //public  int GetNoofCW(DetailInfo obj);
        //public  int GetCLinkProductSelectbyPrdCode(DetailInfo obj);
        //public  System.Data.DataSet GetWBSElementByGroupMarkIdColumn(DetailInfo obj);


        //// 'Drawing & version
        //public  System.Data.DataSet GetColumnWBSDrawingReference(DetailInfo obj);
        //public  int InsertColumnWBSDrawingHistory(DetailInfo obj);

        //public  System.Data.DataSet GetColumnStructureMarkingDrawingReference(DetailInfo obj);
        //public  int InsertColumnStructureMarkingDrawingHistory(DetailInfo obj);


        //// 'Drawing & version
        //public  System.Data.DataSet GetBPCWBSDrawingReference(DetailInfo obj);
        //public  int InsertBPCWBSDrawingHistory(DetailInfo obj);

        //public  System.Data.DataSet GetBPCSMDrawingReference(DetailInfo obj);
        //public  int InsertBPCSMDrawingHistory(DetailInfo obj);


        //// parameter set
        public List<BorePileParamInfoDto>GetBorePileParameterInfo(DetailInfo obj);
        public List<BorePileParamInfoDto> GetBorePileParameterInfoByPrjId(DetailInfo obj);
        public  int[] InsertBPCProjectParameterSet(BPCProjectParamInsertDto obj,out string errorMessage);

        //// structure marking
        public List<GetBorePilePopulateMethodsDto> GetBorePilePopulateMethods(string strType, int intProductL2Id, string strMainBarCode);
        public List<GetBorePilStructureMarkingDto> GetBorePilStructureMarkingDetails(DetailInfo obj);
        public  int InsertBPCStructureMarkingDetails(InsertBPCStructureMarkingDto obj,out string errorMessage);
        public  int UpdateBPCStructureMarkingDetails(UpdateBPCStructureMarkingDto obj, out string errorMessage);
        public  List<GetBPMainBarPatternDetailDto> GetBorePileMainBarPatternDetails(DetailInfo obj);
        public List<GetBPElevationPatternDetails>GetBorePileElevationPatternDetails(DetailInfo obj);
        public int DeleteBPCStructureMarkingDetails(DetailInfo obj);
        public  int UpdateBPCMainBarPattern(UpdateBPCMainBarDto obj, out string errorMessage);
        public  int UpdateBPCElevationPattern(BPCElevationUpdateDto obj, out string errorMessage);
        public  string getBPCMaterialCodeIDForStrucMarking(DetailInfo obj);
        public List<GetSchnellMachineConfig>GetTemplateForSchnellMachine(DetailInfo obj);
        public List<GetSchnellMachineConfig>GetSchnellMachineConfig(DetailInfo obj);
        public  int UpdateBarPostionForSchnelllMcn(SnlMcnBarPositonsUpdateDto obj, out string errorMessage);
        //public  string getWBSforDrawingXML(DetailInfo obj);
        public List<GetCageNotesDto>GetCageNotesPopulateMethods();
        //public  System.Data.DataSet getProductsDetailForXML(DetailInfo obj);
        public List<StiffenerDetailsDto>getStiffenerDetails(DetailInfo obj);
        public List<getAdditionalSpiralDetails>getAdditionalSpiralDetails(DetailInfo obj);
        public List<GetSchnellMachineConfig>GetSchnellTemplates();
        public List<List<GetSchnellMachineConfig>> GetSchnellConfiguration(DetailInfo obj);
        //public  System.Data.DataSet GetSchnellTemplate(DetailInfo obj);
        public List<GetCentralizerDto>GetPopulateCentralizer();
        //public  System.Data.DataSet GetBPCDetailsForDrawing(DetailInfo obj);
        //public  int UpdateBPCPDFPath(DetailInfo obj);
        public List<BPCMEPDetailsDto>GetMEPDetailsForBPCDrawing(DetailInfo obj);
        //public  System.Data.DataSet GetSchnellDetails(DetailInfo obj);
        //public  System.Data.DataSet GetSchnellBarsForPDF(DetailInfo obj);
        //// UpdateBarPostionForSchnelllMcn
        //// PRODUCT MARKING
        //public  System.Data.DataSet GetBorePileProductsListingByStructId(DetailInfo obj);
        public  int UpdateBorePileProducts(BPCProductMarkingUpdateDto obj,out string errorMessage);
        public  int InsertBorePileProducts(DetailInfo obj);
        public List<getAdditionalSpiralDetails>GetBPCPileDiaDependentValue(DetailInfo obj);
        public  string getCoverCodeForStrucMarking(DetailInfo obj);
        //public  int getElevtnPatnLinkDiaForStrucMarking(DetailInfo obj);
        public  string getBarMarkPrefix(DetailInfo obj);

        //// product marking pop up save accessories
        public  int InsertAccessoriesCentralizer(InsertAccessoriesCentralizerDto obj,out string errorMessage);

        //// export for BPC
        //public  System.Data.DataSet GetBPCExportDetails(DetailInfo obj);

        //// FOR GETTING DRAWING PATH   
        //// Public MustOverride Function getDrawingPathForBPC(ByVal obj As DetailInfo) As String
        //// Public MustOverride Function UpdateBPCDWGPath(ByVal obj As DetailInfo) As Integer
        //public  string getDrawingPathForBPC(DetailInfo obj);
        //public  int UpdateBPCDWGPath(DetailInfo obj);
        //public  int InsertBPCSMUploadedDrawing(DetailInfo obj);
        //public  System.Data.DataSet GetBPCSMUploadedDrawingReference(DetailInfo obj);
        //public  int UpdateBPCDetailingXML(DetailInfo obj);


        public List<DrainStructureMarkingDto>DrainStructureMarking_Get(DetailInfo obj);
        public  int DrainStructureMarking_InsUpd(AddDrainStructMarkingDto obj,out string errorMessage);
        public List<DrainParameterSetByPrjIDDto> DrainParameterSetByPrjID_Get(DetailInfo obj);
        //public  System.Data.DataSet DrainParamInfo_Get(DetailInfo obj);
        public List<DrainParamDepthValues_GetNewDto> DrainParamDepthValues_Get(DetailInfo obj);
        public  int DrainStructureMarking_Del(DetailInfo obj);
        public List<GetDrainProdMarkDto> DrainProductMarkingDetails_Get(DetailInfo obj);
        //public  System.Data.DataSet DrainProjectParamDetails_Get(DetailInfo obj);
        //public  System.Data.DataSet DrainOverHangs_Get(DetailInfo obj);
        //public  System.Data.DataSet DrainProductMarkingID_Get(DetailInfo obj);
        //public  int InsertDrainStructureMarkingDrawingHistory(DetailInfo obj);
        //public  System.Data.DataSet GetDrainStructureMarkingDrawingReference(DetailInfo obj);
        //public  int InsertDrainWBSDrawingHistory(DetailInfo obj);
        //public  System.Data.DataSet GetDrainWBSDrawingReference(DetailInfo obj);
        public  int UpdateDrainProductMarking(DetailInfo obj);

        public int DrainProductMarkingDetails_InsUpd(DetailInfo obj);
        public List<DrainParamDepthValues_GetNewDto> DrainParamDepthValues_GetNew(DetailInfo obj);
        public  System.Data.DataSet DrainProjectParamDetails_GetNew(DetailInfo obj);
        public  int DrainProductMarking_Del(DetailInfo obj);
        public  string DrainWireDiameter_Get(DetailInfo obj);
        public  List<GetSWShapeCodeDto>SWShapeCode_Get_Drain(DetailInfo obj);
        public  List<GetSWProductCodeDto> SWProductCode_Drain_Get(DetailInfo obj);
        public  List<DrainProductCodeDetailsDto> OthProductCode_Get(DetailInfo obj);
        public  List<GetOthDrainOverHangsDto> OthDrainOverHangs_Get(DetailInfo obj);
        public  int OtherDrainProductMarkingDetails_Insert(DetailInfo obj);


        //// 'Machine contraint
        //public  System.Data.DataSet GetMachineContraint(DetailInfo obj);
        //public  System.Data.DataSet GetMachineLimits(string chrShapeCode);
        //public  System.Data.DataSet GetMachineCheckValues(DetailInfo obj);


        //public  System.Data.DataSet GetArmaDetailsOnArmaID(int intArmaID);
        //public  System.Data.DataSet GetExportDetails(DetailInfo obj);
        //public  int InsertArmaInfo(string vchFolder, int intNumber, string vchDnumber, string branch, int intRelease, string vchJobTitle, string vchElement, string Structural, int intQty);

        //public  System.Data.DataSet GetProductsDetailForPDFUpdate(DetailInfo obj);
        public  List<PRCEnvelopeDetailsDto> PRCEnvelopeDetails_Get(DetailInfo obj);

        //public  System.Data.DataSet GetStructureElement();
        //public  System.Data.DataSet GetSAPMaterialStructure(DetailInfo obj);
        //public  string GetParentStructureTab(DetailInfo obj);
        //public  System.Data.DataSet PopulatePRCDefault(DetailInfo obj);
        public  int CheckDrainGroupMarking(DetailInfo obj);
        public  int DeleteAccessories(DetailInfo obj);

        public  int DrainOth_InsertShapeDetails(DetailInfo obj);
        public  List<OtherProductMarkingDetailsDto> DrainOtherProductMarkingDetails_Get(DetailInfo obj);
        public  int OtherDrainProductMarkingDetails_Update(DetailInfo obj);
        public  System.Data.DataSet GetPostedWBSForGroupMark(DetailInfo obj);
        public  System.Data.DataSet DrainParamReport_Get(DetailInfo obj);
        public int UpdateStructureMarkingDetails(int structureMarkID, string structuremarkingname, out string errormsg);
    }
}
