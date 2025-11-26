using ProductCodeMasterService.Model;
using ProductCodeMasterService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Reflection.Metadata;
using System.Collections.Generic;
using ProductCodeMasterService.Context;
using ProductCodeMasterService.Constants;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Dapper;
using ProductCodeMasterService.Dtos;

namespace ProductCodeMasterService.Repositories
{
    public class ProductCodeRepository : IProductCode
    {
        private ProductCodeContext _dbContext;
        private readonly IConfiguration _configuration;
        private string connectionString;

        public ProductCodeRepository(ProductCodeContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString(SystemConstants.DefaultDBConnection);

        }
        

        //public async Task<int> DeleteProductCodeAsync(int id)
        //{
        //    ProductCodeMaster itemToRemove = await _dbContext.ProductCodeMaster.SingleOrDefaultAsync(x => x.intProductCodeId == id); //returns a single item.
        //    _dbContext.ProductCodeMaster.Remove(itemToRemove);
        //    return await _dbContext.SaveChangesAsync();

        //}

        public async Task<IEnumerable<ProductType_Dropdown_Dto>> GetProductTypeAsync()
        {
            IEnumerable<ProductType_Dropdown_Dto> productType_Dropdowns; //new IEnumerable<List<ProjectMaster>>();

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                productType_Dropdowns = sqlConnection.Query<ProductType_Dropdown_Dto>(SystemConstants.GetProductType, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return productType_Dropdowns;

            }

        }
        
        #region MESH PRODUCT CODE

        public async Task<IEnumerable<SAPMaterialDropdown_Dto_Mesh>> GetSAPMaterialAsync()
        {

            IEnumerable<SAPMaterialDropdown_Dto_Mesh> sAPMaterials;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                sAPMaterials = sqlConnection.Query<SAPMaterialDropdown_Dto_Mesh>(SystemConstants.SapMaterial_Get, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return sAPMaterials;

            }

        }

        public async Task<IEnumerable<SAPMaterialDropdown_Dto_Mesh>> GetSAPMaterialRawAsync()
        {

            IEnumerable<SAPMaterialDropdown_Dto_Mesh> sAPMaterials;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                sAPMaterials = sqlConnection.Query<SAPMaterialDropdown_Dto_Mesh>(SystemConstants.SapMaterial_Get_Raw, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return sAPMaterials;

            }

        }

        public async Task<IEnumerable<GetMeshData_Dto>> GetMeshDataAsync(int SAPMaterialid)
        {

            IEnumerable<GetMeshData_Dto> sAPMaterials;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@SapMaterialid", SAPMaterialid);
                sAPMaterials = sqlConnection.Query<GetMeshData_Dto>(SystemConstants.Get_MESH_MasterData, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return sAPMaterials;

            }

        }



        public async Task<List<ProductCodeMaster>> GetProductcodeList()
        {
            // return await _dbContext.Shapegroup.ToListAsync();
            return await _dbContext.ProductCodeMaster.ToListAsync();

        }

        public async Task<List<ProductCodeMaster>> GetMeshWireProductList()
        {
            // return await _dbContext.Shapegroup.ToListAsync();
            return await _dbContext.ProductCodeMaster.Where(x=>x.bitRawMaterial==true).ToListAsync();

        }

        public async Task<IEnumerable<TwinIndicator_Dropdown_Mesh>> GetTwinIndicatorAsync()
        {
            //return await _dbContext.ProjectMaster.ToListAsync();
            IEnumerable<TwinIndicator_Dropdown_Mesh> twinIndicator_Dropdowns; //new IEnumerable<List<ProjectMaster>>();

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                twinIndicator_Dropdowns = sqlConnection.Query<TwinIndicator_Dropdown_Mesh>(SystemConstants.TwinIndicator_Get, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return twinIndicator_Dropdowns;

            }

        }

        public async Task<IEnumerable<BOMIndicator_Dropdown_Mesh>> GetBOMIndicatorAsync()
        {

            IEnumerable<BOMIndicator_Dropdown_Mesh> BOMIndicator_Dropdown_s; //new IEnumerable<List<ProjectMaster>>();

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                BOMIndicator_Dropdown_s = sqlConnection.Query<BOMIndicator_Dropdown_Mesh>(SystemConstants.BomIndicator_Get, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return BOMIndicator_Dropdown_s;

            }

        }

        public async Task<IEnumerable<StructureElement_Dropdown_Mesh>> GetStructureElementAsync()
        {

            IEnumerable<StructureElement_Dropdown_Mesh> structureElement_Dropdown_Meshes; //new IEnumerable<List<ProjectMaster>>();

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                structureElement_Dropdown_Meshes = sqlConnection.Query<StructureElement_Dropdown_Mesh>(SystemConstants.StructureElement_Get, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return structureElement_Dropdown_Meshes;

            }

        }


        public async Task<IEnumerable<StructureElement_Dropdown_Mesh>> GetStructureElementMeshAsync()
        {

            IEnumerable<StructureElement_Dropdown_Mesh> structureElement_Dropdown_Meshes; //new IEnumerable<List<ProjectMaster>>();

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                structureElement_Dropdown_Meshes = sqlConnection.Query<StructureElement_Dropdown_Mesh>(SystemConstants.StructureElement_Get_Mesh, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return structureElement_Dropdown_Meshes;

            }

        }


        public async Task<IEnumerable<MeshGetListDto>> GetMeshProductListAsync()
        {
            IEnumerable<MeshGetListDto> meshGetListDtos;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                meshGetListDtos = sqlConnection.Query<MeshGetListDto>(SystemConstants.MeshDetails_Get, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return meshGetListDtos;
            }

        }

        public async Task<IEnumerable<MeshListByIdDto>> GetMeshProductListByIdAsync(int prodcodeId)
        {
            IEnumerable<MeshListByIdDto> meshGetLists;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@ProductCodeID", prodcodeId);

                meshGetLists = sqlConnection.Query<MeshListByIdDto>(SystemConstants.MeshDetails_GetbyId, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return meshGetLists;
            }


        }

        public async Task<bool> CheckduplicateMeshAsync(string productcode)
        {
            try
            {
                var productcodeexist = await _dbContext.ProductCodeMaster.Where(x => x.vchProductCode == productcode)
                    .Select(x => x.vchProductCode).AnyAsync();
                if (productcodeexist)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<int> AddMeshProductCodeAsync(AddMeshDto addMeshDto)
        {
            try
            {
                IEnumerable<AddMeshDto> addMeshDtos;
                int Output = 0;

                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.Add("@intProductCodeId", addMeshDto.ProductCodeId);
                    dynamicParameters.Add("@vchProductCode", addMeshDto.ProductCode);
                    dynamicParameters.Add("@vchProductDescription", addMeshDto.ProductDescription);
                    dynamicParameters.Add("@bitRawMaterial", addMeshDto.RawMaterial);
                    dynamicParameters.Add("@intMWProductCodeId", addMeshDto.MWProductCodeId);
                    dynamicParameters.Add("@vchMWMaterialType", addMeshDto.MWMaterialType);
                    dynamicParameters.Add("@vchMWDiameter", addMeshDto.MWDiameter);
                    //dynamicParameters.Add("@vchMWMaterialAbbr", addMeshDto.MWMaterialAbbr);
                    dynamicParameters.Add("@intMWSpace", addMeshDto.MWSpace);
                    dynamicParameters.Add("@vchMWGrade", addMeshDto.MWGrade);
                    //dynamicParameters.Add("@vchMWLength", addMeshDto.MWLength);
                    //dynamicParameters.Add("@vchMWMaxBendLen", addMeshDto.MWMaxBendLen);
                    dynamicParameters.Add("@vchMWWeightRun", addMeshDto.MWWeightRun);
                    dynamicParameters.Add("@intCWProductcodeId", addMeshDto.CWProductcodeId);
                    dynamicParameters.Add("@vchCWMaterialType", addMeshDto.CWMaterialType);
                    dynamicParameters.Add("@vchCWDiameter", addMeshDto.CWDiameter);
                    //dynamicParameters.Add("@vchCWMaterialAbbr", addMeshDto.CWMaterialAbbr);
                    dynamicParameters.Add("@intCWSpace", addMeshDto.CWSpace);
                    dynamicParameters.Add("@vchCWGrade", addMeshDto.CWGrade);
                    //dynamicParameters.Add("@vchCWLength", addMeshDto.CWLength);
                    //dynamicParameters.Add("@vchCWMaxBendLen", addMeshDto.CWMaxBendLen);
                    dynamicParameters.Add("@vchCWWeightRun", addMeshDto.CWWeightRun);
                    dynamicParameters.Add("@vchWeightSqm", addMeshDto.WeightSqm);
                    //dynamicParameters.Add("@vchConversionFactor", addMeshDto.ConversionFactor);
                    dynamicParameters.Add("@chrTwinInd", addMeshDto.TwinInd);
                    //dynamicParameters.Add("@bitStaggeredInd", addMeshDto.StaggeredInd);
                    //dynamicParameters.Add("@bitBendInd", addMeshDto.BendInd);
                    //dynamicParameters.Add("@charBomInd", addMeshDto.BomInd);
                    dynamicParameters.Add("@intMinLink", addMeshDto.MinLink);
                    dynamicParameters.Add("@intMaxLink", addMeshDto.MaxLink);
                    dynamicParameters.Add("@intSapMaterial", addMeshDto.SapMaterial);
                    dynamicParameters.Add("@intProductType", addMeshDto.ProductTypeID);
                    dynamicParameters.Add("@intStructureElement", addMeshDto.StructureElement);
                    dynamicParameters.Add("@intStatusId", addMeshDto.StatusId);
                    //dynamicParameters.Add("@chrWireType", addMeshDto.WireType);
                    //dynamicParameters.Add("@intShapeCodeId", addMeshDto.ShapeCodeId);
                    dynamicParameters.Add("@intUserId", addMeshDto.UserId);
                    dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);

                    sqlConnection.Query<AddMeshDto>(SystemConstants.ProductCode_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();

                }

                return Output;


            }
            catch (Exception e)
            {
                return 0;
            }



        }

        public async Task<int> UpdateMeshProductCodeAsync(AddMeshDto addMeshDto)
        {
            try
            {
                IEnumerable<AddMeshDto> addMeshDtos;
                int Output = 0;

                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.Add("@intProductCodeId", addMeshDto.ProductCodeId);
                    dynamicParameters.Add("@vchProductCode", addMeshDto.ProductCode);
                    dynamicParameters.Add("@vchProductDescription", addMeshDto.ProductDescription);
                    dynamicParameters.Add("@bitRawMaterial", addMeshDto.RawMaterial);
                    dynamicParameters.Add("@intMWProductCodeId", addMeshDto.MWProductCodeId);
                    dynamicParameters.Add("@vchMWMaterialType", addMeshDto.MWMaterialType);
                    dynamicParameters.Add("@vchMWDiameter", addMeshDto.MWDiameter);
                    //dynamicParameters.Add("@vchMWMaterialAbbr", addMeshDto.MWMaterialAbbr);
                    dynamicParameters.Add("@intMWSpace", addMeshDto.MWSpace);
                    dynamicParameters.Add("@vchMWGrade", addMeshDto.MWGrade);
                    //dynamicParameters.Add("@vchMWLength", addMeshDto.MWLength);
                    //dynamicParameters.Add("@vchMWMaxBendLen", addMeshDto.MWMaxBendLen);
                    dynamicParameters.Add("@vchMWWeightRun", addMeshDto.MWWeightRun);
                    dynamicParameters.Add("@intCWProductcodeId", addMeshDto.CWProductcodeId);
                    dynamicParameters.Add("@vchCWMaterialType", addMeshDto.CWMaterialType);
                    dynamicParameters.Add("@vchCWDiameter", addMeshDto.CWDiameter);
                    //dynamicParameters.Add("@vchCWMaterialAbbr", addMeshDto.CWMaterialAbbr);
                    dynamicParameters.Add("@intCWSpace", addMeshDto.CWSpace);
                    dynamicParameters.Add("@vchCWGrade", addMeshDto.CWGrade);
                    //dynamicParameters.Add("@vchCWLength", addMeshDto.CWLength);
                    //dynamicParameters.Add("@vchCWMaxBendLen", addMeshDto.CWMaxBendLen);
                    dynamicParameters.Add("@vchCWWeightRun", addMeshDto.CWWeightRun);
                    dynamicParameters.Add("@vchWeightSqm", addMeshDto.WeightSqm);
                    //dynamicParameters.Add("@vchConversionFactor", addMeshDto.ConversionFactor);
                    dynamicParameters.Add("@chrTwinInd", addMeshDto.TwinInd);
                    //dynamicParameters.Add("@bitStaggeredInd", addMeshDto.StaggeredInd);
                    //dynamicParameters.Add("@bitBendInd", addMeshDto.BendInd);
                    //dynamicParameters.Add("@charBomInd", addMeshDto.BomInd);
                    dynamicParameters.Add("@intMinLink", addMeshDto.MinLink);
                    dynamicParameters.Add("@intMaxLink", addMeshDto.MaxLink);
                    dynamicParameters.Add("@intSapMaterial", addMeshDto.SapMaterial);
                    dynamicParameters.Add("@intProductType", addMeshDto.ProductTypeID);
                    dynamicParameters.Add("@intStructureElement", addMeshDto.StructureElement);
                    dynamicParameters.Add("@intStatusId", addMeshDto.StatusId);
                    //dynamicParameters.Add("@chrWireType", addMeshDto.WireType);
                    //dynamicParameters.Add("@intShapeCodeId", addMeshDto.ShapeCodeId);
                    dynamicParameters.Add("@intUserId", addMeshDto.UserId);
                    dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);

                    sqlConnection.Query<AddMeshDto>(SystemConstants.ProductCode_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);
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

        public async Task<int> DeleteMESHPRoductAsync(int MESHProductCodeID)
        {
            //CABProductCodeMaster itemToRemove = await _dbContext.CABProductCodeMaster.SingleOrDefaultAsync(x => x.intCABProductCodeID == CABProductCodeID); //returns a single item.
            //_dbContext.CABProductCodeMaster.Remove(itemToRemove);
            //return await _dbContext.SaveChangesAsync();

            IEnumerable<ProductCodeMaster> MESHProductCodeMasters;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intProductCodeID", MESHProductCodeID);
                MESHProductCodeMasters = sqlConnection.Query<ProductCodeMaster>(SystemConstants.MESHProductCodeMasterDetails_Delete, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return await _dbContext.SaveChangesAsync(); ;

            }

        }
        public async Task<IEnumerable<GetMWDataDto>> GetMainWireDataAsync(int ProductCodeId)
        {

            IEnumerable<GetMWDataDto> getMWDataDtos;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@ProductCodeId", ProductCodeId);
                getMWDataDtos = sqlConnection.Query<GetMWDataDto>(SystemConstants.Get_MainWireData, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return getMWDataDtos;

            }

        }

        public async Task<IEnumerable<GetCWDataDto>> GetCrossWireDataAsync(int ProductCodeId)
        {

            IEnumerable<GetCWDataDto> getCWDataDtos;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@ProductCodeId", ProductCodeId);
                getCWDataDtos = sqlConnection.Query<GetCWDataDto>(SystemConstants.Get_CrossWireData, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return getCWDataDtos;

            }

        }

        #endregion

        #region Raw Material
        public async Task<IEnumerable<GradeType_Raw_Material_Dto>> GetGradeTypeAsync_Raw()
        {

            IEnumerable<GradeType_Raw_Material_Dto> gradeType_Raws;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                gradeType_Raws = sqlConnection.Query<GradeType_Raw_Material_Dto>(SystemConstants.Grade_Get_Raw_Mat, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return gradeType_Raws;

            }

        }

        public async Task<IEnumerable<MaterialType_Raw_Material>> GetMaterialTypeAsync_Raw()
        {

            IEnumerable<MaterialType_Raw_Material> MaterialType_Raws;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                MaterialType_Raws = sqlConnection.Query<MaterialType_Raw_Material>(SystemConstants.GetMaterial_Raw_Mat, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return MaterialType_Raws;

            }

        }

        public async Task<IEnumerable<GetRawMaterialList>> GetRawMaterialListAsync()
        {
            IEnumerable<GetRawMaterialList> getRawMaterialLists;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                getRawMaterialLists = sqlConnection.Query<GetRawMaterialList>(SystemConstants.RawMaterialDetails_Get, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return getRawMaterialLists;
            }

        }

        public async Task<IEnumerable<GetRawMaterialList>> GetRawMaterialListbyIdAsync(int prodcodeId)
        {
            IEnumerable<GetRawMaterialList> getRawMaterialLists;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@ProductCodeID", prodcodeId);

                getRawMaterialLists = sqlConnection.Query<GetRawMaterialList>(SystemConstants.RawMaterialDetails_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return getRawMaterialLists;
            }


        }

        public async Task<int> AddRawMaterialAsync(AddRawMaterialDto addRawMaterialDto)
        {
            try
            {
                IEnumerable<AddRawMaterialDto> addRawMaterials;
                int Output = 0;

                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.Add("@intProductCodeId", addRawMaterialDto.ProductCodeId);
                    dynamicParameters.Add("@vchProductCode", addRawMaterialDto.ProductCode);
                    dynamicParameters.Add("@vchProductDescription", addRawMaterialDto.ProductDescription);
                    dynamicParameters.Add("@bitRawMaterial", addRawMaterialDto.RawMaterial);
                    dynamicParameters.Add("@intMaterialTypeId", addRawMaterialDto.MaterialType);
                    dynamicParameters.Add("@decDiameter", addRawMaterialDto.Diameter);
                    dynamicParameters.Add("@vchMaterialAbbr", addRawMaterialDto.MaterialAbbr);
                    dynamicParameters.Add("@vchGradeType", addRawMaterialDto.Grade);
                    dynamicParameters.Add("@decWeightRun", addRawMaterialDto.WeightRun);
                    dynamicParameters.Add("@tntStatusId", addRawMaterialDto.StatusId);
                    dynamicParameters.Add("@intUserId", addRawMaterialDto.UserId);
                    dynamicParameters.Add("@SapMaterialId", addRawMaterialDto.SapMaterialId);


                    dynamicParameters.Add("@intOutput", null, dbType: DbType.Int32, ParameterDirection.Output);

                    sqlConnection.Query<AddRawMaterialDto>(SystemConstants.RawMaterial_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);
                    Output = dynamicParameters.Get<int>("@intOutput");


                    sqlConnection.Close();

                }

                return Output;


            }
            catch (Exception e)
            {
                return 1;
            }



        }

        public async Task<AddRawMaterialDto> UpdateRawMaterialAsync(AddRawMaterialDto updaterawmaterial)
        {
            try
            {
                IEnumerable<AddRawMaterialDto> updateRawMaterials;


                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.Add("@intProductCodeId", updaterawmaterial.ProductCodeId);
                    dynamicParameters.Add("@vchProductCode", updaterawmaterial.ProductCode);
                    dynamicParameters.Add("@vchProductDescription", updaterawmaterial.ProductDescription);
                    dynamicParameters.Add("@bitRawMaterial", updaterawmaterial.RawMaterial);
                    dynamicParameters.Add("@intMaterialTypeId", updaterawmaterial.MaterialType);
                    dynamicParameters.Add("@decDiameter", updaterawmaterial.Diameter);
                    dynamicParameters.Add("@vchMaterialAbbr", updaterawmaterial.MaterialAbbr);
                    dynamicParameters.Add("@vchGradeType", updaterawmaterial.Grade);
                    dynamicParameters.Add("@decWeightRun", updaterawmaterial.WeightRun);
                    dynamicParameters.Add("@tntStatusId", updaterawmaterial.StatusId);
                    dynamicParameters.Add("@intUserId", updaterawmaterial.UserId);

                    updateRawMaterials = sqlConnection.Query<AddRawMaterialDto>(SystemConstants.RawMaterial_Update, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();


                }

                return updaterawmaterial;

            }
            catch (Exception e)
            {
                return updaterawmaterial;
            }

        }

        public async Task<int> DeleteRawMaterialAsync(int ProductCodeID)
        {

            IEnumerable<ProductCodeMasterDto> ProductCodeMasters;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intProductCodeID", ProductCodeID);
                ProductCodeMasters = sqlConnection.Query<ProductCodeMasterDto>(SystemConstants.RawMaterial_Delete, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return await _dbContext.SaveChangesAsync(); ;

            }

        }



        #endregion

        #region CAB
        public async Task<IEnumerable<GradeTypeDropdownDto>> GetGradeTypeAsync()
        {
            IEnumerable<GradeTypeDropdownDto> grades; //new IEnumerable<List<ProjectMaster>>();

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                grades = sqlConnection.Query<GradeTypeDropdownDto>(SystemConstants.GetGradeTypes, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return grades;

            }

        }

        public async Task<IEnumerable<SAPMaterialDropdown_Dto_Cab>> GetCabSAPMaterialAsync()
        {
            IEnumerable<SAPMaterialDropdown_Dto_Cab> SAPMaterialDropdown_Dto_Cabs; 

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                SAPMaterialDropdown_Dto_Cabs = sqlConnection.Query<SAPMaterialDropdown_Dto_Cab>(SystemConstants.GetCabProductCodeRMSAPMaterial, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return SAPMaterialDropdown_Dto_Cabs;

            }

        }


        public async Task<IEnumerable<CABProductCodeMasterDto>> GetCABProductCodeListAsync()
        {
            //return await _dbContext.ProjectMaster.ToListAsync();
            IEnumerable<CABProductCodeMasterDto> CabproductCodemaster;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                CabproductCodemaster = sqlConnection.Query<CABProductCodeMasterDto>(SystemConstants.GetCabProductCodeMasterDetails, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return CabproductCodemaster;
            }

        }
       
        public async Task<IEnumerable<CABProductCodeMasterDto>> GetCABProductCodebyIdAsync(int CabprodId)
        {
            IEnumerable<CABProductCodeMasterDto> CabproductCodemaster;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@CabProductCodeId", CabprodId);

                CabproductCodemaster = sqlConnection.Query<CABProductCodeMasterDto>(SystemConstants.GetCabProductCodeMasterDetails, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return CabproductCodemaster;
            }


        }
        public async Task<bool> CheckduplicateCABAsync(string productcode)
        {
            try
            {
                var productcodeexist = await _dbContext.CABProductCodeMaster.Where(x => x.ProductCode == productcode)
                    .Select(x => x.ProductCode).AnyAsync();
                if (productcodeexist)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<int> AddCABProductCodeAsync(AddCABProductDto CABProductDto)
        {
            try
            {
                IEnumerable<AddCABProductDto> CABProductsList;
                int Output = 0;

                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.Add("@intCabProductCodeId", CABProductDto.CabProductCodeId);
                    dynamicParameters.Add("@chrGradeType", CABProductDto.GradeType);
                    dynamicParameters.Add("@intDiameter", CABProductDto.Diameter);
                    dynamicParameters.Add("@bitCouplerIndicator", CABProductDto.CouplerIndicator);
                    dynamicParameters.Add("@vchCouplerType", CABProductDto.CouplerType);
                    dynamicParameters.Add("@intFGSAPMaterialID", CABProductDto.FGSAPMaterialID);
                    dynamicParameters.Add("@intRMSAPMaterialID", CABProductDto.RMSAPMaterialID);
                    dynamicParameters.Add("@tntStatusId", CABProductDto.StatusId);
                    dynamicParameters.Add("@intUserId", CABProductDto.UserId);
                    dynamicParameters.Add("@vchProductCode", CABProductDto.ProductCode);
                    dynamicParameters.Add("@Description", CABProductDto.Description);
                    dynamicParameters.Add("@ProductType", CABProductDto.ProductType);

                    dynamicParameters.Add("@intOutput", null, dbType: DbType.Int32, ParameterDirection.Output);

                    sqlConnection.Query<AddCABProductDto>(SystemConstants.InsertCabProductCodeMaster, dynamicParameters, commandType: CommandType.StoredProcedure);
                    Output = dynamicParameters.Get<int>("@intOutput");


                    sqlConnection.Close();

                }

                return Output;


            }
            catch (Exception e)
            {
                return 0;
            }



        }

        public async Task<int> UpdateCABProductAsync(AddCABProductDto CABProductDto)
        {
            int Output = 0;        
            try
            {
                IEnumerable<AddCABProductDto> updateCablist; 
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intCabProductCodeId", CABProductDto.CabProductCodeId);
                    dynamicParameters.Add("@chrGradeType", CABProductDto.GradeType);
                    dynamicParameters.Add("@intDiameter", CABProductDto.Diameter);
                    dynamicParameters.Add("@bitCouplerIndicator", CABProductDto.CouplerIndicator);
                    dynamicParameters.Add("@vchCouplerType", CABProductDto.CouplerType);
                    dynamicParameters.Add("@intFGSAPMaterialID", CABProductDto.FGSAPMaterialID);
                    dynamicParameters.Add("@intRMSAPMaterialID", CABProductDto.RMSAPMaterialID);
                    dynamicParameters.Add("@tntStatusId", CABProductDto.StatusId);
                    dynamicParameters.Add("@intUserId", CABProductDto.UserId);
                    dynamicParameters.Add("@vchProductCode", CABProductDto.ProductCode);
                    dynamicParameters.Add("@Description", CABProductDto.Description);
                    dynamicParameters.Add("@ProductType", CABProductDto.ProductType);
                    dynamicParameters.Add("@intOutput", null, dbType: DbType.Int32, ParameterDirection.Output);

                    updateCablist = sqlConnection.Query<AddCABProductDto>(SystemConstants.InsertCabProductCodeMaster, dynamicParameters, commandType: CommandType.StoredProcedure);
                    Output = dynamicParameters.Get<int>("@intOutput");
                    sqlConnection.Close();


                }

                return Output;


            }
            catch (Exception e)
            {
                if(e.GetType().Name == "SqlException")
                {
                     Output = 4;
                }
                return Output;
            }

        }



        public async Task<int> DeleteCABPRoductAsync(int CABProductCodeID)
        {
            //CABProductCodeMaster itemToRemove = await _dbContext.CABProductCodeMaster.SingleOrDefaultAsync(x => x.intCABProductCodeID == CABProductCodeID); //returns a single item.
            //_dbContext.CABProductCodeMaster.Remove(itemToRemove);
            //return await _dbContext.SaveChangesAsync();

            IEnumerable<CABProductCodeMaster> CABProductCodeMasters;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intCabProductCodeID", CABProductCodeID);
                CABProductCodeMasters = sqlConnection.Query<CABProductCodeMaster>(SystemConstants.CabProductCodeMasterDetails_Delete, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return await _dbContext.SaveChangesAsync(); ;

            }

        }

        #endregion


        #region CORECAGE
        public async Task<IEnumerable<SAPMaterialDropdown_Dto_Corecage>> GetCoreCageSAPMaterialAsync()
        {

            IEnumerable<SAPMaterialDropdown_Dto_Corecage> SAPMaterial_Dropdown_Corecages; //new IEnumerable<List<ProjectMaster>>();

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                SAPMaterial_Dropdown_Corecages = sqlConnection.Query<SAPMaterialDropdown_Dto_Corecage>(SystemConstants.GetSapMaterial_CoreCage, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return SAPMaterial_Dropdown_Corecages;

            }

        }

        public async Task<IEnumerable<CARProductCodeListDto>> GetCORECAGEProductCodeListAsync()
        {
            //return await _dbContext.ProjectMaster.ToListAsync();
            IEnumerable<CARProductCodeListDto> Carproductcodemaster;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                Carproductcodemaster = sqlConnection.Query<CARProductCodeListDto>(SystemConstants.GetCARProductCodeMaster, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return Carproductcodemaster;
            }

        }

        public async Task<bool> CheckduplicateCARAsync(string productcode)
        {
            try
            {
                var productcodeexist = await _dbContext.ProductCodeMaster.Where(x => x.vchProductCode == productcode)
                    .Select(x => x.vchProductCode).AnyAsync();
                if (productcodeexist)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<int> AddCARProductCodeAsync(AddCoreCageDto CARProductDto)
        {
            try
            {
                IEnumerable<AddCoreCageDto> AddCARProductDtos;
                int Output = 0;

                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.Add("@intProductCodeId", CARProductDto.ProductCodeId);
                    dynamicParameters.Add("@vchProductCode", CARProductDto.ProductCode);
                    dynamicParameters.Add("@vchProductDescription", CARProductDto.ProductDescription);
                    dynamicParameters.Add("@intSapMaterial", CARProductDto.SapMaterial);
                    dynamicParameters.Add("@intStatusId", CARProductDto.StatusId);
                    dynamicParameters.Add("@vchWeightSqm", CARProductDto.WeightSqm);
                    // dynamicParameters.Add("@vchProductTypeL2", CARProductDto.ProductTypeL2);
                    dynamicParameters.Add("@intStructureElement", CARProductDto.StructureElement);
                    // dynamicParameters.Add("@vchConversionFactor", CARProductDto.ConversionFactor);
                    dynamicParameters.Add("@intPRoductType", CARProductDto.ProductType);
                    dynamicParameters.Add("@intUserId", CARProductDto.UserId);
                    dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);
                    sqlConnection.Query<AddCoreCageDto>(SystemConstants.CARProductCode_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);
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
        
        public async Task<int> DeleteCORECAGEProductCodeAsync(int id)
        {

            IEnumerable<ProductCodeMaster> productCodeMasters;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intProductCodeID", id);
                productCodeMasters = sqlConnection.Query<ProductCodeMaster>(SystemConstants.CARProductCodeMasterDetails_Delete, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return await _dbContext.SaveChangesAsync(); ;

            }

        }

        public async Task<IEnumerable<CARProductCodeListDto>> GetCARProductCodebyIdAsync(int CARprodId)
        {
            IEnumerable<CARProductCodeListDto> productCodemaster;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@ProductCodeId", CARprodId);

                productCodemaster = sqlConnection.Query<CARProductCodeListDto>(SystemConstants.GetCARProductCodeMaster, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return productCodemaster;
            }


        }

        public async Task<int> UpdateCARProductAsync(AddCoreCageDto CARProductDto)
        {
            try
            {

                IEnumerable<AddCoreCageDto> AddCARProductDtos;
                int Output = 0;
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.Add("@intProductCodeId", CARProductDto.ProductCodeId);
                    dynamicParameters.Add("@vchProductCode", CARProductDto.ProductCode);
                    dynamicParameters.Add("@vchProductDescription", CARProductDto.ProductDescription);
                    dynamicParameters.Add("@intSapMaterial", CARProductDto.SapMaterial);
                    dynamicParameters.Add("@intStatusId", CARProductDto.StatusId);
                    dynamicParameters.Add("@vchWeightSqm", CARProductDto.WeightSqm);
                    // dynamicParameters.Add("@vchProductTypeL2", CARProductDto.ProductTypeL2);
                    dynamicParameters.Add("@intStructureElement", CARProductDto.StructureElement);
                    // dynamicParameters.Add("@vchConversionFactor", CARProductDto.ConversionFactor);
                    dynamicParameters.Add("@intPRoductType", CARProductDto.ProductType);
                    dynamicParameters.Add("@intUserId", CARProductDto.UserId);
                    dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);
                    sqlConnection.Query<AddCoreCageDto>(SystemConstants.CARProductCode_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);
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

        #endregion


        #region ACS
        public async Task<IEnumerable<GradeTypeDropdownDto>> GetACSGradeTypeAsync()
        {
            IEnumerable<GradeTypeDropdownDto> grades; //new IEnumerable<List<ProjectMaster>>();

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                grades = sqlConnection.Query<GradeTypeDropdownDto>(SystemConstants.GetGradeTypes_Acs, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return grades;

            }

        }

        public async Task<IEnumerable<SAPMaterialDropdown_Dto_Cab>> GetACSSAPMaterialAsync()
        {
            IEnumerable<SAPMaterialDropdown_Dto_Cab> SAPMaterialDropdown_Dto_Cabs; //new IEnumerable<List<ProjectMaster>>();

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                SAPMaterialDropdown_Dto_Cabs = sqlConnection.Query<SAPMaterialDropdown_Dto_Cab>(SystemConstants.GetACSProductCodeSAPMaterial, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return SAPMaterialDropdown_Dto_Cabs;

            }

        }

        public async Task<IEnumerable<AccessoriesProductCodeMaster_Dto>> GetACSProductCodeListAsync()
        {
            IEnumerable<AccessoriesProductCodeMaster_Dto> acsproductCodemaster;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                acsproductCodemaster = sqlConnection.Query<AccessoriesProductCodeMaster_Dto>(SystemConstants.GetACSProductCodeMasterDetails, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return acsproductCodemaster;
            }

        }

        public async Task<IEnumerable<AccessoriesProductCodeMaster_Dto>> GetACSProductCodebyIdAsync(int AcsprodId)
        {
            IEnumerable<AccessoriesProductCodeMaster_Dto> ACSproductCodemaster;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@AccessoriesProductCodeID", AcsprodId);

                ACSproductCodemaster = sqlConnection.Query<AccessoriesProductCodeMaster_Dto>(SystemConstants.GetACSProductCodeMasterDetails, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return ACSproductCodemaster;
            }


        }

        public async Task<int> AddACSProductCodeAsync(AddAccessoriesDto addAccessoriesDto)
        {
            try
            {
                IEnumerable<AddAccessoriesDto> addAccessories;
                int Output = 0;

                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.Add("@intAccessoriesProductCodeId", addAccessoriesDto.AccessoriesProductCodeId);
                    dynamicParameters.Add("@chrGradeType", addAccessoriesDto.GradeType);
                    dynamicParameters.Add("@intDiameter", addAccessoriesDto.Diameter);
                    dynamicParameters.Add("@intFGSAPMaterialID", addAccessoriesDto.FGSAPMaterialID);
                    dynamicParameters.Add("@intRMSAPMaterialID", addAccessoriesDto.RMSAPMaterialID);
                    dynamicParameters.Add("@tntStatusId", addAccessoriesDto.StatusId);
                    dynamicParameters.Add("@intUserId", addAccessoriesDto.UserId);
                    dynamicParameters.Add("@vchProductCode", addAccessoriesDto.ProductCode);
                    dynamicParameters.Add("@Description", addAccessoriesDto.Description);
                    dynamicParameters.Add("@ProductType", addAccessoriesDto.ProductType);

                    dynamicParameters.Add("@intOutput", null, dbType: DbType.Int32, ParameterDirection.Output);

                    sqlConnection.Query<AddAccessoriesDto>(SystemConstants.InsertAcsProductCodeMaster, dynamicParameters, commandType: CommandType.StoredProcedure);
                    Output = dynamicParameters.Get<int>("@intOutput");


                    sqlConnection.Close();

                }

                return Output;


            }
            catch (Exception e)
            {
                return 0;
            }



        }

        public async Task<int> UpdateACSProductAsync(AddAccessoriesDto addAccessoriesDto)
        {
            int Output = 0;
            try
            {
                IEnumerable<AddAccessoriesDto> addAccessories;
                

                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    
                    dynamicParameters.Add("@intAccessoriesProductCodeId", addAccessoriesDto.AccessoriesProductCodeId);
                    dynamicParameters.Add("@chrGradeType", addAccessoriesDto.GradeType);
                    dynamicParameters.Add("@intDiameter", addAccessoriesDto.Diameter);
                    dynamicParameters.Add("@intFGSAPMaterialID", addAccessoriesDto.FGSAPMaterialID);
                    dynamicParameters.Add("@intRMSAPMaterialID", addAccessoriesDto.RMSAPMaterialID);
                    dynamicParameters.Add("@tntStatusId", addAccessoriesDto.StatusId);
                    dynamicParameters.Add("@intUserId", addAccessoriesDto.UserId);
                    dynamicParameters.Add("@vchProductCode", addAccessoriesDto.ProductCode);
                    dynamicParameters.Add("@Description", addAccessoriesDto.Description);
                    dynamicParameters.Add("@ProductType", addAccessoriesDto.ProductType);
                    dynamicParameters.Add("@intOutput", null, dbType: DbType.Int32, ParameterDirection.Output);

                    sqlConnection.Query<AddAccessoriesDto>(SystemConstants.InsertAcsProductCodeMaster, dynamicParameters, commandType: CommandType.StoredProcedure);
                    Output = dynamicParameters.Get<int>("@intOutput");
                    sqlConnection.Close();
                   
                    


                }

                return Output;

            }
            catch (Exception e)
            {
                if (e.GetType().Name == "SqlException")
                {
                    Output = 4;
                }
                return Output;
            }

        }

        public async Task<int> DeleteACSProductAsync(int ACSProductCodeID)
        {
            
            IEnumerable<AccessoriesProductCodeMaster_Dto> accessoriesProductCodeMasters;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intACSProductCodeID", ACSProductCodeID);
                accessoriesProductCodeMasters = sqlConnection.Query<AccessoriesProductCodeMaster_Dto>(SystemConstants.ACSProductCodeMasterDetails_Delete, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return await _dbContext.SaveChangesAsync(); ;

            }

        }


        #endregion


        #region Common Product Code list        
        public async Task<IEnumerable<ProductCodeCommonListDto>> GetCommonProductCodeListAsync(string producttypes)
        {
            IEnumerable<ProductCodeCommonListDto> acsproductCodemaster;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                //@Producttypes
                var dynamicParameters = new DynamicParameters();

                dynamicParameters.Add("@Producttypes", producttypes);
           
                acsproductCodemaster = sqlConnection.Query<ProductCodeCommonListDto>(SystemConstants.CommonProductcodeList, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return acsproductCodemaster;
            }

        }
       
        public async Task<int> DeleteCommonProductAsync(int ProductCodeID, int ProductTypeID)
        {
            int Output = 0;
            IEnumerable<AccessoriesProductCodeMaster_Dto> accessoriesProductCodeMasters;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@ProductCodeID", ProductCodeID);
                dynamicParameters.Add("@Producttypes", ProductTypeID);
                dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);

                sqlConnection.Query<int>(SystemConstants.CommonProductcode_Delete, dynamicParameters, commandType: CommandType.StoredProcedure);
                Output = dynamicParameters.Get<int>("@Output");
                sqlConnection.Close();
                return Output;

            }

        }


        #endregion
    }
}
