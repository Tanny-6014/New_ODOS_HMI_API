

using DetailingService.Dtos;
using DetailingService.Repositories;

namespace DetailingService.Interfaces
{
    public interface ISlabService
    {

        List<ShapeCodeParameterSet> SlabParameterSetbyProjIdProdType(int projectId, int productTypeId);


        List<ProductCode> PopulateProductCode(out string errorMessage);


        List<ProductCode> FilterProductCode(string enteredText);


        List<ShapeCode> PopulateSlabShapeCode();


        List<ShapeCode> FilterShapeCode(string enteredText, out string errorMessage);


        List<SlabStructure> GetStructureMarkingDetails(int seDetailingID, int structureElementId, out string errorMessage);


        List<SlabProduct> InsertSlabStructureMarking(SlabStructure objStructureMarking, int structureElementId, int projectTypeId, int projectId, int productTypeId, out string errorMessage, int userId);


        List<SlabProduct> UpdateSlabStructureMarkingWithProductGeneration(SlabStructure objStructureMarking, int structureElementId, int projectTypeId, int projectId, int productTypeId, out string errorMessage, int userId);


        bool UpdateSlabStructureMarkingWithoutProductGeneration(SlabStructure objStructureMarking, int structureElementId, int projectTypeId, out string errorMessage, int userId);


       // bool DeleteStructureMarking(SlabStructure objStructureMarking, out string errorMessage);

        bool DeleteStructureMarking(int StructureMarkId);

       

        SlabProduct UpdateSlabProductMarking(SlabStructure structureMarking, SlabProduct productMark, int currentIndex, int structureElementId, bool machineCheck, string bendingCheck, bool transportCheck, int userId, out string errorMessage);



        SlabProduct SaveProductMarkafterConfirmation(SlabStructure structureMarking, SlabProduct productMark, ref int currentIndex, int structureElementId, int userId, out string errorMessage);




        bool DeleteProductMarking(int productMarkid, int seDetailingId, out string errorMessage);


        List<SlabStructure> RegenerateValidation(List<SlabStructure> structureMarkList, ShapeCodeParameterSet parameterSet, int structureElementId, int projectTypeId, int projectId, int productTypeId, int userId, out string errorMessage);

        public SlabProduct InsertSlabProductMarking(SlabProduct objProductMarkingMarking, int structureElementId, int userId);


        List<SlabProduct> GetOverHang(int parameterSetNumber, int projectId, int structureElementId, int productTypeId, int mwLength, int cwLength, int mwSpace, int cwSpace, out string errorMessage);

        //by vidhya 

        bool DeleteProductMarking(SlabProduct productMark, int seDetailingId, out string errorMessage);

    }
}