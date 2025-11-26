
//using SAPConnector.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAP.Middleware.Connector;
using NSAPConnector;
//using SAPConnector.Controllers;
//using System.Data.OracleClient;
//using Oracle.ManagedDataAccess.Client;
//using Microsoft.Extensions.Configuration;
using System.IO;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using SAP_API.Modelss;
using SAP_API.Models;
using SAP_API.Controllers;
//using SAPConnector.Controllers;

namespace SAPConnector
{
    public class CreateSOinSAP
    {
        //private DBContextModels db = new DBContextModels();

        //public string CreateSAPSO(string pCustomerCode, string pProjectCode, string pContractNo,
        //    string pReqDate, string pInRemarks, string pExRemarks, string pInvRemarks, string pPONo, string pPODate,
        //    string pBBSNo, string pBBSDesc,
        //    List<OrderDetailsModels> pBBSDetails, string pPaymentType, SAPVBAKExtModels pVBAKExt,
        //    string pPriceDate, string pPaymentTerm, string pPriceGroup,
        //    string pIncoTerms1, string pIncoTerms2, string pUserID)
        //{

        //    string lReturn = "";
        //    List<string> lSAPCode;
        //    string lBarType = "";
        //    short lBarSize = 0;
        //    int lBarLength = 0;
        //    string lCurrency = "";
        //    string lSQL = "";

        //    OracleCommand cmdOrdHdr = new OracleCommand();
        //    OracleDataReader lOrdRst;
        //    OracleConnection cnCIS = new OracleConnection();

        //    List<string> lSAPMaterialNo = new List<string>();
        //    List<int> lSAPkg = new List<int>();

        //    ProcessController lProcess = new ProcessController();

        //    lProcess.OpenCISConnection(ref cnCIS);

        //    lSQL = "SELECT WAERK " +
        //    "FROM SAPSR3.VBAK " +
        //    "WHERE MANDT = '" + lProcess.strClient + "' " +
        //    "AND VBELN = '" + pContractNo + "' ";

        //    cmdOrdHdr.CommandText = lSQL;
        //    cmdOrdHdr.Connection = cnCIS;
        //    cmdOrdHdr.CommandTimeout = 300;
        //    lOrdRst = cmdOrdHdr.ExecuteReader();
        //    if (lOrdRst.HasRows)
        //    {
        //        lOrdRst.Read();
        //        lCurrency = lOrdRst.GetString(0) == null ? "" : lOrdRst.GetString(0);
        //    }
        //    lOrdRst.Close();
        //    lProcess.CloseCISConnection(ref cnCIS);

        //    lCurrency = lCurrency.Trim();

        //    //convert order to SAP material
        //    var lProdCode = (from p in db.ProdCode
        //                     orderby p.BarType, p.BarSize
        //                     select p).ToList();
        //    if (lProdCode.Count > 0 && pBBSDetails.Count > 0)
        //    {
        //        for (var i = 0; i < pBBSDetails.Count; i++)
        //        {
        //            lSAPCode = new List<string>();
        //            lBarType = pBBSDetails[i].BarType;
        //            lBarSize = (short)(pBBSDetails[i].BarSize == null ? 0 : pBBSDetails[i].BarSize);
        //            lBarLength = (int)(pBBSDetails[i].BarLength == null ? 0 : pBBSDetails[i].BarLength);

        //            lSAPCode = (from p in lProdCode
        //                        where p.BarType == lBarType &&
        //                        p.BarSize == lBarSize &&
        //                        p.BarLength == lBarLength
        //                        select p.ProdCodeSAP).ToList();
        //            if (lSAPCode.Count > 0 && pBBSDetails[i].BarTotalQty > 0)
        //            {
        //                if (lSAPCode[0].Length > 0 && pBBSDetails[i].BarTotalQty > 0)
        //                {
        //                    lSAPMaterialNo.Add(lSAPCode[0]);
        //                    lSAPkg.Add((int)pBBSDetails[i].BarWeight);
        //                }
        //            }
        //        }
        //    }

        //    if (lSAPMaterialNo.Count > 0)
        //    {
        //        // get Contract Infor.

        //        try
        //        {
        //            var lConnect = new SapConnection("DEV");
        //            lConnect.Open();

        //            var lSAP = lConnect.Destination;
        //            IRfcFunction setOrderAPI = lSAP.Repository.CreateFunction("BAPI_SALESORDER_CREATEFROMDAT2");
        //            IRfcFunction setCommit = lSAP.Repository.CreateFunction("BAPI_TRANSACTION_COMMIT");

        //            IRfcStructure SAPStrucOrder_Header_In = setOrderAPI.GetStructure("ORDER_HEADER_IN");

        //            var lOrderType = lProcess.getSAPOrderType(pProjectCode, pContractNo, pPaymentType, "MTS");
        //            SAPStrucOrder_Header_In.SetValue("DOC_TYPE", lOrderType);

        //            var lSalesOrg = lProcess.getSAPSalesOrg(pProjectCode, pContractNo);
        //            SAPStrucOrder_Header_In.SetValue("SALES_ORG", lSalesOrg);

        //            SAPStrucOrder_Header_In.SetValue("DISTR_CHAN", "10");
        //            SAPStrucOrder_Header_In.SetValue("DIVISION", "00");
        //            SAPStrucOrder_Header_In.SetValue("REFDOC_CAT", "G");

        //            SAPStrucOrder_Header_In.SetValue("REQ_DATE_H", pReqDate);


        //            if (pContractNo != "" && pPaymentType == "CREDIT")
        //            {
        //                SAPStrucOrder_Header_In.SetValue("PRICE_DATE", pPriceDate);

        //                if (lCurrency != "")
        //                {
        //                    SAPStrucOrder_Header_In.SetValue("CURRENCY", lCurrency);
        //                }

        //                if (pPaymentTerm != null && pPaymentTerm.Trim() != "")
        //                {
        //                    SAPStrucOrder_Header_In.SetValue("PMNTTRMS", pPaymentTerm);
        //                }

        //                if (pPriceGroup != null && pPriceGroup.Trim() != "")
        //                {
        //                    SAPStrucOrder_Header_In.SetValue("PRICE_GRP", pPriceGroup);
        //                }

        //                if (pIncoTerms1 != null && pIncoTerms1.Trim() != "")
        //                {
        //                    SAPStrucOrder_Header_In.SetValue("INCOTERMS1", pIncoTerms1);

        //                    if (pIncoTerms2 != null && pIncoTerms2.Trim() != "")
        //                    {
        //                        SAPStrucOrder_Header_In.SetValue("INCOTERMS2", pIncoTerms2);
        //                    }
        //                }
        //            }

        //            //PO Number, PO Date
        //            SAPStrucOrder_Header_In.SetValue("PURCH_NO_C", pPONo);
        //            SAPStrucOrder_Header_In.SetValue("PURCH_DATE", pPODate);

        //            //Contract Number
        //            if (pContractNo != "")
        //            {
        //                SAPStrucOrder_Header_In.SetValue("REF_DOC", pContractNo);
        //            }

        //            //Order Source
        //            string lOrderSource = "CUS-UX";
        //            if (pUserID == null || pUserID.IndexOf("@") < 0 || pUserID.ToLower().IndexOf("@natsteel.com.sg") > 0)
        //            {
        //                lOrderSource = "NSH-UX";
        //            }
        //            SAPStrucOrder_Header_In.SetValue("NAME", lOrderSource);

        //            // Internal/External Remarks
        //            IRfcTable lRemarks = setOrderAPI.GetTable("ORDER_TEXT");

        //            lRemarks.Append();
        //            lRemarks.SetValue("TEXT_ID", "Z010");
        //            lRemarks.SetValue("LANGU", "EN");
        //            lRemarks.SetValue("TEXT_LINE", pInRemarks);

        //            lRemarks.Append();
        //            lRemarks.SetValue("TEXT_ID", "Z011");
        //            lRemarks.SetValue("LANGU", "EN");
        //            lRemarks.SetValue("TEXT_LINE", pExRemarks);

        //            if (pContractNo == "")
        //            {
        //                lRemarks.Append();
        //                lRemarks.SetValue("TEXT_ID", "Z013");
        //                lRemarks.SetValue("LANGU", "EN");
        //                lRemarks.SetValue("TEXT_LINE", pInvRemarks);
        //            }

        //            IRfcTable SAPTableOrder_Partners = setOrderAPI.GetTable("ORDER_PARTNERS");

        //            SAPTableOrder_Partners.Append();
        //            //Customer Code
        //            SAPTableOrder_Partners.SetValue("PARTN_ROLE", "AG");
        //            SAPTableOrder_Partners.SetValue("PARTN_NUMB", pCustomerCode);
        //            SAPTableOrder_Partners.Append();
        //            //Project code
        //            SAPTableOrder_Partners.SetValue("PARTN_ROLE", "WE");
        //            SAPTableOrder_Partners.SetValue("PARTN_NUMB", pProjectCode);


        //            IRfcTable SAPTableOrder_Items_In = setOrderAPI.GetTable("ORDER_ITEMS_IN");
        //            IRfcTable SAPTableOrder_Schedule_In = setOrderAPI.GetTable("ORDER_SCHEDULES_IN");

        //            int lngTmpPosNr = 0;

        //            for (int i = 0; i < lSAPMaterialNo.Count; i++)
        //            {
        //                lngTmpPosNr = (i + 1) * 10;
        //                SAPTableOrder_Items_In.Append();
        //                SAPTableOrder_Items_In.SetValue("ITM_NUMBER", lngTmpPosNr);
        //                SAPTableOrder_Items_In.SetValue("MATERIAL", lSAPMaterialNo[i]);
        //                if (pContractNo != "")
        //                {
        //                    SAPTableOrder_Items_In.SetValue("REF_DOC", pContractNo);
        //                    SAPTableOrder_Items_In.SetValue("REF_DOC_IT", "000000");

        //                    SAPTableOrder_Items_In.SetValue("REF_DOC_CA", "G");
        //                }

        //                //BBS No, BBS Description
        //                //SAPTableOrder_Items_In.SetValue("YBBS_NO", pBBSNo);
        //                //SAPTableOrder_Items_In.SetValue("YBBS_DESCR", pBBSDesc);

        //                lRemarks.Append();
        //                lRemarks.SetValue("TEXT_ID", "Z106");
        //                lRemarks.SetValue("ITM_NUMBER", lngTmpPosNr);
        //                lRemarks.SetValue("LANGU", "EN");
        //                lRemarks.SetValue("TEXT_LINE", pBBSNo);

        //                lRemarks.Append();
        //                lRemarks.SetValue("TEXT_ID", "Z107");
        //                lRemarks.SetValue("ITM_NUMBER", lngTmpPosNr);
        //                lRemarks.SetValue("LANGU", "EN");
        //                lRemarks.SetValue("TEXT_LINE", pBBSDesc);

        //                SAPTableOrder_Schedule_In.Append();
        //                SAPTableOrder_Schedule_In.SetValue("ITM_NUMBER", lngTmpPosNr);
        //                SAPTableOrder_Schedule_In.SetValue("SCHED_LINE", 1);
        //                SAPTableOrder_Schedule_In.SetValue("REQ_QTY", lSAPkg[i] / 1000);
        //                SAPTableOrder_Schedule_In.SetValue("REQ_DATE", pReqDate);
        //            }

        //            IRfcTable SAPTableOrder_Ext_In = setOrderAPI.GetTable("EXTENSIONIN");

        //            SAPTableOrder_Ext_In.Append();
        //            SAPTableOrder_Ext_In.SetValue("STRUCTURE", "BAPE_VBAK");
        //            SAPTableOrder_Ext_In.SetValue("VALUEPART1",
        //                "          " +                                                  //SO (10)
        //                (pVBAKExt.YTOT_GRAND / 1000).ToString("0000000000000.000") +    //YTOT_GRAND (17)
        //                "                 " +                                           //YTOT_CAB (17)
        //                "                 " +                                           //YTOT_MESH (17)
        //                (pVBAKExt.YTOT_REBAR / 1000).ToString("0000000000000.000") +    //YTOT_REBAR (17)
        //                "                 " +                                           //YTOT_BPC (17)
        //                "                 " +                                           //YTOT_PRECAGE (17)
        //                "                 " +                                           //YTOTAL_WR (17)
        //                "                 " +                                           //YTOTAL_PCSTRAND (17)
        //                "                " +                                            //YTOT_PCSTRVAL (16)
        //                "                 " +                                           //YTOT_COLD_ROLL (17)
        //                "                 " +                                           //YTOT_PRE_CUTWR (17)
        //                "                " +                                            //YTOTAL_VALUE (16)
        //                "      " +                                                      //YTOT_COUPLER (6)
        //                "                " +                                            //YTOT_COUPVAL (16)
        //                pVBAKExt.YWIREROD_IND +
        //                pVBAKExt.YREBAR_IND +
        //                pVBAKExt.YCAB_IND +
        //                pVBAKExt.YMESH_IND +
        //                pVBAKExt.YPRECAGE +
        //                pVBAKExt.YBPC_IND);

        //            SAPTableOrder_Ext_In.SetValue("VALUEPART2",
        //                pVBAKExt.YPCSTRAND +
        //                pVBAKExt.YMAT_SOURCE +
        //                " " +                                       //YCAB_TYPE
        //                " " +                                       //YORD_TYPE
        //                "                                        " +    //YWBS1 (40)
        //                pVBAKExt.YCOLD_ROLL_WIRE +
        //                pVBAKExt.YPRE_CUT_WIRE +
        //                "                 " +                       //YTOT_BILLET(17)
        //                " " +                                       //YBILLET_IND
        //                " " +                                       //YCON_TYP
        //                "                 " +                       //YTOT_CAR(17)
        //                pVBAKExt.YCARPET_IND +
        //                pVBAKExt.YCSURCHARGE_IND);


        //            SAPTableOrder_Ext_In.Append();
        //            SAPTableOrder_Ext_In.SetValue("STRUCTURE", "BAPE_VBAKX");
        //            SAPTableOrder_Ext_In.SetValue("VALUEPART1", "          X  X" +
        //                " " +       //YTOT_BPC
        //                " " +       //YTOT_PRECAGE
        //                " " +       //YTOTAL_WR
        //                " " +       //YTOTAL_PCSTRAND
        //                " " +       //YTOT_PCSTRVAL
        //                " " +       //YTOT_COLD_ROLL
        //                " " +       //YTOT_PRE_CUTWR
        //                " " +       //YTOTAL_VALUE
        //                " " +       //YTOT_COUPLER
        //                " " +       //YTOT_COUPVAL
        //                "X" +       //YWIREROD_IND
        //                "X" +       //YREBAR_IND
        //                "X" +       //YCAB_IND
        //                "X" +       //YMESH_IND
        //                "X" +       //YPRECAGE
        //                "X" +       //YBPC_IND
        //                "X" +       //YPCSTRAND
        //                "X" +       //YMAT_SOURCE
        //                " " +       //YCAB_TYPE
        //                " " +       //YORD_TYPE
        //                " " +       //YWBS1
        //                "X" +       //YCOLD_ROLL_WIRE
        //                "X" +       //YPRE_CUT_WIRE
        //                " " +       //YTOT_BILLET
        //                " " +       //YBILLET_IND
        //                " " +       //YCON_TYP
        //                " " +       //YTOT_CAR
        //                "X" +       //YCARPET_IND
        //                "X"         //YCSURCHARGE_IND
        //                );

        //            RfcSessionManager.BeginContext(lSAP);
        //            setOrderAPI.Invoke(lSAP);
        //            setCommit.Invoke(lSAP);
        //            RfcSessionManager.EndContext(lSAP);

        //            String strVBELN;
        //            String strReturnType;
        //            String strReturnID;
        //            String strReturnNumber;
        //            String strReturnMessage;
        //            String strReturnType2;
        //            String strReturnID2;
        //            String strReturnNumber2;
        //            String strReturnMessage2;

        //            IRfcTable SAPReturn = setOrderAPI.GetTable("RETURN");
        //            strVBELN = setOrderAPI.GetValue("SALESDOCUMENT").ToString();

        //            strReturnMessage = "";
        //            if (strVBELN.Length <= 8)
        //            {
        //                if (SAPReturn.RowCount > 0)
        //                {
        //                    for (int i = 0; i < SAPReturn.RowCount; i++)
        //                    {
        //                        strReturnType = SAPReturn[i].GetString("TYPE");
        //                        strReturnID = SAPReturn[i].GetString("ID");
        //                        strReturnNumber = SAPReturn[i].GetString("NUMBER");
        //                        if (SAPReturn[i].GetString("MESSAGE") != null && SAPReturn[i].GetString("MESSAGE").ToUpper().IndexOf("SUCCESS") < 0)
        //                        {
        //                            if (strReturnMessage == "")
        //                            {
        //                                strReturnMessage = SAPReturn[i].GetString("MESSAGE");
        //                            }
        //                            else
        //                            {
        //                                strReturnMessage = strReturnMessage + "\n" + SAPReturn[i].GetString("MESSAGE");
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    strReturnType = setOrderAPI.GetTable("RETURN").GetString("TYPE");
        //                    strReturnID = setOrderAPI.GetTable("RETURN").GetString("ID");
        //                    strReturnNumber = setOrderAPI.GetTable("RETURN").GetString("NUMBER");
        //                    strReturnMessage = setOrderAPI.GetTable("RETURN").GetString("MESSAGE");
        //                }
        //            }
        //            strReturnType2 = setCommit.GetStructure("RETURN").GetString("TYPE");
        //            strReturnID2 = setCommit.GetStructure("RETURN").GetString("ID");
        //            strReturnNumber2 = setCommit.GetStructure("RETURN").GetString("NUMBER");
        //            strReturnMessage2 = setCommit.GetStructure("RETURN").GetString("MESSAGE");

        //            if (strVBELN.Length > 8)
        //            {
        //                lReturn = strVBELN;
        //            }
        //            else
        //            {
        //                lReturn = strReturnMessage;
        //            }

        //            lConnect.Dispose();
        //            lSAP = null;
        //            lConnect = null;
        //        }

        //        catch (RfcCommunicationException e)
        //        {
        //            lReturn = e.Message;
        //        }
        //        catch (RfcLogonException e)
        //        {
        //            // user could not logon...
        //            lReturn = e.Message;
        //        }
        //        catch (RfcAbapRuntimeException e)
        //        {
        //            // serious problem on ABAP system side...
        //            lReturn = e.Message;
        //        }
        //        catch (RfcAbapBaseException e)
        //        {
        //            lReturn = e.Message;
        //            // The function module returned an ABAP exception, an ABAP message
        //            // or an ABAP class-based exception...
        //        }
        //        catch (Exception e)
        //        {
        //            lReturn = e.Message;
        //            // Other type of exception
        //        }
        //    }

        //    lProcess = null;

        //    return lReturn;
        //}

        //public string CreateStdSheetSAPSO(string pCustomerCode, string pProjectCode, string pContractNo,
        //    string pReqDate, string pInRemarks, string pExRemarks, string pInvRemarks, string pPONo, string pPODate,
        //    string pBBSNo, string pBBSDesc,
        //    List<StdSheetDetailsModels> pBBSDetails, string pPaymentType, SAPVBAKExtModels pVBAKExt,
        //    string pPriceDate, string pPaymentTerm, string pPriceGroup,
        //    string pIncoTerms1, string pIncoTerms2, string pUserID)
        //{
        //    string lReturn = "";
        //    string lSAPMaterial = "";
        //    int lPieces = 0;

        //    string lCurrency = "";
        //    string lSQL = "";

        //    OracleCommand cmdOrdHdr = new OracleCommand();
        //    OracleDataReader lOrdRst;
        //    OracleConnection cnCIS = new OracleConnection();

        //    ProcessController lProcess = new ProcessController();

        //    lProcess.OpenCISConnection(ref cnCIS);

        //    lSQL = "SELECT WAERK " +
        //    "FROM SAPSR3.VBAK " +
        //    "WHERE MANDT = '" + lProcess.strClient + "' " +
        //    "AND VBELN = '" + pContractNo + "' ";

        //    cmdOrdHdr.CommandText = lSQL;
        //    cmdOrdHdr.Connection = cnCIS;
        //    cmdOrdHdr.CommandTimeout = 300;
        //    lOrdRst = cmdOrdHdr.ExecuteReader();
        //    if (lOrdRst.HasRows)
        //    {
        //        lOrdRst.Read();
        //        lCurrency = lOrdRst.GetString(0) == null ? "" : lOrdRst.GetString(0);
        //    }
        //    lOrdRst.Close();
        //    lProcess.CloseCISConnection(ref cnCIS);

        //    lCurrency = lCurrency.Trim();

        //    List<string> lSAPMaterialNo = new List<string>();
        //    List<int> lSAPPcs = new List<int>();

        //    //convert order to SAP material
        //    if (pBBSDetails.Count > 0)
        //    {
        //        for (var i = 0; i < pBBSDetails.Count; i++)
        //        {
        //            lSAPMaterial = pBBSDetails[i].sap_mcode;
        //            lPieces = pBBSDetails[i].order_pcs;
        //            if (lSAPMaterial.Trim().Length > 0 && lPieces > 0)
        //            {
        //                lSAPMaterialNo.Add(lSAPMaterial);
        //                lSAPPcs.Add(lPieces);
        //            }
        //        }
        //    }

        //    if (lSAPMaterialNo.Count > 0)
        //    {
        //        // get Contract Infor.

        //        try
        //        {

        //            var lConnect = new SapConnection("DEV");
        //            lConnect.Open();

        //            var lSAP = lConnect.Destination;
        //            IRfcFunction setOrderAPI = lSAP.Repository.CreateFunction("BAPI_SALESORDER_CREATEFROMDAT2");
        //            IRfcFunction setCommit = lSAP.Repository.CreateFunction("BAPI_TRANSACTION_COMMIT");

        //            IRfcStructure SAPStrucOrder_Header_In = setOrderAPI.GetStructure("ORDER_HEADER_IN");

        //            var lOrderType = lProcess.getSAPOrderType(pProjectCode, pContractNo, pPaymentType, "MTS");
        //            SAPStrucOrder_Header_In.SetValue("DOC_TYPE", lOrderType);

        //            var lSalesOrg = lProcess.getSAPSalesOrg(pProjectCode, pContractNo);
        //            SAPStrucOrder_Header_In.SetValue("SALES_ORG", lSalesOrg);

        //            SAPStrucOrder_Header_In.SetValue("DISTR_CHAN", "10");
        //            SAPStrucOrder_Header_In.SetValue("DIVISION", "00");
        //            SAPStrucOrder_Header_In.SetValue("REFDOC_CAT", "G");

        //            SAPStrucOrder_Header_In.SetValue("REQ_DATE_H", pReqDate);

        //            //if (pContractNo != "" && pPaymentType == "CREDIT")
        //            if (pContractNo != "")
        //            {
        //                if (pPriceDate != "" && pPriceDate != "000000")
        //                {
        //                    SAPStrucOrder_Header_In.SetValue("PRICE_DATE", pPriceDate);
        //                }

        //                if (pPaymentTerm != null && pPaymentTerm.Trim() != "")
        //                {
        //                    SAPStrucOrder_Header_In.SetValue("PMNTTRMS", pPaymentTerm);
        //                }

        //                if (pPriceGroup != null && pPriceGroup.Trim() != "")
        //                {
        //                    SAPStrucOrder_Header_In.SetValue("PRICE_GRP", pPriceGroup);
        //                }

        //                if (lCurrency != "")
        //                {
        //                    SAPStrucOrder_Header_In.SetValue("CURRENCY", lCurrency);
        //                }

        //                if (pIncoTerms1 != null && pIncoTerms1.Trim() != "")
        //                {
        //                    SAPStrucOrder_Header_In.SetValue("INCOTERMS1", pIncoTerms1);

        //                    if (pIncoTerms2 != null && pIncoTerms2.Trim() != "")
        //                    {
        //                        SAPStrucOrder_Header_In.SetValue("INCOTERMS2", pIncoTerms2);
        //                    }
        //                }
        //            }

        //            //PO Number, PO Date
        //            SAPStrucOrder_Header_In.SetValue("PURCH_NO_C", pPONo);
        //            SAPStrucOrder_Header_In.SetValue("PURCH_DATE", pPODate);

        //            //Contract Number
        //            if (pContractNo != "")
        //            {
        //                SAPStrucOrder_Header_In.SetValue("REF_DOC", pContractNo);
        //            }

        //            //Order Source
        //            string lOrderSource = "CUS-UX";
        //            if (pUserID == null || pUserID.IndexOf("@") < 0 || pUserID.ToLower().IndexOf("@natsteel.com.sg") > 0)
        //            {
        //                lOrderSource = "NSH-UX";
        //            }
        //            SAPStrucOrder_Header_In.SetValue("NAME", lOrderSource);

        //            // Internal/External Remarks
        //            IRfcTable lRemarks = setOrderAPI.GetTable("ORDER_TEXT");

        //            lRemarks.Append();
        //            lRemarks.SetValue("TEXT_ID", "Z010");
        //            lRemarks.SetValue("LANGU", "EN");
        //            lRemarks.SetValue("TEXT_LINE", pInRemarks);

        //            lRemarks.Append();
        //            lRemarks.SetValue("TEXT_ID", "Z011");
        //            lRemarks.SetValue("LANGU", "EN");
        //            lRemarks.SetValue("TEXT_LINE", pExRemarks);

        //            if (pContractNo == "")
        //            {
        //                lRemarks.Append();
        //                lRemarks.SetValue("TEXT_ID", "Z013");
        //                lRemarks.SetValue("LANGU", "EN");
        //                lRemarks.SetValue("TEXT_LINE", pInvRemarks);
        //            }

        //            IRfcTable SAPTableOrder_Partners = setOrderAPI.GetTable("ORDER_PARTNERS");

        //            SAPTableOrder_Partners.Append();
        //            //Customer Code
        //            SAPTableOrder_Partners.SetValue("PARTN_ROLE", "AG");
        //            SAPTableOrder_Partners.SetValue("PARTN_NUMB", pCustomerCode);
        //            SAPTableOrder_Partners.Append();
        //            //Project code
        //            SAPTableOrder_Partners.SetValue("PARTN_ROLE", "WE");
        //            SAPTableOrder_Partners.SetValue("PARTN_NUMB", pProjectCode);


        //            IRfcTable SAPTableOrder_Items_In = setOrderAPI.GetTable("ORDER_ITEMS_IN");
        //            IRfcTable SAPTableOrder_Schedule_In = setOrderAPI.GetTable("ORDER_SCHEDULES_IN");

        //            int lngTmpPosNr = 0;

        //            for (int i = 0; i < lSAPMaterialNo.Count; i++)
        //            {
        //                lngTmpPosNr = (i + 1) * 10;
        //                SAPTableOrder_Items_In.Append();
        //                SAPTableOrder_Items_In.SetValue("ITM_NUMBER", lngTmpPosNr);
        //                SAPTableOrder_Items_In.SetValue("MATERIAL", lSAPMaterialNo[i]);
        //                if (pContractNo != "")
        //                {
        //                    SAPTableOrder_Items_In.SetValue("REF_DOC", pContractNo);
        //                    SAPTableOrder_Items_In.SetValue("REF_DOC_IT", "000000");

        //                    SAPTableOrder_Items_In.SetValue("REF_DOC_CA", "G");
        //                }


        //                SAPTableOrder_Schedule_In.Append();
        //                SAPTableOrder_Schedule_In.SetValue("ITM_NUMBER", lngTmpPosNr);
        //                SAPTableOrder_Schedule_In.SetValue("SCHED_LINE", 1);
        //                SAPTableOrder_Schedule_In.SetValue("REQ_QTY", lSAPPcs[i]);
        //                SAPTableOrder_Schedule_In.SetValue("REQ_DATE", pReqDate);
        //            }

        //            IRfcTable SAPTableOrder_Ext_In = setOrderAPI.GetTable("EXTENSIONIN");

        //            SAPTableOrder_Ext_In.Append();
        //            SAPTableOrder_Ext_In.SetValue("STRUCTURE", "BAPE_VBAK");
        //            if (pContractNo != "")
        //            {
        //                SAPTableOrder_Ext_In.SetValue("VALUEPART1",
        //                    "          " +                                                  //SO (10)
        //                    (pVBAKExt.YTOT_GRAND / 1000).ToString("0000000000000.000") +    //YTOT_GRAND (17)
        //                    "                 " +                                           //YTOT_CAB (17)
        //                    (pVBAKExt.YTOT_MESH / 1000).ToString("0000000000000.000") +     //YTOT_MESH (17)
        //                    "                 " +                                           //YTOT_REBAR (17)
        //                    "                 " +                                           //YTOT_BPC (17)
        //                    "                 " +                                           //YTOT_PRECAGE (17)
        //                    "                 " +                                           //YTOTAL_WR (17)
        //                    "                 " +                                           //YTOTAL_PCSTRAND (17)
        //                    "                " +                                            //YTOT_PCSTRVAL (16)
        //                    "                 " +                                           //YTOT_COLD_ROLL (17)
        //                    "                 " +                                           //YTOT_PRE_CUTWR (17)
        //                    "                " +                                            //YTOTAL_VALUE (16)
        //                    (pVBAKExt.YTOT_COUPLER).ToString("000000") +                    //YTOT_COUPLER (6)
        //                    "                " +                                            //YTOT_COUPVAL (16)
        //                    pVBAKExt.YWIREROD_IND +
        //                    pVBAKExt.YREBAR_IND +
        //                    pVBAKExt.YCAB_IND +
        //                    pVBAKExt.YMESH_IND +
        //                    pVBAKExt.YPRECAGE +
        //                    pVBAKExt.YBPC_IND);
        //            }
        //            else
        //            {
        //                SAPTableOrder_Ext_In.SetValue("VALUEPART1",
        //                    "          " +                                                  //SO (10)
        //                    "                 " +                                           //YTOT_GRAND (17)
        //                    "                 " +                                           //YTOT_CAB (17)
        //                    "                 " +                                           //YTOT_MESH (17)
        //                    "                 " +                                           //YTOT_REBAR (17)
        //                    "                 " +                                           //YTOT_BPC (17)
        //                    "                 " +                                           //YTOT_PRECAGE (17)
        //                    "                 " +                                           //YTOTAL_WR (17)
        //                    "                 " +                                           //YTOTAL_PCSTRAND (17)
        //                    "                " +                                            //YTOT_PCSTRVAL (16)
        //                    "                 " +                                           //YTOT_COLD_ROLL (17)
        //                    "                 " +                                           //YTOT_PRE_CUTWR (17)
        //                    "                " +                                            //YTOTAL_VALUE (16)
        //                    "      " +                                                      //YTOT_COUPLER (6)
        //                    "                " +                                            //YTOT_COUPVAL (16)
        //                    pVBAKExt.YWIREROD_IND +
        //                    pVBAKExt.YREBAR_IND +
        //                    pVBAKExt.YCAB_IND +
        //                    pVBAKExt.YMESH_IND +
        //                    pVBAKExt.YPRECAGE +
        //                    pVBAKExt.YBPC_IND);
        //            }

        //            SAPTableOrder_Ext_In.SetValue("VALUEPART2",
        //                pVBAKExt.YPCSTRAND +
        //                pVBAKExt.YMAT_SOURCE +
        //                " " +                                       //YCAB_TYPE
        //                " " +                                       //YORD_TYPE
        //                "                                        " +    //YWBS1 (40)
        //                pVBAKExt.YCOLD_ROLL_WIRE +
        //                pVBAKExt.YPRE_CUT_WIRE +
        //                "                 " +                       //YTOT_BILLET(17)
        //                " " +                                       //YBILLET_IND
        //                " " +                                       //YCON_TYP
        //                "                 " +                       //YTOT_CAR(17)
        //                pVBAKExt.YCARPET_IND +
        //                pVBAKExt.YCSURCHARGE_IND);


        //            SAPTableOrder_Ext_In.Append();
        //            SAPTableOrder_Ext_In.SetValue("STRUCTURE", "BAPE_VBAKX");
        //            SAPTableOrder_Ext_In.SetValue("VALUEPART1", "          X X " +
        //                " " +       //YTOT_BPC
        //                " " +       //YTOT_PRECAGE
        //                " " +       //YTOTAL_WR
        //                " " +       //YTOTAL_PCSTRAND
        //                " " +       //YTOT_PCSTRVAL
        //                " " +       //YTOT_COLD_ROLL
        //                " " +       //YTOT_PRE_CUTWR
        //                " " +       //YTOTAL_VALUE
        //                "X" +       //YTOT_COUPLER
        //                " " +       //YTOT_COUPVAL
        //                "X" +       //YWIREROD_IND
        //                "X" +       //YREBAR_IND
        //                "X" +       //YCAB_IND
        //                "X" +       //YMESH_IND
        //                "X" +       //YPRECAGE
        //                "X" +       //YBPC_IND
        //                "X" +       //YPCSTRAND
        //                "X" +       //YMAT_SOURCE
        //                " " +       //YCAB_TYPE
        //                " " +       //YORD_TYPE
        //                " " +       //YWBS1
        //                "X" +       //YCOLD_ROLL_WIRE
        //                "X" +       //YPRE_CUT_WIRE
        //                " " +       //YTOT_BILLET
        //                " " +       //YBILLET_IND
        //                " " +       //YCON_TYP
        //                " " +       //YTOT_CAR
        //                "X" +       //YCARPET_IND
        //                "X"         //YCSURCHARGE_IND
        //                );

        //            RfcSessionManager.BeginContext(lSAP);
        //            setOrderAPI.Invoke(lSAP);
        //            setCommit.Invoke(lSAP);
        //            RfcSessionManager.EndContext(lSAP);

        //            String strVBELN;
        //            String strReturnType;
        //            String strReturnID;
        //            String strReturnNumber;
        //            String strReturnMessage;
        //            String strReturnType2;
        //            String strReturnID2;
        //            String strReturnNumber2;
        //            String strReturnMessage2;

        //            IRfcTable SAPReturn = setOrderAPI.GetTable("RETURN");
        //            strVBELN = setOrderAPI.GetValue("SALESDOCUMENT").ToString();

        //            strReturnMessage = "";
        //            if (strVBELN.Length <= 8)
        //            {
        //                if (SAPReturn.RowCount > 0)
        //                {
        //                    for (int i = 0; i < SAPReturn.RowCount; i++)
        //                    {
        //                        strReturnType = SAPReturn[i].GetString("TYPE");
        //                        strReturnID = SAPReturn[i].GetString("ID");
        //                        strReturnNumber = SAPReturn[i].GetString("NUMBER");
        //                        if (SAPReturn[i].GetString("MESSAGE") != null && SAPReturn[i].GetString("MESSAGE").ToUpper().IndexOf("SUCCESS") < 0)
        //                        {
        //                            if (strReturnMessage == "")
        //                            {
        //                                strReturnMessage = SAPReturn[i].GetString("MESSAGE");
        //                            }
        //                            else
        //                            {
        //                                strReturnMessage = strReturnMessage + "\n" + SAPReturn[i].GetString("MESSAGE");
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    strReturnType = setOrderAPI.GetTable("RETURN").GetString("TYPE");
        //                    strReturnID = setOrderAPI.GetTable("RETURN").GetString("ID");
        //                    strReturnNumber = setOrderAPI.GetTable("RETURN").GetString("NUMBER");
        //                    strReturnMessage = setOrderAPI.GetTable("RETURN").GetString("MESSAGE");
        //                }
        //            }

        //            strReturnType2 = setCommit.GetStructure("RETURN").GetString("TYPE");
        //            strReturnID2 = setCommit.GetStructure("RETURN").GetString("ID");
        //            strReturnNumber2 = setCommit.GetStructure("RETURN").GetString("NUMBER");
        //            strReturnMessage2 = setCommit.GetStructure("RETURN").GetString("MESSAGE");

        //            if (strVBELN.Length > 8)
        //            {
        //                lReturn = strVBELN;
        //            }
        //            else
        //            {
        //                lReturn = strReturnMessage;
        //            }

        //            lConnect.Dispose();
        //            lSAP = null;
        //            lConnect = null;
        //        }

        //        catch (RfcCommunicationException e)
        //        {
        //            lReturn = e.Message;
        //        }
        //        catch (RfcLogonException e)
        //        {
        //            // user could not logon...
        //            lReturn = e.Message;
        //        }
        //        catch (RfcAbapRuntimeException e)
        //        {
        //            // serious problem on ABAP system side...
        //            lReturn = e.Message;
        //        }
        //        catch (RfcAbapBaseException e)
        //        {
        //            lReturn = e.Message;
        //            // The function module returned an ABAP exception, an ABAP message
        //            // or an ABAP class-based exception...
        //        }
        //        catch (Exception e)
        //        {
        //            lReturn = e.Message;
        //            // Other type of exception
        //        }
        //    }

        //    lProcess = null;

        //    return lReturn;
        //}

        //public string CreateStdProdSAPSO(string pCustomerCode, string pProjectCode, string pContractNo,
        //    string pReqDate, string pInRemarks, string pExRemarks, string pInvRemarks, string pPONo, string pPODate,
        //    string pBBSNo, string pBBSDesc,
        //    List<StdProdDetailsModels> pBBSDetails, string pPaymentType, SAPVBAKExtModels pVBAKExt,
        //    string pPriceDate, string pPaymentTerm, string pPriceGroup,
        //    string pIncoTerms1, string pIncoTerms2, string pUserID)
        //{
        //    string lReturn = "";
        //    List<string> lSAPCode;
        //    string lBarType = "";
        //    short lBarSize = 0;
        //    int lBarLength = 0;

        //    string lCurrency = "";
        //    string lSQL = "";

        //    OracleCommand cmdOrdHdr = new OracleCommand();
        //    OracleDataReader lOrdRst;
        //    OracleConnection cnCIS = new OracleConnection();

        //    ProcessController lProcess = new ProcessController();

        //    lProcess.OpenCISConnection(ref cnCIS);

        //    lSQL = "SELECT WAERK " +
        //    "FROM SAPSR3.VBAK " +
        //    "WHERE MANDT = '" + lProcess.strClient + "' " +
        //    "AND VBELN = '" + pContractNo + "' ";

        //    cmdOrdHdr.CommandText = lSQL;
        //    cmdOrdHdr.Connection = cnCIS;
        //    cmdOrdHdr.CommandTimeout = 300;
        //    lOrdRst = cmdOrdHdr.ExecuteReader();
        //    if (lOrdRst.HasRows)
        //    {
        //        lOrdRst.Read();
        //        lCurrency = lOrdRst.GetString(0) == null ? "" : lOrdRst.GetString(0);
        //    }
        //    lOrdRst.Close();
        //    lProcess.CloseCISConnection(ref cnCIS);

        //    lCurrency = lCurrency.Trim();

        //    List<string> lSAPMaterialNo = new List<string>();
        //    List<int> lSAPkg = new List<int>();

        //    if (pBBSDetails.Count > 0)
        //    {
        //        for (var i = 0; i < pBBSDetails.Count; i++)
        //        {
        //            if (pBBSDetails[i].ProdCode != null && pBBSDetails[i].ProdCode != "" && (pBBSDetails[i].order_wt > 0 || pBBSDetails[i].order_pcs > 0))
        //            {
        //                if (pBBSDetails[i].ProdCode.Substring(0, 3) == "FUN" || pBBSDetails[i].ProdCode.Substring(0, 3) == "FUE")
        //                {
        //                    lSAPMaterialNo.Add(pBBSDetails[i].ProdCode.Trim());
        //                    lSAPkg.Add((int)pBBSDetails[i].order_pcs);
        //                }
        //                else
        //                {
        //                    lSAPMaterialNo.Add(pBBSDetails[i].ProdCode.Trim());
        //                    lSAPkg.Add((int)pBBSDetails[i].order_wt);
        //                }
        //            }
        //        }
        //    }

        //    if (lSAPMaterialNo.Count > 0)
        //    {
        //        // get Contract Infor.

        //        try
        //        {
        //            var lConnect = new SapConnection("DEV");
        //            lConnect.Open();

        //            var lSAP = lConnect.Destination;
        //            IRfcFunction setOrderAPI = lSAP.Repository.CreateFunction("BAPI_SALESORDER_CREATEFROMDAT2");
        //            IRfcFunction setCommit = lSAP.Repository.CreateFunction("BAPI_TRANSACTION_COMMIT");

        //            IRfcStructure SAPStrucOrder_Header_In = setOrderAPI.GetStructure("ORDER_HEADER_IN");

        //            var lOrderType = lProcess.getSAPOrderType(pProjectCode, pContractNo, pPaymentType, "MTS");
        //            SAPStrucOrder_Header_In.SetValue("DOC_TYPE", lOrderType);

        //            var lSalesOrg = lProcess.getSAPSalesOrg(pProjectCode, pContractNo);
        //            SAPStrucOrder_Header_In.SetValue("SALES_ORG", lSalesOrg);

        //            SAPStrucOrder_Header_In.SetValue("DISTR_CHAN", "10");
        //            SAPStrucOrder_Header_In.SetValue("DIVISION", "00");
        //            SAPStrucOrder_Header_In.SetValue("REFDOC_CAT", "G");

        //            SAPStrucOrder_Header_In.SetValue("REQ_DATE_H", pReqDate);

        //            if (pContractNo != "" && pPaymentType == "CREDIT")
        //            {
        //                SAPStrucOrder_Header_In.SetValue("PRICE_DATE", pPriceDate);

        //                if (pPaymentTerm != null && pPaymentTerm.Trim() != "")
        //                {
        //                    SAPStrucOrder_Header_In.SetValue("PMNTTRMS", pPaymentTerm);
        //                }

        //                if (pPriceGroup != null && pPriceGroup.Trim() != "")
        //                {
        //                    SAPStrucOrder_Header_In.SetValue("PRICE_GRP", pPriceGroup);
        //                }

        //                if (lCurrency != "")
        //                {
        //                    SAPStrucOrder_Header_In.SetValue("CURRENCY", lCurrency);
        //                }

        //                if (pIncoTerms1 != null && pIncoTerms1.Trim() != "")
        //                {
        //                    SAPStrucOrder_Header_In.SetValue("INCOTERMS1", pIncoTerms1);

        //                    if (pIncoTerms2 != null && pIncoTerms2.Trim() != "")
        //                    {
        //                        SAPStrucOrder_Header_In.SetValue("INCOTERMS2", pIncoTerms2);
        //                    }
        //                }
        //            }

        //            //PO Number, PO Date
        //            SAPStrucOrder_Header_In.SetValue("PURCH_NO_C", pPONo);
        //            SAPStrucOrder_Header_In.SetValue("PURCH_DATE", pPODate);

        //            //Contract Number
        //            if (pContractNo != "")
        //            {
        //                SAPStrucOrder_Header_In.SetValue("REF_DOC", pContractNo);
        //            }

        //            //Order Source
        //            string lOrderSource = "CUS-UX";
        //            if (pUserID == null || pUserID.IndexOf("@") < 0 || pUserID.ToLower().IndexOf("@natsteel.com.sg") > 0)
        //            {
        //                lOrderSource = "NSH-UX";
        //            }
        //            SAPStrucOrder_Header_In.SetValue("NAME", lOrderSource);

        //            // Internal/External Remarks
        //            IRfcTable lRemarks = setOrderAPI.GetTable("ORDER_TEXT");

        //            lRemarks.Append();
        //            lRemarks.SetValue("TEXT_ID", "Z010");
        //            lRemarks.SetValue("LANGU", "EN");
        //            lRemarks.SetValue("TEXT_LINE", pInRemarks);

        //            lRemarks.Append();
        //            lRemarks.SetValue("TEXT_ID", "Z011");
        //            lRemarks.SetValue("LANGU", "EN");
        //            lRemarks.SetValue("TEXT_LINE", pExRemarks);

        //            if (pContractNo == "")
        //            {
        //                lRemarks.Append();
        //                lRemarks.SetValue("TEXT_ID", "Z013");
        //                lRemarks.SetValue("LANGU", "EN");
        //                lRemarks.SetValue("TEXT_LINE", pInvRemarks);
        //            }

        //            IRfcTable SAPTableOrder_Partners = setOrderAPI.GetTable("ORDER_PARTNERS");

        //            SAPTableOrder_Partners.Append();
        //            //Customer Code
        //            SAPTableOrder_Partners.SetValue("PARTN_ROLE", "AG");
        //            SAPTableOrder_Partners.SetValue("PARTN_NUMB", pCustomerCode);
        //            SAPTableOrder_Partners.Append();
        //            //Project code
        //            SAPTableOrder_Partners.SetValue("PARTN_ROLE", "WE");
        //            SAPTableOrder_Partners.SetValue("PARTN_NUMB", pProjectCode);


        //            IRfcTable SAPTableOrder_Items_In = setOrderAPI.GetTable("ORDER_ITEMS_IN");
        //            IRfcTable SAPTableOrder_Schedule_In = setOrderAPI.GetTable("ORDER_SCHEDULES_IN");

        //            int lngTmpPosNr = 0;

        //            for (int i = 0; i < lSAPMaterialNo.Count; i++)
        //            {
        //                lngTmpPosNr = (i + 1) * 10;
        //                SAPTableOrder_Items_In.Append();
        //                SAPTableOrder_Items_In.SetValue("ITM_NUMBER", lngTmpPosNr);
        //                SAPTableOrder_Items_In.SetValue("MATERIAL", lSAPMaterialNo[i]);
        //                if (pContractNo != "")
        //                {
        //                    SAPTableOrder_Items_In.SetValue("REF_DOC", pContractNo);
        //                    SAPTableOrder_Items_In.SetValue("REF_DOC_IT", "000000");

        //                    SAPTableOrder_Items_In.SetValue("REF_DOC_CA", "G");
        //                }

        //                //BBS No, BBS Description
        //                //SAPTableOrder_Items_In.SetValue("YBBS_NO", pBBSNo);
        //                //SAPTableOrder_Items_In.SetValue("YBBS_DESCR", pBBSDesc);

        //                lRemarks.Append();
        //                lRemarks.SetValue("TEXT_ID", "Z106");
        //                lRemarks.SetValue("ITM_NUMBER", lngTmpPosNr);
        //                lRemarks.SetValue("LANGU", "EN");
        //                lRemarks.SetValue("TEXT_LINE", pBBSNo);

        //                lRemarks.Append();
        //                lRemarks.SetValue("TEXT_ID", "Z107");
        //                lRemarks.SetValue("ITM_NUMBER", lngTmpPosNr);
        //                lRemarks.SetValue("LANGU", "EN");
        //                lRemarks.SetValue("TEXT_LINE", pBBSDesc);

        //                SAPTableOrder_Schedule_In.Append();
        //                SAPTableOrder_Schedule_In.SetValue("ITM_NUMBER", lngTmpPosNr);
        //                SAPTableOrder_Schedule_In.SetValue("SCHED_LINE", 1);
        //                if (lSAPMaterialNo[i].Substring(0, 3) == "FUN" || lSAPMaterialNo[i].Substring(0, 3) == "FUE")
        //                {
        //                    SAPTableOrder_Schedule_In.SetValue("REQ_QTY", lSAPkg[i]);
        //                }
        //                else
        //                {
        //                    SAPTableOrder_Schedule_In.SetValue("REQ_QTY", (double)Math.Round((double)lSAPkg[i] / 1000, 3));
        //                }
        //                SAPTableOrder_Schedule_In.SetValue("REQ_DATE", pReqDate);
        //            }

        //            IRfcTable SAPTableOrder_Ext_In = setOrderAPI.GetTable("EXTENSIONIN");

        //            SAPTableOrder_Ext_In.Append();
        //            SAPTableOrder_Ext_In.SetValue("STRUCTURE", "BAPE_VBAK");
        //            SAPTableOrder_Ext_In.SetValue("VALUEPART1",
        //                "          " +                                                  //SO (10)
        //                (pVBAKExt.YTOT_GRAND / 1000).ToString("0000000000000.000") +    //YTOT_GRAND (17)
        //                "                 " +                                           //YTOT_CAB (17)
        //                "                 " +                                           //YTOT_MESH (17)
        //                (pVBAKExt.YTOT_REBAR / 1000).ToString("0000000000000.000") +    //YTOT_REBAR (17)
        //                "                 " +                                           //YTOT_BPC (17)
        //                "                 " +                                           //YTOT_PRECAGE (17)
        //                (pVBAKExt.YTOT_WIROD / 1000).ToString("0000000000000.000") +    //YTOTAL_WR (17)
        //                (pVBAKExt.YTOT_PCSTR / 1000).ToString("0000000000000.000") +    //YTOTAL_PCSTRAND (17)
        //                "                " +                                            //YTOT_PCSTRVAL (16)
        //                (pVBAKExt.YTOT_CRWIC / 1000).ToString("0000000000000.000") +    //YTOT_COLD_ROLL (17)
        //                "                 " +                                           //YTOT_PRE_CUTWR (17)
        //                "                " +                                            //YTOTAL_VALUE (16)
        //                (pVBAKExt.YTOT_COUPLER).ToString("000000") +                    //YTOT_COUPLER (6)
        //                "                " +                                            //YTOT_COUPVAL (16)
        //                pVBAKExt.YWIREROD_IND +
        //                pVBAKExt.YREBAR_IND +
        //                pVBAKExt.YCAB_IND +
        //                pVBAKExt.YMESH_IND +
        //                pVBAKExt.YPRECAGE +
        //                pVBAKExt.YBPC_IND);

        //            SAPTableOrder_Ext_In.SetValue("VALUEPART2",
        //                pVBAKExt.YPCSTRAND +
        //                pVBAKExt.YMAT_SOURCE +
        //                " " +                                       //YCAB_TYPE
        //                " " +                                       //YORD_TYPE
        //                "                                        " +    //YWBS1 (40)
        //                pVBAKExt.YCOLD_ROLL_WIRE +
        //                pVBAKExt.YPRE_CUT_WIRE +
        //                "                 " +                       //YTOT_BILLET(17)
        //                " " +                                       //YBILLET_IND
        //                " " +                                       //YCON_TYP
        //                "                 " +                       //YTOT_CAR(17)
        //                pVBAKExt.YCARPET_IND +
        //                pVBAKExt.YCSURCHARGE_IND);


        //            SAPTableOrder_Ext_In.Append();
        //            SAPTableOrder_Ext_In.SetValue("STRUCTURE", "BAPE_VBAKX");
        //            SAPTableOrder_Ext_In.SetValue("VALUEPART1", "          X  X" +
        //                " " +       //YTOT_BPC
        //                " " +       //YTOT_PRECAGE
        //                "X" +       //YTOTAL_WR
        //                "X" +       //YTOTAL_PCSTRAND
        //                " " +       //YTOT_PCSTRVAL
        //                "X" +       //YTOT_COLD_ROLL
        //                " " +       //YTOT_PRE_CUTWR
        //                " " +       //YTOTAL_VALUE
        //                "X" +       //YTOT_COUPLER
        //                " " +       //YTOT_COUPVAL
        //                "X" +       //YWIREROD_IND
        //                "X" +       //YREBAR_IND
        //                "X" +       //YCAB_IND
        //                "X" +       //YMESH_IND
        //                "X" +       //YPRECAGE
        //                "X" +       //YBPC_IND
        //                "X" +       //YPCSTRAND
        //                "X" +       //YMAT_SOURCE
        //                " " +       //YCAB_TYPE
        //                " " +       //YORD_TYPE
        //                " " +       //YWBS1
        //                "X" +       //YCOLD_ROLL_WIRE
        //                "X" +       //YPRE_CUT_WIRE
        //                " " +       //YTOT_BILLET
        //                " " +       //YBILLET_IND
        //                " " +       //YCON_TYP
        //                " " +       //YTOT_CAR
        //                "X" +       //YCARPET_IND
        //                "X"         //YCSURCHARGE_IND
        //                );

        //            RfcSessionManager.BeginContext(lSAP);
        //            setOrderAPI.Invoke(lSAP);
        //            setCommit.Invoke(lSAP);
        //            RfcSessionManager.EndContext(lSAP);

        //            String strVBELN;
        //            String strReturnType;
        //            String strReturnID;
        //            String strReturnNumber;
        //            String strReturnMessage;
        //            String strReturnType2;
        //            String strReturnID2;
        //            String strReturnNumber2;
        //            String strReturnMessage2;

        //            IRfcTable SAPReturn = setOrderAPI.GetTable("RETURN");
        //            strVBELN = setOrderAPI.GetValue("SALESDOCUMENT").ToString();

        //            strReturnMessage = "";
        //            if (strVBELN.Length <= 8)
        //            {
        //                if (SAPReturn.RowCount > 0)
        //                {
        //                    for (int i = 0; i < SAPReturn.RowCount; i++)
        //                    {
        //                        strReturnType = SAPReturn[i].GetString("TYPE");
        //                        strReturnID = SAPReturn[i].GetString("ID");
        //                        strReturnNumber = SAPReturn[i].GetString("NUMBER");
        //                        if (SAPReturn[i].GetString("MESSAGE") != null && SAPReturn[i].GetString("MESSAGE").ToUpper().IndexOf("SUCCESS") < 0)
        //                        {
        //                            if (strReturnMessage == "")
        //                            {
        //                                strReturnMessage = SAPReturn[i].GetString("MESSAGE");
        //                            }
        //                            else
        //                            {
        //                                strReturnMessage = strReturnMessage + "\n" + SAPReturn[i].GetString("MESSAGE");
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    strReturnType = setOrderAPI.GetTable("RETURN").GetString("TYPE");
        //                    strReturnID = setOrderAPI.GetTable("RETURN").GetString("ID");
        //                    strReturnNumber = setOrderAPI.GetTable("RETURN").GetString("NUMBER");
        //                    strReturnMessage = setOrderAPI.GetTable("RETURN").GetString("MESSAGE");
        //                }
        //            }

        //            strReturnType2 = setCommit.GetStructure("RETURN").GetString("TYPE");
        //            strReturnID2 = setCommit.GetStructure("RETURN").GetString("ID");
        //            strReturnNumber2 = setCommit.GetStructure("RETURN").GetString("NUMBER");
        //            strReturnMessage2 = setCommit.GetStructure("RETURN").GetString("MESSAGE");

        //            if (strVBELN.Length > 8)
        //            {
        //                lReturn = strVBELN;
        //            }
        //            else
        //            {
        //                lReturn = strReturnMessage;
        //            }

        //            lConnect.Dispose();
        //            lSAP = null;
        //            lConnect = null;
        //        }

        //        catch (RfcCommunicationException e)
        //        {
        //            lReturn = e.Message;
        //        }
        //        catch (RfcLogonException e)
        //        {
        //            // user could not logon...
        //            lReturn = e.Message;
        //        }
        //        catch (RfcAbapRuntimeException e)
        //        {
        //            // serious problem on ABAP system side...
        //            lReturn = e.Message;
        //        }
        //        catch (RfcAbapBaseException e)
        //        {
        //            lReturn = e.Message;
        //            // The function module returned an ABAP exception, an ABAP message
        //            // or an ABAP class-based exception...
        //        }
        //        catch (Exception e)
        //        {
        //            lReturn = e.Message;
        //            // Other type of exception
        //        }
        //    }

        //    lProcess = null;

        //    return lReturn;
        //}

        private DBContextModels db = new DBContextModels();
        public string CreateSAPSO(string pCustomerCode, string pProjectCode, string pContractNo,
         string pReqDate, string pInRemarks, string pExRemarks, string pInvRemarks, string pPONo, string pPODate,
         string pBBSNo, string pBBSDesc,
         List<OrderDetailsModels> pBBSDetails, string pPaymentType, SAPVBAKExtModels pVBAKExt,
         string pPriceDate, string pPaymentTerm, string pPriceGroup,
         string pIncoTerms1, string pIncoTerms2, string pUserID)
        {

            string lReturn = "";
            List<string> lSAPCode;
            string lBarType = "";
            short lBarSize = 0;
            int lBarLength = 0;
            string lCurrency = "";
            string lSQL = "";

            OracleCommand cmdOrdHdr = new OracleCommand();
            OracleDataReader lOrdRst;
            OracleConnection cnCIS = new OracleConnection();

            List<string> lSAPMaterialNo = new List<string>();
            List<int> lSAPkg = new List<int>();

            ProcessController lProcess = new ProcessController();

            lProcess.OpenCISConnection(ref cnCIS);

            lSQL = "SELECT WAERK " +
            "FROM SAPSR3.VBAK " +
            "WHERE MANDT = '" + lProcess.strClient + "' " +
            "AND VBELN = '" + pContractNo + "' ";

            cmdOrdHdr.CommandText = lSQL;
            cmdOrdHdr.Connection = cnCIS;
            cmdOrdHdr.CommandTimeout = 300;
            lOrdRst = cmdOrdHdr.ExecuteReader();
            if (lOrdRst.HasRows)
            {
                lOrdRst.Read();
                lCurrency = lOrdRst.GetString(0) == null ? "" : lOrdRst.GetString(0);
            }
            lOrdRst.Close();
            lProcess.CloseCISConnection(ref cnCIS);

            lCurrency = lCurrency.Trim();

            //convert order to SAP material
            var lProdCode = (from p in db.ProdCode
                             orderby p.BarType, p.BarSize
                             select p).ToList();
            if (lProdCode.Count > 0 && pBBSDetails.Count > 0)
            {
                for (var i = 0; i < pBBSDetails.Count; i++)
                {
                    lSAPCode = new List<string>();
                    lBarType = pBBSDetails[i].BarType;
                    lBarSize = (short)(pBBSDetails[i].BarSize == null ? 0 : pBBSDetails[i].BarSize);
                    lBarLength = (int)(pBBSDetails[i].BarLength == null ? 0 : pBBSDetails[i].BarLength);

                    lSAPCode = (from p in lProdCode
                                where p.BarType == lBarType &&
                                p.BarSize == lBarSize &&
                                p.BarLength == lBarLength
                                select p.ProdCodeSAP).ToList();
                    if (lSAPCode.Count > 0 && pBBSDetails[i].BarTotalQty > 0)
                    {
                        if (lSAPCode[0].Length > 0 && pBBSDetails[i].BarTotalQty > 0)
                        {
                            lSAPMaterialNo.Add(lSAPCode[0]);
                            lSAPkg.Add((int)pBBSDetails[i].BarWeight);
                        }
                    }
                }
            }

            if (lSAPMaterialNo.Count > 0)
            {
                // get Contract Infor.

                try
                {
                    var lConnect = new SapConnection("DEV");
                    lConnect.Open();

                    var lSAP = lConnect.Destination;
                    IRfcFunction setOrderAPI = lSAP.Repository.CreateFunction("BAPI_SALESORDER_CREATEFROMDAT2");
                    IRfcFunction setCommit = lSAP.Repository.CreateFunction("BAPI_TRANSACTION_COMMIT");

                    IRfcStructure SAPStrucOrder_Header_In = setOrderAPI.GetStructure("ORDER_HEADER_IN");

                    var lOrderType = lProcess.getSAPOrderType(pProjectCode, pContractNo, pPaymentType, "MTS");
                    SAPStrucOrder_Header_In.SetValue("DOC_TYPE", lOrderType);

                    var lSalesOrg = lProcess.getSAPSalesOrg(pProjectCode, pContractNo);
                    SAPStrucOrder_Header_In.SetValue("SALES_ORG", lSalesOrg);

                    SAPStrucOrder_Header_In.SetValue("DISTR_CHAN", "10");
                    SAPStrucOrder_Header_In.SetValue("DIVISION", "00");
                    SAPStrucOrder_Header_In.SetValue("REFDOC_CAT", "G");

                    SAPStrucOrder_Header_In.SetValue("REQ_DATE_H", pReqDate);


                    if (pContractNo != "" && pPaymentType == "CREDIT")
                    {
                        SAPStrucOrder_Header_In.SetValue("PRICE_DATE", pPriceDate);

                        if (lCurrency != "")
                        {
                            SAPStrucOrder_Header_In.SetValue("CURRENCY", lCurrency);
                        }

                        if (pPaymentTerm != null && pPaymentTerm.Trim() != "")
                        {
                            SAPStrucOrder_Header_In.SetValue("PMNTTRMS", pPaymentTerm);
                        }

                        if (pPriceGroup != null && pPriceGroup.Trim() != "")
                        {
                            SAPStrucOrder_Header_In.SetValue("PRICE_GRP", pPriceGroup);
                        }

                        if (pIncoTerms1 != null && pIncoTerms1.Trim() != "")
                        {
                            SAPStrucOrder_Header_In.SetValue("INCOTERMS1", pIncoTerms1);

                            if (pIncoTerms2 != null && pIncoTerms2.Trim() != "")
                            {
                                SAPStrucOrder_Header_In.SetValue("INCOTERMS2", pIncoTerms2);
                            }
                        }
                    }

                    //PO Number, PO Date
                    SAPStrucOrder_Header_In.SetValue("PURCH_NO_C", pPONo);
                    SAPStrucOrder_Header_In.SetValue("PURCH_DATE", pPODate);

                    //Contract Number
                    if (pContractNo != "")
                    {
                        SAPStrucOrder_Header_In.SetValue("REF_DOC", pContractNo);
                    }

                    //Order Source
                    string lOrderSource = "CUS-UX";
                    if (pUserID == null || pUserID.IndexOf("@") < 0 || pUserID.ToLower().IndexOf("@natsteel.com.sg") > 0)
                    {
                        lOrderSource = "NSH-UX";
                    }
                    SAPStrucOrder_Header_In.SetValue("NAME", lOrderSource);

                    // Internal/External Remarks
                    IRfcTable lRemarks = setOrderAPI.GetTable("ORDER_TEXT");

                    lRemarks.Append();
                    lRemarks.SetValue("TEXT_ID", "Z010");
                    lRemarks.SetValue("LANGU", "EN");
                    lRemarks.SetValue("TEXT_LINE", pInRemarks);

                    lRemarks.Append();
                    lRemarks.SetValue("TEXT_ID", "Z011");
                    lRemarks.SetValue("LANGU", "EN");
                    lRemarks.SetValue("TEXT_LINE", pExRemarks);

                    if (pContractNo == "")
                    {
                        lRemarks.Append();
                        lRemarks.SetValue("TEXT_ID", "Z013");
                        lRemarks.SetValue("LANGU", "EN");
                        lRemarks.SetValue("TEXT_LINE", pInvRemarks);
                    }

                    IRfcTable SAPTableOrder_Partners = setOrderAPI.GetTable("ORDER_PARTNERS");

                    SAPTableOrder_Partners.Append();
                    //Customer Code
                    SAPTableOrder_Partners.SetValue("PARTN_ROLE", "AG");
                    SAPTableOrder_Partners.SetValue("PARTN_NUMB", pCustomerCode);
                    SAPTableOrder_Partners.Append();
                    //Project code
                    SAPTableOrder_Partners.SetValue("PARTN_ROLE", "WE");
                    SAPTableOrder_Partners.SetValue("PARTN_NUMB", pProjectCode);


                    IRfcTable SAPTableOrder_Items_In = setOrderAPI.GetTable("ORDER_ITEMS_IN");
                    IRfcTable SAPTableOrder_Schedule_In = setOrderAPI.GetTable("ORDER_SCHEDULES_IN");

                    int lngTmpPosNr = 0;

                    for (int i = 0; i < lSAPMaterialNo.Count; i++)
                    {
                        lngTmpPosNr = (i + 1) * 10;
                        SAPTableOrder_Items_In.Append();
                        SAPTableOrder_Items_In.SetValue("ITM_NUMBER", lngTmpPosNr);
                        SAPTableOrder_Items_In.SetValue("MATERIAL", lSAPMaterialNo[i]);
                        if (pContractNo != "")
                        {
                            SAPTableOrder_Items_In.SetValue("REF_DOC", pContractNo);
                            SAPTableOrder_Items_In.SetValue("REF_DOC_IT", "000000");

                            SAPTableOrder_Items_In.SetValue("REF_DOC_CA", "G");
                        }

                        //BBS No, BBS Description
                        //SAPTableOrder_Items_In.SetValue("YBBS_NO", pBBSNo);
                        //SAPTableOrder_Items_In.SetValue("YBBS_DESCR", pBBSDesc);

                        lRemarks.Append();
                        lRemarks.SetValue("TEXT_ID", "Z106");
                        lRemarks.SetValue("ITM_NUMBER", lngTmpPosNr);
                        lRemarks.SetValue("LANGU", "EN");
                        lRemarks.SetValue("TEXT_LINE", pBBSNo);

                        lRemarks.Append();
                        lRemarks.SetValue("TEXT_ID", "Z107");
                        lRemarks.SetValue("ITM_NUMBER", lngTmpPosNr);
                        lRemarks.SetValue("LANGU", "EN");
                        lRemarks.SetValue("TEXT_LINE", pBBSDesc);

                        SAPTableOrder_Schedule_In.Append();
                        SAPTableOrder_Schedule_In.SetValue("ITM_NUMBER", lngTmpPosNr);
                        SAPTableOrder_Schedule_In.SetValue("SCHED_LINE", 1);
                        SAPTableOrder_Schedule_In.SetValue("REQ_QTY", lSAPkg[i] / 1000);
                        SAPTableOrder_Schedule_In.SetValue("REQ_DATE", pReqDate);
                    }

                    IRfcTable SAPTableOrder_Ext_In = setOrderAPI.GetTable("EXTENSIONIN");

                    SAPTableOrder_Ext_In.Append();
                    SAPTableOrder_Ext_In.SetValue("STRUCTURE", "BAPE_VBAK");
                    SAPTableOrder_Ext_In.SetValue("VALUEPART1",
                        "          " +                                                  //SO (10)
                        (pVBAKExt.YTOT_GRAND / 1000).ToString("0000000000000.000") +    //YTOT_GRAND (17)
                        "                 " +                                           //YTOT_CAB (17)
                        "                 " +                                           //YTOT_MESH (17)
                        (pVBAKExt.YTOT_REBAR / 1000).ToString("0000000000000.000") +    //YTOT_REBAR (17)
                        "                 " +                                           //YTOT_BPC (17)
                        "                 " +                                           //YTOT_PRECAGE (17)
                        "                 " +                                           //YTOTAL_WR (17)
                        "                 " +                                           //YTOTAL_PCSTRAND (17)
                        "                " +                                            //YTOT_PCSTRVAL (16)
                        "                 " +                                           //YTOT_COLD_ROLL (17)
                        "                 " +                                           //YTOT_PRE_CUTWR (17)
                        "                " +                                            //YTOTAL_VALUE (16)
                        "      " +                                                      //YTOT_COUPLER (6)
                        "                " +                                            //YTOT_COUPVAL (16)
                        pVBAKExt.YWIREROD_IND +
                        pVBAKExt.YREBAR_IND +
                        pVBAKExt.YCAB_IND +
                        pVBAKExt.YMESH_IND +
                        pVBAKExt.YPRECAGE +
                        pVBAKExt.YBPC_IND);

                    SAPTableOrder_Ext_In.SetValue("VALUEPART2",
                        pVBAKExt.YPCSTRAND +
                        pVBAKExt.YMAT_SOURCE +
                        " " +                                       //YCAB_TYPE
                        " " +                                       //YORD_TYPE
                        "                                        " +    //YWBS1 (40)
                        pVBAKExt.YCOLD_ROLL_WIRE +
                        pVBAKExt.YPRE_CUT_WIRE +
                        "                 " +                       //YTOT_BILLET(17)
                        " " +                                       //YBILLET_IND
                        " " +                                       //YCON_TYP
                        "                 " +                       //YTOT_CAR(17)
                        pVBAKExt.YCARPET_IND +
                        pVBAKExt.YCSURCHARGE_IND);


                    SAPTableOrder_Ext_In.Append();
                    SAPTableOrder_Ext_In.SetValue("STRUCTURE", "BAPE_VBAKX");
                    SAPTableOrder_Ext_In.SetValue("VALUEPART1", "          X  X" +
                        " " +       //YTOT_BPC
                        " " +       //YTOT_PRECAGE
                        " " +       //YTOTAL_WR
                        " " +       //YTOTAL_PCSTRAND
                        " " +       //YTOT_PCSTRVAL
                        " " +       //YTOT_COLD_ROLL
                        " " +       //YTOT_PRE_CUTWR
                        " " +       //YTOTAL_VALUE
                        " " +       //YTOT_COUPLER
                        " " +       //YTOT_COUPVAL
                        "X" +       //YWIREROD_IND
                        "X" +       //YREBAR_IND
                        "X" +       //YCAB_IND
                        "X" +       //YMESH_IND
                        "X" +       //YPRECAGE
                        "X" +       //YBPC_IND
                        "X" +       //YPCSTRAND
                        "X" +       //YMAT_SOURCE
                        " " +       //YCAB_TYPE
                        " " +       //YORD_TYPE
                        " " +       //YWBS1
                        "X" +       //YCOLD_ROLL_WIRE
                        "X" +       //YPRE_CUT_WIRE
                        " " +       //YTOT_BILLET
                        " " +       //YBILLET_IND
                        " " +       //YCON_TYP
                        " " +       //YTOT_CAR
                        "X" +       //YCARPET_IND
                        "X"         //YCSURCHARGE_IND
                        );

                    RfcSessionManager.BeginContext(lSAP);
                    setOrderAPI.Invoke(lSAP);
                    setCommit.Invoke(lSAP);
                    RfcSessionManager.EndContext(lSAP);

                    String strVBELN;
                    String strReturnType;
                    String strReturnID;
                    String strReturnNumber;
                    String strReturnMessage;
                    String strReturnType2;
                    String strReturnID2;
                    String strReturnNumber2;
                    String strReturnMessage2;

                    IRfcTable SAPReturn = setOrderAPI.GetTable("RETURN");
                    strVBELN = setOrderAPI.GetValue("SALESDOCUMENT").ToString();

                    strReturnMessage = "";
                    if (strVBELN.Length <= 8)
                    {
                        if (SAPReturn.RowCount > 0)
                        {
                            for (int i = 0; i < SAPReturn.RowCount; i++)
                            {
                                strReturnType = SAPReturn[i].GetString("TYPE");
                                strReturnID = SAPReturn[i].GetString("ID");
                                strReturnNumber = SAPReturn[i].GetString("NUMBER");
                                if (SAPReturn[i].GetString("MESSAGE") != null && SAPReturn[i].GetString("MESSAGE").ToUpper().IndexOf("SUCCESS") < 0)
                                {
                                    if (strReturnMessage == "")
                                    {
                                        strReturnMessage = SAPReturn[i].GetString("MESSAGE");
                                    }
                                    else
                                    {
                                        strReturnMessage = strReturnMessage + "\n" + SAPReturn[i].GetString("MESSAGE");
                                    }
                                }
                            }
                        }
                        else
                        {
                            strReturnType = setOrderAPI.GetTable("RETURN").GetString("TYPE");
                            strReturnID = setOrderAPI.GetTable("RETURN").GetString("ID");
                            strReturnNumber = setOrderAPI.GetTable("RETURN").GetString("NUMBER");
                            strReturnMessage = setOrderAPI.GetTable("RETURN").GetString("MESSAGE");
                        }
                    }
                    strReturnType2 = setCommit.GetStructure("RETURN").GetString("TYPE");
                    strReturnID2 = setCommit.GetStructure("RETURN").GetString("ID");
                    strReturnNumber2 = setCommit.GetStructure("RETURN").GetString("NUMBER");
                    strReturnMessage2 = setCommit.GetStructure("RETURN").GetString("MESSAGE");

                    if (strVBELN.Length > 8)
                    {
                        lReturn = strVBELN;
                    }
                    else
                    {
                        lReturn = strReturnMessage;
                    }

                    lConnect.Dispose();
                    lSAP = null;
                    lConnect = null;
                }

                catch (RfcCommunicationException e)
                {
                    lReturn = e.Message;
                }
                catch (RfcLogonException e)
                {
                    // user could not logon...
                    lReturn = e.Message;
                }
                catch (RfcAbapRuntimeException e)
                {
                    // serious problem on ABAP system side...
                    lReturn = e.Message;
                }
                catch (RfcAbapBaseException e)
                {
                    lReturn = e.Message;
                    // The function module returned an ABAP exception, an ABAP message
                    // or an ABAP class-based exception...
                }
                catch (Exception e)
                {
                    lReturn = e.Message;
                    // Other type of exception
                }
            }

            lProcess = null;

            return lReturn;
        }
        public int CancelSAPSO(string pCustomerCode, string pProjectCode, int pJobID,
            string pPONo, string pBBSNo, string pSAPSO)
        {
            //var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            //string connectionString = ConfigurationManager.ConnectionStrings["YourConnectionStringName"].ConnectionString;
            //var configuration = builder.Build();

            // Access a setting
            //string settingValue = ConfigurationManager.AppSettings["DEV"]; // You can also provide a default value in case the setting is not foundstring settingValue = ConfigurationManager.AppSettings["SettingName"] ?? "DefaultValue";
            //var sapDestinationConnectionString = configuration.GetConnectionString("SAPDestination");
            int lReturn = 0;

            if (pSAPSO.Length > 0)
            {
                var lConnect = new SapConnection("DEV");
                lConnect.Open();

                var lSAP = lConnect.Destination;

                try
                {
                    IRfcFunction setCommit = lSAP.Repository.CreateFunction("BAPI_TRANSACTION_COMMIT");

                    //get order item number
                    IRfcFunction getOrderAPI = lSAP.Repository.CreateFunction("BAPI_SALESORDER_GETSTATUS");
                    getOrderAPI.SetValue("SALESDOCUMENT", pSAPSO);
                    RfcSessionManager.BeginContext(lSAP);
                    getOrderAPI.Invoke(lSAP);
                    setCommit.Invoke(lSAP);
                    RfcSessionManager.EndContext(lSAP);

                    IRfcTable lItemsTable = getOrderAPI.GetTable("STATUSINFO");

                    IRfcFunction setOrderAPI = lSAP.Repository.CreateFunction("YSDBAPI_SALESORDER_CHANGE");

                    setOrderAPI.SetValue("SALESDOCUMENT", pSAPSO);

                    IRfcStructure SAPStrucOrder_Header_In = setOrderAPI.GetStructure("ORDER_HEADER_IN");

                    //Change PO NUmber
                    SAPStrucOrder_Header_In.SetValue("PURCH_NO_C", pPONo + "-CXL");

                    IRfcStructure salesHeaderINX = setOrderAPI.GetStructure("ORDER_HEADER_INX");
                    salesHeaderINX.SetValue("UPDATEFLAG", "U");
                    salesHeaderINX.SetValue("PURCH_NO_C", "X");

                    //IRfcTable SAPTableOrder_Ext_In = setOrderAPI.GetTable("EXTENSIONIN");

                    //SAPTableOrder_Ext_In.Append();
                    //SAPTableOrder_Ext_In.SetValue("STRUCTURE", "BAPE_VBAK");
                    //SAPTableOrder_Ext_In.SetValue("VALUEPART1",
                    //pSAPSO +                                                        //SO (10)
                    //"00000000000000000" +                                           //YTOT_GRAND (17)
                    //"00000000000000000" +                                           //YTOT_CAB (17)
                    //"00000000000000000" +                                           //YTOT_MESH (17)
                    //"00000000000000000");                                           //YTOT_REBAR (17)
                    ////"                 " +                                           //YTOT_BPC (17)
                    ////"                 " +                                           //YTOT_PRECAGE (17)
                    ////"                 " +                                           //YTOTAL_WR (17)
                    ////"                 " +                                           //YTOTAL_PCSTRAND (17)
                    ////"                " +                                            //YTOT_PCSTRVAL (16)
                    ////"                 " +                                           //YTOT_COLD_ROLL (17)
                    ////"                 " +                                           //YTOT_PRE_CUTWR (17)
                    ////"                " +                                            //YTOTAL_VALUE (16)
                    ////"      " +                                                      //YTOT_COUPLER (6)
                    ////"                " +                                            //YTOT_COUPVAL (16)
                    ////" " +
                    ////" " +
                    ////" " +
                    ////" " +
                    ////" " +
                    ////" ");

                    ////SAPTableOrder_Ext_In.SetValue("VALUEPART2",
                    ////    " " + //pVBAKExt.YPCSTRAND +
                    ////    " " + //pVBAKExt.YMAT_SOURCE +
                    ////    " " +                                       //YCAB_TYPE
                    ////    " " +                                       //YORD_TYPE
                    ////    "                                        " +    //YWBS1 (40)
                    ////    " " + //pVBAKExt.YCOLD_ROLL_WIRE +
                    ////    " " + //pVBAKExt.YPRE_CUT_WIRE +
                    ////    "                 " +                       //YTOT_BILLET(17)
                    ////    " " +                                       //YBILLET_IND
                    ////    " " +                                       //YCON_TYP
                    ////    "                 " +                       //YTOT_CAR(17)
                    ////    " " + //pVBAKExt.YCARPET_IND +
                    ////    " "); //pVBAKExt.YCSURCHARGE_IND);

                    //SAPTableOrder_Ext_In.Append();
                    //SAPTableOrder_Ext_In.SetValue("STRUCTURE", "BAPE_VBAKX");
                    //SAPTableOrder_Ext_In.SetValue("VALUEPART1", pSAPSO + "XXXX");
                    //    //" " +       //YTOT_BPC
                    //    //" " +       //YTOT_PRECAGE
                    //    //" " +       //YTOTAL_WR
                    //    //" " +       //YTOTAL_PCSTRAND
                    //    //" " +       //YTOT_PCSTRVAL
                    //    //" " +       //YTOT_COLD_ROLL
                    //    //" " +       //YTOT_PRE_CUTWR
                    //    //" " +       //YTOTAL_VALUE
                    //    //" " +       //YTOT_COUPLER
                    //    //" " +       //YTOT_COUPVAL
                    //    //" " +       //YWIREROD_IND
                    //    //" " +       //YREBAR_IND
                    //    //" " +       //YCAB_IND
                    //    //" " +       //YMESH_IND
                    //    //" " +       //YPRECAGE
                    //    //" " +       //YBPC_IND
                    //    //" " +       //YPCSTRAND
                    //    //" " +       //YMAT_SOURCE
                    //    //" " +       //YCAB_TYPE
                    //    //" " +       //YORD_TYPE
                    //    //" " +       //YWBS1
                    //    //" " +       //YCOLD_ROLL_WIRE
                    //    //" " +       //YPRE_CUT_WIRE
                    //    //" " +       //YTOT_BILLET
                    //    //" " +       //YBILLET_IND
                    //    //" " +       //YCON_TYP
                    //    //" " +       //YTOT_CAR
                    //    //" " +       //YCARPET_IND
                    //    //" "         //YCSURCHARGE_IND
                    //    //);


                    IRfcTable salesItems = setOrderAPI.GetTable("ORDER_ITEM_IN");
                    IRfcTable salesItemsINX = setOrderAPI.GetTable("ORDER_ITEM_INX");
                    IRfcTable lTextBBSNo = setOrderAPI.GetTable("ORDER_TEXT");

                    //IRfcTable Schedule_In = setOrderAPI.GetTable("SCHEDULE_LINES");
                    //IRfcTable Schedule_InX = setOrderAPI.GetTable("SCHEDULE_LINESX");

                    lReturn = lItemsTable.RowCount;

                    for (lItemsTable.CurrentIndex = 0; lItemsTable.CurrentIndex < lItemsTable.RowCount; ++(lItemsTable.CurrentIndex))
                    {
                        //Changed to Rejected order with reason 00 -- Assigned by the System (internal)
                        salesItems.Append();
                        salesItems.SetValue("ITM_NUMBER", lItemsTable.GetString("ITM_NUMBER"));
                        salesItems.SetValue("REASON_REJ", "00");

                        salesItemsINX.Append();
                        salesItemsINX.SetValue("UPDATEFLAG", "U");
                        salesItemsINX.SetValue("ITM_NUMBER", lItemsTable.GetString("ITM_NUMBER"));
                        salesItemsINX.SetValue("REASON_REJ", "X");

                        //Change BBS Number
                        lTextBBSNo.Append();
                        lTextBBSNo.SetValue("DOC_NUMBER", pSAPSO);
                        lTextBBSNo.SetValue("TEXT_ID", "Z106");
                        lTextBBSNo.SetValue("ITM_NUMBER", lItemsTable.GetString("ITM_NUMBER"));
                        lTextBBSNo.SetValue("LANGU", "EN");
                        lTextBBSNo.SetValue("TEXT_LINE", pBBSNo + "-CXL");

                        //Schedule_In.Append();
                        //Schedule_In.SetValue("ITM_NUMBER", lItemsTable.GetString("ITM_NUMBER"));
                        //Schedule_In.SetValue("SCHED_LINE", 1);
                        //Schedule_In.SetValue("REQ_QTY", 0);

                        //Schedule_InX.Append();
                        //Schedule_InX.SetValue("UPDATEFLAG", "U");
                        //Schedule_InX.SetValue("ITM_NUMBER", lItemsTable.GetString("ITM_NUMBER"));
                        //Schedule_InX.SetValue("SCHED_LINE", 1);
                        //Schedule_InX.SetValue("REQ_QTY", "X");

                        // exit if reach end of records as "for" statement error by increasing the currentIndex at end.
                        if (lItemsTable.CurrentIndex >= lItemsTable.RowCount - 1)
                        {
                            break;
                        }
                    }


                    RfcSessionManager.BeginContext(lSAP);
                    setOrderAPI.Invoke(lSAP);
                    setCommit.Invoke(lSAP);
                    RfcSessionManager.EndContext(lSAP);

                    String strReturnType;
                    String strReturnID;
                    String strReturnNumber;
                    String strReturnMessage;
                    String strReturnType2;
                    String strReturnID2;
                    String strReturnNumber2;
                    String strReturnMessage2;

                    IRfcTable SAPReturn = setOrderAPI.GetTable("RETURN");
                    if (SAPReturn.RowCount > 0)
                    {
                        strReturnType = SAPReturn[0].GetString("TYPE");
                        strReturnID = SAPReturn[0].GetString("ID");
                        strReturnNumber = SAPReturn[0].GetString("NUMBER");
                        strReturnMessage = SAPReturn[0].GetString("MESSAGE");
                    }
                    else
                    {
                        strReturnType = setOrderAPI.GetTable("RETURN").GetString("TYPE");
                        strReturnID = setOrderAPI.GetTable("RETURN").GetString("ID");
                        strReturnNumber = setOrderAPI.GetTable("RETURN").GetString("NUMBER");
                        strReturnMessage = setOrderAPI.GetTable("RETURN").GetString("MESSAGE");
                    }
                    strReturnType2 = setCommit.GetStructure("RETURN").GetString("TYPE");
                    strReturnID2 = setCommit.GetStructure("RETURN").GetString("ID");
                    strReturnNumber2 = setCommit.GetStructure("RETURN").GetString("NUMBER");
                    strReturnMessage2 = setCommit.GetStructure("RETURN").GetString("MESSAGE");

                    if (strReturnType == "E")
                    {
                        lReturn = -2;
                    }
                }

                catch (RfcCommunicationException e)
                {
                    lReturn = -1;
                }
                catch (RfcLogonException e)
                {
                    // user could not logon...
                    lReturn = -1;
                }
                catch (RfcAbapRuntimeException e)
                {
                    // serious problem on ABAP system side...
                    lReturn = -1;
                }
                catch (RfcAbapBaseException e)
                {
                    lReturn = -1;
                    // The function module returned an ABAP exception, an ABAP message
                    // or an ABAP class-based exception...
                }
                catch (Exception e)
                {
                    lReturn = -1;
                    // Other type of exception
                }
                finally
                {
                    lConnect.Dispose();
                    lSAP = null;
                    lConnect = null;
                }

                if (lReturn >= 0)
                {
                    System.Threading.Thread.Sleep(2000);
                }
            }
            return lReturn;
        }

        public int ChangeSAPSO(string pCustomerCode, string pProjectCode, int pJobID,
            string pPONo, string pBBSNo, string pSAPSO, string pReqDate, string pInRemarks, string pExRemarks, string pInvRemarks,
            int ChReqDate, int ChPONumber, int ChBBSNo, int ChIntRemakrs, int ChExtRemakrs, int ChInvRemakrs)
        {
            int lReturn = 0;

            if (pSAPSO.Length > 0 && (ChPONumber == 1 || ChReqDate == 1 || ChBBSNo == 1 || ChIntRemakrs == 1 || ChExtRemakrs == 1 || ChInvRemakrs == 1))
            {
                var lConnect = new SapConnection("DEV");
                lConnect.Open();

                var lSAP = lConnect.Destination;

                try
                {
                    IRfcFunction setCommit = lSAP.Repository.CreateFunction("BAPI_TRANSACTION_COMMIT");

                    //get order item number
                    IRfcFunction getOrderAPI = lSAP.Repository.CreateFunction("BAPI_SALESORDER_GETSTATUS");
                    getOrderAPI.SetValue("SALESDOCUMENT", pSAPSO);
                    RfcSessionManager.BeginContext(lSAP);
                    getOrderAPI.Invoke(lSAP);
                    setCommit.Invoke(lSAP);
                    RfcSessionManager.EndContext(lSAP);

                    IRfcTable lItemsTable = getOrderAPI.GetTable("STATUSINFO");

                    IRfcFunction setOrderAPI = lSAP.Repository.CreateFunction("BAPI_SALESORDER_CHANGE");

                    setOrderAPI.SetValue("SALESDOCUMENT", pSAPSO);

                    IRfcStructure SAPStrucOrder_Header_In = setOrderAPI.GetStructure("ORDER_HEADER_IN");
                    IRfcStructure salesHeaderINX = setOrderAPI.GetStructure("ORDER_HEADER_INX");
                    salesHeaderINX.SetValue("UPDATEFLAG", "U");

                    //Change PO NUmber
                    if (ChPONumber == 1)
                    {
                        SAPStrucOrder_Header_In.SetValue("PURCH_NO_C", pPONo);
                        salesHeaderINX.SetValue("PURCH_NO_C", "X");
                    }

                    //Change Required date

                    if (ChReqDate == 1)
                    {
                        SAPStrucOrder_Header_In.SetValue("REQ_DATE_H", pReqDate);
                        //Change Required date
                        salesHeaderINX.SetValue("REQ_DATE_H", "X");
                    }

                    // Internal/External Remarks
                    if (ChIntRemakrs == 1 || ChExtRemakrs == 1 || ChInvRemakrs == 1)
                    {
                        IRfcTable lRemarks = setOrderAPI.GetTable("ORDER_TEXT");

                        if (ChIntRemakrs == 1)
                        {
                            lRemarks.Append();
                            lRemarks.SetValue("TEXT_ID", "Z010");
                            lRemarks.SetValue("LANGU", "EN");
                            lRemarks.SetValue("TEXT_LINE", pInRemarks);
                        }

                        if (ChExtRemakrs == 1)
                        {
                            lRemarks.Append();
                            lRemarks.SetValue("TEXT_ID", "Z011");
                            lRemarks.SetValue("LANGU", "EN");
                            lRemarks.SetValue("TEXT_LINE", pExRemarks);
                        }

                        if (ChInvRemakrs == 1)
                        {
                            lRemarks.Append();
                            lRemarks.SetValue("TEXT_ID", "Z013");
                            lRemarks.SetValue("LANGU", "EN");
                            lRemarks.SetValue("TEXT_LINE", pInvRemarks);
                        }
                    }

                    if (ChBBSNo == 1)
                    {
                        IRfcTable salesItems = setOrderAPI.GetTable("ORDER_ITEM_IN");
                        IRfcTable salesItemsINX = setOrderAPI.GetTable("ORDER_ITEM_INX");
                        IRfcTable lTextBBSNo = setOrderAPI.GetTable("ORDER_TEXT");

                        //IRfcTable Schedule_In = setOrderAPI.GetTable("SCHEDULE_LINES");
                        //IRfcTable Schedule_InX = setOrderAPI.GetTable("SCHEDULE_LINESX");

                        lReturn = lItemsTable.RowCount;

                        for (lItemsTable.CurrentIndex = 0; lItemsTable.CurrentIndex < lItemsTable.RowCount; ++(lItemsTable.CurrentIndex))
                        {
                            //Changed to Rejected order with reason 00 -- Assigned by the System (internal)
                            salesItems.Append();
                            salesItems.SetValue("ITM_NUMBER", lItemsTable.GetString("ITM_NUMBER"));

                            salesItemsINX.Append();
                            salesItemsINX.SetValue("UPDATEFLAG", "U");
                            salesItemsINX.SetValue("ITM_NUMBER", lItemsTable.GetString("ITM_NUMBER"));

                            //Change BBS Number
                            lTextBBSNo.Append();
                            lTextBBSNo.SetValue("DOC_NUMBER", pSAPSO);
                            lTextBBSNo.SetValue("TEXT_ID", "Z106");
                            lTextBBSNo.SetValue("ITM_NUMBER", lItemsTable.GetString("ITM_NUMBER"));
                            lTextBBSNo.SetValue("LANGU", "EN");
                            lTextBBSNo.SetValue("TEXT_LINE", pBBSNo);

                            //Schedule_In.Append();
                            //Schedule_In.SetValue("ITM_NUMBER", lItemsTable.GetString("ITM_NUMBER"));
                            //Schedule_In.SetValue("SCHED_LINE", 1);
                            //Schedule_In.SetValue("REQ_QTY", 0);

                            //Schedule_InX.Append();
                            //Schedule_InX.SetValue("UPDATEFLAG", "U");
                            //Schedule_InX.SetValue("ITM_NUMBER", lItemsTable.GetString("ITM_NUMBER"));
                            //Schedule_InX.SetValue("SCHED_LINE", 1);
                            //Schedule_InX.SetValue("REQ_QTY", "X");

                            // exit if reach end of records as "for" statement error by increasing the currentIndex at end.
                            if (lItemsTable.CurrentIndex >= lItemsTable.RowCount - 1)
                            {
                                break;
                            }
                        }
                    }

                    RfcSessionManager.BeginContext(lSAP);
                    setOrderAPI.Invoke(lSAP);
                    setCommit.Invoke(lSAP);
                    RfcSessionManager.EndContext(lSAP);

                    String strReturnType;
                    String strReturnID;
                    String strReturnNumber;
                    String strReturnMessage;
                    String strReturnType2;
                    String strReturnID2;
                    String strReturnNumber2;
                    String strReturnMessage2;

                    IRfcTable SAPReturn = setOrderAPI.GetTable("RETURN");
                    if (SAPReturn.RowCount > 0)
                    {
                        strReturnType = SAPReturn[0].GetString("TYPE");
                        strReturnID = SAPReturn[0].GetString("ID");
                        strReturnNumber = SAPReturn[0].GetString("NUMBER");
                        strReturnMessage = SAPReturn[0].GetString("MESSAGE");
                    }
                    else
                    {
                        strReturnType = setOrderAPI.GetTable("RETURN").GetString("TYPE");
                        strReturnID = setOrderAPI.GetTable("RETURN").GetString("ID");
                        strReturnNumber = setOrderAPI.GetTable("RETURN").GetString("NUMBER");
                        strReturnMessage = setOrderAPI.GetTable("RETURN").GetString("MESSAGE");
                    }
                    strReturnType2 = setCommit.GetStructure("RETURN").GetString("TYPE");
                    strReturnID2 = setCommit.GetStructure("RETURN").GetString("ID");
                    strReturnNumber2 = setCommit.GetStructure("RETURN").GetString("NUMBER");
                    strReturnMessage2 = setCommit.GetStructure("RETURN").GetString("MESSAGE");

                    if (strReturnType == "E")
                    {
                        lReturn = -2;
                    }
                }

                catch (RfcCommunicationException e)
                {
                    lReturn = -1;
                }
                catch (RfcLogonException e)
                {
                    // user could not logon...
                    lReturn = -1;
                }
                catch (RfcAbapRuntimeException e)
                {
                    // serious problem on ABAP system side...
                    lReturn = -1;
                }
                catch (RfcAbapBaseException e)
                {
                    lReturn = -1;
                    // The function module returned an ABAP exception, an ABAP message
                    // or an ABAP class-based exception...
                }
                catch (Exception e)
                {
                    lReturn = -1;
                    // Other type of exception
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
