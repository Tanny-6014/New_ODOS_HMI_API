
using Dapper;
using DetailingService.Constants;
using DetailingService.Context;
using DetailingService.Dtos;
using DetailingService.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DetailingService.Repositories
{
    public class AccessoriesServicecs:IAccessories
    {
        private DetailingApplicationContext _dbContext;
        private readonly IConfiguration _configuration;
        private string connectionString;
        //   TactonHelper objTactonHelper = new TactonHelper();

        TactonHelper_New objTactonHelper_new = new TactonHelper_New();

        string UserName = ""; //+ (new System.Security.Principal.WindowsPrincipal(System.Security.Principal.WindowsIdentity.GetCurrent())).Identity.Name;
        public AccessoriesServicecs(DetailingApplicationContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString(SystemConstant.DefaultDBConnection);
        }


        public List<SAPMaterial> GetSAPMaterialDetails(string sapMaterialCode, out string errorMessage)
        {
            SAPMaterial objSAPMaterial = new SAPMaterial();
            List<SAPMaterial> sapMaterialList = new List<SAPMaterial>();
            errorMessage = "";
            try
            {
                sapMaterialList = objSAPMaterial.GetSAPMaterial(sapMaterialCode);
                return sapMaterialList;
            }
            catch (Exception ex)
            {
                //ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
                return null;
            }
            finally
            {
                objSAPMaterial = null;
                sapMaterialList = null;
            }

        }

        public List<Accessory> GetCABItems(int intSEDetailingID, out string errorMessage)
        {
            //List<CABItem> cabItemList = new List<CABItem>();
            //CABItem objCABItems = new CABItem();
            List<Accessory> accessoryItemList = new List<Accessory>();
            Accessory objAcceessoryItem = new Accessory();
            errorMessage = "";
            try
            {
                accessoryItemList = objAcceessoryItem.GetCABItemForAccessoryBySEDetailingID(intSEDetailingID);
                return accessoryItemList;
            }
            catch (Exception ex)
            {
                //ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
                return null;
            }
            finally
            {
                objAcceessoryItem = null;
                accessoryItemList = null;
            }

        }

        public List<Accessory> GetACCProductMarkDetailsBySEDetailingID(int intSEDetailingID, out string errorMessage)
        {
            List<Accessory> accessoryItemList = new List<Accessory>();
            Accessory objAcceessoryItem = new Accessory();
            errorMessage = "";
            try
            {
                accessoryItemList = objAcceessoryItem.GetACCProductMarkDetailsBySEDetailingID(intSEDetailingID);
                return accessoryItemList;
            }
            catch (Exception ex)
            {
                //ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
                return null;
            }
            finally
            {
                accessoryItemList = null;
                objAcceessoryItem = null;
            }

        }

        public int InsUpdACCProdMarkDetails(Accessory accessoryItem, out string errorMessage)
        {
            int parameterSetNo = 0;
            errorMessage = "";
            try
            {
                accessoryItem.Save(out errorMessage);
                return parameterSetNo;
            }
            catch (Exception ex)
            {
                //ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
                return 0;
            }
        }

        public string GetCustomerIP(out string errorMessage)
        {
            string CustomerIP = string.Empty;
            errorMessage = "";
            try
            {
                //CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString().Trim();
            }
            catch (Exception ex)
            {
                //ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
            }
            return CustomerIP;
        }




        #region " Deletion of ACC Product Mark Detail .. "

        public bool DeleteACCProductMarkDetail(int AccProductMarkID)
        {
            bool isSuccess = false;
            string errorMessage = "";
            try
            {
                Accessory objaccessory =new Accessory();
                objaccessory.AccProductMarkID = AccProductMarkID;

                isSuccess = objaccessory.Delete(out errorMessage);
            }
            catch (Exception ex)
            {
                //ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
            }
            return isSuccess;
        }

        #endregion
    }
}
