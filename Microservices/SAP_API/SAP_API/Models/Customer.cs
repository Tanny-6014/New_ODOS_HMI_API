using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using System.Data.SqlClient;
using Dapper;
using SAP_API.Constants;

namespace SAP_API.Modelss
{
    public class Customer
    {

        private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS";


        public string CustomerName { get; set; }
        public int CustomerId { get; set; }
        public string CustomerNumber { get; set; }
        public Contract CustomerContract { get; set; }
        public string Country { get; set; }

        public List<Customer> GetCustomer()
        {
            
            List<Customer> listGetCustomer = new List<Customer> { };
            DataSet dsGetCustomer = new DataSet();
            try
            {

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    //dbManager.CreateParameters(1);
                    //dbManager.AddParameters(0, "@IsESM", CustomerId);

                    listGetCustomer = (List<Customer>)sqlConnection.Query<Customer>(SystemConstant.usp_Customer_Get, commandType: CommandType.StoredProcedure);

                    return listGetCustomer;
                }
               
                if (dsGetCustomer != null && dsGetCustomer.Tables.Count != 0)
                {
                    foreach (DataRowView drvCustomer in dsGetCustomer.Tables[0].DefaultView)
                    {
                        Customer psCustomer = new Customer
                        {
                            CustomerName = drvCustomer["VCHCUSTOMERNAME"].ToString(),
                            CustomerId = Convert.ToInt32(drvCustomer["INTCUSTOMERCODE"].ToString()),
                            CustomerNumber = drvCustomer["VCHCUSTOMERNO"].ToString(),
                            Country = drvCustomer["CONTRY_KEY"].ToString()
                        };
                        listGetCustomer.Add(psCustomer);
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
            return listGetCustomer;
        }

        public List<Customer> GetCustomer(bool IsESM)
        {
            
            List<Customer> listGetCustomer = new List<Customer> { };
            DataSet dsGetCustomer = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@IsESM", IsESM);

                    listGetCustomer = (List<Customer>)sqlConnection.Query<Customer>(SystemConstant.usp_Customer_Get,dynamicParameters, commandType: CommandType.StoredProcedure);

                    return listGetCustomer;
                }
              
                if (dsGetCustomer != null && dsGetCustomer.Tables.Count != 0)
                {
                    foreach (DataRowView drvCustomer in dsGetCustomer.Tables[0].DefaultView)
                    {
                        Customer psCustomer = new Customer
                        {
                            CustomerName = drvCustomer["VCHCUSTOMERNAME"].ToString(),
                            CustomerId = Convert.ToInt32(drvCustomer["INTCUSTOMERCODE"].ToString()),
                            CustomerNumber = drvCustomer["VCHCUSTOMERNO"].ToString(),
                            Country = drvCustomer["CONTRY_KEY"].ToString()

                        };
                        listGetCustomer.Add(psCustomer);
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
            return listGetCustomer;
        }
    }
}
