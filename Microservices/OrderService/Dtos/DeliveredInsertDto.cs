using Oracle.ManagedDataAccess.Client;
using System.Data.SqlClient;

namespace OrderService.Dtos
{
    public class DeliveredInsertDto
    {
            public List<string> pSOR{get;set;} 
            public string pCustomerCode{get;set;} 
            public string pProjectCode{get;set;} 
            public string pClient{get;set;} 
            public string pUserID{get;set;} 
            public SqlConnection pNDSCon{get;set;} 
            public OracleConnection pCISCon{get;set;}
            public string DateFrom{get;set;}
            public string DateTo { get; set; }
    }
}
