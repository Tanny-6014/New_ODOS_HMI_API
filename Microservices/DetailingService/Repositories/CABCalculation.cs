using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml.Linq;
using System.IO;
using System.Reflection;
using ExpressionParser;
using DetailingService.Constants;
using Dapper;
using Microsoft.Data.SqlClient;
using NatSteel.NDS.BLL;
using MathParser = NatSteel.NDS.BLL.MathParser;

namespace DetailingService.Repositories
{
    public class CABCalculation
    {
        private readonly IConfiguration _configuration;
        //private string connectionString= "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";
        private object dbManager;
        private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";
        public CABCalculation(IConfiguration configuration)
        {
            //_dbContext = dbContext;
            _configuration = configuration;

            connectionString = _configuration.GetConnectionString(SystemConstant.DefaultDBConnection);
        }
        public string ShapeId { get; set; }
        public string ShapeCode { get; set; }
        public string dia { get; set; }
        public string pin { get; set; }
        public List<ShapeParameter> ShapeParametersList { get; set; }
        public List<Accessory> accList { get; set; }

        public CABCalculation()
        {
        }

        /// <summary>
        /// Method to get the production and invoice length.
        /// </summary>
        /// <param name="errMsg"></param>
        /// <param name="r_intInvLen"></param>
        /// <param name="r_intProdLen"></param>
        /// <param name="r_intTotBend"></param>
        /// <param name="r_intTotArc"></param>
        public List<Accessory> ProductionInvLength(out string errMsg, out int r_intInvLen, out int r_intProdLen, out int r_intTotBend, out int r_intTotArc, out string bvbs)
        {
            List<Accessory> accessories;
            //DBManager dbManager = new DBManager();
            DataSet ds = new DataSet();
            DataTable dtShapeDetails = new DataTable();
            //dbManager.Open();
            string[] arrLength = { "ACTUAL", "NORMAL", "OFFSET", "OFFSET+NORMAL" };
            string[] arr3Dlength = { "UPDOT", "UPARROW", "UPRIGHT", "UPLEFT", "DNDOT", "DNARROW", "DNRIGHT", "DNLEFT" };
            string[] arrAngle = { "ANGLE" };
            try
            {
                int intActlen = 0;
                int intArcLen = 0;
                int intArcRad = 0;
                int intArcSwAng = 0;
                float sngActLen = 0;
                r_intInvLen = 0;
                r_intProdLen = 0;
                r_intTotBend = 0;
                r_intTotArc = 0;
                bvbs = string.Empty;
                accList = new List<Accessory>();

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@CHRSHAPECODE", this.ShapeCode.Trim());
                    //ds = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "USP_GET_PRODUCTIONLENGTH_FORMULA_CUBE");

                    var dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = new SqlCommand(SystemConstant.USP_GET_PRODUCTIONLENGTH_FORMULA_CUBE, sqlConnection);
                    dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dataAdapter.SelectCommand.Parameters.AddRange(dynamicParameters.ParameterNames.Select(name => new SqlParameter(name, dynamicParameters.Get<object>(name))).ToArray());
                    dataAdapter.Fill(ds);
                    //ds = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.USP_GET_PRODUCTIONLENGTH_FORMULA_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);

                }

                
                if (ds.Tables.Count > 0)
                {

                    dtShapeDetails = ds.Tables[0];
                    #region 2D Production Length
                    if (!(dtShapeDetails.Rows[0]["CSC_CAT_DESC"].ToString().ToUpper().Equals("3-D") && dtShapeDetails.Rows[0]["CSD_VIEW"].ToString().ToUpper().Equals("PLAN")) && !(dtShapeDetails.Rows[0]["CSC_CAT_DESC"].ToString().ToUpper().Equals("3-D") && dtShapeDetails.Rows[0]["CSD_VIEW"].ToString().ToUpper().Equals("ELEV")) && !(dtShapeDetails.Rows[0]["CSC_CAT_DESC"].ToString().ToUpper().Equals("SPECIAL") && dtShapeDetails.Rows[0]["CSD_VIEW"].ToString().ToUpper().Equals("PLAN")) && !(dtShapeDetails.Rows[0]["CSC_CAT_DESC"].ToString().ToUpper().Equals("SPECIAL") && dtShapeDetails.Rows[0]["CSD_VIEW"].ToString().ToUpper().Equals("ELEV")))
                    {
                        for (int rowinc = 0; rowinc <= dtShapeDetails.Rows.Count - 1; rowinc++)
                        {
                            string parameterValue = string.Empty;
                            for (Int16 row = 0;     row <= ShapeParametersList.Count - 1; row++)
                            {
                                if (dtShapeDetails.Rows[rowinc]["CSD_SEQ_NO"].ToString() == ShapeParametersList[row].SequenceNumber.ToString())
                                {
                                    if (ShapeParametersList[row].symmetricIndex.ToString().Equals("0"))
                                    {
                                        parameterValue = ShapeParametersList[row].ParameterValueCab;
                                        break;
                                    }
                                    else
                                    {
                                        //Get the parameter val for the symmetric element.
                                        ShapeParameter obj = new ShapeParameter();
                                        obj = ShapeParametersList.FirstOrDefault(x => x.ParameterName.ToString().Trim() == ShapeParametersList[row].symmetricIndex.ToString().Trim());
                                        parameterValue = obj.ParameterValueCab;
                                        obj = null;
                                        break;
                                    }
                                }
                            }
                            #region Length
                            if (dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("LENGTH"))
                            {
                                if (Array.IndexOf(arrLength, dtShapeDetails.Rows[rowinc]["CSD_INPUT_TYPE"].ToString().ToUpper()) >= 0)
                                {
                                    intActlen = Convert.ToInt32(parameterValue.ToString());
                                }

                                r_intInvLen += intActlen;
                                r_intProdLen += intActlen;
                            }
                            #endregion
                            #region Angle
                            else if (dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("ANGLE") || dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("ANGLE_3D"))
                            {
                                if (rowinc != 0)
                                {
                                    //r_intTotBend += 1;
                                    if (Convert.ToInt32(parameterValue) < 90)
                                    {
                                        r_intProdLen -= Convert.ToInt32((2 * ((Convert.ToDouble(this.pin) / 2) + Convert.ToDouble(this.dia)) * Convert.ToDouble(Math.Tan(Convert.ToDouble(parameterValue) * Math.PI / 360))) - Convert.ToDouble((Math.PI * Convert.ToDouble(parameterValue) * ((Convert.ToDouble(this.pin) / 2) + (0.5 * Convert.ToDouble(this.dia)))) / 180));
                                    }
                                    else if (Convert.ToInt32(parameterValue) == 90)
                                    {
                                        r_intProdLen -= Convert.ToInt32(1.215 * Convert.ToDouble(this.dia)) + Convert.ToInt32(0.215 * Convert.ToDouble(this.pin));
                                    }
                                    else if (Convert.ToInt32(parameterValue) > 90)
                                    {
                                        r_intProdLen -= Convert.ToInt32(2 * ((Convert.ToDouble(this.pin) / 2) + Convert.ToDouble(this.dia)) - Convert.ToDouble((Math.PI * Convert.ToDouble(parameterValue) * ((Convert.ToDouble(this.pin) / 2) + (0.5 * Convert.ToDouble(this.dia)))) / 180));
                                    }
                                }
                            }
                            #endregion
                            #region Arc
                            else if (dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("ARC"))
                            {
                                r_intTotArc += 1;
                                string char2 = string.Empty;
                                int arc_prodLen = 0;
                                int arc_invLen = 0;

                                char2 = dtShapeDetails.Rows[rowinc]["CSD_INPUT_TYPE"].ToString().ToUpper();

                                if (char2.Equals("RAD+ARCLENGTH"))
                                {
                                    intArcLen = Convert.ToInt32(parameterValue);
                                    arc_prodLen = Convert.ToInt32((0.57 * intArcLen) - (1.57 * Convert.ToDouble(this.dia)));
                                    arc_invLen = intArcLen;
                                }
                                else if (char2.Equals("RAD+SW_ANGLE"))
                                {
                                }
                                else if (char2.Equals("DIA+SW_ANGLE"))
                                {
                                    intArcLen = Convert.ToInt32((Convert.ToDouble(parameterValue) * Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR2"]) * Math.PI) / 360.0);
                                    if (Convert.ToInt32(dtShapeDetails.Rows[rowinc]["CSD_PAR2"].ToString()) > 90)
                                    {
                                        arc_prodLen = intArcLen - Convert.ToInt32((2 * ((Convert.ToDouble(this.pin) / 2) + Convert.ToDouble(this.dia)) * Convert.ToDouble(Math.Tan(Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR2"].ToString()) * Math.PI / 360))) - Convert.ToDouble((Math.PI * Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR2"].ToString()) * ((Convert.ToDouble(this.pin) / 2) + (0.5 * Convert.ToDouble(this.dia)))) / 180));
                                    }
                                    else
                                    {
                                        arc_prodLen = intArcLen - Convert.ToInt32(2 * ((Convert.ToDouble(this.pin) / 2) + Convert.ToDouble(this.dia)) - Convert.ToDouble((Math.PI * Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR2"].ToString()) * ((Convert.ToDouble(this.pin) / 2) + (0.5 * Convert.ToDouble(this.dia)))) / 180));
                                    }
                                    arc_invLen = intArcLen;
                                }
                                else if (char2.Equals("CHORD+NORMAL"))
                                {
                                    intArcRad = Convert.ToInt32(((Convert.ToDouble(parameterValue) * Convert.ToDouble(parameterValue)) + (4.0 * Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR3"]) * Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR3"]))) / (8.0 * Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR3"])));
                                    if (Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR3"]) <= intArcRad)
                                    {
                                        intArcSwAng = Convert.ToInt32(2.0 * Math.Asin(Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR3"]) / (2.0 * intArcRad)));
                                    }
                                    else
                                    {
                                        intArcSwAng = Convert.ToInt32(2.0 * (Math.PI - Math.Asin(Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR3"]) / (2.0 * intArcRad))));
                                    }
                                    intArcLen = intArcRad * intArcSwAng;
                                    arc_invLen = intArcLen;
                                    arc_prodLen = intArcLen;
                                }
                                else if (char2.Equals("OTHER"))
                                {
                                    intArcLen = Convert.ToInt32(parameterValue);
                                    arc_invLen = intArcLen;
                                    arc_prodLen = intArcLen;
                                }
                                r_intProdLen += arc_prodLen;
                                r_intInvLen += arc_invLen;
                            }
                            #endregion
                            #region ARC Radius
                            else if (dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("ARC LENGTH"))
                            {
                                string char2 = string.Empty;
                                int arc_prodLen = 0;
                                int arc_invLen = 0;

                                char2 = dtShapeDetails.Rows[rowinc - 1]["CSD_INPUT_TYPE"].ToString();

                                if (char2.ToUpper().Equals("RAD+SW_ANGLE"))
                                {
                                    int seqNo = Convert.ToInt32(dtShapeDetails.Rows[rowinc]["CSD_SEQ_NO"].ToString());
                                    var paramVal = (from item in ShapeParametersList
                                                    where item.SequenceNumber == seqNo - 1
                                                    select item.ParameterValueCab).ToList();
                                    if (paramVal.Count > 0)
                                    {
                                        intArcLen = Convert.ToInt32(paramVal[0].ToString());
                                        arc_prodLen = Convert.ToInt32((intArcLen * (Convert.ToDouble(parameterValue) + (Convert.ToDouble(this.dia) / 2))) / (Convert.ToDouble(parameterValue) + Convert.ToDouble(this.dia)));
                                        arc_invLen = intArcLen;
                                    }
                                }
                                if (char2.ToUpper().Equals("RAD+RIGHTANGLE"))
                                {
                                    int seqNo = Convert.ToInt32(dtShapeDetails.Rows[rowinc]["CSD_SEQ_NO"].ToString());
                                    var paramVal = (from item in ShapeParametersList
                                                    where item.SequenceNumber == seqNo - 1
                                                    select item.ParameterValueCab).ToList();
                                    if (paramVal.Count > 0)
                                    {
                                        intArcLen = 0;
                                        arc_prodLen = -(2 * Convert.ToInt32(parameterValue)) + Convert.ToInt32((Math.PI / 2) * (Convert.ToDouble(parameterValue) - (Convert.ToDouble(this.dia) / 2)));
                                        arc_invLen = intArcLen;
                                    }
                                }
                                r_intProdLen += arc_prodLen;
                                r_intInvLen += arc_invLen;
                            }
                            #endregion
                            #region Coupler
                            else if (dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("ESPLICE") || dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("NSPLICE") || dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("DEXTRA"))
                            {
                                if (parameterValue.ToString() != "")
                                {
                                    DataSet dsCouplerVal = new DataSet();
                                    using (var sqlConnection = new SqlConnection(connectionString))
                                    {
                                        sqlConnection.Open();
                                        var dynamicParameters = new DynamicParameters();
                                        dynamicParameters.Add("@COUPLERWITHDIA", parameterValue.ToString().Trim() + this.dia.ToString());
                                        //dsCouplerVal = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "USP_GET_COUPLER_DATA_CUBE");

                                        var dataAdapter = new SqlDataAdapter();
                                        dataAdapter.SelectCommand = new SqlCommand(SystemConstant.USP_GET_COUPLER_DATA_CUBE, sqlConnection);
                                        dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                                        dataAdapter.SelectCommand.Parameters.AddRange(dynamicParameters.ParameterNames.Select(name => new SqlParameter(name, dynamicParameters.Get<object>(name))).ToArray());
                                        dataAdapter.Fill(dsCouplerVal);
                                        //dsCouplerVal = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.USP_GET_COUPLER_DATA_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);

                                    }
                                    
                                    if (dsCouplerVal.Tables[0].Rows.Count > 0)
                                    {
                                        r_intProdLen -= Convert.ToInt32(dsCouplerVal.Tables[0].Rows[0]["B"].ToString());
                                        r_intProdLen += Convert.ToInt32(dsCouplerVal.Tables[0].Rows[0]["A"].ToString()) + Convert.ToInt32(dsCouplerVal.Tables[0].Rows[0]["C"].ToString());
                                        foreach (DataRow dr in dsCouplerVal.Tables[0].Rows)
                                        {
                                            Accessory objAcc = new Accessory
                                            {
                                                CouplerType = dr["COUPLER_TYPE"].ToString(),
                                                SAPMaterialCode = dr["MATERIAL"].ToString(),
                                                MaterialType = dr["MATERIAL_TYPE"].ToString(),
                                                BitIsCoupler = 1,
                                                standard = dr["COUPLER_STANDARD"].ToString()
                                            };
                                            accList.Add(objAcc);
                                        }
                                    }
                                    dsCouplerVal.Dispose();
                                }
                            }
                            #endregion
                            #region 3D Length
                            if (Array.IndexOf(arr3Dlength, dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper()) >= 0)
                            {
                                sngActLen = float.Parse(parameterValue.ToString());
                                intActlen = Convert.ToInt32(parameterValue.ToString());

                                r_intInvLen += intActlen;
                                r_intProdLen += intActlen;
                            }
                            #endregion
                        }
                    }
                    #endregion
                    #region 3D Production length
                    else
                    {
                        if (!(dtShapeDetails.Rows[0]["CSC_CAT_DESC"].ToString().Trim().ToUpper().Equals("SPECIAL")))
                        {
                            if (dtShapeDetails.Rows[0]["CSD_PRODLENGTH"].ToString().Trim().ToUpper().Equals("Y") || dtShapeDetails.Rows[0]["CSD_PRODLENGTH"].ToString().Trim().ToUpper().Equals("N"))
                            {
                                #region Plan elevation with indicator
                                for (int rowinc = 0; rowinc <= dtShapeDetails.Rows.Count - 1; rowinc++)
                                {
                                    string parameterValue = string.Empty;
                                    for (Int16 row = 0; row <= ShapeParametersList.Count - 1; row++)
                                    {
                                        if (dtShapeDetails.Rows[rowinc]["CSD_SEQ_NO"].ToString() == ShapeParametersList[row].SequenceNumber.ToString())
                                        {
                                            if (ShapeParametersList[row].symmetricIndex.ToString().Equals("0"))
                                            {
                                                parameterValue = ShapeParametersList[row].ParameterValueCab;
                                                break;
                                            }
                                            else
                                            {
                                                //Get the parameter val for the symmetric element.
                                                ShapeParameter obj = new ShapeParameter();
                                                obj = ShapeParametersList.FirstOrDefault(x => x.ParameterName.ToString().Trim() == ShapeParametersList[row].symmetricIndex.ToString().Trim());
                                                parameterValue = obj.ParameterValueCab;
                                                obj = null;
                                                break;
                                            }
                                        }
                                    }
                                    #region Length
                                    if (dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("LENGTH") && dtShapeDetails.Rows[rowinc]["CSD_PRODLENGTH"].ToString().Trim().ToUpper().Equals("Y"))
                                    {
                                        if (Array.IndexOf(arrLength, dtShapeDetails.Rows[rowinc]["CSD_INPUT_TYPE"].ToString().ToUpper()) >= 0)
                                        {
                                            intActlen = Convert.ToInt32(parameterValue.ToString());
                                        }

                                        r_intInvLen += intActlen;
                                        r_intProdLen += intActlen;
                                    }
                                    #endregion
                                    #region Angle
                                    else if ((dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("ANGLE") && dtShapeDetails.Rows[rowinc]["CSD_PRODLENGTH"].ToString().Trim().ToUpper().Equals("Y")) || (dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("ANGLE_3D") && dtShapeDetails.Rows[rowinc]["CSD_PRODLENGTH"].ToString().Trim().ToUpper().Equals("Y")))
                                    {
                                        if (rowinc != 0)
                                        {
                                            if (Convert.ToInt32(parameterValue) < 90)
                                            {
                                                r_intProdLen -= Convert.ToInt32((2 * ((Convert.ToDouble(this.pin) / 2) + Convert.ToDouble(this.dia)) * Convert.ToDouble(Math.Tan(Convert.ToDouble(parameterValue) * Math.PI / 360))) - Convert.ToDouble((Math.PI * Convert.ToDouble(parameterValue) * ((Convert.ToDouble(this.pin) / 2) + (0.5 * Convert.ToDouble(this.dia)))) / 180));
                                            }
                                            else if (Convert.ToInt32(parameterValue) == 90)
                                            {
                                                r_intProdLen -= Convert.ToInt32(1.215 * Convert.ToDouble(this.dia)) + Convert.ToInt32(0.215 * Convert.ToDouble(this.pin));
                                            }
                                            else if (Convert.ToInt32(parameterValue) > 90)
                                            {
                                                r_intProdLen -= Convert.ToInt32(2 * ((Convert.ToDouble(this.pin) / 2) + Convert.ToDouble(this.dia)) - Convert.ToDouble((Math.PI * Convert.ToDouble(parameterValue) * ((Convert.ToDouble(this.pin) / 2) + (0.5 * Convert.ToDouble(this.dia)))) / 180));
                                            }
                                        }
                                    }
                                    #endregion
                                    #region Arc
                                    else if (dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("ARC") && dtShapeDetails.Rows[rowinc]["CSD_PRODLENGTH"].ToString().Trim().ToUpper().Equals("Y"))
                                    {
                                        r_intTotArc += 1;
                                        string char2 = string.Empty;
                                        int arc_prodLen = 0;
                                        int arc_invLen = 0;

                                        char2 = dtShapeDetails.Rows[rowinc]["CSD_INPUT_TYPE"].ToString().ToUpper();

                                        if (char2.Equals("RAD+ARCLENGTH"))
                                        {
                                            intArcLen = Convert.ToInt32(parameterValue);
                                            arc_prodLen = Convert.ToInt32((0.57 * intArcLen) - (1.57 * Convert.ToDouble(this.dia)));
                                            arc_invLen = intArcLen;
                                        }
                                        else if (char2.Equals("RAD+SW_ANGLE"))
                                        {
                                        }
                                        else if (char2.Equals("DIA+SW_ANGLE"))
                                        {
                                            intArcLen = Convert.ToInt32((Convert.ToDouble(parameterValue) * Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR2"]) * Math.PI) / 360.0);
                                            if (Convert.ToInt32(dtShapeDetails.Rows[rowinc]["CSD_PAR2"].ToString()) > 90)
                                            {
                                                arc_prodLen = intArcLen - Convert.ToInt32((2 * ((Convert.ToDouble(this.pin) / 2) + Convert.ToDouble(this.dia)) * Convert.ToDouble(Math.Tan(Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR2"].ToString()) * Math.PI / 360))) - Convert.ToDouble((Math.PI * Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR2"].ToString()) * ((Convert.ToDouble(this.pin) / 2) + (0.5 * Convert.ToDouble(this.dia)))) / 180));
                                            }
                                            else
                                            {
                                                arc_prodLen = intArcLen - Convert.ToInt32(2 * ((Convert.ToDouble(this.pin) / 2) + Convert.ToDouble(this.dia)) - Convert.ToDouble((Math.PI * Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR2"].ToString()) * ((Convert.ToDouble(this.pin) / 2) + (0.5 * Convert.ToDouble(this.dia)))) / 180));
                                            }
                                            arc_invLen = intArcLen;
                                        }
                                        else if (char2.Equals("CHORD+NORMAL"))
                                        {
                                            intArcRad = Convert.ToInt32(((Convert.ToDouble(parameterValue) * Convert.ToDouble(parameterValue)) + (4.0 * Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR3"]) * Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR3"]))) / (8.0 * Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR3"])));
                                            if (Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR3"]) <= intArcRad)
                                            {
                                                intArcSwAng = Convert.ToInt32(2.0 * Math.Asin(Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR3"]) / (2.0 * intArcRad)));
                                            }
                                            else
                                            {
                                                intArcSwAng = Convert.ToInt32(2.0 * (Math.PI - Math.Asin(Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR3"]) / (2.0 * intArcRad))));
                                            }
                                            intArcLen = intArcRad * intArcSwAng;
                                            arc_invLen = intArcLen;
                                            arc_prodLen = intArcLen;
                                        }
                                        else if (char2.Equals("OTHER"))
                                        {
                                            intArcLen = Convert.ToInt32(parameterValue);
                                            arc_invLen = intArcLen;
                                            arc_prodLen = intArcLen;
                                        }
                                        r_intProdLen += arc_prodLen;
                                        r_intInvLen += arc_invLen;
                                    }
                                    #endregion
                                    #region ARC Radius
                                    else if (dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("ARC LENGTH"))
                                    {
                                        string char2 = string.Empty;
                                        int arc_prodLen = 0;
                                        int arc_invLen = 0;

                                        char2 = dtShapeDetails.Rows[rowinc - 1]["CSD_INPUT_TYPE"].ToString();

                                        if (char2.ToUpper().Equals("RAD+SW_ANGLE") && dtShapeDetails.Rows[rowinc - 1]["CSD_PRODLENGTH"].ToString().Trim().ToUpper().Equals("Y"))
                                        {
                                            int seqNo = Convert.ToInt32(dtShapeDetails.Rows[rowinc]["CSD_SEQ_NO"].ToString());
                                            var paramVal = (from item in ShapeParametersList
                                                            where item.SequenceNumber == seqNo - 1
                                                            select item.ParameterValueCab).ToList();
                                            if (paramVal.Count > 0)
                                            {
                                                intArcLen = Convert.ToInt32(paramVal[0].ToString());
                                                arc_prodLen = Convert.ToInt32((intArcLen * (Convert.ToDouble(parameterValue) + (Convert.ToDouble(this.dia) / 2))) / (Convert.ToDouble(parameterValue) + Convert.ToDouble(this.dia)));
                                                arc_invLen = intArcLen;
                                            }
                                        }
                                        if (char2.ToUpper().Equals("RAD+RIGHTANGLE") && dtShapeDetails.Rows[rowinc - 1]["CSD_PRODLENGTH"].ToString().Trim().ToUpper().Equals("Y"))
                                        {
                                            int seqNo = Convert.ToInt32(dtShapeDetails.Rows[rowinc]["CSD_SEQ_NO"].ToString());
                                            var paramVal = (from item in ShapeParametersList
                                                            where item.SequenceNumber == seqNo - 1
                                                            select item.ParameterValueCab).ToList();
                                            if (paramVal.Count > 0)
                                            {
                                                intArcLen = 0;
                                                arc_prodLen = -(2 * Convert.ToInt32(parameterValue)) + Convert.ToInt32((Math.PI / 2) * (Convert.ToDouble(parameterValue) - (Convert.ToDouble(this.dia) / 2)));
                                                arc_invLen = intArcLen;
                                            }
                                        }
                                        r_intProdLen += arc_prodLen;
                                        r_intInvLen += arc_invLen;
                                    }
                                    #endregion
                                    #region Coupler
                                    else if ((dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("ESPLICE") || dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("NSPLICE") || dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("DEXTRA")) && dtShapeDetails.Rows[rowinc]["CSD_PRODLENGTH"].ToString().Trim().ToUpper().Equals("Y"))
                                    {
                                        if (parameterValue.ToString() != "")
                                        {
                                            DataSet dsCouplerVal = new DataSet();

                                            using (var sqlConnection = new SqlConnection(connectionString))
                                            {
                                                sqlConnection.Open();
                                                var dynamicParameters = new DynamicParameters();
                                                dynamicParameters.Add("@COUPLERWITHDIA", parameterValue.ToString().Trim() + this.dia.ToString());

                                                var dataAdapter = new SqlDataAdapter();
                                                dataAdapter.SelectCommand = new SqlCommand(SystemConstant.USP_GET_COUPLER_DATA_CUBE, sqlConnection);
                                                dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                                                dataAdapter.SelectCommand.Parameters.AddRange(dynamicParameters.ParameterNames.Select(name => new SqlParameter(name, dynamicParameters.Get<object>(name))).ToArray());
                                                dataAdapter.Fill(dsCouplerVal);
                                                //dsCouplerVal = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.USP_GET_COUPLER_DATA_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);

                                            }
                                            
                                            
                                            if (dsCouplerVal.Tables[0].Rows.Count > 0)
                                            {
                                                r_intProdLen -= Convert.ToInt32(dsCouplerVal.Tables[0].Rows[0]["B"].ToString());
                                                r_intProdLen += Convert.ToInt32(dsCouplerVal.Tables[0].Rows[0]["A"].ToString()) + Convert.ToInt32(dsCouplerVal.Tables[0].Rows[0]["C"].ToString());
                                                foreach (DataRow dr in dsCouplerVal.Tables[0].Rows)
                                                {
                                                    Accessory objAcc = new Accessory
                                                    {
                                                        CouplerType = dr["COUPLER_TYPE"].ToString(),
                                                        SAPMaterialCode = dr["MATERIAL"].ToString(),
                                                        MaterialType = dr["MATERIAL_TYPE"].ToString(),
                                                        BitIsCoupler = 1,
                                                        standard = dr["COUPLER_STANDARD"].ToString()
                                                    };
                                                    accList.Add(objAcc);
                                                }
                                            }
                                            dsCouplerVal.Dispose();
                                        }
                                    }
                                    #endregion
                                    #region 3D Length
                                    if (Array.IndexOf(arr3Dlength, dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper()) >= 0 && dtShapeDetails.Rows[rowinc]["CSD_PRODLENGTH"].ToString().Trim().ToUpper().Equals("Y"))
                                    {
                                        sngActLen = float.Parse(parameterValue.ToString());
                                        intActlen = Convert.ToInt32(parameterValue.ToString());

                                        r_intInvLen += intActlen;
                                        r_intProdLen += intActlen;
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                            else
                            {
                                #region Plan Elevation without indicator
                                bool planVisibility = false;
                                bool eleVisibility = false;
                                int planSkipCount = 0;
                                int elevHeightCount = 0;
                                int planHgtCount = 0;
                                int elevSkipCount = 0;

                                //Get the minimum plan sequence.
                                var minPlan = (from item in dtShapeDetails.AsEnumerable()
                                               where item.Field<string>("CSD_VIEW").ToUpper() == "PLAN"
                                               select item.Field<decimal>("CSD_SEQ_NO")).Min();
                                //Get the minimum elevation sequence.
                                var minElev = (from item in dtShapeDetails.AsEnumerable()
                                               where item.Field<string>("CSD_VIEW").ToUpper() == "ELEV"
                                               select item.Field<decimal>("CSD_SEQ_NO")).Min();
                                for (int rowinc = 0; rowinc <= dtShapeDetails.Rows.Count - 1; rowinc++)
                                {
                                    string parameterValue = string.Empty;
                                    for (Int16 row = 0; row <= ShapeParametersList.Count - 1; row++)
                                    {
                                        if (dtShapeDetails.Rows[rowinc]["CSD_SEQ_NO"].ToString() == ShapeParametersList[row].SequenceNumber.ToString())
                                        {
                                            if (ShapeParametersList[row].symmetricIndex.ToString() == "0")
                                            {
                                                parameterValue = ShapeParametersList[row].ParameterValueCab;
                                                break;
                                            }
                                            else
                                            {
                                                //Get the parameter val for the symmetric element.
                                                ShapeParameter obj = new ShapeParameter();
                                                obj = ShapeParametersList.FirstOrDefault(x => x.ParameterName.ToString().Trim() == ShapeParametersList[row].symmetricIndex.ToString().Trim());
                                                parameterValue = obj.ParameterValueCab;
                                                obj = null;
                                                break;
                                            }
                                        }
                                    }
                                    //Logic to calculate the production and invoice length here.
                                    if (minPlan < minElev)
                                    {
                                        if (dtShapeDetails.Rows[rowinc]["CSD_VIEW"].ToString().ToUpper().Equals("PLAN"))
                                        {
                                            if (rowinc != 0)
                                            {
                                                var planVisible = (from item in ShapeParametersList
                                                                   where item.SequenceNumber == Convert.ToInt32(dtShapeDetails.Rows[rowinc]["CSD_SEQ_NO"].ToString())
                                                                   select item).ToList();
                                                if (planVisible.Count > 0)
                                                {
                                                    planVisibility = Convert.ToBoolean(planVisible[0].VisibleFlag);
                                                    if (planVisible[0].AngleType.ToString().ToUpper().Equals("HEIGHT") || planVisible[0].AngleType.ToString().ToUpper().Equals("OFFSET"))
                                                    {
                                                        planSkipCount++;
                                                    }
                                                    else
                                                    {
                                                        //Get the elevation sequence for the looping plan sequence.
                                                        int elevSeq = Convert.ToInt32(minElev) + Convert.ToInt32(dtShapeDetails.Rows[rowinc]["CSD_SEQ_NO"].ToString()) - Convert.ToInt32(minPlan) - planSkipCount + elevSkipCount;

                                                        var eleVisible = (from item in ShapeParametersList
                                                                          where item.SequenceNumber == elevSeq
                                                                          select item).ToList();
                                                        if (eleVisible.Count > 0)
                                                        {
                                                            eleVisibility = Convert.ToBoolean(eleVisible[0].VisibleFlag);
                                                            if (eleVisible[0].AngleType.ToString().ToUpper().Equals("HEIGHT") || eleVisible[0].AngleType.ToString().ToUpper().Equals("OFFSET"))
                                                            {
                                                                elevSeq++;
                                                                eleVisible = (from item in ShapeParametersList
                                                                              where item.SequenceNumber == elevSeq
                                                                              select item).ToList();
                                                                if (eleVisible.Count > 0)
                                                                {
                                                                    eleVisibility = Convert.ToBoolean(eleVisible[0].VisibleFlag);
                                                                }
                                                                elevSkipCount++;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                planSkipCount = 1;
                                            }
                                            #region Length
                                            if ((dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("LENGTH") && planVisibility == true) || (dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("LENGTH") && planVisibility == false && eleVisibility == false))
                                            {
                                                if (Array.IndexOf(arrLength, dtShapeDetails.Rows[rowinc]["CSD_INPUT_TYPE"].ToString().ToUpper()) >= 0)
                                                {
                                                    intActlen = Convert.ToInt32(parameterValue.ToString());
                                                }
                                                r_intInvLen += intActlen;
                                                r_intProdLen += intActlen;
                                            }
                                            #endregion
                                            #region Angle
                                            else if (Array.IndexOf(arrAngle, dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().Trim().ToUpper()) >= 0)
                                            {
                                                if ((planVisibility == true) || (planVisibility == false && eleVisibility == false))
                                                {
                                                    if (rowinc != 0)
                                                    {
                                                        //r_intTotBend += 1;
                                                        if (Convert.ToInt32(parameterValue) < 90)
                                                        {
                                                            r_intProdLen -= Convert.ToInt32((2 * ((Convert.ToDouble(this.pin) / 2) + Convert.ToDouble(this.dia)) * Convert.ToDouble(Math.Tan(Convert.ToDouble(parameterValue) * Math.PI / 360))) - Convert.ToDouble((Math.PI * Convert.ToDouble(parameterValue) * ((Convert.ToDouble(this.pin) / 2) + (0.5 * Convert.ToDouble(this.dia)))) / 180));
                                                        }
                                                        else if (Convert.ToInt32(parameterValue) == 90)
                                                        {
                                                            r_intProdLen -= Convert.ToInt32(1.215 * Convert.ToDouble(this.dia)) + Convert.ToInt32(0.215 * Convert.ToDouble(this.pin));
                                                        }
                                                        else if (Convert.ToInt32(parameterValue) > 90)
                                                        {
                                                            r_intProdLen -= Convert.ToInt32(2 * ((Convert.ToDouble(this.pin) / 2) + Convert.ToDouble(this.dia)) - Convert.ToDouble((Math.PI * Convert.ToDouble(parameterValue) * ((Convert.ToDouble(this.pin) / 2) + (0.5 * Convert.ToDouble(this.dia)))) / 180));
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion
                                            #region Arc
                                            else if ((dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("ARC") && planVisibility == true) || (dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("ARC") && planVisibility == false && eleVisibility == false))
                                            {
                                                r_intTotArc += 1;
                                                string char2 = string.Empty;
                                                int arc_prodLen = 0;
                                                int arc_invLen = 0;

                                                char2 = dtShapeDetails.Rows[rowinc]["CSD_INPUT_TYPE"].ToString();

                                                if (char2.ToUpper().Equals("RAD+ARCLENGTH"))
                                                {
                                                    intArcLen = Convert.ToInt32(parameterValue);
                                                    arc_prodLen = Convert.ToInt32((0.57 * intArcLen) - (1.57 * Convert.ToDouble(this.dia)));
                                                    arc_invLen = intArcLen;
                                                }
                                                else if (char2.ToUpper().Equals("RAD+SW_ANGLE"))
                                                {
                                                }
                                                else if (char2.ToUpper().Equals("DIA+SW_ANGLE"))
                                                {
                                                    intArcLen = Convert.ToInt32((Convert.ToDouble(parameterValue) * Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR2"]) * Math.PI) / 360.0);
                                                    if (Convert.ToInt32(dtShapeDetails.Rows[rowinc]["CSD_PAR2"].ToString()) > 90)
                                                    {
                                                        arc_prodLen = intArcLen - Convert.ToInt32((2 * ((Convert.ToDouble(this.pin) / 2) + Convert.ToDouble(this.dia)) * Convert.ToDouble(Math.Tan(Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR2"].ToString()) * Math.PI / 360))) - Convert.ToDouble((Math.PI * Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR2"].ToString()) * ((Convert.ToDouble(this.pin) / 2) + (0.5 * Convert.ToDouble(this.dia)))) / 180));
                                                    }
                                                    else
                                                    {
                                                        arc_prodLen = intArcLen - Convert.ToInt32(2 * ((Convert.ToDouble(this.pin) / 2) + Convert.ToDouble(this.dia)) - Convert.ToDouble((Math.PI * Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR2"].ToString()) * ((Convert.ToDouble(this.pin) / 2) + (0.5 * Convert.ToDouble(this.dia)))) / 180));
                                                    }
                                                    arc_invLen = intArcLen;
                                                }
                                                else if (char2.ToUpper().Equals("CHORD+NORMAL"))
                                                {
                                                    intArcRad = Convert.ToInt32(((Convert.ToDouble(parameterValue) * Convert.ToDouble(parameterValue)) + (4.0 * Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR3"]) * Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR3"]))) / (8.0 * Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR3"])));
                                                    if (Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR3"]) <= intArcRad)
                                                    {
                                                        intArcSwAng = Convert.ToInt32(2.0 * Math.Asin(Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR3"]) / (2.0 * intArcRad)));
                                                    }
                                                    else
                                                    {
                                                        intArcSwAng = Convert.ToInt32(2.0 * (Math.PI - Math.Asin(Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR3"]) / (2.0 * intArcRad))));
                                                    }
                                                    intArcLen = intArcRad * intArcSwAng;
                                                    arc_invLen = intArcLen;
                                                    arc_prodLen = intArcLen;
                                                }
                                                else if (char2.ToUpper().Equals("OTHER"))
                                                {
                                                    intArcLen = Convert.ToInt32(parameterValue);
                                                    arc_invLen = intArcLen;
                                                    arc_prodLen = intArcLen;
                                                }
                                                r_intProdLen += arc_prodLen;
                                                r_intInvLen += arc_invLen;
                                            }
                                            #endregion
                                            #region ARC Radius
                                            else if ((dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("ARC LENGTH") && planVisibility == true) || (dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("ARC LENGTH") && eleVisibility == false && planVisibility == false))
                                            {
                                                string char2 = string.Empty;
                                                int arc_prodLen = 0;
                                                int arc_invLen = 0;

                                                char2 = dtShapeDetails.Rows[rowinc - 1]["CSD_INPUT_TYPE"].ToString();

                                                if (char2.ToUpper().Equals("RAD+SW_ANGLE"))
                                                {
                                                    int seqNo = Convert.ToInt32(dtShapeDetails.Rows[rowinc]["CSD_SEQ_NO"].ToString());
                                                    var paramVal = (from item in ShapeParametersList
                                                                    where item.SequenceNumber == seqNo - 1
                                                                    select item.ParameterValueCab).ToList();
                                                    if (paramVal.Count > 0)
                                                    {
                                                        intArcLen = Convert.ToInt32(paramVal[0].ToString());
                                                        arc_prodLen = Convert.ToInt32((intArcLen * (Convert.ToDouble(parameterValue) + (Convert.ToDouble(this.dia) / 2))) / (Convert.ToDouble(parameterValue) + Convert.ToDouble(this.dia)));
                                                        arc_invLen = intArcLen;
                                                    }
                                                }
                                                if (char2.ToUpper().Equals("RAD+RIGHTANGLE"))
                                                {
                                                    int seqNo = Convert.ToInt32(dtShapeDetails.Rows[rowinc]["CSD_SEQ_NO"].ToString());
                                                    var paramVal = (from item in ShapeParametersList
                                                                    where item.SequenceNumber == seqNo - 1
                                                                    select item.ParameterValueCab).ToList();
                                                    if (paramVal.Count > 0)
                                                    {
                                                        intArcLen = 0;
                                                        arc_prodLen = -(2 * Convert.ToInt32(parameterValue)) + Convert.ToInt32((Math.PI / 2) * (Convert.ToDouble(parameterValue) - (Convert.ToDouble(this.dia) / 2)));
                                                        arc_invLen = intArcLen;
                                                    }
                                                }
                                                r_intProdLen += arc_prodLen;
                                                r_intInvLen += arc_invLen;
                                            }
                                            #endregion
                                            #region Coupler
                                            else if (dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("ESPLICE") || dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("NSPLICE") || dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("DEXTRA"))
                                            {
                                                if (planVisibility == true || (planVisibility == false && eleVisibility == false))
                                                {
                                                    //string couplerWithDia = string.Empty;
                                                    //couplerWithDia = GetCoupler(parameterValue.ToString());
                                                    if (parameterValue.ToString() != "")
                                                    {
                                                        DataSet dsCouplerVal = new DataSet();
                                                        DataTable dtCouplerVal = new DataTable();

                                                        using (var sqlConnection = new SqlConnection(connectionString))
                                                        {
                                                            sqlConnection.Open();
                                                            var dynamicParameters = new DynamicParameters();
                                                            dynamicParameters.Add("@COUPLERWITHDIA", parameterValue.ToString().Trim() + this.dia.ToString());

                                                            var dataAdapter = new SqlDataAdapter();
                                                            dataAdapter.SelectCommand = new SqlCommand(SystemConstant.USP_GET_COUPLER_DATA_CUBE, sqlConnection);
                                                            dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                                                            dataAdapter.SelectCommand.Parameters.AddRange(dynamicParameters.ParameterNames.Select(name => new SqlParameter(name, dynamicParameters.Get<object>(name))).ToArray());
                                                            dataAdapter.Fill(dsCouplerVal);

                                                            //dsCouplerVal = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.USP_GET_COUPLER_DATA_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);
                                                            dtCouplerVal = dsCouplerVal.Tables[0];

                                                        }

                                                        if (dtCouplerVal.Rows.Count > 0)
                                                        {
                                                            r_intProdLen -= Convert.ToInt32(dtCouplerVal.Rows[0]["B"].ToString());
                                                            r_intProdLen += Convert.ToInt32(dtCouplerVal.Rows[0]["A"].ToString()) + Convert.ToInt32(dtCouplerVal.Rows[0]["C"].ToString());

                                                            Accessory objAcc = new Accessory
                                                            {
                                                                CouplerType = dtCouplerVal.Rows[0]["COUPLER_TYPE"].ToString(),
                                                                SAPMaterialCode = dtCouplerVal.Rows[0]["COUPLER_MATERIAL"].ToString(),
                                                                BitIsCoupler = 1,
                                                                standard = dtCouplerVal.Rows[0]["COUPLER_STANDARD"].ToString()
                                                            };
                                                            accList.Add(objAcc);
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion
                                            #region 3D Length
                                            else if (Array.IndexOf(arr3Dlength, dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().Trim().ToUpper()) >= 0)
                                            {
                                                if ((planVisibility == true) || (planVisibility == false && eleVisibility == false))
                                                {
                                                    sngActLen = float.Parse(parameterValue.ToString());
                                                    intActlen = Convert.ToInt32(parameterValue.ToString());

                                                    r_intInvLen += intActlen;
                                                    r_intProdLen += intActlen;
                                                }

                                            }
                                            #endregion
                                        }
                                        if (dtShapeDetails.Rows[rowinc]["CSD_VIEW"].ToString().ToUpper().Equals("ELEV"))
                                        {
                                            var eleVisible = (from item in ShapeParametersList
                                                              where item.SequenceNumber == Convert.ToInt32(dtShapeDetails.Rows[rowinc]["CSD_SEQ_NO"].ToString())
                                                              select item).ToList();
                                            if (eleVisible.Count > 0)
                                            {
                                                eleVisibility = Convert.ToBoolean(eleVisible[0].VisibleFlag);
                                                if (eleVisible[0].AngleType.ToString().ToUpper().Equals("HEIGHT") || eleVisible[0].AngleType.ToString().ToUpper().Equals("OFFSET"))
                                                {
                                                    elevHeightCount++;
                                                }
                                                else
                                                {
                                                    //Get the corresponding plan sequence
                                                    int planSeq = Convert.ToInt32(minPlan) + Convert.ToInt32(dtShapeDetails.Rows[rowinc]["CSD_SEQ_NO"].ToString()) - Convert.ToInt32(minElev) - elevHeightCount + planHgtCount;

                                                    var planVisible = (from item in ShapeParametersList
                                                                       where item.SequenceNumber == planSeq
                                                                       select item).ToList();
                                                    if (planVisible.Count > 0)
                                                    {
                                                        planVisibility = Convert.ToBoolean(planVisible[0].VisibleFlag);
                                                        if (planVisible[0].AngleType.ToString().ToUpper().Equals("HEIGHT") || planVisible[0].AngleType.ToString().ToUpper().Equals("OFFSET") || (planSeq == 1 && Array.IndexOf(arrAngle, planVisible[0].AngleType.ToString().ToUpper()) >= 0))
                                                        {
                                                            planSeq++;
                                                            planHgtCount++;
                                                            planVisible = (from item in ShapeParametersList
                                                                           where item.SequenceNumber == planSeq
                                                                           select item).ToList();
                                                            if (planVisible.Count > 0)
                                                            {
                                                                planVisibility = Convert.ToBoolean(planVisible[0].VisibleFlag);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            #region Length
                                            if ((dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("LENGTH") && eleVisibility == true) || (dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("LENGTH") && planVisibility == false && eleVisibility == false))
                                            {
                                                if (Array.IndexOf(arrLength, dtShapeDetails.Rows[rowinc]["CSD_INPUT_TYPE"].ToString().ToUpper()) >= 0)
                                                {
                                                    intActlen = Convert.ToInt32(parameterValue.ToString());
                                                }
                                                r_intInvLen += intActlen;
                                                r_intProdLen += intActlen;
                                            }
                                            #endregion
                                            #region Angle
                                            else if (Array.IndexOf(arrAngle, dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().Trim().ToUpper()) >= 0)
                                            {
                                                if ((eleVisibility == true) || (planVisibility == false && eleVisibility == false))
                                                {
                                                    if (rowinc != 0)
                                                    {
                                                        //r_intTotBend += 1;
                                                        if (Convert.ToInt32(parameterValue) < 90)
                                                        {
                                                            r_intProdLen -= Convert.ToInt32((2 * ((Convert.ToDouble(this.pin) / 2) + Convert.ToDouble(this.dia)) * Convert.ToDouble(Math.Tan(Convert.ToDouble(parameterValue) * Math.PI / 360))) - Convert.ToDouble((Math.PI * Convert.ToDouble(parameterValue) * ((Convert.ToDouble(this.pin) / 2) + (0.5 * Convert.ToDouble(this.dia)))) / 180));
                                                        }
                                                        else if (Convert.ToInt32(parameterValue) == 90)
                                                        {
                                                            r_intProdLen -= Convert.ToInt32(1.215 * Convert.ToDouble(this.dia)) + Convert.ToInt32(0.215 * Convert.ToDouble(this.pin));
                                                        }
                                                        else if (Convert.ToInt32(parameterValue) > 90)
                                                        {
                                                            r_intProdLen -= Convert.ToInt32(2 * ((Convert.ToDouble(this.pin) / 2) + Convert.ToDouble(this.dia)) - Convert.ToDouble((Math.PI * Convert.ToDouble(parameterValue) * ((Convert.ToDouble(this.pin) / 2) + (0.5 * Convert.ToDouble(this.dia)))) / 180));
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion
                                            #region Arc
                                            else if ((dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("ARC") && eleVisibility == true) || (dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("ARC") && planVisibility == false && eleVisibility == false))
                                            {
                                                r_intTotArc += 1;
                                                string char2 = string.Empty;
                                                int arc_prodLen = 0;
                                                int arc_invLen = 0;

                                                char2 = dtShapeDetails.Rows[rowinc]["CSD_INPUT_TYPE"].ToString();

                                                if (char2.ToUpper().Equals("RAD+ARCLENGTH"))
                                                {
                                                    intArcLen = Convert.ToInt32(parameterValue);
                                                    arc_prodLen = Convert.ToInt32((0.57 * intArcLen) - (1.57 * Convert.ToDouble(this.dia)));
                                                    arc_invLen = intArcLen;
                                                }
                                                else if (char2.ToUpper().Equals("RAD+SW_ANGLE"))
                                                {
                                                }
                                                else if (char2.ToUpper().Equals("DIA+SW_ANGLE"))
                                                {
                                                    intArcLen = Convert.ToInt32((Convert.ToDouble(parameterValue) * Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR2"]) * Math.PI) / 360.0);
                                                    if (Convert.ToInt32(dtShapeDetails.Rows[rowinc]["CSD_PAR2"].ToString()) > 90)
                                                    {
                                                        arc_prodLen = intArcLen - Convert.ToInt32((2 * ((Convert.ToDouble(this.pin) / 2) + Convert.ToDouble(this.dia)) * Convert.ToDouble(Math.Tan(Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR2"].ToString()) * Math.PI / 360))) - Convert.ToDouble((Math.PI * Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR2"].ToString()) * ((Convert.ToDouble(this.pin) / 2) + (0.5 * Convert.ToDouble(this.dia)))) / 180));
                                                    }
                                                    else
                                                    {
                                                        arc_prodLen = intArcLen - Convert.ToInt32(2 * ((Convert.ToDouble(this.pin) / 2) + Convert.ToDouble(this.dia)) - Convert.ToDouble((Math.PI * Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR2"].ToString()) * ((Convert.ToDouble(this.pin) / 2) + (0.5 * Convert.ToDouble(this.dia)))) / 180));
                                                    }
                                                    arc_invLen = intArcLen;
                                                }
                                                else if (char2.ToUpper().Equals("CHORD+NORMAL"))
                                                {
                                                    intArcRad = Convert.ToInt32(((Convert.ToDouble(parameterValue) * Convert.ToDouble(parameterValue)) + (4.0 * Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR3"]) * Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR3"]))) / (8.0 * Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR3"])));
                                                    if (Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR3"]) <= intArcRad)
                                                    {
                                                        intArcSwAng = Convert.ToInt32(2.0 * Math.Asin(Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR3"]) / (2.0 * intArcRad)));
                                                    }
                                                    else
                                                    {
                                                        intArcSwAng = Convert.ToInt32(2.0 * (Math.PI - Math.Asin(Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR3"]) / (2.0 * intArcRad))));
                                                    }
                                                    intArcLen = intArcRad * intArcSwAng;
                                                    arc_invLen = intArcLen;
                                                    arc_prodLen = intArcLen;
                                                }
                                                else if (char2.ToUpper().Equals("OTHER"))
                                                {
                                                    intArcLen = Convert.ToInt32(parameterValue);
                                                    arc_invLen = intArcLen;
                                                    arc_prodLen = intArcLen;
                                                }
                                                r_intProdLen += arc_prodLen;
                                                r_intInvLen += arc_invLen;
                                            }
                                            #endregion
                                            #region ARC Radius
                                            else if ((dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("ARC LENGTH") && eleVisibility == true) || (dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("ARC LENGTH") && eleVisibility == false && planVisibility == false))
                                            {
                                                string char2 = string.Empty;
                                                int arc_prodLen = 0;
                                                int arc_invLen = 0;

                                                char2 = dtShapeDetails.Rows[rowinc - 1]["CSD_INPUT_TYPE"].ToString();

                                                if (char2.ToUpper().Equals("RAD+SW_ANGLE"))
                                                {
                                                    int seqNo = Convert.ToInt32(dtShapeDetails.Rows[rowinc]["CSD_SEQ_NO"].ToString());
                                                    var paramVal = (from item in ShapeParametersList
                                                                    where item.SequenceNumber == seqNo - 1
                                                                    select item.ParameterValueCab).ToList();
                                                    if (paramVal.Count > 0)
                                                    {
                                                        intArcLen = Convert.ToInt32(paramVal[0].ToString());
                                                        arc_prodLen = Convert.ToInt32((intArcLen * (Convert.ToDouble(parameterValue) + (Convert.ToDouble(this.dia) / 2))) / (Convert.ToDouble(parameterValue) + Convert.ToDouble(this.dia)));
                                                        arc_invLen = intArcLen;
                                                    }
                                                }
                                                if (char2.ToUpper().Equals("RAD+RIGHTANGLE"))
                                                {
                                                    int seqNo = Convert.ToInt32(dtShapeDetails.Rows[rowinc]["CSD_SEQ_NO"].ToString());
                                                    var paramVal = (from item in ShapeParametersList
                                                                    where item.SequenceNumber == seqNo - 1
                                                                    select item.ParameterValueCab).ToList();
                                                    if (paramVal.Count > 0)
                                                    {
                                                        intArcLen = 0;
                                                        arc_prodLen = -(2 * Convert.ToInt32(parameterValue)) + Convert.ToInt32((Math.PI / 2) * (Convert.ToDouble(parameterValue) - (Convert.ToDouble(this.dia) / 2)));
                                                        arc_invLen = intArcLen;
                                                    }
                                                }
                                                r_intProdLen += arc_prodLen;
                                                r_intInvLen += arc_invLen;
                                            }
                                            #endregion
                                            #region Coupler
                                            else if (dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("ESPLICE") || dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("NSPLICE") || dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("DEXTRA"))
                                            {
                                                if (planVisibility == true || (planVisibility == false && eleVisibility == false))
                                                {
                                                    if (parameterValue.ToString() != "")
                                                    {
                                                        DataSet dsCouplerVal = new DataSet();
                                                        DataTable dtCouplerVal = new DataTable();

                                                        using (var sqlConnection = new SqlConnection(connectionString))
                                                        {
                                                            sqlConnection.Open();
                                                            var dynamicParameters = new DynamicParameters();
                                                            dynamicParameters.Add("@COUPLERWITHDIA", parameterValue.ToString().Trim() + this.dia.ToString());

                                                            var dataAdapter = new SqlDataAdapter();
                                                            dataAdapter.SelectCommand = new SqlCommand(SystemConstant.USP_GET_COUPLER_DATA_CUBE, sqlConnection);
                                                            dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                                                            dataAdapter.SelectCommand.Parameters.AddRange(dynamicParameters.ParameterNames.Select(name => new SqlParameter(name, dynamicParameters.Get<object>(name))).ToArray());
                                                            dataAdapter.Fill(dsCouplerVal);
                                                            //dsCouplerVal = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.USP_GET_COUPLER_DATA_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);
                                                            dtCouplerVal = dsCouplerVal.Tables[0];

                                                        }
                                                        if (dtCouplerVal.Rows.Count > 0)
                                                        {
                                                            r_intProdLen -= Convert.ToInt32(dtCouplerVal.Rows[0]["B"].ToString());
                                                            r_intProdLen += Convert.ToInt32(dtCouplerVal.Rows[0]["A"].ToString()) + Convert.ToInt32(dtCouplerVal.Rows[0]["C"].ToString());

                                                            Accessory objAcc = new Accessory
                                                            {
                                                                CouplerType = dtCouplerVal.Rows[0]["COUPLER_TYPE"].ToString(),
                                                                SAPMaterialCode = dtCouplerVal.Rows[0]["COUPLER_MATERIAL"].ToString(),
                                                                BitIsCoupler = 1,
                                                                standard = dtCouplerVal.Rows[0]["COUPLER_STANDARD"].ToString()
                                                            };
                                                            accList.Add(objAcc);
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion
                                            #region 3D Length
                                            if (Array.IndexOf(arr3Dlength, dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().Trim().ToUpper()) >= 0)
                                            {
                                                if ((eleVisibility == true) || (planVisibility == false && eleVisibility == false))
                                                {
                                                    sngActLen = float.Parse(parameterValue.ToString());
                                                    intActlen = Convert.ToInt32(parameterValue.ToString());

                                                    r_intInvLen += intActlen;
                                                    r_intProdLen += intActlen;
                                                }

                                            }
                                            #endregion
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                        else if ((dtShapeDetails.Rows[0]["CSC_CAT_DESC"].ToString().ToUpper().Equals("SPECIAL") && dtShapeDetails.Rows[0]["CSD_VIEW"].ToString().ToUpper().Equals("PLAN")) || (dtShapeDetails.Rows[0]["CSC_CAT_DESC"].ToString().ToUpper().Equals("SPECIAL") && dtShapeDetails.Rows[0]["CSD_VIEW"].ToString().ToUpper().Equals("ELEV")))
                        {
                            if (dtShapeDetails.Rows[0]["CSD_PRODLENGTH"].ToString().Trim().ToUpper().Equals("Y") || dtShapeDetails.Rows[0]["CSD_PRODLENGTH"].ToString().Trim().ToUpper().Equals("N"))
                            {
                                #region Plan elevation with indicator
                                for (int rowinc = 0; rowinc <= dtShapeDetails.Rows.Count - 1; rowinc++)
                                {
                                    string parameterValue = string.Empty;
                                    for (Int16 row = 0; row <= ShapeParametersList.Count - 1; row++)
                                    {
                                        if (dtShapeDetails.Rows[rowinc]["CSD_SEQ_NO"].ToString() == ShapeParametersList[row].SequenceNumber.ToString())
                                        {
                                            if (ShapeParametersList[row].symmetricIndex.ToString().Equals("0"))
                                            {
                                                parameterValue = ShapeParametersList[row].ParameterValueCab;
                                                break;
                                            }
                                            else
                                            {
                                                //Get the parameter val for the symmetric element.
                                                ShapeParameter obj = new ShapeParameter();
                                                obj = ShapeParametersList.FirstOrDefault(x => x.ParameterName.ToString().Trim() == ShapeParametersList[row].symmetricIndex.ToString().Trim());
                                                parameterValue = obj.ParameterValueCab;
                                                obj = null;
                                                break;
                                            }
                                        }
                                    }
                                    #region Length
                                    if (dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("LENGTH") && dtShapeDetails.Rows[rowinc]["CSD_PRODLENGTH"].ToString().Trim().ToUpper().Equals("Y"))
                                    {
                                        if (Array.IndexOf(arrLength, dtShapeDetails.Rows[rowinc]["CSD_INPUT_TYPE"].ToString().ToUpper()) >= 0)
                                        {
                                            intActlen = Convert.ToInt32(parameterValue.ToString());
                                        }

                                        r_intInvLen += intActlen;
                                        r_intProdLen += intActlen;
                                    }
                                    #endregion
                                    #region Angle
                                    else if ((dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("ANGLE") && dtShapeDetails.Rows[rowinc]["CSD_PRODLENGTH"].ToString().Trim().ToUpper().Equals("Y")) || (dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("ANGLE_3D") && dtShapeDetails.Rows[rowinc]["CSD_PRODLENGTH"].ToString().Trim().ToUpper().Equals("Y")))
                                    {
                                        if (rowinc != 0)
                                        {
                                            if (Convert.ToInt32(parameterValue) < 90)
                                            {
                                                r_intProdLen -= Convert.ToInt32((2 * ((Convert.ToDouble(this.pin) / 2) + Convert.ToDouble(this.dia)) * Convert.ToDouble(Math.Tan(Convert.ToDouble(parameterValue) * Math.PI / 360))) - Convert.ToDouble((Math.PI * Convert.ToDouble(parameterValue) * ((Convert.ToDouble(this.pin) / 2) + (0.5 * Convert.ToDouble(this.dia)))) / 180));
                                            }
                                            else if (Convert.ToInt32(parameterValue) == 90)
                                            {
                                                r_intProdLen -= Convert.ToInt32(1.215 * Convert.ToDouble(this.dia)) + Convert.ToInt32(0.215 * Convert.ToDouble(this.pin));
                                            }
                                            else if (Convert.ToInt32(parameterValue) > 90)
                                            {
                                                r_intProdLen -= Convert.ToInt32(2 * ((Convert.ToDouble(this.pin) / 2) + Convert.ToDouble(this.dia)) - Convert.ToDouble((Math.PI * Convert.ToDouble(parameterValue) * ((Convert.ToDouble(this.pin) / 2) + (0.5 * Convert.ToDouble(this.dia)))) / 180));
                                            }
                                        }
                                    }
                                    #endregion
                                    #region Arc
                                    else if (dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("ARC") && dtShapeDetails.Rows[rowinc]["CSD_PRODLENGTH"].ToString().Trim().ToUpper().Equals("Y"))
                                    {
                                        r_intTotArc += 1;
                                        string char2 = string.Empty;
                                        int arc_prodLen = 0;
                                        int arc_invLen = 0;

                                        char2 = dtShapeDetails.Rows[rowinc]["CSD_INPUT_TYPE"].ToString().ToUpper();

                                        if (char2.Equals("RAD+ARCLENGTH"))
                                        {
                                            intArcLen = Convert.ToInt32(parameterValue);
                                            arc_prodLen = Convert.ToInt32((0.57 * intArcLen) - (1.57 * Convert.ToDouble(this.dia)));
                                            arc_invLen = intArcLen;
                                        }
                                        else if (char2.Equals("RAD+SW_ANGLE"))
                                        {
                                        }
                                        else if (char2.Equals("DIA+SW_ANGLE"))
                                        {
                                            intArcLen = Convert.ToInt32((Convert.ToDouble(parameterValue) * Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR2"]) * Math.PI) / 360.0);
                                            if (Convert.ToInt32(dtShapeDetails.Rows[rowinc]["CSD_PAR2"].ToString()) > 90)
                                            {
                                                arc_prodLen = intArcLen - Convert.ToInt32((2 * ((Convert.ToDouble(this.pin) / 2) + Convert.ToDouble(this.dia)) * Convert.ToDouble(Math.Tan(Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR2"].ToString()) * Math.PI / 360))) - Convert.ToDouble((Math.PI * Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR2"].ToString()) * ((Convert.ToDouble(this.pin) / 2) + (0.5 * Convert.ToDouble(this.dia)))) / 180));
                                            }
                                            else
                                            {
                                                arc_prodLen = intArcLen - Convert.ToInt32(2 * ((Convert.ToDouble(this.pin) / 2) + Convert.ToDouble(this.dia)) - Convert.ToDouble((Math.PI * Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR2"].ToString()) * ((Convert.ToDouble(this.pin) / 2) + (0.5 * Convert.ToDouble(this.dia)))) / 180));
                                            }
                                            arc_invLen = intArcLen;
                                        }
                                        else if (char2.Equals("CHORD+NORMAL"))
                                        {
                                            intArcRad = Convert.ToInt32(((Convert.ToDouble(parameterValue) * Convert.ToDouble(parameterValue)) + (4.0 * Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR3"]) * Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR3"]))) / (8.0 * Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR3"])));
                                            if (Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR3"]) <= intArcRad)
                                            {
                                                intArcSwAng = Convert.ToInt32(2.0 * Math.Asin(Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR3"]) / (2.0 * intArcRad)));
                                            }
                                            else
                                            {
                                                intArcSwAng = Convert.ToInt32(2.0 * (Math.PI - Math.Asin(Convert.ToDouble(dtShapeDetails.Rows[rowinc]["CSD_PAR3"]) / (2.0 * intArcRad))));
                                            }
                                            intArcLen = intArcRad * intArcSwAng;
                                            arc_invLen = intArcLen;
                                            arc_prodLen = intArcLen;
                                        }
                                        else if (char2.Equals("OTHER"))
                                        {
                                            intArcLen = Convert.ToInt32(parameterValue);
                                            arc_invLen = intArcLen;
                                            arc_prodLen = intArcLen;
                                        }
                                        r_intProdLen += arc_prodLen;
                                        r_intInvLen += arc_invLen;
                                    }
                                    #endregion
                                    #region ARC Radius
                                    else if (dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("ARC LENGTH"))
                                    {
                                        string char2 = string.Empty;
                                        int arc_prodLen = 0;
                                        int arc_invLen = 0;

                                        char2 = dtShapeDetails.Rows[rowinc - 1]["CSD_INPUT_TYPE"].ToString();

                                        if (char2.ToUpper().Equals("RAD+SW_ANGLE") && dtShapeDetails.Rows[rowinc - 1]["CSD_PRODLENGTH"].ToString().Trim().ToUpper().Equals("Y"))
                                        {
                                            int seqNo = Convert.ToInt32(dtShapeDetails.Rows[rowinc]["CSD_SEQ_NO"].ToString());
                                            var paramVal = (from item in ShapeParametersList
                                                            where item.SequenceNumber == seqNo - 1
                                                            select item.ParameterValueCab).ToList();
                                            if (paramVal.Count > 0)
                                            {
                                                intArcLen = Convert.ToInt32(paramVal[0].ToString());
                                                arc_prodLen = Convert.ToInt32((intArcLen * (Convert.ToDouble(parameterValue) + (Convert.ToDouble(this.dia) / 2))) / (Convert.ToDouble(parameterValue) + Convert.ToDouble(this.dia)));
                                                arc_invLen = intArcLen;
                                            }
                                        }
                                        if (char2.ToUpper().Equals("RAD+RIGHTANGLE") && dtShapeDetails.Rows[rowinc - 1]["CSD_PRODLENGTH"].ToString().Trim().ToUpper().Equals("Y"))
                                        {
                                            int seqNo = Convert.ToInt32(dtShapeDetails.Rows[rowinc]["CSD_SEQ_NO"].ToString());
                                            var paramVal = (from item in ShapeParametersList
                                                            where item.SequenceNumber == seqNo - 1
                                                            select item.ParameterValueCab).ToList();
                                            if (paramVal.Count > 0)
                                            {
                                                intArcLen = 0;
                                                arc_prodLen = -(2 * Convert.ToInt32(parameterValue)) + Convert.ToInt32((Math.PI / 2) * (Convert.ToDouble(parameterValue) - (Convert.ToDouble(this.dia) / 2)));
                                                arc_invLen = intArcLen;
                                            }
                                        }
                                        r_intProdLen += arc_prodLen;
                                        r_intInvLen += arc_invLen;
                                    }
                                    #endregion
                                    #region Coupler
                                    else if ((dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("ESPLICE") || dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("NSPLICE") || dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("DEXTRA")) && dtShapeDetails.Rows[rowinc]["CSD_PRODLENGTH"].ToString().Trim().ToUpper().Equals("Y"))
                                    {
                                        if (parameterValue.ToString() != "")
                                        {
                                            DataSet dsCouplerVal = new DataSet();
                                            using (var sqlConnection = new SqlConnection(connectionString))
                                            {
                                                sqlConnection.Open();
                                                var dynamicParameters = new DynamicParameters();
                                                dynamicParameters.Add("@COUPLERWITHDIA", parameterValue.ToString().Trim() + this.dia.ToString());

                                                var dataAdapter = new SqlDataAdapter();
                                                dataAdapter.SelectCommand = new SqlCommand(SystemConstant.USP_GET_COUPLER_DATA_CUBE, sqlConnection);
                                                dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                                                dataAdapter.SelectCommand.Parameters.AddRange(dynamicParameters.ParameterNames.Select(name => new SqlParameter(name, dynamicParameters.Get<object>(name))).ToArray());
                                                dataAdapter.Fill(dsCouplerVal);
                                                //dsCouplerVal = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.USP_GET_COUPLER_DATA_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);

                                            }
                                            
                                            if (dsCouplerVal.Tables[0].Rows.Count > 0)
                                            {
                                                r_intProdLen -= Convert.ToInt32(dsCouplerVal.Tables[0].Rows[0]["B"].ToString());
                                                r_intProdLen += Convert.ToInt32(dsCouplerVal.Tables[0].Rows[0]["A"].ToString()) + Convert.ToInt32(dsCouplerVal.Tables[0].Rows[0]["C"].ToString());
                                                foreach (DataRow dr in dsCouplerVal.Tables[0].Rows)
                                                {
                                                    Accessory objAcc = new Accessory
                                                    {
                                                        CouplerType = dr["COUPLER_TYPE"].ToString(),
                                                        SAPMaterialCode = dr["MATERIAL"].ToString(),
                                                        MaterialType = dr["MATERIAL_TYPE"].ToString(),
                                                        BitIsCoupler = 1,
                                                        standard = dr["COUPLER_STANDARD"].ToString()
                                                    };
                                                    accList.Add(objAcc);
                                                }
                                            }
                                            dsCouplerVal.Dispose();
                                        }
                                    }
                                    #endregion
                                    #region 3D Length
                                    if (Array.IndexOf(arr3Dlength, dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper()) >= 0 && dtShapeDetails.Rows[rowinc]["CSD_PRODLENGTH"].ToString().Trim().ToUpper().Equals("Y"))
                                    {
                                        sngActLen = float.Parse(parameterValue.ToString());
                                        intActlen = Convert.ToInt32(parameterValue.ToString());

                                        r_intInvLen += intActlen;
                                        r_intProdLen += intActlen;
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                        }
                    }
                    #endregion
                    #region Calculate the invoice length value if formula is provided
                    if (ds.Tables[1] != null)
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            if (ds.Tables[1].Rows[0][1].ToString() != "")
                            {
                                string formula = ds.Tables[1].Rows[0][1].ToString();
                                int count = (from param in ShapeParametersList
                                             where param.ParameterName != "" && param.ParameterName != string.Empty
                                             select param).Count();
                                string[,] arr = new string[count + 2, 2];
                                int j = 0;
                                ShapeParametersList = ShapeParametersList.OrderBy(O => O.ParameterName).ToList();
                                for (int i = 0; i <= ShapeParametersList.Count - 1; i++)
                                {
                                    if (ShapeParametersList[i].ParameterName != "" && ShapeParametersList[i].ParameterName != string.Empty)
                                    {
                                        arr[j, 0] = ShapeParametersList[i].ParameterName.ToString();
                                        arr[j, 1] = ShapeParametersList[i].ParameterValueCab.ToString();
                                        j++;
                                    }
                                }
                                if (this.dia != null)
                                {
                                    arr[count, 0] = "$";
                                    arr[count, 1] = this.dia.ToString();
                                }
                                if (this.pin != null)
                                {
                                    arr[count + 1, 0] = "@";
                                    arr[count + 1, 1] = Convert.ToInt32(Convert.ToInt32(this.pin) / 2).ToString();
                                }
                                //Call math parser class to execute the formula.
                                //MathParserWrapper objMath = new MathParserWrapper();
                                //r_intInvLen = Convert.ToInt32(objMath.EvaluateFormula(formula, Convert.ToBoolean(arr)));
                                MathParser objMath = new MathParser();
                                r_intInvLen = Convert.ToInt32(objMath.Evaluate(formula, arr));
                                objMath = null;
                                Array.Clear(arr, 0, arr.Length);
                            }
                        }
                    }
                    #endregion
                    #region Calculate the production length if formula is provided
                    if (ds.Tables[1] != null)
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            if (ds.Tables[1].Rows[0][0].ToString() != "")
                            {
                                string formula = ds.Tables[1].Rows[0][0].ToString();
                                int count = (from param in ShapeParametersList
                                             where param.ParameterName != "" && param.ParameterName != string.Empty
                                             select param).Count();
                                string[,] arr = new string[count + 2, 2];
                                int j = 0;
                                ShapeParametersList = ShapeParametersList.OrderBy(O => O.ParameterName).ToList();
                                for (int i = 0; i <= ShapeParametersList.Count - 1; i++)
                                {
                                    if (ShapeParametersList[i].ParameterName != "" && ShapeParametersList[i].ParameterName != string.Empty)
                                    {
                                        arr[j, 0] = ShapeParametersList[i].ParameterName.ToString();
                                        arr[j, 1] = ShapeParametersList[i].ParameterValueCab.ToString();
                                        j++;
                                    }
                                }
                                if (this.dia != null)
                                {
                                    arr[count, 0] = "$";
                                    arr[count, 1] = this.dia.ToString();
                                }
                                if (this.pin != null)
                                {
                                    arr[count + 1, 0] = "@";
                                    arr[count + 1, 1] = Convert.ToInt32(Convert.ToInt32(this.pin) / 2).ToString();
                                }
                                //Call math parser class to execute the formula.
                                //MathParserWrapper objMath = new MathParserWrapper();
                                //r_intProdLen = Convert.ToInt32(objMath.EvaluateFormula(formula, Convert.ToBoolean(arr)));
                                MathParser objMath = new MathParser();
                                r_intProdLen = Convert.ToInt32(objMath.Evaluate(formula, arr));
                                objMath = null;
                                Array.Clear(arr, 0, arr.Length);
                            }
                        }
                    }
                    #endregion
                }

                //Get the number of bends from shapemaster.
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[2] != null)
                    {
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            r_intTotBend = Convert.ToInt32(ds.Tables[2].Rows[0][0].ToString());
                        }
                    }
                }
                // START:-= CAB Shape-Custom Production Length Computation | Cut Length = Inv Lengh Modification | Siddhi 
                //if (IsCoupler)
                //{
                DataSet dsCustomInvLength = new DataSet();

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@ShapeID", this.ShapeCode.Trim());
                    dynamicParameters.Add("@InvLength", r_intInvLen.ToString());
                    //dsCustomInvLength = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "sp_Check_IsCustomInvCutLength");

                    var dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = new SqlCommand(SystemConstant.sp_Check_IsCustomInvCutLength, sqlConnection);
                    dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dataAdapter.SelectCommand.Parameters.AddRange(dynamicParameters.ParameterNames.Select(name => new SqlParameter(name, dynamicParameters.Get<object>(name))).ToArray());

                    // Create a new DataSet
                    //DataSet dsShapeCodeForBeam = new DataSet();

                    // Fill the DataSet with the results of the stored procedure
                    dataAdapter.Fill(dsCustomInvLength);

                    //dsCustomInvLength = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.sp_Check_IsCustomInvCutLength, dynamicParameters, commandType: CommandType.StoredProcedure);

                }


                if (dsCustomInvLength.Tables.Count > 0)
                {

                    if (dsCustomInvLength.Tables[0].Rows[0][0].ToString().Equals("1"))
                    {

                        r_intProdLen = r_intInvLen;

                    }
                }
                //}
                // End

                //Form the BVBS string here.
                if (ds.Tables.Count > 0)
                {
                    bvbs = GetBVBS(ds.Tables[0], ShapeParametersList, r_intInvLen, r_intProdLen);
                }

                errMsg = "";
            }
            catch (Exception ex)
            {
                errMsg = ex.Message.ToString();
                r_intInvLen = 0;
                r_intProdLen = 0;
                r_intTotBend = 0;
                r_intTotArc = 0;
                bvbs = string.Empty;
            }
            finally
            {
                //dbManager.Close();
                //dbManager.Dispose();
                Array.Clear(arrLength, 0, arrLength.Length);
                Array.Clear(arr3Dlength, 0, arr3Dlength.Length);
                Array.Clear(arrAngle, 0, arrAngle.Length);
                ds.Dispose();
                dtShapeDetails.Dispose();
            }
            return accList;
        }

        /// <summary>
        /// Method to get the BVBS string.
        /// </summary>
        /// <param name="dtShapeDetails"></param>
        /// <param name="ShapeParametersList"></param>
        /// <param name="iLength"></param>
        /// <param name="pLength"></param>
        /// <returns></returns>
        public string GetBVBS(DataTable dtShapeDetails, List<ShapeParameter> ShapeParametersList, int iLength, int pLength)
        {
            StringBuilder bvbsString = new StringBuilder();
            string[] arrSpecial = { "800", "R88" };
            try
            {
                if (dtShapeDetails.Rows.Count > 0)
                {
                    if (dtShapeDetails.Rows[0]["BVBS_TEMPLATE"].ToString().Equals("0") && Array.IndexOf(arrSpecial, this.ShapeCode.Trim().ToUpper()) < 0)
                    {
                        #region No BVBS Template
                        bvbsString.Append("G");
                        for (int rowinc = 0; rowinc <= dtShapeDetails.Rows.Count - 1; rowinc++)
                        {
                            string parameterValue = string.Empty;
                            string paramValueArcLen = string.Empty;
                            for (Int16 row = 0; row <= ShapeParametersList.Count - 1; row++)
                            {
                                if (dtShapeDetails.Rows[rowinc]["CSD_SEQ_NO"].ToString() == ShapeParametersList[row].SequenceNumber.ToString())
                                {
                                    parameterValue = ShapeParametersList[row].ParameterValueCab;
                                    if (dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("LENGTH"))
                                    {
                                        bvbsString.Append("l" + parameterValue.Trim() + "@");
                                    }
                                    else if (dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("ARC"))
                                    {
                                        if (dtShapeDetails.Rows[rowinc]["CSD_INPUT_TYPE"].ToString().ToUpper().Equals("RAD+ARCLENGTH"))
                                        {
                                            if (dtShapeDetails.Rows[rowinc]["CSD_ARC_DIR"].ToString().ToUpper().Equals("CLOCK"))
                                            {
                                                bvbsString.Append("r" + (Convert.ToInt32(parameterValue) / 2).ToString().Trim() + "@w-" + dtShapeDetails.Rows[rowinc]["CSD_PAR2"].ToString() + "@");
                                            }
                                            else
                                            {
                                                bvbsString.Append("r" + (Convert.ToInt32(parameterValue) / 2).ToString() + "@w" + dtShapeDetails.Rows[rowinc]["CSD_PAR2"].ToString() + "@");
                                            }
                                        }
                                        else if (dtShapeDetails.Rows[rowinc]["CSD_INPUT_TYPE"].ToString().ToUpper().Equals("RAD+SW_ANGLE"))
                                        {
                                            //GrE@W(A*180)/(E*3.14)@w0
                                            paramValueArcLen = ShapeParametersList[row].ParameterValueCab;
                                            parameterValue = (from item in ShapeParametersList
                                                              where item.SequenceNumber == ShapeParametersList[row].SequenceNumber + 1
                                                              select item.ParameterValueCab).Max();
                                            if (dtShapeDetails.Rows[rowinc]["CSD_ARC_DIR"].ToString().ToUpper().Equals("CLOCK"))
                                            {
                                                bvbsString.Append("r" + (parameterValue).ToString() + "@w-" + Math.Floor((Convert.ToInt32(paramValueArcLen) * 180) / (Convert.ToInt32(parameterValue) * Math.PI)).ToString() + "@");
                                            }
                                            else
                                            {
                                                bvbsString.Append("r" + (parameterValue).ToString() + "@w" + Math.Floor((Convert.ToInt32(paramValueArcLen) * 180) / (Convert.ToInt32(parameterValue) * Math.PI)).ToString() + "@");
                                            }
                                        }
                                    }
                                    else if (dtShapeDetails.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("ANGLE"))
                                    {
                                        if (!dtShapeDetails.Rows[rowinc]["CSD_SEQ_NO"].ToString().Equals("1"))
                                        {
                                            if (dtShapeDetails.Rows[rowinc]["CSD_INPUT_TYPE"].ToString().ToUpper().Equals("ANTICLOCKWISE"))
                                            {
                                                bvbsString.Append("w" + parameterValue.ToString() + "@");
                                            }
                                            else
                                            {
                                                bvbsString.Append("w" + "-" + parameterValue.ToString() + "@");
                                            }
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        bvbsString.Append("w0");
                        #endregion
                    }
                    else if (Array.IndexOf(arrSpecial, this.ShapeCode.Trim().ToUpper()) >= 0)
                    {
                        //e.g C= 1050, D=720 
                        //C/2 = 1050/2 = 525; 
                        //[D/(C/2)*180]/3.14= (720/525)*180/3.14=78 (radian)
                        //Gr525@w359@r525@w78 @w0
                        string C = ShapeParametersList.FirstOrDefault(x => x.ParameterName == "C").ParameterValueCab;
                        string D = ShapeParametersList.FirstOrDefault(x => x.ParameterName == "D").ParameterValueCab;
                        bvbsString.Append("Gr" + Convert.ToInt32((Convert.ToInt32(C) / 2)).ToString() + "@w359@r" + Convert.ToInt32((Convert.ToInt32(C) / 2)).ToString() + "@w" + Convert.ToInt32(((2.0 * Convert.ToDouble(D) / Convert.ToDouble(C)) * 180) / 3.14).ToString() + "@w0");
                    }
                    else
                    {
                        //Get the BVBS template
                        string bvbsTemplate = dtShapeDetails.Rows[0]["BVBS_TEMPLATE"].ToString();
                        //Generate the bvbs string by replacing the parameters
                        ShapeParametersList = ShapeParametersList.OrderBy(O => O.ParameterName).ToList();

                        string[] arr = bvbsTemplate.Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries);
                        for (int count = 0; count < arr.Length; count++)
                        {
                            for (int i = 0; i <= ShapeParametersList.Count - 1; i++)
                            {
                                if (ShapeParametersList[i].ParameterName != "" && ShapeParametersList[i].ParameterName != string.Empty)
                                {
                                    if (count == 0)
                                    {
                                        if (ShapeParametersList[i].ParameterName.Equals(arr[count].ToString().Substring(2)))
                                        {
                                            bvbsString.Append(arr[count].ToString().Substring(0, 2) + ShapeParametersList[i].ParameterValueCab.ToString().Trim());
                                            break;
                                        }
                                        else if (arr[count].ToString().Substring(2).Equals("ILength"))
                                        {
                                            bvbsString.Append(arr[count].ToString().Substring(0, 2) + iLength.ToString().Trim());
                                            break;
                                        }
                                        else if (arr[count].ToString().Substring(2).Equals("PLength"))
                                        {
                                            bvbsString.Append(arr[count].ToString().Substring(0, 2) + pLength.ToString().Trim());
                                            break;
                                        }
                                        else if (arr[count].ToString().Substring(2).Contains("*"))
                                        {
                                            if (ShapeParametersList[i].ParameterName.Equals(arr[count].ToString().Substring(2, 1)))
                                            {
                                                if (!ShapeParametersList[i].ParameterValueCab.Equals("0"))
                                                {
                                                    bvbsString.Append(arr[count].ToString().Substring(0, 2) + (Convert.ToInt32(ShapeParametersList[i].ParameterValueCab) * Convert.ToInt32(arr[count].ToString().Substring(arr[count].ToString().IndexOf("/", 0) + 1, 1))).ToString().Trim());
                                                    break;
                                                }
                                            }
                                            else if (arr[count].ToString().Substring(2).Contains("(") && arr[count].ToString().Substring(2).Contains(")"))
                                            {
                                                List<valuePair> valueList = new List<valuePair>();
                                                //Create the value pair.
                                                foreach (ShapeParameter shape in ShapeParametersList)
                                                {
                                                    if (shape.ParameterName != "" && shape.ParameterName != string.Empty && shape.ParameterName != null)
                                                    {
                                                        if (!shape.AngleType.Equals("ESPLICE") && !shape.AngleType.Equals("NSPLICE"))
                                                        {
                                                            valueList.Add(new valuePair(shape.ParameterName, Convert.ToDouble(shape.ParameterValueCab)));
                                                        }
                                                    }
                                                }
                                                MathParserWrapper objParse = new MathParserWrapper();
                                                int result = Convert.ToInt32(objParse.GetCalculatedValue(arr[count].ToString().Substring(2), valueList, false));
                                                objParse = null;
                                                //Append bvbs string.
                                                bvbsString.Append(arr[count].ToString().Substring(0, 2) + result.ToString());
                                                break;
                                            }
                                        }
                                        else if (arr[count].ToString().Substring(2).Contains("/"))
                                        {
                                            if (ShapeParametersList[i].ParameterName.Equals(arr[count].ToString().Substring(2, 1)))
                                            {
                                                if (!ShapeParametersList[i].ParameterValueCab.Equals("0"))
                                                {
                                                    bvbsString.Append(arr[count].ToString().Substring(0, 2) + (Convert.ToInt32(ShapeParametersList[i].ParameterValueCab) / Convert.ToInt32(arr[count].ToString().Substring(arr[count].ToString().IndexOf("/", 0) + 1, 1))).ToString().Trim());
                                                    break;
                                                }
                                            }
                                            else if (arr[count].ToString().Substring(2).Contains("(") && arr[count].ToString().Substring(2).Contains(")"))
                                            {
                                                List<valuePair> valueList = new List<valuePair>();
                                                //Create the value pair.
                                                foreach (ShapeParameter shape in ShapeParametersList)
                                                {
                                                    if (shape.ParameterName != "" && shape.ParameterName != string.Empty && shape.ParameterName != null)
                                                    {
                                                        if (!shape.AngleType.Equals("ESPLICE") && !shape.AngleType.Equals("NSPLICE"))
                                                        {
                                                            valueList.Add(new valuePair(shape.ParameterName, Convert.ToDouble(shape.ParameterValueCab)));
                                                        }
                                                    }
                                                }
                                                MathParserWrapper objParse = new MathParserWrapper();
                                                int result = Convert.ToInt32(objParse.GetCalculatedValue(arr[count].ToString().Substring(2), valueList, false));
                                                objParse = null;
                                                //Append bvbs string.
                                                bvbsString.Append(arr[count].ToString().Substring(0, 2) + result.ToString());
                                                break;
                                            }
                                        }
                                        else if (isNumeric(arr[count].ToString().Substring(2)))
                                        {
                                            bvbsString.Append("@" + arr[count].ToString());
                                        }
                                    }
                                    else if (count > 0)
                                    {
                                        if (!arr[count].ToString().Contains("-"))
                                        {
                                            if (ShapeParametersList[i].ParameterName.Equals(arr[count].ToString().Substring(1)))
                                            {
                                                bvbsString.Append("@" + arr[count].ToString().Substring(0, 1) + ShapeParametersList[i].ParameterValueCab.ToString().Trim());
                                                break;
                                            }
                                            else if (arr[count].ToString().Substring(1).Equals("ILength"))
                                            {
                                                bvbsString.Append("@" + arr[count].ToString().Substring(0, 1) + iLength.ToString().Trim());
                                                break;
                                            }
                                            else if (arr[count].ToString().Substring(1).Equals("PLength"))
                                            {
                                                bvbsString.Append("@" + arr[count].ToString().Substring(0, 1) + pLength.ToString().Trim());
                                                break;
                                            }
                                            else if (arr[count].ToString().Substring(2).Contains("*"))
                                            {
                                                if (ShapeParametersList[i].ParameterName.Equals(arr[count].ToString().Substring(1, 1)))
                                                {
                                                    if (!ShapeParametersList[i].ParameterValueCab.Equals("0"))
                                                    {
                                                        bvbsString.Append("@" + arr[count].ToString().Substring(0, 1) + (Convert.ToInt32(ShapeParametersList[i].ParameterValueCab) * Convert.ToInt32(arr[count].ToString().Substring(arr[count].ToString().IndexOf("/", 0) + 1, 1))).ToString().Trim());
                                                        break;
                                                    }
                                                }
                                                else if (arr[count].ToString().Substring(1).Contains("(") && arr[count].ToString().Substring(1).Contains(")"))
                                                {
                                                    List<valuePair> valueList = new List<valuePair>();
                                                    //Create the value pair.
                                                    foreach (ShapeParameter shape in ShapeParametersList)
                                                    {
                                                        if (shape.ParameterName != "" && shape.ParameterName != string.Empty && shape.ParameterName != null)
                                                        {
                                                            if (!shape.AngleType.Equals("ESPLICE") && !shape.AngleType.Equals("NSPLICE"))
                                                            {
                                                                valueList.Add(new valuePair(shape.ParameterName, Convert.ToDouble(shape.ParameterValueCab)));
                                                            }
                                                        }
                                                    }
                                                    MathParserWrapper objParse = new MathParserWrapper();
                                                    int result = Convert.ToInt32(objParse.GetCalculatedValue(arr[count].ToString().Substring(1), valueList, false));
                                                    objParse = null;
                                                    //Append bvbs string.
                                                    bvbsString.Append("@" + arr[count].ToString().Substring(0, 1) + result.ToString());
                                                    break;
                                                }
                                            }
                                            else if (arr[count].ToString().Substring(2).Contains("/"))
                                            {
                                                if (ShapeParametersList[i].ParameterName.Equals(arr[count].ToString().Substring(1, 1)))
                                                {
                                                    if (!ShapeParametersList[i].ParameterValueCab.Equals("0"))
                                                    {
                                                        bvbsString.Append("@" + arr[count].ToString().Substring(0, 1) + (Convert.ToInt32(ShapeParametersList[i].ParameterValueCab) / Convert.ToInt32(arr[count].ToString().Substring(arr[count].ToString().IndexOf("/", 0) + 1, 1))).ToString().Trim());
                                                        break;
                                                    }
                                                }
                                                else if (arr[count].ToString().Substring(1).Contains("(") && arr[count].ToString().Substring(1).Contains(")"))
                                                {
                                                    List<valuePair> valueList = new List<valuePair>();
                                                    //Create the value pair.
                                                    foreach (ShapeParameter shape in ShapeParametersList)
                                                    {
                                                        if (shape.ParameterName != "" && shape.ParameterName != string.Empty && shape.ParameterName != null)
                                                        {
                                                            if (!shape.AngleType.Equals("ESPLICE") && !shape.AngleType.Equals("NSPLICE"))
                                                            {
                                                                valueList.Add(new valuePair(shape.ParameterName, Convert.ToDouble(shape.ParameterValueCab)));
                                                            }
                                                        }
                                                    }
                                                    MathParserWrapper objParse = new MathParserWrapper();
                                                    int result = Convert.ToInt32(objParse.GetCalculatedValue(arr[count].ToString().Substring(1), valueList, false));
                                                    objParse = null;
                                                    //Append bvbs string.
                                                    bvbsString.Append("@" + arr[count].ToString().Substring(0, 1) + result.ToString());
                                                    break;
                                                }
                                            }
                                            else if (isNumeric(arr[count].ToString().Substring(1)))
                                            {
                                                bvbsString.Append("@" + arr[count].ToString());
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            if (ShapeParametersList[i].ParameterName.Equals(arr[count].ToString().Substring(2)))
                                            {
                                                bvbsString.Append("@" + arr[count].ToString().Substring(0, 2) + ShapeParametersList[i].ParameterValueCab.ToString().Trim());
                                                break;
                                            }
                                            else if (arr[count].ToString().Substring(2).Equals("ILength"))
                                            {
                                                bvbsString.Append("@" + arr[count].ToString().Substring(0, 2) + iLength.ToString().Trim());
                                                break;
                                            }
                                            else if (arr[count].ToString().Substring(2).Equals("PLength"))
                                            {
                                                bvbsString.Append("@" + arr[count].ToString().Substring(0, 2) + pLength.ToString().Trim());
                                                break;
                                            }
                                            else if (arr[count].ToString().Substring(2).Contains("*"))
                                            {
                                                if (ShapeParametersList[i].ParameterName.Equals(arr[count].ToString().Substring(2, 1)))
                                                {
                                                    if (!ShapeParametersList[i].ParameterValueCab.Equals("0"))
                                                    {
                                                        bvbsString.Append("@" + arr[count].ToString().Substring(0, 2) + (Convert.ToInt32(ShapeParametersList[i].ParameterValueCab) * Convert.ToInt32(arr[count].ToString().Substring(arr[count].ToString().IndexOf("/", 0) + 1, 1))).ToString().Trim());
                                                        break;
                                                    }
                                                }
                                                else if (arr[count].ToString().Substring(2).Contains("(") && arr[count].ToString().Substring(2).Contains(")"))
                                                {
                                                    List<valuePair> valueList = new List<valuePair>();
                                                    //Create the value pair.
                                                    foreach (ShapeParameter shape in ShapeParametersList)
                                                    {
                                                        if (shape.ParameterName != "" && shape.ParameterName != string.Empty && shape.ParameterName != null)
                                                        {
                                                            if (!shape.AngleType.Equals("ESPLICE") && !shape.AngleType.Equals("NSPLICE"))
                                                            {
                                                                valueList.Add(new valuePair(shape.ParameterName, Convert.ToDouble(shape.ParameterValueCab)));
                                                            }
                                                        }
                                                    }
                                                    MathParserWrapper objParse = new MathParserWrapper();
                                                    int result = Convert.ToInt32(objParse.GetCalculatedValue(arr[count].ToString().Substring(2), valueList, false));
                                                    objParse = null;
                                                    //Append bvbs string.
                                                    bvbsString.Append("@" + arr[count].ToString().Substring(0, 2) + result.ToString());
                                                    break;
                                                }
                                            }
                                            else if (arr[count].ToString().Substring(2).Contains("/"))
                                            {
                                                if (ShapeParametersList[i].ParameterName.Equals(arr[count].ToString().Substring(2, 1)))
                                                {
                                                    if (!ShapeParametersList[i].ParameterValueCab.Equals("0"))
                                                    {
                                                        bvbsString.Append("@" + arr[count].ToString().Substring(0, 2) + (Convert.ToInt32(ShapeParametersList[i].ParameterValueCab) / Convert.ToInt32(arr[count].ToString().Substring(arr[count].ToString().IndexOf("/", 0) + 1, 1))).ToString().Trim());
                                                        break;
                                                    }
                                                }
                                                else if (arr[count].ToString().Substring(2).Contains("(") && arr[count].ToString().Substring(2).Contains(")"))
                                                {
                                                    List<valuePair> valueList = new List<valuePair>();
                                                    //Create the value pair.
                                                    foreach (ShapeParameter shape in ShapeParametersList)
                                                    {
                                                        if (shape.ParameterName != "" && shape.ParameterName != string.Empty && shape.ParameterName != null)
                                                        {
                                                            if (!shape.AngleType.Equals("ESPLICE") && !shape.AngleType.Equals("NSPLICE"))
                                                            {
                                                                valueList.Add(new valuePair(shape.ParameterName, Convert.ToDouble(shape.ParameterValueCab)));
                                                            }
                                                        }
                                                    }
                                                    MathParserWrapper objParse = new MathParserWrapper();
                                                    int result = Convert.ToInt32(objParse.GetCalculatedValue(arr[count].ToString().Substring(2), valueList, false));
                                                    objParse = null;
                                                    //Append bvbs string.
                                                    bvbsString.Append("@" + arr[count].ToString().Substring(0, 2) + result.ToString());
                                                    break;
                                                }
                                            }
                                            else if (isNumeric(arr[count].ToString().Substring(2)))
                                            {
                                                bvbsString.Append("@" + arr[count].ToString());
                                                break;
                                            }
                                        }
                                    }
                                    else if (count == arr.Length - 1)
                                    {
                                        bvbsString.Append("@w0");
                                    }
                                }
                            }
                        }
                    }
                }
                return bvbsString.ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
            finally
            {
                dtShapeDetails.Dispose();
                bvbsString = null;
                Array.Clear(arrSpecial, 0, arrSpecial.Length);
            }
        }

        /// <summary>
        /// Method to get the height angle.
        /// </summary>
        /// <param name="shapeparam"></param>
        /// <param name="param"></param>
        /// <param name="shape"></param>
        /// <returns></returns>
        public int SetHeightAngle(string[,] shapeparam, string param, string shape)
        {
            //DBManager dbManager = new DBManager();
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            //dbManager.Open();
            try
            {
                int val = 0;
                sb = sb.Append("SELECT VCHHEIGHTANGLEFORMULA FROM SHAPEMASTER_CUBE S, SHAPEPARAMDETAILS_CUBE D ");
                sb = sb.Append("WHERE S.INTSHAPEID = D.INTSHAPEID AND intparamseq = " + (Convert.ToInt32(param)).ToString());
                sb = sb.Append(" AND CHRSHAPECODE = '" + shape + "'");
               
                ds = ExecuteDataSet(CommandType.Text, sb.ToString());
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string formula = ds.Tables[0].Rows[0][0].ToString();
                    //MathParserWrapper mth = new MathParserWrapper();
                    //val = Convert.ToInt32(mth.EvaluateFormula(formula, Convert.ToBoolean(shapeparam)));
                    MathParser mth = new MathParser();
                    val = Convert.ToInt32(mth.Evaluate(formula, shapeparam));
                    mth = null;
                }
                return val;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                //dbManager.Close();
                //dbManager.Dispose();
            }
        }

        private DataSet ExecuteDataSet(CommandType text, string v)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method to check if string is numeric.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool isNumeric(String str)
        {
            try
            {
                double d = Double.Parse(str);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
