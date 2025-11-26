using ParameterSetService.Models;
using ParameterSetService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Reflection.Metadata;
using System.Collections.Generic;
using ParameterSetService.Context;
using ParameterSetService.Constants;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Dapper;
using ParameterSetService.Dtos;

namespace ParameterSetService.Repositories
{
    public class ParameterSetRepository: IParameterSet
    {
        private ParameterSetServiceContext _dbContext;
        private readonly IConfiguration _configuration;
        private string connectionString;

        public ParameterSetRepository(ParameterSetServiceContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString(SystemConstants.DefaultDBConnection);

        }


        public async Task<int> CommonParamterInsert(AddParamDto addMeshParameter)
        {
            int Output = 0;
            // try
            // {
           

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();

                dynamicParameters.Add("@INTPROJECTID", addMeshParameter.ProjectId);
                dynamicParameters.Add("@PRODUCTTYPE", addMeshParameter.ProductType);
                dynamicParameters.Add("@INTUSERID", addMeshParameter.UserId);
                dynamicParameters.Add("@OUTPUT", null, dbType: DbType.Int32, ParameterDirection.Output);
                sqlConnection.Query<int>(SystemConstants.Common_InsertParamterSet, dynamicParameters, commandType: CommandType.StoredProcedure);
                Output = dynamicParameters.Get<int>("@OUTPUT");
                sqlConnection.Close();

            }
            return Output;


            //}
            //catch (Exception e)
            //{
            //    return null;
            //}



        }

        public async Task<IEnumerable<ParameterSetDropdownDto>> GetParameterSetAsync(int projectId)
        {
            IEnumerable<ParameterSetDropdownDto> parameterSetDropdowns;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@INTPROJECTID", projectId);
                parameterSetDropdowns = sqlConnection.Query<ParameterSetDropdownDto>(SystemConstants.GetParameterList, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return parameterSetDropdowns;

            }

        }


        #region Mesh Parameter
        //Insert Mesh Paramter



        public async Task<IEnumerable<MeshProductCodeDto>> GetMeshProductCodeAsync()
        {
            IEnumerable<MeshProductCodeDto> MeshProductCodeDropdowns;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                MeshProductCodeDropdowns = sqlConnection.Query<MeshProductCodeDto>(SystemConstants.MeshProductCode_Get, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return MeshProductCodeDropdowns;

            }

        }

        public async Task<int> MeshProjectParamLap(AddMeshParamaterDto addMeshParamDto)
        {
            try
            {
                int Output = 0;

                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();



                    dynamicParameters.Add("@INTPROJECTID", addMeshParamDto.ProjectId);
                    dynamicParameters.Add("@PARAMSETNUMBER", addMeshParamDto.Paramsetnumber);
                    dynamicParameters.Add("@TNTTRANSPORTMODEID", addMeshParamDto.TNTTRANSPORTMODEID);
                    dynamicParameters.Add("@PRODUCTTYPE", addMeshParamDto.ProductType);
                    dynamicParameters.Add("@INTPRODUCTCODEID", addMeshParamDto.ProductCodeId);
                    dynamicParameters.Add("@SITMWLAP", addMeshParamDto.Mwlap);
                    dynamicParameters.Add("@SITCWLAP", addMeshParamDto.Cwlap);
                    dynamicParameters.Add("@INTMESHLAPID", addMeshParamDto.IntMeshLapId);
                    dynamicParameters.Add("@INTUSERID", addMeshParamDto.UserId);
                    dynamicParameters.Add("@STRUCTUREELEMENTTYPEID", addMeshParamDto.StructureElementTypeId);
                    dynamicParameters.Add("@OutputParam", null, dbType: DbType.Int32, ParameterDirection.Output);
                    sqlConnection.Query<int>(SystemConstants.MeshProjectParamLap_InsUpd, dynamicParameters, commandType: CommandType.StoredProcedure);
                    Output = dynamicParameters.Get<int>("@OutputParam");
                    sqlConnection.Close();
                    return Output;
                }              


            }
            catch (Exception e)
            {
                return 0;
            }



        }

        //added by vidhya
        public async Task<IEnumerable<MeshListing>> GetMeshListAsync(int projectId, int parameternumber)
        {
            IEnumerable<MeshListing> meshListings;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@INTPROJECTID", projectId);
                dynamicParameters.Add("@parameternumber", parameternumber);
                meshListings = sqlConnection.Query<MeshListing>(SystemConstants.GetMeshList, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return meshListings;


            }

        }

        public async Task<IEnumerable<MeshListing>> GetWallListAsync(int projectId, int parameternumber)
        {
            IEnumerable<MeshListing> meshListings;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@INTPROJECTID", projectId);
                dynamicParameters.Add("@parameternumber", parameternumber);
                meshListings = sqlConnection.Query<MeshListing>(SystemConstants.usp_WallCWMWList, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return meshListings;


            }

        }


        public async Task<int> DeleteMesh(int intMeshLapId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@INTMESHLAPID", intMeshLapId);
                sqlConnection.Query<int>(SystemConstants.usp_MeshProjectParamLap_Delete, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return await _dbContext.SaveChangesAsync(); 
            }

        }

        public async Task<int> UpdateMeshParamLap(AddMeshParamaterDto addMeshParamDto)
        {
            try
            {
                int Output = 0;

                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTMESHLAPID", addMeshParamDto.IntMeshLapId);
                    dynamicParameters.Add("@INTPROJECTID", addMeshParamDto.ProjectId);
                    dynamicParameters.Add("@TNTPARAMSETNUMBER", addMeshParamDto.Paramsetnumber);
                    dynamicParameters.Add("@PRODUCTTYPE", addMeshParamDto.ProductType);
                    dynamicParameters.Add("@INTPRODUCTCODEID", addMeshParamDto.ProductCodeId);
                    dynamicParameters.Add("@SITMWLAP", addMeshParamDto.Mwlap);
                    dynamicParameters.Add("@SITCWLAP", addMeshParamDto.Cwlap);
                    dynamicParameters.Add("@INTUSERID", addMeshParamDto.UserId);
                    dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);
                    sqlConnection.Query<int>(SystemConstants.usp_MeshProjectParamLap_Update, dynamicParameters, commandType: CommandType.StoredProcedure);
                    Output = dynamicParameters.Get<int>("@Output");
                    sqlConnection.Close();
                    return Output;
                }


            }
            catch (Exception e)
            {
                return 0;
            }



        }

        //End
        #endregion





        #region Column
        public async Task<int> SaveColumnParameter(AddColumnParameterSet addColumnParamDto)
        {
            try
            {
                int Output = 0;

                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTProjectID", addColumnParamDto.ProjectId);
                    dynamicParameters.Add("@VCHPRODUCTTYPE", "MSH");                    
                    dynamicParameters.Add("@TNTPARAMSETNUMBER", addColumnParamDto.ColumnParameterSetNo);
                    dynamicParameters.Add("@TNTTRANSPORTMODEID", addColumnParamDto.TNTTRANSPORTMODEID);
                    dynamicParameters.Add("@SITTopCover", addColumnParamDto.ColumnTopCover);
                    dynamicParameters.Add("@SITBottomCover", addColumnParamDto.ColumnBottonCover);
                    dynamicParameters.Add("@SITRightCover", addColumnParamDto.ColumnRightCover);
                    dynamicParameters.Add("@SITLeftCover", addColumnParamDto.ColumnLeftCover);
                    dynamicParameters.Add("@INTUSERID", 1);

                    dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);
                    sqlConnection.Query<int>(SystemConstants.ColumnParameterSet_InsUpd, dynamicParameters, commandType: CommandType.StoredProcedure);
                    Output = dynamicParameters.Get<int>("@Output");
                    sqlConnection.Close();

                }

                return Output;


            }
            catch (Exception e)
            {
                return 0;
            }



        }

        public async Task<int> SaveClinkLineItem(AddClinkLineDto addClinkLineDto)
        {
            try
            {
                int Output = 0;

                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.Add("@tntParamCageId", addClinkLineDto.ParamCageId);
                    dynamicParameters.Add("@tntParamSetNumber", addClinkLineDto.ParameterSetNo);
                    dynamicParameters.Add("@intDiameter", addClinkLineDto.Diameter);
                    dynamicParameters.Add("@sitLeg", addClinkLineDto.Leg);
                    dynamicParameters.Add("@Length", addClinkLineDto.CWLength);
                    dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);
                    sqlConnection.Query<int>(SystemConstants.ClinkLineItem_InsUpd, dynamicParameters, commandType: CommandType.StoredProcedure);
                    Output = dynamicParameters.Get<int>("@Output");
                    sqlConnection.Close();
                    return Output;

                }

               


            }
            catch (Exception e)
            {
                return 0;
            }



        }

        public async Task<int> SaveCappingLineItem(AddCappingLineDto cappingLineDto)
        {
            try
            {
                int Output = 0;

                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.Add("@tntParamCageId", cappingLineDto.tntParamCageId);
                    dynamicParameters.Add("@tntParamSetNumber", cappingLineDto.tntParamSetNumber);
                    dynamicParameters.Add("@intDiameter", cappingLineDto.intDiameter);
                    dynamicParameters.Add("@sitLeg", cappingLineDto.sitLeg);
                    dynamicParameters.Add("@sitHook", cappingLineDto.Hook);
                    dynamicParameters.Add("@INTPRODUCTCODEID", cappingLineDto.CappingProductId);
                    dynamicParameters.Add("@Length", cappingLineDto.Length);
                    dynamicParameters.Add("@chrStandard", cappingLineDto.chrStandard);
                    dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);
                    sqlConnection.Query<int>(SystemConstants.CappingItem_InsUpd, dynamicParameters, commandType: CommandType.StoredProcedure);
                    Output = dynamicParameters.Get<int>("@Output");
                    sqlConnection.Close();
                    

                }

                return Output;


            }
            catch (Exception e)
            {
                return 0;
            }



        }

        public async Task<int> DeleteClinkLineItem(int ClinkParamcageID)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@PARAMCAGEID", ClinkParamcageID);
                sqlConnection.Query<int>(SystemConstants.ClinkParamCage_Del, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();

                return await _dbContext.SaveChangesAsync();
            }

        }

        public async Task<IEnumerable<ColumnListingDto>> GetColumnListAsync(int projectId, int parameternumber)
        {
            IEnumerable<ColumnListingDto> columnListingDtos;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@INTPROJECTID", projectId);
                dynamicParameters.Add("@tntParamSetNumber", parameternumber);
                columnListingDtos = sqlConnection.Query<ColumnListingDto>(SystemConstants.GetColumnParameterSetTable, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return columnListingDtos;




            }



        }
        public async Task<IEnumerable<ParameterSetDropdownDto>> GetColumnParameterSetAsync(int projectId)
        {
            IEnumerable<ParameterSetDropdownDto> parameterSetDropdowns;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@INTPROJECTID", projectId);
                parameterSetDropdowns = sqlConnection.Query<ParameterSetDropdownDto>(SystemConstants.GetColumnParameterList, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return parameterSetDropdowns;
            }
        }

        public async Task<IEnumerable<AddClinkLineDto>> GetClinkLineItemAsync(int projectId, int parameternumber)
        {
            IEnumerable<AddClinkLineDto> addClinkLineDto;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@INTPROJECTID", projectId);
                dynamicParameters.Add("@refparameternumber", parameternumber);
                addClinkLineDto = sqlConnection.Query<AddClinkLineDto>(SystemConstants.GetClinkLineItemList, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return addClinkLineDto;


            }

        }

        #endregion




        #region Beam

     

        public async Task<int> SaveBeamParameter(AddBeamParamterSetDto addBeamParamDto)
        {
            try
            {
                int Output = 0;

                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTProjectID", addBeamParamDto.ProjectId);
                    dynamicParameters.Add("@VCHPRODUCTTYPE", "MSH");
                    dynamicParameters.Add("@TNTPARAMSETNUMBER", addBeamParamDto.BeamParameterSetNo);
                    dynamicParameters.Add("@TNTTRANSPORTMODEID", addBeamParamDto.TNTTRANSPORTMODEID);
                    dynamicParameters.Add("@SITTopCover", addBeamParamDto.BeamTopCover);
                    dynamicParameters.Add("@SITBottomCover", addBeamParamDto.BeamBottonCover);
                    dynamicParameters.Add("@SITRightCover", addBeamParamDto.BeamRightCover);
                    dynamicParameters.Add("@SITLeftCover", addBeamParamDto.BeamLeftCover);
                    dynamicParameters.Add("@SITHOOK", addBeamParamDto.BeamHook);
                    dynamicParameters.Add("@SITLEG", addBeamParamDto.BeamLeg);
                    dynamicParameters.Add("@INTUSERID", 1);

                    dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);
                    sqlConnection.Query<int>(SystemConstants.BeamParameterSet_InsUpd, dynamicParameters, commandType: CommandType.StoredProcedure);
                    Output = dynamicParameters.Get<int>("@Output");
                    sqlConnection.Close();

                }

                return Output;


            }
            catch (Exception e)
            {
                return 0;
            }



        }

        public async Task<IEnumerable<ParameterSetDropdownDto>> GetBeamParameterSetAsync(int projectId)
        {
            IEnumerable<ParameterSetDropdownDto> parameterSetDropdowns;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@INTPROJECTID", projectId);
                parameterSetDropdowns = sqlConnection.Query<ParameterSetDropdownDto>(SystemConstants.GetBeamParameterList, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return parameterSetDropdowns;

            }

        }
      
        public async Task<IEnumerable<BeamListingDto>> GetBeamParamterTableListAsync(int projectId, int parameternumber)
        {
            IEnumerable<BeamListingDto> beamListingDtos;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@INTPROJECTID", projectId);
                dynamicParameters.Add("@parameternumber", parameternumber);
                beamListingDtos = sqlConnection.Query<BeamListingDto>(SystemConstants.Get_BeamParameterSetTable, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return beamListingDtos;


            }

        }
        public async Task<IEnumerable<CappingLineDto>> GetCappingLineItemAsync(int projectId, int parameternumber)
        {
            IEnumerable<CappingLineDto> cappingLineDtos;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@INTPROJECTID", projectId);
                dynamicParameters.Add("@refparameternumber", parameternumber);
                cappingLineDtos = sqlConnection.Query<CappingLineDto>(SystemConstants.GetCappingLineItemList, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return cappingLineDtos;


            }

        }
        public async Task<int> DeleteCappingLineItem(int ParamcageID)
        {
           
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@PARAMCAGEID", ParamcageID);
                sqlConnection.Query<int>(SystemConstants.Del_usp_CappingParamCage, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return await _dbContext.SaveChangesAsync();

            }

        }


        
        //

        public async Task<IEnumerable<CappigProductListDto>> GetCappingProductCodeListAsync(string cappingproduct)
        {
            IEnumerable<CappigProductListDto> cappingProductDtos;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@CAPPINGPRODUCT", cappingproduct);
                cappingProductDtos = sqlConnection.Query<CappigProductListDto>(SystemConstants.CappingProduct_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return cappingProductDtos;

            }
        }

        #endregion



        public  List<TransportModeDTO> Get_TransportMode()
        {
            IEnumerable<TransportModeDTO> TransportMaster;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();

                TransportMaster = sqlConnection.Query<TransportModeDTO>(SystemConstants.Get_TRANSPORTMASTER, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return TransportMaster.ToList();

            }



        }


        public async Task<int> DeleteCommonParamterInsert(AddParamDto addMeshParameter)
        {
            int Output = 0;
            // try
            // {


            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();

                dynamicParameters.Add("@INTPROJECTID", addMeshParameter.ProjectId);
                dynamicParameters.Add("@INTPARAMETERSETNO", addMeshParameter.ParamSetnumber);
                //dynamicParameters.Add("@INTUSERID", addMeshParameter.UserId);
                dynamicParameters.Add("@OUTPUT", null, dbType: DbType.Int32, ParameterDirection.Output);
                sqlConnection.Query<int>(SystemConstants.Common_DeleteParamterSet, dynamicParameters, commandType: CommandType.StoredProcedure);
                Output = dynamicParameters.Get<int>("@OUTPUT");
                sqlConnection.Close();
                
            }
            return Output;


            //}
            //catch (Exception e)
            //{
            //    return null;
            //}



        }

    }
}
