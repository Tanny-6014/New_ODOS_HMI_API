
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Xml;
using Tacton.Configurator.Interfaces;
using Tacton.Configurator.ObjectModel;
using Tacton.Configurator.Public;
using DrainService.Repositories;
using DrainService.Constants;


namespace DrainService.Repositories
{
    public class BendingAdjustmentBLL
    {
        DataAccessor accessor = null;
        NameValueCollection properties = null;
        WebConfiguration configuration = null;

        public BOMInfo objBOMInfo = new BOMInfo();

        public DetailInfo objDetailInfo = new DetailInfo();

       // private string _serverNameString = "//127.0.0.1:9090/NDSBendingCheck";

        #region Variables
        private string strBending_Check_type = "2M3";
        private string strBending_Check_mw_dia = "10";
        private string strBending_Check_cw_dia = "10";
        private string strBending_Check_mw_spacing = "100";
        private string strBending_Check_cw_spacing = "100";
        private string strBending_Check_mo1 = "50";
        private string strBending_Check_mo2 = "50";
        private string strBending_Check_co1 = "100";
        private string strBending_Check_co2 = "100";
        private string strMWLength = "2000";
        private string strCWLength = "2000";
        private string strMW = "18";
        private string strCW = "19";

        private string strBending_Check_minMO = "20";
        private string strBending_Check_maxMO = "1000";
        private string strBending_Check_minCO = "10";
        private string strBending_Check_maxCO = "1000";
        private string strTotalSpace = "200";
        private string strMinMWSpace = "50";
        private string strMaxMWSpace = "1200";
        private string strMinCWSpace = "50";
        private string strMaxCWSpace = "1200";
        private int intSegmentCount = 3;
        private int intBendLengthCount = 2;
        private string[] strSegmentNames = new string[] { "a", "b", "c", "d", "e", "f", "g", "h" };
        private string[] strSegmentValues = new string[8]; // { "450", "1000", "450" };
        private string[] strBendLengths = new string[4]; // { "25", "50"};
        private char chrMOCO = 'M';
        private int intNoOfBends = 2;
        private string[] strSegmentBCValue = new string[10];
        private string[] str_no_of_wire = new string[10];
        #endregion

        private readonly IConfiguration _configuration;
        private string connectionString;

        public BendingAdjustmentBLL(IConfiguration configuration)
        {
           
            _configuration = configuration;

            connectionString = _configuration.GetConnectionString(SystemConstants.DefaultDBConnection);
        }

        public BendingAdjustmentBLL()
        {
        }

        //private void StartConfiguration(string serverName)
        //{
        //    _serverNameString = serverName;
        //}

        private string LoadConfiguration(string strPath, WebConfiguration refconfiguration)
        {
            string returnSafeState = "";
            try
            {
                //accessor = new FileAccessor();
                properties = new NameValueCollection();
                //properties.Add("Tacton.Configurator.remoteServers", "true");
                //properties.Add("Tacton.Configurator.servers", _serverNameString);

                //factory = new Factory(accessor, properties);

                // Create and use a configuration object
                //configuration = new WebConfiguration();
                configuration = refconfiguration;

                configuration.Instance.Filter = "groups;parameters;report";

               
                configuration.Instance.StartConfiguration(strPath, null);

                returnSafeState = configuration.Instance.SafeState;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnSafeState;
        }

        public bool BendingCheck(int productMarkID)
        {
            //string FinalOutput = "";
            //string bendCheckStatus = null;

            //bool bolResult = false;
            //try
            //{
            //    Result resultB;
            //    if ((strBending_Check_type != "F" & strBending_Check_type != "FR1"))
            //    {
            //        //LoadConfiguration(@"c:\inetpub\wwwroot\NDS\SlabWall\Data\Bending_Check_2BC1.tcx");
            //        string strShapeShiftTCXfilename = @"c:\inetpub\wwwroot\NDS\SlabWall\Data\Bending_Check_2BC1.tcx";

            //        //bolResult = CommitBendingCheck(strShapeShiftTCXfilename);

            //        //LoadConfiguration(@"c:\inetpub\wwwroot\NDS\SlabWall\Data\Bending_Check_overhang_2BC1.tcx");
            //        strShapeShiftTCXfilename = @"c:\inetpub\wwwroot\NDS\SlabWall\Data\Bending_Check_overhang_2BC1.tcx";
            //        string strMOH1, strMOH2, strCOH1, strCOH2;
            //        if (!bolResult) bolResult = CommitOverHang(strShapeShiftTCXfilename, out strMOH1, out strMOH2, out strCOH1, out strCOH2);
            //        //LoadConfiguration(@"c:\inetpub\wwwroot\NDS\SlabWall\Data\Bending_Check_space_shift_2BC1_normal.tcx");
            //        strShapeShiftTCXfilename = @"c:\inetpub\wwwroot\NDS\SlabWall\Data\Bending_Check_space_shift_2BC1_normal.tcx";
            //        string ParametersForCWSpace, ParametersForMWSpace;
            //        if (!bolResult) bolResult = CommitSpaceShift(strShapeShiftTCXfilename, out strMOH1, out strMOH2, out strCOH1, out strCOH2, out ParametersForCWSpace, out ParametersForMWSpace);

            //        //LoadConfiguration(@"c:\inetpub\wwwroot\NDS\SlabWall\Data\Bending_Check_space_shift_2BC1_normal.tcx");
            //        strShapeShiftTCXfilename = @"c:\inetpub\wwwroot\NDS\SlabWall\Data\Bending_Check_remove_wire_method_2BC1.tcx";
            //        ParametersForCWSpace = "";
            //        ParametersForMWSpace = "";                    
            //        if (!bolResult) bolResult = CommitWireRemove(strShapeShiftTCXfilename, out ParametersForCWSpace, out ParametersForMWSpace);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ErrorHandler.RaiseError(ex, strLogError, string.Empty);
            //}

            return true;
        }

        public void GetBendingCheckValues(int intProductMarkingID, BOMInfo objBOMInfo, DetailInfo objDetailInfo, int intShapeid, string strStructureElementType)
        {
            BOMDal objBOMDal = new();
            try
            {
                DataSet dsBendingCheck;
                objBOMInfo.ProductMarkingId = intProductMarkingID;
                objBOMInfo.ShapeId = intShapeid;
                objBOMInfo.StructureElement = strStructureElementType;
                dsBendingCheck = objBOMDal.GetBendingCheck(objBOMInfo);

                if (dsBendingCheck.Tables.Count > 0)
                {
                    if (dsBendingCheck.Tables[0].Rows.Count > 0)
                    {
                        strBending_Check_mw_dia = dsBendingCheck.Tables[0].Rows[0]["intMWDia"].ToString();
                        strBending_Check_cw_dia = dsBendingCheck.Tables[0].Rows[0]["intCWDia"].ToString();
                        strBending_Check_mw_spacing = dsBendingCheck.Tables[0].Rows[0]["intMWSpace"].ToString();
                        if (strBending_Check_mw_spacing == "")
                            objDetailInfo.intMWSpacing = 0;
                        else
                            objDetailInfo.intMWSpacing = Convert.ToInt32(strBending_Check_mw_spacing);

                        strBending_Check_cw_spacing = dsBendingCheck.Tables[0].Rows[0]["intCWSpace"].ToString();
                        if (strBending_Check_cw_spacing == "")
                            objDetailInfo.intCWSpacing = 0;
                        else
                            objDetailInfo.intCWSpacing = Convert.ToInt32(strBending_Check_cw_spacing);

                        strMW = dsBendingCheck.Tables[0].Rows[0]["MW"].ToString();
                        if (strMW == "")
                            objDetailInfo.intInvoiceMWQty = 0;
                        else
                            objDetailInfo.intInvoiceMWQty = Convert.ToInt32(strMW);

                        strCW = dsBendingCheck.Tables[0].Rows[0]["CW"].ToString();
                        if (strCW == "")
                            objDetailInfo.intInvoiceCWQty = 0;
                        else
                            objDetailInfo.intInvoiceCWQty = Convert.ToInt32(strCW);
                    }
                    if (dsBendingCheck.Tables[1].Rows.Count > 0)
                    {
                        strBending_Check_type = dsBendingCheck.Tables[1].Rows[0]["chrShapeCode"].ToString().Trim();
                        objBOMInfo.Shape = strBending_Check_type;
                        strBending_Check_mo1 = dsBendingCheck.Tables[1].Rows[0]["intProductionMO1"].ToString();
                        strBending_Check_mo2 = dsBendingCheck.Tables[1].Rows[0]["intProductionMO2"].ToString();
                        strBending_Check_co1 = dsBendingCheck.Tables[1].Rows[0]["intProductionCO1"].ToString();
                        strBending_Check_co2 = dsBendingCheck.Tables[1].Rows[0]["intProductionCO2"].ToString();
                        strBending_Check_minMO = dsBendingCheck.Tables[1].Rows[0]["intMinMO1"].ToString();
                        strBending_Check_maxMO = dsBendingCheck.Tables[1].Rows[0]["intMaxMO2"].ToString();
                        strBending_Check_minCO = dsBendingCheck.Tables[1].Rows[0]["intMinCO1"].ToString();
                        strBending_Check_maxCO = dsBendingCheck.Tables[1].Rows[0]["intMaxCO2"].ToString();
                        strMWLength = dsBendingCheck.Tables[1].Rows[0]["MWLength"].ToString();
                        objDetailInfo.decMWLength = Convert.ToDecimal(strMWLength);
                        strCWLength = dsBendingCheck.Tables[1].Rows[0]["CWLength"].ToString();
                        objDetailInfo.decCWLength = Convert.ToDecimal(strCWLength);
                    }
                    if (dsBendingCheck.Tables[4].Rows.Count > 0)
                    {
                        strTotalSpace = dsBendingCheck.Tables[4].Rows[0]["sitTotalSpace"].ToString();
                        objBOMInfo.TotalSpace = strTotalSpace;
                    }

                    if (dsBendingCheck.Tables[5].Rows.Count > 0)
                    {
                        strMinMWSpace = dsBendingCheck.Tables[5].Rows[0]["intMinMWSpace"].ToString();
                        strMaxMWSpace = dsBendingCheck.Tables[5].Rows[0]["intMaxMWSpace"].ToString();
                        strMinCWSpace = dsBendingCheck.Tables[5].Rows[0]["intMinCWSpace"].ToString();
                        strMaxCWSpace = dsBendingCheck.Tables[5].Rows[0]["intMaxCWSpace"].ToString();
                    }
                    //Get Shape Code and its Shape Group Code 
                    //Get Shape Group information like Segment count, Bend Length count and type 
                    if (dsBendingCheck.Tables[6].Rows.Count > 0)
                    {
                        intSegmentCount = (dsBendingCheck.Tables[6].Rows[0].IsNull("intNoOfSegments") ? 0 : Convert.ToInt32(dsBendingCheck.Tables[6].Rows[0]["intNoOfSegments"]));
                        intBendLengthCount = (dsBendingCheck.Tables[6].Rows[0].IsNull("intBendLengthCount") ? 0 : Convert.ToInt32(dsBendingCheck.Tables[6].Rows[0]["intBendLengthCount"]));
                        objDetailInfo.intTotalBend = intBendLengthCount;
                        intNoOfBends = (dsBendingCheck.Tables[6].Rows[0].IsNull("sitNoOfBends") ? 0 : Convert.ToInt32(dsBendingCheck.Tables[6].Rows[0]["sitNoOfBends"]));
                        if (dsBendingCheck.Tables[6].Rows[0]["chrMOCO"] != null)
                            chrMOCO = Convert.ToChar(dsBendingCheck.Tables[6].Rows[0]["chrMOCO"].ToString().Trim());

                        objBOMInfo.OverHang = chrMOCO.ToString();
                    }

                    if (dsBendingCheck.Tables[2].Rows.Count > 0)
                    {
                        for (int i = 0; i <= intSegmentCount - 1; i++)
                        {
                            strSegmentValues[i] = dsBendingCheck.Tables[2].Rows[i]["intSegmentValue"].ToString();
                        }
                    }

                    if (dsBendingCheck.Tables[3].Rows.Count > 0)
                    {
                        for (int i = 0; i <= intBendLengthCount - 1; i++)
                        {
                            strBendLengths[i] = dsBendingCheck.Tables[3].Rows[i]["sitBendLength"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CommitBendingCheck(string tcxFilename, WebConfiguration ConfigBC)
        {
            Result tactonBendingCheck = null;
            string strBendType = "";
            string strMOCO = "";
            Hashtable slpResult = null;
            string resultString = "";
            bool bolResult = false;
            try
            {
                LoadConfiguration(tcxFilename, ConfigBC);
                configuration.Instance.SetStep("bending_check_step");

                configuration.Instance.UnCommit("bending_check_type_field");
                configuration.Instance.UnCommit("bending_check_mw_dia_field");
                configuration.Instance.UnCommit("bending_check_cw_dia_field");
                configuration.Instance.UnCommit("bending_check_mw_spacing_field");
                configuration.Instance.UnCommit("bending_check_cw_spacing_field");
                configuration.Instance.UnCommit("bending_check_mo1_field");
                configuration.Instance.UnCommit("bending_check_mo2_field");
                configuration.Instance.UnCommit("bending_check_co1_field");
                configuration.Instance.UnCommit("bending_check_co2_field");
                configuration.Instance.UnCommit("bending_check_minmo_field");
                configuration.Instance.UnCommit("bending_check_maxmo_field");
                configuration.Instance.UnCommit("bending_check_minco_field");
                configuration.Instance.UnCommit("bending_check_maxco_field");

                tactonBendingCheck = TactonCommit("bending_check_type_field", strBending_Check_type);
                tactonBendingCheck = TactonCommit("bending_check_mw_dia_field", strBending_Check_mw_dia);
                tactonBendingCheck = TactonCommit("bending_check_cw_dia_field", strBending_Check_cw_dia);
                tactonBendingCheck = TactonCommit("bending_check_mw_spacing_field", strBending_Check_mw_spacing);
                tactonBendingCheck = TactonCommit("bending_check_cw_spacing_field", strBending_Check_cw_spacing);
                tactonBendingCheck = TactonCommit("bending_check_mo1_field", strBending_Check_mo1);
                tactonBendingCheck = TactonCommit("bending_check_mo2_field", strBending_Check_mo2);
                tactonBendingCheck = TactonCommit("bending_check_co1_field", strBending_Check_co1);
                tactonBendingCheck = TactonCommit("bending_check_co2_field", strBending_Check_co2);
                tactonBendingCheck = TactonCommit("bending_check_minmo_field", strBending_Check_minMO);
                tactonBendingCheck = TactonCommit("bending_check_maxmo_field", strBending_Check_maxMO);
                tactonBendingCheck = TactonCommit("bending_check_minco_field", strBending_Check_minCO);
                tactonBendingCheck = TactonCommit("bending_check_maxco_field", strBending_Check_maxCO);

                if (intBendLengthCount == 4)
                {
                    strBendType = "reverse";
                }
                else
                {
                    strBendType = "normal";
                }

                if (chrMOCO == 'M')
                {
                    strMOCO = "mo";
                }
                else if (chrMOCO == 'C')
                {
                    strMOCO = "co";
                }

                for (int i = 1; i <= intSegmentCount; i++)
                {
                    configuration.Instance.UnCommit("bending_check_" + strSegmentNames[i - 1] + "_field");
                    tactonBendingCheck = TactonCommit("bending_check_" + strSegmentNames[i - 1] + "_field", strSegmentValues[i - 1]);
                    //CommitFail(tactonBendingCheck, "bending_check_" + strSegmentName(i - 1) + "_field = " + strBending_Check_segment(i - 1));
                }
                for (int i = 1; i <= intBendLengthCount; i++)
                {
                    configuration.Instance.UnCommit("bending_check_bend_length_" + strBendType + "_" + i.ToString() + "_field");
                    tactonBendingCheck = TactonCommit("bending_check_bend_length_" + strBendType + "_" + i.ToString() + "_field", strBendLengths[i - 1]);
                    //CommitFail(tactonBendingCheck, "bending_check_bend_length_" + i.ToString() + "_field = " + strBending_Check_bend_length(i - 1));
                }

                //Result
                Result resultB = configuration.Instance.Result;
                Group GroupResultB = (Group)resultB.RootGroup.SubGroups["result_group"];
                slpResult = new Hashtable();
                foreach (Parameter param in GroupResultB.Parameters.Values)
                {
                    slpResult.Add(param.Name, param.Value);
                }

                int intSegmentName = 0;
                intSegmentName = 0;
                int j = 0;
                j = 0;

                for (int i = 0; i <= intSegmentCount - 1; i++)
                {
                    strSegmentBCValue[j] = slpResult["bending_check_" + strSegmentNames[intSegmentName] + "1_" + strMOCO + "_post"].ToString();
                    j = j + 1;
                    if (i > 0 & i != intSegmentCount - 1)
                    {
                        strSegmentBCValue[j] = slpResult["bending_check_" + strSegmentNames[intSegmentName] + "2_" + strMOCO + "_post"].ToString();
                        j = j + 1;
                    }
                    intSegmentName = intSegmentName + 1;
                }
                for (int i = 1; i <= (intNoOfBends + 1); i++)
                {
                    str_no_of_wire[i] = slpResult["bending_check_" + i.ToString() + "_no_of_wire_" + strMOCO + "_post"].ToString();
                }

                if (strBendType == "normal")
                {
                    resultString = slpResult["bending_check_result_" + intNoOfBends.ToString() + "bend_status_" + strMOCO + "_post"].ToString();
                    if (resultString.ToUpper().Trim() == "PASS")
                    {
                        bolResult = true;
                        return true;
                    }
                }
                else
                {
                    resultB = configuration.Instance.SetStep("bending_check_reverse_result_step");
                    GroupResultB = (Group)resultB.RootGroup.SubGroups["result_group"];
                    slpResult = new Hashtable();
                    foreach (Parameter param in GroupResultB.Parameters.Values)
                    {
                        slpResult.Add(param.Name, param.Value);
                    }
                    resultString = slpResult["bending_check_result_" + intNoOfBends.ToString() + "bend_status_" + strMOCO + "_reverse_post"].ToString();
                    if (resultString.ToUpper().Trim() == "PASS")
                    {
                        bolResult = true;
                        return true;
                    }
                }

                if (resultString.ToUpper().Trim() == "PASS") bolResult = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //configuration.Factory.Shutdown();
                //configuration.Instance.Factory.Reset();
            }
            return bolResult;
        }


        //not used
        public bool CommitOverHang(string tcxFilename, out string strMOH1, out string strMOH2, out string strCOH1, out string strCOH2, WebConfiguration ConfigBC)
        {
            Result tactonOverHang;
            Hashtable slpResult = new Hashtable();
            string strMOCO = "";
            string strBendType = "";
            string resultString = "";
            bool bolResult = false;
            strMOH1 = "";
            strMOH2 = "";
            strCOH1 = "";
            strCOH2 = "";
            try
            {
                LoadConfiguration(tcxFilename, ConfigBC);

                configuration.Instance.UnCommit("bending_check_shape_code_field");
                configuration.Instance.UnCommit("bending_check_mw_wire_dia_field");
                configuration.Instance.UnCommit("bending_check_cw_wire_dia_field");
                configuration.Instance.UnCommit("bending_check_fb_a_field");
                configuration.Instance.UnCommit("bending_check_sb_b_field");
                configuration.Instance.UnCommit("bending_check_fb_b_field");
                configuration.Instance.UnCommit("bending_check_sb_c_field");
                configuration.Instance.UnCommit("bending_check_mo1_field");
                configuration.Instance.UnCommit("bending_check_mo2_field");
                configuration.Instance.UnCommit("bending_check_co1_field");
                configuration.Instance.UnCommit("bending_check_co2_field");

                tactonOverHang = TactonCommit("bending_check_shape_code_field", strBending_Check_type);
                tactonOverHang = TactonCommit("bending_check_mw_wire_dia_field", strBending_Check_mw_dia);
                tactonOverHang = TactonCommit("bending_check_cw_wire_dia_field", strBending_Check_cw_dia);
                //new code by gansi 
                string strFBSB = null;
                strFBSB = "sb";
                int x = 0;
                x = 0;
                for (int i = 1; i <= intSegmentCount; i++)
                {

                    if ((i / 2f) == Convert.ToInt32(i / 2))
                    {
                        strFBSB = "sb";
                    }
                    else
                    {
                        strFBSB = "fb";
                    }
                    if (i == intSegmentCount)
                    {
                        strFBSB = "sb";
                    }
                    configuration.Instance.UnCommit("bending_check_" + strFBSB + "_" + strSegmentNames[i - 1] + "_field");
                    tactonOverHang = TactonCommit("bending_check_" + strFBSB + "_" + strSegmentNames[i - 1] + "_field", strSegmentBCValue[x]);
                    //CommitFail(tactonOverHang, "bending_check_" + strFBSB + "_" + strSegmentName(i - 1) + "_field = " + strSegmentBCValue(x));
                    x = x + 1;
                    if (strFBSB == "sb")
                    {
                        strFBSB = "fb";
                    }
                    else
                    {
                        strFBSB = "sb";
                    }
                    if (i > 1 & i != intSegmentCount)
                    {
                        configuration.Instance.UnCommit("bending_check_" + strFBSB + "_" + strSegmentNames[i - 1] + "_field");
                        tactonOverHang = TactonCommit("bending_check_" + strFBSB + "_" + strSegmentNames[i - 1] + "_field", strSegmentBCValue[x]);
                        //CommitFail(tactonOverHang, "bending_check_" + strFBSB + "_" + strSegmentName(i - 1) + "_field = " + strSegmentBCValue(x));
                        x = x + 1;
                    }
                }

                if (chrMOCO == 'M')
                {
                    strMOCO = "mo";
                    tactonOverHang = TactonCommit("bending_check_" + strMOCO + "1_field", strBending_Check_mo1);
                    //CommitFail(tactonOverHang, "bending_check_" + strMOCO + "1_field = " + strBending_Check_mo1);

                    tactonOverHang = TactonCommit("bending_check_" + strMOCO + "2_field", strBending_Check_mo2);
                    //CommitFail(tactonOverHang, "bending_check_" + strMOCO + "2_field = " + strBending_Check_mo2);
                }
                else if (chrMOCO == 'C')
                {
                    strMOCO = "co";
                    tactonOverHang = TactonCommit("bending_check_" + strMOCO + "1_field", strBending_Check_co1);
                    //CommitFail(tactonOverHang, "bending_check_" + strMOCO + "1_field = " + strBending_Check_co1);

                    tactonOverHang = TactonCommit("bending_check_" + strMOCO + "2_field", strBending_Check_co2);
                    //CommitFail(tactonOverHang, "bending_check_" + strMOCO + "2_field = " + strBending_Check_co2);
                }

                if (intBendLengthCount == 4)
                {
                    strBendType = "reverse";
                }
                else
                {
                    strBendType = "normal";
                }
                //new code ends here - gansi 
                for (int i = 1; i <= intBendLengthCount; i++)
                {
                    configuration.Instance.UnCommit("bending_check_bend_length_" + strBendType + "_" + i.ToString() + "_field");
                    tactonOverHang = TactonCommit("bending_check_bend_length_" + strBendType + "_" + i.ToString() + "_field", strBendLengths[i - 1]);
                    //CommitFail(tactonOverHang, "bending_check_" + strBendType + "_" + i.ToString() + "_bendlength_field = " + strBending_Check_bend_length(i - 1));
                }

                tactonOverHang = TactonCommit("bending_check_minoh_field", strBending_Check_minMO);
                //CommitFail(tactonOverHang, "bending_check_minoh_field = " + strBending_Check_minMO);
                tactonOverHang = TactonCommit("bending_check_maxoh_field", strBending_Check_maxMO);
                //CommitFail(tactonOverHang, "bending_check_maxoh_field = " + strBending_Check_maxMO);

                TactonCommit("bending_check_" + intNoOfBends.ToString() + "bend_" + strBendType + "_shift_overhang_result_field", "Yes");
                Result resultOH1 = configuration.Instance.Result;
                Group GroupResultOH = (Group)resultOH1.RootGroup.SubGroups[2];
                foreach (Parameter param in GroupResultOH.Parameters.Values)
                {
                    slpResult.Add(param.Name, param.Value);
                }
                resultString = slpResult["bending_check_" + intNoOfBends.ToString() + "bend_" + strBendType + "_shift_overhang_result_field"].ToString();
                if (resultString.ToUpper().Trim() == "YES")
                {
                    bolResult = true;
                    if (chrMOCO == 'M')
                    {
                        strMOH1 = slpResult["bending_check_stepped_oh1_field"].ToString();
                        strMOH2 = slpResult["bending_check_stepped_oh2_field"].ToString();
                        strCOH1 = "0";
                        strCOH2 = "0";
                    }
                    else if (chrMOCO == 'C')
                    {
                        strMOH1 = "0";
                        strMOH2 = "0";
                        strCOH1 = slpResult["bending_check_stepped_oh1_field"].ToString();
                        strCOH2 = slpResult["bending_check_stepped_oh2_field"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
               
            }
            return bolResult;
        }

        //not used
        public bool CommitSpaceShift(string tcxFilename, out string strMOH1, out string strMOH2, out string strCOH1, out string strCOH2, out string ParametersForCWSpace, out string ParametersForMWSpace, WebConfiguration ConfigBC)
        {
            Result tactonSpaceShift;
            Hashtable slpResult = new Hashtable();
            string strBendType = "";
            string resultString = "";
            bool bolResult = false;
            strMOH1 = "";
            strMOH2 = "";
            strCOH1 = "";
            strCOH2 = "";
            ParametersForCWSpace = "";
            ParametersForMWSpace = "";
            BOMInfo objBOMInfo = new BOMInfo();
            try
            {
                LoadConfiguration(tcxFilename, ConfigBC);

                if (intBendLengthCount == 2)
                {
                    //Space shift for Normal tcx 


                    configuration.Instance.UnCommit("bending_check_type_field");
                    configuration.Instance.UnCommit("bending_check_cw_dia_field");
                    configuration.Instance.UnCommit("bending_check_mw_dia_field");
                    configuration.Instance.UnCommit("bending_check_mw_spacing_field");
                    configuration.Instance.UnCommit("bending_check_cw_spacing_field");

                    for (int i = 1; i <= (intNoOfBends + 1); i++)
                    {
                        configuration.Instance.UnCommit("bending_check_" + i.ToString() + "_no_of_wire_field");
                    }

                    configuration.Instance.UnCommit("bending_check_total_spacing_field");
                    configuration.Instance.UnCommit("bending_check_production_mo1_field");
                    configuration.Instance.UnCommit("bending_check_production_mo2_field");
                    configuration.Instance.UnCommit("bending_check_production_co1_field");
                    configuration.Instance.UnCommit("bending_check_production_co2_field");

                    tactonSpaceShift = TactonCommit("bending_check_type_field", strBending_Check_type);
                    //CommitFail(tactonSpaceShift, "bending_check_type_field = " + strBending_Check_type);
                    tactonSpaceShift = TactonCommit("bending_check_cw_dia_field", strBending_Check_cw_dia);
                    //CommitFail(tactonSpaceShift, "bending_check_cw_dia_field = " + strBending_Check_cw_dia);
                    tactonSpaceShift = TactonCommit("bending_check_mw_dia_field", strBending_Check_mw_dia);
                    //CommitFail(tactonSpaceShift, "bending_check_mw_dia_field = " + strBending_Check_mw_dia);
                    tactonSpaceShift = TactonCommit("bending_check_mw_spacing_field", strBending_Check_mw_spacing);
                    //CommitFail(tactonSpaceShift, "bending_check_mw_spacing_field = " + strBending_Check_mw_spacing);
                    tactonSpaceShift = TactonCommit("bending_check_cw_spacing_field", strBending_Check_cw_spacing);
                    //CommitFail(tactonSpaceShift, "bending_check_cw_spacing_field = " + strBending_Check_cw_spacing);

                    for (int i = 1; i <= (intNoOfBends + 1); i++)
                    {
                        tactonSpaceShift = TactonCommit("bending_check_" + i.ToString() + "_no_of_wire_field", str_no_of_wire[i]);
                        //CommitFail(tactonSpaceShift, "bending_check_" + i.ToString() + "_no_of_wire_field = " + str_no_of_wire(i));
                    }

                    tactonSpaceShift = TactonCommit("bending_check_total_spacing_field", strTotalSpace);
                    //CommitFail(tactonSpaceShift, "bending_check_total_spacing_field = " + strTotalSpace);
                    tactonSpaceShift = TactonCommit("bending_check_production_mo1_field", strBending_Check_mo1);
                    //CommitFail(tactonSpaceShift, "bending_check_production_mo1_field = " + strBending_Check_mo1);
                    tactonSpaceShift = TactonCommit("bending_check_production_mo2_field", strBending_Check_mo2);
                    //CommitFail(tactonSpaceShift, "bending_check_production_mo2_field = " + strBending_Check_mo2);
                    tactonSpaceShift = TactonCommit("bending_check_production_co1_field", strBending_Check_co1);
                    //CommitFail(tactonSpaceShift, "bending_check_production_co1_field = " + strBending_Check_co1);
                    tactonSpaceShift = TactonCommit("bending_check_production_co2_field", strBending_Check_co2);
                    //CommitFail(tactonSpaceShift, "bending_check_production_co2_field = " + strBending_Check_co2);


                    string strFBSB = null;
                    strFBSB = "sb";
                    int x = 0;
                    x = 0;
                    for (int i = 1; i <= intSegmentCount; i++)
                    {
                        if (i / 2f == Convert.ToInt32(i / 2))
                        {
                            strFBSB = "sb";
                        }
                        else
                        {
                            strFBSB = "fb";
                        }
                        if (i == intSegmentCount)
                        {
                            strFBSB = "sb";
                        }
                        configuration.Instance.UnCommit("bending_check_" + strFBSB + "_" + strSegmentNames[i - 1] + "_field");
                        tactonSpaceShift = TactonCommit("bending_check_" + strFBSB + "_" + strSegmentNames[i - 1] + "_field", strSegmentBCValue[x]);
                        //CommitFail(tactonSpaceShift, "bending_check_" + strFBSB + "_" + strSegmentName(i - 1) + "_field = " + strSegmentBCValue(x));
                        x = x + 1;
                        if (strFBSB == "sb")
                        {
                            strFBSB = "fb";
                        }
                        else
                        {
                            strFBSB = "sb";
                        }
                        if (i > 1 & i != intSegmentCount)
                        {
                            configuration.Instance.UnCommit("bending_check_" + strFBSB + "_" + strSegmentNames[i - 1] + "_field");
                            tactonSpaceShift = TactonCommit("bending_check_" + strFBSB + "_" + strSegmentNames[i - 1] + "_field", strSegmentBCValue[x]);
                            //CommitFail(tactonSpaceShift, "bending_check_" + strFBSB + "_" + strSegmentName(i - 1) + "_field = " + strSegmentBCValue(x));
                            x = x + 1;
                        }
                    }

                    strBendType = "normal";
                    for (int i = 1; i <= intBendLengthCount; i++)
                    {
                        configuration.Instance.UnCommit("bending_check_bend_length_" + strBendType + "_" + i.ToString() + "_field");
                        tactonSpaceShift = TactonCommit("bending_check_bend_length_" + strBendType + "_" + i.ToString() + "_field", strBendLengths[i - 1]);
                        //CommitFail(tactonSpaceShift, "bending_check_bend_length_" + strBendType + "_" + i.ToString() + "_field = " + strBending_Check_bend_length(i - 1));
                    }

                    //Normal 
                    Result resultSpace = configuration.Instance.SetStep("shift_step");
                    //Committing 
                    resultSpace = TactonCommit("bending_check_space_shift_method_status_" + intNoOfBends.ToString() + "bend_" + strBendType + "_field", "Yes");
                    //result 
                    //Result resultSpace1 = configuration.SetStep("shift_step");
                    Group GroupResultSpace = (Group)resultSpace.RootGroup.SubGroups["result_group"];
                    foreach (Parameter param in GroupResultSpace.Parameters.Values)
                    {
                        slpResult.Add(param.Name, param.Value);
                    }

                    TactonCommit("bending_check_spacing_string_1_bend_normal_post", slpResult["bending_check_spacing_string_1_bend_normal_post"].ToString());

                    //result 
                    GroupResultSpace = (Group)resultSpace.RootGroup.SubGroups["result_group"];
                    slpResult = new Hashtable();
                    foreach (Parameter param in GroupResultSpace.Parameters.Values)
                    {
                        slpResult.Add(param.Name, param.Value);
                    }

                    //result 
                    resultString = slpResult["bending_check_space_shift_method_status_" + intNoOfBends.ToString() + "bend_" + strBendType + "_field"].ToString();
                    if (resultString.ToUpper() == "YES")
                    {
                        bolResult = true;
                        if (chrMOCO == 'M')
                        {
                            ParametersForCWSpace = slpResult["bending_check_spacing_string_normal_post"].ToString();
                            ParametersForMWSpace = "0";
                            strMOH1 = slpResult["bending_check_final_oh1_field"].ToString();
                            strMOH2 = slpResult["bending_check_final_oh2_field"].ToString();
                            strCOH1 = "0";
                            strCOH2 = "0";

                        }
                        else if (chrMOCO == 'C')
                        {
                            ParametersForCWSpace = "0";
                            ParametersForMWSpace = slpResult["bending_check_spacing_string_normal_post"].ToString();
                            strMOH1 = "0";
                            strMOH2 = "0";
                            strCOH1 = slpResult["bending_check_final_oh1_field"].ToString();
                            strCOH2 = slpResult["bending_check_final_oh2_field"].ToString();
                        }
                    }
                }
                else
                {
                    //Space shift for reverse tcx 

                    configuration.Instance.UnCommit("bending_check_type_field");
                    configuration.Instance.UnCommit("bending_check_cw_dia_field");
                    configuration.Instance.UnCommit("bending_check_mw_dia_field");
                    configuration.Instance.UnCommit("bending_check_mw_spacing_field");
                    configuration.Instance.UnCommit("bending_check_cw_spacing_field");
                    for (int i = 1; i <= (intNoOfBends + 1); i++)
                    {
                        configuration.Instance.UnCommit("bending_check_" + i.ToString() + "_no_of_wire_field");
                    }
                    configuration.Instance.UnCommit("bending_check_total_spacing_field");
                    configuration.Instance.UnCommit("bending_check_production_mo1_field");
                    configuration.Instance.UnCommit("bending_check_production_mo2_field");
                    configuration.Instance.UnCommit("bending_check_production_co1_field");
                    configuration.Instance.UnCommit("bending_check_production_co2_field");

                    tactonSpaceShift = TactonCommit("bending_check_type_field", strBending_Check_type);
                    //CommitFail(tactonSpaceShift, "bending_check_type_field = " + strBending_Check_type);
                    tactonSpaceShift = TactonCommit("bending_check_cw_dia_field", strBending_Check_cw_dia);
                    //CommitFail(tactonSpaceShift, "bending_check_cw_dia_field = " + strBending_Check_cw_dia);
                    tactonSpaceShift = TactonCommit("bending_check_mw_dia_field", strBending_Check_mw_dia);
                    //CommitFail(tactonSpaceShift, "bending_check_mw_dia_field = " + strBending_Check_mw_dia);
                    tactonSpaceShift = TactonCommit("bending_check_mw_spacing_field", strBending_Check_mw_spacing);
                    //CommitFail(tactonSpaceShift, "bending_check_mw_spacing_field = " + strBending_Check_mw_spacing);
                    tactonSpaceShift = TactonCommit("bending_check_cw_spacing_field", strBending_Check_cw_spacing);
                    //CommitFail(tactonSpaceShift, "bending_check_cw_spacing_field = " + strBending_Check_cw_spacing);
                    tactonSpaceShift = TactonCommit("bending_check_total_spacing_field", strTotalSpace);
                    //CommitFail(tactonSpaceShift, "bending_check_total_spacing_field = " + strTotalSpace);
                    tactonSpaceShift = TactonCommit("bending_check_production_mo1_field", strBending_Check_mo1);
                    //CommitFail(tactonSpaceShift, "bending_check_production_mo1_field = " + strBending_Check_mo1);
                    tactonSpaceShift = TactonCommit("bending_check_production_mo2_field", strBending_Check_mo2);
                    //CommitFail(tactonSpaceShift, "bending_check_production_mo2_field = " + strBending_Check_mo2);
                    tactonSpaceShift = TactonCommit("bending_check_production_co1_field", strBending_Check_co1);
                    //CommitFail(tactonSpaceShift, "bending_check_production_co1_field = " + strBending_Check_co1);
                    tactonSpaceShift = TactonCommit("bending_check_production_co2_field", strBending_Check_co2);
                    //CommitFail(tactonSpaceShift, "bending_check_production_co2_field = " + strBending_Check_co2);

                    for (int i = 1; i <= (intNoOfBends + 1); i++)
                    {
                        tactonSpaceShift = TactonCommit("bending_check_" + i.ToString() + "_no_of_wire_field", str_no_of_wire[i]);
                        //CommitFail(tactonSpaceShift, "bending_check_" + i.ToString() + "_no_of_wire_field = " + str_no_of_wire(i));
                    }

                    string strFBSB = null;
                    strFBSB = "sb";
                    int x = 0;
                    x = 0;
                    for (int i = 1; i <= intSegmentCount; i++)
                    {
                        if (i / 2f == Convert.ToInt32(i / 2))
                        {
                            strFBSB = "sb";
                        }
                        else
                        {
                            strFBSB = "fb";
                        }
                        if (i == intSegmentCount)
                        {
                            strFBSB = "sb";
                        }
                        configuration.Instance.UnCommit("bending_check_" + strFBSB + "_" + strSegmentNames[i - 1] + "_field");
                        tactonSpaceShift = TactonCommit("bending_check_" + strFBSB + "_" + strSegmentNames[i - 1] + "_field", strSegmentBCValue[x]);
                        //CommitFail(tactonSpaceShift, "bending_check_" + strFBSB + "_" + strSegmentName(i - 1) + "_field = " + strSegmentBCValue(x));
                        x = x + 1;
                        if (strFBSB == "sb")
                        {
                            strFBSB = "fb";
                        }
                        else
                        {
                            strFBSB = "sb";
                        }
                        if (i > 1 & i != intSegmentCount)
                        {
                            configuration.Instance.UnCommit("bending_check_" + strFBSB + "_" + strSegmentNames[i - 1] + "_field");
                            tactonSpaceShift = TactonCommit("bending_check_" + strFBSB + "_" + strSegmentNames[i - 1] + "_field", strSegmentBCValue[x]);
                            //CommitFail(tactonSpaceShift, "bending_check_" + strFBSB + "_" + strSegmentName(i - 1) + "_field = " + strSegmentBCValue(x));
                            x = x + 1;
                        }
                    }

                    strBendType = "reverse";

                    for (int i = 1; i <= intBendLengthCount; i++)
                    {
                        configuration.Instance.UnCommit("bending_check_bend_length_" + strBendType + "_" + i.ToString() + "_field");
                        tactonSpaceShift = TactonCommit("bending_check_bend_length_" + strBendType + "_" + i.ToString() + "_field", strBendLengths[i - 1]);
                        //CommitFail(tactonSpaceShift, "bending_check_bend_length_" + strBendType + "_" + i.ToString() + "_field = " + strBending_Check_bend_length(i - 1));
                    }
                    //Reverse 
                    Result resultSpace = configuration.Instance.SetStep("input_Step");
                    //Committing 
                    resultSpace = TactonCommit("bending_check_space_shift_method_status_" + intNoOfBends.ToString() + "bend_" + strBendType + "_field", "Yes");
                    //result 
                    Result resultSpace1 = configuration.Instance.SetStep("shift_step");
                    Group GroupResultSpace = (Group)resultSpace1.RootGroup.SubGroups["result_group"];
                    foreach (Parameter param in GroupResultSpace.Parameters.Values)
                    {
                        slpResult.Add(param.Name, param.Value);
                    }

                    TactonCommit("bending_check_spacing_string_1_bend_reverse_post", slpResult["bending_check_spacing_string_1_bend_reverse_post"].ToString());

                    //Reverse 
                    //result 
                    GroupResultSpace = (Group)resultSpace1.RootGroup.SubGroups["result_group"];
                    slpResult = new Hashtable();
                    foreach (Parameter param in GroupResultSpace.Parameters.Values)
                    {
                        slpResult.Add(param.Name, param.Value);
                    }

                    //result 
                    resultString = slpResult["bending_check_space_shift_method_status_" + intNoOfBends.ToString() + "bend_" + strBendType + "_field"].ToString();
                    if (resultString.ToUpper() == "YES")
                    {
                        bolResult = true;
                        if (chrMOCO == 'M')
                        {
                            ParametersForCWSpace = slpResult["bending_check_spacing_string_reverse_post"].ToString();
                            ParametersForMWSpace = "0";
                            strMOH1 = slpResult["bending_check_final_oh1_field"].ToString();
                            strMOH2 = slpResult["bending_check_final_oh2_field"].ToString();
                            strCOH1 = "0";
                            strCOH2 = "0";
                        }
                        else if (chrMOCO == 'C')
                        {
                            ParametersForCWSpace = "0";
                            ParametersForMWSpace = slpResult["bending_check_spacing_string_reverse_post"].ToString();
                            strMOH1 = "0";
                            strMOH2 = "0";
                            strCOH1 = slpResult["bending_check_final_oh1_field"].ToString();
                            strCOH2 = slpResult["bending_check_final_oh2_field"].ToString();
                        }

                        objBOMInfo.ParametersForCWSpace = ParametersForCWSpace;
                        objBOMInfo.ParametersForMWSpace = ParametersForMWSpace;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //configuration.Factory.Shutdown();
                //configuration.Instance.Factory.Reset();
            }
            return bolResult;
        }

        //not used
        public bool CommitWireRemove(string tcxFilename, out string ParametersForCWSpace, out string ParametersForMWSpace, WebConfiguration ConfigBC)
        {
            Result tactonWireRemove;
            ParametersForCWSpace = "";
            ParametersForMWSpace = "";
            string strBendType = "";
            Hashtable slpResult = null;
            string resultString = "";
            bool bolResult = false;
            BOMInfo objBOMInfo = new BOMInfo();

            try
            {
                LoadConfiguration(tcxFilename, ConfigBC);

                configuration.Instance.SetStep("input_step");

                tactonWireRemove = TactonCommit("bending_check_type_field", strBending_Check_type);
                //CommitFail(tactonWireRemove, "bending_check_type_field = " + strBending_Check_type);
                tactonWireRemove = TactonCommit("bending_check_no_of_cw_field", strCW);
                //CommitFail(tactonWireRemove, "bending_check_no_of_cw_field = " + strCW);
                tactonWireRemove = TactonCommit("bending_check_cw_dia_field", strBending_Check_cw_dia);
                //CommitFail(tactonWireRemove, "bending_check_cw_dia_field = " + strBending_Check_cw_dia);
                tactonWireRemove = TactonCommit("bending_check_no_of_mw_field", strMW);
                //CommitFail(tactonWireRemove, "bending_check_no_of_mw_field = " + strMW);
                tactonWireRemove = TactonCommit("bending_check_mw_dia_field", strBending_Check_mw_dia);
                //CommitFail(tactonWireRemove, "bending_check_mw_dia_field = " + strBending_Check_mw_dia);
                tactonWireRemove = TactonCommit("bending_check_mw_spacing_field", strBending_Check_mw_spacing);
                //CommitFail(tactonWireRemove, "bending_check_mw_spacing_field = " + strBending_Check_mw_spacing);
                tactonWireRemove = TactonCommit("bending_check_cw_spacing_field", strBending_Check_cw_spacing);
                //CommitFail(tactonWireRemove, "bending_check_cw_spacing_field = " + strBending_Check_cw_spacing);

                tactonWireRemove = TactonCommit("bending_check_mo1_field", strBending_Check_mo1);
                tactonWireRemove = TactonCommit("bending_check_mo2_field", strBending_Check_mo2);
                tactonWireRemove = TactonCommit("bending_check_co1_field", strBending_Check_co1);
                tactonWireRemove = TactonCommit("bending_check_co2_field", strBending_Check_co2);

                tactonWireRemove = TactonCommit("bending_check_total_spacing_field", strTotalSpace);
                //CommitFail(tactonWireRemove, "bending_check_total_spacing_field = " + strTotalSpace);

                tactonWireRemove = TactonCommit("bending_check_minmwspace_field", strMinMWSpace);
                //CommitFail(tactonWireRemove, "bending_check_minmwSpace_field = " + strMinMWSpace);

                tactonWireRemove = TactonCommit("bending_check_maxmwspace_field", strMaxMWSpace);
                //CommitFail(tactonWireRemove, "bending_check_maxmwSpace_field = " + strMaxMWSpace);

                tactonWireRemove = TactonCommit("bending_check_mincwspace_field", strMinCWSpace);
                //CommitFail(tactonWireRemove, "bending_check_mincwSpace_field = " + strMinCWSpace);

                tactonWireRemove = TactonCommit("bending_check_maxcwspace_field", strMaxCWSpace);
                //CommitFail(tactonWireRemove, "bending_check_maxcwSpace_field = " + strMaxCWSpace);

                for (int i = 1; i <= (intNoOfBends + 1); i++)
                {
                    tactonWireRemove = TactonCommit("bending_check_" + i.ToString() + "_no_of_wire_field", str_no_of_wire[i]);
                    //CommitFail(tactonWireRemove, "bending_check_" + i.ToString() + "_no_of_wire_field = " + str_no_of_wire(i));
                }

                string strFBSB = null;
                strFBSB = "sb";
                int x = 0;
                x = 0;
                for (int i = 1; i <= intSegmentCount; i++)
                {
                    if (i / 2f == Convert.ToInt32(i / 2))
                    {
                        strFBSB = "sb";
                    }
                    else
                    {
                        strFBSB = "fb";
                    }
                    if (i == intSegmentCount)
                    {
                        strFBSB = "sb";
                    }
                    configuration.Instance.UnCommit("bending_check_" + strFBSB + "_" + strSegmentNames[i - 1] + "_field");
                    tactonWireRemove = TactonCommit("bending_check_" + strFBSB + "_" + strSegmentNames[i - 1] + "_field", strSegmentBCValue[x]);
                    //CommitFail(tactonWireRemove, "bending_check_" + strFBSB + "_" + strSegmentName(i - 1) + "_field = " + strSegmentBCValue(x));
                    x = x + 1;
                    if (strFBSB == "sb")
                    {
                        strFBSB = "fb";
                    }
                    else
                    {
                        strFBSB = "sb";
                    }
                    if (i > 1 & i != intSegmentCount)
                    {
                        configuration.Instance.UnCommit("bending_check_" + strFBSB + "_" + strSegmentNames[i - 1] + "_field");
                        tactonWireRemove = TactonCommit("bending_check_" + strFBSB + "_" + strSegmentNames[i - 1] + "_field", strSegmentBCValue[x]);
                        //CommitFail(tactonWireRemove, "bending_check_" + strFBSB + "_" + strSegmentName(i - 1) + "_field = " + strSegmentBCValue(x));
                        x = x + 1;
                    }
                }
                if (intBendLengthCount == 4)
                {
                    strBendType = "reverse";
                }
                else
                {
                    strBendType = "normal";
                }
                for (int i = 1; i <= intBendLengthCount; i++)
                {
                    configuration.Instance.UnCommit("bending_check_bend_length_" + strBendType + "_" + i.ToString() + "_field");
                    tactonWireRemove = TactonCommit("bending_check_bend_length_" + strBendType + "_" + i.ToString() + "_field", strBendLengths[i - 1]);
                    //CommitFail(tactonWireRemove, "bending_check_bend_length_" + strBendType + "_" + i.ToString() + "_field = " + strBending_Check_bend_length(i - 1));
                }

                //Result
                configuration.Instance.SetStep("wire_removal_step");

                //commiting 
                Result resultWire = TactonCommit("bending_check_wire_removal_method_status_" + intNoOfBends.ToString() + "bend_" + strBendType + "_field", "Yes");

                //result 
                Group GroupResultWire = (Group)resultWire.RootGroup.SubGroups["result_group"];
                slpResult = new Hashtable();
                foreach (Parameter param in GroupResultWire.Parameters.Values)
                {
                    slpResult.Add(param.Name, param.Value);
                }

                if ((strBendType == "normal"))
                {
                    resultWire = TactonCommit("bending_check_spacing_string_mo_commit_1_normal", (string)slpResult["bending_check_spacing_string_mo_commit_1_normal"]);
                    resultWire = TactonCommit("bending_check_spacing_string_mo_commit_2_normal", (string)slpResult["bending_check_spacing_string_mo_commit_2_normal"]);
                    resultWire = TactonCommit("bending_check_spacing_string_co_commit_1_normal", (string)slpResult["bending_check_spacing_string_co_commit_1_normal"]);
                    resultWire = TactonCommit("bending_check_spacing_string_co_commit_2_normal", (string)slpResult["bending_check_spacing_string_co_commit_2_normal"]);
                }
                else if ((strBendType == "reverse"))
                {
                    resultWire = TactonCommit("bending_check_spacing_string_mo_commit_1_reverse", (string)slpResult["bending_check_spacing_string_mo_commit_1_reverse"]);
                    resultWire = TactonCommit("bending_check_spacing_string_mo_commit_2_reverse", (string)slpResult["bending_check_spacing_string_mo_commit_2_reverse"]);
                    resultWire = TactonCommit("bending_check_spacing_string_co_commit_1_reverse", (string)slpResult["bending_check_spacing_string_co_commit_1_reverse"]);
                    resultWire = TactonCommit("bending_check_spacing_string_co_commit_2_reverse", (string)slpResult["bending_check_spacing_string_co_commit_2_reverse"]);
                }

                //result 
                GroupResultWire = (Group)resultWire.RootGroup.SubGroups["result_group"];
                slpResult = new Hashtable();
                foreach (Parameter param in GroupResultWire.Parameters.Values)
                {
                    slpResult.Add(param.Name, param.Value);
                }

                //result 
                resultString = slpResult["bending_check_wire_removal_method_status_" + intNoOfBends.ToString() + "bend_" + strBendType + "_field"].ToString();
                if (resultString.ToUpper().Trim() == "YES")
                {
                    bolResult = true;
                    if (chrMOCO == 'M')
                    {
                        ParametersForCWSpace = slpResult["bending_check_spacing_string_" + strBendType + "_post"].ToString();
                        ParametersForMWSpace = "0";
                    }
                    else if (chrMOCO == 'C')
                    {
                        ParametersForCWSpace = "0";
                        ParametersForMWSpace = slpResult["bending_check_spacing_string_" + strBendType + "_post"].ToString();
                    }

                    objBOMInfo.ParametersForCWSpace = ParametersForCWSpace;
                    objBOMInfo.ParametersForMWSpace = ParametersForMWSpace;
                }
                else
                {
                    ParametersForCWSpace = "";
                    ParametersForMWSpace = "";
                    if (chrMOCO == 'M')
                    {
                        for (int i = 1; i <= intNoOfBends; i++)
                        {
                            ParametersForCWSpace = ParametersForCWSpace + slpResult["bending_check_wire_removal_wire_number_mo_" + i.ToString() + "_bend_" + strBendType + "_post"].ToString();
                            ParametersForCWSpace = ParametersForCWSpace + ';';
                        }
                        ParametersForMWSpace = "0";
                    }
                    else if (chrMOCO == 'C')
                    {
                        ParametersForCWSpace = "0";
                        for (int i = 1; i <= intNoOfBends; i++)
                        {
                            ParametersForMWSpace = ParametersForMWSpace + slpResult["bending_check_wire_removal_wire_number_co_" + i.ToString() + "_bend_" + strBendType + "_post"].ToString();
                            ParametersForMWSpace = ParametersForMWSpace + ';';
                        }
                    }

                    objBOMInfo.ParametersForCWSpace = ParametersForCWSpace;
                    objBOMInfo.ParametersForMWSpace = ParametersForMWSpace;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                
            }
            return bolResult;
        }

        private Result TactonCommit(string paramName, string paramValue)
        {
            Result tactonResult = null;
            string tcxFilename = "";

            try
            {
                tactonResult = configuration.Instance.Commit(paramName, paramValue);
                if (!tactonResult.Response.IsOk)
                {
                    using (XmlReader reader = XmlReader.Create(new StringReader(tactonResult.State)))
                    {
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Text && reader.Value.EndsWith("tcx"))
                            {
                                tcxFilename = reader.Value;
                                break;
                            }
                        }
                    }
                    if (tactonResult.Response.IsResolvable)
                    {
                        throw new Exception("Tacton Commit Alert: " + paramName + " : " + paramValue + " in " + tcxFilename);
                    }
                    else
                    {
                        throw new Exception("Tacton Commit Error: " + paramName + " : " + paramValue + " in " + tcxFilename);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return tactonResult;
        }
    }
}
