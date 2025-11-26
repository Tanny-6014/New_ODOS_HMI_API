using Dapper;

using Microsoft.Data.SqlClient;
using System.Data;
using static Dapper.SqlMapper;
using UtilityService.Constants;

namespace UtilityService.Repositories
{
    public class ParameterSet
    {
        #region "Properties .."
        /* Tactron Properties */
        public int ParameterSetNumber { get; set; }
        public int ParameterSetId { get; set; }
        public int TopCover { get; set; }
        public int BottomCover { get; set; }
        public int LeftCover { get; set; }
        public int RightCover { get; set; }
        public int Gap1 { get; set; }
        public int Gap2 { get; set; }

        public string CustomerName { get; set; }
        public int CustomerCode { get; set; }

        public string ContractNo { get; set; }
        public int ContractID { get; set; }

        public string AllContractName { get; set; }
        public int AllContractID { get; set; }
        public int AllCustomerCode { get; set; }

        public string ProjectCode { get; set; }
        public int ProjectID { get; set; }
        public string ProjectDescription { get; set; }

        public string TransportCode { get; set; }
        public int TransportID { get; set; }
        public string TransportDesc { get; set; }

        public string CappingProduceCode { get; set; }
        public string CappingProductDescription { get; set; }
        public string strDiameter { get; set; }
        public int CappingProductCodeID { get; set; }
        public int CappingCWLength { get; set; }

        public string ClinkProduceCode { get; set; }
        public string ClinkProductDescription { get; set; }
        public string ClinkDiameter { get; set; }
        public int ClinkProductCodeID { get; set; }
        public int ClinkCWLength { get; set; }

        public int ProductCode { get; set; } //added for ship to party
        public string ShipToParty { get; set; } //added for ship to party

        #endregion
      
        private readonly IConfiguration _configuration;
        //private string connectionString;
        private string connectionString = "Server=NSQADB5\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=MES_USER;Password=Mes@123;Connection Timeout=36000000";
        //private string connectionString = "Server=nsprddb10\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";

        #region " List.. "
        public ParameterSet()
        {

        }
     

        public static List<ParameterSet> GetParameterSetsByProjectID(int projectID)
        {
            List<ParameterSet> ParameterSetList = new List<ParameterSet> { };
            return ParameterSetList;
        }

        public List<ParameterSet> GetCappingProductCodes(string CappingProduct)
        {
            
            List<ParameterSet> cappingProductCodeList = new List<ParameterSet> { };

            DataSet dsGetCappingProductDetails = new DataSet();

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@CAPPINGPRODUCT", CappingProduct);
                    //  dsGetCappingProductDetails = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_CappingProduct_Get");
                    dsGetCappingProductDetails = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.CappingProduct_Get, dynamicParameters, commandType: CommandType.StoredProcedure);


                    if (dsGetCappingProductDetails != null && dsGetCappingProductDetails.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBeam in dsGetCappingProductDetails.Tables[0].DefaultView)
                        {
                            ParameterSet parameterSet = new ParameterSet
                            {
                                CappingProduceCode = drvBeam["VCHPRODUCTCODE"].ToString(),
                                CappingProductCodeID = Convert.ToInt32(drvBeam["INTPRODUCTCODEID"]),
                                CappingProductDescription = drvBeam["VCHPRODUCTDESCRIPTION"].ToString(),
                                strDiameter = drvBeam["DECMWDIAMETER"].ToString(),
                                CappingCWLength = Convert.ToInt32(drvBeam["DECCWLENGTH"])
                            };
                            cappingProductCodeList.Add(parameterSet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return cappingProductCodeList;
        }

        public List<ParameterSet> GetClinkProductCodes(string ClinkProduct)
        {
            
            List<ParameterSet> clinkProductCodeList = new List<ParameterSet> { };

            DataSet dsGetClinkProductDetails = new DataSet();

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@CLINKPRODUCT", ClinkProduct);
                    // dsGetClinkProductDetails = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_ClinkProduct_Get");
                    dsGetClinkProductDetails = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.ClinkProduct_Get, dynamicParameters, commandType: CommandType.StoredProcedure);


                    if (dsGetClinkProductDetails != null && dsGetClinkProductDetails.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBeam in dsGetClinkProductDetails.Tables[0].DefaultView)
                        {
                            ParameterSet parameterSet = new ParameterSet
                            {
                                ClinkProduceCode = drvBeam["VCHPRODUCTCODE"].ToString(),
                                ClinkProductCodeID = Convert.ToInt32(drvBeam["INTPRODUCTCODEID"]),
                                ClinkProductDescription = drvBeam["VCHPRODUCTDESCRIPTION"].ToString(),
                                ClinkDiameter = drvBeam["DECMWDIAMETER"].ToString(),
                                ClinkCWLength = Convert.ToInt32(drvBeam["DECCWLENGTH"])
                            };
                            clinkProductCodeList.Add(parameterSet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return clinkProductCodeList;
        }

        public List<ParameterSet> GetCustomer()
        {
            
            List<ParameterSet> listGetCustomer = new List<ParameterSet> { };
            DataSet dsGetCustomer = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    //var dynamicParameters = new DynamicParameters();

                    // dsGetCustomer = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_Customer_Get");
                    dsGetCustomer = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.Customer_Get,  commandType: CommandType.StoredProcedure);


                    if (dsGetCustomer != null && dsGetCustomer.Tables.Count != 0)
                    {
                        foreach (DataRowView drvCustomer in dsGetCustomer.Tables[0].DefaultView)
                        {
                            ParameterSet psCustomer = new ParameterSet
                            {
                                CustomerName = drvCustomer["VCHCUSTOMERNAME"].ToString(),
                                CustomerCode = Convert.ToInt32(drvCustomer["INTCUSTOMERCODE"].ToString())
                            };
                            listGetCustomer.Add(psCustomer);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
            return listGetCustomer;
        }

        public List<ParameterSet> GetContract()
        {
            
            List<ParameterSet> listGetContract = new List<ParameterSet> { };
            DataSet dsGetContract = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTCUSTOMERCODE", this.CustomerCode);
                    // dsGetContract = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_Contract_Get");
                    dsGetContract = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.Contract_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (dsGetContract != null && dsGetContract.Tables.Count != 0)
                    {
                        foreach (DataRowView drvContract in dsGetContract.Tables[0].DefaultView)
                        {
                            ParameterSet psContract = new ParameterSet
                            {
                                ContractNo = drvContract["VCHCONTRACTNUMBER"].ToString(),
                                ContractID = Convert.ToInt32(drvContract["INTCONTRACTID"].ToString())
                            };
                            listGetContract.Add(psContract);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
            return listGetContract;
        }

        public List<ParameterSet> GetProject()
        {
            
            List<ParameterSet> listGetProject = new List<ParameterSet> { };
            DataSet dsGetProject = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTCONTRACTID", this.ContractID);
                    //dsGetProject = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_Project_Get");
                    dsGetProject = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.Project_Get, dynamicParameters, commandType: CommandType.StoredProcedure);


                    if (dsGetProject != null && dsGetProject.Tables.Count != 0)
                    {
                        foreach (DataRowView drvProject in dsGetProject.Tables[0].DefaultView)
                        {
                            ParameterSet psProject = new ParameterSet
                            {
                                ProjectCode = drvProject["VCHPROJECTCODE"].ToString(),
                                ProjectID = Convert.ToInt32(drvProject["INTProjectID"].ToString()),
                                ProjectDescription = drvProject["VCHPROJECTNAME"].ToString()
                            };
                            listGetProject.Add(psProject);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
            return listGetProject;
        }

        public List<ParameterSet> GetTransport()
        {
            
            List<ParameterSet> listGetTransport = new List<ParameterSet> { };
            DataSet dsGetTransport = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    //dsGetTransport = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_Transport_Get");

                    dsGetTransport = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.Transport_Get, commandType: CommandType.StoredProcedure);


                    if (dsGetTransport != null && dsGetTransport.Tables.Count != 0)
                    {
                        foreach (DataRowView drvTransport in dsGetTransport.Tables[0].DefaultView)
                        {
                            ParameterSet psTransport = new ParameterSet
                            {
                                TransportCode = drvTransport["VCHTRANSPORTMODE"].ToString(),
                                TransportID = Convert.ToInt32(drvTransport["TNTTRANSPORTMODEID"].ToString()),
                                TransportDesc = drvTransport["VCHTRANSPORTDESCRIPTION"].ToString()
                            };
                            listGetTransport.Add(psTransport);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }           
            return listGetTransport;
        }

        //added for ship to party
        public List<ParameterSet> GetContract_new()
        {
            
            List<ParameterSet> listGetContract = new List<ParameterSet> { };
            DataSet dsGetContract = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.Add( "@intprojectId", this.ProjectID);
                    dynamicParameters.Add( "@intproducttypeid", this.ProductCode);

                   // dsGetContract = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "contract_Get_new");
                    dsGetContract = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.contract_Get_new, dynamicParameters, commandType: CommandType.StoredProcedure);


                    if (dsGetContract != null && dsGetContract.Tables.Count != 0)
                    {
                        foreach (DataRowView drvContract in dsGetContract.Tables[0].DefaultView)
                        {
                            ParameterSet psContract = new ParameterSet
                            {
                                ContractNo = drvContract["VCHCONTRACTCODE"].ToString(),
                                ContractID = Convert.ToInt32(drvContract["INTCONTRACTID"].ToString())
                            };
                            listGetContract.Add(psContract);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
            return listGetContract;
        }

        public List<ParameterSet> GetProject_new()
        {
            
            List<ParameterSet> listGetProject = new List<ParameterSet> { };
            DataSet dsGetProject = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.Add( "@intCustomerID", this.CustomerCode);
                    //dsGetProject = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "Project_Get_new");
                    dsGetProject = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.Project_Get_new, dynamicParameters, commandType: CommandType.StoredProcedure);


                    if (dsGetProject != null && dsGetProject.Tables.Count != 0)
                    {
                        foreach (DataRowView drvProject in dsGetProject.Tables[0].DefaultView)
                        {
                            ParameterSet psProject = new ParameterSet
                            {
                                ProjectCode = drvProject["vchProjectCode"].ToString(),
                                ProjectID = Convert.ToInt32(drvProject["INTProjectID"].ToString()),
                                ProjectDescription = drvProject["VCHPROJECTNAME"].ToString(),
                                //ShipToParty = drvProject["vchshiptoparty"].ToString(),
                            };
                            listGetProject.Add(psProject);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
            return listGetProject;
        }

        //end added for ship to party

        #endregion

    }
}
