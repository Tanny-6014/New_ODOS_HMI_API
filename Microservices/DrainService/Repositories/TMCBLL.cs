

using System.Web;
using System.Configuration;
using System.Data;
using DrainService.Constants;
using System.Collections.Specialized;
using System.Xml;
using Tacton.Configurator.Public;

namespace DrainService.Repositories
{
    public class TMCBLL
    {
      
        //private readonly IConfiguration _configuration;
        //private string connectionString;

        //public TMCBLL(IConfiguration configuration)
        //{
        //    //_dbContext = dbContext;
        //    _configuration = configuration;
        //    connectionString = _configuration.GetConnectionString(SystemConstants.DefaultDBConnection);
        //}

        public TMCBLL()
        {
        }

       // DataAccessor accessor = null;
        NameValueCollection properties = null;
       // WebConfiguration configuration = null;

        public DetailDal objDetailDal = new DetailDal();


        //private string TCServerName = ConfigurationSettings.AppSettings["Tacton.Configurator.servers"];
       
        public string TCServerName { get; set; }


        private string LoadConfiguration(string strPath, WebConfiguration refconfiguration)
        {
            string returnSafeState = "";
            try
            {
                //accessor = new FileAccessor();
                //properties = new NameValueCollection();
                //properties.Add("Tacton.Configurator.remoteServers", "true");
                //properties.Add("Tacton.Configurator.servers", TCServerName);

                ////factory = new Factory(accessor, properties);
                ////configuration = new WebConfiguration();
                //configuration = refconfiguration;

                //// Create and use a configuration object
                ////configuration = new Configuration(factory);

                //configuration.Instance.Filter = "groups;parameters;report";

                //configuration.Instance.StartConfiguration(strPath, null);

                //returnSafeState = configuration.Instance.SafeState;

            }
            catch (Exception ex)
            {
            }
            return returnSafeState;
        }

        //private Result TactonCommit(string paramName, string paramValue)
        //{
        //    Result tactonResult = null;
        //    string tcxFilename = "";

        //    try
        //    {
        //        tactonResult = null;//configuration.Instance.Commit(paramName, paramValue);
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
        //                throw new Exception("Tacton Commit Alert: " + paramName + " : " + paramValue + " in " + tcxFilename);
        //            }
        //            else
        //            {
        //                throw new Exception("Tacton Commit Error: " + paramName + " : " + paramValue + " in " + tcxFilename);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return tactonResult;
        //}


        public int GetValue(string key, Dictionary<string, string> input)
        {
            if (input.ContainsKey(key))
            {
                return Convert.ToInt32(Decimal.Parse(input[key]));
            }
            else
            {
                return 0;
            }
        }

        //Modified by Aishwarya Patil- Remove Tacton Dependency
        public bool CheckMachineFeasibility(string tcxFilename, string bend_ind, string twin_ind,
                    int mw_dia, int cw_dia, int mw_length, int cw_length,
                    int mw_qty, int cw_qty, int mw_space, int cw_space, int mw_bend_qty,
                    int cw_bend_qty, int mo1, int mo2, int co1, int co2, WebConfiguration ConfigTMC)
        {

            bool isPassed = false;

            DataSet dsTMCCheck;

            string numMinMWDiameter = "";
            string numMaxMWDiameter = "";
            string numMinCWDiameter = "";
            string numMaxCWDiameter = "";
            string intMinMWSpace = "";
            string intMaxMWSpace = "";
            string intMinCWSpace = "";
            string intMaxCWSpace = "";
            string intMinMO1 = "";
            string intMaxMO1 = "";
            string intMinMO2 = "";
            string intMaxMO2 = "";
            string intTotalMinMO1MO2 = "";
            string intTotalMaxMO1MO2 = "";
            string intMinCO1 = "";
            string intMaxCO1 = "";
            string intMinCO2 = "";
            string intMaxCO2 = "";
            string intTotalMinCO1CO2 = "";
            string intTotalMaxCO1CO2 = "";
            string numMinMWLength = "";
            string numMaxMWLength = "";
            string numMinCWLength = "";
            string numMaxCWLength = "";
            string sitMWWelds = "";
            string strShape = "";
            SlabDetailingComponent objInfo = new SlabDetailingComponent();
            Dictionary<string, string> inputs = new Dictionary<string, string>();
            try
            {
                //KA Change - Brought to front.
                dsTMCCheck = objDetailDal.GetMachineLimits(strShape);

                if (dsTMCCheck.Tables.Count > 0)
                {
                    if (dsTMCCheck.Tables[0].Rows.Count > 0)
                    {
                        int i = 0;
                        int _intMinMO1 = Convert.ToInt32(dsTMCCheck.Tables[0].Rows[i]["intMinMO1"]);
                        int _intMinMO2 = Convert.ToInt32(dsTMCCheck.Tables[0].Rows[i]["intMinMO2"]);
                        int _intMinCO1 = Convert.ToInt32(dsTMCCheck.Tables[0].Rows[i]["intMinCO1"]);
                        int _intMinCO2 = Convert.ToInt32(dsTMCCheck.Tables[0].Rows[i]["intMinCO2"]);
                        double _numMaxMWLength = Convert.ToDouble(dsTMCCheck.Tables[0].Rows[i]["numMaxMWLength"]);
                        double _numMaxCWLength = Convert.ToDouble(dsTMCCheck.Tables[0].Rows[i]["numMaxCWLength"]);

                        //Changed by KA to get the values from Master table.
                        if (mo1 < _intMinMO1 || mo2 < _intMinMO2 || co1 < _intMinCO1 || co2 < _intMinCO2)
                            return false;
                        else if (mw_length > _numMaxMWLength || cw_length > _numMaxMWLength)
                            return false;
                        else if (mw_length <= _numMaxMWLength && cw_length <= _numMaxCWLength)
                            return true;
                        else if (cw_length <= _numMaxMWLength && mw_length <= _numMaxCWLength)
                            return true;
                        else
                            return false;
                    }
                }


                //LoadConfiguration(tcxFilename, ConfigTMC);

                if (bend_ind == "F")
                {
                    strShape = "F";
                    bend_ind = "M";
                }

                for (int z = 0; z < 2; z++)
                {
                    if (z == 0)
                    {
                        //configuration.Instance.UnCommit("tmc_bendind_field");
                        //configuration.Instance.UnCommit("tmc_twin_indicator_field");
                        //configuration.Instance.UnCommit("tmc_mw_diameter_field");
                        //configuration.Instance.UnCommit("tmc_cw_diameter_field");
                        //configuration.Instance.UnCommit("tmc_mw_length_field");
                        //configuration.Instance.UnCommit("tmc_cw_length_field");
                        //configuration.Instance.UnCommit("tmc_main_wire_qty_field");
                        //configuration.Instance.UnCommit("tmc_cross_wire_qty_field");
                        //configuration.Instance.UnCommit("tmc_mw_spacing_field");
                        //configuration.Instance.UnCommit("tmc_cw_spacing_field");
                        //configuration.Instance.UnCommit("tmc_no_of_bends_mw_field");
                        //configuration.Instance.UnCommit("tmc_no_of_bends_cw_field");
                        //configuration.Instance.UnCommit("tmc_mo1_field");
                        //configuration.Instance.UnCommit("tmc_mo2_field");
                        //configuration.Instance.UnCommit("tmc_co1_field");
                        //configuration.Instance.UnCommit("tmc_co2_field");

                        objInfo.ClearDictionary();

                        inputs.Add("tmc_bendind_field", bend_ind.ToString().Trim());
                        inputs.Add("tmc_twin_indicator_field", twin_ind);
                        inputs.Add("tmc_mw_diameter_field", mw_dia.ToString().Trim());
                        inputs.Add("tmc_cw_diameter_field", cw_dia.ToString().Trim());
                        inputs.Add("tmc_mw_length_field", mw_length.ToString().Trim());
                        inputs.Add("tmc_cw_length_field", cw_length.ToString().Trim());
                        inputs.Add("tmc_main_wire_qty_field", mw_qty.ToString().Trim());
                        inputs.Add("tmc_cross_wire_qty_field", cw_qty.ToString().Trim());
                        inputs.Add("tmc_mw_spacing_field", mw_space.ToString().Trim());
                        inputs.Add("tmc_cw_spacing_field", cw_space.ToString().Trim());
                        inputs.Add("tmc_no_of_bends_mw_field", mw_bend_qty.ToString().Trim());
                        inputs.Add("tmc_no_of_bends_cw_field", cw_bend_qty.ToString().Trim());
                        inputs.Add("tmc_mo1_field", mo1.ToString().Trim());
                        inputs.Add("tmc_mo2_field", mo2.ToString().Trim());
                        inputs.Add("tmc_co1_field", co1.ToString().Trim());
                        inputs.Add("tmc_co2_field", co2.ToString().Trim());
                    }
                    else
                    {
                        switch (bend_ind.ToString().Trim())
                        {
                            case "M":
                                bend_ind = "C";
                                inputs.Add("tmc_bendind_field", bend_ind.ToString().Trim());
                                break;
                            case "C":
                                bend_ind = "M";
                                inputs.Add("tmc_bendind_field", bend_ind.ToString().Trim());
                                break;
                            default:
                                inputs.Add("tmc_bendind_field", bend_ind.ToString().Trim());
                                break;
                        }

                        inputs.Add("tmc_twin_indicator_field", twin_ind);
                        inputs.Add("tmc_mw_diameter_field", cw_dia.ToString().Trim());
                        inputs.Add("tmc_cw_diameter_field", mw_dia.ToString().Trim());
                        inputs.Add("tmc_mw_length_field", cw_length.ToString().Trim());
                        inputs.Add("tmc_cw_length_field", mw_length.ToString().Trim());
                        inputs.Add("tmc_main_wire_qty_field", cw_qty.ToString().Trim());
                        inputs.Add("tmc_cross_wire_qty_field", mw_qty.ToString().Trim());
                        inputs.Add("tmc_mw_spacing_field", cw_space.ToString().Trim());
                        inputs.Add("tmc_cw_spacing_field", mw_space.ToString().Trim());
                        inputs.Add("tmc_no_of_bends_mw_field", cw_bend_qty.ToString().Trim());
                        inputs.Add("tmc_no_of_bends_cw_field", mw_bend_qty.ToString().Trim());
                        inputs.Add("tmc_mo1_field", co1.ToString().Trim());
                        inputs.Add("tmc_mo2_field", co2.ToString().Trim());
                        inputs.Add("tmc_co1_field", mo1.ToString().Trim());
                        inputs.Add("tmc_co2_field", mo2.ToString().Trim());
                    }
                    //CommitProgramInput();


                    if (dsTMCCheck.Tables.Count > 0)
                    {
                        if (dsTMCCheck.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsTMCCheck.Tables[0].Rows.Count; i++)
                            {
                                numMinMWDiameter = dsTMCCheck.Tables[0].Rows[i]["numMinMWDiameter"].ToString();
                                numMaxMWDiameter = dsTMCCheck.Tables[0].Rows[i]["numMaxMWDiameter"].ToString();
                                numMinCWDiameter = dsTMCCheck.Tables[0].Rows[i]["numMinCWDiameter"].ToString();
                                numMaxCWDiameter = dsTMCCheck.Tables[0].Rows[i]["numMaxCWDiameter"].ToString();
                                intMinMWSpace = dsTMCCheck.Tables[0].Rows[i]["intMinMWSpace"].ToString();
                                intMaxMWSpace = dsTMCCheck.Tables[0].Rows[i]["intMaxMWSpace"].ToString();
                                intMinCWSpace = dsTMCCheck.Tables[0].Rows[i]["intMinCWSpace"].ToString();
                                intMaxCWSpace = dsTMCCheck.Tables[0].Rows[i]["intMaxCWSpace"].ToString();
                                intMinMO1 = dsTMCCheck.Tables[0].Rows[i]["intMinMO1"].ToString();
                                intMaxMO1 = dsTMCCheck.Tables[0].Rows[i]["intMaxMO1"].ToString();
                                intMinMO2 = dsTMCCheck.Tables[0].Rows[i]["intMinMO2"].ToString();
                                intMaxMO2 = dsTMCCheck.Tables[0].Rows[i]["intMaxMO2"].ToString();
                                intTotalMinMO1MO2 = dsTMCCheck.Tables[0].Rows[i]["intTotalMinMO1MO2"].ToString();
                                intTotalMaxMO1MO2 = dsTMCCheck.Tables[0].Rows[i]["intTotalMaxMO1MO2"].ToString();
                                intMinCO1 = dsTMCCheck.Tables[0].Rows[i]["intMinCO1"].ToString();
                                intMaxCO1 = dsTMCCheck.Tables[0].Rows[i]["intMaxCO1"].ToString();
                                intMinCO2 = dsTMCCheck.Tables[0].Rows[i]["intMinCO2"].ToString();
                                intMaxCO2 = dsTMCCheck.Tables[0].Rows[i]["intMaxCO2"].ToString();
                                intTotalMinCO1CO2 = dsTMCCheck.Tables[0].Rows[i]["intTotalMinCO1CO2"].ToString();
                                intTotalMaxCO1CO2 = dsTMCCheck.Tables[0].Rows[i]["intTotalMaxCO1CO2"].ToString();
                                numMinMWLength = dsTMCCheck.Tables[0].Rows[i]["numMinMWLength"].ToString();
                                numMaxMWLength = dsTMCCheck.Tables[0].Rows[i]["numMaxMWLength"].ToString();
                                numMinCWLength = dsTMCCheck.Tables[0].Rows[i]["numMinCWLength"].ToString();
                                numMaxCWLength = dsTMCCheck.Tables[0].Rows[i]["numMaxCWLength"].ToString();
                                sitMWWelds = dsTMCCheck.Tables[0].Rows[i]["sitMWWelds"].ToString();


                                inputs.Add("tmc_min_mw_diameter_field", numMinMWDiameter);
                                inputs.Add("tmc_min_cw_diameter_field", numMinCWDiameter);
                                inputs.Add("tmc_max_mw_diameter_field", numMaxMWDiameter);
                                inputs.Add("tmc_max_cw_diameter_field", numMaxCWDiameter);
                                inputs.Add("tmc_min_mw_space_field", intMinMWSpace);
                                inputs.Add("tmc_min_cw_space_field", intMinCWSpace);
                                inputs.Add("tmc_max_mw_space_field", intMaxMWSpace);
                                inputs.Add("tmc_max_cw_space_field", intMaxCWSpace);
                                inputs.Add("tmc_min_mo1_field", intMinMO1);
                                inputs.Add("tmc_max_mo1_field", intMaxMO1);
                                inputs.Add("tmc_min_mo2_field", intMinMO2);
                                inputs.Add("tmc_max_mo2_field", intMaxMO2);
                                inputs.Add("tmc_min_co1_field", intMinCO1);
                                inputs.Add("tmc_max_co1_field", intMaxCO1);
                                inputs.Add("tmc_min_co2_field", intMinCO2);
                                inputs.Add("tmc_max_co2_field", intMaxCO2);
                                inputs.Add("tmc_total_min_mo_field", intTotalMinMO1MO2);
                                inputs.Add("tmc_total_max_mo_field", intTotalMaxMO1MO2);
                                inputs.Add("tmc_total_min_co_field", intTotalMinCO1CO2);
                                inputs.Add("tmc_total_max_co_field", intTotalMaxCO1CO2);
                                inputs.Add("tmc_min_mw_length_field", numMinMWLength);
                                inputs.Add("tmc_max_mw_length_field", numMaxMWLength);
                                inputs.Add("tmc_min_cw_length_field", numMinCWLength);
                                inputs.Add("tmc_max_cw_length_field", numMaxCWLength);
                                inputs.Add("tmc_welds_field", sitMWWelds);
                                inputs.Add("tmc_bends_field", sitMWWelds);

                                //objInfo.FillInputDictionary(inputs);
                                int no_of_shape_bends_mw_value = 0;
                                int no_of_shape_bends_cw_value = 0;
                                List<bool> resultList = new List<bool>();
                                List<bool> resultList1 = new List<bool>();
                                string mw_bending_result_value = "";
                                string cw_bending_result_value = "";
                                string bending_machine_check_result_value = "";
                                int no_of_bends_mw = GetValue("tmc_no_of_bends_mw_field", inputs);
                                int no_of_bends_cw = GetValue("tmc_no_of_bends_cw_field", inputs);
                                int mw_length_value = GetValue("tmc_mw_length_field", inputs);
                                int mw_diameter_value = GetValue("tmc_mw_diameter_field", inputs);
                                int mw_spacing_value = GetValue("tmc_mw_spacing_field", inputs);
                                int mo1_value = GetValue("tmc_mo1_field", inputs);
                                int mo2_value = GetValue("tmc_mo2_field", inputs);

                                int min_mw_length = GetValue("tmc_min_mw_length_field", inputs);
                                int max_mw_length_value = GetValue("tmc_max_mw_length_field", inputs);
                                int min_mw_diameter = GetValue("tmc_min_mw_diameter_field", inputs);
                                int max_mw_diameter = GetValue("tmc_max_mw_diameter_field", inputs);
                                int min_mw_space = GetValue("tmc_min_mw_space_field", inputs);
                                int max_mw_space = GetValue("tmc_max_mw_space_field", inputs);
                                int total_min_mo = GetValue("tmc_total_min_mo_field", inputs);
                                int total_max_mo = GetValue("tmc_total_max_mo_field", inputs);
                                int min_mo2 = GetValue("tmc_min_mo2_field", inputs);
                                int max_mo2 = GetValue("tmc_max_mo2_field", inputs);
                                int max_mo1 = GetValue("tmc_max_mo1_field", inputs);
                                int min_mo1 = GetValue("tmc_min_mo1_field", inputs);


                                int min_cw_length = GetValue("tmc_min_cw_length_field", inputs);
                                int max_cw_length = GetValue("tmc_max_cw_length_field", inputs);
                                int min_cw_diameter = GetValue("tmc_min_cw_diameter_field", inputs);
                                int max_cw_diameter = GetValue("tmc_max_cw_diameter_field", inputs);
                                int min_cw_space = GetValue("tmc_min_cw_space_field", inputs);
                                int max_cw_space = GetValue("tmc_max_cw_space_field", inputs);
                                int min_co1 = GetValue("tmc_min_co1_field", inputs);
                                int max_co1 = GetValue("tmc_max_co1_field", inputs);
                                int min_co2 = GetValue("tmc_min_co2_field", inputs);
                                int max_co2 = GetValue("tmc_max_co2_field", inputs);
                                int total_min_co = GetValue("tmc_total_min_co_field", inputs);
                                int total_max_co = GetValue("tmc_total_max_co_field", inputs);

                                int cw_length_value = GetValue("tmc_cw_length_field", inputs);
                                int cw_diameter_value = GetValue("tmc_cw_diameter_field", inputs);
                                int cw_spacing_value = GetValue("tmc_cw_spacing_field", inputs);
                                int co1_value = GetValue("tmc_co1_field", inputs);
                                int co2_value = GetValue("tmc_co2_field", inputs);
                                //bendind=B 
                                //and ((no_of_shape_bends_mw=<machine_code.bends) 
                                //and (machine_code.min_mw_length=<mw_length and mw_length=<machine_code.max_mw_length) 
                                //and (machine_code.min_mw_diameter=<product_code.mw_diameter and product_code.mw_diameter=<machine_code.max_mw_diameter) 
                                //and (machine_code.min_mw_space=<product_code.mw_spacing and product_code.mw_spacing=<machine_code.max_mw_space) 
                                //and (machine_code.min_mo1=<mo1 and mo1=<machine_code.max_mo1) 
                                //and (machine_code.min_mo2=<mo2 and mo2=<machine_code.max_mo2) 
                                //and (mo1+mo2>=machine_code.total_min_mo and mo1+mo2=<machine_code.total_max_mo))->mw_bending_result=Yes

                                //bendind=B and twinindicator=Yes->no_of_shape_bends_mw=2*no_of_bends_mw and no_of_shape_bends_cw=2*no_of_bends_cw
                                //bendind=B and twinindicator=No->no_of_shape_bends_mw=no_of_bends_mw and no_of_shape_bends_cw=no_of_bends_cw
                                if (inputs.ContainsKey("tmc_bendind_field").ToString().Trim().ToUpper() == "B")
                                {
                                    if (twin_ind == "Yes")
                                    {
                                        no_of_shape_bends_mw_value = 2 * no_of_bends_mw;
                                        no_of_shape_bends_cw_value = 2 * no_of_bends_cw;
                                    }
                                    else if (twin_ind == "No")
                                    {
                                        no_of_shape_bends_mw_value = no_of_bends_mw;
                                        no_of_shape_bends_cw_value = no_of_bends_cw;
                                    }
                                    if ((no_of_shape_bends_mw_value <= Int32.Parse(sitMWWelds))
                                     && (min_mw_length <= mw_length_value && mw_length_value <= max_mw_length_value)
                                     && (min_mw_diameter <= mw_diameter_value && mw_diameter_value <= max_mw_diameter)
                                     && (min_mw_space <= mw_spacing_value && mw_spacing_value <= max_mw_space)
                                     && (min_mo1 <= mo1_value && mo1_value <= max_mo1)
                                     && (min_mo2 <= mo2_value && mo2_value <= max_mo2)
                                     && ((mo1_value + mo2_value) >= total_min_mo && (mo1_value + mo2_value) <= total_max_mo))
                                    {
                                        mw_bending_result_value = "Yes";
                                    }
                                    else
                                    {
                                        mw_bending_result_value = "No";
                                    }



                                    //bendind=B 
                                    //and ((no_of_shape_bends_cw=<machine_code.bends) 
                                    //and (machine_code.min_cw_length=<cw_length and cw_length=<machine_code.max_cw_length) 
                                    //and (machine_code.min_cw_diameter=<product_code.cw_diameter and product_code.cw_diameter=<machine_code.max_cw_diameter) 
                                    //and (machine_code.min_cw_space=<product_code.cw_spacing and product_code.cw_spacing=<machine_code.max_cw_space) 
                                    //and (machine_code.min_co1=<co1 and co1=<machine_code.max_co1) 
                                    //and (machine_code.min_co2=<co2 and co2=<machine_code.max_co2) 
                                    //and (co1+co2>=machine_code.total_min_co and co1+co2=<machine_code.total_max_co))->cw_bending_result=Yes

                                    if (no_of_shape_bends_cw_value <= Int32.Parse(sitMWWelds)
                                        && (min_cw_length <= cw_length_value && cw_length_value <= max_cw_length)
                                        && (min_cw_diameter <= cw_diameter_value && cw_diameter_value <= max_cw_diameter)
                                        && (min_cw_space <= cw_spacing_value && cw_spacing_value <= max_cw_space)
                                        && (min_co1 <= co1_value && co1_value <= max_co1)
                                        && (min_co2 <= co2_value && co2_value <= max_co2)
                                        && ((co1_value + co2_value) >= total_min_co && (co1_value + co2_value) <= total_max_co))
                                    {
                                        cw_bending_result_value = "Yes";
                                    }
                                    else
                                    {
                                        cw_bending_result_value = "No";
                                    }
                                }
                                else if (inputs.ContainsKey("tmc_bendind_field").ToString().Trim().ToUpper() == "M")
                                {
                                    int no_of_shape_bends_value = 0;
                                    int no_of_bends_cw_value = 0;
                                    // bendind=M and 
                                    //((no_of_shape_bends=<machine_code.bends) 
                                    //and (machine_code.min_mw_length=<mw_length and mw_length=<machine_code.max_mw_length) 
                                    //and (machine_code.min_cw_length=<cw_length and cw_length=<machine_code.max_cw_length) 
                                    //and (machine_code.min_mw_diameter=<product_code.mw_diameter and product_code.mw_diameter=<machine_code.max_mw_diameter) 
                                    //and (machine_code.min_cw_diameter=<product_code.cw_diameter and product_code.cw_diameter=<machine_code.max_cw_diameter) 
                                    //and (machine_code.min_mw_space=<product_code.mw_spacing and product_code.mw_spacing=<machine_code.max_mw_space) 
                                    //and (machine_code.min_cw_space=<product_code.cw_spacing and product_code.cw_spacing=<machine_code.max_cw_space) 
                                    //and (machine_code.min_mo1=<mo1 and mo1=<machine_code.max_mo1) 
                                    //and (machine_code.min_mo2=<mo2 and mo2=<machine_code.max_mo2) 
                                    //and (mo1+mo2>=machine_code.total_min_mo and mo1+mo2=<machine_code.total_max_mo) 
                                    //and (machine_code.min_co1=<co1 and co1=<machine_code.max_co1) 
                                    //and (machine_code.min_co2=<co2 and co2=<machine_code.max_co2) 
                                    //and (co1+co2>=machine_code.total_min_co and co1+co2=<machine_code.total_max_co))->mw_bending_result=Yes
                                    if (twin_ind == "Yes")
                                    {
                                        no_of_shape_bends_value = 2 * no_of_bends_mw;
                                        no_of_bends_cw_value = 0;
                                    }
                                    else if (twin_ind == "No")
                                    {
                                        no_of_shape_bends_value = no_of_bends_mw;
                                        no_of_bends_cw_value = 0;
                                    }
                                    if (no_of_shape_bends_value <= Int32.Parse(sitMWWelds)
                                        && (min_mw_length <= mw_length_value && mw_length_value <= max_mw_length_value)
                                        && (min_cw_length <= cw_length_value && cw_length <= max_cw_length)
                                        && (min_mw_diameter <= mw_diameter_value && mw_diameter_value <= max_mw_diameter)
                                        && (min_cw_diameter <= cw_diameter_value && cw_diameter_value <= max_cw_diameter)
                                        && (min_mw_space <= mw_spacing_value && mw_spacing_value <= max_mw_space)
                                        && (min_cw_space <= cw_spacing_value && cw_spacing_value <= max_cw_space)
                                        && (min_mo1 <= mo1_value && mo1_value <= max_mo1)
                                        && (min_mo2 <= mo2_value && mo2_value <= max_mo2)
                                        && ((mo1_value + mo2_value) >= total_min_mo && (mo1_value + mo2_value) <= total_max_mo)
                                        && (min_co1 <= co1_value && co1_value <= max_co1)
                                        && (min_co2 <= co2_value && co2_value <= max_co2)
                                        && ((co1_value + co2_value) >= total_min_co && (co1_value + co2_value) <= total_max_co))
                                    {
                                        mw_bending_result_value = "Yes";
                                    }
                                    else
                                    {
                                        mw_bending_result_value = "No";
                                    }
                                }
                                else if (inputs.ContainsKey("tmc_bendind_field").ToString().Trim().ToUpper() == "C")
                                {
                                    //bendind=C 
                                    //and ((no_of_shape_bends=<machine_code.bends) 
                                    //and (machine_code.min_mw_length=<mw_length and mw_length=<machine_code.max_mw_length) 
                                    //and (machine_code.min_cw_length=<cw_length and cw_length=<machine_code.max_cw_length) 
                                    //and (machine_code.min_mw_diameter=<product_code.mw_diameter and product_code.mw_diameter=<machine_code.max_mw_diameter) 
                                    //and (machine_code.min_cw_diameter=<product_code.cw_diameter and product_code.cw_diameter=<machine_code.max_cw_diameter) 
                                    //and (machine_code.min_mw_space=<product_code.mw_spacing and product_code.mw_spacing=<machine_code.max_mw_space) 
                                    //and (machine_code.min_cw_space=<product_code.cw_spacing and product_code.cw_spacing=<machine_code.max_cw_space) 
                                    //and (machine_code.min_mo1=<mo1 and mo1=<machine_code.max_mo1) 
                                    //and (machine_code.min_mo2=<mo2 and mo2=<machine_code.max_mo2) 
                                    //and (mo1+mo2>=machine_code.total_min_mo and mo1+mo2=<machine_code.total_max_mo) 
                                    //and (machine_code.min_co1=<co1 and co1=<machine_code.max_co1) 
                                    //and (machine_code.min_co2=<co2 and co2=<machine_code.max_co2) 
                                    //and (co1+co2>=machine_code.total_min_co and co1+co2=<machine_code.total_max_co))->cw_bending_result=Yes
                                    int no_of_shape_bends_value = 0;
                                    int no_of_bends_mw_value = 0;
                                    //no_of_bends_cw=no_of_shape_bends and no_of_bends_mw=0
                                    if (twin_ind == "Yes")
                                    {
                                        no_of_shape_bends_value = 2 * no_of_bends_cw;
                                        no_of_bends_mw_value = 0;
                                    }
                                    else if (twin_ind == "No")
                                    {
                                        no_of_shape_bends_value = no_of_bends_cw;
                                        no_of_bends_mw_value = 0;
                                    }
                                    if (no_of_shape_bends_value <= Int32.Parse(sitMWWelds)
                                        && (min_mw_length <= mw_length_value && mw_length_value <= max_mw_length_value)
                                        && (min_cw_length <= cw_length_value && cw_length_value <= max_cw_length)
                                        && (min_mw_diameter <= mw_diameter_value && mw_diameter_value <= max_mw_diameter)
                                        && (min_cw_diameter <= cw_diameter_value && cw_diameter_value <= max_cw_diameter)
                                        && (min_mw_space <= mw_spacing_value && mw_spacing_value <= max_mw_space)
                                        && (min_cw_space <= cw_spacing_value && cw_spacing_value <= max_cw_space)
                                        && (min_mo1 <= mo1_value && mo1_value <= max_mo1)
                                        && (min_mo2 <= mo2_value && mo2_value <= max_mo2)
                                        && ((mo1_value + mo2_value) >= total_min_mo && (mo1_value + mo2_value) <= total_max_mo)
                                        && (min_co1 <= co1_value && co1_value <= max_co1)
                                        && (min_co2 <= co2_value && co2_value <= max_co2))
                                    {
                                        cw_bending_result_value = "Yes";
                                    }
                                    else
                                    {
                                        cw_bending_result_value = "No";
                                    }
                                }
                                Dictionary<string, string> resultGroup = new Dictionary<string, string>();

                                if (inputs.ContainsKey("tmc_bendind_field").ToString().Trim().ToUpper() == "M" && mw_bending_result_value == "Yes")
                                {
                                    bending_machine_check_result_value = "Yes";
                                }
                                else if (inputs.ContainsKey("tmc_bendind_field").ToString().Trim().ToUpper() == "B" && mw_bending_result_value == "Yes" && cw_bending_result_value == "Yes")
                                {
                                    bending_machine_check_result_value = "Yes";
                                }
                                else if (inputs.ContainsKey("tmc_bendind_field").ToString().Trim().ToUpper() == "C" && cw_bending_result_value == "Yes")
                                {
                                    bending_machine_check_result_value = "Yes";
                                }
                                if (!resultGroup.ContainsKey("bend_machine_check_field"))
                                {
                                    resultGroup.Add("bend_machine_check_field", bending_machine_check_result_value);
                                }


                                //resultProductMarking = objInfo.ExecuteDetailingComponent(shapeId, "");
                                //result 
                                // Group resultGroup = (Group)configuration.Instance.Result.RootGroup.SubGroups["result_group"];
                                foreach (KeyValuePair<string, string> res in resultGroup)
                                {
                                    if (res.Key == "bend_machine_check_field")
                                    {
                                        if (res.Value.ToUpper() == "YES") return true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                //configuration.Factory.Shutdown();
                //configuration.Instance.Factory.Reset();
            }
            return isPassed;
        }

        private void CommitProgramInput()
        {

        }

    }

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
}
