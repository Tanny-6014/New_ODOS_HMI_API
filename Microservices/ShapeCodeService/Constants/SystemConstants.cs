namespace ShapeCodeService.Constants
{
    public class SystemConstants
    {
        public static string DefaultDBConnection = "DefaultConnection";

        public static string usp_ShapeSurcharge_get = "usp_ShapeSurcharge_get";

        public static string ShapeEditable_Get_cube = "ShapeEditable_Get_cube";
        public static string USP_SHAPE_DELETE_cube = "USP_SHAPE_DELETE_cube";
        public static string UpdateShapeParameters_cube = "UpdateShapeParameters_cube";
        public static string CABShapeCatalogDetails_Get = "CABShapeCatalogDetails_Get";
        public static string USP_GET_SHAPE_IMAGE = "USP_GET_SHAPE_IMAGE";
        public static string CABShapeCoords_get_cube = "CABShapeCoords_get_cube";
        public static string CABShapeDetails_Get_cube = "CABShapeDetails_Get_cube";
        public static string CABShapeMaster_Insert_cube = "CABShapeMaster_Insert_cube";
        public static string CABShape_InsUpd = "CABShape_InsUpd";
        //new
        public static string CABShapeCoords_get_cube_New = "CABShapeCoords_get_cube_New";
        public static string USP_ImportImageToDB = "USP_ImportImageToDB";
        public static string USP_GET_SHAPE_IMAGE_ALL = "USP_GET_SHAPE_IMAGE_ALL";
        public static string USP_GET_ShapeCodeDetails_Grid = "USP_GET_ShapeCodeDetails_Grid";
        public static string USP_INSERT_ShapeCodeDetails_Grid = "USP_INSERT_ShapeCodeDetails_Grid";
        public static string USP_Delete_ShapeCodeDetails_Grid = "USP_Delete_ShapeCodeDetails_Grid";


        //DrawShape
        public static string CABShapeDetails_Insert_cube = "CABShapeDetails_Insert_cube";
        public static string InsertShapeGroup = "InsertShapeGroup";
        public static string CABShapeMaster_Delete_cube = "CABShapeMaster_Delete_cube";
        public static string CABShapeDetails_Delete_cube = "CABShapeDetails_Delete_cube";
        public static string CABShapeStatus_Update = "CABShapeStatus_Update";


        //Mesh shape code master
        public static string Shape_Get = "Shape_Get";
        public static string ShapeParam_Get = "ShapeParam_Get";
        public static string WBSStatus_Get = "WBSStatus_Get";
        public static string ShapeCodeDetails_Get = "ShapeCodeDetails_Get";
        public static string ShapeParam_delete = "ShapeParam_delete";
        public static string ShapeParam_InsUpd = "ShapeParam_InsUpd";
        public static string CheckShapeExists = "CheckShapeExists";
        public static string CheckMeshShapeGroupExists = "CheckMeshShapeGroupExists";
        public static string ShapeDetails_InsUpd = "ShapeDetails_InsUpd";
        public static string ShapeDetails_Insert = "ShapeDetails_Insert";//new

        public static string ShapeDetails_update = "ShapeDetails_update";//new
        public static string USP_GET_MESHSHAPE_IMAGE_ALL = "USP_GET_MESHSHAPE_IMAGE_ALL";

        //addValidation
        public static string ValidationParameters_Get = "ValidationParameters_Get"; //new
        public static string Get_InsertedAttributes = "Get_InsertedAttributes";//new
        public static string usp_InsertIpOpValidationConstraints = "usp_InsertIpOpValidationConstraints"; //new
        public static string usp_UpdateIpOpValidationConstraints = "usp_UpdateIpOpValidationConstraints";//new
        public static string usp_DeletIpOpValidationConstraints = "usp_DeletIpOpValidationConstraints";//new

        //add product marking formulae
        public static string Get_FromulaList = "Get_FromulaList"; //new
        public static string Get_FromulaListbyStructEle = "Get_FromulaListbyStructEle";//created
        public static string InsertProductMarkingFormulae = "InsertProductMarkingFormulae";
        public static string usp_InsertProductMarkingFormulae = "usp_InsertProductMarkingFormulae";//created
        public static string usp_UpdateFormula = "usp_UpdateFormula";//created
        public static string Get_LibraryID = "Get_LibraryID";//created

    }
}
