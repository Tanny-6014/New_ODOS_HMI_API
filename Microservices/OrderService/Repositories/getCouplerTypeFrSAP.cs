using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using NSAPConnector;
using SAP.Middleware.Connector;
using OrderService.Models;


namespace OrderService.Repositories
{
    public class getCouplerTypeFrSAP
    {
        private DBContextModels db = new DBContextModels();
        public List<string> getCouplerType(string pContractNo)
        {
            List<string> lReturn = new List<string>();

            if (pContractNo.Length > 0)
            {
                var lConnect = new SapConnection("DEV");
                lConnect.Open();

                var lSAP = lConnect.Destination;

                try
                {
                    IRfcFunction setCommit = lSAP.Repository.CreateFunction("BAPI_TRANSACTION_COMMIT");

                    //get order item number
                    IRfcFunction getOrderAPI = lSAP.Repository.CreateFunction("YMSDFM_RFC_COUPLER_TYP");
                    getOrderAPI.SetValue("CON_NO", pContractNo);
                    RfcSessionManager.BeginContext(lSAP);
                    getOrderAPI.Invoke(lSAP);
                    setCommit.Invoke(lSAP);
                    RfcSessionManager.EndContext(lSAP);

                    IRfcTable lItemsTable = getOrderAPI.GetTable("COU_TYPE");

                    if (lItemsTable.RowCount > 0)
                    {
                        for (lItemsTable.CurrentIndex = 0; lItemsTable.CurrentIndex < lItemsTable.RowCount; ++(lItemsTable.CurrentIndex))
                        {
                            lReturn.Add(lItemsTable.GetString("COU_TYPE"));

                            if (lItemsTable.CurrentIndex >= lItemsTable.RowCount - 1)
                            {
                                break;
                            }
                        }
                    }
                }

                catch (RfcCommunicationException e)
                {
                    //lReturn = -1;
                }
                catch (RfcLogonException e)
                {
                    // user could not logon...
                    //lReturn = -1;
                }
                catch (RfcAbapRuntimeException e)
                {
                    // serious problem on ABAP system side...
                    //lReturn = -1;
                }
                catch (RfcAbapBaseException e)
                {
                    //lReturn = -1;
                    // The function module returned an ABAP exception, an ABAP message
                    // or an ABAP class-based exception...
                }
                finally
                {
                    lConnect.Dispose();
                    lSAP = null;
                    lConnect = null;
                }
            }
            return lReturn;
        }
    }
}