using System.Text;
using System.Data;
using System.Xml.Linq;
using System.Reflection;
using Dapper;
using DetailingService.Constants;
using SharedCache.WinServiceCommon.Provider.Cache;
using Microsoft.Data.SqlClient;
using NatSteel.NDS.BLL;
using ExpressionParser;

namespace DetailingService.Repositories
{

    public class CABItem
    {
        private readonly IConfiguration _configuration;
        //private string connectionString= "Server=NSQADB5\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=MES_USER;Password=Mes@123;Connection Timeout=36000000";
        private object dbManager;
        private string connectionString = "Server=NSQADB5\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=MES_USER;Password=Mes@123;Connection Timeout=36000000";

        private object dynamicParameters;
        private object cmd;

        public CABItem(IConfiguration configuration)
        {
            //_dbContext = dbContext;
            _configuration = configuration;

            connectionString = _configuration.GetConnectionString(SystemConstant.DefaultDBConnection);
        }

        public CABItem()
        {

        }


        #region "Properties .."
        public Int32 SEDetailingID { get; set; }
        public Int32 StructureMarkId { get; set; }
        public Int32 CABProductMarkID { get; set; }
        public Int32 ProductCodeID { get; set; }
        public int  Quantity { get; set; }
        public int  PinSizeID { get; set; }
        public string Status { get; set; }
        public string DDStatus { get; set; } // Added for CAB Shape Parameter Validation Data Discrepancy CR
        public int UserID { get; set; } // Added for CAB Shape Parameter Validation Data Discrepancy CR #Activity Task
        public string Activity { get; set; }// Added for CAB Shape Parameter Validation Data Discrepancy CR #Activity Task

        public Int32  Diameter { get; set; }
        public string ShapeCode { get; set; }
        public string DescScript { get; set; }
        public string CABProductMarkName { get; set; }

        public List<ShapeParameter> ShapeParametersList { get; set; }
        public List<Accessory> accList { get; set; }

        public CABItem CABProductItem { get; set; }
        public int MemberQty { get; set; }
        public int PinSize { get; set; }
        public double InvoiceLength { get; set; }
        public double ProductionLength { get; set; }
        public double InvoiceWeight { get; set; }
        public double ProductionWeight { get; set; }
        public string Grade { get; set; }
        public int ShapeTransHeaderID { get; set; }
        public int GroupMarkId { get; set; }
        public string ShapeGroup { get; set; }
        public int EndCount { get; set; }
        public string ImagePath { get; set; }
        public string CustomerRemarks { get; set; }
        public string PageNumber { get; set; }
        public double EnvLength { get; set; }
        public double EnvWidth { get; set; }
        public double EnvHeight { get; set; }
        public string ShapeImage { get; set; }
        public int NoOfBends { get; set; }
        public int TransportModeId { get; set; }
        public string BVBS { get; set; }
        public string CreatedBy { get; set; }
        public string ProduceIndicator { get; set; }
        public string BarMark { get; set; }
        public string CommercialDesc { get; set; }

        public string ShapeType { get; set; }
        public Accessory accItem { get; set; }

        public bool IsReadOnly { get; set; }
        public int intSEDetailingId { get; set; }
        public string ipAddress { get; set; }

        public string ACCProductNameforCAB { get; set; }


        public string Coupler1 { get; set; }
        public string Coupler2 { get; set; }
        public string Thread1 { get; set; }
        public string Thread2 { get; set; }
        public string Coupler1Type { get; set; }
        public string Coupler1Standard { get; set; }
        public string Thread1Type { get; set; }
        public string Thread1Standard { get; set; }
        public string Coupler2Type { get; set; }
        public string Coupler2Standard { get; set; }
        public string Thread2Type { get; set; }
        public string Thread2Standard { get; set; }

        //Locknut
        public string Locknut1 { get; set; }
        public string Locknut2 { get; set; }

        //BBS No
        public string BBSNo { get; set; }

        //VPN User
        public bool IsVPNUsers { get; set; }

        //New parameter for cube integration
        public string ProduceInd { get; set; }
        public ShapeCode Shape { get; set; }
        public string ShapeName { get; set; }
        public bool IsVariableBar { get; set; }

        public int ShapeParamTotal { get; set; }
        public string ShapeshapeParameters { get; set; }
        public string shapeLengthFormula { get; set; }
        public string shapeParaValidator { get; set; }
        public string shapeTransportValidator { get; set; }

        #endregion


        #region Cube
        /// <summary>
        /// Method to get the sedetailingid from groupmarkid for BPC.
        /// </summary>
        /// <param name="groupMarkId"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public int GetSeDetailingId(int groupMarkId, out string errorMessage)
        {
            int seDetailingId = 0;
            errorMessage = string.Empty;
            DBManager dbManager = new DBManager();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTGROUPMARK", groupMarkId.ToString());
                    object returnVal = sqlConnection.Query<DataSet>(SystemConstant.USP_GET_SEDETAILING_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);
                    return Convert.ToInt32(returnVal);
                }
                //dbManager.Open();
                //dbManager.CreateParameters(1);
                //dbManager.AddParameters(0, "@INTGROUPMARK", groupMarkId.ToString());
                //object returnVal = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "USP_GET_SEDETAILING_CUBE");
                //return Convert.ToInt32(returnVal);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return seDetailingId;
            }
            finally
            {

            }

        }

        /// <summary>
        /// Method to calcullate the envelope length.
        /// </summary>
        /// <param name="shapeCode"></param>
        /// <param name="shapeparameterList"></param>
        /// <param name="dia"></param>
        /// <param name="envLength"></param>
        /// <param name="envWidth"></param>
        /// <param name="envHeight"></param>
        /// <param name="errorMsg"></param>
        public void EnvelopeCalculation(string shapeCode, List<ShapeParameter> shapeparameterList, int dia, out int envLength, out int envWidth, out int envHeight, out string errorMsg)
        {
            envLength = 0;
            envWidth = 0;
            envHeight = 0;
            errorMsg = "";
            string[] arrHeight = { "HEIGHT", "OFFSET LENGTH", "ON_HEIGHT", "ON_OFFSET" };
            DBManager dbManager = new DBManager();
            DataSet ds = new DataSet();
            try
            {
                //Get the shape code cordinates along with the default values.
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@SHAPECODE", shapeCode);
                    //ds = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.USP_GET_SHAPE_CORD_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);
                    var dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = new SqlCommand(SystemConstant.USP_GET_SHAPE_CORD_CUBE, sqlConnection);
                    dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dataAdapter.SelectCommand.Parameters.AddRange(dynamicParameters.ParameterNames.Select(name => new SqlParameter(name, dynamicParameters.Get<object>(name))).ToArray());
                    dataAdapter.Fill(ds);

                }
                
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[1].Rows[0]["ENV_LENGTH"].ToString().Equals("0") || ds.Tables[1].Rows[0]["ENV_LENGTH"].ToString().Trim().Equals(string.Empty) || ds.Tables[1].Rows[0]["ENV_LENGTH"].ToString().Trim().Equals(""))
                        {
                            DataTable dt = ds.Tables[0];
                            //Calcullate the cordinates.
                            if (dt.Rows.Count > 0)
                            {
                                int startX = Convert.ToInt32(dt.Rows[0]["CSM_X_COR"].ToString());
                                int startY = Convert.ToInt32(dt.Rows[0]["CSM_Y_COR"].ToString());
                                double scale = Convert.ToDouble(dt.Rows[0]["CSM_SCALE"].ToString());
                                string catagory = dt.Rows[0]["CSC_CAT_DESC"].ToString().ToUpper();
                                int count = 0;
                                int[,] arr = new int[dt.Rows.Count, 3];
                                if (catagory != "3-D")
                                {
                                    for (int rowinc = 0; rowinc <= dt.Rows.Count - 1; rowinc++)
                                    {
                                        for (Int16 row = 0; row <= shapeparameterList.Count - 1; row++)
                                        {
                                            if (dt.Rows[rowinc]["CSD_SEQ_NO"].ToString() == shapeparameterList[row].SequenceNumber.ToString())
                                            {
                                                if (Convert.ToInt32(dt.Rows[rowinc]["CSD_PAR1"].ToString()) != 0)
                                                {
                                                    if (count == 0)
                                                    {
                                                        arr[rowinc, 0] = startX + Convert.ToInt32((Convert.ToInt32(dt.Rows[rowinc]["CSD_END_POINT_X"].ToString()) - startX) * Convert.ToInt32(shapeparameterList[row].ParameterValueCab) / Convert.ToDouble(dt.Rows[rowinc]["CSD_PAR1"].ToString()));
                                                        arr[rowinc, 1] = startY + Convert.ToInt32((Convert.ToInt32(dt.Rows[rowinc]["CSD_END_POINT_Y"].ToString()) - startY) * Convert.ToInt32(shapeparameterList[row].ParameterValueCab) / Convert.ToDouble(dt.Rows[rowinc]["CSD_PAR1"].ToString()));
                                                        count++;
                                                    }
                                                    else
                                                    {
                                                        if (dt.Rows[rowinc - 1]["CSD_TYPE"].ToString().ToUpper().Equals("ANGLE"))
                                                        {
                                                            int paramVal = 0;
                                                            ShapeParameter shape = shapeparameterList.FirstOrDefault(x => x.SequenceNumber == shapeparameterList[row].SequenceNumber - 1);
                                                            paramVal = Convert.ToInt32(shape.ParameterValueCab);
                                                            arr[rowinc, 0] = arr[rowinc - 1, 0] + Convert.ToInt32((Convert.ToInt32(dt.Rows[rowinc]["CSD_END_POINT_X"].ToString()) - Convert.ToInt32(dt.Rows[rowinc - 1]["CSD_END_POINT_X"].ToString())) * (Convert.ToInt32(shapeparameterList[row].ParameterValueCab) / Convert.ToDouble(dt.Rows[rowinc]["CSD_PAR1"].ToString())) * ((Math.Cos(paramVal * Math.PI / 180)) / (Math.Cos(Convert.ToInt32(dt.Rows[rowinc - 1]["CSD_PAR2"].ToString()) * Math.PI / 180))));
                                                            arr[rowinc, 1] = arr[rowinc - 1, 1] + Convert.ToInt32((Convert.ToInt32(dt.Rows[rowinc]["CSD_END_POINT_Y"].ToString()) - Convert.ToInt32(dt.Rows[rowinc - 1]["CSD_END_POINT_Y"].ToString())) * (Convert.ToInt32(shapeparameterList[row].ParameterValueCab) / Convert.ToDouble(dt.Rows[rowinc]["CSD_PAR1"].ToString())) * ((Math.Sin(paramVal * Math.PI / 180)) / (Math.Sin(Convert.ToInt32(dt.Rows[rowinc - 1]["CSD_PAR2"].ToString()) * Math.PI / 180))));
                                                            count++;
                                                        }
                                                        else
                                                        {
                                                            arr[rowinc, 0] = arr[rowinc - 1, 0] + Convert.ToInt32((Convert.ToInt32(dt.Rows[rowinc]["CSD_END_POINT_X"].ToString()) - Convert.ToInt32(dt.Rows[rowinc - 1]["CSD_END_POINT_X"].ToString())) * (Convert.ToInt32(shapeparameterList[row].ParameterValueCab) / Convert.ToDouble(dt.Rows[rowinc]["CSD_PAR1"].ToString())));
                                                            arr[rowinc, 1] = arr[rowinc - 1, 1] + Convert.ToInt32((Convert.ToInt32(dt.Rows[rowinc]["CSD_END_POINT_Y"].ToString()) - Convert.ToInt32(dt.Rows[rowinc - 1]["CSD_END_POINT_Y"].ToString())) * (Convert.ToInt32(shapeparameterList[row].ParameterValueCab) / Convert.ToDouble(dt.Rows[rowinc]["CSD_PAR1"].ToString())));
                                                            count++;
                                                        }
                                                    }
                                                }
                                                else if (Array.IndexOf(arrHeight, dt.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper()) >= 0)
                                                {
                                                    arr[rowinc, 0] = arr[rowinc - 1, 0];
                                                    arr[rowinc, 1] = arr[rowinc - 1, 1];
                                                    dt.Rows[rowinc]["CSD_END_POINT_X"] = dt.Rows[rowinc - 1]["CSD_END_POINT_X"];
                                                    dt.Rows[rowinc]["CSD_END_POINT_Y"] = dt.Rows[rowinc - 1]["CSD_END_POINT_Y"];
                                                    count++;
                                                }
                                                else
                                                {
                                                    if (count > 0)
                                                    {
                                                        if (!(dt.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("ESPLICE")) && !(dt.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("ESPLICE LENGTH")) && !(dt.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("NSPLICE")) && !(dt.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("SPAN")) && !(dt.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("ARC LENGTH")) && !(dt.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("ARC HEIGHT")))
                                                        {
                                                            arr[rowinc, 0] = arr[rowinc - 1, 0] + Convert.ToInt32((Convert.ToInt32(dt.Rows[rowinc]["CSD_END_POINT_X"].ToString()) - Convert.ToInt32(dt.Rows[rowinc - 1]["CSD_END_POINT_X"].ToString())) * (Math.Cos(Convert.ToInt32(shapeparameterList[row].ParameterValueCab) * Math.PI / 180)) / (Math.Cos(Convert.ToInt32(dt.Rows[rowinc]["CSD_PAR2"].ToString()) * Math.PI / 180)));
                                                            arr[rowinc, 1] = arr[rowinc - 1, 1] + Convert.ToInt32((Convert.ToInt32(dt.Rows[rowinc]["CSD_END_POINT_Y"].ToString()) - Convert.ToInt32(dt.Rows[rowinc - 1]["CSD_END_POINT_Y"].ToString())) * (Math.Sin(Convert.ToInt32(shapeparameterList[row].ParameterValueCab) * Math.PI / 180)) / (Math.Sin(Convert.ToInt32(dt.Rows[rowinc]["CSD_PAR2"].ToString()) * Math.PI / 180)));
                                                            count++;
                                                        }
                                                        else
                                                        {
                                                            arr[rowinc, 0] = arr[rowinc - 1, 0];
                                                            arr[rowinc, 1] = arr[rowinc - 1, 1];
                                                            dt.Rows[rowinc]["CSD_END_POINT_X"] = dt.Rows[rowinc - 1]["CSD_END_POINT_X"];
                                                            dt.Rows[rowinc]["CSD_END_POINT_Y"] = dt.Rows[rowinc - 1]["CSD_END_POINT_Y"];
                                                            count++;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (!(dt.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("HEIGHT")) && !(dt.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("OFFSET LENGTH")))
                                                        {
                                                            arr[rowinc, 0] = startX + (Convert.ToInt32(dt.Rows[rowinc]["CSD_END_POINT_X"].ToString()) - startX);
                                                            arr[rowinc, 1] = startY + (Convert.ToInt32(dt.Rows[rowinc]["CSD_END_POINT_Y"].ToString()) - startY);
                                                            count++;
                                                        }
                                                    }
                                                }
                                                break;
                                            }
                                        }
                                    }
                                    //Get the minimum X cordinate.
                                    int minX = 25000;
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        if (arr[i, 0] < minX)
                                        {
                                            minX = arr[i, 0];
                                        }
                                    }
                                    if (minX > startX)
                                    {
                                        minX = startX;
                                    }
                                    //Get the maximum X cordinate.
                                    int maxX = 0;
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        if (arr[i, 0] > maxX)
                                        {
                                            maxX = arr[i, 0];
                                        }
                                    }
                                    if (maxX < startX)
                                    {
                                        maxX = startX;
                                    }
                                    //Get the minimum Y cordinate
                                    int minY = 25000;
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        if (arr[i, 1] < minY)
                                        {
                                            minY = arr[i, 1];
                                        }
                                    }
                                    if (minY > startY)
                                    {
                                        minY = startY;
                                    }
                                    //Get the maximum Y cordinate
                                    int maxY = 0;
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        if (arr[i, 1] > maxY)
                                        {
                                            maxY = arr[i, 1];
                                        }
                                    }
                                    if (maxY < startY)
                                    {
                                        maxY = startY;
                                    }
                                    //Set the values.
                                    envLength = Convert.ToInt32((maxX - minX) * 10);
                                    envWidth = Convert.ToInt32((maxY - minY) * 10);
                                    if (envWidth == 0)
                                    {
                                        envWidth = dia;
                                    }
                                    envHeight = dia;
                                }
                                else
                                {
                                    //Code here for 3D.
                                }
                                dt.Dispose();
                                Array.Clear(arr, 0, arr.Length);
                            }
                        }
                        else
                        {
                            List<valuePair> valueList = new List<valuePair>();
                            //Create the value pair.
                            ShapeParametersList = ShapeParametersList.OrderBy(O => O.ParameterName).ToList();
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

                            //Add the diameter                                
                            if (dia != 0)
                            {
                                valueList.Add(new valuePair("$", Convert.ToDouble(dia)));
                            }
                            //Add Pin radius
                            if (this.PinSizeID != 0)
                            {
                                valueList.Add(new valuePair("@", Convert.ToDouble(this.PinSizeID / 2)));
                            }
                            //Get the result.
                            MathParserWrapper objParse = new MathParserWrapper();
                            string envLengthFormula = ds.Tables[1].Rows[0]["ENV_LENGTH"].ToString();
                            envLength = Convert.ToInt32(objParse.GetCalculatedValue(envLengthFormula, valueList, false));
                            objParse = null;
                            objParse = new MathParserWrapper();
                            string envWidthFormula = ds.Tables[1].Rows[0]["ENV_WIDTH"].ToString();
                            envWidth = Convert.ToInt32(objParse.GetCalculatedValue(envWidthFormula, valueList, false));
                            objParse = null;
                            objParse = new MathParserWrapper();
                            string envHeightFormula = ds.Tables[1].Rows[0]["ENV_HEIGHT"].ToString();
                            envHeight = Convert.ToInt32(objParse.GetCalculatedValue(envHeightFormula, valueList, false));
                            objParse = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message.ToString();
            }
            finally
            {

                ds.Dispose();
                Array.Clear(arrHeight, 0, arrHeight.Length);
            }
        }



        /// <summary>
        /// Method to copy bar mark on the basis of prod mark id.
        /// </summary>
        /// <param name="cabPodMarkId"></param>
        /// <param name="nextBarMark"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public int CopyBarMark(int cabPodMarkId, string nextBarMark, out string errorMessage)
        {
            int intCABProductMarkID = 0;
            errorMessage = "";
            DBManager dbManager = new DBManager();
            DataSet ds = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTCABPRODUCTMARKID", cabPodMarkId);
                    dynamicParameters.Add("@BARMARKNEXT", nextBarMark);

                    //ds = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.USP_CABPRODUCTMARKINGDETAILS_COPY_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);
                    var dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = new SqlCommand(SystemConstant.USP_CABPRODUCTMARKINGDETAILS_COPY_CUBE, sqlConnection);
                    dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dataAdapter.SelectCommand.Parameters.AddRange(dynamicParameters.ParameterNames.Select(name => new SqlParameter(name, dynamicParameters.Get<object>(name))).ToArray());
                    dataAdapter.Fill(ds);

                }
                //dbManager.Open();
                //dbManager.CreateParameters(2);
                //dbManager.AddParameters(0, "@INTCABPRODUCTMARKID", cabPodMarkId);
                //dbManager.AddParameters(1, "@BARMARKNEXT", nextBarMark);
                //ds = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "USP_CABPRODUCTMARKINGDETAILS_COPY_CUBE");
                if (ds.Tables.Count != 0)
                {
                    foreach (DataRowView drCABProductMarkID in ds.Tables[0].DefaultView)
                    {
                        intCABProductMarkID = Convert.ToInt32(drCABProductMarkID[0].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message.ToString();
            }
            finally
            {

            }
            return intCABProductMarkID;
        }

        /// <summary>
        /// Method to get the pin.
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="dia"></param>
        /// <param name="formerFlag"></param>
        /// <param name="errorMsg"></param>
        /// <param name="pin"></param>
        /// <param name="minLength"></param>
        /// <param name="minHookLen"></param>
        /// <param name="minHookHt"></param>
        public void GetPin(string grade, int dia, int formerFlag, out string errorMsg, out int pin, out int minLength, out int minHookLen, out int minHookHt)
        {

            DBManager dbManager = new DBManager();
            DataSet ds = new DataSet();
            try
            {
                errorMsg = "";
                pin = 0;
                minLength = 0;
                minHookLen = 0;
                minHookHt = 0;


                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTDIA", dia);
                    dynamicParameters.Add("@GRADE", grade);
                    dynamicParameters.Add("@FORMERFLAG", formerFlag);

                    var dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = new SqlCommand(SystemConstant.USP_GET_CABPINBYFORMER_CUBE, sqlConnection);
                    dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dataAdapter.SelectCommand.Parameters.AddRange(dynamicParameters.ParameterNames.Select(name => new SqlParameter(name, dynamicParameters.Get<object>(name))).ToArray());
                    dataAdapter.Fill(ds);

                    //ds = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.USP_GET_CABPINBYFORMER_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);

                }
                //dbManager.Open();
                //dbManager.CreateParameters(3);
                //dbManager.AddParameters(0, "@INTDIA", dia);
                //dbManager.AddParameters(1, "@GRADE", grade);
                //dbManager.AddParameters(2, "@FORMERFLAG", formerFlag);
                //ds = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "USP_GET_CABPINBYFORMER_CUBE");
                if (ds.Tables.Count != 0)
                {
                    foreach (DataRowView drCABPin in ds.Tables[0].DefaultView)
                    {
                        pin = Convert.ToInt32(drCABPin[0].ToString());
                        minLength = Convert.ToInt32(drCABPin[1].ToString());
                        minHookLen = Convert.ToInt32(drCABPin[2].ToString());
                        minHookHt = Convert.ToInt32(drCABPin[3].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message.ToString();
                pin = 0;
                minLength = 0;
                minHookLen = 0;
                minHookHt = 0;
            }
            finally
            {

                ds.Dispose();
            }
        }

        /// <summary>
        /// Method to check and get the pin at edit time.
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="dia"></param>
        /// <param name="pin"></param>
        /// <param name="errorMsg"></param>
        /// <param name="minLength"></param>
        /// <param name="minHookLen"></param>
        /// <param name="minHookHt"></param>
        /// <returns></returns>
        public bool GetPin(string grade, int dia, int pin, out string errorMsg, out int minLength, out int minHookLen, out int minHookHt)
        {
            DBManager dbManager = new DBManager();
            DataSet ds = new DataSet();
            try
            {
                errorMsg = "";
                int count = 0;
                minLength = 0;
                minHookLen = 0;
                minHookHt = 0;

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTDIA", dia);
                    dynamicParameters.Add("@GRADE", grade);
                    dynamicParameters.Add("@PIN", pin);

                    var dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = new SqlCommand(SystemConstant.USP_VALIDATE_CABPIN_CUBE, sqlConnection);
                    dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dataAdapter.SelectCommand.Parameters.AddRange(dynamicParameters.ParameterNames.Select(name => new SqlParameter(name, dynamicParameters.Get<object>(name))).ToArray());
                    dataAdapter.Fill(ds);

                    //ds = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.USP_VALIDATE_CABPIN_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);

                }
                //dbManager.Open();
                //dbManager.CreateParameters(3);
                //dbManager.AddParameters(0, "@INTDIA", dia);
                //dbManager.AddParameters(1, "@GRADE", grade);
                //dbManager.AddParameters(2, "@PIN", pin);
                //ds = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "USP_VALIDATE_CABPIN_CUBE");
                if (ds.Tables.Count != 0)
                {
                    foreach (DataRowView drCABPin in ds.Tables[0].DefaultView)
                    {
                        count = Convert.ToInt32(drCABPin[0].ToString());
                        minLength = Convert.ToInt32(drCABPin[1].ToString());
                        minHookLen = Convert.ToInt32(drCABPin[2].ToString());
                        minHookHt = Convert.ToInt32(drCABPin[3].ToString());
                    }
                }
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message.ToString();
                minLength = 0;
                minHookLen = 0;
                minHookHt = 0;
                return false;
            }
            finally
            {

                ds.Dispose();
            }
        }

        /// <summary>
        /// Method to get the pin.
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="dia"></param>
        /// <param name="groupMarkId"></param>
        /// <param name="productTypeId"></param>
        /// <param name="errorMsg"></param>
        /// <param name="pin"></param>
        /// <param name="minLength"></param>
        public void GetPin(string grade, int dia, int groupMarkId, int productTypeId, out string errorMsg, out int pin, out int minLength, out int minHookLen, out int minHookHt)
        {
            DBManager dbManager = new DBManager();
            DataSet ds = new DataSet();
            try
            {
                errorMsg = "";
                pin = 0;
                minLength = 0;
                minHookLen = 0;
                minHookHt = 0;

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTDIA", dia);
                    dynamicParameters.Add("@GRADE", grade);
                    dynamicParameters.Add("@INTGROUPMARKID", groupMarkId);
                    dynamicParameters.Add("@INTPRODUCTTYPEID", productTypeId);

                     var dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = new SqlCommand(SystemConstant.USP_CABPIN_GET_CUBE, sqlConnection);
                    dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dataAdapter.SelectCommand.Parameters.AddRange(dynamicParameters.ParameterNames.Select(name => new SqlParameter(name, dynamicParameters.Get<object>(name))).ToArray());
                    dataAdapter.Fill(ds);
                    //ds = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.USP_CABPIN_GET_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);

                }
                //dbManager.Open();
                //dbManager.CreateParameters(4);
                //dbManager.AddParameters(0, "@INTDIA", dia);
                //dbManager.AddParameters(1, "@GRADE", grade);
                //dbManager.AddParameters(2, "@INTGROUPMARKID", groupMarkId);
                //dbManager.AddParameters(3, "@INTPRODUCTTYPEID", productTypeId);
                //ds = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "USP_CABPIN_GET_CUBE");
                if (ds.Tables.Count != 0)
                {
                    foreach (DataRowView drCABPin in ds.Tables[0].DefaultView)
                    {
                        pin = Convert.ToInt32(drCABPin[0].ToString());
                        minLength = Convert.ToInt32(drCABPin[1].ToString());
                        minHookLen = Convert.ToInt32(drCABPin[2].ToString());
                        minHookHt = Convert.ToInt32(drCABPin[3].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message.ToString();
                pin = 0;
                minLength = 0;
                minHookLen = 0;
                minHookHt = 0;
            }
            finally
            {

                ds.Dispose();
            }
        }

        /// <summary>
        /// Get the pin for straight bar and other shape codes
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="dia"></param>
        /// <param name="groupMarkId"></param>
        /// <param name="productTypeId"></param>
        /// <param name="shapeCode"></param>
        /// <param name="errorMsg"></param>
        /// <param name="pin"></param>
        /// <param name="minLength"></param>
        /// <param name="minHookLen"></param>
        /// <param name="minHookHt"></param>
        public void GetPin(string grade, int dia, int groupMarkId, int productTypeId, string shapeCode, out string errorMsg, out int pin, out int minLength, out int minHookLen, out int minHookHt)
        {
            DBManager dbManager = new DBManager();
            DataSet ds = new DataSet();
            try
            {
                errorMsg = "";
                pin = 0;
                minLength = 0;
                minHookLen = 0;
                minHookHt = 0;
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTDIA", dia);
                    dynamicParameters.Add("@GRADE", grade);
                    dynamicParameters.Add("@INTGROUPMARKID", groupMarkId);
                    dynamicParameters.Add("@INTPRODUCTTYPEID", productTypeId);
                    dynamicParameters.Add("@SHAPECODE", shapeCode.Trim());

                    var dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = new SqlCommand(SystemConstant.USP_GET_PIN_BYSHAPE_CUBE, sqlConnection);
                    dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dataAdapter.SelectCommand.Parameters.AddRange(dynamicParameters.ParameterNames.Select(name => new SqlParameter(name, dynamicParameters.Get<object>(name))).ToArray());
                    dataAdapter.Fill(ds);

                    //ds = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.USP_GET_PIN_BYSHAPE_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);

                }
                //dbManager.Open();
                //dbManager.CreateParameters(5);
                //dbManager.AddParameters(0, "@INTDIA", dia);
                //dbManager.AddParameters(1, "@GRADE", grade);
                //dbManager.AddParameters(2, "@INTGROUPMARKID", groupMarkId);
                //dbManager.AddParameters(3, "@INTPRODUCTTYPEID", productTypeId);
                //dbManager.AddParameters(4, "@SHAPECODE", shapeCode.Trim());
                //ds = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "USP_GET_PIN_BYSHAPE_CUBE");
                if (ds.Tables.Count != 0)
                {
                    foreach (DataRowView drCABPin in ds.Tables[0].DefaultView)
                    {
                        pin = Convert.ToInt32(drCABPin[0].ToString());
                        minLength = Convert.ToInt32(drCABPin[1].ToString());
                        minHookLen = Convert.ToInt32(drCABPin[2].ToString());
                        minHookHt = Convert.ToInt32(drCABPin[3].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message.ToString();
                pin = 0;
                minLength = 0;
                minHookLen = 0;
                minHookHt = 0;
            }
            finally
            {

                ds.Dispose();
            }
        }

        /// <summary>
        /// Method to get the BBS no from projectid.
        /// </summary>
        /// <param name="prijectId"></param>
        /// <param name="enteredText"></param>
        /// <returns></returns>
        public DataSet GetBBS(int prijectId, string enteredText)
        {
            DBManager dbManager = new DBManager();
            DataSet ds = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@PROJECTID", prijectId);
                    dynamicParameters.Add("@BBSNo", enteredText);

                    var dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = new SqlCommand(SystemConstant.GET_BBS_CUBE, sqlConnection);
                    dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dataAdapter.SelectCommand.Parameters.AddRange(dynamicParameters.ParameterNames.Select(name => new SqlParameter(name, dynamicParameters.Get<object>(name))).ToArray());
                    dataAdapter.Fill(ds);

                    //ds = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.GET_BBS_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);
                    return ds;
                }
                //dbManager.Open();
                //dbManager.CreateParameters(2);
                //dbManager.AddParameters(0, "@PROJECTID", prijectId);
                //dbManager.AddParameters(1, "@BBSNo", enteredText);
                //ds = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "GET_BBS_CUBE");
                //return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
            finally
            {

                ds.Dispose();
            }
        }

        /// <summary>
        /// Method to get the shape code image.
        /// </summary>
        /// <param name="shapeCode"></param>
        /// <returns></returns>

        //by vidya
        //public byte[] GetImage(string shapeCode)
        //{
        //    DBManager dbManager = new DBManager();
        //    try
        //    {
        //        dbManager.Open();
        //        byte[] ret = null;
        //        SqlDataReader rdr = null;
        //        rdr = (SqlDataReader)dbManager.ExecuteReader(CommandType.Text, "select CSM_SHAPE_IMAGE from T_CAB_SHAPE_MAST where CSM_SHAPE_ID='" + shapeCode + "'");
        //        while (rdr.Read())
        //        {
        //            ret = (byte[])rdr["CSM_SHAPE_IMAGE"];
        //        }
        //        return ret;

        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    finally
        //    {

        //    }
        //}


        /// <summary>
        /// Method to delete cab product marking data.
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public bool DeleteCabStructureMark(out string errorMessage)
        {
            bool isSuccess = false;
            DBManager dbManager = new DBManager();
            int Postedvalidate = 0;
            errorMessage = "";
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intcabproductmarkid", this.CABProductMarkID);
                    dynamicParameters.Add("@INTSEDETAILINGID", this.SEDetailingID);
                    dynamicParameters.Add("@BARMARK", this.CABProductMarkName);
                    //dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);

                    sqlConnection.Query<bool>(SystemConstant.USP_CABPRODUCTMARKINGDETAILS_DELETE_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);
                    
                    // Postedvalidate = dynamicParameters.Get<int>("@Output");

                    sqlConnection.Close();


                    //if (Postedvalidate == 0)
                    //{
                    //    errorMessage = "POSTED";
                    //}
                    //else if (Postedvalidate > 0)
                    //{
                    //    isSuccess = true;
                    //}
                    //else
                    //{
                    //    throw new Exception("Error in deleting cab product marking details");
                    //}
                    isSuccess = true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            return isSuccess;
        }

        /// <summary>
        /// Method to delete the CAB details saved by group mark id.
        /// </summary>
        /// <param name="groupMarkId"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public bool DeleteCabDetails(int groupMarkId, out string errorMessage)
        {
            bool isSuccess = false;
            DBManager dbManager = new DBManager();
            int count = 0;
            errorMessage = "";
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTGROUPMARKID", groupMarkId);

                    dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);

                    sqlConnection.Query<bool>(SystemConstant.USP_CABDETAILS_DELETE_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);
                    count = dynamicParameters.Get<int>("@Output");

                    sqlConnection.Close();
                    if (count > 0)
                    {
                        isSuccess = true;
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
            return isSuccess;
        }

        /// <summary>
        /// Method to delete cab product marking.
        /// </summary>
        /// <param name="CABProductMarkID"></param>
        /// <param name="seDetailingId"></param>
        /// <param name="barMark"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public bool DeleteCabStructureMark(int CABProductMarkID, int seDetailingId, string barMark, out string errorMessage)
        {
            bool isSuccess = false;
            DBManager dbManager = new DBManager();
            int Postedvalidate = 0;
            errorMessage = "";
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTCABPRODUCTMARKID", CABProductMarkID);
                    dynamicParameters.Add("@INTSEDETAILINGID", seDetailingId);
                    dynamicParameters.Add("@BARMARK", barMark);

                    dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);

                    sqlConnection.Query<bool>(SystemConstant.USP_CABPRODUCTMARKINGDETAILS_DELETE_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);
                    Postedvalidate = dynamicParameters.Get<int>("@Output");

                    sqlConnection.Close();
                    if (Postedvalidate == 0)
                    {
                        errorMessage = "POSTED";
                    }
                    else if (Postedvalidate > 0)
                    {
                        isSuccess = true;
                    }
                    else
                    {
                        throw new Exception("Error in deleting cab product marking details");
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
            return isSuccess;
        }

        /// <summary>
        /// Method to insert productmarking, accesories and shapeparameter details during variable length.
        /// </summary>
        /// <param name="objCabItem"></param>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="cabProdMarkId"></param>
        /// <param name="count"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public bool InsertVariableLength(CABItem objCabItem, List<ShapeParameter> source, List<ShapeParameter> target, int cabProdMarkId, int count, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                string barMark = objCabItem.CABProductMarkName.Trim();
                for (int i = 0; i < count; i++)
                {
                    //Calculate shape parameter values
                    if (i > 0)
                    {
                        //Calcullate the shape parameter values here as per logic.
                        foreach (ShapeParameter s1 in source)
                        {
                            foreach (ShapeParameter s2 in target)
                            {
                                if (s1.ParameterName == s2.ParameterName && s1.SequenceNumber == s2.SequenceNumber)
                                {
                                    if (!s1.ParameterValueCab.Equals(s2.ParameterValueCab))
                                    {
                                        s1.ParameterValueCab = (Convert.ToInt32(s1.ParameterValueCab) + Convert.ToInt32(Math.Round(Convert.ToDouble(Convert.ToInt32(s2.ParameterValueCab) - Convert.ToInt32(s1.ParameterValueCab)) / (count - i)))).ToString();
                                    }
                                }
                            }
                        }
                    }

                    //Calculate barmark
                    objCabItem.CABProductMarkName = barMark + "V" + (i + 1).ToString();
                    objCabItem.BarMark = objCabItem.CABProductMarkName;
                    objCabItem.ShapeParametersList = source;

                    //Get the envelope length, width and height.
                    int envl = 0;
                    int envw = 0;
                    int envH = 0;
                    EnvelopeCalculation(objCabItem.ShapeCode.Trim(), source, objCabItem.Diameter, out envl, out envw, out envH, out errorMessage);
                    objCabItem.EnvLength = envl;
                    objCabItem.EnvWidth = envw;
                    objCabItem.EnvHeight = envH;

                    //Calcullate production invoice length and weight.
                    int InvLength = 0;
                    int prodLength = 0;
                    int totBend = 0;
                    int totArc = 0;
                    string bvbs = "";
                    double prodWeight = 0;
                    double invWeight = 0;
                    if (objCabItem.ShapeParametersList.Count > 0)
                    {
                        objCabItem.accList = GetProdInvLength(objCabItem, out errorMessage, out InvLength, out prodLength, out totBend, out totArc, out bvbs);
                    }
                    objCabItem.InvoiceLength = InvLength;
                    objCabItem.ProductionLength = prodLength;
                    objCabItem.NoOfBends = totBend;
                    GetProdInvWeight(objCabItem.Diameter, objCabItem.Quantity, 1, Convert.ToInt32(objCabItem.InvoiceLength), Convert.ToInt32(objCabItem.ProductionLength), out prodWeight, out invWeight);
                    objCabItem.ProductionWeight = prodWeight;
                    objCabItem.InvoiceWeight = invWeight;
                    objCabItem.BVBS = bvbs;

                    //Set the coupler parameters
                    if (objCabItem.accList != null)
                    {
                        if (objCabItem.accList.Count == 0)
                        {
                            objCabItem.Coupler1Standard = "";
                            objCabItem.Coupler1Type = "";
                            objCabItem.Coupler1 = "";
                            objCabItem.Coupler2Type = "";
                            objCabItem.Coupler2 = "";
                            objCabItem.Coupler2Standard = "";
                        }
                        else if (objCabItem.accList.Count > 0)
                        {
                            //Get only the coupler materials from accessories list
                            List<Accessory> lstCoupler = (from item in objCabItem.accList
                                                          where item.MaterialType == "COUPLER_MATERIAL"
                                                          select item).ToList<Accessory>();

                            if (lstCoupler.Count == 1)
                            {
                                int counter = 0;
                                foreach (Accessory acc in lstCoupler)
                                {
                                    if (counter == 0)
                                    {
                                        objCabItem.Coupler1Standard = acc.standard.ToString();
                                        objCabItem.Coupler1Type = acc.CouplerType.ToString();
                                        objCabItem.Coupler1 = acc.SAPMaterialCode.ToString();
                                        objCabItem.Coupler2Type = "";
                                        objCabItem.Coupler2 = "";
                                        objCabItem.Coupler2Standard = "";
                                    }
                                    counter++;
                                }
                            }
                            else if (lstCoupler.Count == 2)
                            {
                                int counter = 0;
                                foreach (Accessory acc in lstCoupler)
                                {
                                    if (counter == 0)
                                    {
                                        objCabItem.Coupler1Standard = acc.standard.ToString();
                                        objCabItem.Coupler1Type = acc.CouplerType.ToString();
                                        objCabItem.Coupler1 = acc.SAPMaterialCode.ToString();
                                    }
                                    if (counter == 1)
                                    {
                                        objCabItem.Coupler2Type = acc.CouplerType.ToString();
                                        objCabItem.Coupler2 = acc.SAPMaterialCode.ToString();
                                        objCabItem.Coupler2Standard = acc.standard.ToString();
                                    }
                                    counter++;
                                }
                            }
                            //Clear the accessories list.
                            lstCoupler.Clear();
                        }
                    }
                    else
                    {
                        objCabItem.Coupler1Standard = "";
                        objCabItem.Coupler1Type = "";
                        objCabItem.Coupler1 = "";
                        objCabItem.Coupler2Type = "";
                        objCabItem.Coupler2 = "";
                        objCabItem.Coupler2Standard = "";
                    }
                    //Set the varies identifier
                    objCabItem.IsVariableBar = true;

                    //Call method to insert.
                    InsertUpdCABAccessories(objCabItem, out errorMessage);
                }
                bool flag = DeleteCabStructureMark(cabProdMarkId, objCabItem.intSEDetailingId, barMark, out errorMessage);
                return flag;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message.ToString();
                return false;
            }
            finally
            {
            }
        }

        /// <summary>
        /// Method to update records to database.
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="minLen"></param>
        /// <param name="minHookLen"></param>
        /// <param name="minHookHt"></param>
        /// <returns></returns>
        public bool Update(out string errorMessage, out int minLen, out int minHookLen, out int minHookHt)
        {
            DBManager dbManager = new DBManager();
            errorMessage = "";
            try
            {
                //Calcullate production invoice length and weight.
                int InvLength = 0;
                int prodLength = 0;
                int totBend = 0;
                int totArc = 0;
                string bvbs = "";
                minLen = 0;
                minHookLen = 0;
                minHookHt = 0;
                int envl = 0;
                int envw = 0;
                int envH = 0;
                int pin = 0;
                //Get the pin for the Dia and grade entered.
                bool flag = GetPin(this.Grade, this.Diameter, this.PinSizeID, out errorMessage, out minLen, out minHookLen, out minHookHt);
                if (this.PinSizeID != 0)
                {
                    if (flag == false)
                    {
                        errorMessage = "Please select a correct pin or contact ODOS administrator.";
                        return false;
                    }
                }
                else
                {
                    GetPin(this.Grade, this.Diameter, this.GroupMarkId, this.ProductCodeID, this.ShapeCode, out errorMessage, out pin, out minLen, out minHookLen, out minHookHt);
                    this.PinSizeID = pin;

                }
                if (this.ShapeParametersList.Count > 0)
                {
                    this.ShapeParametersList = ReValidateShapeParameters(this.ShapeParametersList, this.ShapeCode.Trim().ToUpper(), this.Diameter, this.Grade, this.PinSizeID, out errorMessage);
                    this.accList = GetProdInvLength(out errorMessage, out InvLength, out prodLength, out totBend, out totArc, out bvbs);
                }
                else
                {
                    this.accList = GetProdInvLength(out errorMessage, out InvLength, out prodLength, out totBend, out totArc, out bvbs, this.CABProductMarkID);
                }
                this.InvoiceLength = InvLength;
                this.ProductionLength = prodLength;
                this.NoOfBends = totBend;
                //Set the bvbs string
                if (bvbs != string.Empty && bvbs != "")
                {
                    this.BVBS = bvbs;
                }
                GetProdInvWeight(this.Diameter, this.Quantity, 1, Convert.ToInt32(this.InvoiceLength), Convert.ToInt32(this.ProductionLength));
                //Get the envelope lengths
                if (this.ShapeParametersList.Count > 0)
                {
                    EnvelopeCalculation(this.ShapeCode.Trim(), this.ShapeParametersList, this.Diameter, out envl, out envw, out envH, out errorMessage);
                }
                this.EnvLength = envl;
                this.EnvWidth = envw;
                this.EnvHeight = envH;
                //Set the coupler parameters
                if (this.accList != null)
                {
                    if (this.accList.Count == 0)
                    {
                        this.Coupler1Standard = "";
                        this.Coupler1Type = "";
                        this.Coupler1 = "";
                        this.Coupler2Type = "";
                        this.Coupler2 = "";
                        this.Coupler2Standard = "";
                    }
                    else if (this.accList.Count > 0)
                    {
                        //Get only the coupler materials from accessories list
                        List<Accessory> lstCoupler = (from item in this.accList
                                                      where item.MaterialType == "COUPLER_MATERIAL"
                                                      select item).ToList<Accessory>();

                        if (lstCoupler.Count == 1)
                        {
                            int count = 0;
                            foreach (Accessory acc in lstCoupler)
                            {
                                if (count == 0)
                                {
                                    this.Coupler1Standard = acc.standard.ToString();
                                    this.Coupler1Type = acc.CouplerType.ToString();
                                    this.Coupler1 = acc.SAPMaterialCode.ToString();
                                    this.Coupler2Type = "";
                                    this.Coupler2 = "";
                                    this.Coupler2Standard = "";
                                }
                                count++;
                            }
                        }
                        else if (lstCoupler.Count == 2)
                        {
                            int count = 0;
                            foreach (Accessory acc in lstCoupler)
                            {
                                if (count == 0)
                                {
                                    this.Coupler1Standard = acc.standard.ToString();
                                    this.Coupler1Type = acc.CouplerType.ToString();
                                    this.Coupler1 = acc.SAPMaterialCode.ToString();
                                }
                                if (count == 1)
                                {
                                    this.Coupler2Type = acc.CouplerType.ToString();
                                    this.Coupler2 = acc.SAPMaterialCode.ToString();
                                    this.Coupler2Standard = acc.standard.ToString();
                                }
                                count++;
                            }
                        }
                        //Clear the accessories list.
                        lstCoupler.Clear();
                    }
                }
                else
                {
                    this.Coupler1Standard = "";
                    this.Coupler1Type = "";
                    this.Coupler1 = "";
                    this.Coupler2Type = "";
                    this.Coupler2 = "";
                    this.Coupler2Standard = "";
                }
                string err = string.Empty;
                flag = ShapeDataValidationOnUpdate(out err);
                if (flag == true)
                {
                    //Update cabproduct marking details.
                    using (var sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        var dynamicParameters = new DynamicParameters();
                        dynamicParameters.Add("@INTCABPRODUCTMARKID", this.CABProductMarkID);
                        dynamicParameters.Add("@VCHCABPRODUCTMARKNAME", this.CABProductMarkName);
                        dynamicParameters.Add("@INTSEDETAILINGID", this.intSEDetailingId);
                        dynamicParameters.Add("@INTMEMBERQTY", this.Quantity);
                        dynamicParameters.Add("@INTSHAPECODE", this.ShapeCode.ToString().Trim());
                        dynamicParameters.Add("@INTPINSIZEID", this.PinSizeID);
                        dynamicParameters.Add("@NUMINVOICELENGTH", this.InvoiceLength);
                        dynamicParameters.Add("@NUMPRODUCTIONLENGTH", this.ProductionLength);
                        dynamicParameters.Add("@NUMINVOICEWEIGHT", this.InvoiceWeight);
                        dynamicParameters.Add("@NUMPRODUCTIONWEIGHT", this.ProductionWeight);
                        dynamicParameters.Add( "@GRADE", this.Grade);
                        dynamicParameters.Add( "@INTDIAMETER", this.Diameter);
                        dynamicParameters.Add( "@BARSTANDARD", this.Coupler1Standard);
                        dynamicParameters.Add( "@VCHSHAPETYPE", this.ShapeType);
                        dynamicParameters.Add( "@VCHSHAPEGROUP", this.ShapeGroup);
                        dynamicParameters.Add( "@COUPLERTYPE1", this.Coupler1Type);
                        dynamicParameters.Add( "@COUPLERMATERIAL1", this.Coupler1);
                        dynamicParameters.Add( "@COUPLERSTANDARD1", this.Coupler1Standard);
                        dynamicParameters.Add( "@COUPLERTYPE2", this.Coupler2Type);
                        dynamicParameters.Add( "@COUPLERMATERIAL2", this.Coupler2);
                        dynamicParameters.Add( "@COUPLERSTANDARD2", this.Coupler2Standard);
                        dynamicParameters.Add( "@INTSTATUS", this.Status);
                        dynamicParameters.Add( "@VCHCUSTREMARKS", this.DescScript);
                        dynamicParameters.Add( "@VCHSHAPEIMAGE", this.ShapeImage);
                        dynamicParameters.Add( "@VCHCAB_BVBS", this.BVBS);
                        dynamicParameters.Add( "@VCHPAGENUMBER", this.PageNumber);
                        dynamicParameters.Add( "@VCHCOMMDESCRIPT", this.DescScript);
                        dynamicParameters.Add( "@NUMENVLENGTH", this.EnvLength);
                        dynamicParameters.Add( "@NUMENVWIDTH", this.EnvWidth);
                        dynamicParameters.Add( "@NUMENVHEIGHT", this.EnvHeight);
                        dynamicParameters.Add( "@INTNOOFBENDS", this.NoOfBends);
                        dynamicParameters.Add( "@status", this.DDStatus);
                        dynamicParameters.Add( "@Activity", this.Activity);// Added for CAB Shape Parameter Validation CR   #For Activity Management
                        dynamicParameters.Add( "@UserID", this.UserID);// Added for CAB Shape Parameter Validation CR   #For Activity Management
                        
                        sqlConnection.Query<DataSet>(SystemConstant.USP_CABPRODUCTMARKING_UPDATE, dynamicParameters, commandType: CommandType.StoredProcedure);
                        
                    }
               
                    //Update SHAPEDETAILINGTRANS.
                    if (this.ShapeParametersList.Count > 0)
                    {
                        //Delete from shapetransheader and accesories table.
                        DeleteShapeDetailingTransOnUpdate(this.CABProductMarkID);
                        //Call method to insert into accessories.
                        InsertUpdAccessories(this.CABProductMarkID, this.intSEDetailingId);
                        //Save new parameters.
                        ShapeParametersList = ShapeParametersList.OrderBy(x => x.SequenceNumber).ToList<ShapeParameter>();
                        foreach (ShapeParameter shapeItem in ShapeParametersList)
                        {
                            shapeItem.Parameter = shapeItem.ParameterName;
                            shapeItem.Value = shapeItem.ParameterValueCab;
                            shapeItem.SequenceNumber = shapeItem.SequenceNumber;
                            if (shapeItem.VisibleFlag == true)
                            {
                                shapeItem.PrintValue = 1;
                            }
                            else
                            {
                                shapeItem.PrintValue = 0;
                            }
                            shapeItem.Save(this.ShapeTransHeaderID);
                        }
                    }
                }
                else
                {
                    errorMessage = "Problem with shape parameter validation";
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message.ToString();
                minLen = 0;
                minHookLen = 0;
                minHookHt = 0;
                throw ex;
            }
            finally
            {
            }
            return true;
        }

        /// <summary>
        /// Method to update records to database during variable length quantity change.
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="errorMessage"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        public bool UpdateVar(int UserId, out string errorMessage, int qty)
        {
            bool isSuccess = false;
            DBManager dbManager = new DBManager();
            errorMessage = "";
            try
            {
                //Calcullate production invoice length and weight.
                int InvLength = 0;
                int prodLength = 0;
                int totBend = 0;
                int totArc = 0;
                string bvbs = "";
                int envl = 0;
                int envw = 0;
                int envH = 0;
                if (this.ShapeParametersList.Count > 0)
                {
                    this.accList = GetProdInvLength(out errorMessage, out InvLength, out prodLength, out totBend, out totArc, out bvbs);
                }
                else
                {
                    this.accList = GetProdInvLength(out errorMessage, out InvLength, out prodLength, out totBend, out totArc, out bvbs, this.CABProductMarkID);
                }
                this.InvoiceLength = InvLength;
                this.ProductionLength = prodLength;
                this.NoOfBends = totBend;
                //Set the bvbs string
                if (bvbs != string.Empty && bvbs != "")
                {
                    this.BVBS = bvbs;
                }
                GetProdInvWeight(this.Diameter, qty, 1, Convert.ToInt32(this.InvoiceLength), Convert.ToInt32(this.ProductionLength));
                //Get the envelope lengths
                if (this.ShapeParametersList.Count > 0)
                {
                    EnvelopeCalculation(this.ShapeCode.Trim(), this.ShapeParametersList, this.Diameter, out envl, out envw, out envH, out errorMessage);
                }
                this.EnvLength = envl;
                this.EnvWidth = envw;
                this.EnvHeight = envH;
                //Set the coupler parameters
                if (this.accList != null)
                {
                    if (this.accList.Count == 0)
                    {
                        this.Coupler1Standard = "";
                        this.Coupler1Type = "";
                        this.Coupler1 = "";
                        this.Coupler2Type = "";
                        this.Coupler2 = "";
                        this.Coupler2Standard = "";
                    }
                    else if (this.accList.Count > 0)
                    {
                        //Get only the coupler materials from accessories list
                        List<Accessory> lstCoupler = (from item in this.accList
                                                      where item.MaterialType == "COUPLER_MATERIAL"
                                                      select item).ToList<Accessory>();

                        if (lstCoupler.Count == 1)
                        {
                            int count = 0;
                            foreach (Accessory acc in lstCoupler)
                            {
                                if (count == 0)
                                {
                                    this.Coupler1Standard = acc.standard.ToString();
                                    this.Coupler1Type = acc.CouplerType.ToString();
                                    this.Coupler1 = acc.SAPMaterialCode.ToString();
                                    this.Coupler2Type = "";
                                    this.Coupler2 = "";
                                    this.Coupler2Standard = "";
                                }
                                count++;
                            }
                        }
                        else if (lstCoupler.Count == 2)
                        {
                            int count = 0;
                            foreach (Accessory acc in lstCoupler)
                            {
                                if (count == 0)
                                {
                                    this.Coupler1Standard = acc.standard.ToString();
                                    this.Coupler1Type = acc.CouplerType.ToString();
                                    this.Coupler1 = acc.SAPMaterialCode.ToString();
                                }
                                if (count == 1)
                                {
                                    this.Coupler2Type = acc.CouplerType.ToString();
                                    this.Coupler2 = acc.SAPMaterialCode.ToString();
                                    this.Coupler2Standard = acc.standard.ToString();
                                }
                                count++;
                            }
                        }
                        //Clear the accessories list.
                        lstCoupler.Clear();
                    }
                }
                else
                {
                    this.Coupler1Standard = "";
                    this.Coupler1Type = "";
                    this.Coupler1 = "";
                    this.Coupler2Type = "";
                    this.Coupler2 = "";
                    this.Coupler2Standard = "";
                }
                //Update cabproduct marking details.                
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTCABPRODUCTMARKID", this.CABProductMarkID);
                    dynamicParameters.Add("@VCHCABPRODUCTMARKNAME", this.CABProductMarkName);
                    dynamicParameters.Add("@INTSEDETAILINGID", this.intSEDetailingId);
                    dynamicParameters.Add("@INTMEMBERQTY", qty);
                    dynamicParameters.Add("@INTSHAPECODE", this.ShapeCode.ToString().Trim());
                    dynamicParameters.Add("@INTPINSIZEID", this.PinSizeID);
                    dynamicParameters.Add("@NUMINVOICELENGTH", this.InvoiceLength);
                    dynamicParameters.Add("@NUMPRODUCTIONLENGTH", this.ProductionLength);
                    dynamicParameters.Add("@NUMINVOICEWEIGHT", this.InvoiceWeight);
                    dynamicParameters.Add("@NUMPRODUCTIONWEIGHT", this.ProductionWeight);
                    dynamicParameters.Add("@GRADE", this.Grade);
                    dynamicParameters.Add("@INTDIAMETER", this.Diameter);
                    dynamicParameters.Add("@BARSTANDARD", this.Coupler1Standard);
                    dynamicParameters.Add("@VCHSHAPETYPE", this.ShapeType);
                    dynamicParameters.Add("@VCHSHAPEGROUP", this.ShapeGroup);
                    dynamicParameters.Add("@COUPLERTYPE1", this.Coupler1Type);
                    dynamicParameters.Add("@COUPLERMATERIAL1", this.Coupler1);
                    dynamicParameters.Add("@COUPLERSTANDARD1", this.Coupler1Standard);
                    dynamicParameters.Add("@COUPLERTYPE2", this.Coupler2Type);
                    dynamicParameters.Add("@COUPLERMATERIAL2", this.Coupler2);
                    dynamicParameters.Add("@COUPLERSTANDARD2", this.Coupler2Standard);
                    dynamicParameters.Add("@INTSTATUS", this.ProduceIndicator.ToUpper() == "YES" ? "1" : "0");
                    dynamicParameters.Add("@VCHCUSTREMARKS", this.DescScript);
                    dynamicParameters.Add("@VCHSHAPEIMAGE", this.ShapeImage);
                    dynamicParameters.Add("@VCHCAB_BVBS", this.BVBS);
                    dynamicParameters.Add("@VCHPAGENUMBER", this.PageNumber);
                    dynamicParameters.Add("@VCHCOMMDESCRIPT", this.DescScript);
                    dynamicParameters.Add("@NUMENVLENGTH", this.EnvLength);
                    dynamicParameters.Add("@NUMENVWIDTH", this.EnvWidth);
                    dynamicParameters.Add("@NUMENVHEIGHT", this.EnvHeight);
                    dynamicParameters.Add("@INTNOOFBENDS", this.NoOfBends);

                    sqlConnection.Query<DataSet>(SystemConstant.USP_PRODUCTMARK_VAR_UPDATE, dynamicParameters, commandType: CommandType.StoredProcedure);

                }
                
               
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message.ToString();
                throw ex;
            }
            finally
            {

            }
            return isSuccess;
        }

        /// <summary>
        /// Method to delete data from ShapeTransHeader table on update.
        /// </summary>
        /// <param name="ShapeTransHeaderID"></param>
        public void DeleteShapeDetailingTransOnUpdate(int productMarkId)
        {
            DBManager dbManager = new DBManager();
            DataSet dsShapeTransHeaderID = new DataSet();
            try
            {
                //dbManager = new DBManager();
                //dbManager.Open();
                //dbManager.CreateParameters(1);
                //dbManager.AddParameters(0, "@INTCABPRODUCTMARKID", productMarkId);
                //dsShapeTransHeaderID = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "USP_CABSHAPEPARAMETER_DELETE");

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTCABPRODUCTMARKID", productMarkId);
                    var dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = new SqlCommand(SystemConstant.USP_CABSHAPEPARAMETER_DELETE, sqlConnection);
                    dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dataAdapter.SelectCommand.Parameters.AddRange(dynamicParameters.ParameterNames.Select(name => new SqlParameter(name, dynamicParameters.Get<object>(name))).ToArray());
                    dataAdapter.Fill(dsShapeTransHeaderID);
                    //dsShapeTransHeaderID = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.USP_CABSHAPEPARAMETER_DELETE, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (dsShapeTransHeaderID.Tables.Count != 0)
                    {
                        foreach (DataRowView drShapeTransHeaderID in dsShapeTransHeaderID.Tables[0].DefaultView)
                        {
                            this.ShapeTransHeaderID = Convert.ToInt32(drShapeTransHeaderID[0].ToString());
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                dsShapeTransHeaderID.Dispose();
            }
        }

        /// <summary>
        /// Method to get production and invoice length during variable length calcullation.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="errorMsg"></param>
        /// <param name="invoiceLength"></param>
        /// <param name="prodLength"></param>
        /// <param name="r_intTotBend"></param>
        /// <param name="r_intTotArc"></param>
        /// <returns></returns>
        public List<Accessory> GetProdInvLength(CABItem obj, out string errorMsg, out int invoiceLength, out int prodLength, out int r_intTotBend, out int r_intTotArc, out string bvbs)
        {
            errorMsg = "";
            invoiceLength = 0;
            prodLength = 0;
            r_intTotBend = 0;
            r_intTotArc = 0;

            CABCalculation objCabValidation = new CABCalculation();
            List<Accessory> accList = new List<Accessory>();
            try
            {
                objCabValidation.ShapeId = obj.Shape.ShapeID.ToString();
                objCabValidation.ShapeCode = obj.ShapeCode;
                objCabValidation.dia = obj.Diameter.ToString();
                objCabValidation.pin = obj.PinSizeID.ToString();

                objCabValidation.ShapeParametersList = obj.ShapeParametersList;

                accList = objCabValidation.ProductionInvLength(out errorMsg, out invoiceLength, out prodLength, out r_intTotBend, out r_intTotArc, out bvbs);
                //Round up the values to 5.
                if (prodLength != 0)
                {
                    if (prodLength % 5 != 0)
                    {
                        prodLength = prodLength + 5 - (prodLength % 5);
                    }
                }
                if (invoiceLength != 0)
                {
                    if (invoiceLength % 5 != 0)
                    {
                        invoiceLength = invoiceLength + 5 - (invoiceLength % 5);
                    }
                }
                return accList;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message.ToString();
                invoiceLength = 0;
                prodLength = 0;
                r_intTotBend = 0;
                r_intTotArc = 0;
                bvbs = "";
                return null;
            }
            finally
            {
                objCabValidation = null;
            }
        }

        /// <summary>
        /// Method to get production and invoice length.
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <param name="invoiceLength"></param>
        /// <param name="prodLength"></param>
        /// <param name="r_intTotBend"></param>
        /// <param name="r_intTotArc"></param>
        /// <returns></returns>
        public List<Accessory> GetProdInvLength(out string errorMsg, out int invoiceLength, out int prodLength, out int r_intTotBend, out int r_intTotArc, out string bvbs)
        {
            CABCalculation objCabValidation = new CABCalculation();
            List<Accessory> accList = new List<Accessory>();
            errorMsg = "";
            invoiceLength = 0;
            prodLength = 0;
            r_intTotBend = 0;
            r_intTotArc = 0;
            bvbs = "";
            try
            {
                objCabValidation.ShapeId = this.Shape.ShapeID.ToString();
                objCabValidation.ShapeCode = this.ShapeCode;
                objCabValidation.dia = this.Diameter.ToString();
                objCabValidation.pin = this.PinSizeID.ToString();
                objCabValidation.ShapeParametersList = new List<ShapeParameter>();
               objCabValidation.ShapeParametersList = this.ShapeParametersList;

                accList = objCabValidation.ProductionInvLength(out errorMsg, out invoiceLength, out prodLength, out r_intTotBend, out r_intTotArc, out bvbs);
                //Round up the values to 5.
                if (prodLength != 0)
                {
                    if (prodLength % 5 != 0)
                    {
                        prodLength = prodLength + 5 - (prodLength % 5);
                    }
                }
                if (invoiceLength != 0)
                {
                    if (invoiceLength % 5 != 0)
                    {
                        invoiceLength = invoiceLength + 5 - (invoiceLength % 5);
                    }
                }
                return accList;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message.ToString();
                invoiceLength = 0;
                prodLength = 0;
                r_intTotBend = 0;
                r_intTotArc = 0;
                bvbs = "";
                return null;
            }
            finally
            {
                objCabValidation = null;
            }
        }

        /// <summary>
        /// Method to get production and invoice length.
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <param name="invoiceLength"></param>
        /// <param name="prodLength"></param>
        /// <param name="r_intTotBend"></param>
        /// <param name="r_intTotArc"></param>
        /// <param name="productMarkId"></param>
        /// <returns></returns>
        public List<Accessory> GetProdInvLength(out string errorMsg, out int invoiceLength, out int prodLength, out int r_intTotBend, out int r_intTotArc, out string bvbs, int productMarkId)
        {
            CABCalculation objCabValidation = new CABCalculation();
            List<Accessory> accList = new List<Accessory>();
            errorMsg = "";
            invoiceLength = 0;
            prodLength = 0;
            r_intTotBend = 0;
            r_intTotArc = 0;
            bvbs = "";
            try
            {
                objCabValidation.ShapeId = this.Shape.ShapeID.ToString();
                objCabValidation.ShapeCode = this.ShapeCode;
                objCabValidation.dia = this.Diameter.ToString();
                objCabValidation.pin = this.PinSizeID.ToString();
                //objCabValidation.ShapeParametersList = this.Shape.ShapeParam[0];

                List<ShapeParameter> lst = new List<ShapeParameter>();
                DBManager dbManager = new DBManager();
                DataSet dsShapeParameterForCab = new DataSet();

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTCABPRODUCTMARKID", productMarkId);
                    var dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = new SqlCommand(SystemConstant.USP_SHAPEPARAMETERFORCAB_EDIT_CUBE, sqlConnection);
                    dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dataAdapter.SelectCommand.Parameters.AddRange(dynamicParameters.ParameterNames.Select(name => new SqlParameter(name, dynamicParameters.Get<object>(name))).ToArray());
                    dataAdapter.Fill(dsShapeParameterForCab);
                    //dsShapeParameterForCab = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.USP_SHAPEPARAMETERFORCAB_EDIT_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);

                }
              
                if (dsShapeParameterForCab != null && dsShapeParameterForCab.Tables.Count != 0)
                {
                    foreach (DataRowView drvBeam in dsShapeParameterForCab.Tables[0].DefaultView)
                    {
                        ShapeCode objShapeCode = new ShapeCode();
                        objShapeCode.ShapeID = Convert.ToInt32(drvBeam["INTSHAPEID"]);
                        ShapeParameter shapeparam = new ShapeParameter
                        {
                            ShapeId = Convert.ToInt32(drvBeam["INTSHAPEID"]),
                            ParameterName = Convert.ToString(drvBeam["CHRPARAMNAME"]),
                            CriticalIndiacator = Convert.ToString(drvBeam["CHRCRITICALIND"]),
                            SequenceNumber = Convert.ToInt16(drvBeam["INTPARAMSEQ"]),
                            ParameterValueCab = Convert.ToString(drvBeam["INTSEGMENTVALUE"]),
                            ShapeCodeImage = Convert.ToString(drvBeam["CHRSHAPECODE"]),
                            EditFlag = Convert.ToBoolean(drvBeam["BITEDIT"]),
                            VisibleFlag = Convert.ToBoolean(drvBeam["BITVISIBLE"]),
                            WireType = Convert.ToString(drvBeam["CHRWIRETYPE"]),
                            AngleType = Convert.ToString(drvBeam["CHRANGLETYPE"]),
                            AngleDir = Convert.ToInt32(drvBeam["INTANGLEDIR"]),
                            symmetricIndex = Convert.ToString(drvBeam["SYMMETRYINDEX"]),
                            HeghtAngleFormula = Convert.ToString(drvBeam["HEIGHTANGLEFORMULA"]),
                            HeghtSuceedAngleFormula = Convert.ToString(drvBeam["HEIGHTSUCEEDANGLEFORMULA"]),
                            IntXCord = Convert.ToInt32(drvBeam["INTXCOORD"]),
                            IntYCord = Convert.ToInt32(drvBeam["INTYCOORD"]),
                            IntZCord = Convert.ToInt32(drvBeam["INTZCOORD"]),
                            CustFormula = Convert.ToString(drvBeam["VCHCUSTOMFORMULA"]),
                            OffsetAngleFormula = Convert.ToString(drvBeam["VCHOFFSETANGLEFORMULA"]),
                            OffsetSuceedAngleFormula = Convert.ToString(drvBeam["OFFSETSUCEEDANGLEFORMULA"]),
                            CouplerType = Convert.ToString(drvBeam["CSD_INPUT_TYPE"]),
                            ParameterView = Convert.ToString(drvBeam["CSD_VIEW"])
                        };
                        lst.Add(shapeparam);
                    }
                }
                this.ShapeParametersList = lst;
                objCabValidation.ShapeParametersList = lst;

                //Revalidate the shape parameter values
                string errorMessage = "";
                objCabValidation.ShapeParametersList = ReValidateShapeParameters(objCabValidation.ShapeParametersList, objCabValidation.ShapeCode.Trim().ToUpper(), this.Diameter, this.Grade, this.PinSizeID, out errorMessage);

                accList = objCabValidation.ProductionInvLength(out errorMsg, out invoiceLength, out prodLength, out r_intTotBend, out r_intTotArc, out bvbs);
                //Round up the values to 5.
                if (prodLength != 0)
                {
                    if (prodLength % 5 != 0)
                    {
                        prodLength = prodLength + 5 - (prodLength % 5);
                    }
                }
                if (invoiceLength != 0)
                {
                    if (invoiceLength % 5 != 0)
                    {
                        invoiceLength = invoiceLength + 5 - (invoiceLength % 5);
                    }
                }
                return accList;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message.ToString();
                invoiceLength = 0;
                prodLength = 0;
                r_intTotBend = 0;
                r_intTotArc = 0;
                bvbs = "";
                return null;
            }
            finally
            {
                objCabValidation = null;
            }
        }

        /// <summary>
        /// Method to get the invoice and production weight.
        /// </summary>
        /// <param name="dia"></param>
        /// <param name="quantity"></param>
        /// <param name="unitquantity"></param>
        /// <param name="invoiceLength"></param>
        /// <param name="prodLength"></param>
        public void GetProdInvWeight(int dia, int quantity, int unitquantity, int invoiceLength, int prodLength)
        {
            try
            {
                double linearWt = 0;
                ////Get the coupler related info in list object
                //string resourceName = "D:\\PROJECT_FOLDER\\Detailing - NDS\\One Detaling and Order System\\ODOS_API\\Microservices\\DetailingService\\Repositories\\LinearWeight.xml";
                //Assembly assembly = Assembly.GetExecutingAssembly();
                //Stream stream = assembly.GetManifestResourceStream(resourceName);
                XDocument doc1 = XDocument.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LinearWeight.xml"));
                List<LinearWeight> lstLinearWeight = (from l in doc1.Descendants("LinearWeight")
                                                      select new LinearWeight
                                                      {
                                                          Diameter = l.Element("DIAMETER").Value,
                                                          Linear_Weight = l.Element("LINEAR_WEIGHT").Value,
                                                      }).ToList<LinearWeight>();

                //Get the coupler master values by the coupler type selected
                var linearWeightVar = (from l in lstLinearWeight where l.Diameter.Trim().Equals(dia.ToString()) select l).ToList();
                if (linearWeightVar.Count > 0)
                {
                    linearWt = Convert.ToDouble(linearWeightVar[0].Linear_Weight);
                }

                if (quantity.ToString() != "" && quantity.ToString() != string.Empty && quantity.ToString().Trim() != null && unitquantity.ToString() != "" && unitquantity.ToString() != string.Empty && unitquantity.ToString().Trim() != null)
                {
                    double dblInvWt = linearWt * invoiceLength * unitquantity * Convert.ToDouble(quantity) * Math.Pow(10.0, -3.0);
                    dblInvWt = Math.Round(dblInvWt, 3);

                    double dblProdWt = linearWt * prodLength * unitquantity * Convert.ToDouble(quantity) * Math.Pow(10.0, -3.0);
                    dblProdWt = Math.Round(dblProdWt, 3);

                    this.ProductionWeight = dblProdWt;
                    this.InvoiceWeight = dblInvWt;
                }
                lstLinearWeight.Clear();
                doc1 = null;
                //stream.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to get prod and invoice weight for variable length function.
        /// </summary>
        /// <param name="dia"></param>
        /// <param name="quantity"></param>
        /// <param name="unitquantity"></param>
        /// <param name="invoiceLength"></param>
        /// <param name="prodLength"></param>
        /// <param name="ProductionWeight"></param>
        /// <param name="InvoiceWeight"></param>
        public void GetProdInvWeight(int dia, int quantity, int unitquantity, int invoiceLength, int prodLength, out double ProductionWeight, out double InvoiceWeight)
        {
            ProductionWeight = 0;
            InvoiceWeight = 0;
            try
            {
                double linearWt = 0;
                //Get the coupler related info in list object
                var assembly = Assembly.GetExecutingAssembly();
                Stream stream = assembly.GetManifestResourceStream(this.GetType(), "LinearWeight.xml");
                XDocument doc1 = XDocument.Load(stream);
                List<LinearWeight> lstLinearWeight = (from l in doc1.Descendants("LinearWeight")
                                                      select new LinearWeight
                                                      {
                                                          Diameter = l.Element("DIAMETER").Value,
                                                          Linear_Weight = l.Element("LINEAR_WEIGHT").Value,
                                                      }).ToList<LinearWeight>();

                //Get the coupler master values by the coupler type selected
                var linearWeightVar = (from l in lstLinearWeight where l.Diameter.Trim().Equals(dia.ToString()) select l).ToList();
                if (linearWeightVar.Count > 0)
                {
                    linearWt = Convert.ToDouble(linearWeightVar[0].Linear_Weight);
                }

                if (quantity.ToString() != "" && quantity.ToString() != string.Empty && quantity.ToString().Trim() != null && unitquantity.ToString() != "" && unitquantity.ToString() != string.Empty && unitquantity.ToString().Trim() != null)
                {
                    double dblInvWt = linearWt * invoiceLength * unitquantity * Convert.ToDouble(quantity) * Math.Pow(10.0, -3.0);
                    dblInvWt = Math.Round(dblInvWt, 3);

                    double dblProdWt = linearWt * prodLength * unitquantity * Convert.ToDouble(quantity) * Math.Pow(10.0, -3.0);
                    dblProdWt = Math.Round(dblProdWt, 3);

                    ProductionWeight = dblProdWt;
                    InvoiceWeight = dblInvWt;
                }
                lstLinearWeight.Clear();
                doc1 = null;
                stream.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To Insert & Update CAB Product Details.
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int InsertUpdCABAccessories(out string errMsg)
        {
            DBManager dbManager = new DBManager();
            string strCABProductMarkID = string.Empty;
            DataSet dsCABProductMarkID = new DataSet();
            int intCABProductMarkID = 0;
            int ShapeTransHeaderId = 0;
            string err = string.Empty;
            try
            {
                bool flag = ValidateCabDetailVals(out err);
                if (flag == true)
                {
                    //Get the envelope length, width and height.
                    int envl = 0;
                    int envw = 0;
                    int envH = 0;
                    EnvelopeCalculation(this.ShapeCode, this.ShapeParametersList, this.Diameter, out envl, out envw, out envH, out errMsg);
                    this.EnvLength = envl;
                    this.EnvWidth = envw;
                    this.EnvHeight = envH;
                    //Handle varies bar
                    if (this.IsVariableBar == null)
                    {
                        this.IsVariableBar = false;
                    }
                    //Insert the product mark data to database.
                    using (var sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        var dynamicParameters = new DynamicParameters();
                        dynamicParameters.Add("@VCHCABPRODUCTMARKNAME", this.CABProductMarkName.Trim());
                        dynamicParameters.Add("@INTSEDETAILINGID", this.intSEDetailingId); //by CS05
                        dynamicParameters.Add("@INTGROUPMARKID", this.GroupMarkId);
                        dynamicParameters.Add("@INTMEMBERQTY", this.Quantity);
                        dynamicParameters.Add("@INTSHAPECODE", this.ShapeCode.Trim());
                        dynamicParameters.Add("@INTPINSIZEID", this.PinSizeID);
                        dynamicParameters.Add("@NUMINVOICELENGTH", this.InvoiceLength);
                        dynamicParameters.Add("@NUMPRODUCTIONLENGTH", this.ProductionLength);
                        dynamicParameters.Add("@NUMINVOICEWEIGHT", this.InvoiceWeight);
                        dynamicParameters.Add("@NUMPRODUCTIONWEIGHT", this.ProductionWeight);
                        dynamicParameters.Add("@GRADE", this.Grade);
                        dynamicParameters.Add("@INTDIAMETER", this.Diameter);
                        dynamicParameters.Add("@VCHSHAPETYPE", this.ShapeType);
                        dynamicParameters.Add("@VCHSHAPEGROUP", this.ShapeGroup);
                        dynamicParameters.Add("@INTSTATUS", this.Status);
                        dynamicParameters.Add("@VCHCUSTREMARKS", this.CustomerRemarks);
                        dynamicParameters.Add("@VCHSHAPEIMAGE", this.ShapeImage);
                        dynamicParameters.Add("@VCHCAB_BVBS", this.BVBS);
                        dynamicParameters.Add("@VCHPAGENUMBER", this.PageNumber);
                        dynamicParameters.Add("@VCHCOMMDESCRIPT", this.CommercialDesc);

                        dynamicParameters.Add("@NUMENVLENGTH", this.EnvLength);
                        dynamicParameters.Add("@NUMENVWIDTH", this.EnvWidth);
                        dynamicParameters.Add("@NUMENVHEIGHT", this.EnvHeight);
                        dynamicParameters.Add("@INTNOOFBENDS", this.NoOfBends);
                       
                        if (accList.Count == 0)
                        {
                            dynamicParameters.Add("@BARSTANDARD", "");
                            dynamicParameters.Add("@COUPLERTYPE1", "");
                            dynamicParameters.Add("@COUPLERMATERIAL1", "");
                            dynamicParameters.Add("@COUPLERSTANDARD1", "");
                            dynamicParameters.Add("@COUPLERTYPE2", "");
                            dynamicParameters.Add("@COUPLERMATERIAL2", "");
                            dynamicParameters.Add("@COUPLERSTANDARD2", "");
                        }


                        else
                        {
                            dynamicParameters.Add("@BARSTANDARD", this.Coupler1Standard);
                            dynamicParameters.Add("@COUPLERTYPE1", this.Coupler1Type);
                            dynamicParameters.Add("@COUPLERMATERIAL1", this.Coupler1);
                            dynamicParameters.Add("@COUPLERSTANDARD1", this.Coupler1Standard);
                            dynamicParameters.Add("@COUPLERTYPE2", this.Coupler2Type);
                            dynamicParameters.Add("@COUPLERMATERIAL2", this.Coupler2);
                            dynamicParameters.Add("@COUPLERSTANDARD2", this.Coupler2Standard);
                        }
                      
                        dynamicParameters.Add("@BITVARIESBAR", this.IsVariableBar.ToString());
                        dynamicParameters.Add("@status", this.DDStatus); // Added for CAB Shape Parameter Validation CR   #Change
                        dynamicParameters.Add("@Activity", this.Activity);// Added for CAB Shape Parameter Validation CR   #For Activity Management
                        dynamicParameters.Add("@UserID", this.UserID);// Added for CAB Shape Parameter Validation CR   #For Activity Management

                        //dsCABProductMarkID = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "USP_CAB_PRODUCTMARKINGDETAILS_INSUPD_CUBE");
                        var dataAdapter = new SqlDataAdapter();
                        dataAdapter.SelectCommand = new SqlCommand(SystemConstant.USP_CAB_PRODUCTMARKINGDETAILS_INSUPD_CUBE, sqlConnection);
                        dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        dataAdapter.SelectCommand.Parameters.AddRange(dynamicParameters.ParameterNames.Select(name => new SqlParameter(name, dynamicParameters.Get<object>(name))).ToArray());
                        dataAdapter.Fill(dsCABProductMarkID);

                        //KdsCABProductMarkID = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.USP_CAB_PRODUCTMARKINGDETAILS_INSUPD_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);

                    }


                    if (dsCABProductMarkID.Tables.Count != 0)
                    {
                        foreach (DataRowView drCABProductMarkID in dsCABProductMarkID.Tables[0].DefaultView)
                        {
                            intCABProductMarkID = Convert.ToInt32(drCABProductMarkID[0].ToString());
                            ShapeTransHeaderId = Convert.ToInt32(drCABProductMarkID[1].ToString());
                        }
                    }
                    if (intCABProductMarkID != 0)
                    {
                        InsertUpdAccessories(intCABProductMarkID, this.intSEDetailingId);
                        ShapeParametersList = ShapeParametersList.OrderBy(x => x.SequenceNumber).ToList<ShapeParameter>();
                        foreach (ShapeParameter shapeItem in ShapeParametersList)
                        {
                            shapeItem.Parameter = shapeItem.ParameterName;
                            shapeItem.Value = shapeItem.ParameterValueCab;
                            shapeItem.SequenceNumber = shapeItem.SequenceNumber;
                            if (shapeItem.VisibleFlag == true)
                            {
                                shapeItem.PrintValue = 1;
                            }
                            else
                            {
                                shapeItem.PrintValue = 0;
                            }
                            shapeItem.Save(ShapeTransHeaderId);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message.ToString();
                throw ex;
            }
            finally
            {
                errMsg = err;
            }
            return intCABProductMarkID;
        }

        /// <summary>
        /// Insert productmarking and accesorries and shape parameter details during variable length calcullation.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int InsertUpdCABAccessories(CABItem obj, out string errMsg)
        {
            DBManager dbManager = new DBManager();
            string strCABProductMarkID = string.Empty;
            DataSet dsCABProductMarkID = new DataSet();
            int intCABProductMarkID = 0;
            int ShapeTransHeaderId = 0;
            string err = string.Empty;
            try
            {
                //Handle varies bar
                if (obj.IsVariableBar == null)
                {
                    obj.IsVariableBar = false;
                }

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@VCHCABPRODUCTMARKNAME", obj.CABProductMarkName.Trim());
                    dynamicParameters.Add("@INTSEDETAILINGID", obj.intSEDetailingId);
    
                    dynamicParameters.Add("@INTGROUPMARKID", obj.GroupMarkId);
    
                    dynamicParameters.Add("@INTMEMBERQTY", obj.Quantity);
    
                    dynamicParameters.Add("@INTSHAPECODE", obj.ShapeCode.Trim());
    
                    dynamicParameters.Add("@INTPINSIZEID", obj.PinSizeID);
    
                    dynamicParameters.Add("@NUMINVOICELENGTH", obj.InvoiceLength);
    
                    dynamicParameters.Add("@NUMPRODUCTIONLENGTH", obj.ProductionLength);
    
                    dynamicParameters.Add("@NUMINVOICEWEIGHT", obj.InvoiceWeight);
    
                    dynamicParameters.Add("@NUMPRODUCTIONWEIGHT", obj.ProductionWeight);
    
                    dynamicParameters.Add("@GRADE", obj.Grade);
                    dynamicParameters.Add("@INTDIAMETER", obj.Diameter);
                    dynamicParameters.Add("@VCHSHAPETYPE", obj.ShapeType);
                    dynamicParameters.Add("@VCHSHAPEGROUP", obj.ShapeGroup);
                    dynamicParameters.Add("@INTSTATUS", obj.Status);
                    dynamicParameters.Add("@VCHCUSTREMARKS", obj.CustomerRemarks);
                    dynamicParameters.Add("@VCHSHAPEIMAGE", obj.ShapeImage);
                    dynamicParameters.Add("@VCHCAB_BVBS", obj.BVBS);
                    dynamicParameters.Add("@VCHPAGENUMBER", obj.PageNumber);
                    dynamicParameters.Add("@VCHCOMMDESCRIPT", obj.CommercialDesc);
                    dynamicParameters.Add("@NUMENVLENGTH", obj.EnvLength);
                    dynamicParameters.Add("@NUMENVWIDTH", obj.EnvWidth);
                    dynamicParameters.Add("@NUMENVHEIGHT", obj.EnvHeight);
                    dynamicParameters.Add("@INTNOOFBENDS", obj.NoOfBends);

                    if (obj.accList.Count == 0)
                    {
                        dynamicParameters.Add("@BARSTANDARD", "");
                        dynamicParameters.Add("@COUPLERTYPE1", "");
                        dynamicParameters.Add("@COUPLERMATERIAL1", "");
                        dynamicParameters.Add("@COUPLERSTANDARD1", "");
                        dynamicParameters.Add("@COUPLERTYPE2", "");
                        dynamicParameters.Add("@COUPLERMATERIAL2", "");
                        dynamicParameters.Add("@COUPLERSTANDARD2", "");
                    }
                    else
                    {
                        dynamicParameters.Add("@BARSTANDARD", obj.Coupler1Standard);
                        dynamicParameters.Add("@COUPLERTYPE1", obj.Coupler1Type);
                        dynamicParameters.Add("@COUPLERMATERIAL1", obj.Coupler1);
                        dynamicParameters.Add("@COUPLERSTANDARD1", obj.Coupler1Standard);
                        dynamicParameters.Add("@COUPLERTYPE2", obj.Coupler2Type);
                        dynamicParameters.Add("@COUPLERMATERIAL2", obj.Coupler2);
                        dynamicParameters.Add("@COUPLERSTANDARD2", obj.Coupler2Standard);
                    }
                    dynamicParameters.Add("@BITVARIESBAR", obj.IsVariableBar.ToString());
                    //dsCABProductMarkID = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "USP_CAB_PRODUCTMARKINGDETAILS_INSUPD_CUBE");

                    var dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = new SqlCommand(SystemConstant.USP_CAB_PRODUCTMARKINGDETAILS_INSUPD_CUBE, sqlConnection);
                    dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dataAdapter.SelectCommand.Parameters.AddRange(dynamicParameters.ParameterNames.Select(name => new SqlParameter(name, dynamicParameters.Get<object>(name))).ToArray());
                    dataAdapter.Fill(dsCABProductMarkID);


                    //dsCABProductMarkID = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.USP_CAB_PRODUCTMARKINGDETAILS_INSUPD_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);

                }
                if (dsCABProductMarkID.Tables.Count != 0)
                {
                    foreach (DataRowView drCABProductMarkID in dsCABProductMarkID.Tables[0].DefaultView)
                    {
                        intCABProductMarkID = Convert.ToInt32(drCABProductMarkID[0].ToString());
                        ShapeTransHeaderId = Convert.ToInt32(drCABProductMarkID[1].ToString());
                    }
                }
                if (intCABProductMarkID != 0)
                {
                    InsertUpdAccessories(obj, intCABProductMarkID, obj.intSEDetailingId);
                    obj.ShapeParametersList = obj.ShapeParametersList.OrderBy(x => x.SequenceNumber).ToList<ShapeParameter>();
                    foreach (ShapeParameter shapeItem in obj.ShapeParametersList)
                    {
                        shapeItem.Parameter = shapeItem.ParameterName;
                        shapeItem.Value = shapeItem.ParameterValueCab;
                        shapeItem.SequenceNumber = shapeItem.SequenceNumber;
                        if (shapeItem.VisibleFlag == true)
                        {
                            shapeItem.PrintValue = 1;
                        }
                        else
                        {
                            shapeItem.PrintValue = 0;
                        }
                        shapeItem.Save(ShapeTransHeaderId);
                    }
                }

            }
            catch (Exception ex)
            {
                errMsg = ex.Message.ToString();
                throw ex;
            }
            finally
            {
                errMsg = err;
                dsCABProductMarkID.Dispose();
            }
            return intCABProductMarkID;
        }

        /// <summary>
        /// Method to insert fresh bar mark in a perticular sequence.
        /// </summary>
        /// <param name="barMarkStart"></param>
        /// <param name="barMarkEnd"></param>
        /// <param name="groupMarkId"></param>
        /// <param name="seDetailingID"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool InsertProductMarkInLine(string barMarkStart, string barMarkEnd, int groupMarkId, int seDetailingID, out string errorMsg)
        {
            bool isSuccess = false;
            DBManager dbManager = new DBManager();
            errorMsg = "";
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@VCHBARMARKSTART", barMarkStart.ToString());
                    dynamicParameters.Add("@VCHBARMARKEXCLUDE", barMarkEnd.ToString());
                    dynamicParameters.Add("@GROUPMARKID", groupMarkId.ToString());
                    dynamicParameters.Add("@SELEVELID", seDetailingID.ToString());
                    sqlConnection.Query<DataSet>(SystemConstant.SP_CABPRODUCTMARKINGDETAILS_INSERT_INLINE, dynamicParameters, commandType: CommandType.StoredProcedure);

                    return true;
                }
               
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
            finally
            {

            }
        }

        /// <summary>
        /// Supporting method for validation.
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool ValidateCabDetailVals(out string errorMsg)
        {
            bool inputShapevalid = false;
            CABValidation objCabValidation = new CABValidation();
            try
            {
                objCabValidation.intSEDetailingId = this.intSEDetailingId;
                objCabValidation.ProduceInd = this.Status;
                objCabValidation.Prefix = this.CommercialDesc;
                objCabValidation.PageNo = this.PageNumber;
                objCabValidation.BarMarkId = this.BarMark;
                objCabValidation.ShapeId = this.Shape.ShapeID.ToString();
                objCabValidation.ShapeCode = this.ShapeCode;
                objCabValidation.Grade = this.Grade;
                objCabValidation.Dia = this.Diameter.ToString();
                objCabValidation.Quantity = this.Quantity.ToString();
                objCabValidation.UnitQuantity = this.Quantity.ToString();
                objCabValidation.Pin = this.PinSizeID.ToString();
                objCabValidation.InvoiceLength = this.InvoiceLength.ToString();
                objCabValidation.ProdnLength = this.ProductionLength.ToString();
                objCabValidation.InvoiceWeight = this.InvoiceWeight.ToString();
                objCabValidation.ProdnWeight = this.ProductionWeight.ToString();
                objCabValidation.OperationType = "ADD";
                objCabValidation.intSEDetailingId = this.intSEDetailingId;

                objCabValidation.ShapeParametersList = this.ShapeParametersList;


                string errMsg = string.Empty;
                string message = string.Empty;
                inputShapevalid = objCabValidation.DetailingShapeDataValidation(out message);
                if (inputShapevalid == true)
                {
                    errorMsg = "";
                    return true;
                }
                else
                {
                    errorMsg = message;
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message.ToString();
                return false;
            }
            finally
            {
                objCabValidation = null;
            }
        }

        /// <summary>
        /// To get CAB Product Details by SE Detail Id
        /// </summary>
        /// <param name="intSEDetailingID"></param>
        /// <returns></returns>
        public List<CABItem> GetCABProductMarkingDetailsBySEDetailingID(int intSEDetailingID)
        {
            List<CABItem> listGetCABProductMarkingDetails = new List<CABItem> { };
            DataSet dsGetCABProductMarkingDetails = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTSEDETAILINGID", intSEDetailingID.ToString());
                    var dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = new SqlCommand(SystemConstant.USP_CABPRODUCTMARKINGDETAILS_GET_CUBE, sqlConnection);
                    dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dataAdapter.SelectCommand.Parameters.AddRange(dynamicParameters.ParameterNames.Select(name => new SqlParameter(name, dynamicParameters.Get<object>(name))).ToArray());
                    dataAdapter.Fill(dsGetCABProductMarkingDetails);

                    //dsGetCABProductMarkingDetails = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.USP_CABPRODUCTMARKINGDETAILS_GET_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (dsGetCABProductMarkingDetails != null && dsGetCABProductMarkingDetails.Tables.Count != 0)
                    {
                        ShapeParameterCollection shapeParameterCollection = new ShapeParameterCollection();
                       // if (IndexusDistributionCache.SharedCache.Get("CabShapeparamCache") == null)
                        //{
                            shapeParameterCollection = shapeParameterCollection.ShapeParameterForCab_Get(intSEDetailingID);
                       // }
                        //else
                        //{
                        //    shapeParameterCollection = (ShapeParameterCollection)IndexusDistributionCache.SharedCache.Get("CabShapeparamCache");
                        //}
                        foreach (DataRowView drvCABProductMarkingDetails in dsGetCABProductMarkingDetails.Tables[0].DefaultView)
                        {
                            ShapeCode objShapeCode = new ShapeCode();
                            objShapeCode.ShapeID = Convert.ToInt32(drvCABProductMarkingDetails["INTSHAPEID"]);
                            objShapeCode.ShapeCodeName = Convert.ToString(drvCABProductMarkingDetails["SHAPECODE"]);
                            CABProductMarkID = Convert.ToInt32(drvCABProductMarkingDetails["INTCABPRODUCTMARKID"]);

                            shapeParameterCollection = shapeParameterCollection.ShapeParameterForCab_Get1(CABProductMarkID);
                            ShapeParameterCollection strucParamCollFilter = new ShapeParameterCollection();

                            foreach (ShapeParameter shapeParamCollection in shapeParameterCollection)
                            {
                                if (shapeParamCollection.ShapeId == Convert.ToInt32(drvCABProductMarkingDetails["INTSHAPEID"]))
                                {
                                    strucParamCollFilter.Add(shapeParamCollection);
                                }
                            }

                            objShapeCode.ShapeParam = strucParamCollFilter;

                            CABItem cabProductMarkingDetailsItem = new CABItem
                            {
                                CABProductMarkID = Convert.ToInt32(drvCABProductMarkingDetails["INTCABPRODUCTMARKID"]),
                                CABProductMarkName = drvCABProductMarkingDetails["VCHCABPRODUCTMARKNAME"].ToString(),
                                ProductCodeID = Convert.ToInt32(drvCABProductMarkingDetails["INTPRODUCTCODEID"]),
                                Quantity = Convert.ToInt32(drvCABProductMarkingDetails["INTMEMBERQTY"]),
                                //ShapeCodeId = drvCABProductMarkingDetails["INTSHAPECODE"].ToString(),
                                PinSizeID = Convert.ToInt32(drvCABProductMarkingDetails["INTPINSIZEID"]),
                                InvoiceLength = Convert.ToInt32(drvCABProductMarkingDetails["NUMINVOICELENGTH"]),
                                ProductionLength = Convert.ToInt32(drvCABProductMarkingDetails["NUMPRODUCTIONLENGTH"]),
                                InvoiceWeight = Convert.ToInt32(drvCABProductMarkingDetails["NUMINVOICEWEIGHT"]),
                                ProductionWeight = Convert.ToInt32(drvCABProductMarkingDetails["NUMPRODUCTIONWEIGHT"]),
                                ProduceIndicator = drvCABProductMarkingDetails["INTSTATUS"].ToString(),
                                DescScript = drvCABProductMarkingDetails["VCHDESCRIPT"].ToString(),
                                Diameter = Convert.ToInt32(drvCABProductMarkingDetails["INTDIAMETER"]),
                                Grade = drvCABProductMarkingDetails["GRADE"].ToString(),
                                PageNumber = drvCABProductMarkingDetails["PAGE_NUMBER"].ToString(),
                                //ACCProductNameforCAB = drvCABProductMarkingDetails["ACCESSORIESPRODUCTNAME"].ToString(),
                                Shape = objShapeCode,
                                ShapeCode = drvCABProductMarkingDetails["SHAPECODE"].ToString(),
                                IsVariableBar = Convert.ToBoolean(drvCABProductMarkingDetails["BITVARIESBAR"]),
                                // Added for CAB Shape Parameter Validation CR
                                // Status=drvCABProductMarkingDetails["status"].ToString()
                                DDStatus = drvCABProductMarkingDetails["status"].ToString()
                               
                            };
                            cabProductMarkingDetailsItem.ShapeParamTotal = objShapeCode.ShapeParam.Count;
                            listGetCABProductMarkingDetails.Add(cabProductMarkingDetailsItem);
                        }
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
            return listGetCABProductMarkingDetails;
        }

        /// <summary>
        /// To Insert & Update Accessories Product Details.
        /// </summary>
        /// <param name="intCABProductMarkID"></param>
        /// <param name="intSEDetailingId"></param>
        /// <returns></returns> 
        private int InsertUpdAccessories(int intCABProductMarkID, int intSEDetailingId)
        {
            DBManager dbManager = new DBManager();
            int intOutput = 0;
            int count = 1;
            try
            {
                //Get only the coupler materials from accessories list
                List<Accessory> lstCoupler = (from item in accList
                                              where item.MaterialType == "COUPLER_MATERIAL"
                                              select item).ToList<Accessory>();
                //dbManager.Open();
                //Loop through accessory list for distinct coupler type.
                foreach (Accessory couplerAccItem in lstCoupler)
                {
                    //Loop through the whole accessories list to add coupler, thread and lock nut material
                    foreach (Accessory accItem1 in accList)
                    {
                        if (accItem1.CouplerType.Equals(couplerAccItem.CouplerType))
                        {
                            string accProductMarkName = string.Empty;
                            if (accItem1.MaterialType.Equals("COUPLER_MATERIAL"))
                            {
                                accProductMarkName = this.CABProductMarkName + "-C" + count.ToString();
                            }
                            else if (accItem1.MaterialType.Equals("THREAD"))
                            {
                                accProductMarkName = this.CABProductMarkName + "-T" + count.ToString();
                            }
                            else if (accItem1.MaterialType.Equals("LOCK_NUT"))
                            {
                                accProductMarkName = this.CABProductMarkName + "-L" + count.ToString();
                            }
                            using (var sqlConnection = new SqlConnection(connectionString))
                            {
                                sqlConnection.Open();
                                var dynamicParameters = new DynamicParameters();
                                dynamicParameters.Add("@VCHACCPRODUCTMARKINGNAME", accProductMarkName);
                                dynamicParameters.Add("@INTQTY", this.Quantity);
                                dynamicParameters.Add("@INTSEDETAILING", intSEDetailingId);
                                dynamicParameters.Add("@INTNOOFPIECES", this.Quantity);
                                dynamicParameters.Add("@VCHSAPCODE", accItem1.SAPMaterialCode);
                                dynamicParameters.Add("@VCHCABPRODUCTMARKID", intCABProductMarkID);
                                dynamicParameters.Add("@BITISCOUPLER", accItem1.BitIsCoupler);
                                dynamicParameters.Add("@NUMINVOICEWEIGHTPERPC", "0.000");
                                dynamicParameters.Add("@NUMEXTERNALWIDTH", "0");
                                dynamicParameters.Add("@NUMEXTERNALHEIGHT", "0");
                                dynamicParameters.Add("@NUMEXTERNALLENGTH", "0");
                                dynamicParameters.Add("@NUMLENGTH", "0");
                                dynamicParameters.Add("@INTGROUPMARKID", this.GroupMarkId);
                                intOutput = sqlConnection.QueryFirstOrDefault<int>(SystemConstant.USP_CABACC_PRODUCTMARKINGDETAILS_INSUPD_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);
                                sqlConnection.Close();
                            }

                        }
                    }
                    count++;
                }
                lstCoupler.Clear();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            return intOutput;
        }

        /// <summary>
        /// To Insert & Update Accessories Product Details during variable length function.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="intCABProductMarkID"></param>
        /// <param name="intSEDetailingId"></param>
        /// <returns></returns>
        private int InsertUpdAccessories(CABItem obj, int intCABProductMarkID, int intSEDetailingId)
        {
            DBManager dbManager = new DBManager();
            int intOutput = 0;
            int count = 1;
            try
            {
                //Get only the coupler materials from accessories list
                List<Accessory> lstCoupler = (from item in obj.accList
                                              where item.MaterialType == "COUPLER_MATERIAL"
                                              select item).ToList<Accessory>();
                //dbManager.Open();
                //Loop through accessory list for distinct coupler type.
                foreach (Accessory couplerAccItem in lstCoupler)
                {
                    //Loop through the whole accessories list to add coupler, thread and lock nut material
                    foreach (Accessory accItem1 in obj.accList)
                    {
                        if (accItem1.CouplerType.Equals(couplerAccItem.CouplerType))
                        {
                            string accProductMarkName = string.Empty;
                            if (accItem1.MaterialType.Equals("COUPLER_MATERIAL"))
                            {
                                accProductMarkName = obj.CABProductMarkName + "-C" + count.ToString();
                            }
                            else if (accItem1.MaterialType.Equals("THREAD"))
                            {
                                accProductMarkName = obj.CABProductMarkName + "-T" + count.ToString();
                            }
                            else if (accItem1.MaterialType.Equals("LOCK_NUT"))
                            {
                                accProductMarkName = obj.CABProductMarkName + "-L" + count.ToString();
                            }

                            using (var sqlConnection = new SqlConnection(connectionString))
                            {
                                sqlConnection.Open();
                                var dynamicParameters = new DynamicParameters();
                                dynamicParameters.Add("@VCHACCPRODUCTMARKINGNAME", accProductMarkName);
                                dynamicParameters.Add("@INTQTY", obj.Quantity);
                                dynamicParameters.Add("@INTSEDETAILING", intSEDetailingId);
                                dynamicParameters.Add("@INTNOOFPIECES", obj.Quantity);
                                dynamicParameters.Add("@VCHSAPCODE", accItem1.SAPMaterialCode);
                                dynamicParameters.Add("@VCHCABPRODUCTMARKID", intCABProductMarkID);
                                dynamicParameters.Add("@BITISCOUPLER", accItem1.BitIsCoupler);
                                dynamicParameters.Add("@NUMINVOICEWEIGHTPERPC", "0.000");
                                dynamicParameters.Add("@NUMEXTERNALWIDTH", "0");
                                dynamicParameters.Add("@NUMEXTERNALHEIGHT", "0");
                                dynamicParameters.Add( "@NUMEXTERNALLENGTH", "0");
                                dynamicParameters.Add( "@NUMLENGTH", "0");
                                dynamicParameters.Add( "@INTGROUPMARKID", obj.GroupMarkId);
                                //intOutput = Convert.ToInt32(dbManager.ExecuteScalar(CommandType.StoredProcedure, "USP_CABACC_PRODUCTMARKINGDETAILS_INSUPD_CUBE"));
                                intOutput = Convert.ToInt32(sqlConnection.QueryFirstOrDefault<int>(SystemConstant.USP_CABACC_PRODUCTMARKINGDETAILS_INSUPD_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure));
                            }

                            

                        }
                    }
                    count++;
                }
                lstCoupler.Clear();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            return intOutput;
        }

        /// <summary>
        /// Method to copy BBS.
        /// </summary>
        /// <param name="bbsSource"></param>
        /// <param name="bbsTarget"></param>
        /// <param name="pojId"></param>
        /// <param name="wbsId"></param>
        /// <param name="vchStructureElementType"></param>
        /// <returns></returns>
        public int CopyBBS(string bbsSource, string bbsTarget, int pojId, int wbsId, string vchStructureElementType)
        {
            DBManager dbManager = new DBManager();
            int intOutput = 0;
            DataSet ds = new DataSet();
            try
            {

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@BBSSOURCE", bbsSource.Trim());
                    dynamicParameters.Add("@BBSTARGET", bbsTarget.Trim());
                    dynamicParameters.Add("@INTPROJECTID", pojId.ToString());
                    dynamicParameters.Add("@INTWBSELEMENTID", wbsId.ToString());
                    dynamicParameters.Add("@VCHSTRUCTUREELEMENTNAME", vchStructureElementType.ToString());
                    dynamicParameters.Add("@ROWCOUNT", "100");
                    //ds = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "COPY_BBS_INSERT_CUBE");
                    var dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = new SqlCommand(SystemConstant.COPY_BBS_INSERT_CUBE, sqlConnection);
                    dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dataAdapter.SelectCommand.Parameters.AddRange(dynamicParameters.ParameterNames.Select(name => new SqlParameter(name, dynamicParameters.Get<object>(name))).ToArray());
                    dataAdapter.Fill(ds);

                    //ds = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.COPY_BBS_INSERT_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (ds.Tables.Count != 0)
                    {
                        foreach (DataRowView drCABProductMarkID in ds.Tables[0].DefaultView)
                        {
                            intOutput = Convert.ToInt32(drCABProductMarkID[0].ToString());
                            break;
                        }
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
            return intOutput;
        }

        /// <summary>
        /// Method to check if the bbs is from arma plus or from CAB Data Entry. CAB Data Entry if 1, Arma plus 0.
        /// </summary>
        /// <param name="bbsSource"></param>
        /// <returns></returns>
        public bool CheckBbsSource(string bbsSource)
        {
            DBManager dbManager = new DBManager();
            bool boolOutput = false;
            DataSet dsChkBbsSource = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@BBSSOURCE", bbsSource.Trim());
                    //dsChkBbsSource = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "USP_CHECK_BBSSOURCE_CUBE");

                    var dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = new SqlCommand(SystemConstant.USP_CHECK_BBSSOURCE_CUBE, sqlConnection);
                    dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dataAdapter.SelectCommand.Parameters.AddRange(dynamicParameters.ParameterNames.Select(name => new SqlParameter(name, dynamicParameters.Get<object>(name))).ToArray());
                    dataAdapter.Fill(dsChkBbsSource);


                    //dsChkBbsSource = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.USP_CHECK_BBSSOURCE_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (dsChkBbsSource.Tables.Count != 0)
                    {
                        foreach (DataRowView drCABProductMarkID in dsChkBbsSource.Tables[0].DefaultView)
                        {
                            boolOutput = Convert.ToBoolean(drCABProductMarkID[0].ToString());
                            break;
                        }
                    }
                }

                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                dsChkBbsSource.Dispose();
            }
            return boolOutput;
        }

        /// <summary>
        /// Method to save detailing data from parallel table to the new table.
        /// </summary>
        /// <param name="groupMarkId"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool ValidateAndSave(int groupMarkId, out string errorMsg)
        {
            
            errorMsg = "";
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@GROUPMARKID", groupMarkId.ToString());
                    //dbManager.ExecuteNonQuery(CommandType.StoredProcedure, "USP_SAVE_SHAPETRANSDETAILS_INSERT");

                    sqlConnection.Query<DataSet>(SystemConstant.USP_SAVE_SHAPETRANSDETAILS_INSERT, dynamicParameters, commandType: CommandType.StoredProcedure);

                }

                return true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
            finally
            {

            }
        }

        /// <summary>
        /// Method to get all the calculated parameter values.
        /// </summary>
        /// <param name="shapeParametersList"></param>
        /// <param name="shapeCode"></param>
        /// <param name="diameter"></param>
        /// <param name="grade"></param>
        /// <param name="pin"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public List<ShapeParameter> ReValidateShapeParameters(List<ShapeParameter> shapeParametersList, string shapeCode, int diameter, string grade, int pin, out string errorMsg)
        {
            int paramselected = 0;
            errorMsg = "";
            try
            {
                #region Leg value handling
                ////Set the C and D parameter value if shape code is 061 and 079
                //if (shapeCode.Equals("061") || shapeCode.Equals("079"))
                //{
                //    foreach (ShapeParameter s in shapeParametersList)
                //    {
                //        if (s.ParameterName == "C" || s.ParameterName == "D")
                //        {
                //            if (diameter != 0)
                //            {
                //                //Set the value on the shape parameter grid.
                //                if (grade.ToUpper().Equals("H"))
                //                {
                //                    //Set the value to shape parameter observable collection.
                //                    s.ParameterValueCab = (13 * diameter).ToString().Trim();
                //                }
                //                else
                //                {
                //                    //Set the value to shape parameter observable collection.
                //                    s.ParameterValueCab = (12 * diameter).ToString().Trim();
                //                }
                //            }
                //        }
                //    }
                //}
                #endregion

                #region Height/Offset & Angle
                //string[] arr = { "OFF-NOR-HEIGHT", "OFF-NOR-OFFSET" };
                //foreach (ShapeParameter s in shapeParametersList)
                //{
                //    string nextToAngleParam = string.Empty;
                //    string prevToAngleParam = string.Empty;
                //    double[] arrResult = { 0, 0 };
                //    double[] resultOffset = { 0, 0, 0, 0 };
                //    if (s.AngleType.Equals("ANGLE"))
                //    {
                //        //If angle is preceeding angle check for sequence+2 componet for length type.
                //        var nextAngleParam = (from item1 in shapeParametersList
                //                              where item1.SequenceNumber == s.SequenceNumber + 2
                //                              select item1.AngleType).ToList();
                //        if (nextAngleParam.Count > 0)
                //        {
                //            nextToAngleParam = nextAngleParam[0].ToString();
                //        }
                //        //If angle is succeding angle check for sequence+2 componet for length type.
                //        var prevAngleParam = (from item1 in shapeParametersList
                //                              where item1.SequenceNumber == s.SequenceNumber - 1
                //                              select item1.AngleType).ToList();
                //        if (prevAngleParam.Count > 0)
                //        {
                //            prevToAngleParam = prevAngleParam[0].ToString();
                //        }
                //    }
                //    if ((s.AngleType.Equals("ANGLE") && Array.IndexOf(arr, nextToAngleParam) < 0 && Array.IndexOf(arr, prevToAngleParam) < 0) || s.AngleType.Equals("HEIGHT") || s.AngleType.Equals("OFFSET"))
                //    {
                //        #region If param is Angle/Height/Offset- Calculate and Set values
                //        //Get the formula to calculate angle or height value.
                //        var formula = (from item1 in shapeParametersList
                //                       where item1.SequenceNumber == s.SequenceNumber
                //                       select item1).ToList();
                //        //Calculate the result from the formula
                //        if (formula.Count > 0)
                //        {
                //            //Calculate the value from the first formula.
                //            if (formula[0].HeghtAngleFormula != "0")
                //            {
                //                arrResult[0] = CalculateFormulaString(shapeParametersList, formula[0].HeghtAngleFormula, "HEIGHT", diameter, pin);
                //                //Section for iteration method
                //                if (s.AngleType.Equals("HEIGHT"))
                //                {
                //                    var formulaGreaterThan90 = (from item1 in shapeParametersList
                //                                                where item1.SequenceNumber == s.SequenceNumber - 2
                //                                                select item1).ToList();
                //                    List<valuePair> valueList = new List<valuePair>();
                //                    foreach (ShapeParameter sParam in shapeParametersList)
                //                    {
                //                        if (sParam.ParameterName != "" && sParam.ParameterName != string.Empty && sParam.ParameterName != null)
                //                        {
                //                            if (!sParam.AngleType.Equals("ESPLICE") && !sParam.AngleType.Equals("NSPLICE"))
                //                            {
                //                                valueList.Add(new valuePair(sParam.ParameterName, Convert.ToDouble(sParam.ParameterValueCab)));
                //                            }
                //                        }
                //                    }
                //                    var valuePair = (from item1 in valueList
                //                                     where item1._key == formulaGreaterThan90[0].ParameterName
                //                                     select item1).ToList();
                //                    if (formulaGreaterThan90.Count > 0)
                //                    {
                //                        if (formulaGreaterThan90[0].HeghtAngleFormula.Contains("$") && formulaGreaterThan90[0].HeghtAngleFormula.Contains("@"))
                //                        {
                //                            //Iteration method to get the correct angle value					
                //                            double resultAngle = 0;
                //                            double minHtDiff = 0;
                //                            double approxRes = arrResult[0];

                //                            approxRes = approxRes - 20;
                //                            double[,] arrHtAng = new double[9, 2];
                //                            for (int count = 0; count < 9; count++)
                //                            {
                //                                if (valuePair.Count > 0)
                //                                {
                //                                    valuePair[0]._value = Convert.ToInt32(approxRes + (count * 5));
                //                                }
                //                                arrHtAng[count, 0] = Convert.ToInt32(approxRes + (count * 5));
                //                                arrHtAng[count, 1] = Math.Abs(Convert.ToInt32(s.ParameterValueCab.ToString()) - CalculateFormulaString(valueList, formulaGreaterThan90[0].HeghtAngleFormula, "HEIGHT", diameter, pin));
                //                                if (count > 0)
                //                                {
                //                                    if (arrHtAng[count, 1] < minHtDiff)
                //                                    {
                //                                        resultAngle = arrHtAng[count, 0];
                //                                        minHtDiff = arrHtAng[count, 1];
                //                                    }
                //                                }
                //                                else
                //                                {
                //                                    resultAngle = arrHtAng[count, 0];
                //                                    minHtDiff = arrHtAng[count, 1];
                //                                }
                //                            }
                //                            Array.Clear(arrHtAng, 0, arrHtAng.Length);

                //                            approxRes = resultAngle;
                //                            resultAngle = 0;
                //                            arrHtAng = new double[11, 2];
                //                            for (int count = 0; count < 11; count++)
                //                            {
                //                                if (valuePair.Count > 0)
                //                                {
                //                                    valuePair[0]._value = Convert.ToInt32(approxRes - 5 + count);
                //                                }
                //                                arrHtAng[count, 0] = Convert.ToInt32(approxRes - 5 + count);
                //                                arrHtAng[count, 1] = Math.Abs(Convert.ToInt32(s.ParameterValueCab.ToString()) - CalculateFormulaString(valueList, formulaGreaterThan90[0].HeghtAngleFormula, "HEIGHT", diameter, pin));
                //                                if (count > 0)
                //                                {
                //                                    if (arrHtAng[count, 1] < minHtDiff)
                //                                    {
                //                                        resultAngle = arrHtAng[count, 0];
                //                                        minHtDiff = arrHtAng[count, 1];
                //                                    }
                //                                }
                //                                else
                //                                {
                //                                    resultAngle = arrHtAng[count, 0];
                //                                    minHtDiff = arrHtAng[count, 1];
                //                                }
                //                            }
                //                            Array.Clear(arrHtAng, 0, arrHtAng.Length);
                //                            arrResult[0] = resultAngle;
                //                            //Iteration method to get the correct angle value
                //                        }
                //                    }
                //                }
                //            }
                //            //Calculate the value from the second formula.
                //            if (formula[0].HeghtSuceedAngleFormula != "0")
                //            {
                //                arrResult[1] = CalculateFormulaString(shapeParametersList, formula[0].HeghtSuceedAngleFormula, "HEIGHT", diameter, pin);
                //                //Section for iteration method
                //                if (s.AngleType.Equals("HEIGHT"))
                //                {
                //                    var formulaGreaterThan90 = (from item1 in shapeParametersList
                //                                                where item1.SequenceNumber == s.SequenceNumber + 1
                //                                                select item1).ToList();
                //                    List<valuePair> valueList = new List<valuePair>();
                //                    foreach (ShapeParameter sParam in shapeParametersList)
                //                    {
                //                        if (sParam.ParameterName != "" && sParam.ParameterName != string.Empty && sParam.ParameterName != null)
                //                        {
                //                            if (!sParam.AngleType.Equals("ESPLICE") && !sParam.AngleType.Equals("NSPLICE"))
                //                            {
                //                                valueList.Add(new valuePair(sParam.ParameterName, Convert.ToDouble(sParam.ParameterValueCab)));
                //                            }
                //                        }
                //                    }
                //                    var valuePair = (from item1 in valueList
                //                                     where item1._key == formulaGreaterThan90[0].ParameterName
                //                                     select item1).ToList();
                //                    if (formulaGreaterThan90.Count > 0)
                //                    {
                //                        if (formulaGreaterThan90[0].HeghtSuceedAngleFormula.Contains("$") && formulaGreaterThan90[0].HeghtSuceedAngleFormula.Contains("@"))
                //                        {
                //                            //Iteration method to get the correct angle value					
                //                            double resultAngle = 0;
                //                            double minHtDiff = 0;
                //                            double approxRes = arrResult[1];

                //                            approxRes = approxRes - 20;
                //                            double[,] arrHtAng = new double[9, 2];
                //                            for (int count = 0; count < 9; count++)
                //                            {
                //                                if (valuePair.Count > 0)
                //                                {
                //                                    valuePair[0]._value = Convert.ToInt32(approxRes + (count * 5));
                //                                }
                //                                arrHtAng[count, 0] = Convert.ToInt32(approxRes + (count * 5));
                //                                arrHtAng[count, 1] = Math.Abs(Convert.ToInt32(s.ParameterValueCab.ToString()) - CalculateFormulaString(valueList, formulaGreaterThan90[0].HeghtSuceedAngleFormula, "HEIGHT", diameter, pin));
                //                                if (count > 0)
                //                                {
                //                                    if (arrHtAng[count, 1] < minHtDiff)
                //                                    {
                //                                        resultAngle = arrHtAng[count, 0];
                //                                        minHtDiff = arrHtAng[count, 1];
                //                                    }
                //                                }
                //                                else
                //                                {
                //                                    resultAngle = arrHtAng[count, 0];
                //                                    minHtDiff = arrHtAng[count, 1];
                //                                }
                //                            }
                //                            Array.Clear(arrHtAng, 0, arrHtAng.Length);

                //                            approxRes = resultAngle;
                //                            resultAngle = 0;
                //                            arrHtAng = new double[11, 2];
                //                            for (int count = 0; count < 11; count++)
                //                            {
                //                                if (valuePair.Count > 0)
                //                                {
                //                                    valuePair[0]._value = Convert.ToInt32(approxRes - 5 + count);
                //                                }
                //                                arrHtAng[count, 0] = Convert.ToInt32(approxRes - 5 + count);
                //                                arrHtAng[count, 1] = Math.Abs(Convert.ToInt32(s.ParameterValueCab.ToString()) - CalculateFormulaString(valueList, formulaGreaterThan90[0].HeghtSuceedAngleFormula, "HEIGHT", diameter, pin));
                //                                if (count > 0)
                //                                {
                //                                    if (arrHtAng[count, 1] < minHtDiff)
                //                                    {
                //                                        resultAngle = arrHtAng[count, 0];
                //                                        minHtDiff = arrHtAng[count, 1];
                //                                    }
                //                                }
                //                                else
                //                                {
                //                                    resultAngle = arrHtAng[count, 0];
                //                                    minHtDiff = arrHtAng[count, 1];
                //                                }
                //                            }
                //                            Array.Clear(arrHtAng, 0, arrHtAng.Length);
                //                            arrResult[1] = resultAngle;
                //                            //Iteration method to get the correct angle value
                //                        }
                //                    }
                //                }
                //            }

                //            //Calculate the value from the first formula.
                //            if (formula[0].OffsetAngleFormula != "0")
                //            {
                //                arrResult[0] = CalculateFormulaString(shapeParametersList, formula[0].OffsetAngleFormula, "OFFSET", diameter, pin);
                //            }
                //            //Calculate the value from the second formula.
                //            if (formula[0].OffsetSuceedAngleFormula != "0")
                //            {
                //                arrResult[1] = CalculateFormulaString(shapeParametersList, formula[0].OffsetSuceedAngleFormula, "OFFSET", diameter, pin);
                //            }

                //            if (formula[0].HeghtAngleFormula != "0" || formula[0].OffsetAngleFormula != "0")
                //            {
                //                if (s.AngleType.Equals("ANGLE") || s.AngleType.Equals("HEIGHT") || s.AngleType.Equals("OFFSET"))
                //                {
                //                    for (int resultCount = 0; resultCount < arrResult.Length; resultCount++)
                //                    {
                //                        if (arrResult[resultCount] != 0)
                //                        {
                //                            if ((nextToAngleParam.ToUpper().Equals("HEIGHT") && !prevToAngleParam.ToUpper().Equals("HEIGHT")) || (nextToAngleParam.ToUpper().Equals("OFFSET") && !prevToAngleParam.ToUpper().Equals("OFFSET")))
                //                            {
                //                                if (resultCount == 0)
                //                                {
                //                                    paramselected = s.SequenceNumber + 2;
                //                                }
                //                                else if (resultCount == 1)
                //                                {
                //                                    paramselected = s.SequenceNumber + 3;
                //                                }
                //                            }
                //                            else if (prevToAngleParam.ToUpper().Equals("HEIGHT") || prevToAngleParam.ToUpper().Equals("OFFSET"))
                //                            {
                //                                if (resultCount == 0)
                //                                {
                //                                    if (s.SequenceNumber != 4)
                //                                    {
                //                                        paramselected = s.SequenceNumber - 3;
                //                                        if (shapeParametersList.FirstOrDefault(x => x.SequenceNumber == paramselected).AngleType.Equals("COUPLER_LENGTH"))
                //                                        {
                //                                            paramselected = s.SequenceNumber - 1;
                //                                        }
                //                                    }
                //                                    else
                //                                    {
                //                                        paramselected = s.SequenceNumber - 1;
                //                                    }
                //                                }
                //                                else if (resultCount == 1)
                //                                {
                //                                    paramselected = s.SequenceNumber - 1;
                //                                }
                //                            }
                //                            else if (s.AngleType.Equals("HEIGHT") || s.AngleType.Equals("OFFSET"))
                //                            {
                //                                if (resultCount == 0)
                //                                {
                //                                    if (s.SequenceNumber != 3)
                //                                    {
                //                                        paramselected = s.SequenceNumber - 2;
                //                                        if (shapeParametersList.FirstOrDefault(x => x.SequenceNumber == paramselected).AngleType.Equals("COUPLER_LENGTH"))
                //                                        {
                //                                            paramselected = s.SequenceNumber + 1;
                //                                        }
                //                                    }
                //                                    else
                //                                    {
                //                                        paramselected = s.SequenceNumber + 1;
                //                                    }
                //                                }
                //                                else if (resultCount == 1)
                //                                {
                //                                    paramselected = s.SequenceNumber + 1;
                //                                }
                //                            }
                //                            foreach (ShapeParameter sParam in shapeParametersList)
                //                            {
                //                                if (sParam.SequenceNumber == paramselected)
                //                                {
                //                                    sParam.ParameterValueCab = Convert.ToInt32(arrResult[resultCount]).ToString();
                //                                    break;
                //                                }
                //                            }
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //        #endregion
                //    }
                //    else if ((s.AngleType.Equals("ANGLE") && Array.IndexOf(arr, nextToAngleParam) >= 0) || (s.AngleType.Equals("ANGLE") && Array.IndexOf(arr, nextToAngleParam) >= 0) || Array.IndexOf(arr, s.AngleType) >= 0)
                //    {
                //        #region if param is offset+ normal calculate height
                //        //Get the formula to calculate angle or height value.
                //        var formula = (from item1 in shapeParametersList
                //                       where item1.SequenceNumber == s.SequenceNumber
                //                       select item1).ToList();

                //        if (formula.Count > 0)
                //        {
                //            //Calculate the value from the first formula.
                //            if (formula[0].HeghtAngleFormula != "0")
                //            {
                //                resultOffset[0] = CalculateFormulaString(shapeParametersList, formula[0].HeghtAngleFormula, "HEIGHT", diameter, pin);
                //            }
                //            //Calculate the value from the second formula.
                //            if (formula[0].HeghtSuceedAngleFormula != "0")
                //            {
                //                resultOffset[1] = CalculateFormulaString(shapeParametersList, formula[0].HeghtSuceedAngleFormula, "HEIGHT", diameter, pin);
                //            }
                //            //Calculate the value from the first formula.
                //            if (formula[0].OffsetAngleFormula != "0")
                //            {
                //                resultOffset[2] = CalculateFormulaString(shapeParametersList, formula[0].OffsetAngleFormula, "OFFSET", diameter, pin);
                //            }
                //            //Calculate the value from the second formula.
                //            if (formula[0].OffsetSuceedAngleFormula != "0")
                //            {
                //                resultOffset[3] = CalculateFormulaString(shapeParametersList, formula[0].OffsetSuceedAngleFormula, "OFFSET", diameter, pin);
                //            }

                //            if (formula[0].HeghtAngleFormula != "0" || formula[0].OffsetAngleFormula != "0")
                //            {
                //                if (s.AngleType.Equals("ANGLE") || s.AngleType.Equals("OFF-NOR-HEIGHT") || s.AngleType.Equals("OFF-NOR-OFFSET"))
                //                {
                //                    for (int resultCount = 0; resultCount < resultOffset.Length; resultCount++)
                //                    {
                //                        if (resultOffset[resultCount] != 0)
                //                        {
                //                            if (nextToAngleParam.ToUpper().Equals("OFF-NOR-HEIGHT"))
                //                            {
                //                                if (resultCount == 0)
                //                                {
                //                                    paramselected = s.SequenceNumber + 2;
                //                                }
                //                                else if (resultCount == 1)
                //                                {
                //                                    paramselected = s.SequenceNumber + 4;
                //                                }
                //                                else if (resultCount == 2)
                //                                {
                //                                    paramselected = s.SequenceNumber + 3;
                //                                }
                //                            }
                //                            else if (prevToAngleParam.ToUpper().Equals("OFF-NOR-OFFSET"))
                //                            {
                //                                if (resultCount == 0)
                //                                {
                //                                    paramselected = s.SequenceNumber - 4;
                //                                }
                //                                else if (resultCount == 1)
                //                                {
                //                                    paramselected = s.SequenceNumber - 2;
                //                                }
                //                                else if (resultCount == 3)
                //                                {
                //                                    paramselected = s.SequenceNumber - 1;
                //                                }
                //                            }
                //                            else if (s.AngleType.Equals("OFF-NOR-HEIGHT"))
                //                            {
                //                                if (resultCount == 0)
                //                                {
                //                                    paramselected = s.SequenceNumber - 2;
                //                                }
                //                                else if (resultCount == 1)
                //                                {
                //                                    paramselected = s.SequenceNumber + 2;
                //                                }
                //                                else if (resultCount == 2)
                //                                {
                //                                    paramselected = s.SequenceNumber + 1;
                //                                }
                //                            }
                //                            else if (s.AngleType.Equals("OFF-NOR-OFFSET"))
                //                            {
                //                                if (resultCount == 0)
                //                                {
                //                                    paramselected = s.SequenceNumber - 3;
                //                                }
                //                                else if (resultCount == 1)
                //                                {
                //                                    paramselected = s.SequenceNumber + 1;
                //                                }
                //                                else if (resultCount == 2)
                //                                {
                //                                    paramselected = s.SequenceNumber - 1;
                //                                }
                //                            }
                //                            foreach (ShapeParameter sParam in shapeParametersList)
                //                            {
                //                                if (sParam.SequenceNumber == paramselected)
                //                                {
                //                                    sParam.ParameterValueCab = Convert.ToInt32(resultOffset[resultCount]).ToString();
                //                                    break;
                //                                }
                //                            }
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //        #endregion
                //    }
                //    Array.Clear(arrResult, 0, arrResult.Length);
                //    Array.Clear(arrResult, 0, arrResult.Length);
                //}
                #endregion

                #region Custom Formula Evaluation
                ////Check for any formula.
                var custformula = (from item1 in this.ShapeParametersList
                                   where item1.CustFormula != "" && item1.CustFormula != string.Empty && item1.CustFormula != "0"
                                   select item1).ToList();

                if (custformula.Count > 0)
                {
                    //loop through to get all the formulas.
                    for (int count = 0; count < custformula.Count; count++)
                    {
                        string cformula = custformula[count].CustFormula.ToString();
                        if (cformula != "0")
                        {
                            int resultcust = GetCalculatedVal(this.ShapeParametersList, cformula, diameter);

                            foreach (ShapeParameter sparam in this.ShapeParametersList)
                            {
                                if (sparam.SequenceNumber == Convert.ToInt32(custformula[count].SequenceNumber))
                                {
                                    if (sparam.ParameterValueCab.Equals("0") || (sparam.EditFlag == false && sparam.VisibleFlag == false))
                                    {
                                        sparam.ParameterValueCab = Convert.ToInt32(resultcust).ToString();
                                    }
                                    //sparam.parametervaluecab = Convert.ToInt32(resultcust).tostring();
                                    break;
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Coupler validation.
                foreach (ShapeParameter s in shapeParametersList)
                {
                    if (s.AngleType == "ESPLICE")
                    {
                        string[] arrEsplice = { "ESCO", "ESCN", "ESCS", "EECO", "EECN", "EECS", "EBCS", "EESO", "EESN", "EESS", "ESSO", "ESSN", "ESSS", "ELCS", "ELSS" };

                        //Get the coupler related info in list object
                        //var assembly = Assembly.GetExecutingAssembly();
                        //Stream stream = assembly.GetManifestResourceStream(this.GetType(), "CouplerMaster.xml");
                        //XDocument doc1 = XDocument.Load(stream);
                        XDocument doc1 = XDocument.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CouplerMaster.xml"));
                        List<CouplerMaster> lstCoupler = (from c in doc1.Descendants("Coupler")
                                                          select new CouplerMaster
                                                          {
                                                              Type = c.Element("Type").Value,
                                                              CouplerMaterial = c.Element("CouplerMaterial").Value,
                                                              ThreadMaterial = c.Element("ThreadMaterial").Value,
                                                              LockNutMaterial = c.Element("LockNutMaterial").Value,
                                                              ParamA = c.Element("ParamA").Value,
                                                              ParamB = c.Element("ParamB").Value,
                                                              ParamC = c.Element("ParamC").Value,
                                                              CouplerDesc = c.Element("CouplerDesc").Value,
                                                              IntIndex = c.Element("IntIndex").Value,
                                                          }).ToList<CouplerMaster>();

                        //Get the coupler master values by the coupler type selected
                        var couplerMasterVals = (from c in lstCoupler where c.Type.Equals(s.ParameterValueCab.ToString().Trim() + diameter) select c).ToList();
                        if (couplerMasterVals.Count > 0)
                        {
                            int newCouplerLengthVal = 0;
                            string nxtCoupLength = (s.SequenceNumber + 1).ToString();
                            var nxtCoupLenName = (from item1 in shapeParametersList
                                                  where item1.SequenceNumber == Convert.ToInt32(nxtCoupLength)
                                                  select item1).ToList();
                            //Check if the shape has start as coupler or end.
                            var maxPosition = (from item2 in shapeParametersList
                                               where item2.VisibleFlag == true && item2.EditFlag == true
                                               select item2.SequenceNumber).Max();
                            var minPosition = (from item3 in shapeParametersList
                                               where item3.VisibleFlag == true && item3.EditFlag == true
                                               select item3.SequenceNumber).Min();

                            //Select the index to set the values.
                            if (nxtCoupLenName.Count > 0)
                            {
                                //Coupler can either be at start or at the end. Determine the length value accordingly.
                                int lenValEntered = 0;
                                if (Convert.ToInt32(nxtCoupLength) == Convert.ToInt32(maxPosition))
                                {
                                    lenValEntered = Convert.ToInt32(nxtCoupLength) - 2;
                                }
                                if (s.SequenceNumber == Convert.ToInt32(minPosition))
                                {
                                    lenValEntered = Convert.ToInt32(nxtCoupLength) + 1;
                                }

                                var lengthVal = (from item1 in shapeParametersList
                                                 where item1.SequenceNumber == lenValEntered
                                                 select item1).ToList();
                                if (lengthVal.Count > 0)
                                {
                                    string nextCoupParamVal = lengthVal[0].ParameterValueCab.ToString().Trim();
                                    newCouplerLengthVal = Convert.ToInt32(nextCoupParamVal) - Convert.ToInt32(couplerMasterVals[0].ParamB) + Convert.ToInt32(couplerMasterVals[0].ParamA) + Convert.ToInt32(couplerMasterVals[0].ParamC);
                                }
                                else
                                {
                                    newCouplerLengthVal = 0;
                                }
                            }
                            else
                            {
                                //If double ended coupler.
                                if (s.SequenceNumber - 3 > 0)
                                {
                                    var espliceCheck = (from item1 in shapeParametersList
                                                        where item1.SequenceNumber == Convert.ToInt32(s.SequenceNumber - 3)
                                                        select item1.AngleType).ToList();
                                    if (espliceCheck.Count > 0)
                                    {
                                        if (espliceCheck[0].ToUpper() == "ESPLICE")
                                        {
                                            nxtCoupLenName = null;
                                            nxtCoupLenName = (from item1 in shapeParametersList
                                                              where item1.SequenceNumber == Convert.ToInt32(s.SequenceNumber - 2)
                                                              select item1).ToList();
                                            if (nxtCoupLenName.Count > 0)
                                            {
                                                int lenValEntered = 0;
                                                lenValEntered = s.SequenceNumber - 2;
                                                var lengthVal = (from item1 in shapeParametersList
                                                                 where item1.SequenceNumber == lenValEntered
                                                                 select item1).ToList();
                                                if (lengthVal.Count > 0)
                                                {
                                                    string nextCoupParamVal = lengthVal[0].ParameterValueCab.ToString().Trim();
                                                    newCouplerLengthVal = Convert.ToInt32(nextCoupParamVal) - Convert.ToInt32(couplerMasterVals[0].ParamB) + Convert.ToInt32(couplerMasterVals[0].ParamA) + Convert.ToInt32(couplerMasterVals[0].ParamC);
                                                    nxtCoupLength = (s.SequenceNumber - 2).ToString();
                                                }
                                                else
                                                {
                                                    newCouplerLengthVal = 0;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            //Set the value in shape parameter list.
                            foreach (ShapeParameter sParam in shapeParametersList)
                            {
                                if (sParam.SequenceNumber == Convert.ToInt32(nxtCoupLength))
                                {
                                    sParam.ParameterValueCab = newCouplerLengthVal.ToString();
                                    break;
                                }
                            }
                        }
                        lstCoupler.Clear();
                        doc1 = null;
                        //stream.Dispose();
                    }
                }
                #endregion

                #region symmetric component handling.
                //get the sequence no for the symmetric component and assign the values.
                foreach (ShapeParameter s in shapeParametersList)
                {
                    var symSeqList = (from item1 in shapeParametersList where item1.symmetricIndex == s.ParameterName select item1.SequenceNumber).ToList();
                    if (symSeqList.Count > 0)
                    {
                        foreach (ShapeParameter sParam in shapeParametersList)
                        {
                            if (sParam.SequenceNumber == symSeqList[0])
                            {
                                sParam.ParameterValueCab = Convert.ToInt32(s.ParameterValueCab).ToString();
                                break;
                            }
                        }
                    }
                }
                #endregion

                return shapeParametersList;
            }
            catch (Exception ex)
            {
                errorMsg = "Error in shape code " + this.ShapeCode + ":" + ex.Message;
                return null;
            }
        }

        /// <summary>
        /// Method to validate shape parameters before update.
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool ShapeDataValidationOnUpdate(out string errMsg)
        {
            DBManager dbManager = new DBManager();
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            //dbManager.Open();
            string shapeCategory = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                sb = new StringBuilder();
                //Clarify Suni about standard id. if required include in the below query.
                //sb = sb.Append("SELECT CSM_SHAPE_ID,CSC_CAT_DESC FROM T_CAB_SHAPE_MAST INNER JOIN T_CM_SHAPE_CAT ON CSC_CAT_ID=CSM_CAT_ID INNER JOIN T_CM_SHAPE_MAPPING ON CSM_MASTER_SHAPE_ID=CSM_SHAPE_ID WHERE CSM_PLANT_SHAPE_ID= '" + this.ShapeCode + "' And CSM_ACT_INACTIVE=1 ");
                //ds = dbManager.ExecuteDataSet(CommandType.Text, sb.ToString());

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    sb = sb.Append("SELECT CSM_SHAPE_ID,CSC_CAT_DESC FROM T_CAB_SHAPE_MAST INNER JOIN T_CM_SHAPE_CAT ON CSC_CAT_ID=CSM_CAT_ID INNER JOIN T_CM_SHAPE_MAPPING ON CSM_MASTER_SHAPE_ID=CSM_SHAPE_ID WHERE CSM_PLANT_SHAPE_ID= '" + this.ShapeCode + "' And CSM_ACT_INACTIVE=1 ");
                    SqlCommand cmd = new SqlCommand(sb.ToString(), sqlConnection);
                    cmd.CommandType = CommandType.Text;
                   

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(ds);

                }
                    dt = ds.Tables[0];
                if (dt.Rows.Count == 0)
                {
                    errMsg = "Incorrect Shape Id.";
                    return false;
                }
                shapeCategory = dt.Rows[0]["CSC_CAT_DESC"].ToString();

                if (shapeCategory != "3-D")
                {
                    for (Int16 row = 0; row <= ShapeParametersList.Count - 1; row++)
                    {
                        if (ShapeParametersList[row].AngleType != "ESPLICE" && ShapeParametersList[row].AngleType != "DEXTRA" && ShapeParametersList[row].AngleType != "NSPLICE")
                        {
                            if (Convert.ToInt32(ShapeParametersList[row].ParameterValueCab) <= 0 && ShapeParametersList[row].AngleType.ToUpper() == "LENGTH")
                            {
                                errMsg = "Length is less than or equal to 0.";
                                return false;
                            }
                            if (Convert.ToInt32(ShapeParametersList[row].ParameterValueCab) <= 0 && ShapeParametersList[row].AngleType.ToUpper() == "SPRING")
                            {
                                errMsg = "Length is less than or equal to 0.";
                                return false;
                            }

                            if (ShapeParametersList[row].AngleType.ToUpper() == "ANGLE")
                            {
                                if (Convert.ToInt32(ShapeParametersList[row].ParameterValueCab) <= 0)
                                {
                                    errMsg = "Angle is less than or equal to 0.";
                                    return false;
                                }
                                if (Convert.ToInt32(ShapeParametersList[row].ParameterValueCab) > 180)
                                {
                                    errMsg = "Angle is greater than 180.";
                                    return false;
                                }
                            }
                            else if (ShapeParametersList[row].AngleType.ToUpper() == "ARC_RADIUS")
                            {
                                if (Convert.ToInt32(ShapeParametersList[row].ParameterValueCab) <= 0)
                                {
                                    errMsg = "Zero value entered for Arc Radius.";
                                    return false;
                                }
                            }
                        }
                    }
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message.ToString();
                return false;
            }
            finally
            {

            }
        }

        private class DBManager
        {
            internal void CreateParameters(int v)
            {
                throw new NotImplementedException();
            }

            internal DataSet ExecuteDataSet(CommandType text, string v)
            {
                throw new NotImplementedException();
            }

            internal void Open()
            {
                throw new NotImplementedException();
            }
        }
        #endregion


        #region TCT and OES
        /// <summary>
        /// Method for TCT and OES to calculate and save to database.
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool InsertToDbTCT(out string errMsg, out int ProductMarkId)
        {
            try
            {
                //Variable declaration.
                bool flag = true;
                errMsg = string.Empty;
                int pin = 0;
                int minLength = 0;
                int minHookLen = 0;
                int minHookHt = 0;
                int prodLen = 0;
                int invLen = 0;
                int totBend = 0;
                int totArc = 0;
                string bvbs = string.Empty;
                int productTypeId = 4;

                //Calculate the pin.
                if (this.PinSizeID == 0)
                {
                    GetPin(this.Grade.ToUpper().Trim(), this.Diameter, this.GroupMarkId, productTypeId, this.Shape.ShapeCodeName.Trim(), out errMsg, out pin, out minLength, out minHookLen, out minHookHt);
                    this.PinSizeID = pin;
                }

                //Calculate production, invoice length.
                this.accList = GetProdInvLength(out errMsg, out invLen, out prodLen, out totBend, out totArc, out bvbs);
                this.ProductionLength = prodLen;
                this.InvoiceLength = invLen;
                this.NoOfBends = totBend;
                this.BVBS = bvbs;

                //Get production, invoice weight.
                GetProdInvWeight(this.Diameter, this.Quantity, 1, Convert.ToInt32(this.InvoiceLength), Convert.ToInt32(this.ProductionLength));

                //Set the coupler parameters
                if (this.accList != null)
                {
                    if (this.accList.Count == 0)
                    {
                        this.Coupler1Standard = "";
                        this.Coupler1Type = "";
                        this.Coupler1 = "";
                        this.Coupler2Type = "";
                        this.Coupler2 = "";
                        this.Coupler2Standard = "";
                    }
                    else if (this.accList.Count > 0)
                    {
                        //Get only the coupler materials from accessories list
                        List<Accessory> lstCoupler = (from item in this.accList
                                                      where item.MaterialType == "COUPLER_MATERIAL"
                                                      select item).ToList<Accessory>();

                        if (lstCoupler.Count == 1)
                        {
                            int counter = 0;
                            foreach (Accessory acc in lstCoupler)
                            {
                                if (counter == 0)
                                {
                                    this.Coupler1Standard = acc.standard.ToString();
                                    this.Coupler1Type = acc.CouplerType.ToString();
                                    this.Coupler1 = acc.SAPMaterialCode.ToString();
                                    this.Coupler2Type = "";
                                    this.Coupler2 = "";
                                    this.Coupler2Standard = "";
                                }
                                counter++;
                            }
                        }
                        else if (lstCoupler.Count == 2)
                        {
                            int counter = 0;
                            foreach (Accessory acc in lstCoupler)
                            {
                                if (counter == 0)
                                {
                                    this.Coupler1Standard = acc.standard.ToString();
                                    this.Coupler1Type = acc.CouplerType.ToString();
                                    this.Coupler1 = acc.SAPMaterialCode.ToString();
                                }
                                if (counter == 1)
                                {
                                    this.Coupler2Type = acc.CouplerType.ToString();
                                    this.Coupler2 = acc.SAPMaterialCode.ToString();
                                    this.Coupler2Standard = acc.standard.ToString();
                                }
                                counter++;
                            }
                        }
                        //Clear the accessories list.
                        lstCoupler.Clear();
                    }
                }
                else
                {
                    this.Coupler1Standard = "";
                    this.Coupler1Type = "";
                    this.Coupler1 = "";
                    this.Coupler2Type = "";
                    this.Coupler2 = "";
                    this.Coupler2Standard = "";
                }

                //Inserrt to Db.
                ProductMarkId=InsertUpdCABAccessories(out errMsg);
                
                return flag;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                throw ex;
            }
        }

        /// <summary>
        /// Method to get all the minimum lengths depending upon the former type standard or non standard.
        /// </summary>
        /// <param name="groupMarkId"></param>
        /// <returns></returns>
        //public DataSet GetMinLengthOES(int groupMarkId)
        //{
        //    DBManager dbManager = new DBManager();
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        dbManager.Open();
        //        dbManager.CreateParameters(1);
        //        dbManager.AddParameters(0, "@GROUPMARKID", groupMarkId);
        //        ds = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "USP_GET_MIN_LENGTH_OES");
        //    }
        //    catch (Exception ex)
        //    {
        //        ds = null;
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //        ds.Dispose();
        //    }
        //    return ds;
        //}

        ///// <summary>
        ///// Method to get the WBS details from SOR range.
        ///// </summary>
        ///// <param name="fromSor"></param>
        ///// <param name="toSor"></param>
        ///// <returns></returns>
        //public DataSet GetBBSPostingCABRange(string fromSor, string toSor)
        //{
        //    DataSet ds = new DataSet();
        //    DBManager dbManager = new DBManager();
        //    try
        //    {
        //        dbManager.Open();
        //        dbManager.CreateParameters(2);
        //        dbManager.AddParameters(0, "@ORD_REQ_NO_FROM", fromSor);
        //        dbManager.AddParameters(1, "@ORD_REQ_NO_TO", toSor);
        //        ds = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "USP_GET_BBSPOSTING_RANGE_CUBE");
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    finally
        //    {
        //        ds.Dispose();
        //        dbManager.Dispose();
        //    }
        //}






        ///// <summary>
        ///// Method to get the groupmark id from bbs no for OES.
        ///// </summary>
        ///// <param name="intProjectId"></param>
        ///// <param name="intWBSElementId"></param>
        ///// <param name="vchStructureElementType"></param>
        ///// <param name="BBS_NO"></param>
        ///// <param name="intUserid"></param>
        ///// <returns></returns>
        //public int GetGroupMarkIdCAB(int intProjectId, int intWBSElementId, string vchStructureElementType, string BBS_NO, int intUserid)
        //{
        //    DBManager dbManager = new DBManager();
        //    int count = 0;
        //    try
        //    {
        //        dbManager.Open();
        //        dbManager.CreateParameters(5);
        //        dbManager.AddParameters(0, "@intProjectID", intProjectId);
        //        dbManager.AddParameters(1, "@intWBSElementID", intWBSElementId);
        //        dbManager.AddParameters(2, "@vchStructureElementName", vchStructureElementType);
        //        dbManager.AddParameters(3, "@vchGroupMarkingName", BBS_NO);
        //        dbManager.AddParameters(4, "@intUserId", intUserid);
        //        count = Convert.ToInt32(dbManager.ExecuteScalar(CommandType.StoredProcedure, "GET_GROUPMARKID_CUBE"));
        //        return count;
        //    }
        //    catch (Exception ex)
        //    {
        //        return 0;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //}

        ///// <summary>
        /////  Method to get the user id from user name.
        ///// </summary>
        ///// <param name="vchLoginId"></param>
        ///// <param name="vchFormName"></param>
        ///// <returns></returns>
        //public int ValidateUserOES(string vchLoginId, string vchFormName)
        //{
        //    DBManager dbManager = new DBManager();
        //    int count = 0;
        //    try
        //    {
        //        dbManager.Open();
        //        dbManager.CreateParameters(8);
        //        dbManager.AddParameters(0, "@vchLoginId", vchLoginId);
        //        dbManager.AddParameters(1, "@vchFormName", vchFormName);
        //        dbManager.AddParameters(2, "@bitReadAccess", 1);
        //        dbManager.AddParameters(3, "@bitWriteAccess", 1);
        //        dbManager.AddParameters(4, "@bitApproveAccess", 1);
        //        dbManager.AddParameters(5, "@intUserId", 0);
        //        dbManager.AddParameters(6, "@vchEmailId", string.Empty);
        //        dbManager.AddParameters(7, "@vchUserName", string.Empty);
        //        count = Convert.ToInt32(dbManager.ExecuteScalar(CommandType.StoredProcedure, "USERRIGHTS_VALIDATE"));
        //        int userId = (int)dbManager.Parameters[5].Value;
        //        return userId;
        //    }
        //    catch (Exception ex)
        //    {
        //        return 0;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //}

        ///// <summary>
        ///// Method to get all the calculated parameter values.
        ///// </summary>
        ///// <param name="errorMsg"></param>
        public void ValidateShapeParameters(out string errorMsg)
        {
            int paramselected = 0;
            errorMsg = "";
            try
            {
                #region Leg value handling
                //Set the C and D parameter value if shape code is 061 and 079
                if (this.ShapeCode.ToUpper() == "061" || this.ShapeCode.ToUpper() == "079")
                {
                    foreach (ShapeParameter s in this.ShapeParametersList)
                    {
                        if (s.ParameterName == "C" || s.ParameterName == "D")
                        {
                            if (this.Diameter != 0)
                            {
                                //Set the value on the shape parameter grid.
                                if (this.Grade.ToUpper().Equals("H"))
                                {
                                    if (s.ParameterValueCab.Equals("0"))
                                    {
                                        //Set the value to shape parameter observable collection.
                                        s.ParameterValueCab = (13 * this.Diameter).ToString().Trim();
                                    }
                                }
                                else
                                {
                                    if (s.ParameterValueCab.Equals("0"))
                                    {
                                        //Set the value to shape parameter observable collection.
                                        s.ParameterValueCab = (12 * this.Diameter).ToString().Trim();
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Height/Offset & Angle
                string[] arr = { "OFF-NOR-HEIGHT", "OFF-NOR-OFFSET" };
                foreach (ShapeParameter s in this.ShapeParametersList)
                {
                    string nextToAngleParam = string.Empty;
                    string prevToAngleParam = string.Empty;
                    double[] arrResult = { 0, 0 };
                    double[] resultOffset = { 0, 0, 0, 0 };
                    if (s.AngleType.Equals("ANGLE"))
                    {
                        //If angle is preceeding angle check for sequence+2 componet for length type.
                        var nextAngleParam = (from item1 in this.ShapeParametersList
                                              where item1.SequenceNumber == s.SequenceNumber + 2
                                              select item1.AngleType).ToList();
                        if (nextAngleParam.Count > 0)
                        {
                            nextToAngleParam = nextAngleParam[0].ToString();
                        }
                        //If angle is succeding angle check for sequence+2 componet for length type.
                        var prevAngleParam = (from item1 in this.ShapeParametersList
                                              where item1.SequenceNumber == s.SequenceNumber - 1
                                              select item1.AngleType).ToList();
                        if (prevAngleParam.Count > 0)
                        {
                            prevToAngleParam = prevAngleParam[0].ToString();
                        }
                    }
                    if ((s.AngleType.Equals("ANGLE") && Array.IndexOf(arr, nextToAngleParam) < 0 && Array.IndexOf(arr, prevToAngleParam) < 0) || s.AngleType.Equals("HEIGHT") || s.AngleType.Equals("OFFSET"))
                    {
                        #region If param is Angle/Height/Offset- Calculate and Set values
                        //Get the formula to calculate angle or height value.
                        var formula = (from item1 in this.ShapeParametersList
                                       where item1.SequenceNumber == s.SequenceNumber
                                       select item1).ToList();
                        //Calculate the result from the formula
                        if (formula.Count > 0)
                        {
                            //Calculate the value from the first formula.
                            if (formula[0].HeghtAngleFormula != "0")
                            {
                                arrResult[0] = CalculateFormulaString(this.ShapeParametersList, formula[0].HeghtAngleFormula, "HEIGHT", this.Diameter, this.PinSizeID);
                                //Section for iteration method
                                if (s.AngleType.Equals("HEIGHT"))
                                {
                                    var formulaGreaterThan90 = (from item1 in this.ShapeParametersList
                                                                where item1.SequenceNumber == s.SequenceNumber - 2
                                                                select item1).ToList();
                                    List<valuePair> valueList = new List<valuePair>();
                                    foreach (ShapeParameter sParam in this.ShapeParametersList)
                                    {
                                        if (sParam.ParameterName != "" && sParam.ParameterName != string.Empty && sParam.ParameterName != null)
                                        {
                                            if (!sParam.AngleType.Equals("ESPLICE") && !sParam.AngleType.Equals("NSPLICE"))
                                            {
                                                valueList.Add(new valuePair(sParam.ParameterName, Convert.ToDouble(sParam.ParameterValueCab)));
                                            }
                                        }
                                    }
                                    var valuePair = (from item1 in valueList
                                                     where item1._key == formulaGreaterThan90[0].ParameterName
                                                     select item1).ToList();
                                    if (formulaGreaterThan90.Count > 0)
                                    {
                                        if (formulaGreaterThan90[0].HeghtAngleFormula.Contains("$") && formulaGreaterThan90[0].HeghtAngleFormula.Contains("@"))
                                        {
                                            //Iteration method to get the correct angle value					
                                            double resultAngle = 0;
                                            double minHtDiff = 0;
                                            double approxRes = arrResult[0];

                                            approxRes = approxRes - 20;
                                            double[,] arrHtAng = new double[9, 2];
                                            for (int count = 0; count < 9; count++)
                                            {
                                                if (valuePair.Count > 0)
                                                {
                                                    valuePair[0]._value = Convert.ToInt32(approxRes + (count * 5));
                                                }
                                                arrHtAng[count, 0] = Convert.ToInt32(approxRes + (count * 5));
                                                arrHtAng[count, 1] = Math.Abs(Convert.ToInt32(s.ParameterValueCab.ToString()) - CalculateFormulaString(valueList, formulaGreaterThan90[0].HeghtAngleFormula, "HEIGHT", this.Diameter, this.PinSizeID));
                                                if (count > 0)
                                                {
                                                    if (arrHtAng[count, 1] < minHtDiff)
                                                    {
                                                        resultAngle = arrHtAng[count, 0];
                                                        minHtDiff = arrHtAng[count, 1];
                                                    }
                                                }
                                                else
                                                {
                                                    resultAngle = arrHtAng[count, 0];
                                                    minHtDiff = arrHtAng[count, 1];
                                                }
                                            }
                                            Array.Clear(arrHtAng, 0, arrHtAng.Length);

                                            approxRes = resultAngle;
                                            resultAngle = 0;
                                            arrHtAng = new double[11, 2];
                                            for (int count = 0; count < 11; count++)
                                            {
                                                if (valuePair.Count > 0)
                                                {
                                                    valuePair[0]._value = Convert.ToInt32(approxRes - 5 + count);
                                                }
                                                arrHtAng[count, 0] = Convert.ToInt32(approxRes - 5 + count);
                                                arrHtAng[count, 1] = Math.Abs(Convert.ToInt32(s.ParameterValueCab.ToString()) - CalculateFormulaString(valueList, formulaGreaterThan90[0].HeghtAngleFormula, "HEIGHT", this.Diameter, this.PinSizeID));
                                                if (count > 0)
                                                {
                                                    if (arrHtAng[count, 1] < minHtDiff)
                                                    {
                                                        resultAngle = arrHtAng[count, 0];
                                                        minHtDiff = arrHtAng[count, 1];
                                                    }
                                                }
                                                else
                                                {
                                                    resultAngle = arrHtAng[count, 0];
                                                    minHtDiff = arrHtAng[count, 1];
                                                }
                                            }
                                            Array.Clear(arrHtAng, 0, arrHtAng.Length);
                                            arrResult[0] = resultAngle;
                                            //Iteration method to get the correct angle value
                                        }
                                    }
                                }
                            }
                            //Calculate the value from the second formula.
                            if (formula[0].HeghtSuceedAngleFormula != "0")
                            {
                                arrResult[1] = CalculateFormulaString(this.ShapeParametersList, formula[0].HeghtSuceedAngleFormula, "HEIGHT", this.Diameter, this.PinSizeID);
                                //Section for iteration method
                                if (s.AngleType.Equals("HEIGHT"))
                                {
                                    var formulaGreaterThan90 = (from item1 in this.ShapeParametersList
                                                                where item1.SequenceNumber == s.SequenceNumber + 1
                                                                select item1).ToList();
                                    List<valuePair> valueList = new List<valuePair>();
                                    foreach (ShapeParameter sParam in this.ShapeParametersList)
                                    {
                                        if (sParam.ParameterName != "" && sParam.ParameterName != string.Empty && sParam.ParameterName != null)
                                        {
                                            if (!sParam.AngleType.Equals("ESPLICE") && !sParam.AngleType.Equals("NSPLICE"))
                                            {
                                                valueList.Add(new valuePair(sParam.ParameterName, Convert.ToDouble(sParam.ParameterValueCab)));
                                            }
                                        }
                                    }
                                    var valuePair = (from item1 in valueList
                                                     where item1._key == formulaGreaterThan90[0].ParameterName
                                                     select item1).ToList();
                                    if (formulaGreaterThan90.Count > 0)
                                    {
                                        if (formulaGreaterThan90[0].HeghtSuceedAngleFormula.Contains("$") && formulaGreaterThan90[0].HeghtSuceedAngleFormula.Contains("@"))
                                        {
                                            //Iteration method to get the correct angle value					
                                            double resultAngle = 0;
                                            double minHtDiff = 0;
                                            double approxRes = arrResult[1];

                                            approxRes = approxRes - 20;
                                            double[,] arrHtAng = new double[9, 2];
                                            for (int count = 0; count < 9; count++)
                                            {
                                                if (valuePair.Count > 0)
                                                {
                                                    valuePair[0]._value = Convert.ToInt32(approxRes + (count * 5));
                                                }
                                                arrHtAng[count, 0] = Convert.ToInt32(approxRes + (count * 5));
                                                arrHtAng[count, 1] = Math.Abs(Convert.ToInt32(s.ParameterValueCab.ToString()) - CalculateFormulaString(valueList, formulaGreaterThan90[0].HeghtSuceedAngleFormula, "HEIGHT", this.Diameter, this.PinSizeID));
                                                if (count > 0)
                                                {
                                                    if (arrHtAng[count, 1] < minHtDiff)
                                                    {
                                                        resultAngle = arrHtAng[count, 0];
                                                        minHtDiff = arrHtAng[count, 1];
                                                    }
                                                }
                                                else
                                                {
                                                    resultAngle = arrHtAng[count, 0];
                                                    minHtDiff = arrHtAng[count, 1];
                                                }
                                            }
                                            Array.Clear(arrHtAng, 0, arrHtAng.Length);

                                            approxRes = resultAngle;
                                            resultAngle = 0;
                                            arrHtAng = new double[11, 2];
                                            for (int count = 0; count < 11; count++)
                                            {
                                                if (valuePair.Count > 0)
                                                {
                                                    valuePair[0]._value = Convert.ToInt32(approxRes - 5 + count);
                                                }
                                                arrHtAng[count, 0] = Convert.ToInt32(approxRes - 5 + count);
                                                arrHtAng[count, 1] = Math.Abs(Convert.ToInt32(s.ParameterValueCab.ToString()) - CalculateFormulaString(valueList, formulaGreaterThan90[0].HeghtSuceedAngleFormula, "HEIGHT", this.Diameter, this.PinSizeID));
                                                if (count > 0)
                                                {
                                                    if (arrHtAng[count, 1] < minHtDiff)
                                                    {
                                                        resultAngle = arrHtAng[count, 0];
                                                        minHtDiff = arrHtAng[count, 1];
                                                    }
                                                }
                                                else
                                                {
                                                    resultAngle = arrHtAng[count, 0];
                                                    minHtDiff = arrHtAng[count, 1];
                                                }
                                            }
                                            Array.Clear(arrHtAng, 0, arrHtAng.Length);
                                            arrResult[1] = resultAngle;
                                            //Iteration method to get the correct angle value
                                        }
                                    }
                                }
                            }

                            //Calculate the value from the first formula.
                            if (formula[0].OffsetAngleFormula != "0")
                            {
                                arrResult[0] = CalculateFormulaString(this.ShapeParametersList, formula[0].OffsetAngleFormula, "OFFSET", this.Diameter, this.PinSizeID);
                            }
                            //Calculate the value from the second formula.
                            if (formula[0].OffsetSuceedAngleFormula != "0")
                            {
                                arrResult[1] = CalculateFormulaString(this.ShapeParametersList, formula[0].OffsetSuceedAngleFormula, "OFFSET", this.Diameter, this.PinSizeID);
                            }

                            if (formula[0].HeghtAngleFormula != "0" || formula[0].OffsetAngleFormula != "0" || formula[0].HeghtSuceedAngleFormula != "0" || formula[0].OffsetSuceedAngleFormula != "0")
                            {
                                if (s.AngleType.Equals("ANGLE") || s.AngleType.Equals("HEIGHT") || s.AngleType.Equals("OFFSET"))
                                {
                                    for (int resultCount = 0; resultCount < arrResult.Length; resultCount++)
                                    {
                                        if (arrResult[resultCount] != 0)
                                        {
                                            if ((nextToAngleParam.ToUpper().Equals("HEIGHT") && !prevToAngleParam.ToUpper().Equals("HEIGHT")) || (nextToAngleParam.ToUpper().Equals("OFFSET") && !prevToAngleParam.ToUpper().Equals("OFFSET")))
                                            {
                                                if (resultCount == 0)
                                                {
                                                    paramselected = s.SequenceNumber + 2;
                                                }
                                                else if (resultCount == 1)
                                                {
                                                    paramselected = s.SequenceNumber + 3;
                                                }
                                            }
                                            else if (prevToAngleParam.ToUpper().Equals("HEIGHT") || prevToAngleParam.ToUpper().Equals("OFFSET"))
                                            {
                                                if (resultCount == 0)
                                                {
                                                    if (s.SequenceNumber != 4)
                                                    {
                                                        paramselected = s.SequenceNumber - 3;
                                                        if (this.ShapeParametersList.FirstOrDefault(x => x.SequenceNumber == paramselected).AngleType.Equals("COUPLER_LENGTH"))
                                                        {
                                                            paramselected = s.SequenceNumber - 1;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        paramselected = s.SequenceNumber - 1;
                                                    }
                                                }
                                                else if (resultCount == 1)
                                                {
                                                    paramselected = s.SequenceNumber - 1;
                                                }
                                            }
                                            else if (s.AngleType.Equals("HEIGHT") || s.AngleType.Equals("OFFSET"))
                                            {
                                                if (resultCount == 0)
                                                {
                                                    if (s.SequenceNumber != 3)
                                                    {
                                                        paramselected = s.SequenceNumber - 2;
                                                        if (this.ShapeParametersList.FirstOrDefault(x => x.SequenceNumber == paramselected).AngleType.Equals("COUPLER_LENGTH"))
                                                        {
                                                            paramselected = s.SequenceNumber + 1;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        paramselected = s.SequenceNumber + 1;
                                                    }
                                                }
                                                else if (resultCount == 1)
                                                {
                                                    paramselected = s.SequenceNumber + 1;
                                                }
                                            }
                                            foreach (ShapeParameter sParam in this.ShapeParametersList)
                                            {
                                                if (sParam.SequenceNumber == paramselected)
                                                {
                                                    if (sParam.ParameterValueCab.Equals("0") || (sParam.EditFlag == false && sParam.VisibleFlag == false))
                                                    {
                                                        sParam.ParameterValueCab = Convert.ToInt32(arrResult[resultCount]).ToString();
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    else if ((s.AngleType.Equals("ANGLE") && Array.IndexOf(arr, nextToAngleParam) >= 0) || (s.AngleType.Equals("ANGLE") && Array.IndexOf(arr, nextToAngleParam) >= 0) || Array.IndexOf(arr, s.AngleType) >= 0)
                    {
                        #region if param is offset+ normal calculate height
                        //Get the formula to calculate angle or height value.
                        var formula = (from item1 in this.ShapeParametersList
                                       where item1.SequenceNumber == s.SequenceNumber
                                       select item1).ToList();

                        if (formula.Count > 0)
                        {
                            //Calculate the value from the first formula.
                            if (formula[0].HeghtAngleFormula != "0")
                            {
                                resultOffset[0] = CalculateFormulaString(this.ShapeParametersList, formula[0].HeghtAngleFormula, "HEIGHT", this.Diameter, this.PinSizeID);
                            }
                            //Calculate the value from the second formula.
                            if (formula[0].HeghtSuceedAngleFormula != "0")
                            {
                                resultOffset[1] = CalculateFormulaString(this.ShapeParametersList, formula[0].HeghtSuceedAngleFormula, "HEIGHT", this.Diameter, this.PinSizeID);
                            }
                            //Calculate the value from the first formula.
                            if (formula[0].OffsetAngleFormula != "0")
                            {
                                resultOffset[2] = CalculateFormulaString(this.ShapeParametersList, formula[0].OffsetAngleFormula, "OFFSET", this.Diameter, this.PinSizeID);
                            }
                            //Calculate the value from the second formula.
                            if (formula[0].OffsetSuceedAngleFormula != "0")
                            {
                                resultOffset[3] = CalculateFormulaString(this.ShapeParametersList, formula[0].OffsetSuceedAngleFormula, "OFFSET", this.Diameter, this.PinSizeID);
                            }

                            if (formula[0].HeghtAngleFormula != "0" || formula[0].OffsetAngleFormula != "0")
                            {
                                if (s.AngleType.Equals("ANGLE") || s.AngleType.Equals("OFF-NOR-HEIGHT") || s.AngleType.Equals("OFF-NOR-OFFSET"))
                                {
                                    for (int resultCount = 0; resultCount < resultOffset.Length; resultCount++)
                                    {
                                        if (resultOffset[resultCount] != 0)
                                        {
                                            if (nextToAngleParam.ToUpper().Equals("OFF-NOR-HEIGHT"))
                                            {
                                                if (resultCount == 0)
                                                {
                                                    paramselected = s.SequenceNumber + 2;
                                                }
                                                else if (resultCount == 1)
                                                {
                                                    paramselected = s.SequenceNumber + 4;
                                                }
                                                else if (resultCount == 2)
                                                {
                                                    paramselected = s.SequenceNumber + 3;
                                                }
                                            }
                                            else if (prevToAngleParam.ToUpper().Equals("OFF-NOR-OFFSET"))
                                            {
                                                if (resultCount == 0)
                                                {
                                                    paramselected = s.SequenceNumber - 4;
                                                }
                                                else if (resultCount == 1)
                                                {
                                                    paramselected = s.SequenceNumber - 2;
                                                }
                                                else if (resultCount == 3)
                                                {
                                                    paramselected = s.SequenceNumber - 1;
                                                }
                                            }
                                            else if (s.AngleType.Equals("OFF-NOR-HEIGHT"))
                                            {
                                                if (resultCount == 0)
                                                {
                                                    paramselected = s.SequenceNumber - 2;
                                                }
                                                else if (resultCount == 1)
                                                {
                                                    paramselected = s.SequenceNumber + 2;
                                                }
                                                else if (resultCount == 2)
                                                {
                                                    paramselected = s.SequenceNumber + 1;
                                                }
                                            }
                                            else if (s.AngleType.Equals("OFF-NOR-OFFSET"))
                                            {
                                                if (resultCount == 0)
                                                {
                                                    paramselected = s.SequenceNumber - 3;
                                                }
                                                else if (resultCount == 1)
                                                {
                                                    paramselected = s.SequenceNumber + 1;
                                                }
                                                else if (resultCount == 2)
                                                {
                                                    paramselected = s.SequenceNumber - 1;
                                                }
                                            }
                                            foreach (ShapeParameter sParam in this.ShapeParametersList)
                                            {
                                                if (sParam.SequenceNumber == paramselected)
                                                {
                                                    if (sParam.ParameterValueCab.Equals("0") || (sParam.EditFlag == false && sParam.VisibleFlag == false))
                                                    {
                                                        sParam.ParameterValueCab = Convert.ToInt32(resultOffset[resultCount]).ToString();
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    Array.Clear(arrResult, 0, arrResult.Length);
                    Array.Clear(arrResult, 0, arrResult.Length);
                }
                #endregion

                #region Custom Formula Evaluation
                //Check for any formula.
                var custFormula = (from item1 in this.ShapeParametersList
                                   where item1.CustFormula != "" && item1.CustFormula != string.Empty && item1.CustFormula != "0"
                                   select item1).ToList();

                if (custFormula.Count > 0)
                {
                    //Loop through to get all the formulas.
                    for (int count = 0; count < custFormula.Count; count++)
                    {
                        string CFormula = custFormula[count].CustFormula.ToString();
                        if (CFormula != "0")
                        {
                            int resultCust = GetCalculatedVal(this.ShapeParametersList, CFormula, this.Diameter);

                            foreach (ShapeParameter sParam in this.ShapeParametersList)
                            {
                                if (sParam.SequenceNumber == Convert.ToInt32(custFormula[count].SequenceNumber))
                                {
                                    if (sParam.ParameterValueCab.Equals("0") || (sParam.EditFlag == false && sParam.VisibleFlag == false))
                                    {
                                        sParam.ParameterValueCab = Convert.ToInt32(resultCust).ToString();
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Coupler validation.
                foreach (ShapeParameter s in this.ShapeParametersList)
                {
                    if (s.AngleType == "ESPLICE")
                    {
                        string[] arrEsplice = { "ESCO", "ESCN", "ESCS", "EECO", "EECN", "EECS", "EBCS", "EESO", "EESN", "EESS", "ESSO", "ESSN", "ESSS", "ELCS", "ELSS" };

                        //Get the coupler related info in list object
                        //var assembly = Assembly.GetExecutingAssembly();
                        //Stream stream = assembly.GetManifestResourceStream(this.GetType(), "CouplerMaster.xml");
                        XDocument doc1 = XDocument.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CouplerMaster.xml"));
                        List<CouplerMaster> lstCoupler = (from c in doc1.Descendants("Coupler")
                                                          select new CouplerMaster
                                                          {
                                                              Type = c.Element("Type").Value,
                                                              CouplerMaterial = c.Element("CouplerMaterial").Value,
                                                              ThreadMaterial = c.Element("ThreadMaterial").Value,
                                                              LockNutMaterial = c.Element("LockNutMaterial").Value,
                                                              ParamA = c.Element("ParamA").Value,
                                                              ParamB = c.Element("ParamB").Value,
                                                              ParamC = c.Element("ParamC").Value,
                                                              CouplerDesc = c.Element("CouplerDesc").Value,
                                                              IntIndex = c.Element("IntIndex").Value,
                                                          }).ToList<CouplerMaster>();

                        //Get the coupler master values by the coupler type selected
                        var couplerMasterVals = (from c in lstCoupler where c.Type.Equals(s.ParameterValueCab.ToString().Trim() + this.Diameter) select c).ToList();
                        if (couplerMasterVals.Count > 0)
                        {
                            int newCouplerLengthVal = 0;
                            string nxtCoupLength = (s.SequenceNumber + 1).ToString();
                            var nxtCoupLenName = (from item1 in this.ShapeParametersList
                                                  where item1.SequenceNumber == Convert.ToInt32(nxtCoupLength)
                                                  select item1).ToList();
                            //Check if the shape has start as coupler or end.
                            var maxPosition = (from item2 in this.ShapeParametersList
                                               where item2.VisibleFlag == true && item2.EditFlag == true
                                               select item2.SequenceNumber).Max();
                            var minPosition = (from item3 in this.ShapeParametersList
                                               where item3.VisibleFlag == true && item3.EditFlag == true
                                               select item3.SequenceNumber).Min();

                            //Select the index to set the values.
                            if (nxtCoupLenName.Count > 0)
                            {
                                //Coupler can either be at start or at the end. Determine the length value accordingly.
                                int lenValEntered = 0;
                                if (Convert.ToInt32(nxtCoupLength) == Convert.ToInt32(maxPosition))
                                {
                                    lenValEntered = Convert.ToInt32(nxtCoupLength) - 2;
                                }
                                if (s.SequenceNumber == Convert.ToInt32(minPosition))
                                {
                                    lenValEntered = Convert.ToInt32(nxtCoupLength) + 1;
                                }

                                var lengthVal = (from item1 in this.ShapeParametersList
                                                 where item1.SequenceNumber == lenValEntered
                                                 select item1).ToList();
                                if (lengthVal.Count > 0)
                                {
                                    string nextCoupParamVal = lengthVal[0].ParameterValueCab.ToString().Trim();
                                    newCouplerLengthVal = Convert.ToInt32(nextCoupParamVal) - Convert.ToInt32(couplerMasterVals[0].ParamB) + Convert.ToInt32(couplerMasterVals[0].ParamA) + Convert.ToInt32(couplerMasterVals[0].ParamC);
                                }
                                else
                                {
                                    newCouplerLengthVal = 0;
                                }
                            }
                            else
                            {
                                //If double ended coupler.
                                if (s.SequenceNumber - 3 > 0)
                                {
                                    var espliceCheck = (from item1 in this.ShapeParametersList
                                                        where item1.SequenceNumber == Convert.ToInt32(s.SequenceNumber - 3)
                                                        select item1.AngleType).ToList();
                                    if (espliceCheck.Count > 0)
                                    {
                                        if (espliceCheck[0].ToUpper() == "ESPLICE")
                                        {
                                            nxtCoupLenName = null;
                                            nxtCoupLenName = (from item1 in this.ShapeParametersList
                                                              where item1.SequenceNumber == Convert.ToInt32(s.SequenceNumber - 2)
                                                              select item1).ToList();
                                            if (nxtCoupLenName.Count > 0)
                                            {
                                                int lenValEntered = 0;
                                                lenValEntered = s.SequenceNumber - 2;
                                                var lengthVal = (from item1 in this.ShapeParametersList
                                                                 where item1.SequenceNumber == lenValEntered
                                                                 select item1).ToList();
                                                if (lengthVal.Count > 0)
                                                {
                                                    string nextCoupParamVal = lengthVal[0].ParameterValueCab.ToString().Trim();
                                                    newCouplerLengthVal = Convert.ToInt32(nextCoupParamVal) - Convert.ToInt32(couplerMasterVals[0].ParamB) + Convert.ToInt32(couplerMasterVals[0].ParamA) + Convert.ToInt32(couplerMasterVals[0].ParamC);
                                                    nxtCoupLength = (s.SequenceNumber - 2).ToString();
                                                }
                                                else
                                                {
                                                    newCouplerLengthVal = 0;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            //Set the value in shape parameter list.
                            foreach (ShapeParameter sParam in this.ShapeParametersList)
                            {
                                if (sParam.SequenceNumber == Convert.ToInt32(nxtCoupLength))
                                {
                                    sParam.ParameterValueCab = newCouplerLengthVal.ToString();
                                    break;
                                }
                            }
                        }
                        lstCoupler.Clear();
                        doc1 = null;
                      //  stream.Dispose();
                    }
                }
                #endregion

                #region symmetric component handling.
                //get the sequence no for the symmetric component and assign the values.
                foreach (ShapeParameter s in this.ShapeParametersList)
                {
                    var symSeqList = (from item1 in this.ShapeParametersList where item1.symmetricIndex == s.ParameterName select item1.SequenceNumber).ToList();
                    if (symSeqList.Count > 0)
                    {
                        foreach (ShapeParameter sParam in this.ShapeParametersList)
                        {
                            if (sParam.SequenceNumber == symSeqList[0])
                            {
                                sParam.ParameterValueCab = Convert.ToInt32(s.ParameterValueCab).ToString();
                                break;
                            }
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                errorMsg = "Error in shape code " + this.ShapeCode + ":" + ex.Message;
                throw ex;
            }
        }

        /// <summary>
        /// Method to execute arithmatic parser.
        /// </summary>
        /// <param name="shapeparam"></param>
        /// <param name="formula"></param>
        /// <param name="command"></param>
        /// <param name="dia"></param>
        /// <param name="pin"></param>
        /// <returns></returns>
        public int CalculateFormulaString(List<ShapeParameter> shapeparam, string formula, string command, int dia, int pin)
        {
            int result = 0;
            double pinRadius = 0;
            try
            {
                pinRadius = Convert.ToDouble(pin / 2.0);
                int editParamsCount = (from item in shapeparam
                                       where item.ParameterName != "" && item.ParameterName != string.Empty && item.ParameterName != null
                                       select item).Count();


                MathParserWrapper objParse = new MathParserWrapper();
                List<valuePair> valueList = new List<valuePair>();
                //Create the value pair.
                foreach (ShapeParameter shape in shapeparam)
                {
                    if (shape.ParameterName != "" && shape.ParameterName != string.Empty && shape.ParameterName != null)
                    {
                        if (!shape.AngleType.Equals("ESPLICE") && !shape.AngleType.Equals("NSPLICE"))
                        {
                            valueList.Add(new valuePair(shape.ParameterName, Convert.ToDouble(shape.ParameterValueCab)));
                        }
                    }
                }

                //Add the diameter
                valueList.Add(new valuePair("$", dia));
                //Add Pin radius
                valueList.Add(new valuePair("@", pinRadius));
                //Get the result.
                result = Convert.ToInt32(objParse.GetCalculatedValue(formula, valueList, false));
                objParse = null;
            }
            catch (Exception ex)
            {
                result = 0;
            }
            return result;
        }

        /// <summary>
        /// Method to execute arithmatic parser.
        /// </summary>
        /// <param name="valueList"></param>
        /// <param name="formula"></param>
        /// <param name="command"></param>
        /// <param name="dia"></param>
        /// <param name="pin"></param>
        /// <returns></returns>
        public int CalculateFormulaString(List<valuePair> valueList, string formula, string command, int dia, int pin)
        {
            int result = 0;
            double pinRadius = 0;
            try
            {
                pinRadius = (Convert.ToDouble(pin) / 2);
                MathParserWrapper objParse = new MathParserWrapper();
                //Add the diameter
                valueList.Add(new valuePair("$", dia));
                //Add Pin radius
                valueList.Add(new valuePair("@", pinRadius));
                //Get the result.
                result = Convert.ToInt32(objParse.GetCalculatedValue(formula, valueList, false));
                objParse = null;
            }
            catch (Exception ex)
            {
                result = 0;
            }
            return result;
        }

        /// <summary>
        /// Method to calculate height and angle.
        /// </summary>
        /// <param name="shapeparam"></param>
        /// <param name="formula"></param>
        /// <param name="command"></param>
        /// <param name="dia"></param>
        /// <returns></returns>
        //public double GetHeightAngle(List<ShapeParameter> shapeparam, string formula, string command, int dia)
        //{
        //    double result = 0;
        //    string[,] arr = new string[shapeparam.Count, 2];
        //    FormulaParser objFormulaParser = new FormulaParser();
        //    double[,] arrHtAng = new double[22, 2];
        //    try
        //    {
        //        int j = 0;
        //        for (int i = 0; i <= shapeparam.Count - 1; i++)
        //        {
        //            if (shapeparam[i].ParameterName != "" && shapeparam[i].ParameterName != string.Empty)
        //            {
        //                arr[j, 0] = shapeparam[i].ParameterName.ToString();
        //                arr[j, 1] = shapeparam[i].ParameterValueCab.ToString();
        //                j++;
        //            }
        //        }

        //        objFormulaParser._dia = 0;
        //        objFormulaParser._radius = 0;
        //        objFormulaParser._height = 0;
        //        objFormulaParser._length = 0;
        //        objFormulaParser._angle = 0;

        //        objFormulaParser._dia = Convert.ToDouble(dia);
        //        objFormulaParser._radius = (Convert.ToDouble(dia) / 2);

        //        #region Get height and angle for bend less than 90.
        //        if (formula.Length == 14)
        //        {
        //            string[] split = formula.ToUpper().Split(new string[] { "SIN" }, StringSplitOptions.None);
        //            int count = 0;
        //            foreach (string s in split)
        //            {
        //                for (int i = 0; i < arr.GetLength(0); i++)
        //                {
        //                    if (arr[i, 0] != null)
        //                    {
        //                        if (s.ToUpper().Contains(arr[i, 0].ToString().ToUpper()))
        //                        {
        //                            if (count == 0)
        //                            {
        //                                objFormulaParser._length = Convert.ToDouble(arr[i, 1].ToString());
        //                            }
        //                            else if (count == 1)
        //                            {
        //                                objFormulaParser._angle = Convert.ToDouble(arr[i, 1].ToString());
        //                            }
        //                            count++;
        //                        }
        //                    }
        //                }
        //            }
        //            result = objFormulaParser.GetHeightForLessThan90();
        //            if (command == "OFFSET")
        //            {
        //                result = Math.Sqrt((objFormulaParser._length * objFormulaParser._length) - (result * result));
        //            }
        //        }
        //        //Get angle for angle less than 90.
        //        else if (formula.Length == 18)
        //        {
        //            string length = formula.Substring(15, 1);
        //            for (int i = 0; i < arr.GetLength(0); i++)
        //            {
        //                if (arr[i, 0] != null)
        //                {
        //                    if (length == arr[i, 0].ToString().ToUpper())
        //                    {
        //                        objFormulaParser._length = Convert.ToDouble(arr[i, 1].ToString());
        //                        break;
        //                    }
        //                }
        //            }

        //            string height = formula.Substring(6, 1);
        //            for (int i = 0; i < arr.GetLength(0); i++)
        //            {
        //                if (arr[i, 0] != null)
        //                {
        //                    if (height == arr[i, 0].ToString().ToUpper())
        //                    {
        //                        objFormulaParser._height = Convert.ToDouble(arr[i, 1].ToString());
        //                        if (command == "OFFSET")
        //                        {
        //                            objFormulaParser._height = Math.Sqrt((objFormulaParser._length * objFormulaParser._length) - (objFormulaParser._height * objFormulaParser._height));
        //                        }
        //                        break;
        //                    }
        //                }
        //            }
        //            result = objFormulaParser.GetAngleForLessThan90();
        //        }
        //        #endregion

        //        #region Get height and angle for bend less than 90 and in same direction.
        //        if (formula.Length == 25)
        //        {
        //            string[] split = formula.ToUpper().Split(new string[] { "SIN" }, StringSplitOptions.None);
        //            int count = 0;
        //            foreach (string s in split)
        //            {
        //                for (int i = 0; i < arr.GetLength(0); i++)
        //                {
        //                    if (arr[i, 0] != null)
        //                    {
        //                        if (s.ToUpper().Contains(arr[i, 0].ToString().ToUpper()))
        //                        {
        //                            if (count == 0)
        //                            {
        //                                objFormulaParser._length = Convert.ToDouble(arr[i, 1].ToString());
        //                            }
        //                            else if (count == 1)
        //                            {
        //                                objFormulaParser._angle = Convert.ToDouble(arr[i, 1].ToString());
        //                            }
        //                            count++;
        //                        }
        //                    }
        //                }
        //            }
        //            result = objFormulaParser.GetHeightForSameBend();
        //            if (command == "OFFSET")
        //            {
        //                result = Math.Sqrt((objFormulaParser._length * objFormulaParser._length) - (result * result));
        //            }
        //        }
        //        //Get angle for angle less than 90.
        //        else if (formula.Length == 27)
        //        {
        //            string length = formula.Substring(9, 1);
        //            for (int i = 0; i < arr.GetLength(0); i++)
        //            {
        //                if (arr[i, 0] != null)
        //                {
        //                    if (length == arr[i, 0].ToString().ToUpper())
        //                    {
        //                        objFormulaParser._length = Convert.ToDouble(arr[i, 1].ToString());
        //                        break;
        //                    }
        //                }
        //            }

        //            string height = formula.Substring(5, 1);
        //            for (int i = 0; i < arr.GetLength(0); i++)
        //            {
        //                if (arr[i, 0] != null)
        //                {
        //                    if (height == arr[i, 0].ToString().ToUpper())
        //                    {
        //                        objFormulaParser._height = Convert.ToDouble(arr[i, 1].ToString());
        //                        if (command == "OFFSET")
        //                        {
        //                            objFormulaParser._height = Math.Sqrt((objFormulaParser._length * objFormulaParser._length) - (objFormulaParser._height * objFormulaParser._height));
        //                        }
        //                        break;
        //                    }
        //                }
        //            }
        //            result = objFormulaParser.GetAngleForSameBend();
        //        }
        //        #endregion

        //        #region Get height and angle for bend more than 90.
        //        else if (formula.Length == 84)
        //        {
        //            string angle = formula.Substring(25, 1);
        //            for (int i = 0; i < arr.GetLength(0); i++)
        //            {
        //                if (arr[i, 0] != null)
        //                {
        //                    if (angle == arr[i, 0].ToString().ToUpper())
        //                    {
        //                        objFormulaParser._angle = Convert.ToDouble(arr[i, 1].ToString());
        //                        break;
        //                    }
        //                }
        //            }
        //            string length = formula.Substring(32, 1);
        //            for (int i = 0; i < arr.GetLength(0); i++)
        //            {
        //                if (arr[i, 0] != null)
        //                {
        //                    if (length == arr[i, 0].ToString().ToUpper())
        //                    {
        //                        objFormulaParser._length = Convert.ToDouble(arr[i, 1].ToString());
        //                        break;
        //                    }
        //                }
        //            }
        //            result = objFormulaParser.GetHeightForGreaterThan90();
        //            if (command == "OFFSET")
        //            {
        //                result = Math.Sqrt((objFormulaParser._length * objFormulaParser._length) - (result * result));
        //            }
        //        }
        //        //Get angle for angle more than 90.
        //        else if (formula.Length == 16)
        //        {
        //            string length = formula.Substring(13, 1);
        //            for (int i = 0; i < arr.GetLength(0); i++)
        //            {
        //                if (arr[i, 0] != null)
        //                {
        //                    if (length == arr[i, 0].ToString().ToUpper())
        //                    {
        //                        objFormulaParser._length = Convert.ToDouble(arr[i, 1].ToString());
        //                        break;
        //                    }
        //                }
        //            }

        //            string height = formula.Substring(5, 1);
        //            for (int i = 0; i < arr.GetLength(0); i++)
        //            {
        //                if (arr[i, 0] != null)
        //                {
        //                    if (height == arr[i, 0].ToString().ToUpper())
        //                    {
        //                        objFormulaParser._height = Convert.ToDouble(arr[i, 1].ToString());
        //                        if (command == "OFFSET")
        //                        {
        //                            objFormulaParser._height = Math.Sqrt((objFormulaParser._length * objFormulaParser._length) - (objFormulaParser._height * objFormulaParser._height));
        //                        }
        //                        break;
        //                    }
        //                }
        //            }

        //            double approxRes = objFormulaParser.GetAngleForGreaterThan90();

        //            //Iteration method to get the correct angle value					
        //            double resultAngle = 0;
        //            double minHtDiff = 0;
        //            for (int count = 0; count < 22; count++)
        //            {
        //                objFormulaParser._angle = approxRes - 10 + count;
        //                arrHtAng[count, 0] = objFormulaParser._angle;
        //                arrHtAng[count, 1] = Math.Abs(objFormulaParser._height - objFormulaParser.GetHeightForGreaterThan90());
        //                if (count > 0)
        //                {
        //                    if (arrHtAng[count, 1] < minHtDiff)
        //                    {
        //                        resultAngle = arrHtAng[count, 0];
        //                        minHtDiff = arrHtAng[count, 1];
        //                    }
        //                }
        //                else
        //                {
        //                    resultAngle = arrHtAng[count, 0];
        //                    minHtDiff = arrHtAng[count, 1];
        //                }
        //            }
        //            result = resultAngle;
        //            //Iteration method to get the correct angle value
        //        }
        //        #endregion

        //        #region Get height and angle if bend is 90
        //        else if (formula.Length == 10)
        //        {
        //            string angle = formula.Substring(8, 1);
        //            for (int i = 0; i < arr.GetLength(0); i++)
        //            {
        //                if (arr[i, 0] != null)
        //                {
        //                    if (angle == arr[i, 0].ToString().ToUpper())
        //                    {
        //                        objFormulaParser._angle = Convert.ToDouble(arr[i, 1].ToString());
        //                        break;
        //                    }
        //                }
        //            }
        //            string length = formula.Substring(0, 1);
        //            for (int i = 0; i < arr.GetLength(0); i++)
        //            {
        //                if (arr[i, 0] != null)
        //                {
        //                    if (length == arr[i, 0].ToString().ToUpper())
        //                    {
        //                        objFormulaParser._length = Convert.ToDouble(arr[i, 1].ToString());
        //                        break;
        //                    }
        //                }
        //            }
        //            result = objFormulaParser.GetHeightForNormal();
        //            if (command == "OFFSET")
        //            {
        //                result = Math.Sqrt((objFormulaParser._length * objFormulaParser._length) - (result * result));
        //            }
        //        }
        //        //Get angle if shape angle is 90
        //        else if (formula.Length == 9)
        //        {
        //            string length = formula.Substring(7, 1);
        //            for (int i = 0; i < arr.GetLength(0); i++)
        //            {
        //                if (arr[i, 0] != null)
        //                {
        //                    if (length == arr[i, 0].ToString().ToUpper())
        //                    {
        //                        objFormulaParser._length = Convert.ToDouble(arr[i, 1].ToString());
        //                        break;
        //                    }
        //                }
        //            }

        //            string height = formula.Substring(5, 1);
        //            for (int i = 0; i < arr.GetLength(0); i++)
        //            {
        //                if (arr[i, 0] != null)
        //                {
        //                    if (height == arr[i, 0].ToString().ToUpper())
        //                    {
        //                        objFormulaParser._height = Convert.ToDouble(arr[i, 1].ToString());
        //                        if (command == "OFFSET")
        //                        {
        //                            objFormulaParser._height = Math.Sqrt((objFormulaParser._length * objFormulaParser._length) - (objFormulaParser._height * objFormulaParser._height));
        //                        }
        //                        break;
        //                    }
        //                }
        //            }

        //            result = objFormulaParser.GetAngleForNormal();
        //        }
        //        #endregion

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        return result;
        //    }
        //    finally
        //    {
        //        Array.Clear(arr, 0, arr.Length);
        //        Array.Clear(arrHtAng, 0, arrHtAng.Length);
        //        objFormulaParser = null;
        //    }
        //}

        /// <summary>
        /// Method to calculate custom formulas
        /// </summary>
        /// <param name="shapeparam"></param>
        /// <param name="formula"></param>
        /// <param name="dia"></param>
        /// <returns></returns>
        public int GetCalculatedVal(List<ShapeParameter> shapeparam, string formula, int dia)
        {
            int result = 0;
            try
            {
                if (!formula.ToUpper().Contains("SIN") && !formula.ToUpper().Contains("COS") && !formula.ToUpper().Contains("TAN") && !formula.ToUpper().Contains("$") && !formula.ToUpper().Contains("@"))
                {
                    int count = (from param in shapeparam
                                 where param.ParameterName != "" && param.ParameterName != string.Empty
                                 select param).Count();
                    string[,] arr = new string[count + 2, 2];
                    int j = 0;
                    shapeparam = shapeparam.OrderBy(O => O.ParameterName).ToList();
                    for (int i = 0; i <= shapeparam.Count - 1; i++)
                    {
                        if (shapeparam[i].ParameterName != "" && shapeparam[i].ParameterName != string.Empty)
                        {
                            arr[j, 0] = shapeparam[i].ParameterName.ToString();
                            arr[j, 1] = shapeparam[i].ParameterValueCab.ToString();
                            j++;
                        }
                    }
                    if (dia != null)
                    {
                        arr[count, 0] = "$";
                        arr[count, 1] = dia.ToString();
                    }
                    if (this.PinSizeID != null)
                    {
                        arr[count + 1, 0] = "@";
                        arr[count + 1, 1] = (this.PinSizeID / 2).ToString();
                    }
                    //Call math parser class to execute the formula.
                    //MathParserWrapper objMath = new MathParserWrapper();
                    //result = Convert.ToInt32(objMath.EvaluateFormula(formula,Convert.ToBoolean(arr)));
                    NatSteel.NDS.BLL.MathParser objMath = new NatSteel.NDS.BLL.MathParser();
                    result = Convert.ToInt32(objMath.Evaluate(formula, arr));
                    objMath = null;
                    Array.Clear(arr, 0, arr.Length);
                }
                else
                {
                    double pinRadius = this.PinSizeID / 2;
                    ExpressionParser.MathParserWrapper objParse = new ExpressionParser.MathParserWrapper();
                    List<valuePair> valueList = new List<valuePair>();
                    //Create the value pair.
                    foreach (ShapeParameter shape in shapeparam)
                    {
                        if (shape.ParameterName != "" && shape.ParameterName != string.Empty && shape.ParameterName != null)
                        {
                            valueList.Add(new valuePair(shape.ParameterName, Convert.ToDouble(shape.ParameterValueCab)));
                        }
                    }

                    //Add the diameter
                    valueList.Add(new valuePair("$", dia));
                    //Add Pin radius
                    valueList.Add(new valuePair("@", pinRadius));
                    //Get the result.
                    result = Convert.ToInt32(objParse.GetCalculatedValue(formula, valueList, false));
                    objParse = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }



        #endregion


    }
}




