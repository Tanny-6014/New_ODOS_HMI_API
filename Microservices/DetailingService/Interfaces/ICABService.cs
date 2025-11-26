using DetailingService.Dtos;
using DetailingService.Repositories;

namespace DetailingService.Interfaces
{
    public interface ICABService
    {
        List<CABItem> GetCABProductMarkingDetailsBySEDetailingID(int intSEDetailingID, ref string errorMessage);



        List<CABItem> PopulateProduceInd(out string errorMessage);

        List<CABItem> FilterProduceInd(string enteredText, out string errorMessage);

        List<ShapeCode> FilterShapeCode(string enteredText, out string errorMessage);
        //public ShapeParameterCollection ShapeParameterForCab_Get(string enteredText, out string errorMessage);







        //START:ADDED BY SIDDHI FOR CAB MODULE ENHANCEMENT CR ON 13/12/2018

        List<ShapeCode> AllCABShapeCodes();
        //END




        byte[] GetImage(string shapeCode);

        bool DeleteCabMarkingDetail(CABItem objCabMarkingDetail, out string errorMsg);



        List<CABItem> UpdateCABProductMarking(CABItem objCabproductMarking, out string errorMessage, int SEDetailingID, out int minLen, out int minHookLen, out int minHookHt, out bool flag);

        List<CABItem> UpdateCABProductMarkingVar(List<CABItem> list, out string errorMessage, int userId, int SEDetailingID, int qty);



        List<ShapeCode> FilterShapeCodeEdit(string enteredText, int cabProdmarkId, out string errorMessage);

        List<ShapeCode> FilterShapeParam_Edit_Get(string shapeCode, out string errorMessage);

        List<Accessory> GetProdInvLength(CABItem objCab, out string errorMsg, out int invoiceLength, out int prodLength, out int r_intTotBend, out int r_intTotArc, out string bvbs);



        void GetPin(string grade, int dia, int groupMarkId, int productTypeId, out string errorMsg, out int pin, out int minLength, out int minHookLen, out int minHookHt);

        void GetPinByFormerFlag(string grade, int dia, int formerFlag, out string errorMsg, out int pin, out int minLength, out int minHookLen, out int minHookHt);

        bool ValidatePin(string grade, int dia, int pin, out string errorMsg, out int minLength, out int minHookLen, out int minHookHt);

        bool InsertVariableLength(CABItem objcab, List<ShapeParameter> sourceList, List<ShapeParameter> targetList, int cabProdMarkId, int count, out string errorMessage);

        int InsertProductMark(CABItem objCab, int SEDetailingID, out string errorMessage);

        List<ShapeCode> GetShapeCodeAndParam(string enteredText, int cabProdmarkId, int seDetailingId, string barMark, out string errorMessage);

        bool DeletePoductMark(List<CABItem> deleteList, out string errorMsg);



        int SetHeightAngle(List<ShapeParameter> shapeparam, string param, string shape);




        int CopyBarMark(int cabPodMarkId, string nextBarMark, out string errorMessage);

        //bool InsertToDbTCT(List<InsertToDbTCTDto> lstCab);
        bool InsertToDbTCT(CABItem lstCab,bool isValidated, out int ProductMarkId);



        bool InsertToDbTCT(CABItem lstCab, bool isValidated, out string errorMsg);


        //int GetSeDetailingId(int groupMarkId, out string errorMessage);

        //bool GetReleasedPostedGroupMark(int groupMarkId, out string errorMessage);

        bool ValidateAndSave(int groupMarkId, out string errorMsg);

        List<CABItem> InsertProductMarkInLine(InsertProductMarkInLineDto objCab, int SEDetailingID, string barMarkStart, out string errorMessage);

       
    }
}
