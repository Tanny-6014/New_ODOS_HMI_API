using DetailingService.Dtos;
using DetailingService.Repositories;

namespace DetailingService.Interfaces
{
    public interface IMeshDetailing
    {
        Task<IEnumerable<GetGroupMarkListDto>> GetMeshDetailingListAsync(int ProjectID);
        Task<IEnumerable<ShapeCodeParameterSetDto>> SlabParameterSetbyProjIdProdType(int projectId, int productTypeId);
        List<BeamDetailinfo> PopulateHeaderByProjectId(int projectId);
        Task<int> DeleteGroupMarkAsync(int INTGROUPMARKID);
        Task<IEnumerable<MeshStructureMarkingDetailsDto>> MeshStructureMarkingDetails_Get();

        Task<IEnumerable<MeshStructureMarkingDetailsDto>> CarpetStructureMarkingDetails_Get();

        Task<IEnumerable<AddSlabProductMarkingDto>> MeshProductMarkingDetails_Get(int StructureMarkID);
        int GroupMarkList_Edit(ReleaseGroupMarkDto groupMarkDto);     
        int ValidatedPostedGM(int GroupMarkId, out string errorMessage);
        bool SaveGroupMark(GroupMark groupmark, out int GMId, out string InserFlag, out string errorMessage);
     

    }
}
