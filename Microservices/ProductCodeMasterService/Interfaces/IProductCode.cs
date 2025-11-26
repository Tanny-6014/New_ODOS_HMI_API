using ProductCodeMasterService.Dtos;
using ProductCodeMasterService.Model;

namespace ProductCodeMasterService.Interfaces
{
    public interface IProductCode
    {
       
        Task<IEnumerable<ProductType_Dropdown_Dto>> GetProductTypeAsync();
       
        #region MESH
        Task<List<ProductCodeMaster>> GetProductcodeList();

        Task<List<ProductCodeMaster>> GetMeshWireProductList();
        Task<IEnumerable<SAPMaterialDropdown_Dto_Mesh>> GetSAPMaterialAsync();

        Task<IEnumerable<SAPMaterialDropdown_Dto_Mesh>> GetSAPMaterialRawAsync();

        Task<IEnumerable<GetMeshData_Dto>> GetMeshDataAsync(int SAPMaterialid);
        Task<IEnumerable<TwinIndicator_Dropdown_Mesh>> GetTwinIndicatorAsync();
        Task<IEnumerable<BOMIndicator_Dropdown_Mesh>> GetBOMIndicatorAsync();
        Task<IEnumerable<StructureElement_Dropdown_Mesh>> GetStructureElementAsync();
        Task<IEnumerable<MeshGetListDto>> GetMeshProductListAsync();
        Task<IEnumerable<MeshListByIdDto>> GetMeshProductListByIdAsync(int productcode);
        Task<bool> CheckduplicateMeshAsync(string productcode);
        Task<int> AddMeshProductCodeAsync(AddMeshDto AddMeshDtos);
        Task<int> UpdateMeshProductCodeAsync(AddMeshDto AddMeshDtos);
        Task<int> DeleteMESHPRoductAsync(int MESHProductCodeID);
        Task<IEnumerable<GetMWDataDto>> GetMainWireDataAsync(int ProductCodeId);
        Task<IEnumerable<GetCWDataDto>> GetCrossWireDataAsync(int ProductCodeId);

        Task<IEnumerable<StructureElement_Dropdown_Mesh>> GetStructureElementMeshAsync();


        #endregion

        #region Raw Material
        Task<IEnumerable<GradeType_Raw_Material_Dto>> GetGradeTypeAsync_Raw();
        Task<IEnumerable<MaterialType_Raw_Material>> GetMaterialTypeAsync_Raw();
        Task<int> AddRawMaterialAsync(AddRawMaterialDto AddAccessoriesDtos);
        Task<IEnumerable<GetRawMaterialList>> GetRawMaterialListAsync();
        Task<IEnumerable<GetRawMaterialList>> GetRawMaterialListbyIdAsync(int productcode);
        Task<AddRawMaterialDto> UpdateRawMaterialAsync(AddRawMaterialDto productcodeId);
        Task<int> DeleteRawMaterialAsync(int ProductCodeID);
        #endregion

        #region CAB
        Task<IEnumerable<CABProductCodeMasterDto>> GetCABProductCodeListAsync();
        Task<IEnumerable<CABProductCodeMasterDto>> GetCABProductCodebyIdAsync(int CabprodId);
        Task<IEnumerable<GradeTypeDropdownDto>> GetGradeTypeAsync();
        Task<IEnumerable<SAPMaterialDropdown_Dto_Cab>> GetCabSAPMaterialAsync();
        Task<bool> CheckduplicateCABAsync(string productcode);
        Task<int> AddCABProductCodeAsync(AddCABProductDto insertCABPoducts);
        Task<int> UpdateCABProductAsync(AddCABProductDto updatecab);
        Task<int> DeleteCABPRoductAsync(int CABProductCodeID);
        #endregion

        #region CoreCage
        Task<IEnumerable<SAPMaterialDropdown_Dto_Corecage>> GetCoreCageSAPMaterialAsync();
        Task<IEnumerable<CARProductCodeListDto>> GetCORECAGEProductCodeListAsync();
        Task<IEnumerable<CARProductCodeListDto>> GetCARProductCodebyIdAsync(int CARprodId);
        Task<bool> CheckduplicateCARAsync(string productcode);
        Task<int> AddCARProductCodeAsync(AddCoreCageDto AddCoreCageDtos);
        Task<int> UpdateCARProductAsync(AddCoreCageDto AddCoreCageDtos);
        Task<int> DeleteCORECAGEProductCodeAsync(int Id);
        #endregion

        #region Accessories
        Task<IEnumerable<GradeTypeDropdownDto>> GetACSGradeTypeAsync();

        Task<IEnumerable<SAPMaterialDropdown_Dto_Cab>> GetACSSAPMaterialAsync();

        Task<IEnumerable<AccessoriesProductCodeMaster_Dto>> GetACSProductCodeListAsync();

        Task<IEnumerable<AccessoriesProductCodeMaster_Dto>> GetACSProductCodebyIdAsync(int AcsprodcodeId);

        Task<int> AddACSProductCodeAsync(AddAccessoriesDto AddAccessoriesDtos);
        Task<int> UpdateACSProductAsync(AddAccessoriesDto updateAcs);
        Task<int> DeleteACSProductAsync(int ProductCodeID);
        #endregion
       
        #region Common Product Code 
        Task<IEnumerable<ProductCodeCommonListDto>> GetCommonProductCodeListAsync(string producttypes);

        Task<int> DeleteCommonProductAsync(int ProductCodeID,int ProductTypeID);

        #endregion




    }
}
