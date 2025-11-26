using WBSService.Context;
using WBSService.Interfaces;
using WBSService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using WBSService.Constants;
using Dapper;
using WBSService.Dtos;
using static Dapper.SqlMapper;
using WBSService.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Net.Mail;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Identity.Client;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.EntityFrameworkCore.Storage;

namespace WBSService.Repositories
{
    public class WbsRepository : IWbs
    {
        private WbsServiceContext _dbContext;
        private string connectionString;
        private readonly IConfiguration _configuration;

          private const string sLoginID = "DigiOSNoReply@natsteel.com.sg";
        private const string sLoginPwd = "NatSteel@123B";

        public WbsRepository(WbsServiceContext wbsServiceContext, IConfiguration configuration)
        {
            _dbContext = wbsServiceContext;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString(SystemConstant.DefaultDBConnection);

        }

        public async Task<List<WBSElements>> GetWBSElementsListAsync(int wbsid)
        {
            return await _dbContext.WBSElements.Where(x => x.WBSMTNID == wbsid).ToListAsync();
        }

        public async Task<List<WBSAtCollapseLevel>> GetWbsCollapsAsync(int id)
        {
            try
            {

                return await _dbContext.WBSAtCollapseLevel.Where(x => x.WBSMTNID == id).ToListAsync();

            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<ProductTypeMaster>> GetProductType()
        {
            return await _dbContext.ProductTypeMaster.Where(x => x.tntStatusId == 1).ToListAsync();
        }

        public async Task<List<StructureElementMaster>> GetStructElement()
        {
            return await _dbContext.StructureElementMaster.Where(x => x.tntStatusId == 1).ToListAsync();
        }

        public async Task<List<WBSMaintainence>> GetWbsMaintainanceList(int ProjectId)
        {
            try
            {
                int WBSID = await _dbContext.WBS.Where(x => x.intProjectId == ProjectId)
          .Select(o => o.intWBSId)
          .DefaultIfEmpty()
          .FirstOrDefaultAsync();
                return await _dbContext.WBSMaintainence.Where(x => x.intWBSId == WBSID).OrderByDescending(x => x.intWBSMTNCId).ToListAsync();

            }
            catch (Exception e)
            {
                return null;
            }


        }

        //public async Task<List<WBSAtCollapseLevel>> GetStoreyTo()
        //{
        //    return await _dbContext.WBSAtCollapseLevel.Distinct().ToListAsync();
        //}

        public async Task<IEnumerable<StoryToFrom>> GetStoreyTo()
        {
            //return await _dbContext.ProjectMaster.ToListAsync();
            IEnumerable<StoryToFrom> Projlist; //new IEnumerable<List<ProjectMaster>>();

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                Projlist = sqlConnection.Query<StoryToFrom>(SystemConstant.Storey_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();


                return Projlist;


            }


        }

        public async Task<List<WBSAtCollapseLevel>> GetStoreyfrom()
        {
            return await _dbContext.WBSAtCollapseLevel.Distinct().ToListAsync();
        }


        public async Task<int> AddWBSMaintainenceAsync(WBSMaintainence wbs, int ProjectId,int UserID)
        {
            int Output = 0;
            try
            {
                IEnumerable<WBSMaintainence> Projlist;

                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@paProjectID", ProjectId);
                    dynamicParameters.Add("@paBlock", wbs.Block);
                    dynamicParameters.Add("@paStoryFrom", wbs.StoryFrom);
                    dynamicParameters.Add("@paStoryTo", wbs.StoryTo);
                    dynamicParameters.Add("@paPart", wbs.Part);
                    dynamicParameters.Add("@paProductType", wbs.ProductType);
                    dynamicParameters.Add("@paStructure", wbs.Structure);
                    dynamicParameters.Add("@paCreatedBy", UserID);
                    dynamicParameters.Add("@WBSTypeId", wbs.WBSTypeId);
                    dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);
                    sqlConnection.Query<int>(SystemConstant.sp_Insert_WBSMaintenance, dynamicParameters, commandType: CommandType.StoredProcedure);
                    Output = dynamicParameters.Get<int>("@Output");
                    sqlConnection.Close();
                }
                return Output;
            }
            catch (Exception e)
            {
                return Output;
            }
        }

        public async Task<int> UpdateWBSMaintainenceAsync(WBSMaintainence wbs,int UserID)
        {
            int Output = 0;
            try
            {
                IEnumerable<WBSMaintainence> WBSmaintancelist;
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@PaintWBSMTNCId", wbs.intWBSMTNCId);
                    dynamicParameters.Add("@paintWBSId", wbs.intWBSId);
                    dynamicParameters.Add("@paBlock", wbs.Block);
                    dynamicParameters.Add("@paStoryFrom", wbs.StoryFrom);
                    dynamicParameters.Add("@paStoryTo", wbs.StoryTo);
                    dynamicParameters.Add("@paPart", wbs.Part);
                    dynamicParameters.Add("@paProductType", wbs.ProductType);
                    dynamicParameters.Add("@paStructure", wbs.Structure);
                    dynamicParameters.Add("@paUpdatedBy", UserID);
                    dynamicParameters.Add("@WBSTypeId", wbs.WBSTypeId);
                    dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);
                    sqlConnection.Query<int>(SystemConstant.sp_Update_WBSMaintenance, dynamicParameters, commandType: CommandType.StoredProcedure);
                    Output = dynamicParameters.Get<int>("@Output");
                    sqlConnection.Close();
                }

                return Output;
            }
            catch (Exception e)
            {
                return Output;
            }

        }

        public async Task<int> DeleteWbsAsync(int id)
        {
            try
            {
                IEnumerable<WBSMaintainence> Projlist; //new IEnumerable<List<ProjectMaster>>();
                                                       // List<WBSMaintainence> demo = null;
                IEnumerable<int> result_IE;
                List<int> result_list;
                int result = 0;
                List<WBSMaintainence> WBSMaintainence_1;
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@WBSMTNID", id);
                    result_IE = sqlConnection.Query<int>(SystemConstant.sp_Delete_WBSALL, dynamicParameters, commandType: CommandType.StoredProcedure);

                    result_list = result_IE.ToList();
                    result = result_list[0];
                    sqlConnection.Close();
                }

                return result;


            }
            catch (Exception e)
            {
                return 0;
            }
            //WBSMaintainence itemToRemove = await _dbContext.WBSMaintainence.SingleOrDefaultAsync(x => x.intWBSMTNCId == id); //returns a single item.
            //_dbContext.WBSMaintainence.Remove(itemToRemove);
            //return await _dbContext.SaveChangesAsync();

        }

        public async Task<int> DeleteWbsCollapseLevelAsync(int CollapseLevelid)
        {
            try
            {
                IEnumerable<WBSAtCollapseLevel> WBSAtCollapseLevellist; //new IEnumerable<List<ProjectMaster>>();
                int result = 0;

                IEnumerable<int> result_IE;
                List<int> result_list;
                List<WBSAtCollapseLevel> WBSAtCollapseLevellist_1;// List<WBSMaintainence> demo = null;

                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@CollapseLevelid", CollapseLevelid);
                    result_IE = sqlConnection.Query<int>(SystemConstant.DeleteWBSCollapseLevel, dynamicParameters, commandType: CommandType.StoredProcedure);
                    result_list = result_IE.ToList();
                    result = result_list[0];
                    sqlConnection.Close();
                }

                return result;


            }
            catch (Exception e)
            {
                return  0;
            }
            //WBSMaintainence itemToRemove = await _dbContext.WBSMaintainence.SingleOrDefaultAsync(x => x.intWBSMTNCId == id); //returns a single item.
            //_dbContext.WBSMaintainence.Remove(itemToRemove);
            //return await _dbContext.SaveChangesAsync();

        }


        public async Task<int> DeleteSelectedWbs(string wbsid)
        {



            try
            {
                IEnumerable<WBSMaintainence> Projlist;

                IEnumerable<int> result_IE;
                List<int> result_list;

                int result = 0;
                List<WBSMaintainence> WBSMaintainence_1;


                string[] Wbsidarray = wbsid.Split(',');
                for (int i = 0; i < Wbsidarray.Length; i++)
                {
                    using (var sqlConnection = new SqlConnection(connectionString))
                    {



                        sqlConnection.Open();
                        var dynamicParameters = new DynamicParameters();
                        dynamicParameters.Add("@WBSMTNID", Int32.Parse(Wbsidarray[i]));
                        result_IE = sqlConnection.Query<int>(SystemConstant.sp_Delete_WBSALL, dynamicParameters, commandType: CommandType.StoredProcedure);
                        result_list = result_IE.ToList();
                        result = result_list[0];
                        sqlConnection.Close();
                    }
                }



                return result;




            }
            catch (Exception e)
            {
                return 0;
            }





        }
        public async Task<int> DeleteStorey(WBSElements wbs, string IsSet)
        {
            try
            {
                IEnumerable<WBSElements> Projlist; //new IEnumerable<List<ProjectMaster>>();
                                                   // List<WBSMaintainence> demo = null;

                int result = 0;
                List<WBSElements> WBSElements_1;
                IEnumerable<int> result_IE;
                List<int> result_list;


                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intStoreyLevelWBSId", wbs.intStoreyLevelWBSId);
                    dynamicParameters.Add("@WBSMTNID", wbs.WBSMTNID);
                    dynamicParameters.Add("@intWBSId", wbs.intWBSId);
                    dynamicParameters.Add("@intWBSElementId", wbs.intWBSElementId);
                    dynamicParameters.Add("@isSet", IsSet);
                    result_IE = sqlConnection.Query<int>(SystemConstant.sp_Delete_WBSElement, dynamicParameters, commandType: CommandType.StoredProcedure);

                    result_list = result_IE.ToList();
                    result = result_list[0];
                    sqlConnection.Close();


                }

                return result;


            }
            catch (Exception e)
            {
                return 0;
            }

        }

        public async Task<IEnumerable<WBSPosting>> GetWbsPostingListAsync(int ProjectId, int ProductTypeId)
        {
            try
            {
                IEnumerable<WBSPosting> Projlist;

                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTPROJECID", ProjectId);
                    dynamicParameters.Add("@SITPRODUCTTYPEID", ProductTypeId);
                    Projlist = sqlConnection.Query<WBSPosting>(SystemConstant.sp_PostingWbs_Get_PV, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return Projlist;
                }

            }
            catch (Exception e)
            {
                throw e;
            }

        }


        public async Task<IEnumerable<WBSPostGroupMarkingDto>> GetGroupMarkingListAsync(int intProjectId, int intStructureElementId, int sitProductTypeId)
        {
            IEnumerable<WBSPostGroupMarkingDto> GroupMarking;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@PROJECTID", intProjectId);
                dynamicParameters.Add("@STRUCTUREELEMENTTYPEID", intStructureElementId);
                dynamicParameters.Add("@PRODUCTTYPEID", sitProductTypeId);

                GroupMarking = sqlConnection.Query<WBSPostGroupMarkingDto>(SystemConstant.sp_GroupMarking_ddl, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return GroupMarking;
            }


        }

        public async Task<IEnumerable<Update_UnPostBBSDto>> UnPostBBS_Update(int PostHeaderId)
        {

            try
            {
                IEnumerable<Update_UnPostBBSDto> Projlist;
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTPOSTHEADERID", PostHeaderId);
                    //dynamicParameters.Add("@TNTREADYSTATUSID",wbs.StatusID);
                    //dynamicParameters.Add("@ISSUCCESS");
                    //dynamicParameters.Add("@INTSTRUCTUREELEMENTTYPEID");
                    //dynamicParameters.Add("@INTPOSTHEADERIDCPCL");
                    //dynamicParameters.Add("@INTCAPCOUNT");
                    //dynamicParameters.Add("@INTCAPGROUPMARKID");
                    //dynamicParameters.Add("@@INTWBSELEMENTID");
                    //dynamicParameters.Add("@INTPROJECTID");
                    //dynamicParameters.Add("@INTCLINKGROUPMARKID");
                    //dynamicParameters.Add("@INTCLINKCOUNT");
                    //dynamicParameters.Add("@BBSNO");
                    //dynamicParameters.Add("@intSEDetailingId");
                    //dynamicParameters.Add("@oldintSEDetailingId");

                    Projlist = sqlConnection.Query<Update_UnPostBBSDto>(SystemConstant.posting_unpost_update, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                }

                return Projlist;
            }
            catch (Exception e)
            {
                throw e;
                //return wbs;
            }

        }

        public async Task<PostingUpdateDto> PostBBS_UpdateAsync(PostingUpdateDto wbs)
        {
            try
            {
                //wbs.UserName = "JagdishH_TTL";
                wbs.PostedBy= GetUserIDByName(wbs.UserName);

                IEnumerable<PostingUpdateDto> Projlist;
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTPOSTHEADERID", wbs.PostHeaderId);
                    dynamicParameters.Add("@INTPOSTEDBY", wbs.PostedBy);


                    Projlist = sqlConnection.Query<PostingUpdateDto>(SystemConstant.posting_post_update, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                }

                return wbs;
            }
            catch (Exception e)
            {
                return wbs;
            }

        }

        public int GetUserIDByName(string name)
        {
            int Output = 0;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@UserName", name);

                    Output = sqlConnection.QueryFirstOrDefault<int>(SystemConstant.GetUserID, dynamicParameters, commandType: CommandType.StoredProcedure);
                    //Output = dynamicParameters.Get<int>("@Output");
                    sqlConnection.Close();

                }

                return Output;


            }
            catch (Exception e)
            {
                return Output;
            }

        }

        public async Task<IEnumerable<WBSPostGroupMarkingDetailsDto>> GetGroupMarkingDetailsAsync(int intProjectId, int intWBSElementsId, int intStructureElementId, int sitProductTypeId, string BBSNo)
        {
            IEnumerable<WBSPostGroupMarkingDetailsDto> GroupMarkingDetails;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intProjectId", intProjectId);
                dynamicParameters.Add("@intWBSElementsId", intWBSElementsId);
                dynamicParameters.Add("@intStructureElementId", intStructureElementId);
                dynamicParameters.Add("@sitProductTypeId", sitProductTypeId);
                dynamicParameters.Add("@BBSNo", BBSNo);
                GroupMarkingDetails = sqlConnection.Query<WBSPostGroupMarkingDetailsDto>(SystemConstant.sp_GroupMarkingDetails, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return GroupMarkingDetails;
            }


        }

        public async Task<int> AddBBSReleaseAsync(AddBBSReleaseDto wbs)
        {
            int Output = 0;
            try
            {

                //wbs.UserName = "JagdishH_TTL";
                wbs.Releaseby = GetUserIDByName(wbs.UserName);
                IEnumerable<AddBBSReleaseDto> Projlist;

                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@vchSalesOrderId", wbs.SalesOrderId);
                    dynamicParameters.Add("@vchPostHeaderId", wbs.PostHeaderId);
                    dynamicParameters.Add("@chrBBSStatus", wbs.BBSStatusId);
                    dynamicParameters.Add("@intReleaseby", wbs.Releaseby);

                    //dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);
                    sqlConnection.Query<int>(SystemConstant.BBSRelease_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);
                    Output = 1;
                    sqlConnection.Close();


                }

                return Output;


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<int> AddGroupMarkingAsync(AddGroupMarkingDtlDto wbsAddGroupMarking)
        {
            int Output = 0;
            try
            {
                IEnumerable<AddGroupMarkingDtlDto> Projlist;

                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTPOSTHEADERID", wbsAddGroupMarking.INTPOSTHEADERID);
                    dynamicParameters.Add("@INTGROUPMARKID", wbsAddGroupMarking.INTGROUPMARKID);
                    dynamicParameters.Add("@VCHGROUPMARKINGNAME", wbsAddGroupMarking.VCHGROUPMARKINGNAME);
                    dynamicParameters.Add("@TNTGROUPREVNO", wbsAddGroupMarking.TNTGROUPREVNO);
                    dynamicParameters.Add("@TNTGROUPQTY", wbsAddGroupMarking.TNTGROUPQTY);
                    dynamicParameters.Add("@VCHREMARKS", wbsAddGroupMarking.VCHREMARKS);
                    dynamicParameters.Add("@INTCREATEDUID", wbsAddGroupMarking.INTCREATEDUID);
                    dynamicParameters.Add("@INTWBSELEMENTID", wbsAddGroupMarking.INTWBSELEMENTID);
                    dynamicParameters.Add("@INTPROJECID", wbsAddGroupMarking.INTPROJECID);
                    dynamicParameters.Add("@INTSTRUCTUREELEMENTTYPEID", wbsAddGroupMarking.INTSTRUCTUREELEMENTTYPEID);
                    dynamicParameters.Add("@SITPRODUCTTYPEID", wbsAddGroupMarking.SITPRODUCTTYPEID);
                    dynamicParameters.Add("@VCHBBSNO", wbsAddGroupMarking.VCHBBSNO);
                    dynamicParameters.Add("@BBS_DESC", wbsAddGroupMarking.BBS_DESC);

                    dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);
                    sqlConnection.Query<AddGroupMarkingDtlDto>(SystemConstant.BBSGroupMarking_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);
                    Output = dynamicParameters.Get<int>("@Output");
                    sqlConnection.Close();


                }

                return Output;


            }
            catch (Exception e)
            {
                return Output;
            }

        }


        public async Task<IEnumerable<WBSPostCappingInfoDto>> GetPostingCappingInfo(int INTPOSTHEADERID)
        {
            IEnumerable<WBSPostCappingInfoDto> cappingInfo;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@INTPOSTHEADERID", INTPOSTHEADERID);

                cappingInfo = sqlConnection.Query<WBSPostCappingInfoDto>(SystemConstant.BBSCappingPost_Info, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return cappingInfo;
            }


        }

        public async Task<IEnumerable<WBSPostClinkInfoDto>> GetPostingClinkInfo(int INTPOSTHEADERID)
        {
            IEnumerable<WBSPostClinkInfoDto> cappingInfo;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTPOSTHEADERID", INTPOSTHEADERID);

                    cappingInfo = sqlConnection.Query<WBSPostClinkInfoDto>(SystemConstant.BBSClinkPost_Info, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return cappingInfo;
                }
            }
            catch (Exception e)
            {

                throw e;
            }
          


        }
        //added by vidya 
        public async Task<int> AddPostingCCLMarkDetailsAsync(AddPostingCCLMarkDetailsDto wbs)
        {
            int Output = 0;
            try
            {
                IEnumerable<AddPostingCCLMarkDetailsDto> Projlist;

                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTWBSELEMENTID", wbs.WBSElementID);
                    dynamicParameters.Add("@INTPRODUCTCODEID", wbs.ProductCodeID);
                    dynamicParameters.Add("@INTWIDTH", wbs.Width);
                    dynamicParameters.Add("@INTDEPTH", wbs.Depth);
                    dynamicParameters.Add("@INTMWLENGTH", wbs.MWLength);
                    dynamicParameters.Add("@INTCWLENGTH", wbs.CWLength);
                    dynamicParameters.Add("@INTQTY", wbs.Qty);
                    dynamicParameters.Add("@INTREVNO", wbs.RevNo);
                    dynamicParameters.Add("@CHRADDFLAG", wbs.AddFlag);
                    dynamicParameters.Add("@STRSHAPEID", wbs.ShapeId);
                    dynamicParameters.Add("@MO1", wbs.MO1);
                    dynamicParameters.Add("@MO2", wbs.MO2);
                    dynamicParameters.Add("@CO1", wbs.CO1);
                    dynamicParameters.Add("@CO2", wbs.CO2);
                    dynamicParameters.Add("@STRUCTUREELEMENTID ", wbs.StructElementID);
                    dynamicParameters.Add("@PRODUCTTYPEL1ID", wbs.ProductTypeL1Id);
                    dynamicParameters.Add("@USERID", wbs.UserId);
                    dynamicParameters.Add("@INTPOSTHEADERID", wbs.PostHeaderID);


                    sqlConnection.Query<int>(SystemConstant.PostingInsertCCLMarkDetails_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);
                    //Output = dynamicParameters.Get<int>("@Output");
                    sqlConnection.Close();


                }

                return Output;


            }
            catch (Exception e)
            {
                return Output;
            }

        }

        public async Task<int> AddPostingCLinkCCLMarkDetailsAsync(AddPostingCCLMarkDetailsDto wbs)
        {
            int Output = 0;
            try
            {
                IEnumerable<AddPostingCCLMarkDetailsDto> Projlist;

                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTWBSELEMENTID", wbs.WBSElementID);
                    dynamicParameters.Add("@INTPRODUCTCODEID", wbs.ProductCodeID);
                    dynamicParameters.Add("@INTWIDTH", wbs.Width);
                    dynamicParameters.Add("@INTDEPTH", wbs.Depth);
                    dynamicParameters.Add("@INTMWLENGTH", wbs.MWLength);
                    dynamicParameters.Add("@INTCWLENGTH", wbs.CWLength);
                    dynamicParameters.Add("@INTQTY", wbs.Qty);
                    dynamicParameters.Add("@INTREVNO", wbs.RevNo);
                    dynamicParameters.Add("@CHRADDFLAG", wbs.AddFlag);
                    dynamicParameters.Add("@STRSHAPEID", wbs.ShapeId);
                    dynamicParameters.Add("@MO1", wbs.MO1);
                    dynamicParameters.Add("@MO2", wbs.MO2);
                    dynamicParameters.Add("@CO1", wbs.CO1);
                    dynamicParameters.Add("@CO2", wbs.CO2);
                    dynamicParameters.Add("@STRUCTUREELEMENTID ", wbs.StructElementID);
                    dynamicParameters.Add("@PRODUCTTYPEL1ID", wbs.ProductTypeL1Id);
                    dynamicParameters.Add("@USERID", wbs.UserId);
                    dynamicParameters.Add("@INTPOSTHEADERID", wbs.PostHeaderID);


                    sqlConnection.Query<int>(SystemConstant.PostingInsertClinkCCLMarkDetails_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);
                    //Output = dynamicParameters.Get<int>("@Output");
                    sqlConnection.Close();


                }

                return Output;


            }
            catch (Exception e)
            {
                return Output;
            }

        }

        public async Task<IEnumerable<GetPostingCappingHeaderInfoDto>> GetPostingCappingHeaderInfoAsync(int intWBSElementsId, int intParentId)
        {
            IEnumerable<GetPostingCappingHeaderInfoDto> getPostingCappingHeaderInfos;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@WBSELEMENTID", intWBSElementsId);
                dynamicParameters.Add("@INTPARENT", intParentId);

                getPostingCappingHeaderInfos = sqlConnection.Query<GetPostingCappingHeaderInfoDto>(SystemConstant.PostingCappingHeaderInfo_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return getPostingCappingHeaderInfos;
            }


        }

        public async Task<IEnumerable<GetPostingCLinkHeaderInfoDto>> GetPostingCLinkHeaderInfoAsync(int intWBSElementsId, int intParentId)
        {
            IEnumerable<GetPostingCLinkHeaderInfoDto> getPostingClinkHeaderInfos;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@WBSELEMENTID", intWBSElementsId);
                dynamicParameters.Add("@INTPARENT", intParentId);

                getPostingClinkHeaderInfos = sqlConnection.Query<GetPostingCLinkHeaderInfoDto>(SystemConstant.PostingCLinkHeaderInfo_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return getPostingClinkHeaderInfos;
            }


        }

        public async Task<int> DeletePostingCapStructure(int intPostHeaderId,string vchProductCode,int intWidth,string chrShapeCode,string StructMarkId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@POSTHEADERID", intPostHeaderId);
                dynamicParameters.Add("@VCHBEAMPRODUCTCODE", vchProductCode);
                dynamicParameters.Add("@INTWIDTH", intWidth);
                dynamicParameters.Add("@STRSHAPEID", chrShapeCode);
                dynamicParameters.Add("@STRSMID", StructMarkId);

                sqlConnection.Query<int>(SystemConstant.PostingCapStructure_Delete, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<int> DeletePostingCLinkStructure(int intPostHeaderId, string vchProductCode, int intWidth, string chrShapeCode, string StructMarkId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@INTPOSTHEADERID", intPostHeaderId);
                dynamicParameters.Add("@vchBeamProductCode", vchProductCode);
                dynamicParameters.Add("@intWidth", intWidth);
                dynamicParameters.Add("@strShapeID", chrShapeCode);
                dynamicParameters.Add("@strsmid", StructMarkId);

                sqlConnection.Query<int>(SystemConstant.PostingCLinkStructure_Delete, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<int> DeletePostingGroupMarkDetails(int intPostHeaderId, int intGroupMarkId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@INTPOSTHEADERID", intPostHeaderId);
                dynamicParameters.Add("@INTGROUPMARKID", intGroupMarkId);

                sqlConnection.Query<int>(SystemConstant.PostingGroupMarking_Delete, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return await _dbContext.SaveChangesAsync();
            }
        }


        public async Task<IEnumerable<GetCapProductCodeDto>> GetProductCode(string VCHENTEREDTEXT)
        {
            IEnumerable<GetCapProductCodeDto> getCapProductCode;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@VCHENTEREDTEXT", VCHENTEREDTEXT);


                getCapProductCode = sqlConnection.Query<GetCapProductCodeDto>(SystemConstant.CapProductCode_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return getCapProductCode;
            }


        }

        public async Task<IEnumerable<GetCapProductCodeDto>> GetClinkProductCode(string VCHENTEREDTEXT)
        {
            IEnumerable<GetCapProductCodeDto> getCapProductCode;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@VCHENTEREDTEXT", VCHENTEREDTEXT);


                getCapProductCode = sqlConnection.Query<GetCapProductCodeDto>(SystemConstant.ClinkProductCode_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return getCapProductCode;
            }


        }

        public async Task<int> Check_PostingCapClinkExists(int INTPOSTHEADERID, int INTSTRUCTUREELEMENTTYPEID , int PRODUCTTYPEID)
        {
            int Output = 0;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTPOSTHEADERID", INTPOSTHEADERID);
                    dynamicParameters.Add("@INTSTRUCTUREELEMENTTYPEID", INTSTRUCTUREELEMENTTYPEID);
                    dynamicParameters.Add("@PRODUCTTYPEID", PRODUCTTYPEID);
                    dynamicParameters.Add("@OutPut", null, dbType: DbType.Int32, ParameterDirection.Output);
                    sqlConnection.Query<CheckProductCodeDto>(SystemConstant.Exists_Check_PostingCapClinkExists, dynamicParameters, commandType: CommandType.StoredProcedure);
                    Output = dynamicParameters.Get<int>("@OutPut");
                    sqlConnection.Close();
                    return Output;

                }
            }
            catch (Exception e)
            {

                throw e;
            }
            


        }

        public async Task<IEnumerable<GetCapShapeCodeDto>> GetCapShapeCode(string VCHENTEREDTEXT)
        {
            IEnumerable<GetCapShapeCodeDto> getCapShapeCode;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@VCHENTEREDTEXT", VCHENTEREDTEXT);


                getCapShapeCode = sqlConnection.Query<GetCapShapeCodeDto>(SystemConstant.CapShapeCode_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return getCapShapeCode;
            }


        }

        public async Task<IEnumerable<GetCapShapeCodeDto>> GetClinkShapeCode(string VCHENTEREDTEXT)
        {
            IEnumerable<GetCapShapeCodeDto> getCapShapeCode;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@VCHENTEREDTEXT", VCHENTEREDTEXT);


                getCapShapeCode = sqlConnection.Query<GetCapShapeCodeDto>(SystemConstant.ClinkShapeCode_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return getCapShapeCode;
            }


        }

        public async Task<IEnumerable<GetCappingMO1MO2CO1CO2Dto>> GetCappingMO1MO2CO1Co2(int INTPOSTHEADERID,int MWLength,string CapProduct,int CWlength)
        {
            IEnumerable<GetCappingMO1MO2CO1CO2Dto> getCapShapeCode;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@INTPOSTHEADERID", INTPOSTHEADERID);
                dynamicParameters.Add("@MWLength", MWLength);
                dynamicParameters.Add("@CapProduct", CapProduct);
                dynamicParameters.Add("@CWlength", CWlength);


                getCapShapeCode = sqlConnection.Query<GetCappingMO1MO2CO1CO2Dto>(SystemConstant.CapShapeCode_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return getCapShapeCode;
            }


        }

        public async Task<IEnumerable<GetCappingMO1MO2CO1CO2Dto>> GetClinkMO1MO2CO1Co2(int INTPOSTHEADERID, int MWLength, string CapProduct, int CWlength)
        {
            IEnumerable<GetCappingMO1MO2CO1CO2Dto> getCapShapeCode;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@INTPOSTHEADERID", INTPOSTHEADERID);
                dynamicParameters.Add("@MWLength", MWLength);
                dynamicParameters.Add("@CapProduct", CapProduct);
                dynamicParameters.Add("@CWlength", CWlength);


                getCapShapeCode = sqlConnection.Query<GetCappingMO1MO2CO1CO2Dto>(SystemConstant.ClinkMO1MO2CO1Co2_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return getCapShapeCode;
            }


        }


        public async Task<int> Update_PostingGroupMarkDetails(int intPostHeaderId, int intGroupMarkId,int tntGroupQty,string VCHRemarks,int intUpdatedId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@INTPOSTHEADERID", intPostHeaderId);
                dynamicParameters.Add("@INTGROUPMARKID", intGroupMarkId);
                dynamicParameters.Add("@TNTGROUPQTY", tntGroupQty);
                dynamicParameters.Add("@VCHREMARKS", VCHRemarks);
                dynamicParameters.Add("@INTUPDATEDUID", intUpdatedId);

                sqlConnection.Query<int>(SystemConstant.UpdateGroupMarking_Delete, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> SavePostGroupMarking(List<PostGroupMark> postGroupMarkList, int wbsElementId, int structureElementTypeId, int productTypeId, string wbsBBSNo, string wbsBBSDesc, int userId, int postHeaderId, int projectId)
        {
            bool isSuccess = false;
            int Output = 0;
            try {
                IEnumerable<AddGroupMarkingDtlDto> Projlist;
                using (var sqlConnection = new SqlConnection(connectionString))
            {
                    
                    sqlConnection.Open();
                    foreach (PostGroupMark postGroupMark in postGroupMarkList)
                    {
                       
                        var dynamicParameters = new DynamicParameters();
                        //dynamicParameters.CreateParameters(13);
                        dynamicParameters.Add("@INTPOSTHEADERID", postGroupMark.PostHeaderId);
                        dynamicParameters.Add("@INTGROUPMARKID", postGroupMark.GroupMarkId);
                        dynamicParameters.Add("@VCHGROUPMARKINGNAME", postGroupMark.GroupMarkingName);
                        dynamicParameters.Add("@TNTGROUPREVNO", postGroupMark.GroupMarkingRevisionNumber);
                        dynamicParameters.Add("@TNTGROUPQTY", postGroupMark.GroupQty);
                        dynamicParameters.Add("@VCHREMARKS", postGroupMark.Remarks);
                        dynamicParameters.Add("@INTCREATEDUID", userId);
                        dynamicParameters.Add("@INTWBSELEMENTID", wbsElementId);
                        dynamicParameters.Add("@INTPROJECID", postGroupMark.ProjectId);
                        dynamicParameters.Add("@INTSTRUCTUREELEMENTTYPEID", structureElementTypeId);
                        dynamicParameters.Add("@SITPRODUCTTYPEID", productTypeId);
                        dynamicParameters.Add("@VCHBBSNO", wbsBBSNo);
                        dynamicParameters.Add("@BBS_DESC", wbsBBSDesc);
                        dynamicParameters.Add("@OutPut", null, dbType: DbType.Int32, ParameterDirection.Output);
                        //sqlConnection.Query<int>(SystemConstant.UpdateGroupMarking_Delete, dynamicParameters, commandType: CommandType.StoredProcedure);
                        //object objDuplicate = sqlConnection.ExecuteScalar(CommandType.StoredProcedure, "USP_POSTINGPOSTGM_INSERT_CUBE");
                        //sqlConnection.Query<int>(SystemConstant.USP_POSTINGPOSTGM_INSERT_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);
                        sqlConnection.Query<AddGroupMarkingDtlDto>(SystemConstant.USP_POSTINGPOSTGM_INSERT_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);
                        //Modified for CABDE by TCS on 04.07.2016.
                        // object objDuplicate = sqlConnection.ExecuteScalar(CommandType.StoredProcedure, "USP_POSTINGPOSTGM_INSERT_CUBE"); //Modified for CABDE by TCS on 04.07.2016.
                        //this.Result = Convert.ToString(objDuplicate);
                        Output = dynamicParameters.Get<int>("@OutPut");
                       

                        isSuccess = true;
                    }
                    sqlConnection.Close();
            }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
             
            }
            return isSuccess;
        }

    
        public List<PostGroupMark> getGroupMarkwithType(int postHeaderId, out string errorMessage) ///added by TCS on 01.07.2016 for CABDE
        {
            errorMessage = "";
            List<PostGroupMark> listGetGroupMark=new List<PostGroupMark> { };
            DataSet dsGetGroupMarkList = new DataSet();
            try
            {

                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();

                    SqlCommand cmd = new SqlCommand(SystemConstant.get_listGroupMarkWithType, sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@PostHeaderId ", postHeaderId));

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dsGetGroupMarkList);

                    if (dsGetGroupMarkList != null && dsGetGroupMarkList.Tables.Count != 0)
                    {
                        foreach (DataRowView drvGM in dsGetGroupMarkList.Tables[0].DefaultView)
                        {
                            PostGroupMark singleGM = new PostGroupMark
                            {
                                GroupMarkId = Convert.ToInt32(drvGM["groupMarkID"]),
                                GroupMarkingName = Convert.ToString(drvGM["groupMarkName"]),
                                isCABDE = Convert.ToInt32(drvGM["CABDEflag"])
                            };

                            listGetGroupMark.Add(singleGM);
                        }

                    }

                }

                //using (var sqlConnection = new SqlConnection(connectionString))
                //{
                //    var dynamicParameters = new DynamicParameters();
                //    sqlConnection.Open();
                //    dynamicParameters.Add("@PostHeaderId ", postHeaderId);
                //    listGetGroupMark = sqlConnection.Query<PostGroupMark>(SystemConstant.get_listGroupMarkWithType, dynamicParameters, commandType: CommandType.StoredProcedure);

                //}


            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                throw ex;
            }
        
            return listGetGroupMark;
        }



        public List<PostGroupMark> getBlankGroupMarkList(int postHeaderId, out string errorMsg) //added by TCS on 17.03.2016 DIFOT
        {
            
            List<PostGroupMark> listGetBlankGroupMark = new List<PostGroupMark> { };
            DataSet dsGetBlankGroupMarkList = new DataSet();
            errorMsg = "";
            try
            {

                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();

                    SqlCommand cmd = new SqlCommand(SystemConstant.get_listGroupMarkWithZeroPcs, sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@PostHeaderId ", postHeaderId));

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dsGetBlankGroupMarkList);

                    if (dsGetBlankGroupMarkList != null && dsGetBlankGroupMarkList.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBBSDescription in dsGetBlankGroupMarkList.Tables[0].DefaultView)
                        {
                            PostGroupMark blankGM = new PostGroupMark
                            {
                                GroupMarkId = Convert.ToInt32(drvBBSDescription["groupMarkId"]),
                                GroupMarkingName = Convert.ToString(drvBBSDescription["groupMarkName"]),
                                PostedQty = Convert.ToInt32(drvBBSDescription["noPcs"])
                           
                            };
                            listGetBlankGroupMark.Add(blankGM);
                        }

                    }

                  
                }

                //using (var sqlConnection = new SqlConnection(connectionString))
                //{
                //    var dynamicParameters = new DynamicParameters();
                //    dynamicParameters.Add("@PostHeaderId ", postHeaderId);
                //    sqlConnection.Open();
                //    dsGetBlankGroupMarkList=(DataSet)sqlConnection.Query<DataSet>(SystemConstant.get_listGroupMarkWithZeroPcs, dynamicParameters, commandType: CommandType.StoredProcedure);
                //}

                //if (dsGetBlankGroupMarkList != null && dsGetBlankGroupMarkList.Tables.Count != 0)
                //{
                //    foreach (DataRowView drvBBSDescription in dsGetBlankGroupMarkList.Tables[0].DefaultView)
                //    {
                //        PostGroupMark blankGM = new PostGroupMark
                //        {
                //            GroupMarkId = Convert.ToInt32(drvBBSDescription["groupMarkID"]),
                //            GroupMarkingName = Convert.ToString(drvBBSDescription["groupMarkName"]),
                //            PostedQty = Convert.ToInt32(drvBBSDescription["noPcs"])
                //        };
                //        listGetBlankGroupMark.Add(blankGM);
                //    }
                //}
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                throw ex;
            }
         
            return listGetBlankGroupMark;
        }
       
        public List<PostGroupMark> getinvalidmaterialList(int postHeaderId, out string errorMessage) //added by TCS for difot
        {
          
            errorMessage = "";
            List<PostGroupMark> listerrorGroupMark = new List<PostGroupMark> { };
            DataSet dsGetwrongGroupMarkList = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    SqlCommand cmd = new SqlCommand(SystemConstant.get_listGroupMarkWithZeroPcs, sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@PostHeaderId ", postHeaderId));

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dsGetwrongGroupMarkList);

                    if (dsGetwrongGroupMarkList != null && dsGetwrongGroupMarkList.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBBSDescription in dsGetwrongGroupMarkList.Tables[0].DefaultView)
                        {
                            PostGroupMark errorGM = new PostGroupMark
                            {
                                GroupMarkId = Convert.ToInt32(drvBBSDescription["groupMarkid"]),
                                GroupMarkingName = Convert.ToString(drvBBSDescription["groupMarkName"]),
                                //productcode = Convert.ToString(drvBBSDescription["productcode"])
                            };
                            listerrorGroupMark.Add(errorGM);
                        }
                    }
                }

                   
                //using (var sqlConnection = new SqlConnection(connectionString))
                //{
                //    var dynamicParameters = new DynamicParameters();
                //    dynamicParameters.Add("@PostHeaderId ", postHeaderId);
                //    dsGetwrongGroupMarkList = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.get_listGroupMarkWithinvalidmaterial, dynamicParameters, commandType: CommandType.StoredProcedure);

                //}

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                throw ex;
            }
            finally
            {
                //dbManager.Dispose();
            }
            return listerrorGroupMark;
        }

        public bool PostBBS_Modular(int userId, int postHeaderId, out string errormsg, string UserName)
        {
            bool isSuccess = false;
            userId = GetUserIDByName(UserName);
            DataSet result = new DataSet();
            // int result;
            //DBManager dbManager = new DBManager();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = connectionString;
            con.Open();
            errormsg = "";
            try
            {
                using (SqlCommand itemsCommand = new SqlCommand("Usp_posting_post_update_Modular", con))
                {
                    itemsCommand.CommandType = CommandType.StoredProcedure;

                    SqlParameter sqlParameter = new SqlParameter("@INTPOSTHEADERID", SqlDbType.Int) { Direction = ParameterDirection.Input };
                    sqlParameter.Value = postHeaderId;
                    itemsCommand.Parameters.Add(sqlParameter);

                    sqlParameter = new SqlParameter("@INTPOSTEDBY", SqlDbType.Int) { Direction = ParameterDirection.Input };
                    sqlParameter.Value = userId;
                    itemsCommand.Parameters.Add(sqlParameter);

                    sqlParameter = new SqlParameter("@ISSUCCESS", SqlDbType.Int, 0) { Direction = ParameterDirection.Output };
                    itemsCommand.Parameters.Add(sqlParameter);

                    itemsCommand.ExecuteNonQuery();
                    if (sqlParameter.Value.ToString() == "0")
                    {
                        isSuccess = false;
                    }
                    else
                    {
                        isSuccess = true;
                    }
                    // con.Close();
                }

            }

            catch (Exception ex)
            {
                //throw ex;
                errormsg = ex.Message;
            }
            finally
            {
                //dbManager.Dispose();
            }
            return isSuccess;

        }

        public bool PostBBS(int userId,int PostHeaderId, out string errorMessage, string UserName)
        {
            bool isSuccess = false;
            errorMessage = "";
            userId = GetUserIDByName(UserName);

            //DBManager dbManager = new DBManager();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTPOSTHEADERID ", PostHeaderId);
                    dynamicParameters.Add("@INTPOSTEDBY", userId);
                    
                    sqlConnection.Query<int>(SystemConstant.USP_POSTING_POST_UPDATE, dynamicParameters, commandType: CommandType.StoredProcedure);
                    
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                throw ex;
            }
            finally
            {
                //dbManager.Dispose();
            }
            return isSuccess;
        }

        public List<WBSPostGroupMarkingDetailsDto> WBSAttachedGroupMark(int PostHeaderId)
        {
            //DBManager dbManager = new DBManager();
            List<PostGroupMark> listGetGroupMark = new List<PostGroupMark> { };
            List<WBSPostGroupMarkingDetailsDto> result = new List<WBSPostGroupMarkingDetailsDto>();

            DataSet dsGetGroupMark = new DataSet();
            try
            {
                //dbManager.Open();
                //dbManager.CreateParameters(1);
                //dbManager.AddParameters(0, "@INTPOSTHEADERID", this.PostHeaderId);



                //var dynamicParameters = new DynamicParameters();
                //dynamicParameters.Add("@INTPOSTHEADERID", PostHeaderId);
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand cmd = new SqlCommand(SystemConstant.usp_PostingWbsAttachedGM_Get, sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@INTPOSTHEADERID ", PostHeaderId));

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dsGetGroupMark);

                    //dsGetGroupMark =(DataSet)sqlConnection.Query<DataSet>(SystemConstant.usp_PostingWbsAttachedGM_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    //dsGetGroupMark = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_PostingWbsAttachedGM_Get");
                    if (dsGetGroupMark != null && dsGetGroupMark.Tables.Count != 0)
                    {
                        foreach (DataRowView drvGroupMark in dsGetGroupMark.Tables[0].DefaultView)
                        {
                            WBSPostGroupMarkingDetailsDto postGroupMark = new WBSPostGroupMarkingDetailsDto
                            {
                                 //= Convert.ToInt32(drvGroupMark["INTPOSTGROUPMARKINGDETAILSID"]),
                                 //= Convert.ToInt32(drvGroupMark["INTPOSTHEADERID"]),
                                intSMGroupMarkId = Convert.ToInt32(drvGroupMark["INTGROUPMARKID"]),
                                vchSMGroupMarkingName = Convert.ToString(drvGroupMark["VCHGROUPMARKINGNAME"]),
                                tntSMGroupRevNo = Convert.ToInt32(drvGroupMark["TNTGROUPREVNO"]),
                                intSMProductQty = Convert.ToInt32(drvGroupMark["TNTGROUPQTY"]),
                                vchSMRemarks = Convert.ToString(drvGroupMark["VCHREMARKS"]),
                                // = Convert.ToString(drvGroupMark["VCHBBSNO"]),
                                //BBSRemarks = Convert.ToString(drvGroupMark["VCHBBSREMARKS"]),
                                //intSMProductQty = Convert.ToInt32(drvGroupMark["INTPOSTEDQTY"]),
                                // = Convert.ToDecimal(drvGroupMark["NUMPOSTEDWEIGHT"]),
                                //mo = Convert.ToString(drvGroupMark["MODULAR"]) //Modular 31014
                                //intStructureElementTypeId = Convert.ToString(drvGroupMark["intStructureElementTypeId"])

                                //Added By JH
                                PostGroupMarkingDetailsId = Convert.ToInt32(drvGroupMark["INTPOSTGROUPMARKINGDETAILSID"]),
                                PostHeaderId = Convert.ToInt32(drvGroupMark["INTPOSTHEADERID"]),
                                GroupMarkId = Convert.ToInt32(drvGroupMark["INTGROUPMARKID"]),
                                GroupMarkingName = Convert.ToString(drvGroupMark["VCHGROUPMARKINGNAME"]),
                                GroupMarkingRevisionNumber = Convert.ToInt32(drvGroupMark["TNTGROUPREVNO"]),
                                GroupQty = Convert.ToInt32(drvGroupMark["TNTGROUPQTY"]),
                                Remarks = Convert.ToString(drvGroupMark["VCHREMARKS"]),
                                BBSNo = Convert.ToString(drvGroupMark["VCHBBSNO"]),
                                BBSRemarks = Convert.ToString(drvGroupMark["VCHBBSREMARKS"]),
                                PostedQty = Convert.ToInt32(drvGroupMark["INTPOSTEDQTY"]),
                                PostedWeight = Convert.ToDecimal(drvGroupMark["NUMPOSTEDWEIGHT"]),
                            };
                            result.Add(postGroupMark);
                        }
                    }
                }

              
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //dbManager.Dispose();
            }
            return result;
        }

        public List<CapClink> Capping_Get(int PostHeaderId)
        {
            //DBManager dbManager = new DBManager();
            List<CapClink> listCapping = new List<CapClink> { };
            DataSet dsCapping = new DataSet();
            try
            {

                //dbManager.Open();
                //dbManager.CreateParameters(1);
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    //dbManager.AddParameters(0, "@INTPOSTHEADERID", PostHeaderId);
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTPOSTHEADERID", PostHeaderId);

                    dsCapping= (DataSet)sqlConnection.Query<DataSet>(SystemConstant.usp_PostingCappingInfo_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    //dsCapping = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_PostingCappingInfo_Get");
                }
                if (dsCapping != null && dsCapping.Tables.Count != 0)
                {
                    foreach (DataRowView drvCappingInfo in dsCapping.Tables[0].DefaultView)
                    {

                        //For StructureElement
                        StructureElement objStructureElement = new StructureElement();
                        List<StructureElement> listStructureElement = new List<StructureElement>();
                        string listStructureElementCache = "listStructureElementCache";
                        //if (IndexusDistributionCache.SharedCache.Get(listStructureElementCache) == null)
                        //{
                        //    listStructureElement = objStructureElement.GetStructureElement();
                        //}
                        //else
                        //{
                        //    listStructureElement = IndexusDistributionCache.SharedCache.Get(listStructureElementCache) as List<StructureElement>;
                        //}
                        listStructureElement = listStructureElement.FindAll(x => x.StructureElementType == Convert.ToString((drvCappingInfo["STRUCTUREELEMENT"])));

                        if (listStructureElement.Count != 0)
                        {
                            objStructureElement.StructureElementType = Convert.ToString((drvCappingInfo["StructureElement"]));
                        }
                        ShapeCode objShapeCode = new ShapeCode();
                        List<ShapeCode> listShapeCode = new List<ShapeCode>();
                        listShapeCode = objShapeCode.ShapeCodeNoFilter_Get();
                        listShapeCode = listShapeCode.FindAll(x => x.ShapeCodeName == Convert.ToString((drvCappingInfo["ShapeCode"])));
                        if (listShapeCode.Count != 0)
                        {
                            objShapeCode.ShapeCodeName = Convert.ToString((drvCappingInfo["ShapeCode"]));
                            objShapeCode.ShapeID = listShapeCode[0].ShapeID;
                        }
                        else
                        {
                            objShapeCode.ShapeCodeName = "";
                            objShapeCode.ShapeID = 0;
                        }
                        ProductCode objProductCode = new ProductCode();
                        List<ProductCode> listProductCode = new List<ProductCode>();
                        listProductCode = objProductCode.CapProductCodeNoFilter_Get();
                        listProductCode = listProductCode.FindAll(x => x.ProductCodeName == Convert.ToString((drvCappingInfo["CAPPRODUCT"])));
                        if (listProductCode.Count != 0)
                        {
                            objProductCode.ProductCodeName = Convert.ToString((drvCappingInfo["CAPPRODUCT"]));
                            objProductCode.ProductCodeId = listProductCode[0].ProductCodeId;
                        }
                        else
                        {
                            objProductCode.ProductCodeName = "";
                            objProductCode.ProductCodeId = 0;
                        }

                        CapClink capClink = new CapClink
                        {
                            Width = Convert.ToInt32(drvCappingInfo["WIDTH"]),
                            Depth = Convert.ToInt32(drvCappingInfo["DEPTH"]),
                            Count = Convert.ToInt32(drvCappingInfo["COUNT"]),
                            Type = Convert.ToString(drvCappingInfo["TYPE"]),
                            StructureElement = objStructureElement,
                            MWLength = Convert.ToInt32(drvCappingInfo["MWLENGTH"]),
                            CWQty = Convert.ToInt32(drvCappingInfo["CWQTY"]),
                            MO1 = Convert.ToInt32(drvCappingInfo["MO1"]),
                            MO2 = Convert.ToInt32(drvCappingInfo["MO2"]),
                            CWSpace = Convert.ToInt32(drvCappingInfo["CWSPACE"]),
                            MWSpace = Convert.ToInt32(drvCappingInfo["MWSPACE"]),
                            CWLength = Convert.ToInt32(drvCappingInfo["LENGTH"]),
                            CO1 = Convert.ToInt32(drvCappingInfo["CO1"]),
                            CO2 = Convert.ToInt32(drvCappingInfo["CO2"]),
                            MWQty = Convert.ToInt32(drvCappingInfo["MWQTY"]),
                            InvoiceCWWeight = Convert.ToDecimal(drvCappingInfo["INVOICECWWEIGHT"]),
                            InvoiceMWWeight = Convert.ToDecimal(drvCappingInfo["INVOICEMWWEIGHT"]),
                            TheoriticalWeight = Convert.ToDecimal(drvCappingInfo["THEORITICALWEIGHT"]),
                            Area = Convert.ToInt32(drvCappingInfo["AREA"]),
                            SMID = Convert.ToInt32(drvCappingInfo["SMID"]),
                            PMID = Convert.ToInt32(drvCappingInfo["PMID"]),
                            ShapeCode = objShapeCode,
                            ProductCode = objProductCode,
                            Qty = Convert.ToInt32(drvCappingInfo["COUNT"])
                        };
                        listCapping.Add(capClink);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //dbManager.Dispose();
            }
            return listCapping;
        }


        public bool SaveWBS(SaveWBSDto saveWBSDto)
        {
            bool isSuccess = false;
            var sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            var dynamicParameters = new DynamicParameters();
            try
            {

                dynamicParameters.Add("@INTWBSELEMENTID", saveWBSDto.WBSElementId);
                dynamicParameters.Add("@INTPROJECID", saveWBSDto.ProjectId);
                dynamicParameters.Add("@INTSTRUCTUREELEMENTTYPEID", saveWBSDto.StructureElementTypeId);
                dynamicParameters.Add("@SITPRODUCTTYPEID", saveWBSDto.ProductTypeId);
                dynamicParameters.Add("@VCHBBSNO", saveWBSDto.BBSNO);
                dynamicParameters.Add("@BBS_DESC", saveWBSDto.BBSSDesc);
                sqlConnection.Query<int>(SystemConstant.USP_POSTINGBBSPOSTING_INSERT, dynamicParameters, commandType: CommandType.StoredProcedure);
               
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return isSuccess;
        }


        public bool SaveClinkDetails(SaveClinkDetailsDto saveClinkDetailsDto, int UserId,int WBSElementID, int StructElementID, int ProductTypeL1Id)
        {
            bool Output = true;
            try
            {
                IEnumerable<AddPostingCCLMarkDetailsDto> Projlist;

                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTWBSELEMENTID", WBSElementID);
                    dynamicParameters.Add("@INTPRODUCTCODEID", saveClinkDetailsDto.ProductCodeID);
                    dynamicParameters.Add("@INTWIDTH", saveClinkDetailsDto.Width);
                    dynamicParameters.Add("@INTDEPTH", saveClinkDetailsDto.Depth);
                    dynamicParameters.Add("@INTMWLENGTH", saveClinkDetailsDto.MWLength);
                    dynamicParameters.Add("@INTCWLENGTH", saveClinkDetailsDto.CWLength);
                    dynamicParameters.Add("@INTQTY", saveClinkDetailsDto.Qty);
                    dynamicParameters.Add("@INTREVNO", saveClinkDetailsDto.RevNo);
                    dynamicParameters.Add("@CHRADDFLAG", saveClinkDetailsDto.AddFlag);
                    dynamicParameters.Add("@STRSHAPEID", saveClinkDetailsDto.ShapeId);
                    dynamicParameters.Add("@MO1", saveClinkDetailsDto.MO1);
                    dynamicParameters.Add("@MO2", saveClinkDetailsDto.MO2);
                    dynamicParameters.Add("@CO1", saveClinkDetailsDto.CO1);
                    dynamicParameters.Add("@CO2", saveClinkDetailsDto.CO2);
                    dynamicParameters.Add("@STRUCTUREELEMENTID ", StructElementID);
                    dynamicParameters.Add("@PRODUCTTYPEL1ID", ProductTypeL1Id);
                    dynamicParameters.Add("@USERID", UserId);
                    dynamicParameters.Add("@INTPOSTHEADERID", saveClinkDetailsDto.PostHeaderID);


                    sqlConnection.Query<int>(SystemConstant.PostingInsertClinkCCLMarkDetails_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);
                    //Output = dynamicParameters.Get<int>("@Output");
                    sqlConnection.Close();


                }

                return Output;


            }
            catch (Exception e)
            {
                return Output;
            }

        }

        public async Task<string> GenerateBBSNo(int ProjectId,int StructureElementTypeId,int ProductTypeId)
        {
            bool isSuccess = false;
            string BBSNO = "";
            var sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            var dynamicParameters = new DynamicParameters();
            try
            {
               dynamicParameters.Add("@INTPROJECTID", ProjectId);
               dynamicParameters.Add("@INTSTRUCTUREELEMENTTYPEID",StructureElementTypeId);
                dynamicParameters.Add("@INTPRODUCTTYPEID", ProductTypeId);
              dynamic result= sqlConnection.Query<string>(SystemConstant.USP_POSTINGBBSNUMBERGENERATION_GET, dynamicParameters, commandType: CommandType.StoredProcedure);
                BBSNO = result[0];
                //isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
         
            return BBSNO;
        }

        public async Task<int> AddWBSExtension(WBSMaintainence wbs, string Projectcode)
        {
            int Output = 0;
            int ProjectId = 0;
            try
            {
          
                IEnumerable<WBSMaintainence> Projlist;

                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@ProjectCode", Projectcode);
                    dynamicParameters.Add("@paBlock", wbs.Block);
                    dynamicParameters.Add("@paStoryFrom", wbs.StoryFrom);
                    dynamicParameters.Add("@paStoryTo", wbs.StoryTo);
                    dynamicParameters.Add("@paPart", wbs.Part);
                    dynamicParameters.Add("@paProductType", wbs.ProductType);
                    dynamicParameters.Add("@paStructure", wbs.Structure);
                    dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);
                    sqlConnection.Query<int>(SystemConstant.sp_WBS_Extension, dynamicParameters, commandType: CommandType.StoredProcedure);
                    Output = dynamicParameters.Get<int>("@Output");
                    sqlConnection.Close();


                }

                return Output;


            }
            catch (Exception e)
            {
                return Output;
            }

        }


        public  List<InvalidData_Get_dto> InvalidData(int GroupmarkId)
        {
            bool isSuccess = false;
            string BBSNO = "";

            IEnumerable<InvalidData_Get_dto> result;
            var sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            var dynamicParameters = new DynamicParameters();
            try
            {
                dynamicParameters.Add("@INTGroupmarkID", GroupmarkId);

                result = sqlConnection.Query<InvalidData_Get_dto>(SystemConstant.InvalidData_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                

                //isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result.ToList();
        }

        public async System.Threading.Tasks.Task Execute(string EmailFromAddress, string EmailFromName, string EmailTo, string EmailCc, string Subject, string Type, string Content)
        {
            #region GridMail - No more use
            ////string apiKey = Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY", EnvironmentVariableTarget.User);
            //string apiKey = "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";
            //var sg = new SendGridClient(apiKey);
            //string lEmailBK = "";
            ////Email from = new Email("test@example.com");
            ////string subject = "Hello World from the SendGrid CSharp Library!";
            ////Email to = new Email("test@example.com");
            ////Content content = new Content("text/plain", "Hello, Email!");

            ////Attachment attachment = new Attachment();
            ////attachment.Content = "TG9yZW0gaXBzdW0gZG9sb3Igc2l0IGFtZXQsIGNvbnNlY3RldHVyIGFkaXBpc2NpbmcgZWxpdC4gQ3JhcyBwdW12";
            ////attachment.Type = "application/pdf";
            ////attachment.Filename = "balance_001.pdf";
            ////attachment.Disposition = "attachment";
            ////attachment.ContentId = "Balance Sheet";
            ////mail.AddAttachment(attachment);


            ////attachment = new Attachment();
            ////attachment.Content = "BwdW";
            ////attachment.Type = "image/png";
            ////attachment.Filename = "banner.png";
            ////attachment.Disposition = "inline";
            ////attachment.ContentId = "Banner";
            ////mail.AddAttachment(attachment);

            ////Email from = new Email(EmailFromAddress);
            ////from.Name = EmailFromName;

            ////string subject = Subject;
            ////Content content = new Content(Type, Content);
            ////Email to = new Email(EmailTo);

            ////var laTo = EmailTo.Split(';').Distinct().ToArray();
            ////if (laTo.Length > 0)
            ////{
            ////    for (int i = 0; i < laTo.Length; i++)
            ////    {
            ////        Email lTo = new Email(laTo[i]);
            ////        Mail mail = new Mail(from, Subject, lTo, content);
            ////        dynamic response = await sg.client.mail.send.post(requestBody: mail.Get());
            ////    }
            ////}

            //var msg = new SendGridMessage();
            //msg.SetFrom(new EmailAddress(EmailFromAddress, EmailFromName));

            //msg.SetSubject(Subject);
            //if (Type == "text/html")
            //{
            //    msg.AddContent(MimeType.Html, Content);
            //}
            //else
            //{
            //    msg.AddContent(MimeType.Text, Content);
            //}

            //if (EmailTo == null)
            //{
            //    EmailTo = "";
            //}
            //if (EmailCc == null)
            //{
            //    EmailCc = "";
            //}

            //EmailTo = EmailTo.Trim();
            //EmailCc = EmailCc.Trim();

            //if (EmailTo.Length > 0 || EmailCc.Length > 0)
            //{
            //    if (EmailTo.Length == 0 && EmailCc.Length > 0)
            //    {
            //        EmailTo = EmailCc;
            //        EmailCc = "";
            //    }

            //    var laTo = EmailTo.Split(';').Distinct().ToArray();
            //    if (laTo.Length > 0)
            //    {
            //        for (int i = 0; i < laTo.Length; i++)
            //        {
            //            if (laTo[i].Length > 0 && (lEmailBK == "" || lEmailBK.IndexOf(laTo[i]) < 0))
            //            {
            //                msg.AddTo(new EmailAddress(laTo[i]));
            //                lEmailBK = lEmailBK + ";" + laTo[i];
            //            }
            //        }
            //    }

            //    if (EmailCc.Length > 0)
            //    {
            //        var laCC = EmailCc.Split(';').Distinct().ToArray();
            //        if (laCC.Length > 0)
            //        {
            //            for (int i = 0; i < laCC.Length; i++)
            //            {
            //                if (laCC[i].Length > 0 && (lEmailBK == "" || lEmailBK.IndexOf(laCC[i]) < 0))
            //                {
            //                    msg.AddCc(new EmailAddress(laCC[i]));
            //                    lEmailBK = lEmailBK + ";" + laCC[i];
            //                }
            //            }
            //        }

            //    }
            //    var response = await sg.SendEmailAsync(msg);
            //    var lReturnCode = response.StatusCode;
            //}

            #endregion

            //Microsoft Exchange Reast API
            //Web Credential does not work since 2023-04-06 8:00AM
            //ExchangeService myservice = new ExchangeService(ExchangeVersion.Exchange2013_SP1);
            //myservice.Credentials = new WebCredentials(sLoginID, sLoginPwd);

            // Using Microsoft.Identity.Client 4.22.0
            //var cca = ConfidentialClientApplicationBuilder
            //    //.Create(ConfigurationManager.AppSettings["appId"])
            //    //.WithClientSecret(ConfigurationManager.AppSettings["clientSecret"])
            //    //.WithTenantId(ConfigurationManager.AppSettings["tenantId"])
            //    .Create("3975ae35-567f-47f2-b270-52ebad3e92a0")
            //    .WithClientSecret("9IL8Q~AxyT53gtCSEoVS2IXZH_5yIfxPiBqRCaHD")
            //    .WithTenantId("e678edbd-9359-4ccb-9dfa-1c7641b172d8")
            //    .Build();

            // Configure the MSAL client to get tokens
            var pcaOptions = new PublicClientApplicationOptions
            {
                ClientId = "3975ae35-567f-47f2-b270-52ebad3e92a0",
                TenantId = "e678edbd-9359-4ccb-9dfa-1c7641b172d8"
            };

            var pca = PublicClientApplicationBuilder
                .CreateWithApplicationOptions(pcaOptions).Build();

            var ewsScopes = new string[] { "https://outlook.office365.com/.default" };
            //var ewsScopes = new string[] { "https://outlook.office365.com/EWS.AccessAsUser.All" };
            //var ewsScopes = new List<string> { "https://graph.microsoft.com/Mail.Read" };

            try
            {
                //var authResult = await cca.AcquireTokenForClient(ewsScopes).ExecuteAsync();
                //var authResult = await pca.AcquireTokenInteractive(ewsScopes).ExecuteAsync();

                IConfidentialClientApplication app;

                app = ConfidentialClientApplicationBuilder.Create("3975ae35-567f-47f2-b270-52ebad3e92a0")
                           .WithClientSecret("cx28Q~W2U~zy1EwWN-Hl87bAxVlF.4_9GFg.0a78")
                           .WithAuthority(new Uri($"https://login.microsoftonline.com/e678edbd-9359-4ccb-9dfa-1c7641b172d8"))
                .Build();

                //var lAC = await app.GetAccountsAsync();

                var result = app.AcquireTokenForClient(ewsScopes)
                                  .ExecuteAsync().Result;

                //var client = PublicClientApplicationBuilder.Create("3975ae35-567f-47f2-b270-52ebad3e92a0")
                //.WithClientSecret("9IL8Q~AxyT53gtCSEoVS2IXZH_5yIfxPiBqRCaHD")
                //.WithAuthority(new Uri($"https://login.microsoftonline.com/e678edbd-9359-4ccb-9dfa-1c7641b172d8"))
                //.Build();

                //var request = client.AcquireTokenByUsernamePassword(ewsScopes, sLoginID, sLoginPwd);
                //var result = request.ExecuteAsync().Result;

                // Configure the ExchangeService with the access token
                var ewsClient = new ExchangeService();
                ewsClient.Url = new Uri("https://outlook.office365.com/EWS/Exchange.asmx");

                ewsClient.Credentials = new OAuthCredentials(result.AccessToken);

                ewsClient.ImpersonatedUserId =
                    new ImpersonatedUserId(ConnectingIdType.SmtpAddress, sLoginID);

                ////Include x-anchormailbox header
                ewsClient.HttpHeaders.Add("X-AnchorMailbox", sLoginID);

                //Include Token header
                //ewsClient.HttpHeaders.Add("Authorization", "bearer " + result.AccessToken);
                //ewsClient.HttpHeaders.Add("appid", "3975ae35-567f-47f2-b270-52ebad3e92a0");
                //ewsClient.HttpHeaders.Add("roles", "3975ae35-567f-47f2-b270-52ebad3e92a0");

                string lEmailBK = "";

                //string serviceUrl = "https://outlook.office365.com/EWS/Exchange.asmx";
                //myservice.Url = new Uri(serviceUrl);

                EmailMessage emailMessage = new EmailMessage(ewsClient);

                emailMessage.Subject = Subject;
                emailMessage.Body = new MessageBody(Content);

                EmailTo = EmailTo.Trim();
                EmailCc = EmailCc.Trim();

                if (EmailTo.Length > 0 || EmailCc.Length > 0)
                {
                    if (EmailTo.Length == 0 && EmailCc.Length > 0)
                    {
                        EmailTo = EmailCc;
                        EmailCc = "";
                    }

                    var laTo = EmailTo.Split(';').Distinct().ToArray();
                    if (laTo.Length > 0)
                    {
                        for (int i = 0; i < laTo.Length; i++)
                        {
                            laTo[i] = laTo[i].Trim();
                            if (laTo[i].Length > 5 && laTo[i].IndexOf("@") > 0 && laTo[i].IndexOf(".") > 0
                                && laTo[i].Substring(0, 1) != "@"
                                && laTo[i].Substring(0, 1) != "."
                                && laTo[i].Substring(laTo[i].Length - 1, 1) != "@"
                                && laTo[i].Substring(laTo[i].Length - 1, 1) != "."
                                && (lEmailBK == "" || lEmailBK.IndexOf(laTo[i]) < 0))
                            {
                                emailMessage.ToRecipients.Add(laTo[i]);
                                lEmailBK = lEmailBK + ";" + laTo[i];
                            }
                        }
                    }

                    if (EmailCc.Length > 0)
                    {
                        var laCC = EmailCc.Split(';').Distinct().ToArray();
                        if (laCC.Length > 0)
                        {
                            for (int i = 0; i < laCC.Length; i++)
                            {
                                laCC[i] = laCC[i].Trim();
                                if (laCC[i].Length > 5 && laCC[i].IndexOf("@") > 0 && laCC[i].IndexOf(".") > 0
                                    && laCC[i].Substring(0, 1) != "@"
                                    && laCC[i].Substring(0, 1) != "."
                                    && laCC[i].Substring(laCC[i].Length - 1, 1) != "@"
                                    && laCC[i].Substring(laCC[i].Length - 1, 1) != "."
                                    && (lEmailBK == "" || lEmailBK.IndexOf(laCC[i]) < 0))
                                {
                                    emailMessage.CcRecipients.Add(laCC[i]);
                                    lEmailBK = lEmailBK + ";" + laCC[i];
                                }
                            }
                        }
                    }
                    if (emailMessage.ToRecipients.Count > 0 || emailMessage.CcRecipients.Count > 0)
                    {
                        //emailMessage.Send();
                        emailMessage.SendAndSaveCopy();
                    }
                }

            }
            catch (Exception exception)
            {
                string lExchMsg = "Mail cannot be sent(AutodiscoverRemoteException):";
                lExchMsg += exception.Message;
                //throw new Exception(lExchMsg);
            }

        }

        public int Drawing_Approve(bool status,int wbsId)
        {
            int Output = 0;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@Status", status);
                    dynamicParameters.Add("@WBSID", wbsId);

                    sqlConnection.QueryFirstOrDefault<int>(SystemConstant.DrawingStatus, dynamicParameters, commandType: CommandType.StoredProcedure);
                    //Output = dynamicParameters.Get<int>("@Output");
                    Output = 1;
                    sqlConnection.Close();
                    return Output;
                }

            }
            catch (Exception e)
            {
                return Output;
            }

        }

        public  int IsSubmitted_Drawing(Insert_drawing_details lst)
        {
            try
            {


            
                    using (var sqlConnection = new SqlConnection(connectionString))
                    {

                        sqlConnection.Open();
                    foreach (var obj in lst.drawingSubmissions)
                    {


                        var dynamicParameters = new DynamicParameters();
                        dynamicParameters.Add("@intDrawingNo", obj.DrawingNo); // intDrawingNo
                        dynamicParameters.Add("@vchCustomerCode", obj.CustomerCode); // vchCustomerCode
                        dynamicParameters.Add("@vchProjectCode", obj.ProjectCode); // vchProjectCode
                        dynamicParameters.Add("@vchFileName", obj.FileName); // vchFileName
                        dynamicParameters.Add("@intRevision", obj.Revision); // intRevision
                        dynamicParameters.Add("@vchDetailer_Remark", obj.DetailerRemark); // vchDetailer_Remark
                        dynamicParameters.Add("@vchCustomer_Remark", obj.CustomerRemark); // vchCustomer_Remark
                        dynamicParameters.Add("@intWBSElementID", obj.WBSElementID); // intWBSElementID
                        dynamicParameters.Add("@vchSubmitDate", obj.SubmitDate); // vchSubmitDate
                        dynamicParameters.Add("@vchSubmitBy", obj.SubmitBy); // vchSubmitBy
                        dynamicParameters.Add("@vchApprovedBy", obj.ApprovedBy); // vchApprovedBy
                        dynamicParameters.Add("@vchApprovedDate", obj.ApprovedDate); // vchApprovedDate
                        dynamicParameters.Add("@IsApproved", obj.IsApproved); // IsApproved
                        dynamicParameters.Add("@IsSubmitted", obj.IsSubmitted); // IsSubmitted
                        dynamicParameters.Add("@IsSubmitMail", obj.IsSubmitMail); // IsSubmitMail
                        dynamicParameters.Add("@IsApprovedMail", obj.IsApprovedMail); // IsApprovedMail
                        dynamicParameters.Add("@isDeleted", obj.IsDeleted); // isDeleted

                        sqlConnection.Query<int>(SystemConstant.Insert_Drawing_posting, dynamicParameters, commandType: CommandType.StoredProcedure);

                    }

                    return 1;
                    
                    }
              
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public List<User_Drawing_get> User_drawing_get(string projectcode)
        {
            IEnumerable<User_Drawing_get> list = null;
            try
            {



                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@ProjectCode", projectcode); // intDrawingNo   

                    list = sqlConnection.Query<User_Drawing_get>(SystemConstant.Get_Drawing_Data, dynamicParameters, commandType: CommandType.StoredProcedure);

                    foreach (var item in list)
                    {
                        List<DrawingData> lst = Drawing_list_get(item.intWBSElementID);
                        
                        item.Drawing_List = lst;
                    }

                    return list.ToList();

                }

            }
            catch (Exception ex)
            {
                return new List<User_Drawing_get>();
            }
        }

        public List<DrawingData> Drawing_list_get(int Wbsid)
        {
            IEnumerable<DrawingData> list = null;
            try
            {



                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@WBSElementId", Wbsid); // intDrawingNo   

                    list = sqlConnection.Query<DrawingData>(SystemConstant.Get_Drawing_Details_Data, dynamicParameters, commandType: CommandType.StoredProcedure);

                

                    return list.ToList();

                }

            }
            catch (Exception ex)
            {
                return new List<DrawingData>();
            }
        }
        public bool  Edit_user_review(string UserReview,int DrawingId)
        {
            IEnumerable<User_Drawing_get> list = null;
            try
            {



                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    //var dynamicParameters = new DynamicParameters();
                    //dynamicParameters.Add("@UserReview", UserReview); // intDrawingNo   

                    //list = sqlConnection.Query<User_Drawing_get>(SystemConstant.Get_Drawing_Data, dynamicParameters, commandType: CommandType.StoredProcedure);

                    //foreach (var item in list)
                    //{
                    //    List<DrawingData> lst = Drawing_list_get(item.intWBSElementID);

                    //    item.Drawing_List = lst;


                    //}
                    string query = "update  ODDrawingsUploadDetails set vchCustomer_Remark= '" +  UserReview  + "' WHERE intDrawingId = "+ DrawingId;

                    //return list.ToList();
                    using (SqlCommand command = new SqlCommand(query, sqlConnection))
                    {
                        // Execute the query and retrieve the data using SqlDataReader
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                        
                        }
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<DrawingDetails> Get_docs_table(int OrderNumber, string StructureElement, string ProductType, string ScheduledProd)
        {
            List<DrawingDetails> list = new List<DrawingDetails>();
            try
          {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    //var dynamicParameters = new DynamicParameters();
                    //dynamicParameters.Add("@UserReview", UserReview); // intDrawingNo   

                    //list = sqlConnection.Query<User_Drawing_get>(SystemConstant.Get_Drawing_Data, dynamicParameters, commandType: CommandType.StoredProcedure);

                    //foreach (var item in list)
                    //{
                    //    List<DrawingData> lst = Drawing_list_get(item.intWBSElementID);

                    //    item.Drawing_List = lst;


                    //}
                    string query = "SELECT W.DrawingID, " +

                    "W.DrawingNo, " +

                    "W.FileName, " +

                    "O.Revision, " +

                    "O.Remark, " +

                    // "W.UpdatedDate, " +

                    // "W.UpdatedBy " +

                    "O.UpdatedDate," +

                    "O.UpdatedBy, " +

                    "W.Comments_User, " +

                    "W.Approved_Status, " +

                    "W.Comment_Customer, " +

                    "O.FileSubmitedBy, " +

                    "O.DetailerSubmissionCount, " +

                    "O.CustomerSubmissionCount " +

                    "FROM dbo.OESDrawings_posting W, dbo.OESDrawingsOrder_posting O " +

                    "WHERE W.DrawingID = O.DrawingID " +

                    "AND O.WBSElementId = " + OrderNumber.ToString() + " " +

                    "AND O.StructureElement = '" + StructureElement + "' " +

                    "AND O.ProductType = '" + ProductType + "' " +

                    "AND O.ScheduledProd = '" + ScheduledProd + "' " +

                    "ORDER BY W.DrawingID ";

                    //return list.ToList();
                    using (SqlCommand command = new SqlCommand(query, sqlConnection))
                    {
                        // Execute the query and retrieve the data using SqlDataReader
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read()) // Read one row at a time
                                {
                                    DrawingDetails obj = new DrawingDetails();
                                    // Access each column by name or index
                                    obj.DrawingID = reader.GetInt32(0);  // First column is DrawingID
                                    obj.DrawingNo = reader.GetString(1);  // Second column is DrawingNo
                                    obj.FileName = reader.GetString(2);  // Third column is FileName
                                    obj.Revision = reader.GetInt32(3);  // Fourth column is Revision
                                    obj.Remarks = reader.IsDBNull(4) ? null : reader.GetString(4);  // Fifth column is Remarks
                                    obj.UpdatedDate = reader.GetDateTime(5);  // Sixth column is UpdatedDate
                                    obj.UpdatedBy = reader.GetString(6);  // Seventh column is UpdatedBy
                                    obj.Approved_Status = reader.GetBoolean(8);  // Ninth column is Approved_Status
                                    obj.Comment_Customer = reader.IsDBNull(9) ? null : reader.GetString(9);
                                    obj.FileSubmitedBy = reader.IsDBNull(10) ? null : reader.GetString(10);
                                    obj.DetailerSubmissionCount = reader.IsDBNull(11) ? null: reader.GetInt32(11);  // First column is DrawingID
                                    obj.CustomerSubmissionCount = reader.IsDBNull(12) ? null : reader.GetInt32(12);  // First column is DrawingID




                                    // Process the data (for example, add to a list, print values, etc.)
                                    list.Add(obj);
                                }
                            }
                        }
                    }

                }
                return list;
            }
            catch (Exception ex)
            {
                return new List<DrawingDetails>();
            }
        }

        public async Task<IEnumerable<GetEmailDetails>> GetUserNamesByCustomerAndProject(string pCustomerCode, string pProjectCode,string wbsElementIds)

        {

            IEnumerable<GetEmailDetails> Customerlist; //new IEnumerable<List<ProjectMaster>>();

            try

            {

                using (var sqlConnection = new SqlConnection(connectionString))

                {

                    sqlConnection.Open();

                    // sqlConnection.CommandTimeout = 300;  // Set timeout to 5 minutes

                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.Add("@CustomerCode", pCustomerCode);

                    dynamicParameters.Add("@ProjectCode", pProjectCode);

                    dynamicParameters.Add("@WBSElementID", wbsElementIds); 


                    Customerlist = sqlConnection.Query<GetEmailDetails>(SystemConstant.usp_DetailingDWGNotificationList, dynamicParameters, commandType: CommandType.StoredProcedure);

                    sqlConnection.Close();

                    return Customerlist;

                }

            }

            catch (Exception ex)

            {

                //return false;

                throw ex;

            }


        }

        public async Task<List<GetEmailDetails>> GetDetailersByCustomerAndProject(string pCustomerCode, string pProjectCode, int wbselementID)
        {
            List<GetEmailDetails> Customerlist = new List<GetEmailDetails>(); //new IEnumerable<List<ProjectMaster>>();

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    // sqlConnection.CommandTimeout = 300;  // Set timeout to 5 minutes
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@CustomerCode", pCustomerCode);
                    dynamicParameters.Add("@ProjectCode", pProjectCode);
                    dynamicParameters.Add("@WbselementId", wbselementID);
                    Customerlist = sqlConnection.Query<GetEmailDetails>(SystemConstant.usp_CustomerDWGNotificationList, dynamicParameters, commandType: CommandType.StoredProcedure).ToList();
                    sqlConnection.Close();
                    return Customerlist;

                }

            }

            catch (Exception ex)

            {

                //return false;

                throw ex;

            }


        }

        public async Task<int> UpdateDrawingApprovalStatus(DrawingApprovalDto drawingApprovalDto)

        {

            int Output = 0;

            try

            {

                using (var sqlConnection = new SqlConnection(connectionString))

                {

                    sqlConnection.Open();

                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.Add("@ProjectCode", drawingApprovalDto.ProjectCode);

                    dynamicParameters.Add("@WBSElementID", drawingApprovalDto.WBSElementID);

                    dynamicParameters.Add("@CustRemark", drawingApprovalDto.CustRemark);

                    dynamicParameters.Add("@ApprovedBy", drawingApprovalDto.ApprovedBy);

                    dynamicParameters.Add("@Status", drawingApprovalDto.Status);


                    

                    Output = sqlConnection.QueryFirstOrDefault<int>(SystemConstant.usp_UpdateDrawingApprovalStatus, dynamicParameters, commandType: CommandType.StoredProcedure);

                    sqlConnection.Close();

                }

            }

            catch (Exception ex)

            {

                throw ex;

            }

            finally

            {

            }

            return Output;

        }

        public async Task<int> UpdateMailSubmitStatus(string ProjectCode, int WBSElementID)

        {

            int Output = 0;

            try

            {

                using (var sqlConnection = new SqlConnection(connectionString))

                {

                    sqlConnection.Open();

                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.Add("@ProjectCode", ProjectCode);

                    dynamicParameters.Add("@WBSElementID", WBSElementID);

                    Output = sqlConnection.QueryFirstOrDefault<int>(SystemConstant.usp_UpdateMailSentStatus, dynamicParameters, commandType: CommandType.StoredProcedure);

                    sqlConnection.Close();

                }

            }

            catch (Exception ex)

            {

                throw ex;

            }

            finally

            {

            }

            return Output;

        }

        public int IsSubmitted_Drawing_NEw(DrawingSubmission_New obj)
        {
            try
            {



                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
               

                        var dynamicParameters = new DynamicParameters();
                        dynamicParameters.Add("@vchCustomerCode", obj.CustomerCode); // @vchCustomerCode
                        dynamicParameters.Add("@vchProjectCode", obj.ProjectCode); // @vchProjectCode
                        dynamicParameters.Add("@vchCustomer_Remark", obj.CustomerRemark); // @vchCustomer_Remark
                        dynamicParameters.Add("@intWBSElementID", obj.WBSElementID); // @intWBSElementID
                        dynamicParameters.Add("@vchSubmitDate", obj.SubmitDate); // @vchSubmitDate
                        dynamicParameters.Add("@vchSubmitBy", obj.SubmitBy); // @vchSubmitBy
                        dynamicParameters.Add("@vchApprovedBy", obj.ApprovedBy); // @vchApprovedBy
                        dynamicParameters.Add("@vchApprovedDate", obj.ApprovedDate); // @vchApprovedDate
                        dynamicParameters.Add("@IsApproved", obj.IsApproved); // @IsApproved
                        dynamicParameters.Add("@IsSubmitted", obj.IsSubmitted); // @IsSubmitted
                        dynamicParameters.Add("@IsSubmitMail", obj.IsSubmitMail); // @IsSubmitMail
                        dynamicParameters.Add("@IsApprovedMail", obj.IsApprovedMail); // @IsApprovedMail
                        dynamicParameters.Add("@isDeleted", obj.IsDeleted); // @isDeleted
                        dynamicParameters.Add("@WBS1", obj.WBS1); // @WBS1
                        dynamicParameters.Add("@WBS2", obj.WBS2); // @WBS2
                        dynamicParameters.Add("@WBS3", obj.WBS3);
                        dynamicParameters.Add("@structureElement", obj.StructureElement);
                        dynamicParameters.Add("@ProductType", obj.ProductType);
                        dynamicParameters.Add("@DrawingStatus", obj.DrawingStatus);

                    sqlConnection.Query<int>(SystemConstant.InsertODDrawingsUploadDetails_New, dynamicParameters, commandType: CommandType.StoredProcedure);

                    

                    return 1;

                }

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public List<User_Drawing_get_new> User_drawing_get_new(string projectcode)
        {
            IEnumerable<User_Drawing_get_new> list = null;
            try
            {



                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@ProjectCode", projectcode); // intDrawingNo   

                    list = sqlConnection.Query<User_Drawing_get_new>(SystemConstant.Get_Drawing_Data_New, dynamicParameters, commandType: CommandType.StoredProcedure);

                    foreach (var item in list)
                    {
                        List<DrawingDetails> lst = Get_docs_table(item.intWBSElementID, item.StructureElement, item.ProductType, "N");

                        item.Drawing_List = lst;
                    }

                    return list.ToList();

                }

            }
            catch (Exception ex)
            {
                return new List<User_Drawing_get_new>();
            }
        }


        public List<User_Drawing_get_new> Submission_status(int wbsId)
        {
            IEnumerable<User_Drawing_get_new> list = null;
            try
            {



                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@wbsId", wbsId);   

                    list = sqlConnection.Query<User_Drawing_get_new>(SystemConstant.Get_submission, dynamicParameters, commandType: CommandType.StoredProcedure);

               

                    return list.ToList();

                }

            }
            catch (Exception ex)
            {
                return new List<User_Drawing_get_new>();
            }
        }

        public bool ModifyDrawing(ModifyDrawing obj,out string errorMessae)
        {
            List<DrawingDetails> list = new List<DrawingDetails>();
            errorMessae = "";
            try
            {



                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();

                    string query = "UPDATE dbo.OESDrawingsOrder_Posting " +                                 
                                  " SET Remark = '" + obj.Remarks + "' " +
                                  "WHERE DrawingID = " + obj.DrawingID + " " +
                                   " AND Revision = " + obj.Revision + " "; 

                    //return list.ToList();
                    using (SqlCommand command = new SqlCommand(query, sqlConnection))
                    {
                        // Execute the query and retrieve the data using SqlDataReader
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                        }
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                errorMessae = ex.Message;
                return false;
            }
        }

        public bool modifyDrawing_Status(int WBSElementId, out string errorMessae)
        {
            List<DrawingDetails> list = new List<DrawingDetails>();
            errorMessae = "";
            try
            {



                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();

                    string query = "UPDATE dbo.ODDrawingsUploadDetails_New " +
                                  "SET DrawingStatus = 'Reviewed'" +
                                  "WHERE intWBSElementID = " + WBSElementId + " ";

                    //return list.ToList();
                    using (SqlCommand command = new SqlCommand(query, sqlConnection))
                    {
                        // Execute the query and retrieve the data using SqlDataReader
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                        }
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                errorMessae = ex.Message;
                return false;
            }
        }

        public List<DrawingReportDto> GetDrawingReport(string customerCode, string projectCode, string fromDate, string toDate)
        {

            IEnumerable<DrawingReportDto> esmTonnageReports;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@CustomerCode", customerCode);
                    dynamicParameters.Add("@ProjectCode", projectCode);
                    dynamicParameters.Add("@FromDate", fromDate);
                    dynamicParameters.Add("@ToDate", toDate);

                    esmTonnageReports = sqlConnection.Query<DrawingReportDto>(SystemConstant.GetDrawingReportData, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return esmTonnageReports.ToList();


                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CopyDrawingDetails(List<DrawingCopyDto> items,int drawingId,out string errorMessage)
        {

            int Output = 0;
            errorMessage = "";

            int Count = items.Count;

            try

            {

                using (SqlConnection sqlConnection = new SqlConnection(connectionString))

                {

                    sqlConnection.Open();

                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.Add("@DrawingID", drawingId);

                    dynamicParameters.Add("@Count", Count);

                    //dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);


                    IEnumerable<int> result1 = sqlConnection.Query<int>(SystemConstant.usp_CopyDrawingRecord, dynamicParameters, commandType: CommandType.StoredProcedure);
                    //Output = dynamicParameters.Get<int>("@Output");
                    //int DrawingId_Old = drawingId;
                    List<int> result_new = result1.ToList();
                    int i = 0;

                    foreach (DrawingCopyDto drawingCopy in items)
                    {
                        var dynamicParameters_new = new DynamicParameters();

                        dynamicParameters_new.Add("@WBSElementId", drawingCopy.WBSElementId);

                        dynamicParameters_new.Add("@DrawingID", result_new[i]);

                        dynamicParameters_new.Add("@StructureElementID", drawingCopy.StructureElement);

                        dynamicParameters_new.Add("@ProductTypeID", drawingCopy.ProductType);

                        dynamicParameters_new.Add("@DrawingID_OLD", drawingId);



                        var result = sqlConnection.QueryFirstOrDefault<int>(SystemConstant.usp_ADD_Drawing_Record_Multiple, dynamicParameters_new, commandType: CommandType.StoredProcedure);

                        i = +1;    
                    }

                    

                    sqlConnection.Close();

                }
                return true;
            }

            catch (Exception ex)

            {
                errorMessage = ex.Message;
                return false;

            }
        }
        public int GetIsPreCast(int postheaderID)
        {
            try
            {



                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();


                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@postHeaderID", postheaderID);
                    var result = sqlConnection.Query<int>(SystemConstant.GetIsPreCast, dynamicParameters, commandType: CommandType.StoredProcedure);


                    int res = result.FirstOrDefault();
                    return res;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool GetFileSubmissionStatus(string customercode, string projectcode, string filename, int wbselementid,out string errorMessage)
        {
             errorMessage = "";
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    DynamicParameters dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@customercode", customercode);
                    dynamicParameters.Add("@projectcode", projectcode);
                    dynamicParameters.Add("@filename", filename);
                    dynamicParameters.Add("@wbselement", wbselementid);
                    var result = sqlConnection.Query<int>(SystemConstant.GetFileSubmissionStatus, dynamicParameters, commandType: CommandType.StoredProcedure);

                    int count = result.FirstOrDefault();
                    bool result_new = count > 0 ? true : false; //returns isDuplicate 
                    return result_new;
                }

            }
            catch(Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }

        }

        public async Task<ResponseDTO<string>> MigrateWBSReleaseData()
        {
            List<GetSORDetails_Migration> list = new List<GetSORDetails_Migration>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    string result = "select * from AutoReleaseCutoverData where status=0";
                    list = sqlConnection.Query<GetSORDetails_Migration>(result).ToList();
                    if (list.Count > 0)
                    {
                        int Releaseby = GetUserIDByName("JagdishH_TTL");
                        foreach (GetSORDetails_Migration item in list)
                        {                       

                            AddBBSReleaseDto objNew = new AddBBSReleaseDto
                            {
                                SalesOrderId = item.SOR.ToString() + 'i',
                                PostHeaderId = item.PostHeaderId.ToString() + 'i',
                                BBSStatusId = "R",
                                UserName = "JagdishH_TTL",
                                Releaseby = Releaseby
                            };
                            int status = await AddBBSReleaseAsync(objNew);
                            if (status == 0)
                                continue;

                            var dynamicParameters = new DynamicParameters();
                            dynamicParameters.Add("@SOR", item.SOR);
                            dynamicParameters.Add("@UpdatedStatus", null, dbType: DbType.Int32, ParameterDirection.Output);
                            int updatedStatus = sqlConnection.Query<int>(SystemConstant.Update_ReleaseCutoverData_status, dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                        }
                    }

                    sqlConnection.Close();
                    ResponseDTO<string> obj = new ResponseDTO<string>
                    {
                        IsSuccess = true,
                        Message = "ALl BBS Released Successfully"
                    };
                    return obj;
                

                }


            }
            catch(Exception ex)
            {
                return new ResponseDTO<string> { IsSuccess=false,ErrorMessage=ex.Message};
            }
           
        }


    }
}