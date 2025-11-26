using DetailingService.Dtos;
using DetailingService.Repositories;

namespace DetailingService.Interfaces
{
    public interface IColumnService
    {
        
        List<ColumnStructure> GetStructureMarkingDetails(int projectID, int seDetailingID, int structureElementTypeID, int productTypeID);

        
        List<ColumnProduct> InsertColumnStructureMarking(ColumnStructure objStructureMarking, int topCover, int bottomCover, int leftCover, int rightCover, int leg, int seDetailingID, int userId, out string errorMessage);

        
        List<ColumnProduct> UpdateColumnStructureMarking( ColumnStructure objStructureMarking,int topCover, int bottomCover, int leftCover, int rightCover, int leg, int seDetailingID, int userId, out string errorMessage);

        
        bool DeleteStructureMarking(int StructureMarkId, out string errorMsg);

        
        List<ColumnStructure> RegenerateValidation(List<ColumnStructure> structureMarkList, int topCover, int bottomCover, int leftCover, int rightCover, int leg, int seDetailingID, int userId, int structureElementId,out string errorMessage);

        
        List<ShapeCodeParameterSet> ColumnParameterSetbyProjIdProdType(int projectId, int productTypeId);

        
        List<ShapeCode> PopulateShapeCode();

        
        List<ShapeCode> FilterShapeCode(string enteredText, out string errorMsg);

        
        List<ProductCode> PopulateProductCode(int structureElementTypeID, int productTypeID);

        
        List<ProductCode> FilterProductCode(int structureElementTypeID, int productTypeID, string enteredText, out string errorMsg);

        
        List<ProductCode> PopulateClinkProductLength(out string errorMsg);


        List<ProductCode> FilterClinkProduct(string enteredText);

        
        bool UpdateGroupMarking(int SeDetailId, int ParamSetNumber);

        List<ProductCode> CoreCagePopulateProductCode();

        List<ProductCode> CoreCageSelectdProductCode(int GroupMarkId);

        List<ColumnLegDto> GetColumnLeg(int Parameternumber, int productID);

        public int UpdateStructureMarkingDetails(int structureMarkID, string structuremarkingname, int qty, out string errormsg);



    }
}
