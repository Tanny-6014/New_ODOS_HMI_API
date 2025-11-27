using DetailingService.Context;
using DetailingService.Interfaces;
using Microsoft.EntityFrameworkCore;
using DetailingService.Constants;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using DetailingService.Dtos;
using DetailingService.Repositories;
using static Dapper.SqlMapper;



public class GroupMarkDAL : IGroupMarkDal
{
    private DetailingApplicationContext _dbContext;
    private readonly IConfiguration _configuration;
    //private string connectionString;
    //private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";
    private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";

    public GroupMarkDAL()
    {

    }

    public GroupMarkDAL(DetailingApplicationContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;

        connectionString = _configuration.GetConnectionString(SystemConstant.DefaultDBConnection);
    }





/// //////Added for Core Cage
    public List<ProductCode> GetProductCodeIdForGroupMark(int GroupMarkId)
    {
        List<ProductCode> productCodes = new List<ProductCode>();
       
        DataSet dsCarpetProductCode = new DataSet();

        try
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dataAdapter = new SqlDataAdapter();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intGroupMarkId", GroupMarkId);

                dataAdapter.SelectCommand = new SqlCommand(SystemConstant.CoreCageProductCodeID_Get, sqlConnection);
                dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                dataAdapter.SelectCommand.Parameters.AddRange(dynamicParameters.ParameterNames.Select(name => new SqlParameter(name, dynamicParameters.Get<object>(name))).ToArray());
                dataAdapter.Fill(dsCarpetProductCode);

                if (dsCarpetProductCode != null && dsCarpetProductCode.Tables.Count != 0)
                {
                    foreach (DataRowView drvBeam in dsCarpetProductCode.Tables[0].DefaultView)
                    {
                        ProductCode productCode = new ProductCode
                        {
                            ProductCodeId = Convert.ToInt32(drvBeam["intProductCodeId"])

                        };

                        productCodes.Add(productCode);
                    }
                }
            }  
            
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return productCodes;

    }



/// //End added for Core Cage




    // added for ship to party
    //public DataSet GetGroupMarkListing_new(GroupMarkInfo objGroupMarkInfo)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("GroupMarkListing_Get_new");
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intproducttypeid", DbType.Int32, objGroupMarkInfo.ProductId);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intProjectId", DbType.Int32, objGroupMarkInfo.intProjectId);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intStructureElementTypeId", DbType.Int32, objGroupMarkInfo.intStructureElementTypeId);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    // end added for ship to party




    public int UpdateGMINPostGM(GroupMarkInfo objGroupMarkInfo)
    {
        int output = 0;
        try
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@IDENTITYGM", objGroupMarkInfo.IdentityGMId);
                //dynamicParameters.Add("@identityGM", ParamSetNumber);
                dynamicParameters.Add("@tntGroupRevNo", objGroupMarkInfo.GMRevNo);
                dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);
                output = sqlConnection.QueryFirstOrDefault(SystemConstant.UpdateGM_PostGM, dynamicParameters, commandType: CommandType.StoredProcedure);
                return output;

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public async Task<int> DelGroupMarkAsync(int INTGROUPMARKID)
    {
        string ErrorMsg = "";
        try
        {
            int Output = 0;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intGroupMarkId", INTGROUPMARKID);
                dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);



                sqlConnection.Query<GetGroupMarkListDto>(SystemConstant.DeleteGroupmark_GroupMarId, dynamicParameters, commandType: CommandType.StoredProcedure);
                Output = dynamicParameters.Get<int>("@Output");



                sqlConnection.Close();
                return await _dbContext.SaveChangesAsync(); ;



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
    //public async Task<IEnumerable<GetGroupMarkListDto>> GetGroupMarkListing(GroupMarkInfo objGroupMarkInfo)
    //{
    //    try
    //    {
    //        IEnumerable<GetGroupMarkListDto> ProjContractlist; //new IEnumerable<List<ProjectMaster>>();

    //        using (var sqlConnection = new SqlConnection(connectionString))
    //        {

    //            sqlConnection.Open();
    //            var dynamicParameters = new DynamicParameters();
    //            dynamicParameters.Add("@intProjectId", objGroupMarkInfo.intProjectId);
    //            dynamicParameters.Add("@intStructureElementTypeId", objGroupMarkInfo.intStructureElementTypeId);
    //            ProjContractlist = sqlConnection.Query<GetGroupMarkListDto>(SystemConstant.GroupMarkListing_Get_new, dynamicParameters, commandType: CommandType.StoredProcedure);
    //            sqlConnection.Close();
    //            return ProjContractlist;


    //        }


          
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    public List<GetPostedGMDto> GetPostedGroupMark(int intGroupMarkid)
    {

        List<GetPostedGMDto> getPostedGMDtos;
        try
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intGroupMarkid", intGroupMarkid);



                getPostedGMDtos = (List<GetPostedGMDto>)sqlConnection.Query<GetPostedGMDto>(SystemConstant.GroupMarkingPostedCheck_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return getPostedGMDtos;



            }



        }
        catch (Exception ex)
        {
            throw ex;
        }



    }
    public List<GetReleasedGMDto> GetReleasedGroupMark(int intGroupMarkid)
    {
        List<GetReleasedGMDto> getReleasedGMDtos;
        try
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intGroupMarkid", intGroupMarkid);
                getReleasedGMDtos = (List<GetReleasedGMDto>)sqlConnection.Query<GetReleasedGMDto>(SystemConstant.GroupMarkingCheck_GET, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return getReleasedGMDtos;

            }



        }



        catch (Exception ex)
        {
            throw ex;
        }



    }

    //public int InsertBeamExcel(GroupMarkInfo objGroupMarkInfo)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ImportBeam_InsertExcel");
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@strStructureMarking", DbType.String, objGroupMarkInfo.StructureMarking);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@strProductMarking", DbType.String, objGroupMarkInfo.ProductMarking);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@strProductShapeDetails", DbType.String, objGroupMarkInfo.ProductShapeDetails);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intGroupMarkId", DbType.Int32, objGroupMarkInfo.GroupMarkId);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intUserId", DbType.Int32, objGroupMarkInfo.UserId);
    //        DataAccess.DataAccess.db.AddOutParameter(dbcom, "@Output", DbType.Int32, 0);
    //        DataAccess.DataAccess.GetScalar(dbcom);
    //        intReturnValue = (dbcom.Parameters["@Output"].Value);
    //        return intReturnValue;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}



    //public int ImportParameterCheck(GroupMarkInfo objGroupMarkInfo)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ImportParam_Validation");
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intParameterId", DbType.String, objGroupMarkInfo.ParameterId);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@strParameterDetails", DbType.String, objGroupMarkInfo.ParamterDetails);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intGroupMarkId", DbType.Int32, objGroupMarkInfo.GroupMarkId);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intUserId", DbType.Int32, objGroupMarkInfo.UserId);
    //        DataAccess.DataAccess.db.AddOutParameter(dbcom, "@Output", DbType.Int32, 0);
    //        DataAccess.DataAccess.GetScalar(dbcom);
    //        intReturnValue = (dbcom.Parameters["@Output"].Value);
    //        return intReturnValue;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}



    //public int InsertColumnExcel(GroupMarkInfo objGroupMarkInfo)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ImportColumn_InsertExcel");
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@strStructureMarking", DbType.String, objGroupMarkInfo.StructureMarking);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@strProductMarking", DbType.String, objGroupMarkInfo.ProductMarking);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@strProductShapeDetails", DbType.String, objGroupMarkInfo.ProductShapeDetails);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intGroupMarkId", DbType.Int32, objGroupMarkInfo.GroupMarkId);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intUserId", DbType.Int32, objGroupMarkInfo.UserId);
    //        DataAccess.DataAccess.db.AddOutParameter(dbcom, "@Output", DbType.Int32, 0);
    //        DataAccess.DataAccess.GetScalar(dbcom);
    //        intReturnValue = (dbcom.Parameters["@Output"].Value);
    //        return intReturnValue;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}



    public List<GroupMarkInfo> GetProductType(int intProductType, int intGroupMarkId)
    {
        try
        {
            List<GroupMarkInfo>groupMarkInfos;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intProductType", intProductType);
                dynamicParameters.Add("@intGroupMarkId", intGroupMarkId);
                groupMarkInfos = (List<GroupMarkInfo>)sqlConnection.Query<GroupMarkInfo>(SystemConstant.CABProductType_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return groupMarkInfos;

            }



            //dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CABProductType_Get");
            //DataAccess.DataAccess.db.AddInParameter(dbcom, "@intProductType ", DbType.Int32, objGroupMarkInfo.ProductId);
            //DataAccess.DataAccess.db.AddInParameter(dbcom, "@intGroupMarkId", DbType.Int32, objGroupMarkInfo.GroupMarkId);
            //return DataAccess.DataAccess.GetDataSet(dbcom);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public GetGmDetailsForCall CallStructMarkListbyGMID(int GMID)
    {
        DataSet dsGroupMarkDetails = new DataSet();

        IEnumerable<GetGmDetailsForCall> GetGroupmarkingDetails;
        GetGmDetailsForCall groupMarkList = new GetGmDetailsForCall();
        try
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();

                dynamicParameters.Add("@GmID", GMID);
                // dsGroupMarkDetails = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_CopyGMGroupmarking_Get");
                GetGroupmarkingDetails = sqlConnection.Query<GetGmDetailsForCall>(SystemConstant.get_Groupmarking_Data, dynamicParameters, commandType: CommandType.StoredProcedure);
                groupMarkList = GetGroupmarkingDetails.ToList().FirstOrDefault();

                //if (groupMarkList.StructureElementTypeID == 4 && groupMarkList.StructureElementTypeID == 68 && groupMarkList.StructureElementTypeID == 69)
                //{
                //    GetStructureMarkingDetails()
                //}

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return groupMarkList;
    }






    //public int InsertExcel(GroupMarkInfo objGroupMarkInfo)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ImportSlab_InsertExcel"); // "ImportData_Insert")
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@strStructureMarking", DbType.String, objGroupMarkInfo.StructureMarking);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@strProductMarking", DbType.String, objGroupMarkInfo.ProductMarking);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@strProductShapeDetails", DbType.String, objGroupMarkInfo.ProductShapeDetails);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@strCAB", DbType.String, objGroupMarkInfo.CAB);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@strCABShapeDetails", DbType.String, objGroupMarkInfo.CABShapeDetails);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@strAccessories", DbType.String, objGroupMarkInfo.Accessories);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intGroupMarkId", DbType.Int32, objGroupMarkInfo.GroupMarkId);
    //        DataAccess.DataAccess.db.AddOutParameter(dbcom, "@Output", DbType.Int32, 0);
    //        DataAccess.DataAccess.GetScalar(dbcom);
    //        intReturnValue = (dbcom.Parameters["@Output"].Value);
    //        return intReturnValue;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}



    //public DataSet GetBeamExportToTxt(GroupMarkInfo objGroupMarkInfo)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ExportToTextBeam");
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@strGroupMarkId", DbType.String, objGroupMarkInfo.strGroupMarkId);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}



    //public DataSet GetSlabExportToTxt(GroupMarkInfo objGroupMarkInfo)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ExportToTextSlab");
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@strGroupMarkId", DbType.String, objGroupMarkInfo.strGroupMarkId);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

}