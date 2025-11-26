using DetailingService.Repositories;

namespace DetailingService.Interfaces
{
    public interface ICarpetService
    {

        List<ShapeCodeParameterSet> CarpetParameterSetbyProjIdProdType(int projectId, int productTypeId, out string errorMessage);


        List<ProductCode> PopulateProductCode(out string errorMessage);


        List<ProductCode> FilterProductCode(string enteredText, out string errorMessage);


        List<ShapeCode> PopulateShapeCode(out string errorMessage);


        List<ShapeCode> FilterShapeCode(string enteredText, out string errorMessage);


        List<CarpetStructure> GetStructureMarkingDetails(int seDetailingID, int structureElementId, out string errorMessage);


        List<CarpetProduct> InsertCarpetStructureMarking(CarpetStructure objStructureMarking, int structureElementId, int projectTypeId, int projectId, int productTypeId, out string errorMessage, int userId);


        List<CarpetProduct> UpdateCarpetStructureMarkingWithProductGeneration(ref CarpetStructure objStructureMarking, int structureElementId, int projectTypeId, int projectId, int productTypeId, out string errorMessage, int userId);


        bool UpdateCarpetStructureMarkingWithoutProductGeneration(ref CarpetStructure objStructureMarking, int structureElementId, int projectTypeId, out string errorMessage, int userId);


        bool DeleteStructureMarking(CarpetStructure objStructureMarking, out string errorMessage);


        CarpetProduct UpdateCarpetProductMarking(CarpetStructure structureMarking,  CarpetProduct productMark,  int currentIndex, int structureElementId,  bool machineCheck,  string bendingCheck,  bool transportCheck, out string errorMessage, int userId);


        CarpetProduct SaveProductMarkafterConfirmation(CarpetStructure structureMarking, CarpetProduct productMark, ref int currentIndex, int structureElementId, int userId, out string errorMessage);


        bool DeleteProductMarking(CarpetProduct productMark, int seDetailingId, out string errorMessage);


        List<CarpetStructure> RegenerateValidation(List<CarpetStructure> structureMarkList, ShapeCodeParameterSet parameterSet, int structureElementId, int projectTypeId, int projectId, int productTypeId, out string errorMessage, int userId);


        CarpetProduct InsertCarpetProductMarking(CarpetProduct objProductMarkingMarking, int structureElementId, out string errorMessage, int userId);


        List<CarpetProduct> GetOverHang(int parameterSetNumber, int projectId, int structureElementId, int productTypeId, int mwLength, int cwLength, int mwSpace, int cwSpace, out string errorMessage);

        //[OperationContract]  // CARPET ANURAN
        //int PopulateMO1(int MO1, out string errorMessage);

    }
}
