using CommonServices.Dtos;
using CommonServices.Models;

namespace CommonServices.Interfaces
{
    public interface ICommonService
    {
        Task<List<CustomerMaster>> GetCustomerListAsync();
        //Task<List<ProjectMaster>> GetProjectListAsync();
        Task<List<ProductType>> GetProductListAsync();
        Task<IEnumerable<ContractDto>> GetContractListAsync(int projectId);

        Task<IEnumerable<ProjectMaster>> GetProjectListAsync(string customerid);
        Task<IEnumerable<ProjectMaster>> GetProjectListForcontractListAsync(int customerid);


        Task<IEnumerable<ProjectContractListDto>> GetProjectContractListAsync();
        Task<IEnumerable<ProjectContractFilterDto>> GetProjectContractFilterAsync(int CustomerCode, string startDate,string endDate, int projectNo, string contractNo);

        public List<CustomerDto> GetESMCustomerList(bool isEsm);
        public List<getUsersDto> GetUsersList();
        public List<EsmTonnageReportDto> GetEsmTonnageReport(string FromDate,string ToDate);
        public List<TonnageReportDto> GetTonnageReport(string FromDate, string ToDate, string user, string rptType);
        public List<BPCOrderReportDto> GetBPCOrderEntryReport(string FromDate, string ToDate);

        public List<TonnageReportDto> GetTonnageReportByCustomerAndProject(string fromDate, string toDate, string projectCode, string rptType);



        public int GetUserIDByName(string name);

        public Task<ProjectMaster?> GetProjectName(string ProjectCode);
        public Task<CustomerMaster?> GetCustomerAsync(string customerId);

        Task<IEnumerable<dynamic>> GetOrderStatus(string FromDate, string ToDate, string? ProductType = null);
        Task<IEnumerable<dynamic>> GetMissingGIOrders(string FromDate, string ToDate, string? ProductType = null);

        Task<IEnumerable<dynamic>> GetProductType(string FromDate, string ToDate);
    }
}
