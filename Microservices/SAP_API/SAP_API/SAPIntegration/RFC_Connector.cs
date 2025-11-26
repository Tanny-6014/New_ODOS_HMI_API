using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAP.Connector;
using SAP.Middleware.Connector; //sap connector

namespace SAP_API.SAPIntegration
{
    public class RFC_Connector : IDestinationConfiguration
    {     
        public event RfcDestinationManager.ConfigurationChangeHandler ConfigurationChanged;

        public bool ChangeEventsSupported()
        {
            return false;
        }

        public RfcConfigParameters GetParameters(string destinationName)
        {
            if ("SE37".Equals(destinationName))
            {
                RfcConfigParameters Params = new RfcConfigParameters();
                Params.Add(RfcConfigParameters.AppServerHost, "172.25.101.6");//server IP DEV                
                Params.Add(RfcConfigParameters.SystemNumber, "0");//server IP
                Params.Add(RfcConfigParameters.User, "SCM_AUTOPO");//User IP
                Params.Add(RfcConfigParameters.Password, "scm4autopo");//password IP
                Params.Add(RfcConfigParameters.Client, "110");//password IP
                Params.Add(RfcConfigParameters.Language, "EN");//password IP
                Params.Add(RfcConfigParameters.Name, "SE37");//password IP
                Params.Add(RfcConfigParameters.ConnectionIdleTimeout, "600");//ConnectionIdleTimeout
                Params.Add(RfcConfigParameters.PeakConnectionsLimit, "10");//ConnectionIdleTimeout
                Params.Add(RfcConfigParameters.PoolSize, "10");//ConnectionIdleTimeout


                return Params;
            }
            else return null;
           
        }
    }
}