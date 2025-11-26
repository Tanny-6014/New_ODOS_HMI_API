using OrderService.Dtos;
using OrderService.Models;
using static OrderService.Models.SupplierDataResponse;
namespace OrderService.Interfaces
{
    public interface IOrderAssignment
    {
        List<OrderDto> GetCabOrders(DateTime? FromReqDelDate = null, DateTime? ToReqDelDate = null, string? Customercode = null,
            string? Project = null, string? Producttype = null, string? Status = null, string? OrderNo = null);

        List<OrderItemDet> GetOrderItems(string OrderRequestNo);
        List<CustomerOrdAssgn> GetCustomersOrdAssgn();

        bool SaveOrderAssignment(List<string> OrderNoList, string AssignedTo);
        bool SaveOrderWithdraw(List<string> OrderNoList);
        List<OrderDto> GetOrdersOutsource(DateTime? FromReqDelDate = null, DateTime? ToReqDelDate = null, string? Customercode = null,
        string? Project = null, string? Producttype = null, string? Status = null, string? OrderNo = null);

        Task<IEnumerable<FullOutsource>> GetFullOrdersDetails(List<string> orderRequestNos);
        Task<(bool IsSuccess, string Message)> GenerateBatchForOneAsync(GenerateBatchRequest request);
        Task<(bool IsSuccess, string Message, List<SupplierRes> Data)> PostSupplierDataAsync(string? productType);
        Task<(bool IsSuccess, string Message)> AssignOrderOutsource(GenerateBatchRequest req);
        Task<(bool IsSuccess, string Message)> Delete_OutsourceData(string orderNo);
        Task<IEnumerable<dynamic>> GetSupplier_BatchPrinting();

        Task<IEnumerable<dynamic>> GetSONO_BatchPrinting(int NoofDays, string Vendorcode);
        Task<IEnumerable<dynamic>> GetBatch_BatchPrinting(string SONo);

    }
}
