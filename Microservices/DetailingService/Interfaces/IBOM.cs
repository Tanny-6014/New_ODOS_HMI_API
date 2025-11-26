using DetailingService.Dtos;
using DetailingService.Repositories;
namespace DetailingService.Interfaces
{
    public interface IBOM
    {
        Task<IEnumerable<Get_bomDto>> GetBomDetails(int intProductMarkingId, string BOMType, string StructureElement);

        bool InsertBOM(BOMInsert objBOM);

        //Task<IEnumerable<BOMHeaderDto>> GetBOMHeader(int intProductMarkingId, string nvchBOMType, string strStructureElement);

        public List<get_ShapeParamDetailsDTO> GetShapeParamDetails(int ShapeId);

        bool UpdateBOM_PROD(UpdateBomProd_dto ObjBOMupdate);


        List<BomService> GetBOMHeader(int intProductMarkingId, string nvchBOMType, string strStructureElement);


        Task<int> BOMDelete(int BOMDetailId);

        public List<Get_BendingGroup> BendingGroup_Get(int ShapeId);

    }
}
