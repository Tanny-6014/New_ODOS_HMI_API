using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using ShapeCodeService.Repositories;
using System;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace ShapeCodeService.Repositories
{

    public class ARMATools
    {
        public int ArmaID = 0;
        public int ErrorID = 0;
        public string strAplusErrMsg = "";
        //private string strPath = ConfigurationManager.AppSettings.Get("ErrorLog");
        private string Areply = "";
        private string AfinalReply = "";
        private string Addr = "";
        private bool Result = false;
        private DataTable join = new DataTable(), shape = new DataTable(), dimension = new DataTable(), indice = new DataTable(), coupler1 = new DataTable(), coupler2 = new DataTable(), type1 = new DataTable(), type2 = new DataTable(), Value = new DataTable(), Letter = new DataTable(), Printed = new DataTable(), bar = new DataTable();
        private System.Resources.ResourceManager res = new System.Resources.ResourceManager("CABUI.acsx.resx", System.Reflection.Assembly.GetExecutingAssembly());
        private DataSet AIP = new DataSet();
        private string SE = "";
        private string PT = "";
        private string Folder = "";
        private string Branch = "";
        private int Contract = 0;
        private int AplusTimeout = Convert.ToInt32(60000);
        private string CopyQuery = "";
        private string CopyResult = "A+ returns null";
        private DateTime SendTimeStamp, ReceiveTimeStamp;
        private CABDAL objCABDal = new CABDAL();

        private readonly IHttpContextAccessor _httpContextAccessor;

        public ARMATools(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        // Dim AplusTimeout As Integer = 60000

        //public int CustomerSupplyBar(int ArmaId, string UserID, string XMLPath, string strSEDetailingID, string strGroupMarkingID, string strStructureMarkID, Int32 intProductTypeId, Int32 intContract, string NewBBSNo = "")
        //{
        //    // Called during posting.
        //    System.Xml.XmlDocument XMLQrySchema = new System.Xml.XmlDocument();
        //    System.Xml.XmlDataDocument XMLQry = new System.Xml.XmlDataDocument();

        //    System.Data.DataSet ds = new System.Data.DataSet();
        //    System.Data.DataTable dt = new System.Data.DataTable();
        //    System.Data.DataSet DSArma = new System.Data.DataSet();
        //    DetailDal objdal = new DetailDal();
        //    System.Net.Sockets.Socket AplusSocket = null/* TODO Change to default(_) if this is not a reference type */;
        //    string Areply, message, message1;
        //    int Success = 0;
        //    SendTimeStamp = "1/1/1753 12:00:00 AM";
        //    ReceiveTimeStamp = "1/1/1753 12:00:00 AM";

        //    try
        //    {
        //        XMLQrySchema.Load(XMLPath);
        //        string XMLStr = "";
        //        System.IO.StringReader strreader = new System.IO.StringReader(XMLQrySchema.InnerXml);
        //        System.Xml.XmlTextReader txtreader = new System.Xml.XmlTextReader(strreader);
        //        DSArma = objdal.GetArmaDetailsOnArmaID(ArmaId);
        //        if (DSArma.Tables.Count > 0)
        //        {
        //            if (DSArma.Tables[0].Rows.Count > 0)
        //            {
        //                ds.ReadXml(txtreader);
        //                if (!NewBBSNo.Trim().Equals(DSArma.Tables[0].Rows[0]["vchDnumber"].ToString().Trim()))
        //                {
        //                    ds.Tables["Drawing"].Rows[0]["Number"] = DSArma.Tables[0].Rows[0]["vchDnumber"];
        //                    ds.Tables["Drawing"].Rows[1]["Number"] = NewBBSNo;
        //                    ds.Tables["Drawing"].Rows[0]["Release"] = DSArma.Tables[0].Rows[0]["vchrelease"];
        //                    ds.Tables["JOBSITE"].Rows[0]["NUMBER"] = DSArma.Tables[0].Rows[0]["vchnumber"];
        //                    ds.Tables["Drawing"].Rows[1]["Release"] = DSArma.Tables[0].Rows[0]["vchrelease"];
        //                    ds.Tables["JOBSITE_TARGET"].Rows[0]["NUMBER"] = intContract; // DSArma.Tables(0).Rows(0)("vchnumber")
        //                    XMLQry = new System.Xml.XmlDataDocument(ds);
        //                    XMLQry.DataSet.EnforceConstraints = false;
        //                    if (DeleteBBSForCSB(DSArma, NewBBSNo, XMLPath, intContract) == true)
        //                    {
        //                        AplusSocket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        //                        Addr = Current.Request.ServerVariables("REMOTE_ADDR");
        //                        if (!AplusSocket.Connected == true)
        //                        {
        //                            if (!Addr == null)
        //                            {
        //                                if (Addr.Trim().Contains("."))
        //                                    AplusSocket.Connect(Addr, 6800);
        //                                else
        //                                    AplusSocket.Connect("127.0.0.1", 6800);
        //                            }
        //                        }
        //                        CopyQuery = XMLQry.FirstChild.InnerXml.ToString;
        //                        Byte[] Request = System.Text.Encoding.ASCII.GetBytes(XMLQry.FirstChild.InnerXml);
        //                        byte[] buffer = new byte[103601];
        //                        AplusSocket.Send(Request, 0, Request.Length, SocketFlags.None);
        //                        SendTimeStamp = System.DateTime.Now;
        //                        AplusSocket.ReceiveTimeout = AplusTimeout;
        //                        AplusSocket.Receive(buffer);
        //                        AplusSocket.DontFragment = true;
        //                        Areply = System.Text.Encoding.UTF8.GetString(buffer);
        //                        if (Areply.Contains("END"))
        //                        {
        //                            Success = 1;
        //                            AplusSocket.Close();
        //                        }
        //                        else if (Areply.Contains("OK"))
        //                        {
        //                            message = "";
        //                            while (message == "")
        //                            {
        //                                message = SocketRead(ref AplusSocket);

        //                                if (message.Contains("END"))
        //                                {
        //                                    ErrorID = 10;
        //                                    AplusSocket.Close();
        //                                    ReceiveTimeStamp = System.DateTime.Now;
        //                                    break;
        //                                }
        //                                else if (ErrorID == 3)
        //                                {
        //                                    AplusSocket.Close();
        //                                    ErrorID = 3;
        //                                    break;
        //                                }
        //                                else
        //                                    message = "";
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    ErrorID = 10;
        //                    ReceiveTimeStamp = System.DateTime.Now;
        //                }
        //            }
        //        }

        //        if (!(ErrorID == 3 | ErrorID == 4))
        //        {
        //            if (ValidateDataCopy(DSArma, NewBBSNo, XMLPath, BarMarkCount(strSEDetailingID, strGroupMarkingID, strStructureMarkID, intProductTypeId, 0)) == 0)
        //                ErrorID = 6;
        //        }
        //        objCABDal.InsertArmaTraceInfo(ArmaId, CopyQuery, CopyResult, SendTimeStamp, ReceiveTimeStamp);
        //        return Success;
        //    }
        //    catch (Exception ex)
        //    {
        //        objCABDal.InsertArmaTraceInfo(ArmaId, CopyQuery, CopyResult, SendTimeStamp, ReceiveTimeStamp);
        //        if (ex.Message.Contains("No connection could be made"))
        //        {
        //            ErrorID = 4;
        //            return;
        //        }
        //        else if (ex.Message.Contains("An existing connection was forcibly closed"))
        //        {
        //            ErrorID = 4;
        //            return;
        //        }
        //        else if (ex.Message.Contains("failed to respond"))
        //        {
        //            AplusSocket.Close();
        //            ErrorID = 3;
        //            return;
        //        }
        //        else if (ex.Message.Contains("</WARNING>"))
        //        {
        //            ErrorID = 30;
        //            strAplusErrMsg = ex.Message;
        //            AplusSocket.Close();
        //            return;
        //        }
        //        else
        //            ErrorHandler.RaiseError(ex, ConfigurationManager.AppSettings.Get("ErrorLog"));
        //    }
        //    finally
        //    {
        //        if (!AplusSocket == null && AplusSocket.Connected == true)
        //            AplusSocket.Close();
        //    }
        //}

        //public int CopyBBSInArma(int ArmaId, string UserID, string XMLPath, string strSEDetailingID, string strGroupMarkingID, string strStructureMarkID, Int32 intProductTypeId, string NewBBSNo = "")
        //{
        //    // Called during posting.
        //    System.Xml.XmlDocument XMLQrySchema = new System.Xml.XmlDocument();
        //    System.Xml.XmlDataDocument XMLQry = new System.Xml.XmlDataDocument();

        //    System.Data.DataSet ds = new System.Data.DataSet();
        //    System.Data.DataTable dt = new System.Data.DataTable();
        //    System.Data.DataSet DSArma = new System.Data.DataSet();
        //    DetailDal objdal = new DetailDal();
        //    System.Net.Sockets.Socket AplusSocket = null/* TODO Change to default(_) if this is not a reference type */;
        //    string Areply, message, message1;
        //    int Success = 0;
        //    SendTimeStamp = "1/1/1753 12:00:00 AM";
        //    ReceiveTimeStamp = "1/1/1753 12:00:00 AM";

        //    try
        //    {
        //        XMLQrySchema.Load(XMLPath);
        //        string XMLStr = "";
        //        System.IO.StringReader strreader = new System.IO.StringReader(XMLQrySchema.InnerXml);
        //        System.Xml.XmlTextReader txtreader = new System.Xml.XmlTextReader(strreader);
        //        DSArma = objdal.GetArmaDetailsOnArmaID(ArmaId);
        //        if (DSArma.Tables.Count > 0)
        //        {
        //            if (DSArma.Tables[0].Rows.Count > 0)
        //            {
        //                ds.ReadXml(txtreader);
        //                if (!NewBBSNo.Trim().Equals(DSArma.Tables[0].Rows[0]["vchDnumber"].ToString().Trim()))
        //                {
        //                    ds.Tables["Drawing"].Rows[0]["Number"] = DSArma.Tables[0].Rows[0]["vchDnumber"];
        //                    ds.Tables["Drawing"].Rows[1]["Number"] = NewBBSNo;
        //                    ds.Tables["Drawing"].Rows[0]["Release"] = DSArma.Tables[0].Rows[0]["vchrelease"];
        //                    ds.Tables["JOBSITE"].Rows[0]["NUMBER"] = DSArma.Tables[0].Rows[0]["vchnumber"];
        //                    ds.Tables["Drawing"].Rows[1]["Release"] = DSArma.Tables[0].Rows[0]["vchrelease"];
        //                    ds.Tables["JOBSITE_TARGET"].Rows[0]["NUMBER"] = DSArma.Tables[0].Rows[0]["vchnumber"];
        //                    XMLQry = new System.Xml.XmlDataDocument(ds);
        //                    XMLQry.DataSet.EnforceConstraints = false;
        //                    if (DeleteBBS(DSArma, NewBBSNo, XMLPath) == true)
        //                    {
        //                        AplusSocket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        //                        Addr = Current.Request.ServerVariables("REMOTE_ADDR");
        //                        if (!AplusSocket.Connected == true)
        //                        {
        //                            if (!Addr == null)
        //                            {
        //                                if (Addr.Trim().Contains("."))
        //                                    AplusSocket.Connect(Addr, 6800);
        //                                else
        //                                    AplusSocket.Connect("127.0.0.1", 6800);
        //                            }
        //                        }
        //                        CopyQuery = XMLQry.FirstChild.InnerXml.ToString;
        //                        Byte[] Request = System.Text.Encoding.ASCII.GetBytes(XMLQry.FirstChild.InnerXml);
        //                        byte[] buffer = new byte[103601];
        //                        AplusSocket.Send(Request, 0, Request.Length, SocketFlags.None);
        //                        SendTimeStamp = System.DateTime.Now;
        //                        AplusSocket.ReceiveTimeout = AplusTimeout;
        //                        AplusSocket.Receive(buffer);
        //                        AplusSocket.DontFragment = true;
        //                        Areply = System.Text.Encoding.UTF8.GetString(buffer);
        //                        if (Areply.Contains("END"))
        //                        {
        //                            Success = 1;
        //                            AplusSocket.Close();
        //                        }
        //                        else if (Areply.Contains("OK"))
        //                        {
        //                            message = "";
        //                            while (message == "")
        //                            {
        //                                message = SocketRead(ref AplusSocket);

        //                                if (message.Contains("END"))
        //                                {
        //                                    ErrorID = 10;
        //                                    AplusSocket.Close();
        //                                    ReceiveTimeStamp = System.DateTime.Now;
        //                                    break;
        //                                }
        //                                else if (ErrorID == 3 | ErrorID == 31 | ErrorID == 32)
        //                                {
        //                                    AplusSocket.Close();
        //                                    ErrorID = 3;
        //                                    break;
        //                                }
        //                                else
        //                                    message = "";
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    ErrorID = 10;
        //                    ReceiveTimeStamp = System.DateTime.Now;
        //                }
        //            }
        //        }

        //        if (!(ErrorID == 3 | ErrorID == 4))
        //        {
        //            if (ValidateDataCopy(DSArma, NewBBSNo, XMLPath, BarMarkCount(strSEDetailingID, strGroupMarkingID, strStructureMarkID, intProductTypeId, 0)) == 0)
        //                ErrorID = 6;
        //        }
        //        objCABDal.InsertArmaTraceInfo(ArmaId, CopyQuery, CopyResult, SendTimeStamp, ReceiveTimeStamp);
        //        return Success;
        //    }
        //    catch (Exception ex)
        //    {
        //        objCABDal.InsertArmaTraceInfo(ArmaId, CopyQuery, CopyResult, SendTimeStamp, ReceiveTimeStamp);
        //        if (ex.Message.Contains("No connection could be made"))
        //        {
        //            ErrorID = 4;
        //            return;
        //        }
        //        else if (ex.Message.Contains("An existing connection was forcibly closed"))
        //        {
        //            ErrorID = 4;
        //            return;
        //        }
        //        else if (ex.Message.Contains("failed to respond"))
        //        {
        //            AplusSocket.Close();
        //            ErrorID = 3;
        //            return;
        //        }
        //        else if (ex.Message.Contains("</WARNING>"))
        //        {
        //            ErrorID = 30;
        //            strAplusErrMsg = ex.Message;
        //            AplusSocket.Close();
        //            return;
        //        }
        //        else
        //            ErrorHandler.RaiseError(ex, ConfigurationManager.AppSettings.Get("ErrorLog"));
        //    }
        //    finally
        //    {
        //        if (!AplusSocket == null && AplusSocket.Connected == true)
        //            AplusSocket.Close();
        //    }
        //}

        //private DataTable BarMarkCount(Int32 strSEDetailingID, string strGroupMarkingID, Int32 strStructureMarkID, Int32 intProductTypeId, Int64 intArmaid)
        //{
        //    int intSEdetailingID, intGroupMarkID, intStructureMarkID;
        //    try
        //    {
        //        intSEdetailingID = System.Convert.ToInt32(strSEDetailingID);
        //        intGroupMarkID = System.Convert.ToInt32(strGroupMarkingID);
        //        intStructureMarkID = System.Convert.ToInt32(strStructureMarkID);

        //        return objCABDal.GetCAB_BarmarkCount(intSEdetailingID, intGroupMarkID, intStructureMarkID, intProductTypeId, intArmaid);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //private string Generate_Arma_DrawingNo(string UserID)
        //{
        //    try
        //    {
        //        string UniqueNo = string.Empty;
        //        UserID = Interaction.IIf(UserID.Length > 3, UserID.Substring(0, 3), UserID);
        //        UniqueNo = UserID + System.Convert.ToHexString(DateTime.Now.Year).Substring(2, 2) + System.Convert.ToHexString(DateTime.Now.Day) + System.Convert.ToHexString(DateTime.Now.Month) + System.Convert.ToHexString(DateTime.Now.Hour) + System.Convert.ToHexString(DateTime.Now.Minute) + System.Convert.ToHexString(DateTime.Now.Second);
        //        return UniqueNo;
        //    }
        //    catch (Exception ex)
        //    {
        //      throw ex;
        //    }
        //}


        //public bool CopyBBSInArma(Hashtable AIDHash, string XMLPath)
        //{
        //    // Called for copy group mark function.
        //    System.Xml.XmlDocument XMLQrySchema = new System.Xml.XmlDocument();
        //    System.Xml.XmlDocument XMLQry = new System.Xml.XmlDocument();
        //    System.Data.DataSet ds = new System.Data.DataSet();
        //    System.Data.DataTable dt = new System.Data.DataTable();
        //    System.Data.DataSet DSArma = new System.Data.DataSet(), DSNArma = new System.Data.DataSet();
        //    DetailDal objdal = new DetailDal();
        //    System.Net.Sockets.Socket AplusSocket = null/* TODO Change to default(_) if this is not a reference type */;
        //    IEnumerator KeyEnum;
        //    string message, message1;
        //    SendTimeStamp = "1/1/1753 12:00:00 AM";
        //    ReceiveTimeStamp = "1/1/1753 12:00:00 AM";

        //    try
        //    {
        //        KeyEnum = AIDHash.Keys.GetEnumerator;
        //        while (KeyEnum.MoveNext)
        //        {
        //            ArmaID = AIDHash(System.Convert.ToInt32(KeyEnum.Current));
        //            XMLQrySchema.Load(XMLPath);
        //            string XMLStr = "";
        //            System.IO.StringReader strreader = new System.IO.StringReader(XMLQrySchema.InnerXml);
        //            System.Xml.XmlTextReader txtreader = new System.Xml.XmlTextReader(strreader);
        //            DSArma = objdal.GetArmaDetailsOnArmaID(System.Convert.ToInt32(KeyEnum.Current));
        //            DSNArma = objdal.GetArmaDetailsOnArmaID(ArmaID);
        //            if (DSArma.Tables.Count > 0)
        //            {
        //                if (DSArma.Tables[0].Rows.Count > 0)
        //                {
        //                    ds.ReadXml(txtreader);
        //                    ds.Tables["Drawing"].Rows[0]["Number"] = DSArma.Tables[0].Rows[0]["vchDnumber"];
        //                    ds.Tables["Drawing"].Rows[0]["Release"] = DSArma.Tables[0].Rows[0]["vchrelease"];
        //                    ds.Tables["JOBSITE"].Rows[0]["NUMBER"] = DSArma.Tables[0].Rows[0]["vchnumber"];

        //                    ds.Tables["Drawing"].Rows[1]["Number"] = (string)DSNArma.Tables[0].Rows[0]["vchDnumber"];
        //                    ds.Tables["Drawing"].Rows[1]["Release"] = DSNArma.Tables[0].Rows[0]["vchrelease"];
        //                    ds.Tables["JOBSITE_TARGET"].Rows[0]["NUMBER"] = DSNArma.Tables[0].Rows[0]["vchnumber"];
        //                    XMLQry = new System.Xml.XmlDataDocument(ds);
        //                    // XMLQry.DataSet.EnforceConstraints = False
        //                    AplusSocket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        //                    Addr = Current.Request.ServerVariables("REMOTE_ADDR");
        //                    if (!AplusSocket.Connected == true)
        //                    {
        //                        if (!Addr == null)
        //                        {
        //                            if (Addr.Trim().Contains("."))
        //                                AplusSocket.Connect(Addr, 6800);
        //                            else
        //                                AplusSocket.Connect("127.0.0.1", 6800);
        //                        }
        //                    }
        //                    Byte[] Request = System.Text.Encoding.ASCII.GetBytes(XMLQry.FirstChild.InnerXml);
        //                    CopyQuery = XMLQry.FirstChild.InnerXml.ToString();
        //                    byte[] buffer = new byte[103601];
        //                    AplusSocket.Send(Request, 0, Request.Length, SocketFlags.None);
        //                    SendTimeStamp = System.DateTime.Now;
        //                    AplusSocket.ReceiveTimeout = AplusTimeout;
        //                    AplusSocket.Receive(buffer);

        //                    AplusSocket.DontFragment = true;
        //                    Areply = System.Text.Encoding.UTF8.GetString(buffer);
        //                    // If Areply.Contains("OK") Or Areply.Contains("END") Then
        //                    // AplusSocket.Close()
        //                    // End If
        //                    if (Areply.Contains("END"))
        //                        AplusSocket.Close();
        //                    else if (Areply.Contains("OK"))
        //                    {
        //                        message = "";
        //                        while (message == "")
        //                        {
        //                            message = SocketRead(ref AplusSocket);

        //                            if (message.Contains("END"))
        //                            {
        //                                ErrorID = 10;
        //                                ReceiveTimeStamp = System.DateTime.Now;
        //                                AplusSocket.Close();
        //                                break;
        //                            }
        //                            else if (ErrorID == 3)
        //                            {
        //                                AplusSocket.Close();
        //                                ErrorID = 3;
        //                                break;
        //                            }
        //                            else
        //                                message = "";
        //                        }
        //                        if (ErrorID == 3)
        //                            break;
        //                    }
        //                    XMLQry = new System.Xml.XmlDocument();
        //                    ds = new DataSet();
        //                    if (!(ErrorID == 3 | ErrorID == 4))
        //                    {
        //                        // SINCE THIS IS IN TRANSACTION LOOP THE ARMA ID SHOULD BE THE SOURCE ONE - KATHEESH 
        //                        if (ValidateDataCopy(DSNArma, System.Convert.ToHexString(DSNArma.Tables[0].Rows[0]["vchDnumber"]), XMLPath, BarMarkCount(0, 0, 0, 0, System.Convert.ToInt32(KeyEnum.Current))) == 0)
        //                        {
        //                            ErrorID = 6;
        //                            break;
        //                        }
        //                    }
        //                    objCABDal.InsertArmaTraceInfo(ArmaID, CopyQuery, CopyResult, SendTimeStamp, ReceiveTimeStamp);
        //                    DSNArma = new DataSet();
        //                    DSArma = new DataSet();
        //                    if (!AplusSocket == null && AplusSocket.Connected == true)
        //                        AplusSocket.Close();
        //                }
        //            }
        //        }
        //        return Result;
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message.Contains("No connection could be made"))
        //        {
        //            ErrorID = 4;
        //            return;
        //        }
        //        else if (ex.Message.Contains("failed to respond"))
        //        {
        //            ErrorID = 3;
        //            AplusSocket.Close();
        //            return;
        //        }
        //        else if (ex.Message.Contains("</WARNING>"))
        //        {
        //            ErrorID = 30;
        //            strAplusErrMsg = ex.Message;
        //            AplusSocket.Close();
        //            return;
        //        }
        //        else
        //            ErrorHandler.RaiseError(ex, ConfigurationManager.AppSettings.Get("ErrorLog"));
        //    }
        //    finally
        //    {
        //        if (!AplusSocket == null && AplusSocket.Connected == true)
        //            AplusSocket.Close();
        //    }
        //}
        //public string SocketRead1(ref Socket ASoc)
        //{
        //    const int BLOCK_SIZE = 103600;
        //    ErrorID = 0;
        //    byte[] buffer = new byte[103601];
        //    try
        //    {
        //        ASoc.Receive(buffer);

        //        int i, j;
        //        i = Array.FindIndex(buffer, ChkZero);

        //        Array.Resize(ref buffer, i);

        //        SocketRead1 = System.Text.Encoding.UTF8.GetString(buffer);
        //        if (SocketRead1.Contains("</Close>"))
        //            ErrorID = 3;
        //        return SocketRead1.TrimEnd();
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message.Contains("failed to respond") | ex.Message.Contains("An existing connection was forcibly closed by the remote host"))
        //        {
        //            ErrorID = 3;
        //            ASoc.Close();
        //            return "";
        //        }
        //    }
        //    finally
        //    {
        //    }
        //}

        public string SocketRead(ref Socket ASoc)
        {
            const int BLOCK_SIZE = 103600;
            ErrorID = 0;
            byte[] buffer = new byte[103601];
            try
            {

                ASoc.Receive(buffer);

                int i, j;
                i = Array.FindIndex(buffer, ChkZero);

                Array.Resize(ref buffer, i);

                string SocketRead =Encoding.UTF8.GetString(buffer);

                if(SocketRead.Contains("</Close>"))
                {
                    ErrorID = 3;
                }

                return SocketRead.TrimEnd();
                
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("failed to respond"))
                {
                    ErrorID = 31;
                    ASoc.Close();
                    return "";
                }
                else if (ex.Message.Contains("An existing connection was forcibly closed by the remote host"))
                {
                    ErrorID = 32;
                    ASoc.Close();
                    return "";
                }
            }
            finally
            {
            }
            return "";
        }

        //public DataSet CABUICall(string XMLPath, System.Data.SqlClient.SqlCommand cmd, int SEDetailingID, string UserName, string ImagePath)
        //{
        //    string ADDR = "";
        //    Socket AplusSocket = null/* TODO Change to default(_) if this is not a reference type */;
        //    try
        //    {
        //        object obj;
        //        string[] msg = new string[6];
        //        int cnt = 0;

        //        System.Xml.XmlDocument InputXML = new System.Xml.XmlDocument();
        //        System.Xml.XmlDataDocument outPutXML = new System.Xml.XmlDataDocument();

        //        // Dim objCABDal As New CABDAL

        //        AIP = objCABDal.GetArmaInputInfo(SEDetailingID);
        //        Contract = AIP.Tables(0).Rows(0)(0).ToString;
        //        SE = AIP.Tables(0).Rows(0)("Structure").ToString.Trim;
        //        PT = AIP.Tables(0).Rows(0)("Product").ToString.Trim;
        //        Folder = AIP.Tables(0).Rows(0)("Folder").ToString.Trim;
        //        Branch = AIP.Tables(0).Rows(0)("Branch").ToString.Trim;

        //        InputXML.Load(XMLPath);
        //        string XMLStr = "";
        //        string DrawNo = "";
        //        string message = "";
        //        string message1 = "";
        //        System.IO.StringReader strreader = new System.IO.StringReader(InputXML.InnerXml);
        //        System.Xml.XmlTextReader txtreader = new System.Xml.XmlTextReader(strreader);
        //        DataSet ds = new DataSet();
        //        ds.ReadXml(txtreader);

        //        if (PT == "CAB")
        //            DrawNo = IIf(IsDBNull(cmd.Parameters("@vchdnumber").Value), AIP.Tables(0).Rows(0)("GroupMark").ToString.Trim, cmd.Parameters("@vchdnumber").Value);
        //        else
        //            DrawNo = IIf(IsDBNull(cmd.Parameters("@vchdnumber").Value), Generate_Arma_DrawingNo(UserName), cmd.Parameters("@vchdnumber").Value);

        //        ds.Tables("JOBSITE").Rows(0)("FOLDER") = IIf(IsDBNull(cmd.Parameters("@vchfolder").Value), Folder, cmd.Parameters("@vchfolder").Value);
        //        ds.Tables("JOBSITE").Rows(0)("NUMBER") = IIf(IsDBNull(cmd.Parameters("@vchnumber").Value), Contract, cmd.Parameters("@vchnumber").Value);
        //        ds.Tables("DRAWING").Rows(0)("BRANC") = IIf(IsDBNull(cmd.Parameters("@branc").Value), Branch, cmd.Parameters("@branc").Value);
        //        ds.Tables("DRAWING").Rows(0)("NUMBER") = DrawNo;
        //        ds.Tables("DRAWING").Rows(0)("RELEASE") = IIf(IsDBNull(cmd.Parameters("@vchrelease").Value), GetGlobalResourceObject("GlobalResource", "RELEASE"), cmd.Parameters("@vchrelease").Value);
        //        ds.Tables("ELEMENT").Rows(0)("QUANTITY") = IIf(IsDBNull(cmd.Parameters("@intquantity").Value), GetGlobalResourceObject("GlobalResource", "QUANTITY"), cmd.Parameters("@intquantity").Value);
        //        ds.Tables("ELEMENT").Rows(0)("STRUCTURAL") = IIf(IsDBNull(cmd.Parameters("@structural").Value), SE, cmd.Parameters("@structural").Value);
        //        ds.Tables("ELEMENT").Rows(0)("NAME") = IIf(IsDBNull(cmd.Parameters("@elementname").Value), PT, cmd.Parameters("@elementname").Value);
        //        ds.Tables("DRAWING").Rows(0)("TITLE") = DrawNo;
        //        // Exit Try  '' newly added 
        //        armaplusinfo_insert(DrawNo, cmd, Branch, Folder);

        //        outPutXML = new System.Xml.XmlDataDocument(ds);
        //        outPutXML.DataSet.EnforceConstraints = false;

        //        AplusSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        //        ADDR = Current.Request.ServerVariables("REMOTE_ADDR");
        //        if (!ADDR == null)
        //        {
        //            if (ADDR.Trim().Contains("."))
        //                AplusSocket.Connect(ADDR, 6800);
        //            else
        //                AplusSocket.Connect("127.0.0.1", 6800);
        //        }
        //        Byte[] Request = System.Text.Encoding.ASCII.GetBytes(outPutXML.FirstChild.InnerXml);
        //        byte[] buffer = new byte[103601];
        //        AplusSocket.Send(Request, 0, Request.Length, SocketFlags.None);
        //        AplusSocket.ReceiveTimeout = AplusTimeout;
        //        AplusSocket.Receive(buffer);
        //        AplusSocket.DontFragment = true;
        //        message = System.Text.Encoding.UTF8.GetString(buffer);
        //        Array.Clear(buffer, 0, buffer.Length);
        //        byte[] buf = new byte[103601];
        //        AplusSocket.ReceiveTimeout = 0;
        //        if (message.Contains("OK"))
        //        {
        //            message = "";
        //            while (message == "")
        //            {
        //                message = SocketRead(ref AplusSocket);
        //                if (ErrorID == 31 | ErrorID == 32)
        //                    // ErrorID = 3
        //                    // AplusSocket.Close()
        //                    return;
        //                if (message1.ToString().Trim().Length == 0 & message.Trim() == "<END>")
        //                {
        //                    ErrorID = 5;
        //                    AplusSocket.Close();
        //                    return;
        //                }
        //                message = message.Replace("<END>", "");

        //                message1 += message;
        //                if (message != "")
        //                {
        //                    if (message.IndexOf("</JOBSITE>") == -1)
        //                        message = "";
        //                    if (message.IndexOf("</WARNING>") != -1)
        //                        throw new Exception(message);
        //                }
        //            }
        //        }
        //        else
        //        {
        //        }
        //        Array.Clear(buffer, 0, buffer.Length);
        //        AplusSocket.Close();
        //        try
        //        {
        //            System.IO.StreamWriter fi = new System.IO.StreamWriter(System.IO.File.Open(ImagePath.Replace("SHAPE.GIF", DrawNo), System.IO.FileMode.OpenOrCreate));
        //            fi.Write(message1);
        //        }
        //        catch (Exception ex)
        //        {
        //        }

        //        return FormGrid(message1, ImagePath, UserName);
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message.Contains("No connection could be made"))
        //        {
        //            ErrorID = 4;
        //            return;
        //        }
        //        else if (ex.Message.Contains("failed to respond"))
        //        {
        //            ErrorID = 3;
        //            AplusSocket.Close();
        //        }
        //        else if (ex.Message.Contains("</WARNING>"))
        //        {
        //            ErrorID = 30;
        //            strAplusErrMsg = ex.Message;
        //            AplusSocket.Close();
        //        }
        //        else
        //            ErrorHandler.RaiseError(ex, ConfigurationManager.AppSettings.Get("ErrorLog"));
        //    }
        //    // ErrorHandler.WriteLog("A+error: ip:" + ADDR + ex.StackTrace, ConfigurationManager.AppSettings.Get("ErrorLog"))
        //    // ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Msg", "<script>alert('Please Validate again. if the problem persist, try restarting NDS and APlus, NDSFunction: CallAplus ');</script>", False)
        //    finally
        //    {
        //        if (!AplusSocket == null && AplusSocket.Connected == true)
        //            AplusSocket.Close();
        //    }
        //}
        //private DataSet FormGrid(string mes, string ImagePath, string UserName)
        //{
        //    Byte[] data = new Byte[1001];
        //    DataTable join2 = new DataTable();
        //    DataTable shapecopy = new DataTable();
        //    DataSet ds = new DataSet();
        //    System.Xml.XmlDataDocument outXMLDoc = new System.Xml.XmlDataDocument();
        //    mes = mes.Replace("BARBARMARK", "BAR BARMARK").Replace("INDICENUM", "INDICE NUM").Replace("COUPLER1ITEM", "COUPLER1 ITEM").Replace("COUPLER2ITEM", "COUPLER2 ITEM");
        //    mes = mes.TrimEnd();
        //    outXMLDoc.LoadXml(mes);
        //    System.Xml.XmlDocument outXML = new System.Xml.XmlDocument();
        //    outXML.LoadXml(mes);
        //    System.IO.StringReader strreader = new System.IO.StringReader(mes);
        //    System.Xml.XmlTextReader txtreader = new System.Xml.XmlTextReader(strreader);
        //    DataRow dr1, dr2, dr3, dr4, dr5;
        //    string str;
        //    DataColumn column;
        //    ds.ReadXml(txtreader);
        //    ds.Tables.Add(join);
        //    ds.Tables.Add(join2);
        //    try
        //    {
        //        if (ds.Tables.Contains("bar"))
        //        {
        //            bar = ds.Tables("bar");
        //            shape = ds.Tables("shape");
        //            dimension = ds.Tables("dimension");
        //            indice = ds.Tables("indice");

        //            ds.Relations.Clear();
        //            foreach (var column in shape.Columns)
        //                join.Columns.Add(column.ColumnName);
        //            join.Columns.Remove("Bar_Id");

        //            foreach (var column in bar.Columns)
        //                join.Columns.Add(column.ColumnName);

        //            if ((ds.Tables.Contains("coupler1")))
        //            {
        //                join.Columns.Remove("Bar_Id");
        //                coupler1 = ds.Tables("coupler1");
        //                if (!coupler1 == null)
        //                {
        //                    foreach (var column in coupler1.Columns)
        //                        join.Columns.Add(IIf(column.ColumnName.ToUpper == "BAR_ID", column.ColumnName, column.ColumnName + "1"));
        //                }
        //            }
        //            else
        //            {
        //                join.Columns.Add("STANDARD1");
        //                join.Columns.Add("Item1");
        //                join.Columns.Add("STANDARD2");
        //                join.Columns.Add("Item2");
        //                join.Columns.Add("Number_of_Ends");
        //            }


        //            if ((ds.Tables.Contains("Coupler2")))
        //            {
        //                coupler2 = ds.Tables("coupler2");
        //                if (!coupler1 == null)
        //                {
        //                    foreach (var column in coupler2.Columns)
        //                        // join.Columns.Add(IIf(column.ColumnName.ToUpper = "Bar_ID", column.ColumnName, column.ColumnName + "2"))
        //                        join.Columns.Add(column.ColumnName + "2");
        //                }
        //            }
        //            ds.Relations.Add(new DataRelation("join", bar.Columns("Bar_Id"), shape.Columns("Bar_Id"), false));
        //            foreach (var dr1 in shape.Rows)
        //            {
        //                dr3 = join.NewRow;
        //                foreach (var column in shape.Columns)
        //                    dr3(column.ColumnName) = dr1(column.ColumnName);
        //                dr2 = dr1.GetParentRow("join");
        //                foreach (var column in bar.Columns)
        //                    dr3(column.ColumnName) = dr2(column.ColumnName);
        //                join.Rows.Add(dr3);
        //            }
        //            ds.Relations.Remove("join");
        //            ds.Relations.Clear();
        //            if ((ds.Tables.Contains("coupler1")))
        //            {
        //                ds.Relations.Add(new DataRelation("join2", shape.Columns("Bar_Id"), coupler1.Columns("Bar_Id"), false));
        //                if (!coupler1 == null)
        //                {
        //                    foreach (var dr4 in coupler1.Rows)
        //                    {
        //                        if (join.Select("Bar_ID='" + dr4("Bar_ID").ToString + "'").Length > 0)
        //                        {
        //                            join.Select("Bar_ID='" + dr4("Bar_ID").ToString + "'")(0).Item("ITEM1") = dr4("ITEM");
        //                            join.Select("Bar_ID='" + dr4("Bar_ID").ToString + "'")(0).Item("STANDARD1") = dr4("STANDARD");
        //                        }
        //                    }
        //                }
        //            }

        //            if ((ds.Tables.Contains("coupler2")))
        //            {
        //                ds.Relations.Add(new DataRelation("join3", shape.Columns("Bar_Id"), coupler2.Columns("Bar_Id"), false));
        //                if (!coupler1 == null)
        //                {
        //                    foreach (var dr4 in coupler2.Rows)
        //                    {
        //                        if (join.Select("Bar_ID='" + dr4("Bar_ID").ToString + "'").Length > 0)
        //                        {
        //                            if (System.Convert.ToInt32(join.Select("Bar_ID='" + dr4("Bar_ID").ToString + "'")(0)("NUMBER_OF_ENDS")) == 2)
        //                            {
        //                                join.Select("Bar_ID='" + dr4("Bar_ID").ToString + "'")(0).Item("ITEM2") = dr4("ITEM");
        //                                join.Select("Bar_ID='" + dr4("Bar_ID").ToString + "'")(0).Item("STANDARD2") = dr4("STANDARD");
        //                            }
        //                        }
        //                        else
        //                        {
        //                            join.Select("Bar_ID='" + dr4("Bar_ID").ToString + "'")(0).Item("ITEM2") = "";
        //                            join.Select("Bar_ID='" + dr4("Bar_ID").ToString + "'")(0).Item("STANDARD2") = "";
        //                        }
        //                    }
        //                }
        //            }



        //            join.Columns.Remove("RELEASE");
        //            join.Columns.Remove("NB_BENDS");
        //            join.Columns.Add("ImageURL");

        //            join.Columns.Add("bitAssemblyIndicator");
        //            join.Columns.Add("bitMachineIndicator");
        //            join.Columns.Add("chrGenerationStatus");
        //            join.Columns.Add("tntLayer");
        //            join.Columns.Add("intGroupMarkId");
        //            join.Columns.Add("intSEDetailingId");
        //            join.Columns.Add("intProductMarkId");

        //            // added on 2nd july rukshana
        //            if (!(join.Columns.Contains("SHAPE_SURCHARGE")))
        //                join.Columns.Add("SHAPE_SURCHARGE");

        //            CreateImage(ImagePath, ref join);
        //            join.TableName = "Join";
        //            ds.Tables.Remove("Table2");
        //        }

        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //      throw ex;
        //    }
        //}
        //private void armaplusinfo_insert(string DrawNo, SqlCommand cmdop, string Branch, string Folder)
        //{
        //    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings("con").ConnectionString);
        //    SqlCommand cmd = new SqlCommand();
        //    SqlDataAdapter ada = new SqlDataAdapter();
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        cmd = new SqlCommand("ArmaPlusInfo_Insert", con);
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        cmd.Parameters.Add("@vchfolder", SqlDbType.NVarChar);
        //        cmd.Parameters("@vchfolder").Direction = ParameterDirection.Input;
        //        cmd.Parameters("@vchfolder").Value = IIf(IsDBNull(cmdop.Parameters("@vchfolder").Value), Folder, cmdop.Parameters("@vchfolder").Value.ToString.Trim);

        //        cmd.Parameters.Add("@vchnumber", SqlDbType.NVarChar);
        //        cmd.Parameters("@vchnumber").Direction = ParameterDirection.Input;
        //        cmd.Parameters("@vchnumber").Value = IIf(IsDBNull(cmdop.Parameters("@vchnumber").Value), Contract, cmdop.Parameters("@vchnumber").Value.ToString.Trim);

        //        cmd.Parameters.Add("@vchdnumber", SqlDbType.NVarChar);
        //        cmd.Parameters("@vchdnumber").Direction = ParameterDirection.Input;
        //        cmd.Parameters("@vchdnumber").Value = DrawNo;

        //        cmd.Parameters.Add("@vchrelease", SqlDbType.NVarChar);
        //        cmd.Parameters("@vchrelease").Direction = ParameterDirection.Input;
        //        cmd.Parameters("@vchrelease").Value = IIf(IsDBNull(cmdop.Parameters("@vchrelease").Value), GetGlobalResourceObject("GlobalResource", "RELEASE"), cmdop.Parameters("@vchrelease").Value.ToString.Trim);

        //        cmd.Parameters.Add("@vchjobtitle", SqlDbType.NVarChar);
        //        cmd.Parameters("@vchjobtitle").Direction = ParameterDirection.Input;
        //        cmd.Parameters("@vchjobtitle").Value = IIf(IsDBNull(cmdop.Parameters("@vchjobtitle").Value), DrawNo, cmdop.Parameters("@vchjobtitle").Value.ToString.Trim);

        //        cmd.Parameters.Add("@branc", SqlDbType.NVarChar);
        //        cmd.Parameters("@branc").Direction = ParameterDirection.Input;
        //        cmd.Parameters("@branc").Value = IIf(IsDBNull(cmdop.Parameters("@branc").Value), Branch, cmdop.Parameters("@branc").Value.ToString.Trim);

        //        cmd.Parameters.Add("@elementname", SqlDbType.NVarChar);
        //        cmd.Parameters("@elementname").Direction = ParameterDirection.Input;
        //        cmd.Parameters("@elementname").Value = IIf(IsDBNull(cmdop.Parameters("@elementname").Value), PT, cmdop.Parameters("@elementname").Value.ToString.Trim);

        //        cmd.Parameters.Add("@intquantity", SqlDbType.Int);
        //        cmd.Parameters("@intquantity").Direction = ParameterDirection.Input;
        //        cmd.Parameters("@intquantity").Value = IIf(IsDBNull(cmdop.Parameters("@intquantity").Value), GetGlobalResourceObject("GlobalResource", "QUANTITY"), cmdop.Parameters("@intquantity").Value.ToString.Trim);

        //        cmd.Parameters.Add("@structural", SqlDbType.NVarChar);
        //        cmd.Parameters("@structural").Direction = ParameterDirection.Input;
        //        cmd.Parameters("@structural").Value = IIf(IsDBNull(cmdop.Parameters("@structural").Value), SE, cmdop.Parameters("@structural").Value.ToString.Trim);
        //        con.Open();
        //        ArmaID = cmd.ExecuteScalar();
        //        con.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    // ErrorHandler.RaiseError(ex, strLogError)
        //    finally
        //    {
        //        con.close();
        //    }
        //}
        //protected void CreateImage(string ImagePath, ref DataTable Join, string Path = "")
        //{
        //    try
        //    {
        //        //System.Web.UI.WebControls.Image SImg = new System.Web.UI.WebControls.Image();
        //        string HexCode = "";
        //        byte[] imageBytes = new byte[50001];
        //        int j = 0;
        //        var ii = 0;
        //        var cnt = 0;
        //        string str = "";
        //        foreach (DataRow dr in Join.Rows)
        //        {
        //            HexCode = dr("Image").ToString;
        //            if (!HexCode == null | !IsDBNull(dr("Image")))
        //            {
        //                for (j = 0; j <= HexCode.ToCharArray().Length - 1; j += 2)
        //                {
        //                    str = HexCode.ToCharArray(j, 2);
        //                    imageBytes[ii] = System.Convert.ToInt32(str, 16);
        //                    ii += 1;
        //                }
        //                // Array.Resize(Of Byte)(imageBytes, j)
        //                try
        //                {
        //                    System.IO.BinaryWriter Flux = new System.IO.BinaryWriter(System.IO.File.Open(ImagePath.Replace("SHAPE", "SHAPE" + cnt.ToString()), System.IO.FileMode.OpenOrCreate));
        //                    Flux.Write(imageBytes);
        //                    Flux.Close();
        //                }
        //                catch (Exception ex)
        //                {
        //                }
        //                dr("ImageURL") = ImagePath.Replace("SHAPE", "SHAPE" + cnt.ToString());
        //            }
        //            Array.Clear(imageBytes, 0, imageBytes.Length - 1);
        //            ii = 0;
        //            cnt += 1;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
        protected string CreateImage(string HexCode, int ID, string UserName, string ImagePath)
        {
            try
            {
                byte[] imageBytes = new byte[50001];
                int j = 0;
                var ii = 0;
                var cnt = 0;
                string str = "";
                if (HexCode != null)
                {
                    for (j = 0; j <= HexCode.ToCharArray().Length - 1; j += 2)
                    {
                        str = HexCode.ToCharArray(j, 2).ToString();
                        imageBytes[ii] = (byte)Convert.ToInt32(str, 16);
                        ii += 1;
                    }
                    try
                    {
                        if ((System.IO.File.Exists(UserName.Replace("SHAPE", "SHAPE" + UserName.Split(new char[] { '\\' })[1]) + ".GIF")))
                        System.IO.File.Delete(UserName.Replace("SHAPE", "SHAPE" + UserName.Split(new char[] { '\\' })[1]) + ".GIF");


                        System.IO.BinaryWriter Flux = new System.IO.BinaryWriter(System.IO.File.Open(UserName.Replace("SHAPE", "SHAPE" + UserName.Split(new char[] { '\\' })[1]) + ".GIF", System.IO.FileMode.OpenOrCreate));
                        Flux.Write(imageBytes);
                        Flux.Close();
                    }
                    catch (Exception ex)
                    {
                    }
                }
                Array.Clear(imageBytes, 0, imageBytes.Length - 1);
                ii = 0;
                cnt += 1;
            }
            catch (Exception ex)
            {
            }
            return UserName.Replace("SHAPE", "SHAPE" + UserName.Split(new char[] { '\\' })[1]) + ID.ToString() + ".GIF";
        }
        //public int UpdateSO(DataSet ADS, string SO, string XMLPath)
        //{
        //    System.Xml.XmlDocument InputXML = new System.Xml.XmlDocument();
        //    System.Xml.XmlDataDocument OutputXML = new System.Xml.XmlDataDocument();
        //    System.Net.Sockets.Socket AplusSocket = null/* TODO Change to default(_) if this is not a reference type */;
        //    string message = "";
        //    string message2 = "";

        //    try
        //    {
        //        if (SO == "" | SO == null)
        //            ErrorID = 5;
        //        else if (!ADS.Tables(0).Rows.Count == 0)
        //        {
        //            foreach (DataRow dr in ADS.Tables(0).Rows)
        //            {
        //                InputXML.Load(XMLPath);
        //                string XMLStr = "";
        //                System.IO.StringReader strreader = new System.IO.StringReader(InputXML.InnerXml);
        //                System.Xml.XmlTextReader txtreader = new System.Xml.XmlTextReader(strreader);
        //                DataSet ds = new DataSet();
        //                ds.ReadXml(txtreader);
        //                ds.Tables("JOBSITE").Rows(0)("FOLDER") = dr("vchFolder");
        //                ds.Tables("JOBSITE").Rows(0)("NUMBER") = dr("vchNumber");
        //                ds.Tables("DRAWING").Rows(0)("BRANC") = dr("branc");
        //                ds.Tables("DRAWING").Rows(0)("SAP_SO_NUMBER") = SO;
        //                ds.Tables("DRAWING").Rows(0)("NUMBER") = dr("vchdnumber");
        //                ds.Tables("DRAWING").Rows(0)("RELEASE") = dr("vchRelease");
        //                ds.Tables("ELEMENT").Rows(0)("QUANTITY") = dr("Quantity");
        //                ds.Tables("ELEMENT").Rows(0)("STRUCTURAL") = dr("Structural");
        //                ds.Tables("ELEMENT").Rows(0)("NAME") = dr("elementname");
        //                ds.Tables("DRAWING").Rows(0)("TITLE") = dr("vchjobtitle");

        //                // Exit Try  '' newly added 

        //                OutputXML = new System.Xml.XmlDataDocument(ds);
        //                OutputXML.DataSet.EnforceConstraints = false;

        //                AplusSocket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        //                Addr = Current.Request.ServerVariables("REMOTE_ADDR");
        //                if (!AplusSocket.Connected == true)
        //                {
        //                    if (!Addr == null)
        //                    {
        //                        if (Addr.Trim().Contains("."))
        //                            AplusSocket.Connect(Addr, 6800);
        //                        else
        //                            AplusSocket.Connect("127.0.0.1", 6800);
        //                    }
        //                }

        //                Byte[] Request = System.Text.Encoding.ASCII.GetBytes(OutputXML.FirstChild.InnerXml);
        //                // Const BLOCK_SIZE = 103600
        //                byte[] buffer = new byte[103601];
        //                System.Threading.Thread.Sleep(1500);
        //                AplusSocket.Send(Request, 0, Request.Length, SocketFlags.None);
        //                AplusSocket.ReceiveTimeout = AplusTimeout;
        //                AplusSocket.Receive(buffer);
        //                AplusSocket.DontFragment = true;
        //                Areply = System.Text.Encoding.UTF8.GetString(buffer);
        //                if (Areply.Contains("END"))
        //                    AplusSocket.Close();
        //                else if (Areply.Contains("OK"))
        //                {
        //                    message = "";
        //                    while (message == "")
        //                    {
        //                        message = SocketRead(ref AplusSocket);
        //                        message2 += message;
        //                        if (message.Contains("END"))
        //                        {
        //                            if (message2.Contains(SO) | message2.Contains("Element already totally in PS"))
        //                                ErrorID = 10;
        //                            else
        //                            {
        //                                ErrorID = 3;
        //                                AplusSocket.Close();
        //                                return;
        //                            }
        //                            AplusSocket.Close();
        //                            break;
        //                        }
        //                        else if (ErrorID == 3)
        //                        {
        //                            AplusSocket.Close();
        //                            ErrorID = 3;
        //                            return;
        //                        }
        //                        else
        //                            message = "";
        //                    }
        //                }
        //                AplusSocket.Close();
        //            }
        //        }
        //        else
        //        {
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message.Contains("No connection could be made"))
        //        {
        //            ErrorID = 4;
        //            return;
        //        }
        //        else if (ex.Message.Contains("failed to respond"))
        //        {
        //            AplusSocket.Close();
        //            ErrorID = 3;
        //            return;
        //        }
        //        else
        //            ErrorHandler.RaiseError(ex, ConfigurationManager.AppSettings.Get("ErrorLog"));
        //    }
        //}

        //public int UpdatePostQtyInArma(DataSet ADS, string SO, string XMLPath)
        //{
        //    System.Xml.XmlDocument InputXML = new System.Xml.XmlDocument();
        //    System.Xml.XmlDataDocument OutputXML = new System.Xml.XmlDataDocument();
        //    System.Net.Sockets.Socket AplusSocket = null/* TODO Change to default(_) if this is not a reference type */;
        //    string message = "";
        //    string message2 = "";
        //    // XMLPath = "E:\Inetpub\wwwroot\NDSGoLiveBuild\beamcage\APLUSCABSOUPDATE.XML"
        //    try
        //    {
        //        if (!ADS.Tables(0).Rows.Count == 0)
        //        {
        //            foreach (DataRow dr in ADS.Tables(0).Rows)
        //            {
        //                InputXML.Load(XMLPath);
        //                string XMLStr = "";
        //                System.IO.StringReader strreader = new System.IO.StringReader(InputXML.InnerXml);
        //                System.Xml.XmlTextReader txtreader = new System.Xml.XmlTextReader(strreader);
        //                DataSet ds = new DataSet();
        //                ds.ReadXml(txtreader);
        //                ds.Tables("JOBSITE").Rows(0)("FOLDER") = dr("vchFolder");
        //                ds.Tables("JOBSITE").Rows(0)("NUMBER") = dr("vchNumber");
        //                ds.Tables("DRAWING").Rows(0)("BRANC") = dr("branc");
        //                ds.Tables("DRAWING").Rows(0)("NUMBER") = dr("vchdnumber");
        //                ds.Tables("DRAWING").Rows(0)("RELEASE") = dr("vchRelease");
        //                ds.Tables("ELEMENT").Rows(0)("QUANTITY") = dr("Quantity");
        //                ds.Tables("ELEMENT").Rows(0)("STRUCTURAL") = dr("Structural");
        //                ds.Tables("ELEMENT").Rows(0)("NAME") = dr("elementname");
        //                ds.Tables("DRAWING").Rows(0)("TITLE") = dr("vchjobtitle");
        //                ds.Tables("DRAWING").Rows(0)("SAP_SO_NUMBER") = SO;
        //                // Exit Try  '' newly added 
        //                ArmaID = dr("vchArPlusid");
        //                OutputXML = new System.Xml.XmlDataDocument(ds);
        //                OutputXML.DataSet.EnforceConstraints = false;
        //                AplusSocket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        //                Addr = Current.Request.ServerVariables("REMOTE_ADDR");
        //                if (!AplusSocket.Connected == true)
        //                {
        //                    if (!Addr == null)
        //                    {
        //                        if (Addr.Trim().Contains("."))
        //                            AplusSocket.Connect(Addr, 6800);
        //                        else
        //                            AplusSocket.Connect("127.0.0.1", 6800);
        //                    }
        //                }

        //                Byte[] Request = System.Text.Encoding.ASCII.GetBytes(OutputXML.FirstChild.InnerXml);
        //                // Const BLOCK_SIZE = 103600
        //                byte[] buffer = new byte[103601];
        //                System.Threading.Thread.Sleep(1500);
        //                AplusSocket.Send(Request, 0, Request.Length, SocketFlags.None);
        //                AplusSocket.ReceiveTimeout = AplusTimeout;
        //                AplusSocket.Receive(buffer);
        //                AplusSocket.DontFragment = true;
        //                Areply = System.Text.Encoding.UTF8.GetString(buffer);
        //                if (Areply.Contains("END"))
        //                    AplusSocket.Close();
        //                else if (Areply.Contains("OK"))
        //                {
        //                    message = "";
        //                    while (message == "")
        //                    {
        //                        message = SocketRead1(ref AplusSocket);
        //                        message2 += message;
        //                        if (message.Contains("END"))
        //                        {
        //                            if (message2.Contains(SO) | message2.Contains("Element already totally in PS"))
        //                                ErrorID = 10;
        //                            else
        //                            {
        //                                ErrorID = 3;
        //                                AplusSocket.Close();
        //                                return;
        //                            }
        //                            AplusSocket.Close();
        //                            break;
        //                        }
        //                        else if (ErrorID == 3)
        //                        {
        //                            AplusSocket.Close();
        //                            ErrorID = 3;
        //                            return;
        //                        }
        //                        else
        //                            message = "";
        //                    }
        //                }
        //                AplusSocket.Close();
        //            }
        //        }
        //    }
        //    // since this was not allowing to post in bbsposting this has been commented
        //    // objCABDal.InsertArmaTraceInfo(ArmaID, OutputXML.FirstChild.InnerXml, message2, System.DateTime.Now, System.DateTime.Now)
        //    catch (Exception ex)
        //    {
        //        if (ex.Message.Contains("No connection could be made"))
        //        {
        //            ErrorID = 4;
        //            return;
        //        }
        //        else if (ex.Message.Contains("failed to respond"))
        //        {
        //            AplusSocket.Close();
        //            ErrorID = 3;
        //            return;
        //        }
        //        else
        //            ErrorHandler.RaiseError(ex, ConfigurationManager.AppSettings.Get("ErrorLog"));
        //    }
        //    finally
        //    {
        //        if (!AplusSocket == null && AplusSocket.Connected == true)
        //            AplusSocket.Close();
        //    }
        //}

        //public int ValidateDataCopy(DataSet DSArmaInf, string BBSNo, string XMLPATH, DataTable dtBarmark)
        //{
        //    string CHECKXMLPATH = XMLPATH.ToUpper().Replace("COPYBBS", "APLUSCABCHECK");
        //    System.Xml.XmlDocument InputXML = new System.Xml.XmlDocument();
        //    System.Xml.XmlDataDocument OutPutXML = new System.Xml.XmlDataDocument();
        //    Socket AplusSocket = null/* TODO Change to default(_) if this is not a reference type */;
        //    string message = "";
        //    string message1 = "";

        //    try
        //    {
        //        InputXML.Load(CHECKXMLPATH);

        //        System.IO.StringReader strreader = new System.IO.StringReader(InputXML.InnerXml);
        //        System.Xml.XmlTextReader txtreader = new System.Xml.XmlTextReader(strreader);
        //        DataSet ds = new DataSet();
        //        ds.ReadXml(txtreader);

        //        // ds.Tables("JOBSITE").Rows(0)("FOLDER") = DSArmaInf.Tables(0).Rows(0)("vchFolder")
        //        // ds.Tables("JOBSITE").Rows(0)("NUMBER") = DSArmaInf.Tables(0).Rows(0)("vchNumber")
        //        // ds.Tables("DRAWING").Rows(0)("BRANC") = DSArmaInf.Tables(0).Rows(0)("branc")
        //        // ds.Tables("DRAWING").Rows(0)("NUMBER") = BBSNo
        //        // ds.Tables("DRAWING").Rows(0)("RELEASE") = DSArmaInf.Tables(0).Rows(0)("vchRelease")
        //        // ds.Tables("ELEMENT").Rows(0)("QUANTITY") = DSArmaInf.Tables(0).Rows(0)("intQuantity")
        //        // ds.Tables("ELEMENT").Rows(0)("STRUCTURAL") = DSArmaInf.Tables(0).Rows(0)("STRUCTURAL")
        //        // ds.Tables("ELEMENT").Rows(0)("NAME") = DSArmaInf.Tables(0).Rows(0)("ELEMENTNAME")
        //        // ds.Tables("DRAWING").Rows(0)("TITLE") = DSArmaInf.Tables(0).Rows(0)("vchJobTitle")

        //        ds.Tables("JOBSITE").Rows(0)("FOLDER") = DSArmaInf.Tables(0).Rows(0)("vchFolder");
        //        ds.Tables("JOBSITE").Rows(0)("NUMBER") = DSArmaInf.Tables(0).Rows(0)("vchNumber");
        //        ds.Tables("DRAWING").Rows(0)("NUMBER") = BBSNo;
        //        ds.Tables("DRAWING").Rows(0)("RELEASE") = DSArmaInf.Tables(0).Rows(0)("vchRelease");

        //        OutPutXML = new System.Xml.XmlDataDocument(ds);
        //        OutPutXML.DataSet.EnforceConstraints = false;

        //        AplusSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        //        Addr = Current.Request.ServerVariables("REMOTE_ADDR");
        //        if (!Addr == null)
        //        {
        //            if (Addr.Trim().Contains("."))
        //                AplusSocket.Connect(Addr, 6800);
        //            else
        //                AplusSocket.Connect("127.0.0.1", 6800);
        //        }

        //        Byte[] Request = System.Text.Encoding.ASCII.GetBytes(OutPutXML.FirstChild.InnerXml);
        //        byte[] buffer = new byte[103601];
        //        AplusSocket.Send(Request, 0, Request.Length, SocketFlags.None);
        //        AplusSocket.ReceiveTimeout = AplusTimeout;
        //        AplusSocket.Receive(buffer);
        //        AplusSocket.DontFragment = true;
        //        message = System.Text.Encoding.UTF8.GetString(buffer);
        //        Array.Clear(buffer, 0, buffer.Length);
        //        byte[] buf = new byte[103601];
        //        AplusSocket.ReceiveTimeout = 0;
        //        if (message.Contains("OK"))
        //        {
        //            message = "";
        //            AplusSocket.ReceiveTimeout = AplusTimeout;
        //            while (message == "")
        //            {
        //                message = SocketRead(ref AplusSocket);
        //                if (ErrorID == 3)
        //                {
        //                    AplusSocket.Close();
        //                    return;
        //                }
        //                if (message1.ToString().Trim().Length == 0 & message.Trim() == "<END>")
        //                {
        //                    ErrorID = 5;
        //                    AplusSocket.Close();
        //                    return;
        //                }
        //                message = message.Replace("<END>", "");

        //                message1 += message;
        //                if (message != "")
        //                {
        //                    if (message.IndexOf("</JOBSITE>") == -1)
        //                        message = "";
        //                    if (message.IndexOf("</WARNING>") != -1)
        //                        throw new Exception(message);
        //                }
        //            }
        //        }
        //        else
        //        {
        //        }
        //        Array.Clear(buffer, 0, buffer.Length);
        //        AplusSocket.Close();
        //        message1 = message1.Replace("BARBARMARK", "BAR BARMARK").Replace("INDICENUM", "INDICE NUM").Replace("COUPLER1ITEM", "COUPLER1 ITEM").Replace("COUPLER2ITEM", "COUPLER2 ITEM");
        //        message1 = message1.TrimEnd();

        //        CopyResult = message1;
        //        strreader = new System.IO.StringReader(message1);
        //        txtreader = new System.Xml.XmlTextReader(strreader);
        //        ds = new DataSet();
        //        ds.ReadXml(txtreader);

        //        if ((dtBarmark.Rows.Count > 0))
        //        {
        //            if ((ds.Tables.Contains("ELEMENT") & ds.Tables.Contains("BARMARK")))
        //            {
        //                if (System.Convert.ToInt32(ds.Tables("BARMARK").Rows(0)("NUM_LINES")) == System.Convert.ToInt32(dtBarmark.Rows(0)("BarmarkCount")) & System.Convert.ToInt32(ds.Tables("BARMARK").Rows(0)("TOTAL_QUANTITY")) == System.Convert.ToInt32(dtBarmark.Rows(0)("Qty")) & System.Convert.ToInt32(ds.Tables("BARMARK").Rows(0)("TOTAL_PROD_LENGTH")) == System.Convert.ToInt32(dtBarmark.Rows(0)("ProductionLength")) & System.Convert.ToInt32(ds.Tables("BARMARK").Rows(0)("TOTAL_DIAMETER")) == System.Convert.ToInt32(dtBarmark.Rows(0)("Diameter")) & System.Convert.ToInt32(ds.Tables("BARMARK").Rows(0)("TOTAL_PIN")) == System.Convert.ToInt32(dtBarmark.Rows(0)("Pinsize")))
        //                    return 1;
        //                else
        //                {
        //                    return 0;
        //                    ErrorID = 41;
        //                }
        //            }
        //            else
        //                return 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message.Contains("No connection could be made"))
        //        {
        //            ErrorID = 4;
        //            return;
        //        }
        //        else if (ex.Message.Contains("failed to respond"))
        //        {
        //            AplusSocket.Close();
        //            ErrorID = 3;
        //            return;
        //        }
        //        else if (ex.Message.Contains("</WARNING>"))
        //            throw ex;
        //        else
        //        {
        //            ErrorHandler.Publish(ex, "Validate data copy");
        //            throw ex;
        //        }
        //    }
        //    finally
        //    {
        //        if (!AplusSocket == null && AplusSocket.Connected == true)
        //            AplusSocket.Close();
        //    }
        //}

        //public int checkAplusconnection()
        //{
        //    return 1;
        //}

        public bool ChkZero(byte chk)
        {
            if (chk == 0)
                return true;
            else
                return false;
        }

        //public bool DeleteBBS(DataSet DSArmaInf, string BBSNo, string XMLPATH)
        //{
        //    string DELXMLPATH = XMLPATH.ToUpper().Replace("COPYBBS", "DELETEBBS");

        //    System.Xml.XmlDocument InputXML = new System.Xml.XmlDocument();
        //    System.Xml.XmlDataDocument OutPutXML = new System.Xml.XmlDataDocument();
        //    Socket AplusSocket = null/* TODO Change to default(_) if this is not a reference type */;
        //    string message = "";
        //    string message1 = "";
        //    bool result = true;
        //    try
        //    {
        //        InputXML.Load(DELXMLPATH);
        //        System.IO.StringReader strreader = new System.IO.StringReader(InputXML.InnerXml);
        //        System.Xml.XmlTextReader txtreader = new System.Xml.XmlTextReader(strreader);
        //        DataSet ds = new DataSet();
        //        ds.ReadXml(txtreader);
        //        ds.Tables("JOBSITE").Rows(0)("FOLDER") = DSArmaInf.Tables(0).Rows(0)("vchFolder");
        //        ds.Tables("JOBSITE").Rows(0)("NUMBER") = DSArmaInf.Tables(0).Rows(0)("vchNumber");
        //        ds.Tables("DRAWING").Rows(0)("NUMBER") = BBSNo;
        //        ds.Tables("DRAWING").Rows(0)("RELEASE") = DSArmaInf.Tables(0).Rows(0)("vchRelease");



        //        OutPutXML = new System.Xml.XmlDataDocument(ds);
        //        OutPutXML.DataSet.EnforceConstraints = false;

        //        AplusSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        //        Addr = Current.Request.ServerVariables("REMOTE_ADDR");
        //        if (!Addr == null)
        //        {
        //            if (Addr.Trim().Contains("."))
        //                AplusSocket.Connect(Addr, 6800);
        //            else
        //                AplusSocket.Connect("127.0.0.1", 6800);
        //        }

        //        Byte[] Request = System.Text.Encoding.ASCII.GetBytes(OutPutXML.FirstChild.InnerXml);
        //        byte[] buffer = new byte[103601];
        //        AplusSocket.Send(Request, 0, Request.Length, SocketFlags.None);
        //        AplusSocket.ReceiveTimeout = AplusTimeout;
        //        AplusSocket.Receive(buffer);
        //        AplusSocket.DontFragment = true;
        //        message = System.Text.Encoding.UTF8.GetString(buffer);
        //        Array.Clear(buffer, 0, buffer.Length);
        //        byte[] buf = new byte[103601];
        //        AplusSocket.ReceiveTimeout = 0;
        //        if (message.Contains("OK"))
        //        {
        //            message = "";
        //            AplusSocket.ReceiveTimeout = AplusTimeout;
        //            while (message == "")
        //            {
        //                message = SocketRead(ref AplusSocket);
        //                if (ErrorID == 3)
        //                {
        //                    AplusSocket.Close();
        //                    return;
        //                }
        //                if (message.Trim().Contains("<END>") | message.Trim().Contains("Cannot Find drawing"))
        //                {
        //                    result = true;
        //                    AplusSocket.Close();
        //                    return result;
        //                    return;
        //                }
        //                else
        //                {
        //                    result = false;
        //                    message = "";
        //                }
        //            }
        //        }
        //        else
        //        {
        //        }
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message.Contains("No connection could be made"))
        //        {
        //            ErrorID = 4;
        //            return;
        //        }
        //        else if (ex.Message.Contains("An existing connection was forcibly closed"))
        //        {
        //            ErrorID = 4;
        //            return;
        //        }
        //        else if (ex.Message.Contains("failed to respond"))
        //        {
        //            AplusSocket.Close();
        //            ErrorID = 3;
        //            return;
        //        }
        //        else
        //          throw ex;
        //    }
        //    finally
        //    {
        //        if (!AplusSocket == null && AplusSocket.Connected == true)
        //            AplusSocket.Close();
        //    }
        //}

        //public bool DeleteBBSForCSB(DataSet DSArmaInf, string BBSNo, string XMLPATH, Int32 intContract)
        //{
        //    string DELXMLPATH = XMLPATH.ToUpper().Replace("COPYBBS", "DELETEBBS");

        //    System.Xml.XmlDocument InputXML = new System.Xml.XmlDocument();
        //    System.Xml.XmlDataDocument OutPutXML = new System.Xml.XmlDataDocument();
        //    Socket AplusSocket = null/* TODO Change to default(_) if this is not a reference type */;
        //    string message = "";
        //    string message1 = "";
        //    bool result = true;
        //    try
        //    {
        //        InputXML.Load(DELXMLPATH);
        //        System.IO.StringReader strreader = new System.IO.StringReader(InputXML.InnerXml);
        //        System.Xml.XmlTextReader txtreader = new System.Xml.XmlTextReader(strreader);
        //        DataSet ds = new DataSet();
        //        ds.ReadXml(txtreader);
        //        ds.Tables("JOBSITE").Rows(0)("FOLDER") = DSArmaInf.Tables(0).Rows(0)("vchFolder");
        //        ds.Tables("JOBSITE").Rows(0)("NUMBER") = intContract;
        //        ds.Tables("DRAWING").Rows(0)("NUMBER") = BBSNo;
        //        ds.Tables("DRAWING").Rows(0)("RELEASE") = DSArmaInf.Tables(0).Rows(0)("vchRelease");



        //        OutPutXML = new System.Xml.XmlDataDocument(ds);
        //        OutPutXML.DataSet.EnforceConstraints = false;

        //        AplusSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        //        Addr = Current.Request.ServerVariables("REMOTE_ADDR");
        //        if (!Addr == null)
        //        {
        //            if (Addr.Trim().Contains("."))
        //                AplusSocket.Connect(Addr, 6800);
        //            else
        //                AplusSocket.Connect("127.0.0.1", 6800);
        //        }

        //        Byte[] Request = System.Text.Encoding.ASCII.GetBytes(OutPutXML.FirstChild.InnerXml);
        //        byte[] buffer = new byte[103601];
        //        AplusSocket.Send(Request, 0, Request.Length, SocketFlags.None);
        //        AplusSocket.ReceiveTimeout = AplusTimeout;
        //        AplusSocket.Receive(buffer);
        //        AplusSocket.DontFragment = true;
        //        message = System.Text.Encoding.UTF8.GetString(buffer);
        //        Array.Clear(buffer, 0, buffer.Length);
        //        byte[] buf = new byte[103601];
        //        AplusSocket.ReceiveTimeout = 0;
        //        if (message.Contains("OK"))
        //        {
        //            message = "";
        //            AplusSocket.ReceiveTimeout = AplusTimeout;
        //            while (message == "")
        //            {
        //                message = SocketRead(ref AplusSocket);
        //                if (ErrorID == 3)
        //                {
        //                    AplusSocket.Close();
        //                    return;
        //                }
        //                if (message.Trim().Contains("<END>") | message.Trim().Contains("Cannot Find drawing"))
        //                {
        //                    result = true;
        //                    AplusSocket.Close();
        //                    return result;
        //                    return;
        //                }
        //                else
        //                {
        //                    result = false;
        //                    message = "";
        //                }
        //            }
        //        }
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message.Contains("No connection could be made"))
        //        {
        //            ErrorID = 4;
        //            return;
        //        }
        //        else if (ex.Message.Contains("An existing connection was forcibly closed"))
        //        {
        //            ErrorID = 4;
        //            return;
        //        }
        //        else if (ex.Message.Contains("failed to respond"))
        //        {
        //            AplusSocket.Close();
        //            ErrorID = 3;
        //            return;
        //        }
        //        else
        //          throw ex;
        //    }
        //    finally
        //    {
        //        if (!AplusSocket == null && AplusSocket.Connected == true)
        //            AplusSocket.Close();
        //    }
        //}

        public void CallShapeModule(string ShapeCode, string ShapeCatalog, string ShapeXMLPath, ref string Shapepath)
        {
            Socket AplusSocket = null/* TODO Change to default(_) if this is not a reference type */;
            System.Xml.XmlDocument ShapeXML = new System.Xml.XmlDocument();
            System.Xml.XmlDataDocument OutputXML = new System.Xml.XmlDataDocument();
            string message = "";
            string message1 = "";
            try
            {
                ShapeXML.Load(ShapeXMLPath);
                System.IO.StringReader strreader = new System.IO.StringReader(ShapeXML.InnerXml);
                System.Xml.XmlTextReader txtreader = new System.Xml.XmlTextReader(strreader);
                DataSet ds = new DataSet();
                ds.ReadXml(txtreader);
                //ds.Tables("SHAPE").Rows[0]("CATALOG") = ShapeCatalog;
                //ds.Tables("SHAPE").Rows(0)("CODE") = ShapeCode;

                DataTable ShapeTable = ds.Tables["SHAPE"];
                ShapeTable.Rows[0]["CATALOG"] = ShapeCatalog;
                ShapeTable.Rows[0]["CODE"] = ShapeCode;


                OutputXML = new System.Xml.XmlDataDocument(ds);
                OutputXML.DataSet.EnforceConstraints = false;
                AplusSocket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                HttpContext context = _httpContextAccessor.HttpContext;
                //Addr = context.Current.Request.ServerVariables("REMOTE_ADDR"); //byvidya

                if (!AplusSocket.Connected == true)
                {
                    if (Addr != null)
                    {
                        if (Addr.Trim().Contains("."))
                            AplusSocket.Connect(Addr, 6800);
                        else
                            AplusSocket.Connect("127.0.0.1", 6800);
                    }
                }

                Byte[] Request = System.Text.Encoding.ASCII.GetBytes(OutputXML.FirstChild.InnerXml);
                // Const BLOCK_SIZE = 103600
                byte[] buffer = new byte[103601];
                AplusSocket.Send(Request, 0, Request.Length, SocketFlags.None);
                AplusSocket.ReceiveTimeout = AplusTimeout;
                AplusSocket.Receive(buffer);
                AplusSocket.DontFragment = true;
                message = System.Text.Encoding.UTF8.GetString(buffer);
                Array.Clear(buffer, 0, buffer.Length);
                AplusSocket.ReceiveTimeout = 0;
                // If message.Contains("OK") Then
                message = "";

                while (message == "")
                {
                    message = SocketRead(ref AplusSocket);
                    if (ErrorID == 3)
                    {
                        AplusSocket.Close();
                        return;
                    }


                    if (message1.ToString().Trim().Length == 0 & message.Trim() == "<END>")
                    {
                        ErrorID = 5;
                        AplusSocket.Close();
                        return;
                    }
                    message1 += message;
                    if (!message.Contains("<END>"))
                        message = "";
                    else
                        message1 = message1.Replace("<END>", "");
                }

                ds = new DataSet();
                strreader = new System.IO.StringReader(message1);
                txtreader = new System.Xml.XmlTextReader(strreader);
                ds.ReadXml(txtreader);
                string ShapeINDICE = "";
                DataTable ShapeDt;
                string ParamType = "";

                if (ds.Tables.Contains("INDICE"))
                {
                    foreach (DataRow sr in ds.Tables["INDICE"].Rows)
                    {
                        switch (sr["TYPE"].ToString().ToUpper())
                        {
                            case "LENGTH":
                                {
                                    ParamType = "'S'";
                                    break;
                                }

                            case "ANGLE":
                                {
                                    if (ds.Tables["INDICE"].Columns.Contains("ANGLE"))
                                        //ParamType = IIf(sr.IsNull("ANGLE"), "'W'", "'" + sr["ANGLE"] + "'");
                                        ParamType = sr.IsNull("ANGLE") ? "'W'" : "'" + sr["ANGLE"] + "'";
                                    else
                                        ParamType = "'W'";
                                    break;
                                }

                            case "HOOK":
                                {
                                    ParamType = "'HK'";
                                    break;
                                }

                            default:
                                {
                                    ParamType = "'S'";
                                    break;
                                }
                        }

                        string printed = sr.IsNull("PRINTED") ? "0" : "1";
                        ShapeINDICE += (ParamType + "-'" + sr["LETTER"] + "'-'" + printed + "'-'" + sr["NUM"] + "'");
                        ShapeINDICE += ";";
                    }
                    ShapeINDICE.Remove(ShapeINDICE.LastIndexOf(";"));


                    //Shapepath = CreateImage(ds.Tables("SHAPE").Rows(0)("IMAGE"), "1", ShapeXMLPath.Remove(ShapeXMLPath.LastIndexOf(@"XML Files\")) + @"Images\CAB\" + ds.Tables("SHAPE").Rows(0)("CODE"), ShapeXMLPath.Remove(ShapeXMLPath.LastIndexOf(@"XML Files\")) + @"Images\CAB\" + ds.Tables("SHAPE").Rows(0)("CODE"));
                    Shapepath = CreateImage((string)ShapeTable.Rows[0]["IMAGE"], 1, ShapeXMLPath.Remove(ShapeXMLPath.LastIndexOf(@"XML Files\")) + @"Images\CAB\" + ShapeTable.Rows[0]["CODE"], ShapeXMLPath.Remove(ShapeXMLPath.LastIndexOf(@"XML Files\")) + @"Images\CAB\" + ShapeTable.Rows[0]["CODE"]);

                }

                //ShapeDt = ds.Tables("SHAPE");

                //objCABDal.SaveCABShapeDetails(ShapeDt.Rows(0)("CODE"), ShapeDt.Rows(0)("BVBS"), ShapeDt.Rows(0)("BENDS"), ShapeINDICE, ShapeDt.Rows(0)("SHAPE_GROUP"), ShapeDt.Rows(0)("BAR_To_BAR"), ShapeDt.Rows(0)("SHAPE_SURCHARGE"), ShapeDt.Rows(0)("Catalog"), ShapeDt.Rows(0)("Release"));
                objCABDal.SaveCABShapeDetails((string)ShapeTable.Rows[0]["CODE"], (string)ShapeTable.Rows[0]["BVBS"], (int)ShapeTable.Rows[0]["BENDS"], ShapeINDICE, (string)ShapeTable.Rows[0]["SHAPE_GROUP"], (bool)ShapeTable.Rows[0]["BAR_To_BAR"], (string)ShapeTable.Rows[0]["SHAPE_SURCHARGE"], (short)ShapeTable.Rows[0]["Catalog"], (string)ShapeTable.Rows[0]["Release"]);

                Array.Clear(buffer, 0, buffer.Length);
                AplusSocket.Close();
                //return ds;
            }
            catch (Exception ex)
            {
                if ((ex.Message.Contains("No connection could be made")))
                    ErrorID = 3;
                else
                    throw ex;
            }
            finally
            {
                if (AplusSocket != null && AplusSocket.Connected == true)
                    AplusSocket.Close();
            }
        }

        //public DataTable PivotShapeDt(DataTable Source)
        //{
        //    DataTable dest = new DataTable();

        //    try
        //    {
        //        dest.Columns.Add(new DataColumn("Bar_ID"));
        //        dest.Columns.Add(new DataColumn("Parameter"));
        //        dest.Columns.Add(new DataColumn("Value"));
        //        dest.Columns.Add(new DataColumn("Num"));

        //        foreach (DataRow dr in Source.Rows)
        //        {
        //            int barid = dr.Item(0);
        //            int num = 1;
        //            for (int cnt = 1; cnt <= Source.Columns.Count - 1; cnt++)
        //            {
        //                if (!dr(cnt) == "")
        //                {
        //                    DataRow newrow = dest.NewRow;
        //                    newrow("Bar_ID") = barid;
        //                    newrow("Parameter") = Source.Columns(cnt).ColumnName.ToUpper.Replace("SHAPE", "");
        //                    newrow("Value") = dr(cnt);
        //                    newrow("NUM") = num;
        //                    num += 1;
        //                    dest.Rows.Add(newrow);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return dest;
        //}

        //private void armaplusinfo_insert(string DrawNo, int Contract, string SE, string PT)
        //{
        //    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings("con").ConnectionString);
        //    SqlCommand cmd = new SqlCommand();
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        cmd = new SqlCommand("ArmaPlusInfo_Insert", con);
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        cmd.Parameters.Add("@vchfolder", SqlDbType.NVarChar);
        //        cmd.Parameters("@vchfolder").Direction = ParameterDirection.Input;
        //        cmd.Parameters("@vchfolder").Value = GetGlobalResourceObject("GlobalResource", "FOLDER");

        //        cmd.Parameters.Add("@vchnumber", SqlDbType.NVarChar);
        //        cmd.Parameters("@vchnumber").Direction = ParameterDirection.Input;
        //        cmd.Parameters("@vchnumber").Value = Contract;

        //        cmd.Parameters.Add("@vchdnumber", SqlDbType.NVarChar);
        //        cmd.Parameters("@vchdnumber").Direction = ParameterDirection.Input;
        //        cmd.Parameters("@vchdnumber").Value = DrawNo;

        //        cmd.Parameters.Add("@vchrelease", SqlDbType.NVarChar);
        //        cmd.Parameters("@vchrelease").Direction = ParameterDirection.Input;
        //        cmd.Parameters("@vchrelease").Value = GetGlobalResourceObject("GlobalResource", "RELEASE");

        //        cmd.Parameters.Add("@vchjobtitle", SqlDbType.NVarChar);
        //        cmd.Parameters("@vchjobtitle").Direction = ParameterDirection.Input;
        //        cmd.Parameters("@vchjobtitle").Value = DrawNo;

        //        cmd.Parameters.Add("@branc", SqlDbType.NVarChar);
        //        cmd.Parameters("@branc").Direction = ParameterDirection.Input;
        //        cmd.Parameters("@branc").Value = GetGlobalResourceObject("GlobalResource", "BRANCH");

        //        cmd.Parameters.Add("@elementname", SqlDbType.NVarChar);
        //        cmd.Parameters("@elementname").Direction = ParameterDirection.Input;
        //        cmd.Parameters("@elementname").Value = PT;

        //        cmd.Parameters.Add("@intquantity", SqlDbType.Int);
        //        cmd.Parameters("@intquantity").Direction = ParameterDirection.Input;
        //        cmd.Parameters("@intquantity").Value = 1;

        //        cmd.Parameters.Add("@structural", SqlDbType.NVarChar);
        //        cmd.Parameters("@structural").Direction = ParameterDirection.Input;
        //        cmd.Parameters("@structural").Value = SE;
        //        con.Open();
        //        ArmaID = cmd.ExecuteScalar();
        //        con.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    // ErrorHandler.RaiseError(ex, strLogError)
        //    finally
        //    {
        //        con.Close();
        //    }
        //}
        //public void CABNONUICALL(DataTable BarDt, DataTable ShapeDT, string inputSchemapath, int SEDetailingID, string UserName, int StructureID)
        //{
        //    System.Xml.XmlDocument InputSchema = new System.Xml.XmlDocument();
        //    System.Net.Sockets.Socket AplusSocket = null/* TODO Change to default(_) if this is not a reference type */;
        //    string message, message1, DrawNo;
        //    DetailDal objdal = new DetailDal();
        //    DataSet dsArma = new DataSet();
        //    message = "";
        //    message1 = "";
        //    try
        //    {
        //        AIP = objCABDal.GetArmaInputInfo(SEDetailingID);
        //        Contract = AIP.Tables(0).Rows(0)(0).ToString;
        //        SE = AIP.Tables(0).Rows(0)("Structure").ToString.Trim;
        //        PT = AIP.Tables(0).Rows(0)("Product").ToString.Trim;
        //        // If StructureID = 0 Or StructureID = Nothing Then
        //        DrawNo = Generate_Arma_DrawingNo(UserName);
        //        armaplusinfo_insert(DrawNo, Contract, SE, PT);
        //        // Else
        //        // ArmaID = objCABDal.GetArmaIDPileStruct(StructureID, SE)
        //        // If Not IsDBNull(ArmaID) Then
        //        // dsArma = objdal.GetArmaDetailsOnArmaID(ArmaID)
        //        // 'DeleteBBS(dsArma, dsArma.Tables(0).Rows(0)("vchDnumber"), inputSchemapath, PT)
        //        // DrawNo = dsArma.Tables(0).Rows(0)("vchDnumber")
        //        // End If
        //        // End If

        //        InputSchema.Load(inputSchemapath);
        //        System.IO.StringReader strreader = new System.IO.StringReader(InputSchema.InnerXml);
        //        System.Xml.XmlTextReader txtreader = new System.Xml.XmlTextReader(strreader);
        //        DataSet dsInput = new DataSet();
        //        dsInput.ReadXml(txtreader, XmlReadMode.InferSchema);
        //        dsInput.Tables("DRAWING").Rows(0)("NUMBER") = DrawNo;
        //        dsInput.Tables("DRAWING").Rows(0)("TITLE") = DrawNo;
        //        dsInput.Tables("DRAWING").Rows(0)("BRANC") = "NSH";
        //        dsInput.Tables("JOBSITE").Rows(0)("NUMBER") = Contract;
        //        dsInput.Tables("ELEMENT").Rows(0)("ELEMENT_ID") = 1;
        //        dsInput.Tables("ELEMENT").Rows(0)("STRUCTURAL") = SE;
        //        dsInput.Tables("ELEMENT").Rows(0)("NAME") = PT;
        //        dsInput.Tables("ELEMENT").Rows(0)("QUANTITY") = 1;
        //        dsInput.Tables("DIMENSION").Rows.Clear();
        //        dsInput.Tables("SHAPE").Rows.Clear();
        //        dsInput.Tables("BAR").Rows.Clear();
        //        DataColumn todel = new DataColumn();

        //        dsInput.Tables("DIMENSION").Columns.RemoveAt(0); // to remove the dummy column "A" from the schema
        //        foreach (DataColumn Col in ShapeDT.Columns)
        //        {
        //            if (!Col.ColumnName == "barmarkid")
        //            {
        //                dsInput.Tables("DIMENSION").Columns.Add(new DataColumn(Col.ColumnName.ToUpper.Replace("SHAPE", ""), Col.DataType));
        //                Col.ColumnName = Col.ColumnName.ToUpper.Replace("SHAPE", "");
        //            }
        //        }
        //        foreach (DataRow bar in BarDt.Rows)
        //        {
        //            DataRow newBar = dsInput.Tables("BAR").NewRow;
        //            newBar("Element_ID") = 1;
        //            // newBar("Pin") = 0
        //            newBar("Bar_Id") = bar("barmarkID");
        //            newBar("BarMark") = bar("BarMark");
        //            newBar("Grade") = bar("grade");
        //            newBar("Diameter") = bar("Diameter");
        //            newBar("Quantity") = bar("Quantity");
        //            dsInput.Tables("BAR").Rows.Add(newBar);
        //            DataRow newShape = dsInput.Tables("SHAPE").NewRow;
        //            newShape("Bar_ID") = bar("barmarkID");
        //            newShape("Shape_ID") = bar("barmarkID");
        //            newShape("Code") = bar("ShapeCode");
        //            dsInput.Tables("SHAPE").Rows.Add(newShape);

        //            foreach (DataRow indRw in ShapeDT.Select("BarmarkID=" + bar("barmarkID").ToString))
        //            {
        //                DataRow newDimension = dsInput.Tables("DIMENSION").NewRow;
        //                newDimension("Shape_ID") = bar("barmarkID");
        //                foreach (DataColumn col in ShapeDT.Columns)
        //                {
        //                    if (!col.ColumnName == "barmarkid")
        //                        newDimension(col.ColumnName) = indRw(col.ColumnName);
        //                }
        //                dsInput.Tables("DIMENSION").Rows.Add(newDimension);
        //            }
        //        }
        //        System.Xml.XmlDataDocument AplusInput = new System.Xml.XmlDataDocument();
        //        AplusInput = new System.Xml.XmlDataDocument(dsInput);
        //        AplusInput.DataSet.EnforceConstraints = false;

        //        AplusSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        //        Addr = Current.Request.ServerVariables("REMOTE_ADDR");
        //        if (!Addr == null)
        //        {
        //            if (Addr.Trim().Contains("."))
        //                AplusSocket.Connect(Addr, 6800);
        //            else
        //                AplusSocket.Connect("127.0.0.1", 6800);
        //        }

        //        Byte[] Request = System.Text.Encoding.ASCII.GetBytes(AplusInput.FirstChild.InnerXml.Replace("</BAR>", "<PIN>0</PIN></BAR>"));
        //        byte[] buffer = new byte[103601];
        //        AplusSocket.Send(Request, 0, Request.Length, SocketFlags.None);
        //        AplusSocket.ReceiveTimeout = AplusTimeout;
        //        AplusSocket.Receive(buffer);
        //        AplusSocket.DontFragment = true;
        //        message = System.Text.Encoding.UTF8.GetString(buffer);
        //        Array.Clear(buffer, 0, buffer.Length);
        //        byte[] buf = new byte[103601];
        //        AplusSocket.ReceiveTimeout = AplusTimeout;
        //        if (message.Contains("OK"))
        //        {
        //            message = "";
        //            while (message == "")
        //            {
        //                message = SocketRead(ref AplusSocket);

        //                if (message1.ToString().Trim().Length == 0 & message.Trim() == "<END>")
        //                {
        //                    ErrorID = 5;
        //                    AplusSocket.Close();
        //                    return;
        //                }
        //                message = message.Replace("<END>", "");

        //                message1 += message;
        //                if (message != "")
        //                {
        //                    if (message.IndexOf("</JOBSITE>") == -1)
        //                        message = "";
        //                    if (message.IndexOf("</WARNING>") != -1)
        //                        throw new Exception(message);
        //                }
        //            }
        //        }
        //        else
        //        {
        //        }
        //        Array.Clear(buffer, 0, buffer.Length);
        //        // ArmaID = DrawNo
        //        AplusSocket.Close();
        //        return FormGrid(message1, inputSchemapath.Substring(0, inputSchemapath.LastIndexOf(@"\")) + @"\SHAPE.Gif", "");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (!AplusSocket == null && AplusSocket.Connected == true)
        //            AplusSocket.Close();
        //    }
        //}
        //public int CopyBBSInArmaSlab(int ArmaId, string XMLPath, string UpdateQtyXMLPath, string NewBBSNo, int intGroupQty)
        //{
        //    // Called during posting.
        //    System.Xml.XmlDocument XMLQrySchema = new System.Xml.XmlDocument();
        //    System.Xml.XmlDataDocument XMLQry = new System.Xml.XmlDataDocument();

        //    System.Data.DataSet ds = new System.Data.DataSet();
        //    System.Data.DataTable dt = new System.Data.DataTable();
        //    System.Data.DataSet DSArma = new System.Data.DataSet();
        //    DetailDal objdal = new DetailDal();
        //    System.Net.Sockets.Socket AplusSocket = null/* TODO Change to default(_) if this is not a reference type */;
        //    string Areply, message;
        //    int Success = 0;
        //    SendTimeStamp = "1/1/1753 12:00:00 AM";
        //    ReceiveTimeStamp = "1/1/1753 12:00:00 AM";

        //    try
        //    {
        //        XMLQrySchema.Load(XMLPath);
        //        string XMLStr = "";
        //        System.IO.StringReader strreader = new System.IO.StringReader(XMLQrySchema.InnerXml);
        //        System.Xml.XmlTextReader txtreader = new System.Xml.XmlTextReader(strreader);
        //        DSArma = objdal.GetArmaDetailsOnArmaID(ArmaId);
        //        if (DSArma.Tables.Count > 0)
        //        {
        //            if (DSArma.Tables[0].Rows.Count > 0)
        //            {
        //                ds.ReadXml(txtreader);
        //                if (!NewBBSNo.Trim().Equals(DSArma.Tables[0].Rows[0]["vchDnumber"].ToString().Trim()))
        //                {
        //                    ds.Tables["Drawing"].Rows[0]["Number"] = DSArma.Tables[0].Rows[0]["vchDnumber"];
        //                    ds.Tables["Drawing"].Rows[1]["Number"] = NewBBSNo;
        //                    ds.Tables["Drawing"].Rows[0]["Release"] = DSArma.Tables[0].Rows[0]["vchrelease"];
        //                    ds.Tables["JOBSITE"].Rows[0]["NUMBER"] = DSArma.Tables[0].Rows[0]["vchnumber"];
        //                    ds.Tables["Drawing"].Rows[1]["Release"] = DSArma.Tables[0].Rows[0]["vchrelease"];
        //                    ds.Tables["JOBSITE_TARGET"].Rows[0]["NUMBER"] = DSArma.Tables[0].Rows[0]["vchnumber"];
        //                    XMLQry = new System.Xml.XmlDataDocument(ds);
        //                    XMLQry.DataSet.EnforceConstraints = false;
        //                    if (DeleteBBS(DSArma, NewBBSNo, XMLPath) == true)
        //                    {
        //                        AplusSocket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        //                        Addr = Current.Request.ServerVariables("REMOTE_ADDR");
        //                        if (!AplusSocket.Connected == true)
        //                        {
        //                            if (!Addr == null)
        //                            {
        //                                if (Addr.Trim().Contains("."))
        //                                    AplusSocket.Connect(Addr, 6800);
        //                                else
        //                                    AplusSocket.Connect("127.0.0.1", 6800);
        //                            }
        //                        }
        //                        CopyQuery = XMLQry.FirstChild.InnerXml.ToString;
        //                        Byte[] Request = System.Text.Encoding.ASCII.GetBytes(XMLQry.FirstChild.InnerXml);
        //                        byte[] buffer = new byte[103601];
        //                        AplusSocket.Send(Request, 0, Request.Length, SocketFlags.None);
        //                        SendTimeStamp = System.DateTime.Now;
        //                        AplusSocket.ReceiveTimeout = AplusTimeout;
        //                        AplusSocket.Receive(buffer);
        //                        AplusSocket.DontFragment = true;
        //                        Areply = System.Text.Encoding.UTF8.GetString(buffer);
        //                        if (Areply.Contains("END"))
        //                        {
        //                            Success = 1;
        //                            AplusSocket.Close();
        //                        }
        //                        else if (Areply.Contains("OK"))
        //                        {
        //                            message = "";
        //                            while (message == "")
        //                            {
        //                                message = SocketRead(ref AplusSocket);

        //                                if (message.Contains("END"))
        //                                {
        //                                    ErrorID = 10;
        //                                    AplusSocket.Close();
        //                                    ReceiveTimeStamp = System.DateTime.Now;
        //                                    break;
        //                                }
        //                                else if (ErrorID == 3 | ErrorID == 31 | ErrorID == 32)
        //                                {
        //                                    AplusSocket.Close();
        //                                    ErrorID = 3;
        //                                    break;
        //                                }
        //                                else
        //                                    message = "";
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    ErrorID = 10;
        //                    ReceiveTimeStamp = System.DateTime.Now;
        //                }
        //            }
        //        }

        //        // If Not (ErrorID = 3 Or ErrorID = 4) Then
        //        // If ValidateDataCopy(DSArma, NewBBSNo, XMLPath, BarMarkCount(strSEDetailingID, strGroupMarkingID, strStructureMarkID, intProductTypeId, 0)) = 0 Then
        //        // ErrorID = 6
        //        // End If
        //        // End If
        //        // Update POST QTY
        //        UpdatePostQtyInArmaSlab(UpdateQtyXMLPath, DSArma.Tables[0].Rows[0]["vchfolder"], DSArma.Tables[0].Rows[0]["vchnumber"], DSArma.Tables[0].Rows[0]["branc"], NewBBSNo, intGroupQty
        //        , DSArma.Tables[0].Rows[0]["structural"], DSArma.Tables[0].Rows[0]["elementname"], DSArma.Tables[0].Rows[0]["vchDnumber"]);


        //        objCABDal.InsertArmaTraceInfo(ArmaId, CopyQuery, CopyResult, SendTimeStamp, ReceiveTimeStamp);
        //        return Success;
        //    }
        //    catch (Exception ex)
        //    {
        //        objCABDal.InsertArmaTraceInfo(ArmaId, CopyQuery, CopyResult, SendTimeStamp, ReceiveTimeStamp);
        //        if (ex.Message.Contains("No connection could be made"))
        //        {
        //            ErrorID = 4;
        //            return;
        //        }
        //        else if (ex.Message.Contains("An existing connection was forcibly closed"))
        //        {
        //            ErrorID = 4;
        //            return;
        //        }
        //        else if (ex.Message.Contains("failed to respond"))
        //        {
        //            AplusSocket.Close();
        //            ErrorID = 3;
        //            return;
        //        }
        //        else if (ex.Message.Contains("</WARNING>"))
        //        {
        //            ErrorID = 30;
        //            strAplusErrMsg = ex.Message;
        //            AplusSocket.Close();
        //            return;
        //        }
        //        else
        //            ErrorHandler.RaiseError(ex, ConfigurationManager.AppSettings.Get("ErrorLog"));
        //    }
        //    finally
        //    {
        //        if (!AplusSocket == null && AplusSocket.Connected == true)
        //            AplusSocket.Close();
        //    }
        //}
        //public int UpdatePostQtyInArmaSlab(string XMLPath, string jobsiteFolder, string jobsitenumber, string drawingbranch, string dnumber, int intpostqty, string elementstructure, string elementname, string elementtitle)
        //{
        //    System.Xml.XmlDocument InputXML = new System.Xml.XmlDocument();
        //    System.Xml.XmlDataDocument OutputXML = new System.Xml.XmlDataDocument();
        //    System.Net.Sockets.Socket AplusSocket = null/* TODO Change to default(_) if this is not a reference type */;
        //    string message = "";
        //    string message2 = "";
        //    string SO = "TBU";
        //    // XMLPath = "E:\Inetpub\wwwroot\NDSGoLiveBuild\beamcage\APLUSCABSOUPDATE.XML"
        //    try
        //    {
        //        // If Not ADS.Tables(0).Rows.Count = 0 Then
        //        // For Each dr As DataRow In ADS.Tables(0).Rows
        //        InputXML.Load(XMLPath);
        //        string XMLStr = "";
        //        System.IO.StringReader strreader = new System.IO.StringReader(InputXML.InnerXml);
        //        System.Xml.XmlTextReader txtreader = new System.Xml.XmlTextReader(strreader);
        //        DataSet ds = new DataSet();
        //        ds.ReadXml(txtreader);
        //        ds.Tables("JOBSITE").Rows(0)("FOLDER") = jobsiteFolder;
        //        ds.Tables("JOBSITE").Rows(0)("NUMBER") = jobsitenumber;
        //        ds.Tables("DRAWING").Rows(0)("BRANC") = drawingbranch;
        //        ds.Tables("DRAWING").Rows(0)("NUMBER") = dnumber;
        //        ds.Tables("DRAWING").Rows(0)("RELEASE") = "0";
        //        ds.Tables("ELEMENT").Rows(0)("QUANTITY") = intpostqty.ToString();
        //        ds.Tables("ELEMENT").Rows(0)("STRUCTURAL") = elementstructure;
        //        ds.Tables("ELEMENT").Rows(0)("NAME") = elementname;
        //        ds.Tables("DRAWING").Rows(0)("TITLE") = elementtitle;
        //        ds.Tables("DRAWING").Rows(0)("SAP_SO_NUMBER") = SO;
        //        // Exit Try  '' newly added 

        //        OutputXML = new System.Xml.XmlDataDocument(ds);
        //        OutputXML.DataSet.EnforceConstraints = false;
        //        AplusSocket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        //        Addr = Current.Request.ServerVariables("REMOTE_ADDR");
        //        if (!AplusSocket.Connected == true)
        //        {
        //            if (!Addr == null)
        //            {
        //                if (Addr.Trim().Contains("."))
        //                    AplusSocket.Connect(Addr, 6800);
        //                else
        //                    AplusSocket.Connect("127.0.0.1", 6800);
        //            }
        //        }

        //        Byte[] Request = System.Text.Encoding.ASCII.GetBytes(OutputXML.FirstChild.InnerXml);
        //        // Const BLOCK_SIZE = 103600
        //        byte[] buffer = new byte[103601];
        //        System.Threading.Thread.Sleep(1500);
        //        AplusSocket.Send(Request, 0, Request.Length, SocketFlags.None);
        //        AplusSocket.ReceiveTimeout = AplusTimeout;
        //        AplusSocket.Receive(buffer);
        //        AplusSocket.DontFragment = true;
        //        Areply = System.Text.Encoding.UTF8.GetString(buffer);
        //        if (Areply.Contains("END"))
        //            AplusSocket.Close();
        //        else if (Areply.Contains("OK"))
        //        {
        //            message = "";
        //            while (message == "")
        //            {
        //                message = SocketRead1(ref AplusSocket);
        //                message2 += message;
        //                if (message.Contains("END"))
        //                {
        //                    if (message2.Contains(SO) | message2.Contains("Element already totally in PS"))
        //                        ErrorID = 10;
        //                    else
        //                    {
        //                        ErrorID = 3;
        //                        AplusSocket.Close();
        //                        return;
        //                    }
        //                    AplusSocket.Close();
        //                    break;
        //                }
        //                else if (ErrorID == 3)
        //                {
        //                    AplusSocket.Close();
        //                    ErrorID = 3;
        //                    return;
        //                }
        //                else
        //                    message = "";
        //            }
        //        }
        //        AplusSocket.Close();
        //    }
        //    // Next
        //    // End If
        //    // since this was not allowing to post in bbsposting this has been commented
        //    // objCABDal.InsertArmaTraceInfo(ArmaID, OutputXML.FirstChild.InnerXml, message2, System.DateTime.Now, System.DateTime.Now)
        //    catch (Exception ex)
        //    {
        //        if (ex.Message.Contains("No connection could be made"))
        //        {
        //            ErrorID = 4;
        //            return;
        //        }
        //        else if (ex.Message.Contains("failed to respond"))
        //        {
        //            AplusSocket.Close();
        //            ErrorID = 3;
        //            return;
        //        }
        //        else
        //            ErrorHandler.RaiseError(ex, ConfigurationManager.AppSettings.Get("ErrorLog"));
        //    }
        //    finally
        //    {
        //        if (!AplusSocket == null && AplusSocket.Connected == true)
        //            AplusSocket.Close();
        //    }
        //}

    }
}
