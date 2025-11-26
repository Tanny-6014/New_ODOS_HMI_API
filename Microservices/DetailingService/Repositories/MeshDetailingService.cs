
using DetailingService.Context;
using DetailingService.Interfaces;
using Microsoft.EntityFrameworkCore;
using DetailingService.Constants;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using DetailingService.Dtos;



namespace DetailingService.Repositories
{
    public class MeshDetailingService : IMeshDetailing
    {
        //TactonHelper_New objTactonHelper_new = new TactonHelper_New();

        private DetailingApplicationContext _dbContext;
        private readonly IConfiguration _configuration;
        //private string connectionString = "Server=NSQADB5\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=MES_USER;Password=Mes@123;Connection Timeout=36000000";
        private string connectionString;

        BeamDetailinfo objBeamDetailInfo = new BeamDetailinfo();

        public MeshDetailingService(DetailingApplicationContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString(SystemConstant.DefaultDBConnection);

        }
        public async Task<IEnumerable<GetGroupMarkListDto>> GetMeshDetailingListAsync(int ProjectID)
        {
            //return await _dbContext.ProjectMaster.ToListAsync();
            IEnumerable<GetGroupMarkListDto> ProjContractlist; //new IEnumerable<List<ProjectMaster>>();

            using (var sqlConnection = new SqlConnection(connectionString))
            {

                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@INTPROJECTID", ProjectID);              
                ProjContractlist = sqlConnection.Query<GetGroupMarkListDto>(SystemConstant.GroupMarkListing_Get_new, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return ProjContractlist;


            }

        }


        //added by vidya 
        public async Task<IEnumerable<ShapeCodeParameterSetDto>> SlabParameterSetbyProjIdProdType(int projectId, int productTypeId)
        {

            IEnumerable<ShapeCodeParameterSetDto> shapeCodeParameterSetDtos;// 
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@INTPROJECTID", projectId);
                dynamicParameters.Add("@SITPRODUCTTYPEID", productTypeId);
                shapeCodeParameterSetDtos = sqlConnection.Query<ShapeCodeParameterSetDto>(SystemConstant.SlabParameterSetbyProjIdProdType_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return shapeCodeParameterSetDtos;


            }

        }

        public List<BeamDetailinfo> PopulateHeaderByProjectId(int projectId)
        {

            List<BeamDetailinfo> beamDetailinfos = new List<BeamDetailinfo>();
            BeamDetailinfo ObjbeamDetailinfo = new BeamDetailinfo();
            try
            {

                beamDetailinfos = ObjbeamDetailinfo.CustContProjID_Get(projectId);
            }
            catch (Exception ex)
            {
                //Exception exception = ex;

            }
            finally
            {
                ObjbeamDetailinfo = null;
            }
            return beamDetailinfos;
        }

        public async Task<int> DeleteGroupMarkAsync(int INTGROUPMARKID)
        {
            string ErrorMsg = "";
            //IEnumerable<GetGroupMarkListDto> getGroupMarkLists;
            try
            {
                int Output = 0;
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intGroupMarkId", INTGROUPMARKID);
                    dynamicParameters.Add("@Output", 0);

                    sqlConnection.Query<int>(SystemConstant.DeleteGroupmark_GroupMarId, dynamicParameters, commandType: CommandType.StoredProcedure);
                    Output = dynamicParameters.Get<int>("@Output");
                    sqlConnection.Close();
                    

                }

                if (Output == 1)
                {
                    ErrorMsg = ("GroupMarkID :" + INTGROUPMARKID + " deleted successfully");
                }
                else if (Output == 2)
                {
                    ErrorMsg = ("Cannot delete the groupmarking.It has been posted already");
                }
                else
                {
                    BeamDetailinfo objBeamDetailInfo = new BeamDetailinfo();
                    string AttachedWBS = "";
                    objBeamDetailInfo.intGroupMarkId = INTGROUPMARKID;
                    AttachedWBS = objBeamDetailInfo.UnPostedWBSAttachedWithGroupMarking_Get();

                    ErrorMsg = "alert('Cannot delete the groupmarking." + AttachedWBS.ToString() + "' );";

                }

                return Output;

            }

            
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<IEnumerable<MeshStructureMarkingDetailsDto>> MeshStructureMarkingDetails_Get()
        {
            IEnumerable<MeshStructureMarkingDetailsDto> meshStructureMarkingDetails;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                meshStructureMarkingDetails = sqlConnection.Query<MeshStructureMarkingDetailsDto>(SystemConstant.MeshStructureMarkingDetails_Get, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return meshStructureMarkingDetails;



            }



        }


        public async Task<IEnumerable<MeshStructureMarkingDetailsDto>> CarpetStructureMarkingDetails_Get()
        {
            IEnumerable<MeshStructureMarkingDetailsDto> meshStructureMarkingDetails;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                meshStructureMarkingDetails = sqlConnection.Query<MeshStructureMarkingDetailsDto>(SystemConstant.CarpetStructureMarkingDetails_Get, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return meshStructureMarkingDetails;



            }



        }

        public async Task<IEnumerable<AddSlabProductMarkingDto>> MeshProductMarkingDetails_Get(int StructureMarkID)
        {



            IEnumerable<AddSlabProductMarkingDto> addSlabProducts;



            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@StructureMarkId", StructureMarkID);



                addSlabProducts = sqlConnection.Query<AddSlabProductMarkingDto>(SystemConstant.MeshStructMarkDetailsByStructMarkId_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return addSlabProducts;



            }



        }

        public int GroupMarkList_Edit(ReleaseGroupMarkDto groupMarkDto)
        {
            GroupMarkListing groupMarkListing = new GroupMarkListing();
            GroupMarkDAL groupMarkDAL = new GroupMarkDAL();
            List<GroupMarkInfo> groupMarkInfos = new List<GroupMarkInfo>();
            string errorMessage = "";
            int isEdit = 0;

            try
            {

                List<GetReleasedGMDto> getReleasedGMs = groupMarkDAL.GetReleasedGroupMark(groupMarkDto.INTGROUPMARKID);
                if (getReleasedGMs != null)
                {
                    
                    isEdit = groupMarkListing.CopyGroupMarking(groupMarkDto);
                }
                else
                {
                    GroupMarkInfo objGroupMarkInfo = new GroupMarkInfo();
                    isEdit = groupMarkDAL.UpdateGMINPostGM(objGroupMarkInfo);
                }
            }
            catch (Exception ex)
            {

                errorMessage = ex.Message;
            }
            return isEdit;
        }


        public bool SaveGroupMark(GroupMark groupmark, out int GMId, out string InserFlag, out string errorMessage)
        {
            bool isSuccuess = false;
            GMId = 0;
            InserFlag = "";
            errorMessage = "";
            try
            {
                isSuccuess = groupmark.SaveGroupMark();
                GMId = groupmark.GroupMarkId;
                InserFlag = groupmark.InsertFlag;

            }
            catch (Exception ex)
            {
               // ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
            }
            return isSuccuess;
        }

        #region Validate Posted GM

        public int ValidatedPostedGM(int GroupMarkId, out string errorMessage)
        {
           
            int intRecordCount = 0;
            errorMessage = "";
            try
            {
                objBeamDetailInfo.intGroupMarkId = Convert.ToInt32(GroupMarkId);
                intRecordCount = objBeamDetailInfo.PostedBeamByGroupmark_Get();

            }
            catch (Exception ex)
            {
                //ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
            }
            finally
            {
                objBeamDetailInfo = null;
            }
            return intRecordCount;
        }


        # endregion



    }
}
