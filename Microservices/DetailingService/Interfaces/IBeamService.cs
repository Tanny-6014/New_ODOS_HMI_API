using DetailingService.Repositories;

namespace DetailingService.Interfaces
{
    public interface IBeamService
    {
       
        List<BeamStructure> GetStructureMarkingDetails(string groupMarkName, int projectID, int seDetailingID, int structureElementTypeID, int productTypeID, int groupMarkID, out string errorMessage);

       
        bool DeleteStructureMarking(int StructureMarkId, out string errorMessage);

       
        List<ProductCode> PopulateProductCode(int structureElementTypeID, int productTypeID);

       
        List<ProductCode> FilterProductCode(int structureElementTypeID, int productTypeID, string enteredText, out string errorMessage);

       
        List<ShapeCode> PopulateShapeCode();

       
        List<ShapeCode> FilterShapeCode(string enteredText, out string errorMessage);

       
        List<ProductCode> PopulateCapProductCode();

       
        List<ProductCode> FilterCapProductCode(string enteredText, out string errorMessage);

       
        List<BeamProduct> VerifyStructureMarkingInputsNew( BeamStructure objStructureMarking, out string errorMessage, int gap1, int gap2, int topCover, int bottomCover, int leftCover, int rightCover, int hook, int leg, int seDetailingID, out string bendingcheck);

       
        List<BeamProduct> EditEnd(BeamStructure objStructureMarking, out string errorMessage, int gap1, int gap2, int topCover, int bottomCover, int leftCover, int rightCover, int hook, int leg, int seDetailingID, out string bendingcheck);

       
        List<BeamStructure> RegenerateValidation(List<BeamStructure> structureMarkList, int gap1, int gap2, int topCover, int bottomCover, int leftCover, int rightCover, int hook, int leg, int seDetailingID, int structureElementId, out string errorMessage, out string bendingcheck);

       
        bool UpdateGroupMarking(int SeDetailId, int ParamSetNumber, out string errorMessage);

       
        List<ShapeCodeParameterSet> ParameterSetByProjectProductTypeId(int ProjectId, int productTypeID);

       
        List<ShapeCodeParameterSet> ParameterSetByProjectID(int GroupMarkId, out string errorMessage);

        public int UpdateStructureMarkingDetails(int structureMarkID, string structuremarkingname, int qty, out string errormsg);

    }
}
