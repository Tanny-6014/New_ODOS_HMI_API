using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using WBSService.Constants;

namespace WBSService.Repositories
{
    public class ProductType
    {
        private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=NDSWebApps;Password=DBAdmin4*NDS";
        //private string connectionString = "Server=NSPRDDB19\\MSSQL2022\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";

        public string ProductTypeName { get; set; }
        public int ProductTypeId { get; set; }
        public string Prefix { get; set; }

        public List<ProductType> GetProductType()
        {
            
            List<ProductType> listProductType = new List<ProductType>();
            DataSet dsProductType = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();


                    listProductType = (List<ProductType>)sqlConnection.Query<ProductType>(SystemConstant.usp_ProductType_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    return listProductType;
                }
                if (dsProductType != null && dsProductType.Tables.Count != 0)
                {
                    foreach (DataRowView drvProductType in dsProductType.Tables[0].DefaultView)
                    {
                        ProductType productType = new ProductType
                        {
                            ProductTypeId = Convert.ToInt32(drvProductType["SITPRODUCTTYPEID"]),
                            ProductTypeName = drvProductType["VCHPRODUCTTYPE"].ToString(),
                            Prefix = drvProductType["VCHPRODUCTTYPE"].ToString(),
                        };
                        listProductType.Add(productType);
                    }
                }
                return listProductType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                
            }
        }

        public List<ProductType> GetProductTypeForParameterSet()
        {
            
            List<ProductType> listProductType = new List<ProductType>();
            DataSet dsProductType = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();


                    listProductType = (List<ProductType>)sqlConnection.Query<ProductType>(SystemConstant.usp_ParameterSetProductType_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    return listProductType;
                }
                if (dsProductType != null && dsProductType.Tables.Count != 0)
                {
                    foreach (DataRowView drvProductType in dsProductType.Tables[0].DefaultView)
                    {
                        ProductType productType = new ProductType
                        {
                            ProductTypeId = Convert.ToInt32(drvProductType["SITPRODUCTTYPEID"]),
                            ProductTypeName = drvProductType["VCHPRODUCTTYPE"].ToString()
                        };
                        listProductType.Add(productType);
                    }
                }
                return listProductType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                
            }
        }

        //added for ship to party

        public List<ProductType> GetProductType_New(int ProjectId)
        {
            
            List<ProductType> listProductType = new List<ProductType> { };
            DataSet dsProductType = new DataSet();
            try
            {

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intprojectid", ProjectId);

                    listProductType = (List<ProductType>)sqlConnection.Query<ProductType>(SystemConstant.WBSProductType_Get_new, dynamicParameters, commandType: CommandType.StoredProcedure);

                    return listProductType;
                }
               
                if (dsProductType != null && dsProductType.Tables.Count != 0)
                {
                    foreach (DataRowView drvProductType in dsProductType.Tables[0].DefaultView)
                    {
                        ProductType productType = new ProductType
                        {
                            ProductTypeId = Convert.ToInt32(drvProductType["SITPRODUCTTYPEID"]),
                            ProductTypeName = drvProductType["VCHPRODUCTTYPE"].ToString(),
                            Prefix = drvProductType["VCHPRODUCTTYPE"].ToString(),
                        };
                        listProductType.Add(productType);
                    }
                }
                return listProductType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                
            }
        }
        public List<ProductType> GetProductTypeForParameterSetForBorePile()
        {
            
            List<ProductType> listProductType = new List<ProductType>();
            DataSet dsProductType = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                   

                    listProductType = (List<ProductType>)sqlConnection.Query<ProductType>(SystemConstant.usp_ParameterSetProductType_Get_BorePile, commandType: CommandType.StoredProcedure);

                    return listProductType;
                }

                if (dsProductType != null && dsProductType.Tables.Count != 0)
                {
                    foreach (DataRowView drvProductType in dsProductType.Tables[0].DefaultView)
                    {
                        ProductType productType = new ProductType
                        {
                            ProductTypeId = Convert.ToInt32(drvProductType["SITPRODUCTTYPEID"]),
                            ProductTypeName = drvProductType["VCHPRODUCTTYPE"].ToString()
                        };
                        listProductType.Add(productType);
                    }
                }
                return listProductType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                
            }
        }
        //end added for ship to party
    }
}
