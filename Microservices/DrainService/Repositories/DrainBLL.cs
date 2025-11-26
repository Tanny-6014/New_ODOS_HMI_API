using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using Tacton.Configurator.Core;
using Tacton.Configurator.Interfaces;
using Tacton.Configurator.ObjectModel;
using Tacton.Configurator.Helpers;
using Tacton.Configurator.Public;
using System.Data.SqlClient;
using System.Data;
using System.Xml;
using System.Collections;
using System.IO;
using System.Configuration;
//using BendingCheckSlab;
using DrainService.Dtos;
using DrainService.Constants;
using Dapper;
using System.Web;

//using System.Web.UI.WebControls;
using Microsoft.AspNetCore.Mvc.Rendering;

//using Newtonsoft.Json;
// Added by anuran CHG0031337>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  

namespace DrainService.Repositories
{

    //class FileAccessor : DataAccessor
    //{
    //    public ModelFile GetModel(string modelkey)
    //    {
    //        return new DiskModelFile(modelkey);
    //    }

    //    public AttributeSet GetData(string datakey)
    //    {
    //        return null;
    //    }
    //}

    public class DrainBLL
    {
        private readonly IConfiguration _configuration;
        private string connectionString;


        public DrainBLL(IConfiguration configuration)
        {
            //_dbContext = dbContext;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString(SystemConstants.DefaultDBConnection);
        }

        //System.Configuration.Configuration configuration = null;
        //private string _serverNameString = ConfigurationSettings.AppSettings["Tacton.Configurator.DrainageServers"]; //"";// "//127.0.0.1:9090/NDSBendingCheck";
        public string strTcxPath = null;// ConfigurationSettings.AppSettings["TCX_Folder_Path_Drain"];
        public string strSlabTcxPath = null;//ConfigurationSettings.AppSettings["TCX_Folder_Path_Slab"];
        public int intPinSize = 32; // Convert.ToInt32(ConfigurationSettings.AppSettings["SlabPinsize"]);

         DetailInfo objDetailInfo = new DetailInfo();
         DetailDal objDetailDal = new DetailDal();
         InsertShapeValFrmTCDto insertShapeValFrmTC  = new InsertShapeValFrmTCDto();
        InsertDrainProdMarkDto drainProductMarking = new InsertDrainProdMarkDto();

        private AdminInfo objAdminInfo = new AdminInfo();
        private AdminDal objAdminDal = new AdminDal();
        private BOMInfo objBOMInfo = new BOMInfo();
        private BOMDal objBOMDal = new BOMDal();
        
        private string errorMesg = "";
        float decCrossWire = 2.4F;
        int counter = 0;
        int intDrainTypeId = 0;
        string strCurrentLayer = "";
        Boolean ignoreLayer = false;
        string vchShape = "";
        string strPreviousShapeCode = "";
        int NoOfDepth = 0;
        string preDefinedA = "";
        string preDefinedB = "";
        string preDefinedC = "";
        Boolean IsReverse = false;
        Boolean IsIgnoreNext = false;
        decimal decLastEndChainage = 0.0M;
        int loopCount = 0;
        Hashtable hsDrainParamDepth;
        ArrayList alUsedDepth;
        int intConfigCounter = 0;
        decimal decLastSlabEndChainage = 0.0M;
        decimal decLastBaseEndChainage = 0.0M;
        Boolean isFailedMO2Commit = false;
        Boolean isFailedCO2Commit = false;
        string strPreviousLap = "";
        int intPreviousDepthCounter = 0;
        string bitOHVal = "0@0";

        ArrayList alCSStartTopLevel = new ArrayList();
        ArrayList alCSEndTopLevel = new ArrayList();
        ArrayList alCSStartInvertLevel = new ArrayList();
        ArrayList alCSEndInvertLevel = new ArrayList();
        ArrayList alCSDistance = new ArrayList();

        int intProductMarkingCounter = 0;
        private TMCBLL objTMC = new TMCBLL();

        public int intProductValidator { get; set; } 
        public string BndinAdjmntChk = "NO";         

        public DrainBLL()
        {

        }

        public void StartConfiguration(string serverName, WebConfiguration refconfiguration)
        {
            //_serverNameString = serverName;
            //configuration = refconfiguration;
            Factory factory = null;
            DataAccessor accessor = null;
            NameValueCollection properties = null;

            //accessor = new FileAccessor();
            properties = new NameValueCollection();
            properties.Add("Tacton.Configurator.remoteServers", "true");
            //properties.Add("Tacton.Configurator.DrainageServers", _serverNameString);
            //properties.Add("Tacton.Configurator.servers", _serverNameString);

            factory = new Factory(accessor, properties);

            //configuration = new WebConfiguration();
            //configuration = refconfiguration;

            // Create and use a configuration object
            //configuration = new Configuration(factory);
        }

        //by vidya START
        //private string LoadConfigurationFromFile(string strPath)
        //{
        //    string returnSafeState = "";
        //    try
        //    {
        //        configuration.Filter = "groups;parameters;report";
        //        configuration.StartConfiguration(strPath, null);
        //        returnSafeState = configuration.SafeState;
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
        //        configuration.Filter = "groups;parameters;report";
        //        configuration.StartConfiguration(strSafeState);
        //        returnSafeState = configuration.SafeState;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return returnSafeState;
        //}
        //END BY VIDYA
        public bool GenerateDrainProducts(out string strErrorMesg)
        {

            try
            {
                SlabDetailingComponent objInfo = new SlabDetailingComponent();
                intProductMarkingCounter = 0;
                string strGroupMark = _vchGroupMarkName;
                string[] arGM = null;
                int intDrainWidth = 0;
                string vchDrainType = "";

                _intDrainStructureMarkId = intDrainStructureMarkId;
                // define which character is seperating fields
                char[] splitter = { '-' };
                arGM = strGroupMark.Split(splitter);
                if (arGM.Length > 0)
                {
                    intDrainWidth = Convert.ToInt32(arGM[0].ToString().Trim());
                    vchDrainType = arGM[1].ToString().Trim();
                }
                objDetailInfo.intDrainWidth = intDrainWidth;
                objDetailInfo.vchDrainType = vchDrainType;

                objDetailInfo.tntParamSetNumber = _intParameterSet;
                //DataSet dsDrainParam = objDetailDal.DrainProjectParamDetails_Get(objDetailInfo);
                DataSet dsDrainParam = objDetailDal.DrainProjectParamDetails_GetNew(objDetailInfo);
                DataTable dtParamDrainDepth = new DataTable();
                DataTable dtParamDrainWM = new DataTable();
                DataTable dtParamSegments = new DataTable();

                if (dsDrainParam.Tables.Count > 0)
                {
                    dtParamDrainDepth = dsDrainParam.Tables[0];
                    dtParamDrainWM = dsDrainParam.Tables[1];
                    dtParamSegments = dsDrainParam.Tables[2];
                }

                for (int e = 0; e < dtParamDrainDepth.Rows.Count; e++)
                {
                    decLastSlabEndChainage = 0.0M;
                    decLastBaseEndChainage = 0.0M;
                    counter = 0;
                    intDrainTypeId = Convert.ToInt32(dtParamDrainDepth.Rows[e]["sitDrainTypeId"].ToString().Trim());
                    strPreviousLap = "";
                    intPreviousDepthCounter = 0;

                    //strPreviousLayer = "";
                    strCurrentLayer = "";
                    ignoreLayer = false;
                    //intPreviousSLDepth = 0;
                    //intPreviousBLDepth = 0;

                    NoOfDepth = 0;
                    int DptCounter = 0;
                    hsDrainParamDepth = new Hashtable();
                    for (int d = 0; d < dtParamDrainDepth.Columns.Count; d++)
                    {
                        if (dtParamDrainDepth.Columns[d].ColumnName.StartsWith("sitMaxDepth"))
                        {
                            hsDrainParamDepth.Add(DptCounter, dtParamDrainDepth.Rows[e][d].ToString().Trim());
                            DptCounter++;
                            if (dtParamDrainDepth.Rows[e][d].ToString().Trim() != "9999")
                            {
                                NoOfDepth++;
                            }
                        }
                    }
                    for (int a = 0; a < dtParamDrainWM.Rows.Count; a++)
                    {

                        alUsedDepth = new ArrayList();
                        decLastEndChainage = 0.0M;
                        loopCount = 0;
                        string vchLayer = "";
                        //Depth Calculations added for Cascade on 9th March

                        decimal UIEndDepth = Convert.ToDecimal(_endDepth);
                        decimal UIStartDepth = Convert.ToDecimal(_startDepth);
                        int depthCounter = 0;
                        Hashtable hsConsideredDepth = new Hashtable();
                        decimal decParamDepth = 0.0M;
                        for (int h = 1; h <= hsDrainParamDepth.Count; h++)
                        {
                            decParamDepth = 0.0M;
                            decParamDepth = Convert.ToDecimal(Convert.ToInt32(hsDrainParamDepth[h - 1].ToString()) / 1000M);
                            depthCounter++;
                            hsConsideredDepth.Add(h, decParamDepth.ToString());
                            if (decParamDepth >= UIEndDepth)
                            {
                                break;
                            }
                        }
                        if (depthCounter > 0)
                        {
                            for (int i = 1; i <= depthCounter; i++)
                            {
                                decParamDepth = 0.0M;
                                decParamDepth = Convert.ToDecimal(hsConsideredDepth[i].ToString());
                                if (UIStartDepth >= decParamDepth)
                                {
                                    hsConsideredDepth.Remove(i);
                                    depthCounter--;
                                }
                                if (i == 1)
                                {
                                    if (UIEndDepth <= decParamDepth)
                                    {
                                        hsConsideredDepth.Remove(i);
                                        depthCounter--;
                                    }
                                }
                            }
                        }
                        if (depthCounter > 0)
                        {
                            foreach (DictionaryEntry Item in hsConsideredDepth)
                            {
                                double dblDepth = Convert.ToDouble(Item.Value.ToString());
                                if (dblDepth == 9.999)
                                {
                                    depthCounter--;
                                }
                                else
                                {
                                    alUsedDepth.Add(Convert.ToInt32(dblDepth * 1000));
                                }
                            }
                        }


                        //////////////////////////////////////

                        vchLayer = dtParamDrainWM.Rows[a]["vchDrainLayer"].ToString().Trim();
                        //if ((vchLayer == "IL"))// || (vchLayer == "BL")
                        //{
                        if (_isCascade == true)
                        {
                            preDefinedA = "";
                            preDefinedB = "";
                            preDefinedC = "";

                            string strCurrentLap = dtParamDrainWM.Rows[a]["sitLap"].ToString().Trim();
                            if ((strCurrentLap == strPreviousLap) && (intPreviousDepthCounter == depthCounter))
                            {

                            }
                            else
                            {

                                strPreviousLap = dtParamDrainWM.Rows[a]["sitLap"].ToString().Trim();
                                intPreviousDepthCounter = depthCounter;

                                if (Convert.ToInt32(Convert.ToDecimal(flCsCrossLength.ToString())) <= 0)
                                {
                                    decCrossWire = 2.4F;
                                }
                                else
                                {
                                    decCrossWire = flCsCrossLength;
                                }

                                if (Convert.ToInt16(dtParamDrainWM.Rows[a]["bitPredefined"]) > 0)
                                {
                                    int intDrainWMId = Convert.ToInt32(dtParamDrainWM.Rows[a]["intDrainWMId"]);
                                    for (int b = 0; b < dtParamSegments.Rows.Count; b++)
                                    {
                                        if (Convert.ToInt32(dtParamSegments.Rows[b]["intDrainWMId"]) == intDrainWMId)
                                        {
                                            if (dtParamSegments.Rows[b]["chrSegmentParameter"].ToString().Trim() == "A")
                                            {
                                                preDefinedA = dtParamSegments.Rows[b]["intSegmentValue"].ToString().Trim();
                                            }
                                            if (dtParamSegments.Rows[b]["chrSegmentParameter"].ToString().Trim() == "B")
                                            {
                                                preDefinedB = dtParamSegments.Rows[b]["intSegmentValue"].ToString().Trim();
                                            }
                                            if (dtParamSegments.Rows[b]["chrSegmentParameter"].ToString().Trim() == "C")
                                            {
                                                preDefinedC = dtParamSegments.Rows[b]["intSegmentValue"].ToString().Trim();
                                            }
                                        }
                                    }
                                }

                                string strParamA = "";
                                int intLeftWallThick = Convert.ToInt32(Convert.ToDecimal(dtParamDrainWM.Rows[a]["numLeftWallThickness"].ToString()));
                                int intLeftWallFactor = Convert.ToInt32(Convert.ToDecimal(dtParamDrainWM.Rows[a]["numLeftWallFactor"].ToString()));
                                int intRightWallThick = Convert.ToInt32(Convert.ToDecimal(dtParamDrainWM.Rows[a]["numRightWallThickness"].ToString()));
                                int intRightWallFactor = Convert.ToInt32(Convert.ToDecimal(dtParamDrainWM.Rows[a]["numRightWallFactor"].ToString()));
                                int intBaseThick = Convert.ToInt32(Convert.ToDecimal(dtParamDrainWM.Rows[a]["numBaseThickness"].ToString()));


                                if (preDefinedA == "")
                                {
                                    strParamA = dtParamDrainWM.Rows[a]["intParamA"].ToString().Trim();
                                }
                                else
                                {
                                    strParamA = preDefinedA;
                                }

                                if (intConfigCounter > 0)
                                {
                                    //configuration.Factory.Shutdown();
                                }

                                //string strCas_Drn = strTcxPath + "Cascade_Drain.tcx";
                                //LoadConfigurationFromFile(strCas_Drn);
                                intConfigCounter++;

                                //Step: parameter_master_step
                                //Group: project_parameter_group
                                //configuration.SetStep("parameter_master_step");

                                Dictionary<string, string> inputs = new Dictionary<string, string>();
                                objInfo.ClearDictionary();
                                inputs.Add("drainage_top_cover_field", _drainTopCover.ToString());
                                inputs.Add("drainage_bottom_cover_field", _drainBottomCover.ToString());
                                inputs.Add("drainage_outer_cover_field", _drainOuterCover.ToString());
                                inputs.Add("drainage_inner_cover_field", _drainInnerCover.ToString());

                                inputs.Add("drainage_width_field", dtParamDrainDepth.Rows[e]["sitDrainWidth"].ToString().Trim());
                                inputs.Add("drainage_adjust_field", dtParamDrainDepth.Rows[e]["sitAdjust"].ToString().Trim());
                                inputs.Add("drainage_channel_field", dtParamDrainDepth.Rows[e]["sitChannel"].ToString());
                                inputs.Add("drainage_slab_thick_field", dtParamDrainDepth.Rows[e]["sitSlabThickness"].ToString().Trim());

                                if (NoOfDepth == 0)
                                {
                                    inputs.Add("drainage_number_of_depth_field", "0");
                                }
                                else
                                {
                                    if (depthCounter == 0)
                                    {
                                        inputs.Add("drainage_number_of_depth_field", "0");
                                    }
                                    else
                                    {
                                        if (depthCounter == 1)
                                        {
                                            inputs.Add("drainage_number_of_depth_field", "1");
                                            inputs.Add("drainage_max_depth_1_field", alUsedDepth[0].ToString());
                                        }
                                        else if (depthCounter == 2)
                                        {
                                            inputs.Add("drainage_number_of_depth_field", "2");
                                            inputs.Add("drainage_max_depth_1_field", alUsedDepth[0].ToString());
                                            inputs.Add("drainage_max_depth_2_field", alUsedDepth[1].ToString());
                                        }
                                        else if (depthCounter == 3)
                                        {
                                            inputs.Add("drainage_number_of_depth_field", "3");
                                            inputs.Add("drainage_max_depth_1_field", alUsedDepth[0].ToString());
                                            inputs.Add("drainage_max_depth_2_field", alUsedDepth[1].ToString());
                                            inputs.Add("drainage_max_depth_3_field", alUsedDepth[2].ToString());
                                        }
                                        else if (depthCounter == 4)
                                        {
                                            inputs.Add("drainage_number_of_depth_field", "4");
                                            inputs.Add("drainage_max_depth_1_field", alUsedDepth[0].ToString());
                                            inputs.Add("drainage_max_depth_2_field", alUsedDepth[1].ToString());
                                            inputs.Add("drainage_max_depth_3_field", alUsedDepth[2].ToString());
                                            inputs.Add("drainage_max_depth_4_field", alUsedDepth[3].ToString());
                                        }
                                        else if (depthCounter == 5)
                                        {
                                            inputs.Add("drainage_number_of_depth_field", "5");
                                            inputs.Add("drainage_max_depth_1_field", alUsedDepth[0].ToString());
                                            inputs.Add("drainage_max_depth_2_field", alUsedDepth[1].ToString());
                                            inputs.Add("drainage_max_depth_3_field", alUsedDepth[2].ToString());
                                            inputs.Add("drainage_max_depth_4_field", alUsedDepth[3].ToString());
                                            inputs.Add("drainage_max_depth_5_field", alUsedDepth[4].ToString());
                                        }
                                    }
                                }

                                inputs.Add("drainage_cross_wire_length_field", decCrossWire.ToString());
                                inputs.Add("drainage_lapping_field", dtParamDrainWM.Rows[a]["sitLap"].ToString().Trim());
                                inputs.Add("drainage_left_wall_thick_field", intLeftWallThick.ToString());
                                inputs.Add("drainage_left_wall_factor_field", intLeftWallFactor.ToString());
                                inputs.Add("drainage_right_wall_thick_field", intRightWallThick.ToString());
                                inputs.Add("drainage_right_wall_factor_field", intRightWallFactor.ToString());
                                inputs.Add("drainage_base_thick_field", intBaseThick.ToString());


                                //User Inputs
                                inputs.Add("drainage_type_field", "cascade");
                                inputs.Add("drainage_start_chainage_field", _startChainage.ToString());
                                inputs.Add("drainage_end_chainage_field", _endChainage.ToString());
                                inputs.Add("drainage_start_top_level_field", _startTopLevel.ToString());
                                inputs.Add("drainage_end_top_level_field", _endTopLevel.ToString());
                                inputs.Add("drainage_start_invert_level_field", _startInvertLevel.ToString());
                                inputs.Add("drainage_end_invert_level_field", _endInvertLevel.ToString());
                                inputs.Add("drainage_cascade_qty_field", _intCascadeNo.ToString());
                                inputs.Add("drainage_cascade_drop_height_field", _flCsDropHeight.ToString());
                                inputs.Add("drainage_cascade_cross_length_field", _flCsCrossLength.ToString());

                                //Hardcoded value don't know how to calculate - To fix
                                //inputs.Add("level",1.ToString());

                                //objInfo.FillInputDictionary(inputs);

                                //configuration.SetStep("cascade_step");
                                //configuration.Result.GetStep("cascade_step");
                                ////Result CS_resultLayer = configuration.Result;
                                //Group Cascade_Drain_Group = (Group)configuration.Result.RootGroup.SubGroups["cascade_group"];

                                alCSStartTopLevel = new ArrayList();
                                alCSEndTopLevel = new ArrayList();
                                alCSStartInvertLevel = new ArrayList();
                                alCSEndInvertLevel = new ArrayList();
                                alCSDistance = new ArrayList();




                                int level = 1;
                                for (int k = 1; k <= _intCascadeNo; k++)
                                {
                                    level = k;

                                    //drainage_cascade_start_top_level_field = start_top_level-((cascade.level-1)*(start_top_level-end_top_level)/cascade_qty)
                                    double start_top_level_field = _startTopLevel - ((level - 1) * (_startTopLevel - _endTopLevel) / _intCascadeNo);
                                    alCSStartTopLevel.Add(start_top_level_field.ToString());

                                    //drainage_cascade_end_top_level_field = start_top_level-((cascade.level)*(start_top_level-end_top_level)/cascade_qty)
                                    double end_top_level_field = _startTopLevel - ((level) * (_startTopLevel - _endTopLevel) / _intCascadeNo);
                                    alCSEndTopLevel.Add(end_top_level_field.ToString());

                                    //drainage_cascade_start_invert_level_field = start_invert_level-(((start_invert_level-end_invert_level-((shape_drain_type.cascade_drop_height/100000)*(cascade_qty-1)))/(cascade_qty))*cascade.prelevel)-((shape_drain_type.cascade_drop_height/100000)*cascade.prelevel)
                                    double start_invert_level_field = _startInvertLevel - (((_startInvertLevel - _endInvertLevel - (((_flCsDropHeight * 100) / 100000) * (_intCascadeNo - 1))) / (_intCascadeNo)) * (level - 1)) - (((_flCsDropHeight * 100) / 100000) * (level - 1));
                                    alCSStartInvertLevel.Add(start_invert_level_field.ToString());

                                    //drainage_cascade_end_invert_level_field =start_invert_level-((start_invert_level-end_invert_level-((shape_drain_type.cascade_drop_height/100000)*(cascade_qty-1)))/cascade_qty)-((shape_drain_type.cascade_drop_height/100000)*(cascade.level-1))-((cascade.level-1)*((start_invert_level-end_invert_level-((shape_drain_type.cascade_drop_height/100000)*(cascade_qty-1)))/cascade_qty))
                                    double end_invert_level_field = _startInvertLevel - ((_startInvertLevel - _endInvertLevel - (((_flCsDropHeight * 100) / 100000) * (_intCascadeNo - 1))) / _intCascadeNo) - (((_flCsDropHeight * 100) / 100000) * (level - 1)) - ((level - 1) * ((_startInvertLevel - _endInvertLevel - (((_flCsDropHeight * 100) / 100000) * (_intCascadeNo - 1))) / _intCascadeNo));
                                    alCSEndInvertLevel.Add(end_invert_level_field.ToString());

                                    //distance=end_chainage-start_chainage
                                    //drainage_cascade_distance_field = distance/(cascade_qty)
                                    double distance = _endChainage - _startChainage;
                                    double drainage_cascade_distance = distance / (_intCascadeNo);
                                    alCSDistance.Add(drainage_cascade_distance.ToString());
                                }


                                //foreach (Tacton.Configurator.ObjectModel.Parameter p in Cascade_Drain_Group.Parameters)
                                //{
                                //    if (p.Description.ToLower().StartsWith("drainage_cascade_start_top_level_field"))
                                //    {
                                //        alCSStartTopLevel.Add(p.ValueDescription);
                                //    }
                                //    if (p.Description.ToLower().StartsWith("drainage_cascade_end_top_level_field"))
                                //    {
                                //        alCSEndTopLevel.Add(p.ValueDescription);
                                //    }
                                //    if (p.Description.ToLower().StartsWith("drainage_cascade_start_invert_level_field"))
                                //    {
                                //        alCSStartInvertLevel.Add(p.ValueDescription);
                                //    }
                                //    if (p.Description.ToLower().StartsWith("drainage_cascade_end_invert_level_field"))
                                //    {
                                //        alCSEndInvertLevel.Add(p.ValueDescription);
                                //    }
                                //    if (p.Description.ToLower().StartsWith("drainage_cascade_distance_field"))
                                //    {
                                //        alCSDistance.Add(p.ValueDescription);
                                //    }
                                //}
                            }

                            float CS_StartChainage = 0.0F;
                            float CS_EndChainage = 0.0F;
                            float CS_StartTopLevel = 0.0F;
                            float CS_EndTopLevel = 0.0F;
                            float CS_StartInvertLevel = 0.0F;
                            float CS_EndInvertLevel = 0.0F;
                            for (int g = 0; g < alCSDistance.Count; g++)
                            {

                                if (g == 0)
                                {
                                    CS_StartChainage = _startChainage;
                                    CS_EndChainage = _startChainage + (float)Convert.ToDouble(alCSDistance[g].ToString().Trim());
                                }
                                else
                                {
                                    CS_StartChainage = CS_StartChainage + (float)Convert.ToDouble(alCSDistance[g].ToString().Trim());
                                    CS_EndChainage = CS_EndChainage + (float)Convert.ToDouble(alCSDistance[g].ToString().Trim());
                                }

                                CS_StartTopLevel = (float)Convert.ToDouble(alCSStartTopLevel[g].ToString().Trim());
                                CS_EndTopLevel = (float)Convert.ToDouble(alCSEndTopLevel[g].ToString().Trim());
                                CS_StartInvertLevel = (float)Convert.ToDouble(alCSStartInvertLevel[g].ToString().Trim());
                                CS_EndInvertLevel = (float)Convert.ToDouble(alCSEndInvertLevel[g].ToString().Trim());

                                if ((vchLayer == "SL") || (vchLayer == "BL"))
                                {
                                    if (g == 0)
                                    {
                                        GenerateNonCascadeDrain(dtParamDrainDepth, dtParamDrainWM, dtParamSegments, CS_StartChainage, CS_EndChainage, CS_StartTopLevel, CS_EndTopLevel, CS_StartInvertLevel, CS_EndInvertLevel, _isCascade, a, e, NoOfDepth, decCrossWire, g, ref alUsedDepth, depthCounter);
                                    }
                                }
                                else
                                {
                                    GenerateNonCascadeDrain(dtParamDrainDepth, dtParamDrainWM, dtParamSegments, CS_StartChainage, CS_EndChainage, CS_StartTopLevel, CS_EndTopLevel, CS_StartInvertLevel, CS_EndInvertLevel, _isCascade, a, e, NoOfDepth, decCrossWire, g, ref alUsedDepth, depthCounter);
                                }
                            }

                        }
                        else
                        {

                            if (vchLayer == "OU")
                            {
                                if (Convert.ToInt32(Convert.ToDecimal(_drainOuterCrossWire.ToString())) <= 0)
                                {
                                    decCrossWire = 2.4F;
                                }
                                else
                                {
                                    decCrossWire = _drainOuterCrossWire;
                                }
                            }
                            else if ((vchLayer == "IL") || (vchLayer == "IR"))
                            {
                                if (Convert.ToInt32(Convert.ToDecimal(_drainInnerCrossWire.ToString())) <= 0)
                                {
                                    decCrossWire = 2.4F;
                                }
                                else
                                {
                                    decCrossWire = _drainInnerCrossWire;
                                }
                            }
                            else if (vchLayer == "SL")
                            {
                                if (Convert.ToInt32(Convert.ToDecimal(_drainSlabCrossWire.ToString())) <= 0)
                                {
                                    decCrossWire = 2.4F;
                                }
                                else
                                {
                                    decCrossWire = _drainSlabCrossWire;
                                }
                            }
                            else if (vchLayer == "BL")
                            {
                                if (Convert.ToInt32(Convert.ToDecimal(_drainBaseCrossWire.ToString())) <= 0)
                                {
                                    decCrossWire = 2.4F;
                                }
                                else
                                {
                                    decCrossWire = _drainBaseCrossWire;
                                }
                            }
                            else
                            {
                                decCrossWire = 2.4F;
                            }
                            int g = 0;

                            GenerateNonCascadeDrain(dtParamDrainDepth, dtParamDrainWM, dtParamSegments, _startChainage, _endChainage, _startTopLevel, _endTopLevel, _startInvertLevel, _endInvertLevel, _isCascade, a, e, NoOfDepth, decCrossWire, g, ref alUsedDepth, depthCounter);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strErrorMesg = ex.Message.ToString().Trim();
                return false;
                //throw ex;//ErrorHandler.RaiseError(ex, strLogError, string.Empty);

            }
            finally
            {
                alCSStartTopLevel = null;
                alCSEndTopLevel = null;
                alCSStartInvertLevel = null;
                alCSEndInvertLevel = null;
                alCSDistance = null;
                objBOMDal = null;
                objBOMInfo = null;
                objAdminDal = null;
                objAdminInfo = null;

            }
            if (intProductMarkingCounter > 0)
            {
                strErrorMesg = errorMesg;
                return true;
            }
            else
            {
                strErrorMesg = errorMesg;
                return false;
            }
        }
        public bool GenerateNonCascadeDrain(DataTable dtParamDrainDepth, DataTable dtParamDrainWM, DataTable dtParamSegments, float flStartChainage, float flEndChainage, float flStartTopLevel, float flEndTopLevel, float flStartInvertLevel, float flEndInvertLevel, bool blnCascade, int a, int e, int NoOfDepth, float decCrossWire, int g, ref ArrayList alUsedDepth, int depthCounter)
        {

            try
            {
                SlabDetailingComponent objInfo = new SlabDetailingComponent();
                Dictionary<string, string> inputs = new Dictionary<string, string>();
                string convertedString = string.Empty;
                string convertedString_drainage1 = string.Empty;
                Int16 intProductTypeId = 0;
                int intMWPrdLength = 0;
                int intCWPrdLength = 0;
                if (Convert.ToInt32(dtParamDrainWM.Rows[a]["sitDrainTypeId"].ToString().Trim()) == intDrainTypeId)
                {
                    preDefinedA = "";
                    preDefinedB = "";
                    preDefinedC = "";
                    vchShape = "";
                    int intEvenMO1 = 0;
                    int intEvenMO2 = 0;
                    int intEvenCO1 = 0;
                    int intEvenCO2 = 0;
                    int intOddMO1 = 0;
                    int intOddMO2 = 0;
                    int intOddCO1 = 0;
                    int intOddCO2 = 0;
                    int intMWSpacing = 0;
                    int intCWSpacing = 0;
                    int intBaseThickness = 0;
                    int intQty = 0;

                    DataSet dsOverHangs = null;
                    strCurrentLayer = dtParamDrainWM.Rows[a]["vchDrainLayer"].ToString().Trim();
                    vchShape = dtParamDrainWM.Rows[a]["chrShapeCode"].ToString().Trim();
                    if(vchShape=="1M1")
                    {
                        var abbsabas = "";
                    }
                    intQty = Convert.ToInt32(dtParamDrainWM.Rows[a]["intQty"].ToString().Trim());
                    intMWSpacing = Convert.ToInt32(dtParamDrainWM.Rows[a]["intMWSpace"].ToString().Trim());
                    intCWSpacing = Convert.ToInt32(dtParamDrainWM.Rows[a]["intCWSpace"].ToString().Trim());
                    intBaseThickness = Convert.ToInt32(Convert.ToDecimal(dtParamDrainWM.Rows[a]["numBaseThickness"].ToString().Trim()));
                    bitOHVal = dtParamDrainWM.Rows[a]["bitOHDtls"].ToString().Trim();
                    if (vchShape != "")
                    {
                        dsOverHangs = new DataSet();
                        objDetailInfo.tntParamSetNumber = _intParameterSet;
                        objDetailInfo.chrShapeCode = vchShape;
                        objDetailInfo.intMWSpacing = intMWSpacing;
                        objDetailInfo.intCWSpacing = intCWSpacing;
                        dsOverHangs = objDetailDal.DrainOverHangs_Get(objDetailInfo);
                        if (dsOverHangs.Tables.Count > 0)
                        {
                            intEvenMO1 = Convert.ToInt32(dsOverHangs.Tables[0].Rows[0]["EvenMO1"].ToString().Trim());
                            intEvenMO2 = Convert.ToInt32(dsOverHangs.Tables[0].Rows[0]["EvenMO2"].ToString().Trim());
                            intEvenCO1 = Convert.ToInt32(dsOverHangs.Tables[0].Rows[0]["EvenCO1"].ToString().Trim());
                            intEvenCO2 = Convert.ToInt32(dsOverHangs.Tables[0].Rows[0]["EvenCO2"].ToString().Trim());
                            intOddMO1 = Convert.ToInt32(dsOverHangs.Tables[0].Rows[0]["OddMO1"].ToString().Trim());
                            intOddMO2 = Convert.ToInt32(dsOverHangs.Tables[0].Rows[0]["OddMO2"].ToString().Trim());
                            intOddCO1 = Convert.ToInt32(dsOverHangs.Tables[0].Rows[0]["OddCO1"].ToString().Trim());
                            intOddCO2 = Convert.ToInt32(dsOverHangs.Tables[0].Rows[0]["OddCO2"].ToString().Trim());



                            //Start Drain Overhang Logic
                            int llap = int.Parse(dtParamDrainWM.Rows[a]["sitLap"].ToString().Trim());
                            if (llap != 0)
                            {
                                if (llap % 100 != 0)
                                {
                                    llap = llap - (llap % 100);
                                }
                            }
                            int CO1_spacing = intMWSpacing / 2;
                            int CO2_spacing = intMWSpacing / 2;
                            if (CO1_spacing != 0)
                            {
                                if (CO1_spacing % 5 != 0)
                                {
                                    CO1_spacing = CO1_spacing + 5 - (CO1_spacing % 5);
                                    CO2_spacing = intMWSpacing - CO1_spacing;
                                }
                            }
                            intEvenCO1 = llap + CO1_spacing;
                            intEvenCO2 = CO2_spacing;
                            intOddCO1 = llap + CO1_spacing;
                            intOddCO2 = CO2_spacing + (intMWSpacing == 200 ? 100 : 0);
                            //END Drain Overhang Logic
                        }
                        dsOverHangs = null;
                    }
                    if (NoOfDepth > 0)
                    {
                        if (depthCounter > 0)
                        {
                            ignoreLayer = false;
                        }
                        else
                        {
                            ArrayList alDepths = new ArrayList();
                            ArrayList alIgnoreDepths = new ArrayList();
                            for (int k = 0; k < hsDrainParamDepth.Count; k++)
                            {
                                string strDepth = "";
                                strDepth = hsDrainParamDepth[k].ToString();
                                if (strDepth != "9999")
                                {
                                    alDepths.Add(strDepth);
                                }
                            }
                            for (int l = 0; l < alDepths.Count; l++)
                            {
                                decimal decDepth = 0.0M;
                                decDepth = Convert.ToDecimal(Convert.ToInt32(alDepths[l].ToString()) / 1000M);
                                decimal UIEndDepth = Convert.ToDecimal(_endDepth);
                                decimal UIStartDepth = Convert.ToDecimal(_startDepth);
                                if (UIStartDepth >= decDepth)
                                {
                                    alIgnoreDepths.Add(l + 1);
                                }
                                else
                                {
                                    ignoreLayer = false;
                                }
                                int currentDepth = Convert.ToInt32(dtParamDrainWM.Rows[a]["sitMaxDepth"].ToString().Trim());
                                if (alIgnoreDepths.Count > 0)
                                {
                                    if (alIgnoreDepths.Contains(currentDepth))
                                    {
                                        ignoreLayer = true;
                                    }
                                    else
                                    {
                                        ignoreLayer = false;
                                    }
                                }
                                else
                                {
                                    if (currentDepth > 1)
                                    {
                                        ignoreLayer = true;
                                    }
                                    else
                                    {
                                        ignoreLayer = false;
                                    }
                                }
                            }

                        }
                    }


                    if (!ignoreLayer)
                    {
                        if (Convert.ToInt16(dtParamDrainWM.Rows[a]["bitPredefined"]) > 0)
                        {
                            int intDrainWMId = Convert.ToInt32(dtParamDrainWM.Rows[a]["intDrainWMId"]);
                            for (int b = 0; b < dtParamSegments.Rows.Count; b++)
                            {
                                if (Convert.ToInt32(dtParamSegments.Rows[b]["intDrainWMId"]) == intDrainWMId)
                                {
                                    if (dtParamSegments.Rows[b]["chrSegmentParameter"].ToString().Trim() == "A")
                                    {
                                        preDefinedA = dtParamSegments.Rows[b]["intSegmentValue"].ToString().Trim();
                                    }
                                    if (dtParamSegments.Rows[b]["chrSegmentParameter"].ToString().Trim() == "B")
                                    {
                                        preDefinedB = dtParamSegments.Rows[b]["intSegmentValue"].ToString().Trim();
                                    }
                                    if (dtParamSegments.Rows[b]["chrSegmentParameter"].ToString().Trim() == "C")
                                    {
                                        preDefinedC = dtParamSegments.Rows[b]["intSegmentValue"].ToString().Trim();
                                    }
                                }
                            }
                        }

                        if (intConfigCounter > 0)
                        {
                            //configuration.Factory.Shutdown();
                        }
                        else
                        {
                            if (intConfigCounter > 0)
                            {
                                //configuration.Factory.Shutdown();
                            }
                        }
                        //string strCas_Drn = strTcxPath + "Cascade_Drain.tcx";
                        //LoadConfigurationFromFile(strCas_Drn);
                        intConfigCounter++;
                        //LoadConfiguration(@"D:\DrainModule\DrainBLL\Data\Cascade_Drain.tcx");

                        //Step: parameter_master_step
                        //Group: project_parameter_group
                        //configuration.SetStep("parameter_master_step");

                        objInfo.ClearDictionary();
                        inputs.Add("top_cover", _drainTopCover.ToString());
                        inputs.Add("bottom_cover", _drainBottomCover.ToString());
                        inputs.Add("outer_cover", _drainOuterCover.ToString());
                        inputs.Add("inner_cover", _drainInnerCover.ToString());
                        inputs.Add("drain_width", dtParamDrainDepth.Rows[e]["sitDrainWidth"].ToString().Trim());
                        inputs.Add("adjust", dtParamDrainDepth.Rows[e]["sitAdjust"].ToString().Trim());
                        inputs.Add("channel", dtParamDrainDepth.Rows[e]["sitChannel"].ToString());
                        inputs.Add("slab_thick", dtParamDrainDepth.Rows[e]["sitSlabThickness"].ToString().Trim());

                        //NoOfDepth=Convert.ToInt32 (dtParamDrainWM.Rows[a]["sitMaxDepth"].ToString ().Trim());//Fix it
                        if (NoOfDepth == 0)
                        {
                            //inputs.Add("drainage_number_of_depth_field", "0");
                            inputs.Add("type", "0");
                        }
                        else
                        {
                            if (depthCounter == 0)
                            {
                                // inputs.Add("drainage_number_of_depth_field", "0");
                                inputs.Add("type", "0");
                            }
                            else
                            {
                                if (depthCounter == 1)
                                {
                                    //inputs.Add("drainage_number_of_depth_field", "1");
                                    inputs.Add("type", "1");
                                    inputs.Add("max_depth_1", alUsedDepth[0].ToString());
                                }
                                else if (depthCounter == 2)
                                {
                                    //inputs.Add("drainage_number_of_depth_field", "2");
                                    inputs.Add("type", "2");
                                    inputs.Add("max_depth_1", alUsedDepth[0].ToString());
                                    inputs.Add("max_depth_2", alUsedDepth[1].ToString());
                                }
                                else if (depthCounter == 3)
                                {
                                    //inputs.Add("drainage_number_of_depth_field", "3");
                                    inputs.Add("type", "3");
                                    inputs.Add("max_depth_1", alUsedDepth[0].ToString());
                                    inputs.Add("max_depth_2", alUsedDepth[1].ToString());
                                    inputs.Add("max_depth_3", alUsedDepth[2].ToString());
                                }
                                else if (depthCounter == 4)
                                {
                                    //inputs.Add("drainage_number_of_depth_field", "4");
                                    inputs.Add("type", "4");
                                    inputs.Add("max_depth_1", alUsedDepth[0].ToString());
                                    inputs.Add("max_depth_2", alUsedDepth[1].ToString());
                                    inputs.Add("max_depth_3", alUsedDepth[2].ToString());
                                    inputs.Add("max_depth_4", alUsedDepth[3].ToString());
                                }
                                else if (depthCounter == 5)
                                {
                                    //inputs.Add("drainage_number_of_depth_field", "5");
                                    inputs.Add("type", "5");
                                    inputs.Add("max_depth_1", alUsedDepth[0].ToString());
                                    inputs.Add("max_depth_2", alUsedDepth[1].ToString());
                                    inputs.Add("max_depth_3", alUsedDepth[2].ToString());
                                    inputs.Add("max_depth_4", alUsedDepth[3].ToString());
                                    inputs.Add("max_depth_5", alUsedDepth[4].ToString());
                                }
                            }

                        }

                        //Layer Specific
                        string vchLayer = "";
                        string strParamA = "";
                        int intLeftWallThick = Convert.ToInt32(Convert.ToDecimal(dtParamDrainWM.Rows[a]["numLeftWallThickness"].ToString()));
                        int intLeftWallFactor = Convert.ToInt32(Convert.ToDecimal(dtParamDrainWM.Rows[a]["numLeftWallFactor"].ToString()));
                        int intRightWallThick = Convert.ToInt32(Convert.ToDecimal(dtParamDrainWM.Rows[a]["numRightWallThickness"].ToString()));
                        int intRightWallFactor = Convert.ToInt32(Convert.ToDecimal(dtParamDrainWM.Rows[a]["numRightWallFactor"].ToString()));
                        int intBaseThick = Convert.ToInt32(Convert.ToDecimal(dtParamDrainWM.Rows[a]["numBaseThickness"].ToString()));
                        vchLayer = dtParamDrainWM.Rows[a]["vchDrainLayer"].ToString().Trim();

                        if (preDefinedA == "")
                        {
                            strParamA = dtParamDrainWM.Rows[a]["intParamA"].ToString().Trim();
                        }
                        else
                        {
                            strParamA = preDefinedA;
                        }

                        inputs.Add("cross_wire_length", decCrossWire.ToString());
                        inputs.Add("lapping", dtParamDrainWM.Rows[a]["sitLap"].ToString().Trim());

                        inputs.Add("left_wall_thick", intLeftWallThick.ToString());
                        inputs.Add("left_wall_factor", intLeftWallFactor.ToString());
                        inputs.Add("right_wall_thick", intRightWallThick.ToString());
                        inputs.Add("drain_right_wall_factor", intRightWallFactor.ToString());
                        inputs.Add("base_thick", intBaseThick.ToString());

                        //User Inputs-Currently working
                        inputs.Add("drain_type", "noncascade");
                        inputs.Add("start_chainage", flStartChainage.ToString());
                        inputs.Add("end_chainage", flEndChainage.ToString());
                        inputs.Add("start_top_level", flStartTopLevel.ToString());
                        inputs.Add("end_top_level", flEndTopLevel.ToString());
                        inputs.Add("start_invert_level", flStartInvertLevel.ToString());
                        inputs.Add("end_invert_level", flEndInvertLevel.ToString());
                        //configuration.UnCommit("drainage_layer_field");
                        //configuration.UnCommit("drainage_shape_code_field");
                        inputs.Add("layer", vchLayer);
                        inputs.Add("shape_code", vchShape);
                        if ((vchShape == "1M1") || (vchShape == "1MR1"))
                        {
                            //configuration.UnCommit("drainage_inner_left_parameter_a_field");
                            inputs.Add("inner_left_parameter_a", strParamA);
                        }

                        //drainageSafeState = configuration.SafeState;

                        // Get the result                
                        //configuration.SetStep("result_step");
                        //configuration.Result.GetStep("result_step");

                        Dictionary<string, string> Cascade_Drain_Result = new Dictionary<string, string>();
                        objInfo.FillInputDictionary(inputs);
                        Cascade_Drain_Result = objInfo.GetDrainProductMarkingFormulae("Drain", vchShape, vchLayer);

                        // Group Cascade_Drain_Result = (Group)configuration.Result.RootGroup.SubGroups["result_group"];

                        ArrayList alStartDepth = new ArrayList();
                        ArrayList alEndDepth = new ArrayList();
                        ArrayList alChainageDistance = new ArrayList();
                        ArrayList alStartMainLength = new ArrayList();
                        ArrayList alEndMainLength = new ArrayList();
                        ArrayList alTotalMeshQty = new ArrayList();

                        foreach (KeyValuePair<string, string> item in Cascade_Drain_Result)
                        {
                            if (item.Key.ToLower().EndsWith("start_main_length"))
                            {
                                alStartMainLength.Add(item.Value);
                            }
                            if (item.Key.ToLower().EndsWith("end_main_length"))
                            {
                                alEndMainLength.Add(item.Value);
                            }
                            if (item.Key.ToLower().StartsWith("chainage_distance"))
                            {
                                alChainageDistance.Add(item.Value);
                            }
                            if (item.Key.ToLower().EndsWith("start_depth"))
                            {
                                alStartDepth.Add(item.Value);
                            }
                            if (item.Key.ToLower().EndsWith("end_depth"))
                            {
                                alEndDepth.Add(item.Value);
                            }
                            if (item.Key.ToLower().EndsWith("total_mesh_qty"))
                            {
                                alTotalMeshQty.Add(item.Value);
                            }
                        }

                        if (IsCascade != true)
                        {
                            decLastEndChainage = 0.0M;
                            loopCount = 0;
                        }
                        decimal decLoopEndChainage = 0.0M;
                        int intChainageCounter = 0;

                        for (int i = 0; i < alChainageDistance.Count; i++)
                        {

                            if ((alChainageDistance[i].ToString() != "inf") & (alChainageDistance[i].ToString() != "-inf"))
                            {

                                if (Convert.ToInt32(Convert.ToDecimal(alChainageDistance[i].ToString())) > 0)
                                {
                                    decimal decStartDepth = 0.0M;
                                    decimal decEndDepth = 0.0M;
                                    decStartDepth = Convert.ToDecimal(alStartDepth[i].ToString());
                                    decEndDepth = Convert.ToDecimal(alEndDepth[i].ToString());
                                    if (decStartDepth != decEndDepth)
                                    {
                                        intChainageCounter++;
                                        decimal decMeshQty = 0.0M;
                                        int TotMeshQty = 0;

                                        if (alTotalMeshQty.Count > 0)
                                        {
                                            decMeshQty = Convert.ToDecimal(Math.Ceiling(Convert.ToDecimal(alTotalMeshQty[i])));
                                            TotMeshQty = TotMeshQty + Convert.ToInt32(decMeshQty);
                                        }
                                        //Load the second TCX                
                                        //configuration.Factory.Reset();

                                        if ((vchLayer == "OU") || (vchLayer == "IL") || (vchLayer == "IR"))
                                        {
                                            int intMWDia = 0;
                                            int intCWDia = 0;
                                            intMWDia = Convert.ToInt32(Convert.ToDecimal(dtParamDrainWM.Rows[a]["decMWDiameter"].ToString()));
                                            intCWDia = Convert.ToInt32(Convert.ToDecimal(dtParamDrainWM.Rows[a]["decCWDiameter"].ToString()));
                                            int intCurrentDepth = 0;
                                            intCurrentDepth = Convert.ToInt32(dtParamDrainWM.Rows[a]["sitMaxDepth"].ToString().Trim());
                                            Boolean ignoreLyr = false;
                                            {
                                                if (depthCounter == 0)
                                                {
                                                    ignoreLyr = false;
                                                }
                                                else
                                                {
                                                    if (intChainageCounter == intCurrentDepth)
                                                    {
                                                        ignoreLyr = false;
                                                    }
                                                    else
                                                    {
                                                        ignoreLyr = true;
                                                    }
                                                }
                                            }
                                            if (!ignoreLyr)
                                            {
                                                IsReverse = false;
                                                if (intConfigCounter > 0)
                                                {
                                                    //configuration.Factory.Shutdown();
                                                }
                                                //string strDrng1 = strTcxPath + "Drainage1.tcx";
                                                //LoadConfigurationFromFile(strDrng1);
                                                intConfigCounter++;
                                                //LoadConfiguration(@"D:\DrainModule\DrainBLL\Data\Drainage1.tcx");


                                                objInfo.ClearDictionary();

                                                inputs.Add("drainage_layer_field", vchLayer);
                                                inputs.Add("drainage_shape_code_field", vchShape);

                                                inputs.Add("level", (i).ToString()); //To Fix

                                                decimal prevEndChainage = 0.0M;
                                                if (i == 0)
                                                {
                                                    prevEndChainage = Convert.ToDecimal(flStartChainage.ToString());
                                                    prevEndChainage = decimal.Round(prevEndChainage, 2);
                                                    inputs.Add("drainage_previous_end_chainage_field", prevEndChainage.ToString());
                                                }
                                                else
                                                {

                                                    //prevEndChainage = Convert.ToDecimal(alChainageDistance[i - 1].ToString());
                                                    prevEndChainage = decLastEndChainage;
                                                    prevEndChainage = decimal.Round(prevEndChainage, 2);
                                                    inputs.Add("drainage_previous_end_chainage_field", prevEndChainage.ToString());
                                                }
                                                decimal startMainLen = 0.0M;
                                                decimal endMainLen = 0.0M;
                                                startMainLen = Convert.ToDecimal(alStartMainLength[i].ToString());
                                                //startMainLen = decimal.Round(startMainLen, 2);
                                                endMainLen = Convert.ToDecimal(alEndMainLength[i].ToString());
                                                //endMainLen = decimal.Round(endMainLen, 2);

                                                if (startMainLen < endMainLen)
                                                {
                                                    IsReverse = false;
                                                    string strStartMainLen = "";
                                                    string strEndMainLen = "";
                                                    strStartMainLen = RoundToTwo(startMainLen.ToString());
                                                    strEndMainLen = RoundToTwo(endMainLen.ToString());
                                                    inputs.Add("drainage_start_main_length_field", strStartMainLen);
                                                    inputs.Add("drainage_end_main_length_field", strEndMainLen);
                                                    if ((vchLayer == "IL") || (vchLayer == "IR"))
                                                    {
                                                        if (vchShape.Trim() == "2MR1")
                                                        {
                                                            inputs.Add("drainage_start_main_length_round_field", (decimal.Round(startMainLen, 1)).ToString());
                                                            inputs.Add("drainage_end_main_length_round_field", (decimal.Round(endMainLen, 1)).ToString());
                                                        }
                                                        else
                                                        {
                                                            string strStartLen = RoundLength(strStartMainLen);
                                                            string strEndLen = RoundLength(strEndMainLen);
                                                            inputs.Add("drainage_start_main_length_round_field", strStartLen.Trim());
                                                            inputs.Add("drainage_end_main_length_round_field", strEndLen.Trim());
                                                        }
                                                    }
                                                    else
                                                    {
                                                        inputs.Add("drainage_start_main_length_round_field", (decimal.Round(startMainLen, 1)).ToString());
                                                        inputs.Add("drainage_end_main_length_round_field", (decimal.Round(endMainLen, 1)).ToString());
                                                    }
                                                }
                                                else
                                                {
                                                    IsReverse = true;
                                                    string strStartMainLen = "";
                                                    string strEndMainLen = "";
                                                    strStartMainLen = RoundToTwo(startMainLen.ToString());
                                                    strEndMainLen = RoundToTwo(endMainLen.ToString());
                                                    inputs.Add("drainage_start_main_length_field", strEndMainLen);
                                                    inputs.Add("drainage_end_main_length_field", strStartMainLen);
                                                    if ((vchLayer == "IL") || (vchLayer == "IR"))
                                                    {
                                                        if (vchShape.Trim() == "2MR1")
                                                        {
                                                            inputs.Add("drainage_start_main_length_round_field", (decimal.Round(endMainLen, 1)).ToString());
                                                            inputs.Add("drainage_end_main_length_round_field", (decimal.Round(startMainLen, 1)).ToString());
                                                        }
                                                        else
                                                        {
                                                            string strStartLen = RoundLength(strStartMainLen);
                                                            string strEndLen = RoundLength(strEndMainLen);
                                                            inputs.Add("drainage_start_main_length_round_field", strEndLen.Trim());
                                                            inputs.Add("drainage_end_main_length_round_field", strStartLen.Trim());
                                                        }
                                                    }
                                                    else
                                                    {
                                                        inputs.Add("drainage_start_main_length_round_field", (decimal.Round(endMainLen, 1)).ToString());
                                                        inputs.Add("drainage_end_main_length_round_field", (decimal.Round(startMainLen, 1)).ToString());
                                                    }
                                                }

                                                int meshQty = 0;
                                                //decimal mesh = Convert.ToDecimal(alTotalMeshQty[i].ToString());
                                                decimal mesh = Math.Ceiling(Convert.ToDecimal(alTotalMeshQty[i].ToString()));
                                                meshQty = Convert.ToInt32(mesh);
                                                inputs.Add("drainage_total_mesh_qty_field", meshQty.ToString());
                                                inputs.Add("drainage_cross_wire_length_field", decCrossWire.ToString());
                                                inputs.Add("drainage_lapping_field", dtParamDrainWM.Rows[a]["sitLap"].ToString().Trim());
                                                inputs.Add("drainage_left_wall_thick_field", intLeftWallThick.ToString());
                                                inputs.Add("drainage_width_field", dtParamDrainWM.Rows[a]["sitDrainWidth"].ToString());
                                                inputs.Add("drainage_right_wall_thick_field", intRightWallThick.ToString());
                                                inputs.Add("drainage_outer_cover_field", _drainOuterCover.ToString());
                                                inputs.Add("drainage_inner_cover_field", _drainInnerCover.ToString());
                                                inputs.Add("drainage_left_wall_factor_field", intLeftWallFactor.ToString());
                                                inputs.Add("drainage_right_wall_factor_field", intRightWallFactor.ToString());
                                                if ((vchShape == "1M1") || (vchShape == "1MR1"))
                                                {
                                                    //configuration.UnCommit("drainage_inner_left_parameter_a_field");
                                                    inputs.Add("drainage_inner_left_parameter_a_field", strParamA);
                                                }
                                                //configuration.Result.GetStep("result_step");
                                                //configuration.SetStep("result_step");
                                                //Result resultLayer = configuration.Result;
                                                double increment = 0.0;
                                                if ((vchShape == "1M1") || (vchShape == "1MR1") || (vchShape == "F"))
                                                {
                                                    increment = 0.05;
                                                }
                                                else if ((vchShape == "2M1") || (vchShape == "2MR1"))
                                                {
                                                    increment = 0.1;

                                                }

                                                int product_qty = 0;
                                                if ((vchShape == "F") || (vchShape == "1M1") || (vchShape == "1MR1"))
                                                {
                                                    product_qty = 2* intQty;
                                                }
                                                else if ((vchShape == "2M1") || (vchShape == "2MR1"))
                                                {
                                                    product_qty = intQty;
                                                }
                                                int level_val = Int32.Parse(inputs["level"]);
                                                int no_of_instance = 0;
                                                //no_of_instance~=round((end_main_length_round-start_main_length_round)/increment)+1
                                                if (inputs.ContainsKey("drainage_start_main_length_round_field") && inputs.ContainsKey("drainage_end_main_length_round_field"))
                                                {
                                                    double start_main_length_round_value = Double.Parse(inputs["drainage_start_main_length_round_field"]);
                                                    double end_main_length_round_value = Double.Parse(inputs["drainage_end_main_length_round_field"]);
                                                    no_of_instance = Convert.ToInt32((end_main_length_round_value - start_main_length_round_value) / increment) + 1;
                                                }



                                                Dictionary<string, Double> output_drainage1 = new Dictionary<string, Double>();
                                                for (int m = 1; m <= no_of_instance; m++)
                                                {
                                                    level_val = m;
                                                    //1.outer_chainage.mainlength~=start_main_length_round+((outer_chainage.level-1)*increment)
                                                    //2.outer_chainage.mainlength1~=start_main_length_round+((outer_chainage.prelevel-1)*increment)
                                                    double mainlength_value = 0.0;
                                                    double mainlength1_value = 0.0;
                                                    double current_accumulated_qty_value = 0.0;
                                                    double current_accumulated_qty1_value = 0.0;
                                                    double actual_qty_value = 0.0;
                                                    double final_actual_qty = 0.0;
                                                    double start_chainage_value = 0.0;
                                                    double end_chainage_value = 0.0;

                                                    if (inputs.ContainsKey("drainage_start_main_length_round_field"))
                                                    {
                                                        double start_main_length_round_value = Double.Parse(inputs["drainage_start_main_length_round_field"]);
                                                        mainlength_value = start_main_length_round_value + (((level_val - 1)) * increment);
                                                        mainlength1_value = start_main_length_round_value + (((level_val - 1) - 1) * increment);
                                                    

                                                    }
                                                    output_drainage1.Add("drainage_chainage_mainlength" + "_" + m, mainlength_value);

                                                    //outer_chainage.current_accumulated_qty~=floor(((total_mesh_qty*(outer_chainage.mainlength-start_main_length)))/(end_main_length-start_main_length))
                                                    //outer_chainage.actual_qty~=floor(((total_mesh_qty*(outer_chainage.mainlength-start_main_length)))/(end_main_length-start_main_length))-((floor(((total_mesh_qty*(outer_chainage.mainlength1-start_main_length)))/(end_main_length-start_main_length))+abs(floor(((total_mesh_qty*(outer_chainage.mainlength1-start_main_length)))/(end_main_length-start_main_length))))/2)
                                                    //outer_chainage.current_accumulated_qty_1~=floor(((total_mesh_qty*(outer_chainage.mainlength1-start_main_length)))/(end_main_length-start_main_length))
                                                    //no_of_instance~=round((end_main_length_round-start_main_length_round)/increment)+1


                                                    if (inputs.ContainsKey("drainage_start_main_length_field") && inputs.ContainsKey("drainage_end_main_length_field"))
                                                    {

                                                        double drainage_start_main_length_value = Double.Parse(inputs["drainage_start_main_length_field"]);
                                                        double drainage_end_main_length_value = Double.Parse(inputs["drainage_end_main_length_field"]);


                                                        Double drainage_start_main_length_round_value = Double.Parse(inputs["drainage_start_main_length_round_field"]);
                                                        Double drainage_end_main_length_round_value = Double.Parse(inputs["drainage_end_main_length_round_field"]);


                                                        Double no_of_instance_value = Math.Round((drainage_end_main_length_round_value - drainage_start_main_length_round_value) / increment) + 1;

                                                        current_accumulated_qty_value = Math.Floor(((meshQty * (mainlength_value - drainage_start_main_length_value))) / (drainage_end_main_length_value - drainage_start_main_length_value));
                                                        //current_accumulated_qty_value = Math.Floor(((meshQty * (mainlength_value - drainage_start_main_length_value))) / (drainage_end_main_length_value - drainage_start_main_length_round_value));
                                                        current_accumulated_qty1_value = Math.Floor(((meshQty * (mainlength1_value - drainage_start_main_length_value))) / (drainage_end_main_length_value - drainage_start_main_length_value));
                                                        if (level_val == Convert.ToInt32(no_of_instance_value) && Convert.ToInt32(current_accumulated_qty_value) <= meshQty)
                                                        {
                                                            current_accumulated_qty_value = meshQty;
                                                        }

                                                        if (level_val == 1)
                                                        {
                                                            actual_qty_value = current_accumulated_qty_value;
                                                        }
                                                        else if (level_val == Convert.ToInt32(no_of_instance_value))
                                                        {
                                                            //outer_chainage.actual_qty=total_mesh_qty-((outer_chainage.current_accumulated_qty_1+abs(outer_chainage.current_accumulated_qty_1))/2)
                                                            actual_qty_value = meshQty - ((current_accumulated_qty1_value + Math.Abs(current_accumulated_qty1_value)) / 2);
                                                        }
                                                        else
                                                        {
                                                            actual_qty_value = Math.Floor(((meshQty * (mainlength_value - drainage_start_main_length_value))) / (drainage_end_main_length_value - drainage_start_main_length_value)) - ((Math.Floor(((meshQty * (mainlength1_value - drainage_start_main_length_value))) / (drainage_end_main_length_value - drainage_start_main_length_value)) + Math.Abs(Math.Floor(((meshQty * (mainlength1_value - drainage_start_main_length_value))) / (drainage_end_main_length_value - drainage_start_main_length_value)))) / 2);
                                                        }

                                                    }
                                                    output_drainage1.Add("drainage_chainage_total_current_accumulated_qty" + "_" + m, current_accumulated_qty_value);
                                                    output_drainage1.Add("drainage_chainage_actual_qty_field" + "_" + m, actual_qty_value);
                                                    //prelevel=level-1
                                                    //outer_chainage.final_actual_qty=product_qty*outer_chainage.actual_qty
                                                    final_actual_qty = product_qty * actual_qty_value;
                                                    output_drainage1.Add("drainage_chainage_final_actual_qty" + "_" + m, final_actual_qty);

                                                    //outer_chainage.start_chainage~=((cross_wire_length-(lapping/1000))*round(outer_chainage.current_accumulated_qty_1))+previous_end_chainage
                                                    //outer_chainage.end_chainage~=((cross_wire_length-(lapping/1000))*round(outer_chainage.current_accumulated_qty))+previous_end_chainage
                                                    if (inputs.ContainsKey("drainage_cross_wire_length_field") && inputs.ContainsKey("drainage_lapping_field") && inputs.ContainsKey("drainage_previous_end_chainage_field"))
                                                    {
                                                        double drainage_cross_wire_length_value = Double.Parse(inputs["drainage_cross_wire_length_field"]);
                                                        double drainage_lapping_value = Double.Parse(inputs["drainage_lapping_field"]);
                                                        double drainage_previous_end_chainage_value = Double.Parse(inputs["drainage_previous_end_chainage_field"]);
                                                        if (level_val == 1)
                                                        {
                                                            start_chainage_value = drainage_previous_end_chainage_value;
                                                        }
                                                        else
                                                        {
                                                            start_chainage_value = ((drainage_cross_wire_length_value - (drainage_lapping_value / 1000)) * Math.Round(current_accumulated_qty1_value)) + drainage_previous_end_chainage_value;
                                                        }

                                                        end_chainage_value = ((drainage_cross_wire_length_value - (drainage_lapping_value / 1000)) * Math.Round(current_accumulated_qty_value)) + drainage_previous_end_chainage_value;
                                                    }

                                                    output_drainage1.Add("drainage_chainage_start_chainage" + "_" + m, start_chainage_value);
                                                    output_drainage1.Add("drainage_chainage_end_chainage" + "_" + m, end_chainage_value);
                                                    double shape_code_a_value = 0.0;
                                                    double shape_code_b_value = 0.0;
                                                    double shape_code_c_value = 0.0;

                                                    //shape_code=2M1->outer_chainage.shape_code_b=(left_wall_thick+drain_width+right_wall_thick-(2*outer_cover))
                                                    //shape_code=2MR1->outer_chainage.shape_code_b=drain_width+(2*inner_cover)
                                                    //outer_chainage.shape_code_a=((outer_chainage.mainlength*1000)-outer_chainage.shape_code_b-right_wall_factor-left_wall_factor)/2+(left_wall_factor)
                                                    //outer_chainage.shape_code_c=((outer_chainage.mainlength*1000)-outer_chainage.shape_code_b-right_wall_factor-left_wall_factor)/2+(right_wall_factor)
                                                    if (vchShape == "2M1")
                                                    {
                                                        int drainWidth = Convert.ToInt16(dtParamDrainWM.Rows[a]["sitDrainWidth"]);
                                                        shape_code_b_value = (intLeftWallThick + drainWidth + intRightWallThick - (2 * _drainOuterCover));
                                                        shape_code_a_value = ((mainlength_value * 1000) - shape_code_b_value - intRightWallFactor - intLeftWallFactor) / 2 + (intLeftWallFactor);
                                                        shape_code_c_value = ((mainlength_value * 1000) - shape_code_b_value - intRightWallFactor - intLeftWallFactor) / 2 + (intRightWallFactor);

                                                        output_drainage1.Add("drainage_chainage_shape_code_a" + "_" + m, shape_code_a_value);
                                                        output_drainage1.Add("drainage_chainage_shape_code_b" + "_" + m, shape_code_b_value);
                                                        output_drainage1.Add("drainage_chainage_shape_code_c" + "_" + m, shape_code_c_value);
                                                    }
                                                    if (vchShape == "2MR1")
                                                    {
                                                        int drainWidth = Convert.ToInt16(dtParamDrainWM.Rows[a]["sitDrainWidth"]);
                                                        shape_code_b_value = drainWidth + (2 * _drainInnerCover);
                                                        shape_code_a_value = ((mainlength_value * 1000) - shape_code_b_value - intRightWallFactor - intLeftWallFactor) / 2 + (intLeftWallFactor);
                                                        shape_code_c_value = ((mainlength_value * 1000) - shape_code_b_value - intRightWallFactor - intLeftWallFactor) / 2 + (intRightWallFactor);

                                                        output_drainage1.Add("drainage_chainage_shape_code_a" + "_" + m, shape_code_a_value);
                                                        output_drainage1.Add("drainage_chainage_shape_code_b" + "_" + m, shape_code_b_value);
                                                        output_drainage1.Add("drainage_chainage_shape_code_c" + "_" + m, shape_code_c_value);
                                                    }
                                                    //shape_code=1MR1 or shape_code=1M1->outer_chainage.shape_code_a=inner_left_parameter_a
                                                    //shape_code=1MR1 or shape_code=1M1->outer_chainage.shape_code_b=(outer_chainage.mainlength*1000)-outer_chainage.shape_code_a

                                                    if (vchShape == "1M1" || vchShape == "1MR1")
                                                    {
                                                        shape_code_a_value = Double.Parse(strParamA);
                                                        shape_code_b_value = (mainlength_value * 1000) - shape_code_a_value;

                                                        output_drainage1.Add("drainage_chainage_shape_code_a" + "_" + m, shape_code_a_value);
                                                        output_drainage1.Add("drainage_chainage_shape_code_b" + "_" + m, shape_code_b_value);
                                                    }
                                                    if (vchShape == "F")
                                                    {
                                                        shape_code_a_value = mainlength_value * 1000;
                                                        output_drainage1.Add("drainage_chainage_shape_code_a" + "_" + m, shape_code_a_value);
                                                    }
                                                    //Group Drainage1_Result = (Group)configuration.Result.RootGroup.SubGroups["drain_result_group"];

                                                }

                                                ArrayList alChainage_MainLength = new ArrayList();
                                                ArrayList alChainage_Final_Actual_Qty = new ArrayList();
                                                ArrayList alEnd_Chainage = new ArrayList();
                                                ArrayList alStart_Chainage = new ArrayList();
                                                ArrayList alShape_a = new ArrayList();
                                                ArrayList alShape_b = new ArrayList();
                                                ArrayList alShape_c = new ArrayList();
                                                ArrayList alAccumulatedQty = new ArrayList();
                                                ArrayList alChainageActualQty = new ArrayList();
                                                foreach (KeyValuePair<string, double> entry in output_drainage1)
                                                {
                                                    if (entry.Key.ToLower().StartsWith("drainage_chainage_mainlength"))
                                                    {
                                                        alChainage_MainLength.Add(entry.Value.ToString());
                                                    }

                                                    if (entry.Key.ToLower().StartsWith("drainage_chainage_final_actual_qty"))
                                                    {
                                                        alChainage_Final_Actual_Qty.Add(entry.Value.ToString());
                                                    }
                                                    if (entry.Key.ToLower().StartsWith("drainage_chainage_end_chainage"))
                                                    {
                                                        alEnd_Chainage.Add(entry.Value.ToString());
                                                    }
                                                    if (entry.Key.ToLower().StartsWith("drainage_chainage_start_chainage"))
                                                    {
                                                        alStart_Chainage.Add(entry.Value.ToString());
                                                    }
                                                    if (entry.Key.ToLower().StartsWith("drainage_chainage_shape_code_a"))
                                                    {
                                                        alShape_a.Add((Math.Round(entry.Value)).ToString());
                                                    }
                                                    if (entry.Key.ToLower().StartsWith("drainage_chainage_shape_code_b"))
                                                    {
                                                        alShape_b.Add((Math.Round(entry.Value)).ToString());
                                                    }
                                                    if (entry.Key.ToLower().StartsWith("drainage_chainage_shape_code_c"))
                                                    {
                                                        alShape_c.Add((Math.Round(entry.Value)).ToString());
                                                    }
                                                    if (entry.Key.ToLower().StartsWith("drainage_chainage_total_current_accumulated_qty"))
                                                    {
                                                        alAccumulatedQty.Add(entry.Value.ToString());
                                                    }
                                                    if (entry.Key.ToLower().StartsWith("drainage_chainage_actual_qty_field"))
                                                    {
                                                        alChainageActualQty.Add(entry.Value.ToString());
                                                    }
                                                }

                                                if (IsReverse == true)
                                                {
                                                    ArrayList alNewEnd_Chainage = new ArrayList();
                                                    ArrayList alNewStart_Chainage = new ArrayList();
                                                    alChainage_MainLength.Reverse();
                                                    alChainage_Final_Actual_Qty.Reverse();
                                                    alEnd_Chainage.Reverse();
                                                    decimal decPrevValue = 0;
                                                    decPrevValue = Convert.ToDecimal(_startChainage);
                                                    for (int k = 0; k < alEnd_Chainage.Count - 1; k++)
                                                    {
                                                        decimal decEndChainageVal = 0;
                                                        decEndChainageVal = Convert.ToDecimal(alEnd_Chainage[k]) - Convert.ToDecimal(alEnd_Chainage[k + 1]);
                                                        decPrevValue = decPrevValue + decEndChainageVal;
                                                        alNewEnd_Chainage.Add(decPrevValue);
                                                    }
                                                    alNewEnd_Chainage.Add(alEnd_Chainage[0]);
                                                    for (int l = 0; l < alNewEnd_Chainage.Count; l++)
                                                    {
                                                        if (l == 0)
                                                        {
                                                            //alNewStart_Chainage.Add(0.0);
                                                            alNewStart_Chainage.Add(Convert.ToDecimal(_startChainage));
                                                        }
                                                        else
                                                        {
                                                            alNewStart_Chainage.Add(alNewEnd_Chainage[l - 1]);
                                                        }
                                                    }

                                                    alEnd_Chainage = null;
                                                    alStart_Chainage = null;
                                                    alEnd_Chainage = alNewEnd_Chainage;
                                                    alStart_Chainage = alNewStart_Chainage;
                                                    alShape_a.Reverse();
                                                    alShape_b.Reverse();
                                                    alShape_c.Reverse();
                                                    alAccumulatedQty.Reverse();
                                                    alChainageActualQty.Reverse();

                                                }
                                                int intCummulativeFinalQty = 0;
                                                Boolean ignoreChainage = false;
                                                Boolean splNextChainage = false;


                                                for (int x = 0; x < alChainage_MainLength.Count; x++)
                                                {
                                                    if ((!ignoreChainage) & (!splNextChainage))
                                                    {
                                                        int CurrentAccQty = 0;
                                                        int nextAccQty = 0;
                                                        //Used when we use actualQuantity is greater then Mesh Qty

                                                        CurrentAccQty = Convert.ToInt32(Convert.ToDecimal(alAccumulatedQty[x].ToString().Trim()));
                                                        //if (CurrentAccQty <= TotMeshQty)
                                                        //{
                                                        decimal decFinalActualQty = Convert.ToDecimal(alChainage_Final_Actual_Qty[x].ToString());
                                                        int intFinalActualQty = Convert.ToInt32(decFinalActualQty);
                                                        int intChainActualQuantity = Convert.ToInt32(Convert.ToDecimal(alChainageActualQty[x].ToString()));

                                                        if ((CurrentAccQty > 0) & (intFinalActualQty > 0) & (intChainActualQuantity > 0))
                                                        {
                                                            decimal decStartChn = Convert.ToDecimal(alStart_Chainage[x].ToString());
                                                            int intStartChn = Convert.ToInt32(decStartChn);
                                                            decimal decEndChn = Convert.ToDecimal(alEnd_Chainage[x].ToString());
                                                            int intEndChn = Convert.ToInt32(decEndChn);
                                                            //if ((intStartChn >= 0) & (intEndChn > 0))                                                                                                                               
                                                            if (IsCascade == true)
                                                            {
                                                                //to avoid the next validation, becoz for cascade end chainage will be calculated manually
                                                                intEndChn = 1;
                                                            }
                                                            if (intEndChn > 0)
                                                            {

                                                                if (x < alAccumulatedQty.Count - 1)
                                                                {
                                                                    nextAccQty = Convert.ToInt32(Convert.ToDecimal(alAccumulatedQty[x + 1].ToString().Trim()));
                                                                    if (nextAccQty > TotMeshQty)
                                                                    {
                                                                        int intDiffCurrent = TotMeshQty - Convert.ToInt32(Convert.ToDecimal(alAccumulatedQty[x].ToString().Trim()));
                                                                        int intDiffNext = nextAccQty - TotMeshQty;
                                                                        if (intDiffNext == intDiffCurrent)
                                                                        {
                                                                            ignoreChainage = true;
                                                                        }
                                                                        else if (intDiffNext > intDiffCurrent)
                                                                        {
                                                                            ignoreChainage = true;
                                                                        }
                                                                        else
                                                                        {
                                                                            ignoreChainage = false;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        ignoreChainage = false;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (Convert.ToInt32(Convert.ToDecimal(alAccumulatedQty[x].ToString().Trim())) > TotMeshQty)
                                                                    {
                                                                        splNextChainage = true;
                                                                    }
                                                                    else
                                                                    {
                                                                        splNextChainage = false;
                                                                    }
                                                                }


                                                                string vchMeshShape = "";
                                                                vchMeshShape = dtParamDrainWM.Rows[a]["vchMeshShapeGroup"].ToString();
                                                                List<ProductCode> lstData = GetProductCode(dtParamDrainWM.Rows[a]["vchProductCode"].ToString().Trim());

                                                                int intProduction_MO1 = 0;
                                                                int intProduction_MO2 = 0;
                                                                int intProduction_CO1 = 0;
                                                                int intProduction_CO2 = 0;
                                                                int envelop_height = 0;
                                                                int envelop_width = 0;
                                                                int envelop_length = 0;
                                                                decimal actual_tonnage = 0.0M;
                                                                int intNoOfCW = 0;
                                                                int intNoOfMW = 0;
                                                                int intChainageMainLength = 0;
                                                                int intCrossWireLength = 0;
                                                                if (vchMeshShape != "")
                                                                {

                                                                    if (IsCascade == true)
                                                                    {
                                                                        IsIgnoreNext = false;
                                                                        if (x < alAccumulatedQty.Count - 1)
                                                                        {
                                                                            int intNextFinalActualQty = Convert.ToInt32(Convert.ToDecimal(alChainage_Final_Actual_Qty[x + 1].ToString()));
                                                                            int intNextAccQty = Convert.ToInt32(Convert.ToDecimal(alAccumulatedQty[x + 1].ToString().Trim()));
                                                                            //int intNextEndChainage = Convert.ToInt32(Convert.ToDecimal(alEnd_Chainage[x + 1].ToString()));

                                                                            if ((intNextAccQty > 0) & (intNextFinalActualQty > 0))
                                                                            {
                                                                                IsIgnoreNext = false;
                                                                            }
                                                                            else
                                                                            {
                                                                                IsIgnoreNext = true;
                                                                            }
                                                                        }

                                                                    }
                                                                    //currently working -aishwarya
                                                                    //Load the third TCX       
                                                                    //string slabTcxPath = "";
                                                                    //tcxPath = @"D:\DrainModule\DrainBLL\Data\" + vchMeshShape + ".tcx";
                                                                    //if (intConfigCounter > 0)
                                                                    //{
                                                                    //    //configuration.Factory.Shutdown();
                                                                    //}
                                                                    //slabTcxPath = strTcxPath + "d" + vchMeshShape + ".tcx";
                                                                    //LoadConfigurationFromFile(slabTcxPath);
                                                                    intConfigCounter++;
                                                                    string weight_per_area = "";
                                                                    string mw_weight_m_run = "";
                                                                    string cw_weight_m_run = "";
                                                                    Dictionary<string, string> dMeshShapeInputs = new Dictionary<string, string>();
                                                                    objInfo.ClearDictionary();
                                                                    if (lstData != null)
                                                                    {
                                                                        cw_weight_m_run = Convert.ToString(lstData[0].CwWeightPerMeterRun);
                                                                        weight_per_area = Convert.ToString(lstData[0].WeightArea);
                                                                        mw_weight_m_run = Convert.ToString(lstData[0].WeightPerMeterRun);
                                                                    }
                                                                    dMeshShapeInputs.Add("weight_per_area", weight_per_area);
                                                                    dMeshShapeInputs.Add("mw_weight_m_run", mw_weight_m_run);
                                                                    dMeshShapeInputs.Add("cw_weight_m_run", cw_weight_m_run);
                                                                    dMeshShapeInputs.Add("MeshShapeGroup", vchMeshShape);
                                                                    if (vchMeshShape == "flat")
                                                                    {
                                                                        //configuration.SetStep("flat_step");
                                                                        dMeshShapeInputs.Add("shape_code", dtParamDrainWM.Rows[a]["chrShapeCode"].ToString().Trim());
                                                                        if ((alShape_a[x].ToString().Trim() != "-inf") & (alShape_a[x].ToString().Trim() != "inf"))
                                                                        {
                                                                            dMeshShapeInputs.Add("a", alShape_a[x].ToString().Trim());
                                                                        }
                                                                        else
                                                                        {
                                                                            dMeshShapeInputs.Add("a", (Convert.ToInt32(Convert.ToDecimal(alChainage_MainLength[x].ToString().Trim()) * 1000)).ToString());
                                                                        }

                                                                    }
                                                                    if (vchMeshShape == "1sw3")
                                                                    {

                                                                        // configuration.SetStep("1sw3_step");
                                                                        dMeshShapeInputs.Add("shape_code", dtParamDrainWM.Rows[a]["chrShapeCode"].ToString().Trim());
                                                                        dMeshShapeInputs.Add("a", alShape_a[x].ToString().Trim());//dtParamDrainWM.Rows[a]["intParamA"].ToString ());
                                                                        dMeshShapeInputs.Add("b", alShape_b[x].ToString().Trim());
                                                                    }
                                                                    if (vchMeshShape == "2sw3")
                                                                    {
                                                                        // configuration.SetStep("2sw3_step");
                                                                        dMeshShapeInputs.Add("shape_code", dtParamDrainWM.Rows[a]["chrShapeCode"].ToString().Trim());
                                                                        dMeshShapeInputs.Add("a", alShape_a[x].ToString().Trim());
                                                                        dMeshShapeInputs.Add("b", alShape_b[x].ToString().Trim());
                                                                        dMeshShapeInputs.Add("c", alShape_c[x].ToString().Trim());
                                                                    }

                                                                    dMeshShapeInputs.Add("mw_length", (Convert.ToInt32(Convert.ToDecimal(alChainage_MainLength[x].ToString()) * 1000)).ToString());
                                                                    dMeshShapeInputs.Add("cw_length", (Convert.ToInt32(decCrossWire * 1000)).ToString());
                                                                    dMeshShapeInputs.Add("cw_spacing", intCWSpacing.ToString().Trim());
                                                                    dMeshShapeInputs.Add("mw_spacing", intMWSpacing.ToString().Trim());
                                                                    dMeshShapeInputs.Add("cw_dia", intCWDia.ToString());
                                                                    dMeshShapeInputs.Add("mw_dia", intMWDia.ToString());

                                                                    //to calculate the MO1 and MO2
                                                                    isFailedMO2Commit = false;
                                                                    isFailedCO2Commit = false;
                                                                    int intUsedMO1 = 0;
                                                                    int intUsedCO1 = 0;

                                                                    intChainageMainLength = Convert.ToInt32(Convert.ToDecimal(alChainage_MainLength[x]) * 1000);
                                                                    intCrossWireLength = Convert.ToInt32(decCrossWire * 1000);
                                                                    if (Convert.ToInt16(dtParamDrainWM.Rows[a]["bitPredefined"]) > 0)
                                                                    {
                                                                        int intDrainWMId = Convert.ToInt32(dtParamDrainWM.Rows[a]["intDrainWMId"]);
                                                                        for (int m = 0; m < dtParamSegments.Rows.Count; m++)
                                                                        {
                                                                            if (Convert.ToInt32(dtParamSegments.Rows[m]["intDrainWMId"]) == intDrainWMId)
                                                                            {
                                                                                int intMO1 = 0;
                                                                                int intMO2 = 0;
                                                                                intMO1 = Convert.ToInt32(dtParamSegments.Rows[m]["intMo1"].ToString().Trim());
                                                                                intMO2 = Convert.ToInt32(dtParamSegments.Rows[m]["intMo2"].ToString().Trim());
                                                                                isFailedMO2Commit = false;
                                                                                //intCommitMO1 = intMO1 + intBaseThickness;
                                                                                if ((vchShape == "1M1") || (vchShape == "1MR1"))
                                                                                {
                                                                                    intMO1 = Convert.ToInt32(alShape_a[x].ToString().Trim()) + intBaseThickness;
                                                                                }
                                                                                dMeshShapeInputs.Add("mo1", intMO1.ToString());
                                                                                dMeshShapeInputs.Add("mo2", intMO2.ToString());
                                                                                intUsedMO1 = intMO1;
                                                                                objDetailInfo.intInvoiceMO1 = intMO1;
                                                                                objDetailInfo.intInvoiceMO2 = intMO2;
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {

                                                                        if ((intChainageMainLength % intCWSpacing) > 0)
                                                                        {
                                                                            //added by vidhya - NEW -END

                                                                            if ((vchShape == "F"))
                                                                            {
                                                                                int NoOfCW = (Int32)Math.Round(Convert.ToDecimal((intChainageMainLength - intOddMO1 - intOddMO2) / (double)intCWSpacing), MidpointRounding.AwayFromZero);
                                                                                int intNewMO2 = intChainageMainLength - intOddMO1 - ((NoOfCW - 1) * intCWSpacing);
                                                                                if (intNewMO2 > intCWSpacing)
                                                                                {
                                                                                    NoOfCW = NoOfCW + (intNewMO2 / intCWSpacing);
                                                                                    intNewMO2 = intChainageMainLength - intOddMO1 - ((NoOfCW - 1) * intCWSpacing);
                                                                                    intOddMO2 = intNewMO2;
                                                                                }
                                                                                //Main overhang logic to adjust with spacing and length value
                                                                                //added by vidhya - NEW -END

                                                                            }
                                                                            isFailedMO2Commit = false;
                                                                            if ((vchShape == "1M1") || (vchShape == "1MR1"))
                                                                            {
                                                                                intOddMO1 = Convert.ToInt32(alShape_a[x].ToString().Trim()) + intBaseThickness;
                                                                            }
                                                                            dMeshShapeInputs.Add("mo1", intOddMO1.ToString());
                                                                            dMeshShapeInputs.Add("mo2", intOddMO2.ToString());
                                                                            intUsedMO1 = intOddMO1;
                                                                            objDetailInfo.intInvoiceMO1 = intOddMO1;
                                                                            objDetailInfo.intInvoiceMO2 = intOddMO2;
                                                                        }
                                                                        else
                                                                        {
                                                                            isFailedMO2Commit = false;
                                                                            if ((vchShape == "1M1") || (vchShape == "1MR1"))
                                                                            {
                                                                                intEvenMO1 = Convert.ToInt32(alShape_a[x].ToString().Trim()) + intBaseThickness;
                                                                            }
                                                                            dMeshShapeInputs.Add("mo1", intEvenMO1.ToString());
                                                                            dMeshShapeInputs.Add("mo2", intEvenMO2.ToString());
                                                                            intUsedMO1 = intEvenMO1;
                                                                            objDetailInfo.intInvoiceMO1 = intEvenMO1;
                                                                            objDetailInfo.intInvoiceMO2 = intEvenMO2;
                                                                        }
                                                                    }
                                                                    if ((intCrossWireLength % intMWSpacing) > 0)
                                                                    {
                                                                        isFailedCO2Commit = false;
                                                                        dMeshShapeInputs.Add("co1", intOddCO1.ToString());
                                                                        dMeshShapeInputs.Add("co2", intOddCO2.ToString());
                                                                        intUsedCO1 = intOddCO1;
                                                                        objDetailInfo.intInvoiceCO1 = intOddCO1;
                                                                        objDetailInfo.intInvoiceCO2 = intOddCO2;
                                                                    }
                                                                    else
                                                                    {
                                                                        isFailedCO2Commit = false;
                                                                        dMeshShapeInputs.Add("co1", intEvenCO1.ToString());
                                                                        dMeshShapeInputs.Add("co2", intEvenCO2.ToString());
                                                                        intUsedCO1 = intEvenCO1;
                                                                        objDetailInfo.intInvoiceCO1 = intEvenCO1;
                                                                        objDetailInfo.intInvoiceCO2 = intEvenCO2;
                                                                    }

                                                                    if (isFailedCO2Commit == true)
                                                                    {
                                                                        //configuration.UnCommit(vchMeshShape.Trim() + "_co2_field");
                                                                        intNoOfMW = 0;
                                                                        //intNoOfMW = Convert.ToInt32(Convert.ToDecimal((intCrossWireLength - intUsedCO1) / intMWSpacing));
                                                                        intNoOfMW = (Int32)Math.Round(Convert.ToDecimal((intCrossWireLength - intUsedCO1) / (double)intMWSpacing), MidpointRounding.AwayFromZero);
                                                                        if (intNoOfMW < 2)
                                                                        {
                                                                            intNoOfMW = 2;
                                                                        }

                                                                        int intNewCO2 = 0;
                                                                        intNewCO2 = intCrossWireLength - intUsedCO1 - ((intNoOfMW - 1) * intMWSpacing);
                                                                        if (intNewCO2 > intMWSpacing)
                                                                        {
                                                                            intNoOfMW = intNoOfMW + 1;
                                                                            intNewCO2 = 0;
                                                                            intNewCO2 = intCrossWireLength - intUsedCO1 - ((intNoOfMW - 1) * intMWSpacing);
                                                                        }
                                                                        dMeshShapeInputs.Add("no_of_mw", intNoOfMW.ToString());
                                                                        if (intNewCO2 <= 0)
                                                                        {
                                                                            return false;
                                                                        }
                                                                        else
                                                                        {
                                                                            objDetailInfo.intInvoiceCO2 = intNewCO2;
                                                                        }
                                                                    }
                                                                    if (isFailedMO2Commit == true)
                                                                    {
                                                                        //configuration.UnCommit(vchMeshShape.Trim() + "_mo2_field");
                                                                        intNoOfCW = 0;

                                                                        intNoOfCW = (Int32)Math.Round(Convert.ToDecimal((intChainageMainLength - intUsedMO1) / (double)intCWSpacing), MidpointRounding.AwayFromZero);
                                                                        //Math.Round(5.5, MidpointRounding.AwayFromZero);
                                                                        if (intNoOfCW < 2)
                                                                        {
                                                                            intNoOfCW = 2;
                                                                        }
                                                                        int intNewMO2 = 0;
                                                                        intNewMO2 = intChainageMainLength - intUsedMO1 - ((intNoOfCW - 1) * intCWSpacing);
                                                                        if (intNewMO2 > intCWSpacing)
                                                                        {
                                                                            intNoOfCW = intNoOfCW + 1;
                                                                            intNewMO2 = 0;
                                                                            intNewMO2 = intChainageMainLength - intUsedMO1 - ((intNoOfCW - 1) * intCWSpacing);
                                                                        }
                                                                        dMeshShapeInputs.Add("no_of_cw", intNoOfCW.ToString());
                                                                        if (intNewMO2 <= 0)
                                                                        {
                                                                            return false;
                                                                        }
                                                                        else
                                                                        {
                                                                            objDetailInfo.intInvoiceMO2 = intNewMO2;
                                                                        }
                                                                    }
                                                                    dMeshShapeInputs.Add("pin_dia", intPinSize.ToString().Trim());
                                                                    objDetailInfo.sitPinSize = intPinSize;
                                                                    int shapeCodeId = Convert.ToInt32(dtParamDrainWM.Rows[a]["intShapeId"]);
                                                                    dMeshShapeInputs.Add("ShapeCodeId", shapeCodeId.ToString());
                                                                    objInfo.FillInputDictionary(dMeshShapeInputs);


                                                                    convertedString = ConvertJsonFormat(dMeshShapeInputs);

                                                                    //to develop-aishwarya

                                                                    string validations = string.Empty;
                                                                    string error = string.Empty;

                                                                    Dictionary<string, string> SlabShape_Result = objInfo.ExecuteDetailingComponent(shapeCodeId, string.Empty);

                                                                   // Dictionary<string, string> SlabShape_Result = objInfo.ExecuteDetailingComponent(9, string.Empty);


                                                                    //Group SlabShape_Result = (Group)configuration.Result.RootGroup.SubGroups["result_group"];
                                                                    intMWPrdLength = 0;
                                                                    intCWPrdLength = 0;
                                                                    //group.Parameters   

                                                                    if (SlabShape_Result != null)
                                                                    {
                                                                        validations = SlabShape_Result["ValidationFailed"];
                                                                        SlabShape_Result.Remove("ValidationFailed");
                                                                        if (validations == "False")
                                                                        {
                                                                            foreach (KeyValuePair<string, string> result in SlabShape_Result)
                                                                            {
                                                                                if (result.Key.ToLower() == "no_of_mw")
                                                                                {
                                                                                    objDetailInfo.intInvoiceMainQty = Convert.ToInt32(Double.Parse(result.Value));
                                                                                    objDetailInfo.intProductionMainQty = Convert.ToInt32(Double.Parse(result.Value));
                                                                                    intNoOfMW = Convert.ToInt32(Double.Parse(result.Value));
                                                                                }
                                                                                else if (result.Key.ToLower() == "no_of_cw")
                                                                                {
                                                                                    objDetailInfo.intInvoiceCrossQty = Convert.ToInt32(Double.Parse(result.Value));
                                                                                    objDetailInfo.intProductionCrossQty = Convert.ToInt32(Double.Parse(result.Value));
                                                                                    intNoOfCW = Convert.ToInt32(Double.Parse(result.Value));
                                                                                }
                                                                                else if (result.Key.ToLower() == "area_post")
                                                                                {
                                                                                    objDetailInfo.numInvoiceArea = Convert.ToDecimal(Decimal.Parse(result.Value));
                                                                                }
                                                                                else if (result.Key.ToLower() == "envelope_length")
                                                                                {
                                                                                    objDetailInfo.intEnvelopeLength = Convert.ToInt32(Decimal.Parse(result.Value));
                                                                                    envelop_length = Convert.ToInt32(Decimal.Parse(result.Value));
                                                                                }
                                                                                else if (result.Key.ToLower() == "envelope_width")
                                                                                {
                                                                                    objDetailInfo.intEnvelopewidth = Convert.ToInt32(Decimal.Parse(result.Value));
                                                                                    envelop_width = Convert.ToInt32(Decimal.Parse(result.Value));
                                                                                }
                                                                                else if (result.Key.ToLower() == "envelope_height")
                                                                                {
                                                                                    objDetailInfo.intEnvelopeHeight = Convert.ToInt32(Decimal.Parse(result.Value));
                                                                                    envelop_height = Convert.ToInt32(Decimal.Parse(result.Value));
                                                                                }

                                                                                else if (result.Key.ToLower() == "theoretical_tonnage")
                                                                                {
                                                                                    objDetailInfo.numTheoraticalWeight = Decimal.Parse(result.Value);
                                                                                }
                                                                                else if (result.Key.ToLower() == "net_tonnage")
                                                                                {
                                                                                    objDetailInfo.numNetWeight = Decimal.Parse(result.Value);
                                                                                }
                                                                                else if (result.Key.ToLower() == "production_mw_length")
                                                                                {
                                                                                    //objDetailInfo.numProductionMWLength = Convert.ToDecimal(p.ValueDescription);
                                                                                    intMWPrdLength = GetRoundOffValuefrmDb(Decimal.Parse(result.Value), intProductTypeId);
                                                                                }
                                                                                else if (result.Key.ToLower() == "production_cw_length")
                                                                                {
                                                                                    //objDetailInfo.numProductionCWLength = Convert.ToDecimal(p.ValueDescription);
                                                                                    intCWPrdLength = GetRoundOffValuefrmDb(Decimal.Parse(result.Value), intProductTypeId);
                                                                                }
                                                                                else if (result.Key.ToLower() == "actual_tonnage")
                                                                                {
                                                                                    objDetailInfo.numProductionWeight = Decimal.Parse(result.Value);
                                                                                    actual_tonnage = Decimal.Parse(result.Value);
                                                                                }
                                                                                else if (result.Key.ToLower() == "production_mo1")
                                                                                {
                                                                                    intProduction_MO1 = Convert.ToInt32(Decimal.Parse(result.Value));
                                                                                    //objDetailInfo.intProductionMO1 = intProduction_MO1;
                                                                                }
                                                                                else if (result.Key.ToLower() == "production_mo2")
                                                                                {
                                                                                    intProduction_MO2 = Convert.ToInt32(Decimal.Parse(result.Value));
                                                                                    //objDetailInfo.intProductionMO2 = intProduction_MO2;
                                                                                }
                                                                                else if (result.Key.ToLower() == "production_co1")
                                                                                {
                                                                                    intProduction_CO1 = Convert.ToInt32(Decimal.Parse(result.Value));                                                                            //objDetailInfo.intProductionCO1 = intProduction_CO1;
                                                                                }
                                                                                else if (result.Key.ToLower() == "production_co2")
                                                                                {
                                                                                    intProduction_CO2 = Convert.ToInt32(Decimal.Parse(result.Value));
                                                                                    //objDetailInfo.intProductionCO2 = intProduction_CO2;
                                                                                }
                                                                                else if (result.Key.ToLower() == "mw_bvbs_string")
                                                                                {
                                                                                    objDetailInfo.vchMWBVBSString = result.Value;
                                                                                }
                                                                                else if (result.Key.ToLower() == "cw_bvbs_string")
                                                                                {
                                                                                    objDetailInfo.vchCWBVBSString = result.Value;
                                                                                }
                                                                                else if (result.Key.ToLower() == "string_post")
                                                                                {
                                                                                    objDetailInfo.ParamValues = result.Value;
                                                                                }
                                                                            }

                                                                        }
                                                                        else
                                                                        {
                                                                            foreach (var item in SlabShape_Result)
                                                                            {
                                                                                if (item.Key != "ValidationFailed")
                                                                                {
                                                                                    string errormessage = item.Value;
                                                                                    throw new ArgumentOutOfRangeException(errormessage);
                                                                                }
                                                                            }
                                                                        }
                                                                    }


                                                                    objDetailInfo.numInvoiceMWLength = Convert.ToDecimal(alChainage_MainLength[x].ToString()) * 1000;
                                                                    objDetailInfo.numInvoiceCWLength = Convert.ToDecimal(decCrossWire) * 1000;
                                                                    objDetailInfo.numProductionMWLength = Convert.ToDecimal(intMWPrdLength);
                                                                    objDetailInfo.numProductionCWLength = Convert.ToDecimal(intCWPrdLength);
                                                                    //Rounding on basis of creep
                                                                    string strBitOH = bitOHVal;
                                                                    string strCreepMO1 = "0";
                                                                    string strCreepCO1 = "0";
                                                                    strCreepMO1 = bitOHVal.Split('@')[0].ToString().Trim();
                                                                    strCreepCO1 = bitOHVal.Split('@')[1].ToString().Trim();
                                                                    if (strCreepMO1 == "1")
                                                                    {
                                                                        objDetailInfo.intProductionMO1 = Convert.ToInt32(objDetailInfo.intInvoiceMO1) - (Convert.ToInt32(objDetailInfo.numInvoiceMWLength) - intMWPrdLength);
                                                                        intProduction_MO1 = Convert.ToInt32(objDetailInfo.intInvoiceMO1) - (Convert.ToInt32(objDetailInfo.numInvoiceMWLength) - intMWPrdLength);
                                                                        intProduction_MO2 = Convert.ToInt32(objDetailInfo.intInvoiceMO2);
                                                                        objDetailInfo.intProductionMO2 = intProduction_MO2;
                                                                    }
                                                                    else
                                                                    {
                                                                        objDetailInfo.intProductionMO2 = Convert.ToInt32(objDetailInfo.intInvoiceMO2) - (Convert.ToInt32(objDetailInfo.numInvoiceMWLength) - intMWPrdLength);
                                                                        intProduction_MO2 = Convert.ToInt32(objDetailInfo.intInvoiceMO2) - (Convert.ToInt32(objDetailInfo.numInvoiceMWLength) - intMWPrdLength);
                                                                        intProduction_MO1 = Convert.ToInt32(objDetailInfo.intInvoiceMO1);
                                                                        objDetailInfo.intProductionMO1 = intProduction_MO1;
                                                                    }
                                                                    if (strCreepCO1 == "1")
                                                                    {
                                                                        objDetailInfo.intProductionCO1 = Convert.ToInt32(objDetailInfo.intInvoiceCO1) - (Convert.ToInt32(objDetailInfo.numInvoiceCWLength) - intCWPrdLength);
                                                                        intProduction_CO1 = Convert.ToInt32(objDetailInfo.intInvoiceCO1) - (Convert.ToInt32(objDetailInfo.numInvoiceCWLength) - intCWPrdLength);
                                                                        intProduction_CO2 = Convert.ToInt32(objDetailInfo.intInvoiceCO2);
                                                                        objDetailInfo.intProductionCO2 = intProduction_CO2;
                                                                    }
                                                                    else
                                                                    {
                                                                        objDetailInfo.intProductionCO2 = Convert.ToInt32(objDetailInfo.intInvoiceCO2) - (Convert.ToInt32(objDetailInfo.numInvoiceCWLength) - intCWPrdLength);
                                                                        intProduction_CO2 = Convert.ToInt32(objDetailInfo.intInvoiceCO2) - (Convert.ToInt32(objDetailInfo.numInvoiceCWLength) - intCWPrdLength);
                                                                        intProduction_CO1 = Convert.ToInt32(objDetailInfo.intInvoiceCO1);
                                                                        objDetailInfo.intProductionCO1 = intProduction_CO1;
                                                                    }

                                                                }

                                                                string paramA, paramB, paramC, paramValues = "";
                                                                if (alShape_a.Count > 0)
                                                                {
                                                                    paramA = alShape_a[x].ToString();
                                                                    if ((paramA != "inf") & (paramA != "-inf"))
                                                                    {
                                                                        paramValues = paramValues + "A:" + paramA;
                                                                    }

                                                                }
                                                                if (alShape_b.Count > 0)
                                                                {
                                                                    paramB = alShape_b[x].ToString();
                                                                    if ((paramB != "inf") & (paramB != "-inf"))
                                                                    {
                                                                        paramValues = paramValues + ";" + "B:" + paramB;
                                                                    }
                                                                }
                                                                if (alShape_c.Count > 0)
                                                                {
                                                                    paramC = alShape_c[x].ToString();
                                                                    if ((paramC != "inf") & (paramC != "-inf"))
                                                                    {
                                                                        paramValues = paramValues + ";" + "C:" + paramC;
                                                                    }

                                                                }
                                                                counter++;
                                                                objDetailInfo.intStructureMarkId = 0;
                                                                objDetailInfo.intDrainStructureMarkId = _intDrainStructureMarkId;
                                                                objDetailInfo.tntStructureMarkRevNo = _tntStructureRevNo;
                                                                objDetailInfo.vchProductMarkingName = _structMarkingName + "-" + counter.ToString();
                                                                objDetailInfo.intProductCodeId = Convert.ToInt32(dtParamDrainWM.Rows[a]["intProductCodeId"]);
                                                                objDetailInfo.intShapeCodeId = Convert.ToInt32(dtParamDrainWM.Rows[a]["intShapeId"]);

                                                                if (ignoreChainage)
                                                                {
                                                                    if ((vchShape == "1M1") || (vchShape == "1MR1") || (vchShape == "F"))
                                                                    {
                                                                        objDetailInfo.intInvoiceTotalQty = 2 * (TotMeshQty - intCummulativeFinalQty);
                                                                        objDetailInfo.intProductionTotalQty = 2 * (TotMeshQty - intCummulativeFinalQty);
                                                                        objDetailInfo.intMemberQty = 2 * (TotMeshQty - intCummulativeFinalQty);
                                                                    }
                                                                    else
                                                                    {
                                                                        objDetailInfo.intInvoiceTotalQty = TotMeshQty - intCummulativeFinalQty;
                                                                        objDetailInfo.intProductionTotalQty = TotMeshQty - intCummulativeFinalQty;
                                                                        objDetailInfo.intMemberQty = TotMeshQty - intCummulativeFinalQty;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    objDetailInfo.intInvoiceTotalQty = intFinalActualQty;
                                                                    objDetailInfo.intProductionTotalQty = intFinalActualQty;
                                                                    objDetailInfo.intMemberQty = intFinalActualQty;
                                                                    if ((vchShape == "1M1") || (vchShape == "1MR1") || (vchShape == "F"))
                                                                    {
                                                                        intCummulativeFinalQty = intCummulativeFinalQty + (intFinalActualQty / 2);
                                                                    }
                                                                    else
                                                                    {
                                                                        intCummulativeFinalQty = intCummulativeFinalQty + intFinalActualQty;
                                                                    }
                                                                }

                                                                objDetailInfo.intMWSpacing = Convert.ToInt32(intMWSpacing.ToString());
                                                                objDetailInfo.intCWSpacing = Convert.ToInt32(intCWSpacing.ToString());
                                                                //objDetailInfo.sitPinSize = intPinSize;
                                                                objDetailInfo.bitCoatingIndicator = false;
                                                                objDetailInfo.numConversionFactor = 0;
                                                                objDetailInfo.vchShapeSurcharge = "Y";

                                                                objDetailInfo.bitBendIndicator = false;
                                                               
                                                                objDetailInfo.chrCalculationIndicator = "Y";
                                                                objDetailInfo.tntGenerationStatus = 1;

                                                                if (_bitTransportChk)
                                                                {
                                                                    string TCResult = "";
                                                                    TCResult = TransportCheck(actual_tonnage, envelop_height, envelop_width, envelop_length);
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
                                                                if (_bitMachineChk)
                                                                {
                                                                    DataSet dsMcnChk = new DataSet();
                                                                    DataTable dtMachineCheck = new DataTable();
                                                                    objDetailInfo.chrShapeCode = vchShape;
                                                                    objDetailInfo.intProductCodeId = Convert.ToInt32(dtParamDrainWM.Rows[a]["intProductCodeId"]); ;
                                                                    dsMcnChk = objDetailDal.GetMachineCheckValues(objDetailInfo);
                                                                    if (dsMcnChk.Tables.Count > 0)
                                                                    {
                                                                        dtMachineCheck = dsMcnChk.Tables[0];
                                                                        if ((dtMachineCheck.Rows.Count > 0))
                                                                        {
                                                                            int intMWQty, intCWQty;
                                                                            //string bend_ind, twin_ind;
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
                                                                            //TMC = objTMC.CheckMachineFeasibility(strTcxPath + "TMC.tcx", bend_ind, twin_ind, intMWDia, intCWDia, intChainageMainLength, intCrossWireLength, intNoOfMW, intNoOfCW, intMWSpacing, intCWSpacing, intMWQty, intCWQty, intProduction_MO1, intProduction_MO2, intProduction_CO1, intProduction_CO2, _TMCConfig);

                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    TMC = false;
                                                                }
                                                                if (TMC == false)
                                                                {
                                                                    //throw new Exception("Machine Check Failed.");
                                                                    // Added by Anuran CHG0031337>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                                                                    //string PdMrkNam = objDetailInfo.vchProductMarkingName;
                                                                    //Int32 PdCd = objDetailInfo.intProductCodeId;
                                                                    //int PrdValdCd = 16;



                                                                    //Int32 intReturnValue = 0;
                                                                    //try
                                                                    //{
                                                                    //    System.Data.Common.DbCommand dbcom;

                                                                    //    dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("Updt_DrainProductValidator");
                                                                    //    DataAccess.DataAccess.db.AddInParameter(dbcom, "@ProductMarkingName", DbType.String, PdMrkNam);
                                                                    //    DataAccess.DataAccess.db.AddInParameter(dbcom, "@ProductCodeId", DbType.Int32, PdCd);
                                                                    //    DataAccess.DataAccess.db.AddInParameter(dbcom, "@ProductValidator", DbType.Int32, PrdValdCd);


                                                                    //    intReturnValue = DataAccess.DataAccess.ExecuteNonQuery(dbcom);


                                                                    //}
                                                                    //catch (Exception ex) 
                                                                    //{
                                                                    //    throw ex; 
                                                                    //}
                                                                    intProductValidator = 16;

                                                                    // Added by Anuran CHG0031337<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                                                                }

                                                                objDetailInfo.bitMachineCheckIndicator = TMC;
                                                                ///////////////////////////////////
                                                                objDetailInfo.vchBendCheckResult = "P";
                                                                // objDetailInfo.xmlResult = configuration.SafeState.ToString();
                                                                objDetailInfo.xmlResult = convertedString;
                                                                objDetailInfo.vchFilePath = "";
                                                                objDetailInfo.numInvoiceMWWeight = 0;
                                                                objDetailInfo.numInvoiceCWWeight = 0;
                                                                objDetailInfo.numProductionMWWeight = 0;
                                                                objDetailInfo.numProductionCWWeight = 0;
                                                                //objDetailInfo.ParamValues = paramValues;
                                                                objDetailInfo.BendingPos = "";
                                                                objDetailInfo.intShapeTransHeaderId = 0;
                                                                objDetailInfo.intShapeTransHeaderId = 0;
                                                                //' newly added when there is insert of productmarkinng when the intShapeTransHeaderId is null
                                                                if ((objDetailInfo.intShapeTransHeaderId <= 0))
                                                                {
                                                                    objDetailInfo.intShapeId = Convert.ToInt32(dtParamDrainWM.Rows[a]["intShapeId"]);
                                                                    //lblShapeId.Text.Trim
                                                                    objDetailInfo.vchShapeDescription = "";
                                                                    objDetailInfo.nvchParamValues = paramValues.Replace(";", "ì");
                                                                    objDetailInfo.intUserid = _intUserID;//ViewState("UserId");
                                                                    objDetailInfo.intShapeTransHeaderId = objDetailDal.InsertShapeValFrmTC(objDetailInfo);
                                                                }
                                                                objDetailInfo.intStructureElementTypeId = 5;
                                                                objDetailInfo.tntLayer = Convert.ToInt32(dtParamDrainWM.Rows[a]["tntDrainLayerId"].ToString().Trim());

                                                                //To assign the previous end chainage as the start chainage of next
                                                                if (Convert.ToInt32(decLastEndChainage) == 0)
                                                                {
                                                                    if (loopCount == 0)
                                                                    {
                                                                        objDetailInfo.decStartChainage = Convert.ToDecimal(flStartChainage);
                                                                    }
                                                                    else
                                                                    {
                                                                        objDetailInfo.decStartChainage = Convert.ToDecimal(alStart_Chainage[x].ToString());
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    objDetailInfo.decStartChainage = decLastEndChainage;
                                                                }
                                                                if ((ignoreChainage) || (splNextChainage))
                                                                {
                                                                    decimal decEndChainValue = 0;
                                                                    double dblLap = Convert.ToDouble(dtParamDrainWM.Rows[a]["sitLap"].ToString().Trim());
                                                                    decEndChainValue = Convert.ToDecimal((decCrossWire - (dblLap / 1000)) * TotMeshQty);
                                                                    if (loopCount == 0)
                                                                    {
                                                                        objDetailInfo.decEndChainage = Convert.ToDecimal(flStartChainage) + decEndChainValue; //Modified by aishwarya
                                                                        decLastEndChainage = Convert.ToDecimal(flStartChainage) + decEndChainValue; //Modified by aishwarya
                                                                    }
                                                                    else
                                                                    {
                                                                        objDetailInfo.decEndChainage = decLoopEndChainage + decEndChainValue;
                                                                        decLastEndChainage = decLoopEndChainage + decEndChainValue;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (IsCascade == true)
                                                                    {
                                                                        decimal decDistance = Convert.ToDecimal(_endChainage - _startChainage);
                                                                        if (IsIgnoreNext == true)
                                                                        {
                                                                            decLastEndChainage = Convert.ToDecimal(_startChainage) + (decDistance / intCascadeNo) * (g + 1);
                                                                            objDetailInfo.decEndChainage = decLastEndChainage;
                                                                        }
                                                                        else
                                                                        {
                                                                            if (loopCount == 0)
                                                                            {
                                                                                decimal decLap = Convert.ToDecimal(dtParamDrainWM.Rows[a]["sitLap"].ToString().Trim());
                                                                                decLastEndChainage = (Convert.ToDecimal(_startChainage) + Convert.ToDecimal(decCrossWire * intChainActualQuantity)) - Convert.ToDecimal((decLap / 1000) * intChainActualQuantity);
                                                                            }
                                                                            else
                                                                            {
                                                                                decimal decLap = Convert.ToDecimal(dtParamDrainWM.Rows[a]["sitLap"].ToString().Trim());
                                                                                decLastEndChainage = ((decLastEndChainage) + Convert.ToDecimal(decCrossWire * intChainActualQuantity)) - Convert.ToDecimal((decLap / 1000) * intChainActualQuantity);
                                                                            }
                                                                            objDetailInfo.decEndChainage = decLastEndChainage;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        objDetailInfo.decEndChainage = Convert.ToDecimal(alEnd_Chainage[x].ToString());
                                                                        decLastEndChainage = Convert.ToDecimal(alEnd_Chainage[x].ToString());
                                                                    }
                                                                }
                                                                if (bitProduceIndicator)
                                                                {
                                                                    objDetailInfo.nvchProduceIndicator = "Yes";
                                                                }
                                                                else
                                                                {
                                                                    objDetailInfo.nvchProduceIndicator = "No";
                                                                }
                                                                int intOutput = objDetailDal.DrainProductMarkingDetails_InsUpd(objDetailInfo);

                                                                // added by anuran CHG0031337>>>>>>>>>>>>>>
                                                                if (intProductValidator == 16)
                                                                {
                                                                    updatevalidator();
                                                                    intProductValidator = 0;
                                                                }
                                                                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                                                                intProductMarkingCounter++;
                                                                bool blnBendChk = false;     //maybe not going in if condition ever
                                                                if (blnBendInd)
                                                                {
                                                                    if (!((vchMeshShape.Trim() == "flat") | (vchMeshShape.ToLower().Trim().StartsWith("clr") == true)))
                                                                    {
                                                                        blnBendChk = BendingCheck(intOutput, Convert.ToInt32(dtParamDrainWM.Rows[a]["intShapeId"]), vchShape, intProduction_MO1.ToString(), intProduction_CO1.ToString(), paramValues, intProduction_MO2.ToString(), intProduction_CO2.ToString(), vchBendingGroup, bend_ind, vchMeshShape, "A");
                                                                    }
                                                                }

                                                            }//End of EndChainage greater then 0

                                                        }//End Of CurrentAccQty Nad FinalActualQty greater then 0
                                                        //}//End of CurrentAccumulated Qty less then Total Mesh Qty
                                                    }
                                                }//End of No Of Chainage loop
                                                loopCount++;
                                                decLoopEndChainage = decLastEndChainage;
                                            }//End of Ignore Layer on basis of MaxDepth
                                            else
                                            {
                                                decLastEndChainage = 0.0M;
                                                //for (int m = 0; m <= i; m++)
                                                //{
                                                //    decLastEndChainage = decLastEndChainage + Convert.ToDecimal(alChainageDistance[i].ToString());
                                                //}
                                                for (int m = 0; m <= i; m++)
                                                {
                                                    decLastEndChainage = decLastEndChainage + Convert.ToDecimal(alChainageDistance[i].ToString());
                                                }
                                                if (intChainageCounter < intCurrentDepth)
                                                {
                                                    string strEndChn = alChainageDistance[intChainageCounter - 1].ToString();
                                                    strEndChn = strEndChn.Remove(strEndChn.LastIndexOf(".") + 3);
                                                    decLastEndChainage = Convert.ToDecimal(flStartChainage) + Convert.ToDecimal(strEndChn);
                                                }
                                            }
                                        }// End of OU,IR,IL

                                        //Start-Layer SL and BL
                                        else if ((vchLayer == "SL") || (vchLayer == "BL"))
                                        {
                                            Boolean blnDepthExists = false;
                                            decimal decMW_Dia = 0.0M;
                                            decimal decCW_Dia = 0.0M;
                                            int intCurrentDepth = 0;
                                            string weight_per_area = "";
                                            string mw_weight_m_run = "";
                                            string cw_weight_m_run = "";
                                            List<ProductCode> lstData = GetProductCode(dtParamDrainWM.Rows[a]["vchProductCode"].ToString().Trim());
                                            intCurrentDepth = Convert.ToInt32(dtParamDrainWM.Rows[a]["sitMaxDepth"].ToString().Trim());
                                            Boolean ignoreLyr = false;
                                            {
                                                if (depthCounter == 0)
                                                {
                                                    ignoreLyr = false;
                                                }
                                                else
                                                {
                                                    if (intChainageCounter == intCurrentDepth)
                                                    {
                                                        ignoreLyr = false;
                                                    }
                                                    else
                                                    {
                                                        ignoreLyr = true;
                                                    }
                                                }
                                            }
                                            if (!ignoreLyr)
                                            {
                                                blnDepthExists = true;
                                                int intRows = 0;

                                                int slMainLength = 0;
                                                string paramValues = "";
                                                int meshQty = 0;

                                                preDefinedA = "";
                                                preDefinedB = "";
                                                preDefinedC = "";
                                                if (intConfigCounter > 0)
                                                {
                                                    //configuration.Factory.Shutdown();
                                                }
                                                //string strDrng2 = strTcxPath + "Drainage2.tcx";
                                                //LoadConfigurationFromFile(strDrng2);
                                                intConfigCounter++;
                                                //LoadConfiguration(@"D:\DrainModule\DrainBLL\Data\Drainage2.tcx");

                                                //configuration.UnCommit("drainage_layer_field");
                                                //configuration.UnCommit("drainage_shape_code_field");
                                                //configuration.UnCommit("drainage_previous_end_chainage_field");
                                                //configuration.UnCommit("drainage_start_main_length_parameter_7_field");
                                                //configuration.UnCommit("drainage_total_mesh_qty_field");
                                                //configuration.UnCommit("drainage_cross_wire_length_field");
                                                //configuration.UnCommit("drainage_lapping_field");

                                                Dictionary<string, string> inputs_drainage2 = new Dictionary<string, string>();
                                                inputs_drainage2.Add("drainage_layer_field", vchLayer);

                                                //For checking which tcx should be loaded                                  

                                                ArrayList alStart_Chainage_DRN2 = new ArrayList();
                                                ArrayList alEnd_Chainage_DRN2 = new ArrayList();

                                                vchShape = "";
                                                string vchMeshShape = "";
                                                int intProductCodeID = 0;
                                                int intShapeCodeID = 0;
                                                int tntLayerID = 0;
                                                decMW_Dia = 0.0M;
                                                decCW_Dia = 0.0M;
                                                intMWSpacing = 0;
                                                intCWSpacing = 0;


                                                vchShape = dtParamDrainWM.Rows[a]["chrShapeCode"].ToString().Trim();
                                                vchMeshShape = dtParamDrainWM.Rows[a]["vchMeshShapeGroup"].ToString();
                                                intProductCodeID = Convert.ToInt32(dtParamDrainWM.Rows[a]["intProductCodeId"]);
                                                intShapeCodeID = Convert.ToInt32(dtParamDrainWM.Rows[a]["intShapeId"]);
                                                tntLayerID = Convert.ToInt32(dtParamDrainWM.Rows[a]["tntDrainLayerId"].ToString().Trim());
                                                decMW_Dia = Convert.ToDecimal(dtParamDrainWM.Rows[a]["decMWDiameter"].ToString().Trim());
                                                decCW_Dia = Convert.ToDecimal(dtParamDrainWM.Rows[a]["decCWDiameter"].ToString().Trim());
                                                intMWSpacing = Convert.ToInt32(dtParamDrainWM.Rows[a]["intMWSpace"].ToString().Trim());
                                                intCWSpacing = Convert.ToInt32(dtParamDrainWM.Rows[a]["intCWSpace"].ToString().Trim());
                                                if (Convert.ToInt16(dtParamDrainWM.Rows[a]["bitPredefined"]) > 0)
                                                {
                                                    int intDrainWMId = Convert.ToInt32(dtParamDrainWM.Rows[a]["intDrainWMId"]);
                                                    for (int b = 0; b < dtParamSegments.Rows.Count; b++)
                                                    {
                                                        if (Convert.ToInt32(dtParamSegments.Rows[b]["intDrainWMId"]) == intDrainWMId)
                                                        {
                                                            if (dtParamSegments.Rows[b]["chrSegmentParameter"].ToString().Trim() == "A")
                                                            {
                                                                preDefinedA = dtParamSegments.Rows[b]["intSegmentValue"].ToString().Trim();
                                                            }
                                                            if (dtParamSegments.Rows[b]["chrSegmentParameter"].ToString().Trim() == "B")
                                                            {
                                                                preDefinedB = dtParamSegments.Rows[b]["intSegmentValue"].ToString().Trim();
                                                            }
                                                            if (dtParamSegments.Rows[b]["chrSegmentParameter"].ToString().Trim() == "C")
                                                            {
                                                                preDefinedC = dtParamSegments.Rows[b]["intSegmentValue"].ToString().Trim();
                                                            }
                                                        }
                                                    }
                                                }
                                                //}
                                                if (strPreviousShapeCode != vchShape)
                                                {
                                                    if (vchShape != "")
                                                    {
                                                        dsOverHangs = new DataSet();
                                                        objDetailInfo.tntParamSetNumber = _intParameterSet;
                                                        objDetailInfo.chrShapeCode = vchShape;
                                                        objDetailInfo.intMWSpacing = intMWSpacing;
                                                        objDetailInfo.intCWSpacing = intCWSpacing;
                                                        dsOverHangs = objDetailDal.DrainOverHangs_Get(objDetailInfo);
                                                        if (dsOverHangs.Tables.Count > 0)
                                                        {
                                                            intEvenMO1 = Convert.ToInt32(dsOverHangs.Tables[0].Rows[0]["EvenMO1"].ToString().Trim());
                                                            intEvenMO2 = Convert.ToInt32(dsOverHangs.Tables[0].Rows[0]["EvenMO2"].ToString().Trim());
                                                            intEvenCO1 = Convert.ToInt32(dsOverHangs.Tables[0].Rows[0]["EvenCO1"].ToString().Trim());
                                                            intEvenCO2 = Convert.ToInt32(dsOverHangs.Tables[0].Rows[0]["EvenCO2"].ToString().Trim());
                                                            intOddMO1 = Convert.ToInt32(dsOverHangs.Tables[0].Rows[0]["OddMO1"].ToString().Trim());
                                                            intOddMO2 = Convert.ToInt32(dsOverHangs.Tables[0].Rows[0]["OddMO2"].ToString().Trim());
                                                            intOddCO1 = Convert.ToInt32(dsOverHangs.Tables[0].Rows[0]["OddCO1"].ToString().Trim());
                                                            intOddCO2 = Convert.ToInt32(dsOverHangs.Tables[0].Rows[0]["OddCO2"].ToString().Trim());


                                                            //Start Drain Overhang Logic
                                                            int llap = int.Parse(dtParamDrainWM.Rows[a]["sitLap"].ToString().Trim());
                                                            if (llap != 0)
                                                            {
                                                                if (llap % 100 != 0)
                                                                {
                                                                    llap = llap - (llap % 100);
                                                                }
                                                            }
                                                            int CO1_spacing = intMWSpacing / 2;
                                                            int CO2_spacing = intMWSpacing / 2;
                                                            if (CO1_spacing != 0)
                                                            {
                                                                if (CO1_spacing % 5 != 0)
                                                                {
                                                                    CO1_spacing = CO1_spacing + 5 - (CO1_spacing % 5);
                                                                    CO2_spacing = intMWSpacing - CO1_spacing;
                                                                }
                                                            }
                                                            intEvenCO1 = llap + CO1_spacing;
                                                            intEvenCO2 =   CO2_spacing;
                                                            intOddCO1 = llap + CO1_spacing;
                                                            intOddCO2 = CO2_spacing + (intMWSpacing == 200 ? 100 : 0);
                                                            //END Drain Overhang Logic
                                                        }
                                                        dsOverHangs = null;
                                                    }
                                                }
                                                inputs_drainage2.Add("drainage_shape_code_field", vchShape);



                                                decimal prevEndChainage = 0.0M;
                                                if (i == 0)
                                                {
                                                    prevEndChainage = Convert.ToDecimal(flStartChainage.ToString());
                                                    inputs_drainage2.Add("drainage_previous_end_chainage_field", prevEndChainage.ToString());
                                                }
                                                else
                                                {
                                                    if (depthCounter == 0)
                                                    {
                                                        prevEndChainage = 0.0M;
                                                    }
                                                    else
                                                    {
                                                        if (vchLayer == "BL")
                                                        {
                                                            prevEndChainage = decLastBaseEndChainage;
                                                            decLastEndChainage = prevEndChainage;
                                                        }
                                                        else
                                                        {
                                                            prevEndChainage = decLastSlabEndChainage;
                                                            decLastEndChainage = prevEndChainage;
                                                        }
                                                    }
                                                    inputs_drainage2.Add("drainage_previous_end_chainage_field", prevEndChainage.ToString());
                                                }

                                                if (preDefinedA != "")
                                                {
                                                    slMainLength = slMainLength + Convert.ToInt32(preDefinedA);
                                                    paramValues = "A:" + preDefinedA;
                                                }
                                                if (preDefinedB != "")
                                                {
                                                    slMainLength = slMainLength + Convert.ToInt32(preDefinedB);
                                                    paramValues = paramValues + ";" + "B:" + preDefinedB;
                                                }
                                                if (preDefinedC != "")
                                                {
                                                    slMainLength = slMainLength + Convert.ToInt32(preDefinedC);
                                                    paramValues = paramValues + ";" + "C:" + preDefinedC;
                                                }

                                                inputs_drainage2.Add("drainage_start_main_length_parameter_7_field", (Convert.ToInt32(slMainLength.ToString()) / 1000).ToString());

                                                //decimal mesh = Convert.ToDecimal(alTotalMeshQty[i].ToString());
                                                decimal mesh = Math.Ceiling(Convert.ToDecimal(alTotalMeshQty[i].ToString()));
                                                meshQty = Convert.ToInt32(mesh);

                                                inputs_drainage2.Add("drainage_total_mesh_qty_field", meshQty.ToString());
                                                inputs_drainage2.Add("drainage_cross_wire_length_field", decCrossWire.ToString());
                                                inputs_drainage2.Add("drainage_lapping_field", dtParamDrainWM.Rows[a]["sitLap"].ToString().Trim());

                                                //Result resultLayer = configuration.Result;
                                                //Group Drn2group = (Group)configuration.Result.RootGroup.SubGroups["drain_result_group"];

                                                Dictionary<string, string> Drn2group_result = new Dictionary<string, string>();
                                                // end_chainage=previous_end_chainage+((cross_wire_length-(lapping/1000))*total_mesh_qty)
                                                //start_chainage=previous_end_chainage
                                                float end_chainage_value = 0;
                                                float start_chainage_value = 0;

                                                if (inputs_drainage2.ContainsKey("drainage_previous_end_chainage_field"))
                                                {
                                                    float lapping_value = float.Parse(dtParamDrainWM.Rows[a]["sitLap"].ToString().Trim());
                                                    float drainage_previous_end_chainage_value = float.Parse(inputs_drainage2["drainage_previous_end_chainage_field"]);
                                                    end_chainage_value = drainage_previous_end_chainage_value + ((decCrossWire - (lapping_value / 1000)) * meshQty);
                                                    start_chainage_value = drainage_previous_end_chainage_value;
                                                }
                                                Drn2group_result.Add("drainage_end_chainage_field", end_chainage_value.ToString());
                                                Drn2group_result.Add("drainage_start_chainage_field", start_chainage_value.ToString());

                                                ArrayList a_param_list = new ArrayList();
                                                ArrayList b_param_list = new ArrayList();
                                                double param_a_value;
                                                double param_b_value;
                                                double param_c_value;
                                                //layer=SL and shape_code=1M1->shape_parameter_b=(start_main_length_parameter*1000)-shape_parameter_a
                                                //layer=SL and shape_code=1M1->shape_parameter_a=project_parameter_level_3_4_a
                                                //layer=BL and shape_code=1M1->shape_parameter_a=project_parameter_level_3_4_a
                                                //layer=BL and shape_code=1M1->shape_parameter_b=(start_main_length_parameter*1000)-shape_parameter_a
                                                //shape_code=2M1 or shape_code=2MR1->shape_parameter_a=project_parameter_level_3_4_a
                                                //shape_code=2M1 or shape_code=2MR1->shape_parameter_c=shape_parameter_a
                                                //shape_code=2M1 or shape_code=2MR1->shape_parameter_b=(start_main_length_parameter*1000)-(shape_parameter_a+shape_parameter_c)
                                                //if (vchLayer == "SL")
                                                //{
                                                //    //if ((vchShape == "1M1"))
                                                //    //{
                                                //    //    param_a_value = 
                                                //    //}
                                                //}


                                                foreach (KeyValuePair<string, string> result in Drn2group_result)
                                                {
                                                    if (result.Key.ToLower().StartsWith("drainage_start_chainage_field"))
                                                    {
                                                        alStart_Chainage_DRN2.Add(result.Value);
                                                    }
                                                    if (result.Key.ToLower().StartsWith("drainage_end_chainage_field"))
                                                    {
                                                        alEnd_Chainage_DRN2.Add(result.Value);
                                                    }
                                                }

                                                for (int c = 0; c < alStart_Chainage_DRN2.Count; c++)
                                                {
                                                    if (blnDepthExists)
                                                    {
                                                        int intProduction_MO1 = 0;
                                                        int intProduction_MO2 = 0;
                                                        int intProduction_CO1 = 0;
                                                        int intProduction_CO2 = 0;
                                                        int envelop_height = 0;
                                                        int envelop_width = 0;
                                                        int envelop_length = 0;
                                                        decimal actual_tonnage = 0.0M;
                                                        int intNoOfCW = 0;
                                                        int intNoOfMW = 0;
                                                        int intChainageMainLength = 0;
                                                        int intCrossWireLength = 0;
                                                        if (vchMeshShape != "")
                                                        {
                                                            //Load the third TCX      
                                                            //tcxPath = @"D:\DrainModule\DrainBLL\Data\" + vchMeshShape + ".tcx";                                                                
                                                            if (intConfigCounter > 0)
                                                            {
                                                                //configuration.Factory.Shutdown();
                                                            }
                                                            //string slabTcxPath = strTcxPath + "d" + vchMeshShape + ".tcx";
                                                            //LoadConfigurationFromFile(slabTcxPath);
                                                            intConfigCounter++;
                                                            //LoadConfiguration(tcxPath);
                                                            //Currently-working

                                                            Dictionary<string, string> inputs_drainage = new Dictionary<string, string>();
                                                            objInfo.ClearDictionary();
                                                            if (lstData != null)
                                                            {
                                                                cw_weight_m_run = Convert.ToString(lstData[0].CwWeightPerMeterRun);
                                                                weight_per_area = Convert.ToString(lstData[0].WeightArea);
                                                                mw_weight_m_run = Convert.ToString(lstData[0].WeightPerMeterRun);
                                                            }
                                                            inputs_drainage.Add("weight_per_area", weight_per_area);
                                                            inputs_drainage.Add("mw_weight_m_run", mw_weight_m_run);
                                                            inputs_drainage.Add("cw_weight_m_run", cw_weight_m_run);
                                                            inputs_drainage.Add("MeshShapeGroup", vchMeshShape);
                                                            if (vchMeshShape == "flat")
                                                            {
                                                                // configuration.SetStep("flat_step");
                                                                inputs_drainage.Add("shape_code", vchShape.Trim());
                                                                inputs_drainage.Add("a", preDefinedA.Trim());
                                                            }
                                                            if (vchMeshShape == "1sw3")
                                                            {

                                                                //configuration.SetStep("1sw3_step");
                                                                inputs_drainage.Add("shape_code", vchShape.Trim());
                                                                inputs_drainage.Add("a", preDefinedA.Trim());//dtParamDrainWM.Rows[a]["intParamA"].ToString ());
                                                                inputs_drainage.Add("b", preDefinedB.Trim());
                                                            }
                                                            if (vchMeshShape == "2sw3")
                                                            {
                                                                //configuration.SetStep("2sw3_step");
                                                                inputs_drainage.Add("shape_code", vchShape.Trim());
                                                                inputs_drainage.Add("a", preDefinedA.Trim());
                                                                inputs_drainage.Add("b", preDefinedB.Trim());
                                                                inputs_drainage.Add("c", preDefinedC.Trim());
                                                            }
                                                            inputs_drainage.Add("mw_length", slMainLength.ToString());
                                                            inputs_drainage.Add("cw_length", (Convert.ToInt32(decCrossWire * 1000)).ToString());
                                                            inputs_drainage.Add("cw_spacing", intCWSpacing.ToString());
                                                            inputs_drainage.Add("mw_spacing", intMWSpacing.ToString());
                                                            inputs_drainage.Add("cw_dia", Convert.ToInt32(decCW_Dia).ToString());
                                                            inputs_drainage.Add("mw_dia", Convert.ToInt32(decMW_Dia).ToString());
                                                            //to calculate the MO1 and MO2
                                                            isFailedMO2Commit = false;
                                                            isFailedCO2Commit = false;
                                                            int intUsedMO1 = 0;
                                                            int intUsedCO1 = 0;
                                                            intChainageMainLength = 0;
                                                            intChainageMainLength = slMainLength;
                                                            intCrossWireLength = Convert.ToInt32(decCrossWire * 1000);
                                                            if (Convert.ToInt16(dtParamDrainWM.Rows[a]["bitPredefined"]) > 0)
                                                            {
                                                                int intDrainWMId = Convert.ToInt32(dtParamDrainWM.Rows[a]["intDrainWMId"]);
                                                                for (int m = 0; m < dtParamSegments.Rows.Count; m++)
                                                                {
                                                                    if (Convert.ToInt32(dtParamSegments.Rows[m]["intDrainWMId"]) == intDrainWMId)
                                                                    {
                                                                        int intMO1 = 0;
                                                                        int intMO2 = 0;
                                                                        intMO1 = Convert.ToInt32(dtParamSegments.Rows[m]["intMo1"].ToString().Trim());
                                                                        intMO2 = Convert.ToInt32(dtParamSegments.Rows[m]["intMo2"].ToString().Trim());
                                                                        isFailedMO2Commit = false;
                                                                        if ((vchShape == "1M1") || (vchShape == "1MR1"))
                                                                        {
                                                                            intMO1 = Convert.ToInt32(preDefinedA.Trim()) + intBaseThickness;
                                                                        }
                                                                        if (!inputs_drainage.ContainsKey("mo1") && !inputs_drainage.ContainsKey("mo2"))
                                                                        {
                                                                            inputs_drainage.Add("mo1", intMO1.ToString());
                                                                            inputs_drainage.Add("mo2", intMO2.ToString());
                                                                        }

                                                                        intUsedMO1 = intMO1;
                                                                        objDetailInfo.intInvoiceMO1 = intMO1;
                                                                        objDetailInfo.intInvoiceMO2 = intMO2;
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if ((intChainageMainLength % intCWSpacing) > 0)
                                                                {
                                                                    isFailedMO2Commit = false;
                                                                    if ((vchShape == "1M1") || (vchShape == "1MR1"))
                                                                    {
                                                                        intOddMO1 = Convert.ToInt32(preDefinedA.Trim()) + intBaseThickness;
                                                                    }

                                                                    if (!inputs_drainage.ContainsKey("mo1") && !inputs_drainage.ContainsKey("mo2"))
                                                                    {
                                                                        inputs_drainage.Add("mo1", intOddMO1.ToString());
                                                                        inputs_drainage.Add("mo2", intOddMO2.ToString());
                                                                    }
                                                                    intUsedMO1 = intOddMO1;
                                                                    objDetailInfo.intInvoiceMO1 = intOddMO1;
                                                                    objDetailInfo.intInvoiceMO2 = intOddMO2;
                                                                }
                                                                else
                                                                {
                                                                    isFailedMO2Commit = false;
                                                                    if ((vchShape == "1M1") || (vchShape == "1MR1"))
                                                                    {
                                                                        intEvenMO1 = Convert.ToInt32(preDefinedA.Trim()) + intBaseThickness;
                                                                    }
                                                                    if (!inputs_drainage.ContainsKey("mo1") && !inputs_drainage.ContainsKey("mo2"))
                                                                    {
                                                                        inputs_drainage.Add("mo1", intEvenMO1.ToString());
                                                                        inputs_drainage.Add("mo2", intEvenMO2.ToString());
                                                                    }

                                                                    intUsedMO1 = intEvenMO1;
                                                                    objDetailInfo.intInvoiceMO1 = intEvenMO1;
                                                                    objDetailInfo.intInvoiceMO2 = intEvenMO2;
                                                                }
                                                            }
                                                            if ((intCrossWireLength % intMWSpacing) > 0)
                                                            {
                                                                isFailedCO2Commit = false;
                                                                if (!inputs_drainage.ContainsKey("co1") && !inputs_drainage.ContainsKey("co2"))
                                                                {
                                                                    inputs_drainage.Add("co1", intOddCO1.ToString());
                                                                    inputs_drainage.Add("co2", intOddCO2.ToString());
                                                                }

                                                                intUsedCO1 = intOddCO1;
                                                                objDetailInfo.intInvoiceCO1 = intOddCO1;
                                                                objDetailInfo.intInvoiceCO2 = intOddCO2;
                                                            }
                                                            else
                                                            {
                                                                isFailedCO2Commit = false;
                                                                if (!inputs_drainage.ContainsKey("co1") && !inputs_drainage.ContainsKey("co2"))
                                                                {
                                                                    inputs_drainage.Add("co1", intEvenCO1.ToString());
                                                                    inputs_drainage.Add("co2", intEvenCO2.ToString());
                                                                }
                                                                intUsedCO1 = intOddCO1;
                                                                objDetailInfo.intInvoiceCO1 = intEvenCO1;
                                                                objDetailInfo.intInvoiceCO2 = intEvenCO2;
                                                            }

                                                            if (isFailedCO2Commit == true)
                                                            {
                                                                //configuration.UnCommit(vchMeshShape.Trim() + "_co2_field");
                                                                intNoOfMW = 0;
                                                                //intNoOfMW = Convert.ToInt32(Convert.ToDecimal((intCrossWireLength - intUsedCO1) / intMWSpacing));
                                                                intNoOfMW = (Int32)Math.Round(Convert.ToDecimal((intCrossWireLength - intUsedCO1) / (double)intMWSpacing), MidpointRounding.AwayFromZero);
                                                                if (intNoOfMW < 2)
                                                                {
                                                                    intNoOfMW = 2;
                                                                }
                                                                int intNewCO2 = 0;
                                                                intNewCO2 = intCrossWireLength - intUsedCO1 - ((intNoOfMW - 1) * intMWSpacing);
                                                                if (intNewCO2 > intMWSpacing)
                                                                {
                                                                    intNoOfMW = intNoOfMW + 1;
                                                                    intNewCO2 = 0;
                                                                    intNewCO2 = intCrossWireLength - intUsedCO1 - ((intNoOfMW - 1) * intMWSpacing);
                                                                }
                                                                inputs_drainage.Add("no_of_mw", intNoOfMW.ToString());
                                                                objDetailInfo.intInvoiceCO2 = intNewCO2;
                                                            }
                                                            if (isFailedMO2Commit == true)
                                                            {
                                                                //configuration.UnCommit(vchMeshShape.Trim() + "_mo2_field");
                                                                intNoOfCW = 0;
                                                                //intNoOfCW = Convert.ToInt32(Convert.ToDecimal((intChainageMainLength - intUsedMO1) / intCWSpacing));
                                                                intNoOfCW = (Int32)Math.Round(Convert.ToDecimal((intChainageMainLength - intUsedMO1) / (double)intCWSpacing), MidpointRounding.AwayFromZero);
                                                                if (intNoOfCW < 2)
                                                                {
                                                                    intNoOfCW = 2;
                                                                }
                                                                int intNewMO2 = 0;
                                                                intNewMO2 = intChainageMainLength - intUsedMO1 - ((intNoOfCW - 1) * intCWSpacing);
                                                                if (intNewMO2 > intCWSpacing)
                                                                {
                                                                    intNoOfCW = intNoOfCW + 1;
                                                                    intNewMO2 = 0;
                                                                    intNewMO2 = intChainageMainLength - intUsedMO1 - ((intNoOfCW - 1) * intCWSpacing);
                                                                }
                                                                inputs_drainage.Add("no_of_cw", intNoOfCW.ToString());
                                                                objDetailInfo.intInvoiceMO2 = intNewMO2;
                                                            }
                                                            inputs_drainage.Add("pin_dia", intPinSize.ToString().Trim());
                                                            objDetailInfo.sitPinSize = intPinSize;
                                                            inputs_drainage.Add("ShapeCodeId", intShapeCodeID.ToString());
                                                            //  Group SlabShape_ResultDRN2 = (Group)configuration.Result.RootGroup.SubGroups["result_group"];

                                                            objInfo.FillInputDictionary(inputs_drainage);

                                                            convertedString_drainage1 = ConvertJsonFormat(inputs_drainage);
                                                            //to develop
                                                            //result 

                                                            intMWPrdLength = 0;
                                                            intCWPrdLength = 0;

                                                            string validations = string.Empty;
                                                            string error = string.Empty;
                                                            Dictionary<string, string> SlabShape_ResultDRN2 = objInfo.ExecuteDetailingComponent(intShapeCodeID, string.Empty);

                                                            if (SlabShape_ResultDRN2 != null)
                                                            {
                                                                validations = SlabShape_ResultDRN2["ValidationFailed"];
                                                                SlabShape_ResultDRN2.Remove("ValidationFailed");
                                                                if (validations == "False")
                                                                {
                                                                    foreach (KeyValuePair<string, string> result in SlabShape_ResultDRN2)
                                                                    {
                                                                        if (result.Key.ToLower() == "no_of_mw")
                                                                        {
                                                                            objDetailInfo.intInvoiceMainQty = Convert.ToInt32(Decimal.Parse(result.Value));
                                                                            objDetailInfo.intProductionMainQty = Convert.ToInt32(Decimal.Parse(result.Value));
                                                                            intNoOfMW = Convert.ToInt32(Decimal.Parse(result.Value));
                                                                        }
                                                                        else if (result.Key.ToLower() == "no_of_cw")
                                                                        {
                                                                            objDetailInfo.intInvoiceCrossQty = Convert.ToInt32(Decimal.Parse(result.Value));
                                                                            objDetailInfo.intProductionCrossQty = Convert.ToInt32(Decimal.Parse(result.Value));
                                                                            intNoOfCW = Convert.ToInt32(Decimal.Parse(result.Value));
                                                                        }
                                                                        else if (result.Key.ToLower() == "area_post")
                                                                        {
                                                                            objDetailInfo.numInvoiceArea = Convert.ToDecimal(Decimal.Parse(result.Value));
                                                                        }

                                                                        else if (result.Key.ToLower() == "envelope_length")
                                                                        {
                                                                            objDetailInfo.intEnvelopeLength = Convert.ToInt32(Decimal.Parse(result.Value));
                                                                            envelop_length = Convert.ToInt32(Decimal.Parse(result.Value));
                                                                        }
                                                                        else if (result.Key.ToLower() == "envelope_width")
                                                                        {
                                                                            objDetailInfo.intEnvelopewidth = Convert.ToInt32(Decimal.Parse(result.Value));
                                                                            envelop_width = Convert.ToInt32(Decimal.Parse(result.Value));
                                                                        }
                                                                        else if (result.Key.ToLower() == "envelope_height")
                                                                        {
                                                                            objDetailInfo.intEnvelopeHeight = Convert.ToInt32(Decimal.Parse(result.Value));
                                                                            envelop_height = Convert.ToInt32(Decimal.Parse(result.Value));
                                                                        }

                                                                        else if (result.Key.ToLower() == "theoretical_tonnage")
                                                                        {
                                                                            objDetailInfo.numTheoraticalWeight = Decimal.Parse(result.Value);
                                                                        }
                                                                        else if (result.Key.ToLower() == "net_tonnage")
                                                                        {
                                                                            objDetailInfo.numNetWeight = Decimal.Parse(result.Value);
                                                                        }
                                                                        else if (result.Key.ToLower() == "production_mw_length")
                                                                        {
                                                                            //objDetailInfo.numProductionMWLength = Convert.ToDecimal(p.ValueDescription);
                                                                            intMWPrdLength = GetRoundOffValuefrmDb(Decimal.Parse(result.Value), intProductTypeId);
                                                                        }
                                                                        else if (result.Key.ToLower() == "production_cw_length")
                                                                        {
                                                                            //objDetailInfo.numProductionCWLength = Convert.ToDecimal(p.ValueDescription);
                                                                            intCWPrdLength = GetRoundOffValuefrmDb(Decimal.Parse(result.Value), intProductTypeId);
                                                                        }
                                                                        else if (result.Key.ToLower() == "actual_tonnage")
                                                                        {
                                                                            objDetailInfo.numProductionWeight = Decimal.Parse(result.Value);
                                                                            actual_tonnage = Decimal.Parse(result.Value);
                                                                        }

                                                                        else if (result.Key.ToLower() == "production_mo1")
                                                                        {
                                                                            intProduction_MO1 = Convert.ToInt32(Decimal.Parse(result.Value));
                                                                            //objDetailInfo.intProductionMO1 = intProduction_MO1;
                                                                        }
                                                                        else if (result.Key.ToLower() == "production_mo2")
                                                                        {
                                                                            intProduction_MO2 = Convert.ToInt32(Decimal.Parse(result.Value));
                                                                            //objDetailInfo.intProductionMO2 = intProduction_MO2;
                                                                        }
                                                                        else if (result.Key.ToLower() == "production_co1")
                                                                        {
                                                                            intProduction_CO1 = Convert.ToInt32(Decimal.Parse(result.Value));
                                                                            //objDetailInfo.intProductionCO1 = intProduction_CO1;
                                                                        }
                                                                        else if (result.Key.ToLower() == "production_co2")
                                                                        {
                                                                            intProduction_CO2 = Convert.ToInt32(Decimal.Parse(result.Value));
                                                                            //objDetailInfo.intProductionCO2 = intProduction_CO2;
                                                                        }
                                                                        else if (result.Key.ToLower() == "mw_bvbs_string")
                                                                        {
                                                                            objDetailInfo.vchMWBVBSString = result.Value;
                                                                        }
                                                                        else if (result.Key.ToLower() == "cw_bvbs_string")
                                                                        {
                                                                            objDetailInfo.vchCWBVBSString = result.Value;
                                                                        }
                                                                        else if (result.Key.ToLower() == "string_post")
                                                                        {
                                                                            objDetailInfo.ParamValues = result.Value;
                                                                        }
                                                                    }//End of reading Result
                                                                }
                                                            }

                                                            objDetailInfo.numInvoiceMWLength = slMainLength;
                                                            objDetailInfo.numInvoiceCWLength = Convert.ToDecimal(decCrossWire) * 1000;
                                                            objDetailInfo.numProductionMWLength = Convert.ToDecimal(intMWPrdLength);
                                                            objDetailInfo.numProductionCWLength = Convert.ToDecimal(intCWPrdLength);
                                                            //Rounding on basis of creep
                                                            string strBitOH = bitOHVal;
                                                            string strCreepMO1 = "0";
                                                            string strCreepCO1 = "0";
                                                            strCreepMO1 = bitOHVal.Split('@')[0].ToString().Trim();
                                                            strCreepCO1 = bitOHVal.Split('@')[1].ToString().Trim();
                                                            if (strCreepMO1 == "1")
                                                            {
                                                                objDetailInfo.intProductionMO1 = Convert.ToInt32(objDetailInfo.intInvoiceMO1) - (Convert.ToInt32(objDetailInfo.numInvoiceMWLength) - intMWPrdLength);
                                                                intProduction_MO1 = Convert.ToInt32(objDetailInfo.intInvoiceMO1) - (Convert.ToInt32(objDetailInfo.numInvoiceMWLength) - intMWPrdLength);
                                                                intProduction_MO2 = Convert.ToInt32(objDetailInfo.intInvoiceMO2);
                                                                objDetailInfo.intProductionMO2 = intProduction_MO2;
                                                            }
                                                            else
                                                            {
                                                                objDetailInfo.intProductionMO2 = Convert.ToInt32(objDetailInfo.intInvoiceMO2) - (Convert.ToInt32(objDetailInfo.numInvoiceMWLength) - intMWPrdLength);
                                                                intProduction_MO2 = Convert.ToInt32(objDetailInfo.intInvoiceMO2) - (Convert.ToInt32(objDetailInfo.numInvoiceMWLength) - intMWPrdLength);
                                                                intProduction_MO1 = Convert.ToInt32(objDetailInfo.intInvoiceMO1);
                                                                objDetailInfo.intProductionMO1 = intProduction_MO1;
                                                            }
                                                            if (strCreepCO1 == "1")
                                                            {
                                                                objDetailInfo.intProductionCO1 = Convert.ToInt32(objDetailInfo.intInvoiceCO1) - (Convert.ToInt32(objDetailInfo.numInvoiceCWLength) - intCWPrdLength);
                                                                intProduction_CO1 = Convert.ToInt32(objDetailInfo.intInvoiceCO1) - (Convert.ToInt32(objDetailInfo.numInvoiceCWLength) - intCWPrdLength);
                                                                intProduction_CO2 = Convert.ToInt32(objDetailInfo.intInvoiceCO2);
                                                                objDetailInfo.intProductionCO2 = intProduction_CO2;
                                                            }
                                                            else
                                                            {
                                                                objDetailInfo.intProductionCO2 = Convert.ToInt32(objDetailInfo.intInvoiceCO2) - (Convert.ToInt32(objDetailInfo.numInvoiceCWLength) - intCWPrdLength);
                                                                intProduction_CO2 = Convert.ToInt32(objDetailInfo.intInvoiceCO2) - (Convert.ToInt32(objDetailInfo.numInvoiceCWLength) - intCWPrdLength);
                                                                intProduction_CO1 = Convert.ToInt32(objDetailInfo.intInvoiceCO1);
                                                                objDetailInfo.intProductionCO1 = intProduction_CO1;
                                                            }

                                                            counter++;
                                                            objDetailInfo.intStructureMarkId = 0;
                                                            objDetailInfo.intDrainStructureMarkId = _intDrainStructureMarkId;
                                                            objDetailInfo.tntStructureMarkRevNo = _tntStructureRevNo;
                                                            objDetailInfo.vchProductMarkingName = _structMarkingName + "-" + counter.ToString();
                                                            objDetailInfo.intProductCodeId = intProductCodeID;
                                                            objDetailInfo.intShapeCodeId = intShapeCodeID;

                                                            if (IsCascade == true)
                                                            {
                                                                objDetailInfo.intInvoiceTotalQty = meshQty * _intCascadeNo;
                                                                objDetailInfo.intProductionTotalQty = meshQty * _intCascadeNo;
                                                                objDetailInfo.intMemberQty = meshQty * _intCascadeNo;
                                                            }
                                                            else
                                                            {
                                                                objDetailInfo.intInvoiceTotalQty = meshQty;
                                                                objDetailInfo.intProductionTotalQty = meshQty;
                                                                objDetailInfo.intMemberQty = meshQty;
                                                            }

                                                            objDetailInfo.intMWSpacing = Convert.ToInt32(intMWSpacing.ToString());
                                                            objDetailInfo.intCWSpacing = Convert.ToInt32(intCWSpacing.ToString());
                                                            objDetailInfo.sitPinSize = intPinSize;

                                                            objDetailInfo.bitCoatingIndicator = false;
                                                            objDetailInfo.numConversionFactor = 0;
                                                            objDetailInfo.vchShapeSurcharge = "Y";
                                                            objDetailInfo.bitBendIndicator = _bitBendingChk;
                                                            objDetailInfo.chrCalculationIndicator = "Y";
                                                            objDetailInfo.tntGenerationStatus = 1;

                                                            if (_bitTransportChk)
                                                            {
                                                                string TCResult = "";
                                                                TCResult = TransportCheck(actual_tonnage, envelop_height, envelop_width, envelop_length);
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

                                                            Boolean TMC = false;
                                                            string bend_ind = "";
                                                            string twin_ind = "";
                                                            string vchBendingGroup = "";
                                                            bool blnBendInd = false;
                                                            if (_bitMachineChk)
                                                            {
                                                                DataSet dsMcnChk = new DataSet();
                                                                DataTable dtMachineCheck = new DataTable();
                                                                objDetailInfo.chrShapeCode = vchShape;
                                                                objDetailInfo.intProductCodeId = Convert.ToInt32(dtParamDrainWM.Rows[a]["intProductCodeId"]); ;
                                                                dsMcnChk = objDetailDal.GetMachineCheckValues(objDetailInfo);
                                                                if (dsMcnChk.Tables.Count > 0)
                                                                {
                                                                    dtMachineCheck = dsMcnChk.Tables[0];
                                                                    if ((dtMachineCheck.Rows.Count > 0))
                                                                    {
                                                                        int intMWQty, intCWQty;
                                                                        //string bend_ind, twin_ind;
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
                                                                      //  TMC = objTMC.CheckMachineFeasibility(strTcxPath + "TMC.tcx", bend_ind, twin_ind, Convert.ToInt32(decMW_Dia), Convert.ToInt32(decCW_Dia), intChainageMainLength, intCrossWireLength, intNoOfMW, intNoOfCW, intMWSpacing, intCWSpacing, intMWQty, intCWQty, intProduction_MO1, intProduction_MO2, intProduction_CO1, intProduction_CO2, _TMCConfig);

                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                TMC = false;
                                                            }
                                                            if (TMC == false)
                                                            {

                                                                //throw new Exception("Machine Check Failed.");
                                                                ////// Added by Anuran CHG0031337>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                                                                //string PdMrkNam1 = objDetailInfo.vchProductMarkingName.ToString();
                                                                //Int32 PdCd1 = objDetailInfo.intProductCodeId;
                                                                //int PrdValdCd1 = 16;

                                                                //Int32 intReturnValue1 = 0;
                                                                //try
                                                                //{
                                                                //    System.Data.Common.DbCommand dbcom1;

                                                                //    dbcom1 = DataAccess.DataAccess.db.GetStoredProcCommand("Updt_DrainProductValidator");
                                                                //    DataAccess.DataAccess.db.AddInParameter(dbcom1, "@ProductMarkingName", DbType.String, PdMrkNam1);
                                                                //    DataAccess.DataAccess.db.AddInParameter(dbcom1, "@ProductCodeId", DbType.Int32, PdCd1);
                                                                //    DataAccess.DataAccess.db.AddInParameter(dbcom1, "@ProductValidator", DbType.Int32, PrdValdCd1);


                                                                //    intReturnValue1 = DataAccess.DataAccess.ExecuteNonQuery(dbcom1);


                                                                //}
                                                                //catch (Exception ex)
                                                                //{
                                                                //    throw ex;
                                                                //}

                                                                intProductValidator = 16;
                                                                //////// Added by Anuran CHG0031337<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


                                                            }
                                                            objDetailInfo.bitMachineCheckIndicator = TMC;
                                                            objDetailInfo.vchBendCheckResult = "P";
                                                            //objDetailInfo.xmlResult = configuration.SafeState.ToString();
                                                            objDetailInfo.xmlResult = convertedString_drainage1;
                                                            objDetailInfo.vchFilePath = "";
                                                            objDetailInfo.numInvoiceMWWeight = 0;
                                                            objDetailInfo.numInvoiceCWWeight = 0;
                                                            objDetailInfo.numProductionMWWeight = 0;
                                                            objDetailInfo.numProductionCWWeight = 0;

                                                            objDetailInfo.BendingPos = "";

                                                            objDetailInfo.intShapeTransHeaderId = 0;
                                                            //' newly added when there is insert of productmarkinng when the intShapeTransHeaderId is null
                                                            if ((objDetailInfo.intShapeTransHeaderId <= 0))
                                                            {
                                                                objDetailInfo.intShapeId = intShapeCodeID;
                                                                objDetailInfo.vchShapeDescription = "";
                                                                objDetailInfo.nvchParamValues = paramValues.Replace(";", "ì");
                                                                objDetailInfo.intUserid = _intUserID;
                                                                objDetailInfo.intShapeTransHeaderId = objDetailDal.InsertShapeValFrmTC(objDetailInfo);
                                                            }
                                                            //if (vchMeshShape == "flat")
                                                            //{
                                                            //    objDetailInfo.ParamValues = "";
                                                            //}
                                                            //else
                                                            //{
                                                            //objDetailInfo.ParamValues = paramValues;
                                                            //}
                                                            objDetailInfo.intStructureElementTypeId = 5;
                                                            objDetailInfo.tntLayer = tntLayerID;
                                                            //To assign the previous end chainage as the start chainage of next
                                                            if (Convert.ToInt32(decLastEndChainage) == 0)
                                                            {
                                                                objDetailInfo.decStartChainage = Convert.ToDecimal(flStartChainage);
                                                            }
                                                            else
                                                            {
                                                                objDetailInfo.decStartChainage = decLastEndChainage;
                                                            }
                                                            if (IsCascade == true)
                                                            {
                                                                objDetailInfo.decEndChainage = Convert.ToDecimal(_endChainage);
                                                            }
                                                            else
                                                            {
                                                                objDetailInfo.decEndChainage = Convert.ToDecimal(alEnd_Chainage_DRN2[c].ToString());
                                                                decLastEndChainage = Convert.ToDecimal(alEnd_Chainage_DRN2[c].ToString());
                                                            }

                                                            if (vchLayer == "SL")
                                                            {
                                                                decLastSlabEndChainage = decLastEndChainage;
                                                            }
                                                            else
                                                            {
                                                                decLastBaseEndChainage = decLastEndChainage;
                                                            }
                                                            if (bitProduceIndicator)
                                                            {
                                                                objDetailInfo.nvchProduceIndicator = "Yes";
                                                            }
                                                            else
                                                            {
                                                                objDetailInfo.nvchProduceIndicator = "No";
                                                            }
                                                            int intOutput = objDetailDal.DrainProductMarkingDetails_InsUpd(objDetailInfo);

                                                            // added by anuran CHG0031337>>>>>>>>>>>>>>
                                                            if (intProductValidator == 16)
                                                            {
                                                                updatevalidator();
                                                                intProductValidator = 0;
                                                            }
                                                            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                                                            intProductMarkingCounter++;
                                                            bool blnBendChk = false;
                                                            if (blnBendInd)
                                                            {
                                                                if (!((vchMeshShape.Trim() == "flat") | (vchMeshShape.ToLower().Trim().StartsWith("clr") == true)))
                                                                {
                                                                    blnBendChk = BendingCheck(intOutput, intShapeCodeID, vchShape, intProduction_MO1.ToString(), intProduction_CO1.ToString(), paramValues, intProduction_MO2.ToString(), intProduction_CO2.ToString(), vchBendingGroup, bend_ind, vchMeshShape, "A");
                                                                }
                                                            }

                                                        }//End of meshShape not null
                                                    }//End of ignoreChainage if MaxDepth does not match 
                                                    else
                                                    {
                                                        decLastEndChainage = Convert.ToDecimal(alEnd_Chainage_DRN2[c].ToString()); ;
                                                    }
                                                }//End of ChainageDRN2 loop
                                            }//Depth and Chainage not equal
                                            else
                                            {
                                                //string strEndChn = alChainageDistance[intChainageCounter - 1].ToString();
                                                //strEndChn = strEndChn.Remove(strEndChn.LastIndexOf(".") + 3);
                                                //if (vchLayer == "SL")
                                                //{
                                                //    decLastSlabEndChainage = Convert.ToDecimal(strEndChn);
                                                //}
                                                //else
                                                //{
                                                //    decLastBaseEndChainage = Convert.ToDecimal(strEndChn);
                                                //}

                                                string strEndChn = alChainageDistance[intChainageCounter - 1].ToString();
                                                strEndChn = strEndChn.Remove(strEndChn.LastIndexOf(".") + 3);
                                                if (vchLayer == "SL")
                                                {
                                                    decLastSlabEndChainage = Convert.ToDecimal(strEndChn);
                                                }
                                                else
                                                {
                                                    decLastBaseEndChainage = Convert.ToDecimal(strEndChn);
                                                }



                                                if (intChainageCounter < intCurrentDepth)
                                                {
                                                    if (vchLayer == "SL")
                                                    {
                                                        decLastSlabEndChainage = Convert.ToDecimal(flStartChainage) + Convert.ToDecimal(strEndChn);
                                                    }
                                                    else
                                                    {
                                                        decLastBaseEndChainage = Convert.ToDecimal(flStartChainage) + Convert.ToDecimal(strEndChn);
                                                    }
                                                }


                                            }

                                        }//End of SL and BL
                                    }// End of StartDepth nad EndDepth Not Equal(Applied for Cascade prob)
                                }//End of ChainageDistance grater then 0

                            }//End of ChainageDistance Not Infinity

                        }
                        alStartDepth = null;
                        alEndDepth = null;
                        alChainageDistance = null;
                        alStartMainLength = null;
                        alEndMainLength = null;
                        alTotalMeshQty = null;


                    }//End Of LastLayer Not same as current Layer for SL and BL
                }//End of not same drain type

            }
            catch (Exception ex)
            {
                throw ex;//ErrorHandler.RaiseTactonErrorToFile(ex, strLogError, string.Empty);
            }
            finally
            {
                if (intConfigCounter > 0)
                {
                    //configuration.Factory.Shutdown();
                }
            }
            return false;
        }
        public string RoundToTwo(string length)
        {
            string strStartLen = length.ToString().Trim();
            if (length.IndexOf(".") > 0)
            {

                string strBeforeDecimal = strStartLen.Substring(0, strStartLen.IndexOf("."));
                string strAfterDecimal = strStartLen.Substring(strStartLen.IndexOf(".") + 1);
                string strAD1, strAD2 = "";
                if (strAfterDecimal.Length > 2)
                {
                    strAD1 = strAfterDecimal.Substring(0, 2);
                    strAD2 = strAfterDecimal.Substring(2);
                    if ((strAD2 == "5"))
                    {
                        int lastDigit = 0;
                        lastDigit = Convert.ToInt16(strAD1.Substring(1)) + 1;
                        strStartLen = strBeforeDecimal + "." + strAD1.Substring(0, 1) + lastDigit.ToString();
                    }
                    else
                    {
                        strStartLen = (decimal.Round(Convert.ToDecimal(strStartLen), 2)).ToString();
                    }
                }
                else
                {
                    strStartLen = (decimal.Round(Convert.ToDecimal(strStartLen), 2)).ToString();
                }
            }

            return strStartLen;
        }
        public string RoundLength(string length)
        {
            string strStartLen = length.ToString().Trim();
            if (length.IndexOf(".") > 0)
            {
                string strBeforeDecimal = strStartLen.Substring(0, strStartLen.IndexOf("."));
                string strAfterDecimal = strStartLen.Substring(strStartLen.IndexOf(".") + 1);
                string strAD1, strAD2 = "";
                if (strAfterDecimal.Length > 1)
                {
                    strAD1 = strAfterDecimal.Substring(0, 1);
                    strAD2 = strAfterDecimal.Substring(1);
                    if ((strAD2 == "0") || (strAD2 == "1") || (strAD2 == "2"))
                    {
                        strStartLen = strBeforeDecimal + "." + strAD1 + "0";
                    }
                    else if ((strAD2 == "3") || (strAD2 == "4") || (strAD2 == "5") || (strAD2 == "6") || (strAD2 == "7"))
                    {
                        strStartLen = strBeforeDecimal + "." + strAD1 + "5";
                    }
                    else
                    {
                        strStartLen = (decimal.Round(Convert.ToDecimal(strStartLen), 1)).ToString();
                    }
                }
            }
            return strStartLen;
        }

        public string TransportCheck(decimal actual_tonnage_post, int envelope_height_field, int envelope_width_field, int envelope_length_field)
        {
            try
            {
                int intParameterSet = 0;

                intParameterSet = _intParameterSet;
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
            catch //(Exception ex)
            {
                //ErrorHandler.RaiseError(ex, strLogError);
                //return "Pass";
                throw new Exception("Transport check failed");
            }
        }


        private Result TactonCommit(string paramName, string paramValue)
        {
            Result tactonResult = null;
            string tcxFilename = "";

            try
            {
               // tactonResult = configuration.Commit(paramName, paramValue);//by vidya
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
                        try
                        {
                            if (paramName.EndsWith("co2_field"))
                            {
                                isFailedCO2Commit = true;
                            }
                            else if (paramName.EndsWith("mo2_field"))
                            {
                                isFailedMO2Commit = true;
                            }
                            //throw new Exception("Tacton Commit Alert: " + paramName + " : " + paramValue + " in " + tcxFilename);
                        }
                        catch //(Exception ex)
                        {
                            //ErrorHandler.RaiseTactonErrorToFile(ex, strLogError, string.Empty);
                        }

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
        private bool BendingCheck(int ProductMarkId, int intShapeid, string chrShapeCode, string ProductMO1, string ProductCO1, string strParamValues, string strProductMO2, string strProductCO2, string vchBendingGroup, string chrMOCO, string vchShapeGroup, string mode)
        {
            bool bolResult = false;
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
                    if (strParamValues.Contains("ì"))
                    {
                        alBendChk.Add(strParamValues.Replace("ì", ";"));
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

                    //BendingCheck objBendingCheckSlab = new BendingCheckSlab.BendingCheck();//by vidya

                    //strStatus = (string)objBendingCheckSlab.BendingCheck(alBendChk);//by vidya
                   
                    strResult = strStatus.Split(',');
                    strBendingResult = strResult[0];
                    if (strBendingResult.Trim() == "Pass")
                    {
                        //if (mode == "A")
                        //{ }
                        //else
                        //{
                        //if (strResult.Length > 5)
                        //{
                        //    if ((strResult[5].ToString().Trim() == "OH") || (strResult[5].ToString().Trim() == "SS") || (strResult[5].ToString().Trim() == "WR"))
                        //    {
                        bolResult = true;
                        //    }
                        //    else
                        //    {
                        //        bolResult = true;
                        //    }
                        //}
                        //}

                    }
                    else
                    {
                        bolResult = false;
                    }
                    //if (mode == "A")
                    //{
                    if (!bolResult)
                    {
                        //ViewState("BendingAlert") = 1;
                    }
                    else if (strResult[5] == "OH")
                    {
                        SetOverhang(ref strResult[1], ref strResult[2], ref strResult[3], ref strResult[4], ProductMarkId, chrMOCO);
                    }
                    else if (strResult[5] == "SS")
                    {
                        SetSpaceShift(ref strResult[1], ref strResult[2], ref strResult[3], ref strResult[4], ref strResult[6], ref strResult[7], ProductMarkId, chrMOCO);
                    }
                    else if (strResult[5] == "WR")
                    {
                        if (strResult[0] == "Pass")
                        {
                            SetWireRemovalPass(ref strResult[6], ref strResult[7], ProductMarkId);
                        }
                        else
                        {
                            SetWireRemovalFail();
                        }
                    }
                    //}
                    //}
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Bending check failed");
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
            catch //(Exception ex)
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
        public int ReLoadOverHangs(string strSafeState, string vchMeshShape, string strMO1, string strMO2, string strCO1, string strCO2, string strProductMarkId, string strStructureMarkId, string strProductCodeId, string blnTC, string blnBC, string blnMC, string strMWSpacing, string strCWSpacing, string strShapeCode, string strProductCode,  int intShapeId, string strParam, string ProInd,string strErrorMesg, string bitOHVal, string strMWLen, string strCWLen, string stringProductMarking,int TotalQty)
        {
            int intResult = 0;
            int intProduction_MO1 = 0;
            int intProduction_MO2 = 0;
            int intProduction_CO1 = 0;
            int intProduction_CO2 = 0;
            int envelop_height = 0;
            int envelop_width = 0;
            int envelop_length = 0;
            decimal actual_tonnage = 0.0M;
            int intNoOfCW = 0;
            int intNoOfMW = 0;
            int intChainageMainLength = 0;
            int intCrossWireLength = 0;
            int MainWireDia = 0;
            int CrossWireDia = 0;

            Int16 intProductTypeId = 0;
            int intMWPrdLength = 0;
            int intCWPrdLength = 0;
            int shapeCodeId = 0;
            isFailedMO2Commit = false;
            isFailedCO2Commit = false;
            Dictionary<string, string> ExtractedValues = new Dictionary<string, string>();
            Dictionary<string, string> input_values = new Dictionary<string, string>();
            SlabDetailingComponent objInfo = new SlabDetailingComponent();
            try
            {
                if (strSafeState == null || strSafeState == "") throw new Exception("Safesate is empty");
                // LoadConfigurationFromSafeState(strSafeState);
                objInfo.ClearDictionary();
                ExtractedValues = GetDictionaryFromString(strSafeState);
                ExtractedValues.Remove("mo1");
                ExtractedValues.Remove("mo2");
                ExtractedValues.Remove("co1");
                ExtractedValues.Remove("co2");
                //foreach (KeyValuePair<string, string> item in ExtractedValues)
                //{
                //     if (ExtractedValues.ContainsKey("mw_length"))
                //     {
                //         input_values.Add("mw_length", ExtractedValues["mw_length"]);
                //     }
                //     if (ExtractedValues.ContainsKey("cw_length"))
                //     {
                //         input_values.Add("cw_length", ExtractedValues["cw_length"]);
                //     }
                //     if (ExtractedValues.ContainsKey("mw_spacing"))
                //     {
                //         input_values.Add("mw_spacing", ExtractedValues["mw_spacing"]);
                //     }
                //     if (ExtractedValues.ContainsKey("cw_spacing"))
                //     {
                //         input_values.Add("cw_spacing", ExtractedValues["cw_spacing"]);
                //     }
                //     if (ExtractedValues.ContainsKey("mw_dia"))
                //     {
                //         input_values.Add("mw_dia", ExtractedValues["mw_dia"]);
                //     }
                //     if (ExtractedValues.ContainsKey("cw_dia"))
                //     {
                //         input_values.Add("cw_dia", ExtractedValues["cw_dia"]);
                //     }
                //     if (ExtractedValues.ContainsKey("weight_per_area"))
                //     {
                //         input_values.Add("weight_per_area", ExtractedValues["weight_per_area"]);
                //     }
                //     if (ExtractedValues.ContainsKey("mw_weight_m_run"))
                //     {
                //         input_values.Add("mw_weight_m_run", ExtractedValues["mw_weight_m_run"]);
                //     }
                //     if (ExtractedValues.ContainsKey("cw_weight_m_run"))
                //     {
                //         input_values.Add("cw_weight_m_run", ExtractedValues["cw_weight_m_run"]);
                //     }
                //     if (ExtractedValues.ContainsKey("pin_dia"))
                //     {
                //         input_values.Add("pin_dia", ExtractedValues["pin_dia"]);
                //     }
                //     if (ExtractedValues.ContainsKey("ShapeCodeId"))
                //     {
                //         shapeCodeId= Int32.Parse( ExtractedValues["ShapeCodeId"]);
                //         input_values.Add("ShapeCodeId", ExtractedValues["ShapeCodeId"]);
                //     }
                //     if (ExtractedValues.ContainsKey("MeshShapeGroup"))
                //     {
                //         vchMeshShape = ExtractedValues["MeshShapeGroup"];
                //     }
                //// }
                if (ExtractedValues.ContainsKey("ShapeCodeId"))
                {
                    shapeCodeId = Int32.Parse(ExtractedValues["ShapeCodeId"]);
                }
                if (ExtractedValues.ContainsKey("MeshShapeGroup"))
                {
                    vchMeshShape = ExtractedValues["MeshShapeGroup"];
                }
                ExtractedValues.Add("mo1", strMO1);
                ExtractedValues.Add("mo2", strMO2);
                ExtractedValues.Add("co1", strCO1);
                ExtractedValues.Add("co2", strCO2);
                objInfo.FillInputDictionary(ExtractedValues);
                string convertedString = ConvertJsonFormat(ExtractedValues);
                Dictionary<string, string> SlabShape_Result = new Dictionary<string, string>();
                SlabShape_Result = objInfo.ExecuteDetailingComponent(shapeCodeId, null);

                if (!isFailedMO2Commit && !isFailedCO2Commit)
                {
                    objDetailInfo.intInvoiceMO1 = Convert.ToInt32(strMO1);
                    objDetailInfo.intInvoiceMO2 = Convert.ToInt32(strMO2);
                    objDetailInfo.intInvoiceCO1 = Convert.ToInt32(strCO1);
                    objDetailInfo.intInvoiceCO2 = Convert.ToInt32(strCO2);

                    objDetailInfo.xmlResult = convertedString;
                    //Group SlabShape_Result = (Group)configuration.Result.RootGroup.SubGroups["result_group"];
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
                            objDetailInfo.ParamValues = item.Value;
                        }

                    }//End of reading Result

                    objDetailInfo.numProductionMWLength = Convert.ToDecimal(intMWPrdLength);
                    objDetailInfo.numProductionCWLength = Convert.ToDecimal(intCWPrdLength);
                    //Rounding on basis of creep
                    string strBitOH = bitOHVal;
                    string strCreepMO1 = "0";
                    string strCreepCO1 = "0";
                    strCreepMO1 = bitOHVal.Split('@')[0].ToString().Trim();
                    strCreepCO1 = bitOHVal.Split('@')[1].ToString().Trim();
                    if (strCreepMO1 == "1")
                    {
                        objDetailInfo.intProductionMO1 = Convert.ToInt32(strMO1) - (Convert.ToInt32(strMWLen) - intMWPrdLength);
                        intProduction_MO1 = Convert.ToInt32(strMO1) - (Convert.ToInt32(strMWLen) - intMWPrdLength);
                        intProduction_MO2 = Convert.ToInt32(strMO2);
                        objDetailInfo.intProductionMO2 = intProduction_MO2;
                    }
                    else
                    {
                        intProduction_MO1 = Convert.ToInt32(strMO1);
                        objDetailInfo.intProductionMO1 = intProduction_MO1;
                        objDetailInfo.intProductionMO2 = Convert.ToInt32(strMO2) - (Convert.ToInt32(strMWLen) - intMWPrdLength);
                        intProduction_MO2 = Convert.ToInt32(strMO2) - (Convert.ToInt32(strMWLen) - intMWPrdLength);
                    }
                    if (strCreepCO1 == "1")
                    {
                        objDetailInfo.intProductionCO1 = Convert.ToInt32(strCO1) - (Convert.ToInt32(strCWLen) - intCWPrdLength);
                        intProduction_CO1 = Convert.ToInt32(strCO1) - (Convert.ToInt32(strCWLen) - intCWPrdLength);
                        //objDetailInfo.intProductionCO2 = Convert.ToInt32(strCO2) - (Convert.ToInt32(strCWLen) - intCWPrdLength);
                        intProduction_CO2 = Convert.ToInt32(strCO2);
                        objDetailInfo.intProductionCO2 = intProduction_CO2;
                    }
                    else
                    {
                        //objDetailInfo.intProductionCO1 = Convert.ToInt32(strCO1) - (Convert.ToInt32(strCWLen) - intCWPrdLength);
                        //intProduction_CO1 = Convert.ToInt32(strCO1) - (Convert.ToInt32(strCWLen) - intCWPrdLength);
                        objDetailInfo.intProductionCO2 = Convert.ToInt32(strCO2) - (Convert.ToInt32(strCWLen) - intCWPrdLength);
                        intProduction_CO2 = Convert.ToInt32(strCO2) - (Convert.ToInt32(strCWLen) - intCWPrdLength);
                        intProduction_CO1 = Convert.ToInt32(strCO1);
                        objDetailInfo.intProductionCO1 = intProduction_CO1;

                    }
                    if (Convert.ToBoolean(blnTC))
                    {
                        string TCResult = "";
                        //TCResult = TransportCheck(actual_tonnage, envelop_height, envelop_width, envelop_length);
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

                    Boolean TMC = false;
                    string bend_ind = "";
                    string twin_ind = "";
                    string vchBendingGroup = "";
                    bool blnBendInd = false;
                    if (Convert.ToBoolean(blnMC))
                    {
                        objDetailInfo.vchProductCode = strProductCode.Trim();
                        string strWireDia = "";
                        strWireDia = objDetailDal.DrainWireDiameter_Get(objDetailInfo);
                        if (strWireDia != "")
                        {
                            MainWireDia = Convert.ToInt32(strWireDia.Split('-')[0].Trim());
                            CrossWireDia = Convert.ToInt32(strWireDia.Split('-')[1].Trim());
                        }
                        //wireDia = Convert.ToInt32(strProductCode.Substring(1));
                        DataSet dsMcnChk = new DataSet();
                        DataTable dtMachineCheck = new DataTable();
                        objDetailInfo.chrShapeCode = strShapeCode.Trim();
                        objDetailInfo.intProductCodeId = Convert.ToInt32(strProductCodeId);
                       // dsMcnChk = objDetailDal.GetMachineCheckValues(objDetailInfo);
                        if (dsMcnChk.Tables.Count > 0)
                        {
                            dtMachineCheck = dsMcnChk.Tables[0];
                            if ((dtMachineCheck.Rows.Count > 0))
                            {
                                int intMWQty, intCWQty;
                                //string bend_ind, twin_ind;
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
                                //TMC = objTMC.CheckMachineFeasibility(strTcxPath + "TMC.tcx", bend_ind, twin_ind, Convert.ToInt32(MainWireDia), Convert.ToInt32(CrossWireDia), intChainageMainLength, intCrossWireLength, intNoOfMW, intNoOfCW, Convert.ToInt32(strMWSpacing), Convert.ToInt32(strCWSpacing), intMWQty, intCWQty, intProduction_MO1, intProduction_MO2, intProduction_CO1, intProduction_CO2);

                            }
                        }
                    }
                    else
                    {
                        TMC = false;
                    }
                    if (TMC == false)
                    {
                        //Added by anuran
                        //throw new Exception("Machine Check Failed.");
                        //errorMesg = "Machine Check Failed.";
                        //return -1;
                    }
                    objDetailInfo.tntGenerationStatus = 1;
                    objDetailInfo.bitMachineCheckIndicator = TMC;
                    objDetailInfo.bitBendIndicator = Convert.ToBoolean(blnBC);
                    objDetailInfo.vchBendCheckResult = "P";
                    objDetailInfo.intDrainStructureMarkId = Convert.ToInt32(strStructureMarkId);
                    objDetailInfo.intMeshProductMarkId = Convert.ToInt32(strProductMarkId);
                    objDetailInfo.intUserid = _intUserID;

                    objDetailInfo.vchProductMarkingName = stringProductMarking;
                    //if (blnProInd = true)
                    //{
                    objDetailInfo.nvchProduceIndicator = ProInd.Trim();
                    //}
                    //else
                    //{
                    //    objDetailInfo.nvchProduceIndicator = "No";
                    //}
                    objDetailInfo.intInvoiceTotalQty = TotalQty;
                    intResult = objDetailDal.UpdateDrainProductMarking(objDetailInfo);
                    string paramValues = strParam.Replace(";", "ì");
                    bool blnBendChk = true;
                    if (blnBendInd)
                    {
                        if (!((vchMeshShape.Trim() == "flat") | (vchMeshShape.ToLower().Trim().StartsWith("clr") == true)))
                        {
                            //blnBendChk = BendingCheck(intResult, intShapeId, vchShape, intProduction_MO1.ToString(), intProduction_CO1.ToString(), paramValues, intProduction_MO2.ToString(), intProduction_CO2.ToString(), vchBendingGroup, bend_ind, vchMeshShape,"U");
                            //string strElementType = "drain";
                            //blnBendChk = BendingCheckNew(Convert.ToInt32(strProductMarkId), vchMeshShape, strElementType, ConfigBC, vchBendingGroup, intShapeId);
                            //if (blnBendChk == false)
                            //{
                            //    //changed by anuran
                            //    //errorMesg = "Bending check failed. Please do necessary amendments in BOM.";
                            //    updatevalidator();
                            //}
                        }
                    }
                }
                else
                {
                    intResult = -1;
                }
            }
            catch (Exception ex)
            {
                //ErrorHandler.RaiseError(ex, strLogError, string.Empty);
                throw new Exception("Error occured while updating product marking");
                //throw new Exception(ex.ToString());
            }
            finally
            {
                //configuration.Factory.Shutdown();
                //configuration.Factory.Reset();
                strErrorMesg = errorMesg;
            }
            strErrorMesg = errorMesg;
            return intResult;
        }
        int intStatus = 0;

        int intCWShapeid = 0;
        string MWBendingGroup = "";
        string CWBendingGroup = "";
        string strDoubleBend = "";

        private bool BendingCheckNew(int ProductMarkId, string vchShapeGroup, string strElementType, WebConfiguration ConfigBC, string vchBendingGroup, int intShapeid)
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
                    BendingCheckNew(ProductMarkId, vchShapeGroup, strElementType, ConfigBC, vchBendingGroup, intShapeid);
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
                        string strShapeShiftTCXfilename = strSlabTcxPath.Trim() + "Bending_Check_" + strBendingGroup.Trim() + ".tcx";
                        bolResult = objBendingCheck.CommitBendingCheck(strShapeShiftTCXfilename, ConfigBC);
                    }

                    //' ''Newly added for Double shape
                    if (intStatus == 1)
                    {
                        BendingCheckNew(ProductMarkId, vchShapeGroup, strElementType, ConfigBC, vchBendingGroup, intShapeid);
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
                throw new Exception("Bending check failed");
            }
            return bolResult;
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



        #region ValuesFromStructureMarking

        //Drain StructureMarking Parameters
        private string _vchGroupMarkName;
        private int _intDrainStructureMarkId;
        private int _intParameterSet;
        private string _structMarkingName;
        private string _txtMemQty;
        private int _tntStructureRevNo;
        private int _intUserID;

        private bool _bitBendingChk = false;
        private bool _bitMachineChk = false;
        private bool _bitTransportChk = false;
        private bool _bitProduceIndicator = true;
        private WebConfiguration _TMCConfig;

        //private string _bitOHVal;

        //private WebConfiguration _Config;
        //public WebConfiguration Config
        //{
        //    get { return _Config; }
        //    set { _Config = value; }
        //}
        public int intUserID
        {
            get { return _intUserID; }
            set { _intUserID = value; }
        }
        public WebConfiguration TMCConfig
        {
            get { return _TMCConfig; }
            set { _TMCConfig = value; }
        }

        public bool bitBendingChk
        {
            get { return _bitBendingChk; }
            set { _bitBendingChk = value; }
        }
        public bool bitMachineChk
        {
            get { return _bitMachineChk; }
            set { _bitMachineChk = value; }
        }
        public bool bitProduceIndicator
        {
            get { return _bitProduceIndicator; }
            set { _bitProduceIndicator = value; }
        }
        public bool bitTransportChk
        {
            get { return _bitTransportChk; }
            set { _bitTransportChk = value; }
        }
        //Parameter Set For Product Marking
        private int _drainTopCover = -1;
        private int _drainBottomCover = -1;
        private int _drainOuterCover = -1;
        private int _drainInnerCover = -1;
        private float _drainOuterCrossWire = 2.4F;
        private float _drainInnerCrossWire = 2.4F;
        private float _drainSlabCrossWire = 2.4F;
        private float _drainBaseCrossWire = 2.4F;

        //For Cascade
        private int _intCascadeNo = 0;
        private float _flCsDropHeight = 0.0F;
        private float _flCsWidth = 0.0F;
        private float _flCsCrossLength = 0.0F;
        private bool _isCascade = false;

        //User Input
        private float _startChainage = -1;
        private float _endChainage = -1;
        private float _startTopLevel = -1;
        private float _endTopLevel = -1;
        private float _startInvertLevel = -1;
        private float _endInvertLevel = -1;
        private float _startDepth = 0;
        private float _endDepth = 0;

        //Drain StructureMarking Parameters
        //public string bitOHVal
        //{
        //    get { return _bitOHVal; }
        //    set { _bitOHVal = value; }
        //}
        public string vchGroupMarkName
        {
            get { return _vchGroupMarkName; }
            set { _vchGroupMarkName = value; }
        }
        public int intDrainStructureMarkId
        {
            get { return _intDrainStructureMarkId; }
            set { _intDrainStructureMarkId = value; }
        }
        public int intParameterSet
        {
            get { return _intParameterSet; }
            set { _intParameterSet = value; }
        }
        public string structMarkingName
        {
            get { return _structMarkingName; }
            set { _structMarkingName = value; }
        }
        public string txtMemQty
        {
            get { return _txtMemQty; }
            set { _txtMemQty = value; }
        }
        public int tntStructureRevNo
        {
            get { return _tntStructureRevNo; }
            set { _tntStructureRevNo = value; }
        }

        //Parameter Set
        public int drainTopCover
        {
            get { return _drainTopCover; }
            set { _drainTopCover = value; }
        }
        public int drainBottomCover
        {
            get { return _drainBottomCover; }
            set { _drainBottomCover = value; }
        }
        public int drainOuterCover
        {
            get { return _drainOuterCover; }
            set { _drainOuterCover = value; }
        }
        public int drainInnerCover
        {
            get { return _drainInnerCover; }
            set { _drainInnerCover = value; }
        }
        public float drainOuterCrossWire
        {
            get { return _drainOuterCrossWire; }
            set { _drainOuterCrossWire = value; }
        }
        public float drainInnerCrossWire
        {
            get { return _drainInnerCrossWire; }
            set { _drainInnerCrossWire = value; }
        }
        public float drainSlabCrossWire
        {
            get { return _drainSlabCrossWire; }
            set { _drainSlabCrossWire = value; }
        }
        public float drainBaseCrossWire
        {
            get { return _drainBaseCrossWire; }
            set { _drainBaseCrossWire = value; }
        }

        //For Cascade
        public float flCsDropHeight
        {
            get { return _flCsDropHeight; }
            set { _flCsDropHeight = value; }
        }
        public float flCsWidth
        {
            get { return _flCsWidth; }
            set { _flCsWidth = value; }
        }
        public float flCsCrossLength
        {
            get { return _flCsCrossLength; }
            set { _flCsCrossLength = value; }
        }
        public int intCascadeNo
        {
            get { return _intCascadeNo; }
            set { _intCascadeNo = value; }
        }
        public bool IsCascade
        {
            get { return _isCascade; }
            set { _isCascade = value; }
        }


        //User Input    

        public float startChainage
        {
            get { return _startChainage; }
            set { _startChainage = value; }
        }
        public float endChainage
        {
            get { return _endChainage; }
            set { _endChainage = value; }
        }
        public float startTopLevel
        {
            get { return _startTopLevel; }
            set { _startTopLevel = value; }
        }
        public float endTopLevel
        {
            get { return _endTopLevel; }
            set { _endTopLevel = value; }
        }
        public float startInvertLevel
        {
            get { return _startInvertLevel; }
            set { _startInvertLevel = value; }
        }
        public float endInvertLevel
        {
            get { return _endInvertLevel; }
            set { _endInvertLevel = value; }
        }
        public float startDepth
        {
            get { return _startDepth; }
            set { _startDepth = value; }
        }
        public float endDepth
        {
            get { return _endDepth; }
            set { _endDepth = value; }
        }
        #endregion


        public void updatevalidator()
        {

            ////// Added by Anuran CHG0031337>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            string PdMrkNam1 = objDetailInfo.vchProductMarkingName.ToString();
            Int32 PdCd1 = objDetailInfo.intProductCodeId;
            int PrdValdCd1 = 16;

            Int32 intReturnValue1 = 0;
            try
            {
                //System.Data.Common.DbCommand dbcom1;
                //dbcom1 = DataAccess.DataAccess.db.GetStoredProcCommand("Updt_DrainProductValidator");
                //DataAccess.DataAccess.db.AddInParameter(dbcom1, "@ProductMarkingName", DbType.String, PdMrkNam1);
                //DataAccess.DataAccess.db.AddInParameter(dbcom1, "@ProductCodeId", DbType.Int32, PdCd1);
                //DataAccess.DataAccess.db.AddInParameter(dbcom1, "@ProductValidator", DbType.Int32, PrdValdCd1);

                //intReturnValue1 = DataAccess.DataAccess.ExecuteNonQuery(dbcom1);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int updatevalidator_byprdmrkid(int PrdMrkId)
        {

            try
            {
                Int32 intReturnValue1 = 0;
                //System.Data.Common.DbCommand dbcom2; 
                //dbcom2 = DataAccess.DataAccess.db.GetStoredProcCommand("Updt_DrainProductValidator_byprdmrkid");
                //DataAccess.DataAccess.db.AddInParameter(dbcom2, "@ProductMarkId", DbType.Int64, PrdMrkId);
                //intReturnValue1 = DataAccess.DataAccess.ExecuteNonQuery(dbcom2);

                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@ProductMarkId",PrdMrkId);
                    
                    intReturnValue1 = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.Updt_DrainProductValidator_byprdmrkid, dynamicParameters, commandType: CommandType.StoredProcedure);

                    sqlConnection.Close();
                    return intReturnValue1;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public string ConvertJsonFormat(Dictionary<string, string> inputs)
        {
            //DrainJsonFormat DrainJsonObj = new DrainJsonFormat();
            string dictionaryString = "{";
            foreach (KeyValuePair<string, string> item in inputs)
            {
                dictionaryString += item.Key + " : " + item.Value + ", ";
            }
            return dictionaryString.TrimEnd(',', ' ') + "}";
        }

        public Dictionary<string, string> GetDictionaryFromString(string ConcatedString)
        {
            Dictionary<string, string> OutputDictionary = new Dictionary<string, string>();
            try
            {
                string trimstartstr = ConcatedString.TrimStart('{', ' ');
                string trimendstr = trimstartstr.TrimEnd('}', ' ');
                string[] arrSplit = trimendstr.Split(',');
                string[] arrKeyValuePair;
                for (int i = 0; i < arrSplit.Length; i++)
                {
                    string KeyValuePair = arrSplit[i].Trim();
                    arrKeyValuePair = KeyValuePair.Split(':');
                    if (!OutputDictionary.ContainsKey(arrKeyValuePair[0].Trim()))
                    {
                        OutputDictionary.Add(arrKeyValuePair[0].Trim(), arrKeyValuePair[1].Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return OutputDictionary;
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
    }

    //class DrainInputs
    //{


    //}

    //class DrainJsonFormat
    //{
    //    public string ParamName;
    //    public string ParamValue;
    //}

}
