namespace OrderService.Constants
{
    public static class SystemConstant
    {
        public static string DefaultDBConnection = "DefaultConnection";

        public static string ddl_ProjectList = "GetOrder_ProjectList";

        public static string ddl_CustomerList = "GetOrder_CustomerList";

        public static string getActiveOrder_GridList = "GetActiveOrder_FilterData";

        public static string getDeliveredOrder_GridList = "GetDeliveredOrder_FilterData";


        //ESMTrackingGenerator
        public static string spGetDocNo_ESM = "spGetDocNo_ESM";

        public static string usp_ESMTrackingDetails_Insert = "usp_ESMTrackingDetails_Insert";

        public static string usp_ESMTrackingDetails_Update = "usp_ESMTrackingDetails_Update";

        public static string usp_TrackingDetails_Get_ESM = "usp_TrackingDetails_Get_ESM";
        public static string usp_TrackingDetailsByID_Get_ESM= "usp_TrackingDetailsByID_Get_ESM";
        public static string usp_ESM_BBSNumber_Get = "usp_ESM_BBSNumber_Get";
        public static string usp_PostingBBSNumberGeneration_Get = "usp_PostingBBSNumberGeneration_Get";
        public static string DeleteUpcomingOrder = "Delete_UpcomingOrder";
        public static string ConvertedUpcomingOrder = "UpdateConvertedOrderNo";
        public static string LoadData_ForUpcomingOrder = "GetDataForUpcomingOrders";
        public static string InsertData = "InsertDataUpcomingOrders";
        public static string DeleteSubmittedUpcomingOrder = "OES_DeleteSubmittedUpcomingOrder";
        public static string UpcomingNotification = "usp_OESUpcomingNotificationMail";
        //Precast by vidya
        public static string usp_getPrecastDetails = "usp_getPrecastDetails";
        public static string usp_PrecastDetaildInsert = "usp_PrecastDetaildInsert";

        //BarShapeCode
        public static string DeletePrecastDetailsById = "DeletePrecastDetailsById";
        public static string BarIdDetails_Update = "BarIdDetails_Update";


        public static string RecoverUpcomingOrder = "Recover_UpcomingOrder";

        public static string ResetConvertedOrder = "ResetConvertedOrderUpcoming";

        public static string usp_getBPCJobAdviceDetails = "usp_getBPCJobAdviceDetails";
        public static string usp_InsertBPCJobAdviceDetails = "usp_InsertBPCJobAdviceDetails";
        public static string usp_UpdateBPCJobAdviceDetails = "usp_UpdateBPCJobAdviceDetails";

        public static string usp_UpdatePrecastFlag = "usp_UpdatePrecastFlag";

        //public static string usp_GetOrderAssignment_OrdH = "GetOrderAssignment_OrdH_test";

        public static string usp_GetOrderAssignment_OrdH = "GetOrderAssignment_OrdH";

        public static string GetCustomer_OrdAssgn = "GetCustomer_OrdAssgn";

        public static string Save_OrderAssignment = "Save_OrderAssignment";

        public static string GetOrderAssignment_OrdItem = "GetOrderAssignment_OrdItem";

        public static string Save_OrderWithdraw = "Save_OrderWithdraw";

        //public static string GetOrderAssignment_Summary = "GetOrderAssignment_Summary";

        public static string GetOrderUpdateddata_IDB = "GetOrderUpdateddata_IDB";

        public static string usp_UpdateIncomingData = "usp_UpdateIncomingData";


        public static string usp_InsertAutoReleaseCutoverData = "usp_InsertAutoReleaseCutoverData";

        public static string GetOrderAssignment_Outsource_OrdH = "GetOrderAssignment_Outsource_OrdH";

        public static string GetFullOutsourceOrderDetails = "GetFullOutsourceOrderDetails";

        public static string GetSupplier_BatchPrinting = "GetSupplier_BatchPrinting";
        public static string GetSONO_BatchPrinting = "GetSONO_BatchPrinting";

        public static string GetBatch_BatchPrinting = "GetBatch_BatchPrinting";
    }
}
