
using Dapper;
using SAP_API.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace SAP_API.Modelss
{
    public class Contract
    {
        private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS";

        public int ContractId { get; set; }
        public string ContractName { get; set; }
        public string ContractDescription { get; set; }
        public string SAPContractCode { get; set; }
        public int SAPContractId { get; set; }
        public string SAPContractDescription { get; set; }
        //public Project Project { get; set; }ajit

        public List<Contract> GetContract(int CustomerId)
        {
            
            List<Contract> listGetContract = new List<Contract> { };
            DataSet dsGetContract = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTCUSTOMERCODE", CustomerId);

                    listGetContract = (List<Contract>)sqlConnection.Query<Contract>(SystemConstant.usp_Contract_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    return listGetContract;
                }
                
                
              
                if (dsGetContract != null && dsGetContract.Tables.Count != 0)
                {
                    foreach (DataRowView drvContract in dsGetContract.Tables[0].DefaultView)
                    {
                        Contract psContract = new Contract
                        {
                            ContractName = drvContract["VCHCONTRACTNUMBER"].ToString(),
                            ContractId = Convert.ToInt32(drvContract["INTCONTRACTID"].ToString()),
                            ContractDescription = drvContract["VCHNDSCONTRACTDESCRIPTION"].ToString(),
                            SAPContractCode = drvContract["SAPCONTRACTCODE"].ToString(),
                            SAPContractId = Convert.ToInt32(drvContract["SAPCONTRACTNO"].ToString()),
                            SAPContractDescription = drvContract["SAPDESCRIPTION"].ToString()
                        };
                        listGetContract.Add(psContract);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                
            }
            return listGetContract;
        }

        //added for ship to party
        public List<Contract> GetContract_new(int CustomerId, int ProjectId, int ProductTypeId)
        {
            
            List<Contract> listGetContract = new List<Contract> { };
            DataSet dsGetContract = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intprojectId", ProjectId);
                    dynamicParameters.Add("@intproducttypeid", ProductTypeId);

                    listGetContract = (List<Contract>)sqlConnection.Query<Contract>(SystemConstant.Contract_Get_new, dynamicParameters, commandType: CommandType.StoredProcedure);

                    return listGetContract;
                }

                if (dsGetContract != null && dsGetContract.Tables.Count != 0)
                {
                    foreach (DataRowView drvContract in dsGetContract.Tables[0].DefaultView)
                    {
                        Contract psContract = new Contract
                        {
                            ContractName = drvContract["vchcontractcode"].ToString(),
                            ContractId = Convert.ToInt32(drvContract["INTCONTRACTID"].ToString()),
                            ContractDescription = drvContract["VCHNDSCONTRACTDESCRIPTION"].ToString(),
                            SAPContractCode = drvContract["SAPCONTRACTCODE"].ToString(),
                            SAPContractId = Convert.ToInt32(drvContract["SAPCONTRACTNO"].ToString()),
                            SAPContractDescription = drvContract["SAPDESCRIPTION"].ToString()
                        };
                        listGetContract.Add(psContract);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                
            }
            return listGetContract;
        }

    }
}
