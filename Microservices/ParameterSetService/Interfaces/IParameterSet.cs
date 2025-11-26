using Microsoft.AspNetCore.Mvc;
using ParameterSetService.Dtos;
using ParameterSetService.Models;

namespace ParameterSetService.Interfaces
{
    public interface IParameterSet
    {

        Task<int> CommonParamterInsert(AddParamDto addMeshParameter);

        #region Mesh Parameter Set


        // Task<IEnumerable<ParameterSetDropdownDto>> MeshParamterInsert(AddMeshParamDto addMeshParameter);
        Task<IEnumerable<ParameterSetDropdownDto>> GetParameterSetAsync(int projectId);
        Task<IEnumerable<MeshProductCodeDto>> GetMeshProductCodeAsync();
        Task<IEnumerable<MeshListing>> GetMeshListAsync(int projectId, int parameternumber);
        Task<IEnumerable<MeshListing>> GetWallListAsync(int projectId, int parameternumber);
        Task<int> MeshProjectParamLap(AddMeshParamaterDto addMeshParamDto);
        Task<int> UpdateMeshParamLap(AddMeshParamaterDto addMeshParamDto);

        Task<int> DeleteMesh(int intMeshLapId);

        #endregion


        #region Column Parameter Set
        Task<int> SaveColumnParameter(AddColumnParameterSet addColumnParamDto);
        Task<int> SaveClinkLineItem(AddClinkLineDto addClinkLineDto);
        Task<int> DeleteClinkLineItem(int ClinkParamcageID);
        Task<IEnumerable<ColumnListingDto>> GetColumnListAsync(int projectId, int parameternumber);
        Task<IEnumerable<ParameterSetDropdownDto>> GetColumnParameterSetAsync(int projectId);
        Task<IEnumerable<AddClinkLineDto>> GetClinkLineItemAsync(int projectId, int parameternumber);
        Task<int> SaveCappingLineItem(AddCappingLineDto cappingLineDto);
        #endregion

        #region Beam

        Task<int> SaveBeamParameter(AddBeamParamterSetDto addBeamParamDto);
        Task<IEnumerable<ParameterSetDropdownDto>> GetBeamParameterSetAsync(int projectId);
        Task<IEnumerable<BeamListingDto>> GetBeamParamterTableListAsync(int projectId, int parameternumber);
        Task<IEnumerable<CappingLineDto>> GetCappingLineItemAsync(int projectId, int parameternumber);
        Task<int> DeleteCappingLineItem(int ParamcageID);

        Task<IEnumerable<CappigProductListDto>> GetCappingProductCodeListAsync(string cappingproduct);
        #endregion


        public List<TransportModeDTO> Get_TransportMode();

        public  Task<int> DeleteCommonParamterInsert(AddParamDto addMeshParameter);


    }
}
