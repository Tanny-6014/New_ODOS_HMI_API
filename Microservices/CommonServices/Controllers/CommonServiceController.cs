using AutoMapper;
using CommonServices.Dtos;
using CommonServices.Interfaces;
using CommonServices.Models;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Metrics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace ShapeCodeService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommonServiceController : ControllerBase 
    {
        private readonly ICommonService _CommonServiceRepository;
        private readonly IMapper _mapper;

        public CommonServiceController(ICommonService commonService, IMapper mapper)
        {
            _CommonServiceRepository = commonService;
            _mapper = mapper;

        }


        [HttpGet]
        [Route("/GetCustomers")]
        public async Task<IActionResult> GetCustomers()
        {
            try
            {
                List<CustomerMaster> CommonServiceList = await _CommonServiceRepository.GetCustomerListAsync();
                var result = _mapper.Map<List<CustomerDto>>(CommonServiceList);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
      
        }

        [HttpGet]
        [Route("/GetProject/{customerid}")]
        public async Task<IActionResult> GetProject(string customerid)
        {
            IEnumerable<ProjectMaster> CommonServiceList = await _CommonServiceRepository.GetProjectListAsync(customerid);
            var result = _mapper.Map<List<ProjectMasterDtos>>(CommonServiceList);
            return Ok(result);
        }
       
        [HttpGet]
        [Route("/GetProjectListForcontractList/{customerid}")]
        public async Task<IActionResult> GetProjectListForcontractListAsync(int customerid)
        {
            IEnumerable<ProjectMaster> CommonServiceList = await _CommonServiceRepository.GetProjectListForcontractListAsync(customerid);
            var result = _mapper.Map<List<ProjectMasterDtos>>(CommonServiceList);
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetContractListAsyncNew_Tanmay/{projectId}")]
        public async Task<IActionResult> GetContractListAsync(int projectId)
        {
            IEnumerable<ContractDto> ContractList = await _CommonServiceRepository.GetContractListAsync(projectId);
            var result = _mapper.Map<List<ContractDto>>(ContractList);
            return Ok(result);
        }

        

        [HttpGet]
        [Route("/GetProjectContractList")]
        public async Task<IActionResult> GetProjectContractList()
        {
            IEnumerable<ProjectContractListDto> ProjectContract_List = await _CommonServiceRepository.GetProjectContractListAsync();
            var result = _mapper.Map<List<ProjectContractListDto>>(ProjectContract_List);
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetProductType")]
        public async Task<IActionResult> GetProductType()
        {
            List<ProductType> CommonServiceList = await _CommonServiceRepository.GetProductListAsync();
            var result = _mapper.Map<List<ProductTypeDtos>>(CommonServiceList);
            return Ok(result);
        }


        [HttpGet]
        [Route("/GetProjectContractFilter/{CustomerCode}/{startDate}/{endDate}/{projectNo}/{contractNo}")]
        public async Task<IActionResult> GetProjectContractFilter(int CustomerCode, string startDate, string endDate,int projectNo,string contractNo)
        {
            IEnumerable<ProjectContractFilterDto> ProjectContractFilter = await _CommonServiceRepository.GetProjectContractFilterAsync(CustomerCode, startDate,endDate, projectNo, contractNo);
            var result = _mapper.Map<List<ProjectContractFilterDto>>(ProjectContractFilter);
            return Ok(result);
        }


        [HttpGet]
        [Route("/GetESMCustomers/{isESM}")]
        public async Task<IActionResult> GetESMCustomerList(bool isESM)
        {
            List<CustomerDto> result = _CommonServiceRepository.GetESMCustomerList(isESM);
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetUsers")]
        public async Task<IActionResult> GetUsersList()
        {
            List<getUsersDto> result = _CommonServiceRepository.GetUsersList();
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetEsmTonnageReport/{FromDate}/{ToDate}")]
        public async Task<IActionResult> GetEsmTonnageReport(string FromDate, string ToDate)
        {
            List<EsmTonnageReportDto> result = _CommonServiceRepository.GetEsmTonnageReport( FromDate,ToDate);
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetTonnageReport/{FromDate}/{ToDate}/{user}/{rptType}")]
        public async Task<IActionResult> GetTonnageReport(string FromDate, string ToDate,string user, string rptType)
        {
            List<TonnageReportDto> result = _CommonServiceRepository.GetTonnageReport(FromDate, ToDate,user,rptType);
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetUserId/{Username}")]
        public async Task<IActionResult> GetUserId(string Username)
        {
            var result = _CommonServiceRepository.GetUserIDByName(Username);
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetTonnageReportByCustomerAndProject/{fromDate}/{toDate}/{projectCode}/{rptType}")]
        public async Task<IActionResult> GetTonnageReportByCustomerAndProject(string fromDate, string toDate, string projectCode, string rptType)
        {
            List<TonnageReportDto> result = _CommonServiceRepository.GetTonnageReportByCustomerAndProject(fromDate, toDate, projectCode, rptType);
            return Ok(result);
        }


        [HttpGet]
        [Route("/GetBPCOrderReport/{FromDate}/{ToDate}")]
        public async Task<IActionResult> GetBPCOrderEntryReport(string FromDate, string ToDate)
        {
            List<BPCOrderReportDto> result = _CommonServiceRepository.GetBPCOrderEntryReport(FromDate, ToDate);
            return Ok(result);
        }

        //start - added by vishal
        [HttpGet]
        [Route("/GetCustomerName/{CustomerCode}")]
        public async Task<IActionResult> GetCustomerName(string CustomerCode)
        {
            var result = await _CommonServiceRepository.GetCustomerAsync(CustomerCode);
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetProjectName/{ProjectCode}")]
        public async Task<IActionResult> GetProjectName(string ProjectCode)
        {
            var result = await _CommonServiceRepository.GetProjectName(ProjectCode);
            return Ok(result);
        }
        //end - added by vishal new Added to checck 

        [HttpGet]
        [Route("/GetOrderStatus/{FromDate}/{ToDate}")]
        public async Task<IActionResult> GetOrderStatus(string FromDate, string ToDate,string? ProductType = null)
        {
            IEnumerable<dynamic> result = await _CommonServiceRepository.GetOrderStatus(FromDate, ToDate, ProductType);
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetMissingGIOrders/{FromDate}/{ToDate}")]
        public async Task<IActionResult> GetMissingGIOrders(string FromDate, string ToDate, string? ProductType=null)
        {
            IEnumerable<dynamic> result = await _CommonServiceRepository.GetMissingGIOrders(FromDate, ToDate, ProductType); 
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetProductType/{FromDate}/{ToDate}")]
        public async Task<IActionResult> GetProductType(string FromDate, string ToDate)
        {
            IEnumerable<dynamic> result = await _CommonServiceRepository.GetProductType(FromDate, ToDate);
            return Ok(result);
        }
    }

    public class CustomerData
    {
        public string CustomerSAPID { get; set; }
        public List<string> ProjectSAPIDs { get; set; }
    }
    //test

}
