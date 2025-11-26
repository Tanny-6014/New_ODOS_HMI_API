using ShapeCodeService.Dtos;
using ShapeCodeService.Repositories;
using ShapeCodeService.Models;


namespace ShapeCodeService.Interfaces
{
    public interface ICABDAL
    {

        // ' Detail Creation
        //public  System.Data.DataSet GetHeaderInfo(CABInfo obj);
        //public  int InsUpdWBSDetails(CABInfo obj);
        //public  System.Data.DataSet GetConsignmentType(CABInfo obj);

        //public  System.Data.DataSet GetProductAccessoriesDetail(CABInfo obj);
        //public  System.Data.DataSet GetGroupMark(CABInfo obj);

        //public  int UpdateBeamCageAccess(CABInfo obj);
        //public  int InsertBeamCageAccess(CABInfo obj);
        //public  int DelCabAccess(CABInfo obj);
        //public  int InsertGroupMarking(CABInfo obj);


        //public  System.Data.DataSet GetWBSList(CABInfo obj);
        //public  int GetGroupMarkIsExist(CABInfo obj);

        //public  int InsertWbsDetailing(CABInfo obj);
        //public  int DeleteWbsDetailing(CABInfo obj);
        //public  string[] GetGroupMarkingByID(CABInfo obj);
        //// Public MustOverride Function InsertProductMarking(ByVal obj As CABInfo) As Integer
        //// Public MustOverride Function GetRoundOffValue(ByVal obj As CABInfo) As Integer

        //public  System.Data.DataSet GetPageInfoFromGroupMarkID(CABInfo obj);

        //public  System.Data.DataSet GetWBSElementByGroupMarkId(CABInfo obj);
        //// Public MustOverride Function CappingProductSelectbyPrdCode_Get(ByVal obj As CABInfo) As Integer
        //public  int GetStructElementIsExist(CABInfo obj);

        //// Public MustOverride Function GetParentProductMarkid(ByVal obj As CABInfo) As DataSet
        //// Public MustOverride Function GetParentStructureMarkid(ByVal obj As CABInfo) As DataSet
        //public  System.Data.DataSet GetProductType();
        //public  System.Data.DataSet GetStructureType(CABInfo obj);
        //public  System.Data.DataSet GetTransportMode(CABInfo obj);

        //// 'Drawing & version
        //public  System.Data.DataSet GetGroupMarkingDrawingReference(CABInfo obj);
        //public  int InsertGroupMarkingDrawingHistory(CABInfo obj);

        //public  System.Data.DataSet GetWBSDrawingReference(CABInfo obj);
        //public  int InsertWBSDrawingHistory(CABInfo obj);

        //// Public MustOverride Function GetStructureMarkingDrawingReference(ByVal obj As CABInfo) As DataSet
        //// Public MustOverride Function InsertStructureMarkingDrawingHistory(ByVal obj As CABInfo) As Integer
        //public  System.Data.DataSet GetSAPMaterialId(CABInfo obj);
        //// Public MustOverride Function GetCreepLength(ByVal obj As CABInfo) As Integer
        //// Public MustOverride Function GetCABProductCodeID(ByVal obj As CABInfo) As DataSet
        //public  System.Data.DataSet GetCABDetails(CABInfo obj);
        //// Public MustOverride Function DelCABProductMarkingDetails(ByVal obj As CABInfo) As Integer
        //public  System.Data.DataSet GetAccProductMarkingDetails(CABInfo obj);
        //public  System.Data.DataSet GetAccSAPMaterialDetails(CABInfo obj);
        //public  int InsertAccessories(CABInfo obj);
        //public  int DeleteAccessories(CABInfo obj);
        //public  System.Data.DataSet GetAccSAPDetails(CABInfo obj);
        //public  int InsertBeamPRCDetails(CABInfo obj);
        //public  System.Data.DataSet GetPRCDetails(CABInfo obj);
        //public  int InsertColumnPRCDetails(CABInfo obj);



        //public  System.Data.DataSet GetWBSListColumn(CABInfo obj);

        //public  System.Data.DataSet GetWBSElementByGroupMarkIdColumn(CABInfo obj);


        //public  int InsertSlabPRCDetails(CABInfo obj);

        //public  bool BBSPostingGetDataDiscrepancy(int GroupMarkId); // 'Added By AK for CAB Shape Data Discrepancy CR
        //public  System.Data.DataSet GetShapePathDetails(CABInfo obj);
        //public  System.Data.DataSet GetArmaInputInfo(int intSEDetailingID);
        //public  void InsertArmaTraceInfo(int intArmaID, string CopyQuery, string Result, DateTime SendTimeStamp, DateTime ReceiveTimeStamp);
        //public  int GetArmaIDPileStruct(int intStructID, string StructureElement);
        public  bool SaveCABShapeDetails(string ShapeCode, string BVBS, int NoofBends, string ShapeIndice, string ShapeGroup, bool BarToBar, string ShapeSurCharge, Int16 intCatalog, string chrVersion);
        public List<getShapeDetailsDto> GetExistingCABShapeDetails(Int16 intCatalog);
        //public  System.Data.DataTable GetCAB_BarmarkCount(int intSEDetailingID, int intGroupMarkID, int intStructureMarkID, int intProductTypeID, int intArmaid);
        public List<getShapeDetailsDto> GetCABShapeCatalogDetails();
        //public  System.Data.DataSet GetCABTransportCheck();
        //public  System.Data.DataSet GetSAPMaterialStructure(CABInfo obj);
        //public  System.Data.DataSet PopulatePRCDefault(CABInfo obj);
        //public  System.Data.DataSet GetCABTransportMode();
        public List<GetShapeCoordinatesDto>GetShapeCoordinates(string StrShapeID);
        //public  void UpdateCABCoords(CABInfo obj);
        //public  void DeleteCABShape(CABInfo obj);
        public bool UpdateShapeParameters(UpdateShapeParamDto shapeParamDto);
        //public  System.Data.DataSet GetLockNutForCouplerType(CABInfo obj);
        //public List<LoadSORDto> BBSPostingCABSOR_Get(CABInfo obj);
        //public List<LoadSORDto>ESM_BBSPostingCABSOR_Get(CABInfo obj); //As DataSet '' Added for ESM Order Processing
        //public List<LoadSORDto>BBSPostingCAB_Get(CABInfo obj);
        //public List<BBSPostingCABGMDto>BBSPostingCABGM_Get(CABInfo obj);
        //public  int BBSPostingCAB_Post(BBSCABPostDto obj);
        //public  int BBSPostingCAB_Unpost(CABInfo obj);
        //public  System.Data.DataSet GetArmaInfoByArmaID(int intArmaID);
        //public List<GroupMarkIdGetDto> GroupMarkIdCAB_Get(GroupMarkIdCABDto Obj);
        //public  int BBSReleaseCAB_Insert(CABInfo obj);
        //public  int BBSReleaseBySOR_Insert(CABInfo obj);
        //public  System.Data.DataSet BBSPostingCAB_Get_Range(CABInfo obj);
        //public List<LoadSORDto> ESM_BBSPostingCAB_Get_Range(CABInfo obj);
        public  int InsertCabShapeMaster(string ShapeID, string createdUser, string description);
        public  int InsertCabShapeDetails(CABInfo obj);
        public  int DeleteCabShapeMaster(CABInfo obj);
        public  int DeleteCabShapeDetails(CABInfo obj);
        //public  int CheckShapeExists(CABInfo obj);
        //public  System.Data.DataSet SelectExitingShapeDetails(CABInfo obj);
        //public  int InsertShapeDetailsTMast(CABInfo obj);
        //public  int InsertShapeDetailsTDetails(CABInfo obj);
        //public  System.Data.DataSet ExistingShapeTDetails(CABInfo obj);
        //public  System.Data.DataSet CABShapeProperties(CABInfo obj);
        //public  System.Data.DataSet GetBBS(int prijectId);
        //public  int CopyBBS(string bbsSource, string bbsTarget, int pojId, int wbsId, string vchStructureElementType);
        public  bool ImportImageToDB(ImportImageDto importImage);
        public  bool DeleteShapeCode(string shapeCode);
        //public  System.Data.DataSet GetBBSName(CABInfo obj);
        //public  int InsertCabShapeMasterSpecial(CABInfo obj);
        //public  int InsertCabShapeDetailsSpecial(CABInfo obj, string match, int SeqNo);
        public  byte[] GetShapeImage(string StrShapeID);
        public List<GetAllShapeImgDto> GetAllShapeImage();
        //// START:ADDED FOR ESM BY SIDDHI
        //public  System.Data.DataSet ESM_BBSPostingCABSOR_Get(CABInfo obj);
        //// END

        //// START:ADDED FOR ESM BY SIDDHI
        //public  int ESM_BBSReleaseCAB_Insert(BBSCABReleaseDto obj);
        //// END

        //// START: ADDED FOR ACTIVE INACTIVE SHAPE
        //public  void CABShape_Status_Update(string StrShapeID, int status);
        //// END

        //// START:
        //public  void ESMTrackerDataDownload(string source, string strFrom, string strTo);

        //public  System.Data.DataSet ESM_SOR_Get(CABInfo obj);
        public bool CABShape_Status_Update(string StrShapeID, int status, out string errorMessage);


    }
}



