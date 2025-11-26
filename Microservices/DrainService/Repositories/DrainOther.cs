using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using Tacton.Configurator.Core;
//using Tacton.Configurator.Interfaces;
using Tacton.Configurator.ObjectModel;
using Tacton.Configurator.Helpers;
//using Tacton.Configurator.Public;

using System.Data.SqlClient;
using System.Data;
using System.Xml;
using System.Collections;
using System.IO;
using System.Configuration;
using DrainService.Constants;

namespace DrainService.Repositories
{
    public class DrainOther
    {
        private readonly IConfiguration _configuration;
        private string connectionString;


        public DrainOther(IConfiguration configuration)
        {
            //_dbContext = dbContext;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString(SystemConstants.DefaultDBConnection);
        }
        public DrainOther()
        {

        }


        //DataAccessor accessor = null;
        SlabDetailingComponent objInfo = new SlabDetailingComponent();
        NameValueCollection properties = null;
      
        private string _serverNameString = null;
        private string _safeStateString = null;
        //public string strLogError = ConfigurationSettings.AppSettings["ErrorLog"];
        //private string strTcxPath = ConfigurationSettings.AppSettings["TCX_Folder_Path_Slab"];
        private int intPinSize = 30;
        private DetailInfo objDetailInfo = new DetailInfo();
        private DetailDal objDetailDal = new DetailDal();
        private BOMInfo objBOMInfo = new BOMInfo();
        private BOMDal objBOMDal = new BOMDal();
        private string errorMesg = "";

        public void StartConfiguration(string serverName)
        {
            _serverNameString = serverName;
           /* configuration = refconfiguration*/;
        }

        //private string LoadConfigurationFromFile(string strPath)
        //{
        //    string returnSafeState = "";
        //    try
        //    {
        //        configuration.Instance.Filter = "groups;parameters;report";

        //        configuration.Instance.StartConfiguration(strPath, null);

        //        returnSafeState = configuration.Instance.SafeState;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return returnSafeState;
        //}

        //private string LoadConfigurationFromSafeState(string strSafeState)
        //{
        //    string returnSafeState = "";
        //    try
        //    {
        //        configuration.Instance.Filter = "groups;parameters;report";
        //        configuration.Instance.StartConfiguration(strSafeState);
        //        returnSafeState = configuration.Instance.SafeState;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return returnSafeState;
        //}

        public bool GenerateOtherDrainProducts(string strProductMarkName, string vchMeshShape, int intShapeId, string strMWLen, string strCWLen, string strMWSpace, string strCWSpace, string strMWDia, string strCWDia, string strMO1, string strMO2, string strCO1, string strCO2, string strProductMarkId, int intStructureMarkId, string strProductCodeId, bool blnTC, bool blnBC, bool blnMC, string strShapeCode, string strProductCode, int intTransHeaderId, int TotMeshQty, string strSequence, string strParamValues, string strCriticalIndicator, int intParameterSet, int tntStructureRevNo, string strUserId, string strPrdInd, out string strErrorMesg, string bitOHVal)
        {
            strErrorMesg = "";
            AdminInfo objAdminInfo = new AdminInfo();
            AdminDal objAdminDal = new AdminDal();
            //TMCBLL objTMC = new TMCBLL();
            ArrayList alShapes = new ArrayList();

            Boolean blnResult = false;
            //int intResult = 0;
            int intProduction_MO1 = 0;
            int intProduction_MO2 = 0;
            int intProduction_CO1 = 0;
            int intProduction_CO2 = 0;
            int envelop_height = 0;
            int envelop_width = 0;
            int envelop_length = 0;
            decimal actual_tonnage = 0.0M;
            Int16 intProductTypeId = 0;
            int intMWPrdLength = 0;
            int intCWPrdLength = 0;

            int intNoOfMW = 0;
            int intNoOfCW = 0;
            try
            {
                ////string slabTcxPath = strTcxPath + vchMeshShape.Trim() + ".tcx";
                /// commented by Pankaj

                //LoadConfigurationFromFile(slabTcxPath);

                //configuration.Instance.UnCommit(vchMeshShape.Trim() + "_a_field");
                //configuration.Instance.UnCommit(vchMeshShape.Trim() + "_b_field");
                //configuration.Instance.UnCommit(vchMeshShape.Trim() + "_c_field");
                //configuration.Instance.UnCommit(vchMeshShape.Trim() + "_d_field");
                //configuration.Instance.UnCommit(vchMeshShape.Trim() + "_e_field");
                //configuration.Instance.UnCommit(vchMeshShape.Trim() + "_f_field");
                //configuration.Instance.UnCommit(vchMeshShape.Trim() + "_g_field");
                //configuration.Instance.UnCommit(vchMeshShape.Trim() + "_h_field");
                //configuration.Instance.UnCommit(vchMeshShape.Trim() + "_i_field");
                //configuration.Instance.UnCommit(vchMeshShape.Trim() + "_j_field");
                //configuration.Instance.UnCommit(vchMeshShape.Trim() + "_k_field");
                //configuration.Instance.UnCommit(vchMeshShape.Trim() + "_p_field");
                //configuration.Instance.UnCommit(vchMeshShape.Trim() + "_q_field");
                //configuration.Instance.UnCommit(vchMeshShape.Trim() + "_r_field");
                //configuration.Instance.UnCommit(vchMeshShape.Trim() + "_s_field");
                //configuration.Instance.UnCommit(vchMeshShape.Trim() + "_t_field");
                //configuration.Instance.UnCommit(vchMeshShape.Trim() + "_u_field");
                //configuration.Instance.UnCommit(vchMeshShape.Trim() + "_v_field");
                //configuration.Instance.UnCommit(vchMeshShape.Trim() + "_w_field");

                SlabDetailingComponent objInfo = new SlabDetailingComponent();
                Dictionary<string, string> inputOtherDrainProducts = new Dictionary<string, string>();
                objInfo.ClearDictionary();
                ArrayList alShapeDtl = new ArrayList();
                //string[] aShapeDtl=null;
                if (strParamValues != "")
                {
                    if (strParamValues.Contains("i"))
                    {
                        //aShapeDtl = strParamValues.Split('i');
                        alShapeDtl.AddRange(strParamValues.Split(new char[] { 'i' }));
                    }
                    else if (strParamValues.Contains(";"))
                    {
                        //aShapeDtl = new string[1];
                        //aShapeDtl = strParamValues.Split(';');
                        alShapeDtl.AddRange(strParamValues.Split(new char[] { ';' }));
                    }
                    else
                    {
                        alShapeDtl.Add(strParamValues);
                    }
                }
                //if (strCommitRequired != "")
                //{ 

                //}
                Hashtable htShapeDetails = new Hashtable();
                foreach (string shape in alShapeDtl)
                {
                    if (shape != "")
                    {
                        htShapeDetails.Add(shape.Split(':')[0].ToString().Trim().ToUpper(), shape.Split(':')[1]);
                    }
                }

                //configuration.Instance.SetStep(vchMeshShape.Trim() + "_step");
                inputOtherDrainProducts.Add("shape_code", strShapeCode.Trim());
                if (htShapeDetails.ContainsKey("A"))
                {
                    if (Convert.ToInt32(htShapeDetails["A"].ToString().Trim()) != 0)
                    {
                        inputOtherDrainProducts.Add("a", htShapeDetails["A"].ToString().Trim());
                    }
                }
                if (htShapeDetails.ContainsKey("B"))
                {
                    if (Convert.ToInt32(htShapeDetails["B"].ToString().Trim()) != 0)
                    {
                        inputOtherDrainProducts.Add("b", htShapeDetails["B"].ToString().Trim());
                    }
                }
                if (htShapeDetails.ContainsKey("C"))
                {
                    if (Convert.ToInt32(htShapeDetails["C"].ToString().Trim()) != 0)
                    {
                        inputOtherDrainProducts.Add("c", htShapeDetails["C"].ToString().Trim());
                    }
                }
                if (htShapeDetails.ContainsKey("D"))
                {
                    if (Convert.ToInt32(htShapeDetails["D"].ToString().Trim()) != 0)
                    {
                        inputOtherDrainProducts.Add("d", htShapeDetails["D"].ToString().Trim());
                    }
                }
                if (htShapeDetails.ContainsKey("E"))
                {
                    if (Convert.ToInt32(htShapeDetails["E"].ToString().Trim()) != 0)
                    {
                        inputOtherDrainProducts.Add("e", htShapeDetails["E"].ToString().Trim());
                    }
                }
                if (htShapeDetails.ContainsKey("F"))
                {
                    if (Convert.ToInt32(htShapeDetails["F"].ToString().Trim()) != 0)
                    {
                        inputOtherDrainProducts.Add("f", htShapeDetails["F"].ToString().Trim());
                    }
                }
                if (htShapeDetails.ContainsKey("G"))
                {
                    if (Convert.ToInt32(htShapeDetails["G"].ToString().Trim()) != 0)
                    {
                        inputOtherDrainProducts.Add("g", htShapeDetails["G"].ToString().Trim());
                    }
                }
                if (htShapeDetails.ContainsKey("H"))
                {
                    if (Convert.ToInt32(htShapeDetails["H"].ToString().Trim()) != 0)
                    {
                        inputOtherDrainProducts.Add("h", htShapeDetails["H"].ToString().Trim());
                    }
                }
                if (htShapeDetails.ContainsKey("I"))
                {
                    if (Convert.ToInt32(htShapeDetails["I"].ToString().Trim()) != 0)
                    {
                        inputOtherDrainProducts.Add("i", htShapeDetails["I"].ToString().Trim());
                    }
                }
                if (htShapeDetails.ContainsKey("J"))
                {
                    if (Convert.ToInt32(htShapeDetails["J"].ToString().Trim()) != 0)
                    {
                        inputOtherDrainProducts.Add("j", htShapeDetails["J"].ToString().Trim());
                    }
                }
                if (htShapeDetails.ContainsKey("K"))
                {
                    if (Convert.ToInt32(htShapeDetails["K"].ToString().Trim()) != 0)
                    {
                        inputOtherDrainProducts.Add("k", htShapeDetails["K"].ToString().Trim());
                    }
                }
                if (htShapeDetails.ContainsKey("P"))
                {
                    if (Convert.ToInt32(htShapeDetails["P"].ToString().Trim()) != 0)
                    {
                        inputOtherDrainProducts.Add("p", htShapeDetails["P"].ToString().Trim());
                    }
                }
                if (htShapeDetails.ContainsKey("Q"))
                {
                    if (Convert.ToInt32(htShapeDetails["Q"].ToString().Trim()) != 0)
                    {
                        inputOtherDrainProducts.Add("q", htShapeDetails["Q"].ToString().Trim());
                    }
                }
                if (htShapeDetails.ContainsKey("R"))
                {
                    if (Convert.ToInt32(htShapeDetails["R"].ToString().Trim()) != 0)
                    {
                        inputOtherDrainProducts.Add("r", htShapeDetails["R"].ToString().Trim());
                    }
                }
                if (htShapeDetails.ContainsKey("S"))
                {
                    if (Convert.ToInt32(htShapeDetails["S"].ToString().Trim()) != 0)
                    {
                        inputOtherDrainProducts.Add("s", htShapeDetails["S"].ToString().Trim());
                    }
                }
                if (htShapeDetails.ContainsKey("T"))
                {
                    if (Convert.ToInt32(htShapeDetails["T"].ToString().Trim()) != 0)
                    {
                        inputOtherDrainProducts.Add("t", htShapeDetails["T"].ToString().Trim());
                    }
                }
                if (htShapeDetails.ContainsKey("U"))
                {
                    if (Convert.ToInt32(htShapeDetails["U"].ToString().Trim()) != 0)
                    {
                        inputOtherDrainProducts.Add("u", htShapeDetails["U"].ToString().Trim());
                    }
                }
                if (htShapeDetails.ContainsKey("V"))
                {
                    if (Convert.ToInt32(htShapeDetails["V"].ToString().Trim()) != 0)
                    {
                        inputOtherDrainProducts.Add("v", htShapeDetails["V"].ToString().Trim());
                    }
                }
                if (htShapeDetails.ContainsKey("W"))
                {
                    if (Convert.ToInt32(htShapeDetails["W"].ToString().Trim()) != 0)
                    {
                        inputOtherDrainProducts.Add("w", htShapeDetails["W"].ToString().Trim());
                    }
                }
                string weight_per_area = "";
                string mw_weight_m_run = "";
                string cw_weight_m_run = "";
                List<ProductCode> lstData = GetProductCode(strProductCode.ToString().Trim());

                if (lstData != null)
                {
                    cw_weight_m_run = Convert.ToString(lstData[0].CwWeightPerMeterRun);
                    weight_per_area = Convert.ToString(lstData[0].WeightArea);
                    mw_weight_m_run = Convert.ToString(lstData[0].WeightPerMeterRun);
                }
                inputOtherDrainProducts.Add("MeshShapeGroup", vchMeshShape);
                inputOtherDrainProducts.Add("weight_per_area", weight_per_area);
                inputOtherDrainProducts.Add("mw_weight_m_run", mw_weight_m_run);
                inputOtherDrainProducts.Add("cw_weight_m_run", cw_weight_m_run);
                inputOtherDrainProducts.Add("mw_length", strMWLen.Trim());
                inputOtherDrainProducts.Add("cw_length", strCWLen.Trim());
                inputOtherDrainProducts.Add("cw_spacing", strCWSpace.Trim());
                inputOtherDrainProducts.Add("mw_spacing", strMWSpace.Trim());
                inputOtherDrainProducts.Add("cw_dia", Convert.ToInt32(strCWDia).ToString().Trim());
                inputOtherDrainProducts.Add("mw_dia", Convert.ToInt32(strMWDia).ToString().Trim());
                inputOtherDrainProducts.Add("mo1", strMO1.Trim());
                objDetailInfo.intInvoiceMO1 = Convert.ToInt32(strMO1);
                inputOtherDrainProducts.Add("mo2", strMO2.Trim());
                objDetailInfo.intInvoiceMO2 = Convert.ToInt32(strMO2);
                inputOtherDrainProducts.Add("co1", strCO1.Trim());
                objDetailInfo.intInvoiceCO1 = Convert.ToInt32(strCO1);
                inputOtherDrainProducts.Add("co2", strCO2.Trim());
                objDetailInfo.intInvoiceCO2 = Convert.ToInt32(strCO2);
                inputOtherDrainProducts.Add("pin_dia", intPinSize.ToString().Trim());
                objDetailInfo.sitPinSize = intPinSize;
                string convertedString = string.Empty;
                objInfo.FillInputDictionary(inputOtherDrainProducts);
                convertedString = ConvertJsonFormat(inputOtherDrainProducts);
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //Group SlabShape_Result = (Group)configuration.Instance.Result.RootGroup.SubGroups["result_group"];
                Dictionary<string, string> SlabShape_Result = objInfo.ExecuteDetailingComponent(intShapeId, string.Empty);

                //group.Parameters                                      
                foreach (KeyValuePair<string, string> item in SlabShape_Result)
                {
                    if (item.Key.ToLower() == "no_of_mw")
                    {
                        objDetailInfo.intInvoiceMainQty = Convert.ToInt32(Decimal.Parse(item.Value));
                        objDetailInfo.intProductionMainQty = Convert.ToInt32(Decimal.Parse(item.Value));
                        intNoOfMW = Convert.ToInt32(Decimal.Parse(item.Value));
                    }
                    else if (item.Key.ToLower() == "no_of_cw")
                    {
                        objDetailInfo.intInvoiceCrossQty = Convert.ToInt32(Decimal.Parse(item.Value));
                        objDetailInfo.intProductionCrossQty = Convert.ToInt32(Decimal.Parse(item.Value));
                        intNoOfCW = Convert.ToInt32(Decimal.Parse(item.Value));
                    }
                    else if (item.Key.ToLower() == "area_post")
                    {
                        objDetailInfo.numInvoiceArea = Convert.ToDecimal(Decimal.Parse(item.Value));
                    }
                    else if (item.Key.ToLower() == "envelope_length")
                    {
                        objDetailInfo.intEnvelopeLength = Convert.ToInt32(Decimal.Parse(item.Value));
                        envelop_length = Convert.ToInt32(Decimal.Parse(item.Value));
                    }
                    else if (item.Key.ToLower() == "envelope_width")
                    {
                        objDetailInfo.intEnvelopewidth = Convert.ToInt32(Decimal.Parse(item.Value));
                        envelop_width = Convert.ToInt32(Decimal.Parse(item.Value));
                    }
                    else if (item.Key.ToLower() == "envelope_height")
                    {
                        objDetailInfo.intEnvelopeHeight = Convert.ToInt32(Decimal.Parse(item.Value));
                        envelop_height = Convert.ToInt32(Decimal.Parse(item.Value));
                    }
                    else if (item.Key.ToLower() == "theoretical_tonnage")
                    {
                        objDetailInfo.numTheoraticalWeight = Decimal.Parse(item.Value);
                    }
                    else if (item.Key.ToLower() == "net_tonnage")
                    {
                        objDetailInfo.numNetWeight = Decimal.Parse(item.Value);
                    }
                    else if (item.Key.ToLower() == "production_mw_length")
                    {
                        //objDetailInfo.numProductionMWLength = Convert.ToDecimal(p.ValueDescription);
                        intMWPrdLength = GetRoundOffValuefrmDb(Decimal.Parse(item.Value), intProductTypeId);
                    }
                    else if (item.Key.ToLower() == "production_cw_length")
                    {
                        //objDetailInfo.numProductionCWLength = Convert.ToDecimal(p.ValueDescription);
                        intCWPrdLength = GetRoundOffValuefrmDb(Decimal.Parse(item.Value), intProductTypeId);
                    }
                    else if (item.Key.ToLower() == "actual_tonnage")
                    {
                        objDetailInfo.numProductionWeight = Decimal.Parse(item.Value);
                        actual_tonnage = Decimal.Parse(item.Value);
                    }
                    else if (item.Key.ToLower() == "production_mo1")
                    {
                        intProduction_MO1 = Convert.ToInt32(Decimal.Parse(item.Value));
                        //objDetailInfo.intProductionMO1 = intProduction_MO1;
                    }
                    else if (item.Key.ToLower() == "production_mo2")
                    {
                        intProduction_MO2 = Convert.ToInt32(Decimal.Parse(item.Value));
                        //objDetailInfo.intProductionMO2 = intProduction_MO2;
                    }
                    else if (item.Key.ToLower() == "production_co1")
                    {
                        intProduction_CO1 = Convert.ToInt32(Decimal.Parse(item.Value));
                        //objDetailInfo.intProductionCO1 = intProduction_CO1;
                    }
                    else if (item.Key.ToLower() == "production_co2")
                    {
                        intProduction_CO2 = Convert.ToInt32(Decimal.Parse(item.Value));
                        //objDetailInfo.intProductionCO2 = intProduction_CO2;
                    }
                    else if (item.Key.ToLower() == "mw_bvbs_string")
                    {
                        objDetailInfo.vchMWBVBSString = item.Value;
                    }
                    else if (item.Key.ToLower() == "cw_bvbs_string")
                    {
                        objDetailInfo.vchCWBVBSString = item.Value;
                    }
                    else if (item.Key.ToLower() == "string_post")
                    {
                        strParamValues = item.Value;
                    }

                }
                objDetailInfo.numProductionMWLength = Convert.ToDecimal(intMWPrdLength);
                objDetailInfo.numProductionCWLength = Convert.ToDecimal(intCWPrdLength);
                //Rounding on basis of creep
                string strBitOH = bitOHVal;
                string strCreepMO1 = "0";
                string strCreepCO1 = "0";
                try
                {
                    strCreepMO1 = bitOHVal.Split('@')[0].ToString().Trim();
                    strCreepCO1 = bitOHVal.Split('@')[1].ToString().Trim();
                }
                catch (Exception ed)
                {

                }
                finally
                {
                    strCreepMO1 = "0";
                    strCreepCO1 = "0";
                }
                if (strCreepMO1 == "1")
                {
                    objDetailInfo.intProductionMO1 = Convert.ToInt32(strMO1) - (Convert.ToInt32(strMWLen) - intMWPrdLength);
                    intProduction_MO1 = Convert.ToInt32(strMO1) - (Convert.ToInt32(strMWLen) - intMWPrdLength);
                    intProduction_MO2 = Convert.ToInt32(strMO2);
                    objDetailInfo.intProductionMO2 = intProduction_MO2;
                }
                else
                {
                    objDetailInfo.intProductionMO2 = Convert.ToInt32(strMO2) - (Convert.ToInt32(strMWLen) - intMWPrdLength);
                    intProduction_MO2 = Convert.ToInt32(strMO2) - (Convert.ToInt32(strMWLen) - intMWPrdLength);
                    intProduction_MO1 = Convert.ToInt32(strMO1);
                    objDetailInfo.intProductionMO1 = intProduction_MO1;
                }
                if (strCreepCO1 == "1")
                {
                    objDetailInfo.intProductionCO1 = Convert.ToInt32(strCO1) - (Convert.ToInt32(strCWLen) - intCWPrdLength);
                    intProduction_CO1 = Convert.ToInt32(strCO1) - (Convert.ToInt32(strCWLen) - intCWPrdLength);
                    intProduction_CO2 = Convert.ToInt32(strCO2);
                    objDetailInfo.intProductionCO2 = intProduction_CO2;
                }
                else
                {
                    objDetailInfo.intProductionCO2 = Convert.ToInt32(strCO2) - (Convert.ToInt32(strCWLen) - intCWPrdLength);
                    intProduction_CO2 = Convert.ToInt32(strCO2) - (Convert.ToInt32(strCWLen) - intCWPrdLength);
                    intProduction_CO1 = Convert.ToInt32(strCO1);
                    objDetailInfo.intProductionCO1 = intProduction_CO1;
                }

                objDetailInfo.intStructureMarkId = 0;

                objDetailInfo.intDrainStructureMarkId = intStructureMarkId;
                objDetailInfo.tntStructureMarkRevNo = tntStructureRevNo;
                objDetailInfo.vchProductMarkingName = strProductMarkName;
                objDetailInfo.intProductCodeId = Convert.ToInt32(strProductCodeId);
                objDetailInfo.intShapeCodeId = Convert.ToInt32(intShapeId);
                objDetailInfo.numInvoiceMWLength = Convert.ToDecimal(strMWLen);
                objDetailInfo.numInvoiceCWLength = Convert.ToDecimal(strCWLen);

                objDetailInfo.intInvoiceTotalQty = TotMeshQty;
                objDetailInfo.intProductionTotalQty = TotMeshQty;
                objDetailInfo.intMemberQty = TotMeshQty;

                objDetailInfo.intMWSpacing = Convert.ToInt32(strMWSpace);
                objDetailInfo.intCWSpacing = Convert.ToInt32(strCWSpace);
                //objDetailInfo.sitPinSize = intPinSize;
                objDetailInfo.bitCoatingIndicator = false;
                objDetailInfo.numConversionFactor = 0;
                objDetailInfo.vchShapeSurcharge = "Y";
                objDetailInfo.bitBendIndicator = blnBC;
                objDetailInfo.chrCalculationIndicator = "Y";
                objDetailInfo.tntGenerationStatus = 1;

                if (blnTC)
                {
                    string TCResult = "";
                    TCResult = TransportCheck(actual_tonnage, envelop_height, envelop_width, envelop_length, intParameterSet);
                    objDetailInfo.vchTransportCheckResult = TCResult;
                    if (TCResult.Trim() == "Pass")
                    {
                        objDetailInfo.bitTransportIndicator = true;
                    }
                    else
                    {
                        objDetailInfo.bitTransportIndicator = false;
                    }
                }
                else
                {
                    objDetailInfo.vchTransportCheckResult = "N/A";
                    objDetailInfo.bitTransportIndicator = false;
                }

                ///////////////////////////////////
                Boolean TMC = false;
                string bend_ind = "";
                string twin_ind = "";
                string vchBendingGroup = "";
                bool blnBendInd = false;
                if (blnMC)
                {
                    DataSet dsMcnChk = new DataSet();
                    DataTable dtMachineCheck = new DataTable();
                    objDetailInfo.chrShapeCode = strShapeCode;
                    objDetailInfo.intProductCodeId = Convert.ToInt32(strProductCodeId); ;
                    dsMcnChk = objDetailDal.GetMachineCheckValues(objDetailInfo);
                    if (dsMcnChk.Tables.Count > 0)
                    {
                        dtMachineCheck = dsMcnChk.Tables[0];
                        if ((dtMachineCheck.Rows.Count > 0))
                        {
                            int intMWQty, intCWQty;

                            intMWQty = 0;
                            intCWQty = 0;
                            bend_ind = "";
                            twin_ind = "";
                            if ((dtMachineCheck.Rows[0]["MWQty"].ToString() != ""))
                            {
                                intMWQty = (int)dtMachineCheck.Rows[0]["MWQty"];
                            }
                            if ((dtMachineCheck.Rows[0]["CWQty"].ToString() != ""))
                            {
                                intCWQty = (int)dtMachineCheck.Rows[0]["CWQty"];
                            }
                            if (dtMachineCheck.Rows[0]["Twin_Ind"].ToString() == "M")
                            {
                                twin_ind = "Yes";
                            }
                            else
                            {
                                twin_ind = "No";
                            }
                            vchBendingGroup = dtMachineCheck.Rows[0]["vchBendingGroup"].ToString();
                            blnBendInd = (bool)dtMachineCheck.Rows[0]["bitBendIndicator"];
                            bend_ind = dtMachineCheck.Rows[0]["Bend_Ind"].ToString();
                          //  TMC = objTMC.CheckMachineFeasibility(strTcxPath + "TMC.tcx", bend_ind, twin_ind, Convert.ToInt32(strMWDia), Convert.ToInt32(strCWDia), Convert.ToInt32(strMWLen), Convert.ToInt32(strCWLen), intNoOfMW, intNoOfCW, Convert.ToInt32(strMWSpace), Convert.ToInt32(strCWSpace), intMWQty, intCWQty, intProduction_MO1, intProduction_MO2, intProduction_CO1, intProduction_CO2, ConfigTMC); //by vidhya 

                        }
                    }
                }
                else
                {
                    TMC = false;
                }
                if (TMC == false)
                {
                    //errorMesg = "Machine Check Failed.";
                    //return false; //Anuran 
                }
                objDetailInfo.bitMachineCheckIndicator = TMC;
                ///////////////////////////////////
                objDetailInfo.vchBendCheckResult = "P";
                objDetailInfo.xmlResult = convertedString;
                objDetailInfo.vchFilePath = "";
                objDetailInfo.numInvoiceMWWeight = 0;
                objDetailInfo.numInvoiceCWWeight = 0;
                objDetailInfo.numProductionMWWeight = 0;
                objDetailInfo.numProductionCWWeight = 0;
                if (strParamValues.Contains("i"))
                {
                    objDetailInfo.ParamValues = strParamValues.Replace("i", ";");
                }
                else
                {
                    objDetailInfo.ParamValues = strParamValues;
                }
                objDetailInfo.BendingPos = "";
                objDetailInfo.intShapeId = Convert.ToInt32(intShapeId);
                if (strParamValues.Contains(";"))
                {
                    strParamValues = strParamValues.Replace(";", "i");
                }

                //else if (strParamValues.Contains("i"))
                //{
                if (strParamValues.EndsWith("i"))
                {
                    objDetailInfo.nvchParamValues = strParamValues;
                }
                else
                {
                    objDetailInfo.nvchParamValues = strParamValues.Trim() + "i";
                }
                //}
                //else
                //{
                //    objDetailInfo.nvchParamValues = strParamValues.Trim() + "i";
                //}
                objDetailInfo.intShapeTransHeaderId = intTransHeaderId;
                if (strSequence.EndsWith("i"))
                {
                    objDetailInfo.vchSequence = strSequence;
                }
                else
                {
                    objDetailInfo.vchSequence = strSequence.Trim() + "i";
                }
                if (strSequence.EndsWith("i"))
                {
                    objDetailInfo.vchCriticalIndicator = strCriticalIndicator;
                }
                else
                {
                    objDetailInfo.vchCriticalIndicator = strCriticalIndicator.Trim() + "i";
                }

                if (strUserId != string.Empty)
                {
                    objDetailInfo.intUserid = Convert.ToInt32(strUserId);
                }
                else
                {
                    objDetailInfo.intUserid = 0;
                }
                if ((strSequence != "") || (strCriticalIndicator != ""))
                {
                    objDetailInfo.intShapeTransHeaderId = objDetailDal.DrainOth_InsertShapeDetails(objDetailInfo);
                }
                //objDetailInfo.intStructureElementTypeId = 5;
                //objDetailInfo.tntLayer = 7;

                objDetailInfo.nvchProduceIndicator = strPrdInd;
                int intOutput = 0;
                if (strProductMarkId == "0")
                {
                    objDetailInfo.intProductMarkId = 0;
                    intOutput = objDetailDal.OtherDrainProductMarkingDetails_Insert(objDetailInfo);
                }
                else
                {
                    objDetailInfo.intProductMarkId = Convert.ToInt32(strProductMarkId);
                    intOutput = objDetailDal.OtherDrainProductMarkingDetails_Update(objDetailInfo);
                }
                bool blnBendChk = true;
                if (blnBendInd)
                {
                    if (!((vchMeshShape.Trim() == "flat") | (vchMeshShape.ToLower().Trim().StartsWith("clr") == true)))
                    {

                        //blnBendChk = BendingCheck(intOutput, intShapeId, strShapeCode, intProduction_MO1.ToString(), intProduction_CO1.ToString(), strParamValues, intProduction_MO2.ToString(), intProduction_CO2.ToString(), vchBendingGroup, bend_ind, vchMeshShape);
                        //int intStrElemntId = 5;
                        string strElementType = "drain";
                        ///blnBendChk = BendingCheckNew(intOutput, vchMeshShape, strElementType, ConfigBC, vchBendingGroup, intShapeId);
                        if (blnBendChk == false)
                        {
                            // Anuran >>>>>
                            //errorMesg = "Bending check failed. Please do necessary amendments in BOM.";
                        }
                    }
                }
                blnResult = true;

            }//End of reading Result
            catch (Exception ex)
            {
                //ErrorHandler.RaiseError(ex, strLogError);
                errorMesg = ex.Message.ToString().Trim();
                blnResult = false;
            }
            finally
            {
                objAdminInfo = null;
                objAdminDal = null;
                //objTMC = null;
                alShapes = null;
                objBOMInfo = null;
                objBOMDal = null;
                strErrorMesg = errorMesg;
            }
            strErrorMesg = errorMesg;
            return blnResult;

        }

        public string TransportCheck(decimal actual_tonnage_post, int envelope_height_field, int envelope_width_field, int envelope_length_field, int intParameterSet)
        {
            try
            {

                DataSet dsTCChk = new DataSet();
                DataTable dtTransportCheck = new DataTable();
                objDetailInfo.tntParamSetNumber = intParameterSet;
                dsTCChk = objDetailDal.GetTcTransChTCMaster(objDetailInfo);
                dtTransportCheck = dsTCChk.Tables[0];
                if ((dtTransportCheck.Rows.Count > 0))
                {
                    int intMaxLength = Convert.ToInt32(dtTransportCheck.Rows[0]["intMaxLength"].ToString().Trim());
                    int intMaxWidth = Convert.ToInt32(dtTransportCheck.Rows[0]["intMaxWidth"].ToString().Trim());
                    int intMaxHeight = Convert.ToInt32(dtTransportCheck.Rows[0]["intMaxHeight"].ToString().Trim());
                    if ((intMaxLength < envelope_length_field) | (intMaxWidth < envelope_width_field) | (intMaxHeight < envelope_height_field))
                    {
                        return "Fail";
                    }
                    else
                    {
                        return "Pass";
                    }
                }
                else
                {
                    return "Pass";
                }
            }
            catch
            {
                //ErrorHandler.RaiseError(ex, strLogError);
                //return "Fail";
                throw new Exception("Transport Check Failed");

            }
        }


        //private Result TactonCommit(string paramName, string paramValue)
        //{
        //    Result tactonResult = null;
        //    string tcxFilename = "";

        //    try
        //    {
        //        tactonResult = configuration.Instance.Commit(paramName, paramValue);
        //        if (!tactonResult.Response.IsOk)
        //        {
        //            using (XmlReader reader = XmlReader.Create(new StringReader(tactonResult.State)))
        //            {
        //                while (reader.Read())
        //                {
        //                    if (reader.NodeType == XmlNodeType.Text && reader.Value.EndsWith("tcx"))
        //                    {
        //                        tcxFilename = reader.Value;
        //                        break;
        //                    }
        //                }
        //            }
        //            if (tactonResult.Response.IsResolvable)
        //            {
        //                //try
        //                //{
        //                //    if (paramName.EndsWith("co2_field"))
        //                //    {
        //                //        //isFailedCO2Commit = true;
        //                //    }
        //                //    else if (paramName.EndsWith("mo2_field"))
        //                //    {
        //                //        //isFailedMO2Commit = true;
        //                //    }
        //                //}
        //                //catch
        //                //{
        //                //    //ErrorHandler.RaiseTactonErrorToFile(ex, strLogError, string.Empty);
        //                //}
        //                throw new Exception("Tacton Commit Error: " + paramName + " : " + paramValue + " in " + tcxFilename);

        //            }
        //            else
        //            {
        //                throw new Exception("Tacton Commit Error: " + paramName + " : " + paramValue + " in " + tcxFilename);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return tactonResult;
        //}
        int intStatus = 0;
        int intShapeid = 0;
        int intCWShapeid = 0;
        string MWBendingGroup = "";
        string CWBendingGroup = "";
        string strDoubleBend = "";

        private bool BendingCheckNew(int ProductMarkId, string vchShapeGroup, string strElementType,  string vchBendingGroup, int intShapeid)
        {
            bool bolResult = true;
            DataSet dsShape = new DataSet();

            string strBendingGroup = vchBendingGroup;
            BendingAdjustmentBLL objBendingCheck = new BendingAdjustmentBLL();
            try
            {
                objBOMInfo.ProductMarkingId = ProductMarkId;
                dsShape = objBOMDal.GetDoubleBend(objBOMInfo);
                if (dsShape.Tables.Count > 0)
                {
                    if (dsShape.Tables[0].Rows.Count > 0)
                    {
                        strDoubleBend = dsShape.Tables[0].Rows[0]["chrMOCO"].ToString().Trim();
                        if (strDoubleBend == "B" && intStatus == 0)
                        {
                            intStatus = 1;
                            intShapeid = Convert.ToInt32(dsShape.Tables[0].Rows[0]["MWShapeId"].ToString().Trim());
                            MWBendingGroup = dsShape.Tables[0].Rows[0]["MWBendingGroup"].ToString().Trim();
                            intCWShapeid = Convert.ToInt32(dsShape.Tables[0].Rows[0]["CWShapeId"].ToString().Trim());
                            CWBendingGroup = dsShape.Tables[0].Rows[0]["CWBendingGroup"].ToString().Trim();
                            strBendingGroup = MWBendingGroup;
                        }
                        else if (strDoubleBend == "B" && intStatus == 1)
                        {
                            intStatus = 2;
                            intShapeid = intCWShapeid;
                            strBendingGroup = CWBendingGroup;
                        }

                    }
                }
                else
                {
                    return bolResult;
                }

                //string ParametersForCWSpace = "";
                //string ParametersForMWSpace = "";

                if ((strDoubleBend == "B") && (MWBendingGroup == ""))
                {
                    BendingCheckNew(ProductMarkId, vchShapeGroup, strElementType,  vchBendingGroup, intShapeid);
                }
                else if ((strDoubleBend == "B") && (intStatus == 2) && (CWBendingGroup == ""))
                {
                    intStatus = 0;
                    MWBendingGroup = "";
                    CWBendingGroup = "";
                    strDoubleBend = "";
                    return bolResult;
                }
                objBendingCheck.GetBendingCheckValues(ProductMarkId, objBOMInfo, objDetailInfo, intShapeid, strElementType.ToString().Trim());
                if (!((vchShapeGroup.Trim() == "flat") || (vchShapeGroup.ToLower().Trim().StartsWith("clr") == true)))
                {
                    if (vchShapeGroup.Trim() != "0")
                    {
                        //string strShapeShiftTCXfilename = strTcxPath.Trim() + "Bending_Check_" + strBendingGroup.Trim() + ".tcx";
                        //bolResult = objBendingCheck.CommitBendingCheck(strShapeShiftTCXfilename, ConfigBC);
                       // Commented by Pankaj
                    }

                    //' ''Newly added for Double shape
                    if (intStatus == 1)
                    {
                        BendingCheckNew(ProductMarkId, vchShapeGroup, strElementType,  vchBendingGroup, intShapeid);
                    }
                    if ((intStatus == 2))
                    {
                        intStatus = 0;
                        MWBendingGroup = "";
                        CWBendingGroup = "";
                        strDoubleBend = "";
                    }
                    //' ''Newly added for Double shape                
                }
            }
            catch (Exception ex)
            {
                //throw new Exception("Bending check failed");
            }
            return bolResult;
        }

        private bool BendingCheck(int ProductMarkId, int intShapeid, string chrShapeCode, string ProductMO1, string ProductCO1, string strParamValues, string strProductMO2, string strProductCO2, string vchBendingGroup, string chrMOCO, string vchShapeGroup)
        {
            bool bolResult = true;
            string strBendingResult = "";
            try
            {
                if (!((vchShapeGroup.Trim() == "flat") | (vchShapeGroup.ToLower().Trim().StartsWith("clr") == true)))
                {
                    //if (vchBendingGroup != "0")
                    //{
                    ArrayList alBendChk = new ArrayList();
                    alBendChk.Add(ProductMarkId);
                    alBendChk.Add(intShapeid);
                    alBendChk.Add(chrShapeCode);
                    alBendChk.Add(ProductMO1);
                    alBendChk.Add(ProductCO1);
                    if (strParamValues.Contains("i"))
                    {
                        alBendChk.Add(strParamValues.Replace("i", ";"));
                    }
                    else
                    {
                        alBendChk.Add(strParamValues);
                    }
                    alBendChk.Add(strProductMO2);
                    alBendChk.Add(strProductCO2);
                    alBendChk.Add("Drain");
                    string strStatus = null;
                    string[] strResult = new string[7];

                    BendingCheck objBendingCheckSlab = new BendingCheck();
                    //strStatus =objBendingCheckSlab.BendingCheck(alBendChk); //by vidya
                    strResult = strStatus.Split(',');
                    strBendingResult = strResult[0];
                    if (strBendingResult.Trim() == "Pass")
                    {
                        if (strResult.Length > 5)
                        {
                            if ((strResult[5].ToString().Trim() == "OH") || (strResult[5].ToString().Trim() == "SS") || (strResult[5].ToString().Trim() == "WR"))
                            {
                                bolResult = false;
                            }
                            else
                            {
                                bolResult = true;
                            }
                        }
                    }
                    else
                    {
                        bolResult = false;
                    }
                    //if (!bolResult)
                    //{
                    //    //ViewState("BendingAlert") = 1;
                    //}
                    //else if (strResult[5] == "OH")
                    //{
                    //    SetOverhang(ref strResult[1], ref strResult[2], ref strResult[3], ref strResult[4], ProductMarkId, chrMOCO);
                    //}
                    //else if (strResult[5] == "SS")
                    //{
                    //    SetSpaceShift(ref strResult[1], ref strResult[2], ref strResult[3], ref strResult[4], ref strResult[6], ref strResult[7], ProductMarkId, chrMOCO);
                    //}
                    //else if (strResult[5] == "WR")
                    //{
                    //    if (strResult[0] == "Pass")
                    //    {
                    //        SetWireRemovalPass(ref strResult[6], ref strResult[7], ProductMarkId);
                    //    }
                    //    else
                    //    {
                    //        SetWireRemovalFail();
                    //    }
                    //}
                    //}
                }
            }
            catch (Exception ex)
            {
                //throw new Exception("Bending check failed");
            }
            return bolResult;
        }
        public void SetOverhang(ref string MOH1, ref string MOH2, ref string COH1, ref string COH2, int intProductMarkId, string chrMOCO)
        {
            try
            {
                objBOMInfo.ProductMarkingId = intProductMarkId;
                objBOMInfo.StructureElement = "Drain";
                objBOMInfo.OverHang = chrMOCO;
                objBOMInfo.MOH1 = MOH1;
                objBOMInfo.MOH2 = MOH2;
                objBOMInfo.COH1 = COH1;
                objBOMInfo.COH2 = COH2;
                int intOutput = objBOMDal.GetOverHang(objBOMInfo);

                if (intOutput == 1)
                {
                    errorMesg = "The bending adjustment is made by the system; please verify production BOM.";
                }
            }
            catch //(Exception ex)
            {
                throw new Exception("Error occured while making adjustment for overhangs");
            }
        }
        public void SetSpaceShift(ref string MOH1, ref string MOH2, ref string COH1, ref string COH2, ref string ParametersForCWSpace, ref string ParametersForMWSpace, int intProductMarkId, string chrMOCO)
        {
            try
            {
                objBOMInfo.ProductMarkingId = intProductMarkId;
                objBOMInfo.StructureElement = "Drain";
                objBOMInfo.OverHang = chrMOCO;
                objBOMInfo.MOH1 = MOH1;
                objBOMInfo.MOH2 = MOH2;
                objBOMInfo.COH1 = COH1;
                objBOMInfo.COH2 = COH2;
                objBOMInfo.ParametersForCWSpace = ParametersForCWSpace;
                objBOMInfo.ParametersForMWSpace = ParametersForMWSpace;
                int intOutput = objBOMDal.GetSpaceShift(objBOMInfo);
                if (intOutput == 1)
                {
                    errorMesg = "The bending adjustment is made by the system; please verify production BOM.";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occured while making adjustment");
            }
        }
        public void SetWireRemovalPass(ref string ParametersForCWSpace, ref string ParametersForMWSpace, int intProductMarkId)
        {
            try
            {
                objBOMInfo.ProductMarkingId = intProductMarkId;
                objBOMInfo.StructureElement = "Drain";
                objBOMInfo.ParametersForCWSpace = ParametersForCWSpace;
                objBOMInfo.ParametersForMWSpace = ParametersForMWSpace;
                int intOutput = objBOMDal.GetWireRemovalPass(objBOMInfo);
                if (intOutput == 1)
                {
                    errorMesg = "The bending adjustment is made by the system; please verify production BOM.";
                }
            }
            catch //(Exception ex)
            {
                throw new Exception("Error occured while making adjustment");
            }
        }
        public void SetWireRemovalFail()
        {
            try
            {
                errorMesg = "Normal BOM generated as shifting / skipping unsucessfull, Please change manually.";
            }
            catch //(Exception ex)
            {
                throw new Exception("Error occured while making adjustment");
            }
        }

        //private int RoundDownToFive(int intOHVal)
        //{
        //    int returnVal;
        //    try
        //    {
        //        int intRemainder = intOHVal % 5;
        //        if (intRemainder <= 3)
        //        {
        //            returnVal = 5 * (int)Math.Floor(intOHVal / 5.0);
        //        }
        //        else
        //        {
        //            returnVal = 5 * (int)Math.Round(intOHVal / 5.0);
        //        }
        //        //
        //    }
        //    catch
        //    {
        //        returnVal = intOHVal;
        //    }
        //    return returnVal;
        //}

        private int GetRoundOffValuefrmDb(decimal decProductionMWLength, int intProductTypeId)
        {
            int retunVal = 0;
            try
            {
                retunVal = Convert.ToInt32(decProductionMWLength);
                int numProductionMWLength = 0;
                int intProductionMWLength = 0;
                int intModValue = 0;
                int intBaseValue = 0;
                intProductionMWLength = (int)Math.Round(decProductionMWLength);
                intModValue = intProductionMWLength % 10;
                intBaseValue = intProductionMWLength - intModValue;

                objDetailInfo.sitProductTypeId = (Int16)intProductTypeId;
                objDetailInfo.intStructureElementTypeId = 0;
                objDetailInfo.intConsignmentType = 0;
                objDetailInfo.numProductionMWLength = intModValue;
                numProductionMWLength = objDetailDal.GetRoundOffValue(objDetailInfo);

                retunVal = intBaseValue + Convert.ToInt32(numProductionMWLength);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retunVal;
        }

        public List<ProductCode> GetProductCode(string enteredText)
        {
            List<ProductCode> listSlabProductCode = new List<ProductCode>();
            ProductCode objProductCode = new ProductCode();
            try
            {
                listSlabProductCode = objProductCode.SlabProductCodeFilter(enteredText);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objProductCode = null;
            }
            return listSlabProductCode;
        }

        public string ConvertJsonFormat(Dictionary<string, string> inputs)
        {
           // DrainJsonFormat DrainJsonObj = new DrainJsonFormat(); //by vidya
            string dictionaryString = "{";
            foreach (KeyValuePair<string, string> item in inputs)
            {
                dictionaryString += item.Key + " : " + item.Value + ", ";
            }
            return dictionaryString.TrimEnd(',', ' ') + "}";
        }


    }
}
