using OrderService.Dtos;

namespace OrderService.Interfaces
{
    public interface IOrder
    {
        Task<IEnumerable<ProjectListDto>> GetProjectAsync(string CustomerCode);

        Task<IEnumerable<CustomerListDto>> GetCustomerAsync(string UserName);

        Task<IEnumerable<ActiveGridListDto>> GetActiveGridList(string customerCode, string projectCode);

        Task<IEnumerable<DeliveredGridListDto>> GetDeliveredGridList(string customerCode, string projectCode);
        Task<bool> DeleteUpcomingOrder(string OrderNumber);

        Task<bool> ConvertedUpcomingOrder(string pOrderNumber, string nOrderNumber, string LoginUser);

        Task<IEnumerable<UpcomingJobAdviceListDto>> LoadDataFor_UpcomingOrder(string customerCode, string projectCode);

        Task<IEnumerable<UpcomingJobAdviceListDto>> InsertIntoUpcomingOrderData(UpcomingJobAdviceListDto insertDataUpcomingOrdersDto);

        Task<bool> DeletedSubmittedUpcomingOrder(string pOrderNumber);

        Task<IEnumerable<OESUpcomingNotificationMailDto>> UpcomingNotificationMail(string pOrderNumber);

        Task<bool> ResetUpcomingOrder(int pOrderNo);

        Task<bool> GetUpdateIncomingData(string orderNumber);

        Task<bool> InserAutoReleaseData();

        



    }
}
