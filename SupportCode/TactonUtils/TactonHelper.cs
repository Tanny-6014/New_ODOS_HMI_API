using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Configuration;
using ExceptionHandling;
namespace TactonUtils
{
    static public class TactonHelper
    { //class added by gansi for path transformation based on persisted configuration from DB
        public static string TransformPath(string currentConfiguration, string moduleName)
        {           
            string tcxOldFilename = ""; 
            string tcxNewFilename = "";
            try
            {
                //to change the physical path to the current application path
                using (XmlReader reader = XmlReader.Create(new StringReader(currentConfiguration)))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Text && reader.Value.EndsWith("tcx"))
                        {
                            tcxOldFilename = reader.Value;
                            switch (moduleName.ToUpper().Trim())
                            {
                                case "SLAB":
                                    tcxNewFilename = ConfigurationSettings.AppSettings["TCX_Folder_Path_Slab"] + tcxOldFilename.Substring(tcxOldFilename.LastIndexOf(@"\") + 1, tcxOldFilename.Length - (tcxOldFilename.LastIndexOf(@"\") + 1));
                                    break;
                                case "BEAM":
                                    tcxNewFilename = ConfigurationSettings.AppSettings["TCX_Folder_Path_Beam"] + tcxOldFilename.Substring(tcxOldFilename.LastIndexOf(@"\") + 1, tcxOldFilename.Length - (tcxOldFilename.LastIndexOf(@"\") + 1));
                                    break;
                                case "COLUMN":
                                    tcxNewFilename = ConfigurationSettings.AppSettings["TCX_Folder_Path_Column"] + tcxOldFilename.Substring(tcxOldFilename.LastIndexOf(@"\") + 1, tcxOldFilename.Length - (tcxOldFilename.LastIndexOf(@"\") + 1));
                                    break;
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.RaiseError(ex,ConfigurationSettings.AppSettings["ErrorLog"].ToString(),string.Empty);
            }
            return currentConfiguration.Replace(tcxOldFilename, tcxNewFilename);
            
        }      
    }
}
