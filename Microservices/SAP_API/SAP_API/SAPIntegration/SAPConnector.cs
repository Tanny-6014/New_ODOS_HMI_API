using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAP_API.SAPIntegration
{
    public class SAPRFCConnector
    {
        public bool GetConnection()
        {
            try
            {
                IDestinationConfiguration destinationConfig;
                string destinationConfigName = "SE37";
                destinationConfig = null;
                bool destinationIsInitialised = false;
                if (!destinationIsInitialised)
                {
                    destinationConfig = new RFC_Connector();
                    destinationConfig.GetParameters(destinationConfigName);

                    if (RfcDestinationManager.TryGetDestination(destinationConfigName) == null)
                    {
                        RfcDestinationManager.RegisterDestinationConfiguration(destinationConfig);
                        destinationIsInitialised = true;
                    }
                    SAPDATA.rfcDestination = RfcDestinationManager.GetDestination("SE37");
                    SAPDATA.rfcRepository = SAPDATA.rfcDestination.Repository;
                }


                return true;
            }
            catch (Exception)
            {                
                throw;
            }
        }
    }
}