using DetailingService.Repositories;

namespace DetailingService.Interfaces
{
    public interface IPRCDetailingService
    {
       
        void DoWork();
       
        List<BeamDetailinfo> GetCustomerContractProjectByGroupMarkId(int GroupMarkId, out List<GroupMark> listHeaderDetails, out string errorMessage);  // To Get PRC Detailing Header Details By Group Mark Id
       
        List<BeamDetailinfo> GetCustomerContractProjectByProjectId(int ProjectId, out string errorMessage);      // To Get PRC Detailing Header Details By Project Id
       
        //List<WBSInfoClass> GetWBFInfoByGroupMarkId(int GroupMarkId, out string errorMessage);                    // To Get WBF Infor Details By Group Mark Id
       
        //List<WBSInfoClass> GetWBFInfoByElementId(string strWBSValueParameter, out string errorMessage);          // To Get WBF Infor Details By Element Id
       
        int ValidatedPostedGM(int GroupMarkId, out string errorMessage);                                         // To Validate Posted GM
       
        List<BeamDetailinfo> GetGroupMarkingDetailsByGroupMarkingId(int GroupMarkId, out string errorMessage);   // To Get Group Marking Details By Group Marking Id
       
        List<SAPMaterial> GetSAPMaterialByStructureElementId(int intStructureElementId, out string errorMessage);    // To Get SAP Material By Structure Element Id
       
        List<GroupMark> GetPRCDefaultValuesByStructureElementId(int intStructureElementId, out string errorMessage);  // To Get PRC Default By Structure Element Id
       
        List<GroupMark> SaveGroupMarkDetails(GroupMark groupMark, int? Selector_id, out string errorMessage);                          // To Save the Group Marking Details
       
        bool SaveWBSInfo(string InputValueParameter, int GroupMarkId, int GroupRevNumber, out string errorMessage);  // To Save the WBSInfo
       
        List<GroupMark> FilterGroupMarkName(string enteredText, int ProjectId, int StructureElementTypeId, out string errorMessage); // To Filter the Group Mark Name
       
        List<GroupMark> GetHeaderValuesByGroupMarkId(int intGroupMarkId, out string errorMessage);
       
        List<ParameterSet> GetAllTransport(out string errorMessage);           // To Get the Tranpsort Details
        #region start for dwall prc posted report
       
        bool insertcabproductmark(int groupmarkid, out string errorMessage);
       
        //bool insertshape(int groupmarkid, out string errorMessage);
       
        
        #endregion


    }
}
