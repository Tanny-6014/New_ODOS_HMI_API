namespace OrderService.Interfaces
{
    public interface IInterface
    {
        Task<(bool IsSuccess, string Message)> Delete_OrderAssignmentData(string orderRequestNo);

        Task<(bool IsSuccess, string Message)> Delete_ODOS_Scnhell_Data(string orderRequestNo);

        Task<(bool IsSuccess, string Message)> Update_OrderAssignmentData(string orderNo, int LayerNo, string LoadNo);

        Task<(bool IsSuccess, string Message)> Update_OrderAssignmentData(string OrderRequestNo);
        Task<(bool IsSuccess, string Message)> Update_OrderAssignmentData_Schedule();
        Task<(bool IsSuccess, string Message)> POST_OrderAssignmentData();


    }
}
