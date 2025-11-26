using DrainService.Interfaces;

using static DrainService.Repositories.BorePileParameterSetService;

namespace DrainService.Repositories
{
   
        public class BorePileParameterSetService : IBorePileParameterSetService
        {
            string UserName = "" + (new System.Security.Principal.WindowsPrincipal(System.Security.Principal.WindowsIdentity.GetCurrent())).Identity.Name;

            public void DoWork()
            {
            }

            #region "Get"

            // To get the Parameter Set Details By Project Id.
            public List<BorePileParameterSet> GetParameterSetDetails(int ProjectId, out string errorMessage)
            {
                List<BorePileParameterSet> listGetBorePilePS = new List<BorePileParameterSet>();    // List to store the Parameterset details and return the list value to the Silverlight Page.
                BorePileParameterSet objBorePileParameterset = new BorePileParameterSet();          // Object to call the function in BLL Layer.
                errorMessage = "";
                try
                {
                    listGetBorePilePS = objBorePileParameterset.GetBorePileParameterSetDetailsByProjectId(ProjectId);
                    return listGetBorePilePS;
                }
                catch (Exception ex)
                {
                    
                    errorMessage = ex.Message;
                    return null;
                }
                finally
                {
                    listGetBorePilePS = null;
                    objBorePileParameterset = null;
                }
            }

            // To get the transport.
            //public List<ParameterSet> GetTransport(out string errorMessage)
            //{
            //    List<ParameterSet> listGetTransport = new List<ParameterSet>();
            //    ParameterSet objParameterSet = new ParameterSet();
            //    errorMessage = "";
            //    try
            //    {
            //        listGetTransport = objParameterSet.GetTransport();
            //        return listGetTransport;
            //    }
            //    catch (Exception ex)
            //    {
                    
            //        errorMessage = ex.Message;
            //        return null;
            //    }
            //    finally
            //    {
            //        listGetTransport = null;
            //        objParameterSet = null;
            //    }
            //}

            // To get the Structure Element Type.
            public List<BorePileParameterSet> GetProductType(out String errorMessage)
            {
                List<BorePileParameterSet> listGetProdType = new List<BorePileParameterSet>();      // List to store the Product Type details and return the list value to the Silverlight Page.
                BorePileParameterSet objProdType = new BorePileParameterSet();                      // Object to call the function in BLL Layer.
                errorMessage = "";
                try
                {
                    listGetProdType = objProdType.GetProductCode();
                    return listGetProdType;
                }
                catch (Exception ex)
                {
                    
                    errorMessage = ex.Message;
                    return null;
                }
                finally
                {
                    listGetProdType = null;
                    objProdType = null;
                }
            }

            #endregion

            #region "Save"

            // To Insert the Bore Pile Parameter Set 
            public int InsertParameterSet(int ProjectId, int ProductTypeId,int UserId, out string errorMessage)
            {
                List<BorePileParameterSet> listGetBorePilePS = new List<BorePileParameterSet>();
            BorePileParameterSet borePilePS = new BorePileParameterSet();
                errorMessage = "";
                int Output = 0;
                try
                {
                    Output = borePilePS.InsertParameterSet(ProjectId, ProductTypeId,UserId);
             
                    return Output;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    listGetBorePilePS = null;
                    borePilePS = null;
                }
            }

            // To Update the Bore Pile Parameter Set 
            public List<BorePileParameterSet> UpdateParameterSet(int ProjectId, BorePileParameterSet borePilePS, out string errorMessage)
            {
                List<BorePileParameterSet> listGetBorePilePS = new List<BorePileParameterSet>();
                errorMessage = "";
                int Output = 0;
                try
                {
                    Output = borePilePS.UpdateParameterSet(ProjectId);
                    listGetBorePilePS = borePilePS.GetBorePileParameterSetDetailsByProjectId(ProjectId);
                    return listGetBorePilePS;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    listGetBorePilePS = null;
                    borePilePS = null;
                }
            }

            #endregion
        }


    
}
