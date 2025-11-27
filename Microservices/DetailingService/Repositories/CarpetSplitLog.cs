using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Configuration;
using DetailingService.Constants;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace DetailingService.Repositories
{
    public class CarpetSplitLog
    {
        private string connectionstring = "server=NSPRDDB19\\MSSQL2022;database=odos;trustservercertificate=true;multipleactiveresultsets=true;user=ndswebapps;password=dbadmin4*nds;connection timeout=36000000";

        //private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";


        public static void LogCarpetLogic(string sedetailingId, string structureMarkName, string CarpetLogicLog, string strUserName)    //
        {
            //******************************************************************************************************** 
            // Date : 18-Dec-2012
            // Author : S.Surendar 
            // Description : This procedure writes the log details to a log file. 
            // 
            // Revision History : 
            // ------------------ 
            // 
            // Sl.no. Date          Author           Modification Reason 
            // ------ ----------- ----------- ------------------------------------------------- 
            //
            //********************************************************************************************************* 
            string strMessage = null;
            string strFinalPath = null;

            StreamWriter sw = default(StreamWriter);
            FileStream fw = default(FileStream);
            try
            {
                strMessage = System.Environment.NewLine + "--------------------------------------------------------------------------------------------------------------------------";
                strMessage = strMessage + System.Environment.NewLine + "Log Details of Carpet Split Logic are ";
                strMessage = strMessage + System.Environment.NewLine + " Login User : " + strUserName;
                strMessage = strMessage + System.Environment.NewLine + " Log Occured Date & Time : " + System.DateTime.Now;
                strMessage = strMessage + System.Environment.NewLine + " SeDetailingId : " + sedetailingId + "";
                strMessage = strMessage + System.Environment.NewLine + " Structure Mark Name : " + structureMarkName + "";
                strMessage = strMessage + System.Environment.NewLine + " Log Source : " + CarpetLogicLog + "";
                strMessage = strMessage + System.Environment.NewLine + "-------------------------------------------------------------------------------------------------------------------------- ";
                strFinalPath = ConfigurationManager.AppSettings.Get("CarpetLogPath") + "\\NDSV2SlabLog" + ((string)(System.DateTime.Today.ToShortDateString())).Replace("/", "_") + ".txt";
                
                if (System.IO.Directory.Exists(ConfigurationManager.AppSettings.Get("CarpetLogPath")) == true)
                {
                    if (File.Exists(strFinalPath))
                    {

                        sw = File.AppendText(strFinalPath);
                        sw.WriteLine(strMessage);
                        sw.Close();
                    }
                    else
                    {
                        fw = File.Create(strFinalPath);
                        fw.Close();
                        sw = File.AppendText(strFinalPath);
                        sw.WriteLine(strMessage);
                        sw.Close();
                    }
                }
                else
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings.Get("CarpetLogPath") + "\\");
                    if (File.Exists(strFinalPath))
                    {

                        sw = File.AppendText(strFinalPath);
                        sw.WriteLine(strMessage);
                        sw.Close();
                    }
                    else
                    {
                        fw = File.Create(strFinalPath);
                        fw.Close();
                        sw = File.AppendText(strFinalPath);
                        sw.WriteLine(strMessage);
                        sw.Close();

                    }
                }
                //return true;
            }
            catch (Exception)
            {

            }
        }
    }
}
