using WBSService.Model;
using WBSService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace WBSService.Interfaces
{
    public interface IWbs
    {
        Task<List<WBSElements>> GetWBSElementsListAsync(int wbsid);

        Task<List<WBSMaintainence>> GetWbsMaintainanceList(int ProjectId);
        
        Task<List<WBSAtCollapseLevel>> GetWbsCollapsAsync(int id);
        Task<List<ProductTypeMaster>> GetProductType();
        Task<List<StructureElementMaster>> GetStructElement();
        Task<List<WBSAtCollapseLevel>> GetStoreyfrom();
        Task<IEnumerable<StoryToFrom>> GetStoreyTo();

        public int Drawing_Approve(bool status, int wbsId);

        public  System.Threading.Tasks.Task Execute(string EmailFromAddress, string EmailFromName, string EmailTo, string EmailCc, string Subject, string Type, string Content);



        Task<int> UpdateWBSMaintainenceAsync(WBSMaintainence wbsMaintainence,int UserID);

        Task<int> AddWBSMaintainenceAsync(WBSMaintainence wbsMaintainence,int ProjectId,int UserID);

        Task<int> DeleteWbsAsync(int id);
        Task<int> DeleteWbsCollapseLevelAsync(int CollapseLevelid);

        Task<int> DeleteSelectedWbs(string wbsid);
        Task<int> DeleteStorey(WBSElements WBSElements,string IsSet);

        Task<ResponseDTO<string>> MigrateWBSReleaseData();




        #region WBS Posting
        Task<IEnumerable<WBSPosting>> GetWbsPostingListAsync(int ProjectId, int ProductTypeId);

        //Task<IEnumerable<WBSPostGroupMarkingDto>> GetGroupMarkingListAsync(int intProjectId, int intWBSElementsId, int intStructureElementId, int sitProductTypeId);

        Task<IEnumerable<WBSPostGroupMarkingDto>> GetGroupMarkingListAsync(int intProjectId, int intStructureElementId, int sitProductTypeId);

        Task<IEnumerable<Update_UnPostBBSDto>> UnPostBBS_Update(int PostHeaderId);
        Task<PostingUpdateDto> PostBBS_UpdateAsync(PostingUpdateDto wBSPosting);

        public int GetUserIDByName(string name);
        Task<int> AddBBSReleaseAsync(AddBBSReleaseDto bbsReleaseDto);
        Task<IEnumerable<WBSPostGroupMarkingDetailsDto>> GetGroupMarkingDetailsAsync(int intProjectId, int intWBSElementsId, int intStructureElementId, int sitProductTypeId, string BBSNo);

        Task<int> AddGroupMarkingAsync(AddGroupMarkingDtlDto wbsAddGroupMarking);

        Task<IEnumerable<WBSPostCappingInfoDto>> GetPostingCappingInfo(int INTPOSTHEADERID);

        Task<IEnumerable<WBSPostClinkInfoDto>> GetPostingClinkInfo(int INTPOSTHEADERID);
        Task<int> AddPostingCCLMarkDetailsAsync(AddPostingCCLMarkDetailsDto postingCCLMarkDetailsDto);
        Task<int> AddPostingCLinkCCLMarkDetailsAsync(AddPostingCCLMarkDetailsDto postingCCLMarkDetailsDto);
        Task<IEnumerable<GetPostingCappingHeaderInfoDto>> GetPostingCappingHeaderInfoAsync(int intWBSElementsId, int intParentId);
        Task<IEnumerable<GetPostingCLinkHeaderInfoDto>> GetPostingCLinkHeaderInfoAsync(int intWBSElementsId, int intParentId);
        Task<int> DeletePostingCapStructure(int intPostHeaderId,string vchProductCode, int intWidth, string chrShapeCode,string StructMarkId);
        Task<int> DeletePostingCLinkStructure(int intPostHeaderId, string vchProductCode, int intWidth, string chrShapeCode, string StructMarkId);

        Task<int> DeletePostingGroupMarkDetails(int intPostHeaderId, int intGroupMarkId);

        Task<IEnumerable<GetCapProductCodeDto>> GetProductCode(string VCHENTEREDTEXT);

        Task<IEnumerable<GetCapProductCodeDto>> GetClinkProductCode(string VCHENTEREDTEXT);

        Task<int> Check_PostingCapClinkExists(int INTPOSTHEADERID, int INTSTRUCTUREELEMENTTYPEID, int PRODUCTTYPEID);

        Task<IEnumerable<GetCapShapeCodeDto>> GetCapShapeCode(string VCHENTEREDTEXT);

        Task<IEnumerable<GetCapShapeCodeDto>> GetClinkShapeCode(string VCHENTEREDTEXT);

        Task<IEnumerable<GetCappingMO1MO2CO1CO2Dto>> GetCappingMO1MO2CO1Co2(int INTPOSTHEADERID, int MWLength, string CapProduct, int CWlength);

        Task<IEnumerable<GetCappingMO1MO2CO1CO2Dto>> GetClinkMO1MO2CO1Co2(int INTPOSTHEADERID, int MWLength, string CapProduct, int CWlength);

        Task<int> Update_PostingGroupMarkDetails(int intPostHeaderId, int intGroupMarkId, int tntGroupQty, string VCHRemarks, int intUpdatedId);

        Task<bool> SavePostGroupMarking(List<PostGroupMark> postGroupMarkList, int wbsElementId, int structureElementTypeId, int productTypeId, string wbsBBSNo, string wbsBBSDesc, int userId, int postHeaderId, int projectId);
        List<PostGroupMark> getGroupMarkwithType(int postHeaderId, out string errorMessage);
        bool PostBBS_Modular(int userId, int postHeaderId, out string errormsg, string UserName);
        bool PostBBS(int userId, int PostHeaderId, out string errorMessage,string UserName);
        List<PostGroupMark> getinvalidmaterialList(int postHeaderId, out string errorMessage);
        List<PostGroupMark> getBlankGroupMarkList(int postHeaderId, out string errorMsg);
        List<WBSPostGroupMarkingDetailsDto> WBSAttachedGroupMark(int PostHeaderId);

        bool SaveWBS(SaveWBSDto saveWBSDto);

        bool SaveClinkDetails(SaveClinkDetailsDto saveClinkDetailsDto, int UserId, int WBSElementID, int StructElementID, int ProductTypeL1Id);

        Task<string> GenerateBBSNo(int ProjectId, int StructureElementTypeId, int ProductTypeId);

        Task<int> AddWBSExtension(WBSMaintainence wbs, string Projectcode);

        List<InvalidData_Get_dto> InvalidData(int GroupmarkId);

        public int IsSubmitted_Drawing(Insert_drawing_details lst);

        public List<User_Drawing_get> User_drawing_get(string projectcode);

        public bool Edit_user_review(string UserReview, int DrawingId);

        public List<DrawingDetails> Get_docs_table(int OrderNumber, string StructureElement, string ProductType, string ScheduledProd);

        Task<IEnumerable<GetEmailDetails>> GetUserNamesByCustomerAndProject(string pCustomerCode, string pProjectCode, string wbsElementIds);

        Task<List<GetEmailDetails>> GetDetailersByCustomerAndProject(string pCustomerCode, string pProjectCode, int wbselementID);

        public List<DrawingData> Drawing_list_get(int Wbsid);

        Task<int> UpdateDrawingApprovalStatus(DrawingApprovalDto approvalDto);

        Task<int> UpdateMailSubmitStatus(string ProjectCode, int WBSElementID);

        public int IsSubmitted_Drawing_NEw(DrawingSubmission_New obj);

        public List<User_Drawing_get_new> User_drawing_get_new(string projectcode);

        public List<User_Drawing_get_new> Submission_status(int wbsId);

        public bool ModifyDrawing(ModifyDrawing obj, out string errorMessae);

        public bool modifyDrawing_Status(int WBSElementId, out string errorMessae);

        public List<DrawingReportDto> GetDrawingReport(string customerCode, string projectCode, string fromDate, string toDate);

        public bool CopyDrawingDetails(List<DrawingCopyDto> items, int drawingId, out string errorMessage);

        public int GetIsPreCast(int postheaderID);

        public bool GetFileSubmissionStatus(string customercode, string projectcode, string filename, int wbselementid, out string errorMessage);











        #endregion



    }
}



