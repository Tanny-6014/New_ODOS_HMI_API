using Microsoft.VisualBasic;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using DrainService.Interfaces;
using DrainService.Constants;
using Dapper;
using DrainService.Dtos;


namespace DrainService.Repositories
{
    public class OESCABItem
    {

        private readonly IConfiguration _configuration;
        private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";
        //private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";
        #region "Properties .."
        public Int32 SEDetailingID { get; set; }
        public Int32 StructureMarkId { get; set; }
        public Int32 CABProductMarkID { get; set; }
        public Int32 ProductCodeID { get; set; }
        public Int32 Quantity { get; set; }
        public Int32 PinSizeID { get; set; }
        public string Status { get; set; }
        public Int32 Diameter { get; set; }
        public string ShapeCode { get; set; }
        public string DescScript { get; set; }
        public string CABProductMarkName { get; set; }

        public List<ShapeParameter> ShapeParametersList { get; set; }
        public List<Accessory> accList { get; set; }

        public OESCABItem CABProductItem { get; set; }
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

        // BBS No
        public string BBSNo { get; set; }

        // VPN User
        public bool IsVPNUsers { get; set; }

        //New parameter for cube integration
        public string ProduceInd { get; set; }
        public ShapeCode Shape { get; set; }
        public bool IsVariableBar { get; set; }
        // Parameter for pin type
        public int PinType { get; set; }

        #endregion

        #region "Constructor"

        public OESCABItem()
        {

        }

        #endregion

        //#region Cube
        ///// <summary>
        ///// Method to get the sedetailingid from groupmarkid for BPC.
        ///// </summary>
        ///// <param name="groupMarkId"></param>
        ///// <param name="errorMessage"></param>
        ///// <returns></returns>
        //public int GetSeDetailingId(int groupMarkId, out string errorMessage)
        //{
        //    int seDetailingId = 0;
        //    errorMessage = string.Empty;
        //    OESDBManager dbManager = new OESDBManager();
        //    try
        //    {
        //        sqlConnection.Open();
        //        dbManager.CreateParameters(1);
        //        dynamicParameters.Add(0, "@INTGROUPMARK", groupMarkId);
        //        object returnVal = dbManager.ExecuteScalar(CommandType.StoredProcedure, "USP_GET_SEDETAILING_CUBE");
        //        return Convert.ToInt32(returnVal);
        //    }
        //    catch (Exception ex)
        //    {
        //        errorMessage = ex.Message;
        //        return seDetailingId;
        //    }
        //    finally
        //    {
        //        
        //    }
        //}

        ///// <summary>
        ///// Method to calcullate the envelope length.
        ///// </summary>
        ///// <param name="shapeCode"></param>
        ///// <param name="shapeparameterList"></param>
        ///// <param name="dia"></param>
        ///// <param name="envLength"></param>
        ///// <param name="envWidth"></param>
        ///// <param name="envHeight"></param>
        ///// <param name="errorMsg"></param>
        //public void EnvelopeCalculation(string shapeCode, List<ShapeParameter> shapeparameterList, int dia, out int envLength, out int envWidth, out int envHeight, out string errorMsg)
        //{
        //    envLength = 0;
        //    envWidth = 0;
        //    envHeight = 0;
        //    errorMsg = "";
        //    string[] arrHeight = { "HEIGHT", "OFFSET LENGTH", "ON_HEIGHT", "ON_OFFSET" };
        //    OESDBManager dbManager = new OESDBManager();
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        //Get the shape code cordinates along with the default values.
        //        sqlConnection.Open();
        //        dbManager.CreateParameters(1);
        //        dynamicParameters.Add(0, "@SHAPECODE", shapeCode);
        //        ds = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "USP_GET_SHAPE_CORD_CUBE");
        //        if (ds != null)
        //        {
        //            if (ds.Tables.Count > 0)
        //            {
        //                if (ds.Tables[1].Rows[0]["ENV_LENGTH"].ToString().Equals("0") || ds.Tables[1].Rows[0]["ENV_LENGTH"].ToString().Trim().Equals(string.Empty) || ds.Tables[1].Rows[0]["ENV_LENGTH"].ToString().Trim().Equals(""))
        //                {
        //                    DataTable dt = ds.Tables[0];
        //                    //Calcullate the cordinates.
        //                    if (dt.Rows.Count > 0)
        //                    {
        //                        int startX = Convert.ToInt32(dt.Rows[0]["CSM_X_COR"].ToString());
        //                        int startY = Convert.ToInt32(dt.Rows[0]["CSM_Y_COR"].ToString());
        //                        double scale = Convert.ToDouble(dt.Rows[0]["CSM_SCALE"].ToString());
        //                        string catagory = dt.Rows[0]["CSC_CAT_DESC"].ToString().ToUpper();
        //                        int count = 0;
        //                        int[,] arr = new int[dt.Rows.Count, 3];
        //                        if (catagory != "3-D")
        //                        {
        //                            for (int rowinc = 0; rowinc <= dt.Rows.Count - 1; rowinc++)
        //                            {
        //                                for (Int16 row = 0; row <= shapeparameterList.Count - 1; row++)
        //                                {
        //                                    if (dt.Rows[rowinc]["CSD_SEQ_NO"].ToString() == shapeparameterList[row].SequenceNumber.ToString())
        //                                    {
        //                                        if (Convert.ToInt32(dt.Rows[rowinc]["CSD_PAR1"].ToString()) != 0)
        //                                        {
        //                                            if (count == 0)
        //                                            {
        //                                                arr[rowinc, 0] = startX + Convert.ToInt32((Convert.ToInt32(dt.Rows[rowinc]["CSD_END_POINT_X"].ToString()) - startX) * Convert.ToInt32(shapeparameterList[row].ParameterValueCab) / Convert.ToDouble(dt.Rows[rowinc]["CSD_PAR1"].ToString()));
        //                                                arr[rowinc, 1] = startY + Convert.ToInt32((Convert.ToInt32(dt.Rows[rowinc]["CSD_END_POINT_Y"].ToString()) - startY) * Convert.ToInt32(shapeparameterList[row].ParameterValueCab) / Convert.ToDouble(dt.Rows[rowinc]["CSD_PAR1"].ToString()));
        //                                                count++;
        //                                            }
        //                                            else
        //                                            {
        //                                                if (dt.Rows[rowinc - 1]["CSD_TYPE"].ToString().ToUpper().Equals("ANGLE"))
        //                                                {
        //                                                    int paramVal = 0;
        //                                                    ShapeParameter shape = shapeparameterList.FirstOrDefault(x => x.SequenceNumber == shapeparameterList[row].SequenceNumber - 1);
        //                                                    paramVal = Convert.ToInt32(shape.ParameterValueCab);
        //                                                    arr[rowinc, 0] = arr[rowinc - 1, 0] + Convert.ToInt32((Convert.ToInt32(dt.Rows[rowinc]["CSD_END_POINT_X"].ToString()) - Convert.ToInt32(dt.Rows[rowinc - 1]["CSD_END_POINT_X"].ToString())) * (Convert.ToInt32(shapeparameterList[row].ParameterValueCab) / Convert.ToDouble(dt.Rows[rowinc]["CSD_PAR1"].ToString())) * ((Math.Cos(paramVal * Math.PI / 180)) / (Math.Cos(Convert.ToInt32(dt.Rows[rowinc - 1]["CSD_PAR2"].ToString()) * Math.PI / 180))));
        //                                                    arr[rowinc, 1] = arr[rowinc - 1, 1] + Convert.ToInt32((Convert.ToInt32(dt.Rows[rowinc]["CSD_END_POINT_Y"].ToString()) - Convert.ToInt32(dt.Rows[rowinc - 1]["CSD_END_POINT_Y"].ToString())) * (Convert.ToInt32(shapeparameterList[row].ParameterValueCab) / Convert.ToDouble(dt.Rows[rowinc]["CSD_PAR1"].ToString())) * ((Math.Sin(paramVal * Math.PI / 180)) / (Math.Sin(Convert.ToInt32(dt.Rows[rowinc - 1]["CSD_PAR2"].ToString()) * Math.PI / 180))));
        //                                                    count++;
        //                                                }
        //                                                else
        //                                                {
        //                                                    arr[rowinc, 0] = arr[rowinc - 1, 0] + Convert.ToInt32((Convert.ToInt32(dt.Rows[rowinc]["CSD_END_POINT_X"].ToString()) - Convert.ToInt32(dt.Rows[rowinc - 1]["CSD_END_POINT_X"].ToString())) * (Convert.ToInt32(shapeparameterList[row].ParameterValueCab) / Convert.ToDouble(dt.Rows[rowinc]["CSD_PAR1"].ToString())));
        //                                                    arr[rowinc, 1] = arr[rowinc - 1, 1] + Convert.ToInt32((Convert.ToInt32(dt.Rows[rowinc]["CSD_END_POINT_Y"].ToString()) - Convert.ToInt32(dt.Rows[rowinc - 1]["CSD_END_POINT_Y"].ToString())) * (Convert.ToInt32(shapeparameterList[row].ParameterValueCab) / Convert.ToDouble(dt.Rows[rowinc]["CSD_PAR1"].ToString())));
        //                                                    count++;
        //                                                }
        //                                            }
        //                                        }
        //                                        else if (Array.IndexOf(arrHeight, dt.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper()) >= 0)
        //                                        {
        //                                            arr[rowinc, 0] = arr[rowinc - 1, 0];
        //                                            arr[rowinc, 1] = arr[rowinc - 1, 1];
        //                                            dt.Rows[rowinc]["CSD_END_POINT_X"] = dt.Rows[rowinc - 1]["CSD_END_POINT_X"];
        //                                            dt.Rows[rowinc]["CSD_END_POINT_Y"] = dt.Rows[rowinc - 1]["CSD_END_POINT_Y"];
        //                                            count++;
        //                                        }
        //                                        else
        //                                        {
        //                                            if (count > 0)
        //                                            {
        //                                                if (!(dt.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("ESPLICE")) && !(dt.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("ESPLICE LENGTH")) && !(dt.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("NSPLICE")) && !(dt.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("SPAN")) && !(dt.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("ARC LENGTH")))
        //                                                {
        //                                                    arr[rowinc, 0] = arr[rowinc - 1, 0] + Convert.ToInt32((Convert.ToInt32(dt.Rows[rowinc]["CSD_END_POINT_X"].ToString()) - Convert.ToInt32(dt.Rows[rowinc - 1]["CSD_END_POINT_X"].ToString())) * (Math.Cos(Convert.ToInt32(shapeparameterList[row].ParameterValueCab) * Math.PI / 180)) / (Math.Cos(Convert.ToInt32(dt.Rows[rowinc]["CSD_PAR2"].ToString()) * Math.PI / 180)));
        //                                                    arr[rowinc, 1] = arr[rowinc - 1, 1] + Convert.ToInt32((Convert.ToInt32(dt.Rows[rowinc]["CSD_END_POINT_Y"].ToString()) - Convert.ToInt32(dt.Rows[rowinc - 1]["CSD_END_POINT_Y"].ToString())) * (Math.Sin(Convert.ToInt32(shapeparameterList[row].ParameterValueCab) * Math.PI / 180)) / (Math.Sin(Convert.ToInt32(dt.Rows[rowinc]["CSD_PAR2"].ToString()) * Math.PI / 180)));
        //                                                    count++;
        //                                                }
        //                                                else
        //                                                {
        //                                                    arr[rowinc, 0] = arr[rowinc - 1, 0];
        //                                                    arr[rowinc, 1] = arr[rowinc - 1, 1];
        //                                                    dt.Rows[rowinc]["CSD_END_POINT_X"] = dt.Rows[rowinc - 1]["CSD_END_POINT_X"];
        //                                                    dt.Rows[rowinc]["CSD_END_POINT_Y"] = dt.Rows[rowinc - 1]["CSD_END_POINT_Y"];
        //                                                    count++;
        //                                                }
        //                                            }
        //                                            else
        //                                            {
        //                                                if (!(dt.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("HEIGHT")) && !(dt.Rows[rowinc]["CSD_TYPE"].ToString().ToUpper().Equals("OFFSET LENGTH")))
        //                                                {
        //                                                    arr[rowinc, 0] = startX + (Convert.ToInt32(dt.Rows[rowinc]["CSD_END_POINT_X"].ToString()) - startX);
        //                                                    arr[rowinc, 1] = startY + (Convert.ToInt32(dt.Rows[rowinc]["CSD_END_POINT_Y"].ToString()) - startY);
        //                                                    count++;
        //                                                }
        //                                            }
        //                                        }
        //                                        break;
        //                                    }
        //                                }
        //                            }
        //                            //Get the minimum X cordinate.
        //                            int minX = 25000;
        //                            for (int i = 0; i < dt.Rows.Count; i++)
        //                            {
        //                                if (arr[i, 0] < minX)
        //                                {
        //                                    minX = arr[i, 0];
        //                                }
        //                            }
        //                            if (minX > startX)
        //                            {
        //                                minX = startX;
        //                            }
        //                            //Get the maximum X cordinate.
        //                            int maxX = 0;
        //                            for (int i = 0; i < dt.Rows.Count; i++)
        //                            {
        //                                if (arr[i, 0] > maxX)
        //                                {
        //                                    maxX = arr[i, 0];
        //                                }
        //                            }
        //                            if (maxX < startX)
        //                            {
        //                                maxX = startX;
        //                            }
        //                            //Get the minimum Y cordinate
        //                            int minY = 25000;
        //                            for (int i = 0; i < dt.Rows.Count; i++)
        //                            {
        //                                if (arr[i, 1] < minY)
        //                                {
        //                                    minY = arr[i, 1];
        //                                }
        //                            }
        //                            if (minY > startY)
        //                            {
        //                                minY = startY;
        //                            }
        //                            //Get the maximum Y cordinate
        //                            int maxY = 0;
        //                            for (int i = 0; i < dt.Rows.Count; i++)
        //                            {
        //                                if (arr[i, 1] > maxY)
        //                                {
        //                                    maxY = arr[i, 1];
        //                                }
        //                            }
        //                            if (maxY < startY)
        //                            {
        //                                maxY = startY;
        //                            }
        //                            //Set the values.
        //                            envLength = Convert.ToInt32((maxX - minX) * 10);
        //                            envWidth = Convert.ToInt32((maxY - minY) * 10);
        //                            if (envWidth == 0)
        //                            {
        //                                envWidth = dia;
        //                            }
        //                            envHeight = dia;
        //                        }
        //                        else
        //                        {
        //                            //Code here for 3D.
        //                        }
        //                        dt.Dispose();
        //                        Array.Clear(arr, 0, arr.Length);
        //                    }
        //                }
        //                else
        //                {
        //                    int count = (from param in ShapeParametersList
        //                                 where param.ParameterName != "" && param.ParameterName != string.Empty
        //                                 select param).Count();
        //                    string[,] arr = new string[count + 2, 2];
        //                    int j = 0;
        //                    ShapeParametersList = ShapeParametersList.OrderBy(O => O.ParameterName).ToList();
        //                    for (int i = 0; i <= ShapeParametersList.Count - 1; i++)
        //                    {
        //                        if (ShapeParametersList[i].ParameterName != "" && ShapeParametersList[i].ParameterName != string.Empty)
        //                        {
        //                            arr[j, 0] = ShapeParametersList[i].ParameterName.ToString();
        //                            arr[j, 1] = ShapeParametersList[i].ParameterValueCab.ToString();
        //                            j++;
        //                        }
        //                    }
        //                    if (dia != 0)
        //                    {
        //                        arr[count, 0] = "$";
        //                        arr[count, 1] = dia.ToString();
        //                    }
        //                    if (this.PinSizeID != 0)
        //                    {
        //                        arr[count + 1, 0] = "@";
        //                        arr[count + 1, 1] = this.PinSizeID.ToString();
        //                    }
        //                    //Call math parser class to execute the formula.
        //                    MathParser objMath = new MathParser();
        //                    string envLengthFormula = ds.Tables[1].Rows[0]["ENV_LENGTH"].ToString();
        //                    envLength = Convert.ToInt32(objMath.Evaluate(envLengthFormula, arr));
        //                    string envWidthFormula = ds.Tables[1].Rows[0]["ENV_WIDTH"].ToString();
        //                    envWidth = Convert.ToInt32(objMath.Evaluate(envWidthFormula, arr));
        //                    string envHeightFormula = ds.Tables[1].Rows[0]["ENV_HEIGHT"].ToString();
        //                    envHeight = Convert.ToInt32(objMath.Evaluate(envHeightFormula, arr));
        //                    objMath = null;
        //                    Array.Clear(arr, 0, arr.Length);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        errorMsg = ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        
        //        ds.Dispose();
        //        Array.Clear(arrHeight, 0, arrHeight.Length);
        //    }
        //}

        ///// <summary>
        ///// Method to check and get the pin at edit time.
        ///// </summary>
        ///// <param name="grade"></param>
        ///// <param name="dia"></param>
        ///// <param name="pin"></param>
        ///// <param name="errorMsg"></param>
        ///// <param name="minLength"></param>
        ///// <param name="minHookLen"></param>
        ///// <param name="minHookHt"></param>
        ///// <returns></returns>
        //public bool GetPin(string grade, int dia, int pin, out string errorMsg, out int minLength, out int minHookLen, out int minHookHt)
        //{
        //    OESDBManager dbManager = new OESDBManager();
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        errorMsg = "";
        //        int count = 0;
        //        minLength = 0;
        //        minHookLen = 0;
        //        minHookHt = 0;

        //        sqlConnection.Open();
        //        dbManager.CreateParameters(3);
        //        dynamicParameters.Add(0, "@INTDIA", dia);
        //        dynamicParameters.Add(1, "@GRADE", grade);
        //        dynamicParameters.Add(2, "@PIN", pin);
        //        ds = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "USP_VALIDATE_CABPIN_CUBE");
        //        if (ds.Tables.Count != 0)
        //        {
        //            foreach (DataRowView drCABPin in ds.Tables[0].DefaultView)
        //            {
        //                count = Convert.ToInt32(drCABPin[0].ToString());
        //                minLength = Convert.ToInt32(drCABPin[1].ToString());
        //                minHookLen = Convert.ToInt32(drCABPin[2].ToString());
        //                minHookHt = Convert.ToInt32(drCABPin[3].ToString());
        //            }
        //        }
        //        if (count > 0)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        errorMsg = ex.Message.ToString();
        //        minLength = 0;
        //        minHookLen = 0;
        //        minHookHt = 0;
        //        return false;
        //    }
        //    finally
        //    {
        //        
        //        ds.Dispose();
        //    }
        //}

        ///// <summary>
        ///// Get the pin for straight bar and other shape codes
        ///// </summary>
        ///// <param name="grade"></param>
        ///// <param name="dia"></param>
        ///// <param name="groupMarkId"></param>
        ///// <param name="productTypeId"></param>
        ///// <param name="shapeCode"></param>
        ///// <param name="errorMsg"></param>
        ///// <param name="pin"></param>
        ///// <param name="minLength"></param>
        ///// <param name="minHookLen"></param>
        ///// <param name="minHookHt"></param>
        //public void GetPin(string grade, int dia, int groupMarkId, int productTypeId, string shapeCode, out string errorMsg, out int pin, out int minLength, out int minHookLen, out int minHookHt)
        //{
        //    OESDBManager dbManager = new OESDBManager();
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        errorMsg = "";
        //        pin = 0;
        //        minLength = 0;
        //        minHookLen = 0;
        //        minHookHt = 0;

        //        sqlConnection.Open();
        //        dbManager.CreateParameters(5);
        //        dynamicParameters.Add(0, "@INTDIA", dia);
        //        dynamicParameters.Add(1, "@GRADE", grade);
        //        dynamicParameters.Add(2, "@INTGROUPMARKID", groupMarkId);
        //        dynamicParameters.Add(3, "@INTPRODUCTTYPEID", productTypeId);
        //        dynamicParameters.Add(4, "@SHAPECODE", shapeCode.Trim());
        //        ds = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "USP_GET_PIN_BYSHAPE_CUBE");
        //        if (ds.Tables.Count != 0)
        //        {
        //            foreach (DataRowView drCABPin in ds.Tables[0].DefaultView)
        //            {
        //                pin = Convert.ToInt32(drCABPin[0].ToString());
        //                minLength = Convert.ToInt32(drCABPin[1].ToString());
        //                minHookLen = Convert.ToInt32(drCABPin[2].ToString());
        //                minHookHt = Convert.ToInt32(drCABPin[3].ToString());
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        errorMsg = ex.Message.ToString();
        //        pin = 0;
        //        minLength = 0;
        //        minHookLen = 0;
        //        minHookHt = 0;
        //    }
        //    finally
        //    {
        //        
        //        ds.Dispose();
        //    }
        //}

        ///// <summary>
        ///// Method to delete the CAB details saved by group mark id.
        ///// </summary>
        ///// <param name="groupMarkId"></param>
        ///// <param name="errorMessage"></param>
        ///// <returns></returns>
        //public bool DeleteCabDetails(int groupMarkId, out string errorMessage)
        //{
        //    bool isSuccess = false;
        //    OESDBManager dbManager = new OESDBManager();
        //    int count = 0;
        //    errorMessage = "";
        //    try
        //    {
        //        sqlConnection.Open();
        //        dbManager.CreateParameters(1);
        //        dynamicParameters.Add(0, "@INTGROUPMARKID", groupMarkId);
        //        count = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, "USP_CABDETAILS_DELETE_CUBE");

        //        if (count > 0)
        //        {
        //            isSuccess = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        
        //    }
        //    return isSuccess;
        //}

        ///// <summary>
        ///// Method to get production and invoice length.
        ///// </summary>
        ///// <param name="errorMsg"></param>
        ///// <param name="invoiceLength"></param>
        ///// <param name="prodLength"></param>
        ///// <param name="r_intTotBend"></param>
        ///// <param name="r_intTotArc"></param>
        ///// <returns></returns>
        //public List<Accessory> GetProdInvLength(out string errorMsg, out int invoiceLength, out int prodLength, out int r_intTotBend, out int r_intTotArc, out string bvbs)
        //{
        //    List<Accessory> accList = new List<Accessory>();
        //    try
        //    {
        //        errorMsg = "";
        //        invoiceLength = 0;
        //        prodLength = 0;
        //        r_intTotBend = 0;
        //        r_intTotArc = 0;
        //        bvbs = "";

        //        CABCalculation objCabValidation = new CABCalculation();
        //        objCabValidation.ShapeId = this.Shape.ShapeID.ToString();
        //        objCabValidation.ShapeCode = this.ShapeCode;
        //        objCabValidation.dia = this.Diameter.ToString();
        //        objCabValidation.pin = this.PinSizeID.ToString();

        //        objCabValidation.ShapeParametersList = this.ShapeParametersList;
        //        accList = objCabValidation.ProductionInvLength(out errorMsg, out invoiceLength, out prodLength, out r_intTotBend, out r_intTotArc, out bvbs);
        //        //Round up the values to 5.
        //        if (prodLength != 0)
        //        {
        //            if (prodLength % 5 != 0)
        //            {
        //                prodLength = prodLength + 5 - (prodLength % 5);
        //            }
        //        }
        //        return accList;
        //    }
        //    catch (Exception ex)
        //    {
        //        errorMsg = ex.Message.ToString();
        //        invoiceLength = 0;
        //        prodLength = 0;
        //        r_intTotBend = 0;
        //        r_intTotArc = 0;
        //        bvbs = "";
        //        return null;
        //    }
        //}

        ///// <summary>
        ///// Method to get the invoice and production weight.
        ///// </summary>
        ///// <param name="dia"></param>
        ///// <param name="quantity"></param>
        ///// <param name="unitquantity"></param>
        ///// <param name="invoiceLength"></param>
        ///// <param name="prodLength"></param>
        //public void GetProdInvWeight(int dia, int quantity, int unitquantity, int invoiceLength, int prodLength)
        //{
        //    try
        //    {
        //        double linearWt = 0;
        //        //Get the coupler related info in list object
        //        var assembly = Assembly.GetExecutingAssembly();
        //        Stream stream = assembly.GetManifestResourceStream(this.GetType(), "LinearWeight.xml");
        //        XDocument doc1 = XDocument.Load(stream);
        //        List<LinearWeight> lstLinearWeight = (from l in doc1.Descendants("LinearWeight")
        //                                              select new LinearWeight
        //                                              {
        //                                                  Diameter = l.Element("DIAMETER").Value,
        //                                                  Linear_Weight = l.Element("LINEAR_WEIGHT").Value,
        //                                              }).ToList<LinearWeight>();

        //        //Get the coupler master values by the coupler type selected
        //        var linearWeightVar = (from l in lstLinearWeight where l.Diameter.Trim().Equals(dia.ToString()) select l).ToList();
        //        if (linearWeightVar.Count > 0)
        //        {
        //            linearWt = Convert.ToDouble(linearWeightVar[0].Linear_Weight);
        //        }

        //        if (quantity.ToString() != "" && quantity.ToString() != string.Empty && quantity.ToString().Trim() != null && unitquantity.ToString() != "" && unitquantity.ToString() != string.Empty && unitquantity.ToString().Trim() != null)
        //        {
        //            double dblInvWt = linearWt * invoiceLength * unitquantity * Convert.ToDouble(quantity) * Math.Pow(10.0, -3.0);
        //            dblInvWt = Math.Round(dblInvWt, 3);

        //            double dblProdWt = linearWt * prodLength * unitquantity * Convert.ToDouble(quantity) * Math.Pow(10.0, -3.0);
        //            dblProdWt = Math.Round(dblProdWt, 3);

        //            this.ProductionWeight = dblProdWt;
        //            this.InvoiceWeight = dblInvWt;
        //        }
        //        lstLinearWeight.Clear();
        //        doc1 = null;
        //        stream.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        ///// <summary>
        ///// To Insert & Update CAB Product Details.
        ///// </summary>
        ///// <param name="errMsg"></param>
        ///// <returns></returns>
        //public int InsertUpdCABAccessories(out string errMsg)
        //{
        //    OESDBManager dbManager = new OESDBManager();
        //    string strCABProductMarkID = string.Empty;
        //    DataSet dsCABProductMarkID = new DataSet();
        //    int intCABProductMarkID = 0;
        //    int ShapeTransHeaderId = 0;
        //    string err = string.Empty;
        //    try
        //    {
        //        bool flag = ValidateCabDetailVals(out err);
        //        if (flag == true)
        //        {
        //            //Get the envelope length, width and height.
        //            int envl = 0;
        //            int envw = 0;
        //            int envH = 0;
        //            EnvelopeCalculation(this.ShapeCode, this.ShapeParametersList, this.Diameter, out envl, out envw, out envH, out errMsg);
        //            this.EnvLength = envl;
        //            this.EnvWidth = envw;
        //            this.EnvHeight = envH;
        //            //Handle varies bar
        //            if (this.IsVariableBar == null)
        //            {
        //                this.IsVariableBar = false;
        //            }
        //            //Insert the product mark data to database.
        //            sqlConnection.Open();
        //            dbManager.CreateParameters(32);
        //            dynamicParameters.Add(0, "@VCHCABPRODUCTMARKNAME", this.CABProductMarkName.Trim());
        //            dynamicParameters.Add(1, "@INTSEDETAILINGID", this.intSEDetailingId);
        //            dynamicParameters.Add(2, "@INTGROUPMARKID", this.GroupMarkId);
        //            dynamicParameters.Add(3, "@INTMEMBERQTY", this.Quantity);
        //            dynamicParameters.Add(4, "@INTSHAPECODE", this.ShapeCode.Trim());
        //            dynamicParameters.Add(5, "@INTPINSIZEID", this.PinSizeID);
        //            dynamicParameters.Add(6, "@NUMINVOICELENGTH", this.InvoiceLength);
        //            dynamicParameters.Add(7, "@NUMPRODUCTIONLENGTH", this.ProductionLength);
        //            dynamicParameters.Add(8, "@NUMINVOICEWEIGHT", this.InvoiceWeight);
        //            dynamicParameters.Add(9, "@NUMPRODUCTIONWEIGHT", this.ProductionWeight);
        //            dynamicParameters.Add(10, "@GRADE", this.Grade);
        //            dynamicParameters.Add(11, "@INTDIAMETER", this.Diameter);
        //            dynamicParameters.Add(12, "@VCHSHAPETYPE", this.ShapeType);
        //            dynamicParameters.Add(13, "@VCHSHAPEGROUP", this.ShapeGroup);
        //            dynamicParameters.Add(14, "@INTSTATUS", this.Status);
        //            dynamicParameters.Add(15, "@VCHCUSTREMARKS", this.CustomerRemarks);
        //            dynamicParameters.Add(16, "@VCHSHAPEIMAGE", this.ShapeImage);
        //            dynamicParameters.Add(17, "@VCHCAB_BVBS", this.BVBS);
        //            dynamicParameters.Add(18, "@VCHPAGENUMBER", this.PageNumber);
        //            dynamicParameters.Add(19, "@VCHCOMMDESCRIPT", this.CommercialDesc);

        //            dynamicParameters.Add(20, "@NUMENVLENGTH", this.EnvLength);
        //            dynamicParameters.Add(21, "@NUMENVWIDTH", this.EnvWidth);
        //            dynamicParameters.Add(22, "@NUMENVHEIGHT", this.EnvHeight);
        //            dynamicParameters.Add(23, "@INTNOOFBENDS", this.NoOfBends);

        //            if (accList.Count == 0)
        //            {
        //                dynamicParameters.Add(24, "@BARSTANDARD", "");
        //                dynamicParameters.Add(25, "@COUPLERTYPE1", "");
        //                dynamicParameters.Add(26, "@COUPLERMATERIAL1", "");
        //                dynamicParameters.Add(27, "@COUPLERSTANDARD1", "");
        //                dynamicParameters.Add(28, "@COUPLERTYPE2", "");
        //                dynamicParameters.Add(29, "@COUPLERMATERIAL2", "");
        //                dynamicParameters.Add(30, "@COUPLERSTANDARD2", "");
        //            }
        //            else
        //            {
        //                dynamicParameters.Add(24, "@BARSTANDARD", this.Coupler1Standard);
        //                dynamicParameters.Add(25, "@COUPLERTYPE1", this.Coupler1Type);
        //                dynamicParameters.Add(26, "@COUPLERMATERIAL1", this.Coupler1);
        //                dynamicParameters.Add(27, "@COUPLERSTANDARD1", this.Coupler1Standard);
        //                dynamicParameters.Add(28, "@COUPLERTYPE2", this.Coupler2Type);
        //                dynamicParameters.Add(29, "@COUPLERMATERIAL2", this.Coupler2);
        //                dynamicParameters.Add(30, "@COUPLERSTANDARD2", this.Coupler2Standard);
        //            }
        //            dynamicParameters.Add(31, "@BITVARIESBAR", this.IsVariableBar);
        //            dsCABProductMarkID = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "USP_CAB_PRODUCTMARKINGDETAILS_INSUPD_CUBE");
        //            if (dsCABProductMarkID.Tables.Count != 0)
        //            {
        //                foreach (DataRowView drCABProductMarkID in dsCABProductMarkID.Tables[0].DefaultView)
        //                {
        //                    intCABProductMarkID = Convert.ToInt32(drCABProductMarkID[0].ToString());
        //                    ShapeTransHeaderId = Convert.ToInt32(drCABProductMarkID[1].ToString());
        //                }
        //            }
        //            if (intCABProductMarkID != 0)
        //            {
        //                InsertUpdAccessories(intCABProductMarkID, intSEDetailingId);
        //                ShapeParametersList = ShapeParametersList.OrderBy(x => x.SequenceNumber).ToList<ShapeParameter>();
        //                foreach (ShapeParameter shapeItem in ShapeParametersList)
        //                {
        //                    shapeItem.Parameter = shapeItem.ParameterName;
        //                    shapeItem.Value = shapeItem.ParameterValueCab;
        //                    shapeItem.SequenceNumber = shapeItem.SequenceNumber;
        //                    if (shapeItem.VisibleFlag == true)
        //                    {
        //                        shapeItem.PrintValue = 1;
        //                    }
        //                    else
        //                    {
        //                        shapeItem.PrintValue = 0;
        //                    }
        //                    shapeItem.Save(ShapeTransHeaderId);
        //                }
        //            }
        //            
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        errMsg = ex.Message.ToString();
        //        throw ex;
        //    }
        //    finally
        //    {
        //        errMsg = err;
        //    }
        //    return intCABProductMarkID;
        //}

        ///// <summary>
        ///// Supporting method for validation.
        ///// </summary>
        ///// <param name="errorMsg"></param>
        ///// <returns></returns>
        //public bool ValidateCabDetailVals(out string errorMsg)
        //{
        //    //bool inputDatavalid = false;
        //    bool inputShapevalid = false;
        //    try
        //    {
        //        CABValidation objCabValidation = new CABValidation();
        //        objCabValidation.intSEDetailingId = this.intSEDetailingId;
        //        objCabValidation.ProduceInd = this.Status;
        //        objCabValidation.Prefix = this.CommercialDesc;
        //        objCabValidation.PageNo = this.PageNumber;
        //        objCabValidation.BarMarkId = this.BarMark;
        //        objCabValidation.ShapeId = this.Shape.ShapeID.ToString();
        //        objCabValidation.ShapeCode = this.ShapeCode;
        //        objCabValidation.Grade = this.Grade;
        //        objCabValidation.Dia = this.Diameter.ToString();
        //        objCabValidation.Quantity = this.Quantity.ToString();
        //        objCabValidation.UnitQuantity = this.Quantity.ToString();
        //        objCabValidation.Pin = this.PinSizeID.ToString();
        //        objCabValidation.InvoiceLength = this.InvoiceLength.ToString();
        //        objCabValidation.ProdnLength = this.ProductionLength.ToString();
        //        objCabValidation.InvoiceWeight = this.InvoiceWeight.ToString();
        //        objCabValidation.ProdnWeight = this.ProductionWeight.ToString();
        //        objCabValidation.OperationType = "ADD";

        //        objCabValidation.ShapeParametersList = this.ShapeParametersList;

        //        string errMsg = string.Empty;
        //        string message = string.Empty;
        //        inputShapevalid = objCabValidation.DetailingShapeDataValidation(out message);
        //        if (inputShapevalid == true)
        //        {
        //            errorMsg = "";
        //            return true;
        //        }
        //        else
        //        {
        //            errorMsg = message;
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        errorMsg = ex.Message.ToString();
        //        return false;
        //    }
        //}

        ///// <summary>
        ///// To Insert & Update Accessories Product Details.
        ///// </summary>
        ///// <param name="intCABProductMarkID"></param>
        ///// <param name="intSEDetailingId"></param>
        ///// <returns></returns> 
        //private int InsertUpdAccessories(int intCABProductMarkID, int intSEDetailingId)
        //{
        //    OESDBManager dbManager = new OESDBManager();
        //    int intOutput = 0;
        //    int count = 1;
        //    try
        //    {
        //        //Get only the coupler materials from accessories list
        //        List<Accessory> lstCoupler = (from item in accList
        //                                      where item.MaterialType == "COUPLER_MATERIAL"
        //                                      select item).ToList<Accessory>();
        //        sqlConnection.Open();
        //        //Loop through accessory list for distinct coupler type.
        //        foreach (Accessory couplerAccItem in lstCoupler)
        //        {
        //            //Loop through the whole accessories list to add coupler, thread and lock nut material
        //            foreach (Accessory accItem1 in accList)
        //            {
        //                if (accItem1.CouplerType.Equals(couplerAccItem.CouplerType))
        //                {
        //                    string accProductMarkName = string.Empty;
        //                    if (accItem1.MaterialType.Equals("COUPLER_MATERIAL"))
        //                    {
        //                        accProductMarkName = this.CABProductMarkName + "-C" + count.ToString();
        //                    }
        //                    else if (accItem1.MaterialType.Equals("THREAD"))
        //                    {
        //                        accProductMarkName = this.CABProductMarkName + "-T" + count.ToString();
        //                    }
        //                    else if (accItem1.MaterialType.Equals("LOCK_NUT"))
        //                    {
        //                        accProductMarkName = this.CABProductMarkName + "-L" + count.ToString();
        //                    }
        //                    dbManager.CreateParameters(13);
        //                    dynamicParameters.Add(0, "@VCHACCPRODUCTMARKINGNAME", accProductMarkName);
        //                    dynamicParameters.Add(1, "@INTQTY", this.Quantity);
        //                    dynamicParameters.Add(2, "@INTSEDETAILING", intSEDetailingId);
        //                    dynamicParameters.Add(3, "@INTNOOFPIECES", this.Quantity);
        //                    dynamicParameters.Add(4, "@VCHSAPCODE", accItem1.SAPMaterialCode);
        //                    dynamicParameters.Add(5, "@VCHCABPRODUCTMARKID", intCABProductMarkID);
        //                    dynamicParameters.Add(6, "@BITISCOUPLER", accItem1.BitIsCoupler);
        //                    dynamicParameters.Add(7, "@NUMINVOICEWEIGHTPERPC", "0.000");
        //                    dynamicParameters.Add(8, "@NUMEXTERNALWIDTH", "0");
        //                    dynamicParameters.Add(9, "@NUMEXTERNALHEIGHT", "0");
        //                    dynamicParameters.Add(10, "@NUMEXTERNALLENGTH", "0");
        //                    dynamicParameters.Add(11, "@NUMLENGTH", "0");
        //                    dynamicParameters.Add(12, "@INTGROUPMARKID", this.GroupMarkId);
        //                    intOutput = Convert.ToInt32(dbManager.ExecuteScalar(CommandType.StoredProcedure, "USP_CABACC_PRODUCTMARKINGDETAILS_INSUPD_CUBE"));
        //                }
        //            }
        //            count++;
        //        }
        //        lstCoupler.Clear();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        
        //    }
        //    return intOutput;
        //}

        //#endregion

        //#region TCT and OES
        ///// <summary>
        ///// Method for TCT and OES to calculate and save to database.
        ///// </summary>
        ///// <returns></returns>
        //public bool InsertToDbTCT()
        //{
        //    try
        //    {
        //        //Variable declaration.
        //        bool flag = true;
        //        string errMsg = string.Empty;
        //        int pin = 0;
        //        int minLength = 0;
        //        int minHookLen = 0;
        //        int minHookHt = 0;
        //        int prodLen = 0;
        //        int invLen = 0;
        //        int totBend = 0;
        //        int totArc = 0;
        //        string bvbs = string.Empty;
        //        int productTypeId = 4;

        //        //Calculate the pin.
        //        if (this.PinSizeID == 0)
        //        {
        //            GetPin(this.Grade.ToUpper().Trim(), this.Diameter, this.GroupMarkId, productTypeId, this.Shape.ShapeCodeName.Trim(), out errMsg, out pin, out minLength, out minHookLen, out minHookHt);
        //            this.PinSizeID = pin;
        //        }

        //        //Calculate production, invoice length.
        //        this.accList = GetProdInvLength(out errMsg, out invLen, out prodLen, out totBend, out totArc, out bvbs);
        //        this.ProductionLength = prodLen;
        //        this.InvoiceLength = invLen;
        //        this.NoOfBends = totBend;
        //        this.BVBS = bvbs;

        //        //Get production, invoice weight.
        //        GetProdInvWeight(this.Diameter, this.Quantity, 1, Convert.ToInt32(this.InvoiceLength), Convert.ToInt32(this.ProductionLength));

        //        //Set the coupler parameters
        //        if (this.accList != null)
        //        {
        //            if (this.accList.Count == 0)
        //            {
        //                this.Coupler1Standard = "";
        //                this.Coupler1Type = "";
        //                this.Coupler1 = "";
        //                this.Coupler2Type = "";
        //                this.Coupler2 = "";
        //                this.Coupler2Standard = "";
        //            }
        //            else if (this.accList.Count > 0)
        //            {
        //                //Get only the coupler materials from accessories list
        //                List<Accessory> lstCoupler = (from item in this.accList
        //                                              where item.MaterialType == "COUPLER_MATERIAL"
        //                                              select item).ToList<Accessory>();

        //                if (lstCoupler.Count == 1)
        //                {
        //                    int counter = 0;
        //                    foreach (Accessory acc in lstCoupler)
        //                    {
        //                        if (counter == 0)
        //                        {
        //                            this.Coupler1Standard = acc.standard.ToString();
        //                            this.Coupler1Type = acc.CouplerType.ToString();
        //                            this.Coupler1 = acc.SAPMaterialCode.ToString();
        //                            this.Coupler2Type = "";
        //                            this.Coupler2 = "";
        //                            this.Coupler2Standard = "";
        //                        }
        //                        counter++;
        //                    }
        //                }
        //                else if (lstCoupler.Count == 2)
        //                {
        //                    int counter = 0;
        //                    foreach (Accessory acc in lstCoupler)
        //                    {
        //                        if (counter == 0)
        //                        {
        //                            this.Coupler1Standard = acc.standard.ToString();
        //                            this.Coupler1Type = acc.CouplerType.ToString();
        //                            this.Coupler1 = acc.SAPMaterialCode.ToString();
        //                        }
        //                        if (counter == 1)
        //                        {
        //                            this.Coupler2Type = acc.CouplerType.ToString();
        //                            this.Coupler2 = acc.SAPMaterialCode.ToString();
        //                            this.Coupler2Standard = acc.standard.ToString();
        //                        }
        //                        counter++;
        //                    }
        //                }
        //                //Clear the accessories list.
        //                lstCoupler.Clear();
        //            }
        //        }
        //        else
        //        {
        //            this.Coupler1Standard = "";
        //            this.Coupler1Type = "";
        //            this.Coupler1 = "";
        //            this.Coupler2Type = "";
        //            this.Coupler2 = "";
        //            this.Coupler2Standard = "";
        //        }

        //        //Inserrt to Db.
        //        InsertUpdCABAccessories(out errMsg);
        //        return flag;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        ///// <summary>
        ///// Method to save detailing data from parallel table to the new table.
        ///// </summary>
        ///// <param name="groupMarkId"></param>
        ///// <param name="errorMsg"></param>
        ///// <returns></returns>
        //public bool ValidateAndSave(int groupMarkId, out string errorMsg)
        //{
        //    OESDBManager dbManager = new OESDBManager();
        //    errorMsg = "";
        //    try
        //    {
        //        sqlConnection.Open();
        //        dbManager.CreateParameters(1);
        //        dynamicParameters.Add(0, "@GROUPMARKID", groupMarkId);
        //        dbManager.ExecuteNonQuery(CommandType.StoredProcedure, "USP_SAVE_SHAPETRANSDETAILS_INSERT");
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        errorMsg = ex.Message;
        //        return false;
        //    }
        //    finally
        //    {
        //        
        //    }
        //}

        ///// <summary>
        ///// Method to get the WBS details from SOR range.
        ///// </summary>
        ///// <param name="fromSor"></param>
        ///// <param name="toSor"></param>
        ///// <returns></returns>
        public List<LoadSORDto>GetBBSPostingCABRange(string fromSor, string toSor)
        {
            DataSet ds = new DataSet();
            
            try
            {
                IEnumerable<LoadSORDto> loadSORDtos;
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@ORD_REQ_NO_FROM", fromSor);
                    dynamicParameters.Add("@ORD_REQ_NO_TO", toSor);
                    loadSORDtos = sqlConnection.Query<LoadSORDto>(SystemConstants.USP_GET_BBSPOSTING_RANGE_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return loadSORDtos.ToList();

                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
               
            }
        }

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
        //    OESDBManager dbManager = new OESDBManager();
        //    int count = 0;
        //    try
        //    {
        //        sqlConnection.Open();
        //        dbManager.CreateParameters(5);
        //        dynamicParameters.Add(0, "@intProjectID", intProjectId);
        //        dynamicParameters.Add(1, "@intWBSElementID", intWBSElementId);
        //        dynamicParameters.Add(2, "@vchStructureElementName", vchStructureElementType);
        //        dynamicParameters.Add(3, "@vchGroupMarkingName", BBS_NO);
        //        dynamicParameters.Add(4, "@intUserId", intUserid);
        //        count = Convert.ToInt32(dbManager.ExecuteScalar(CommandType.StoredProcedure, "GET_GROUPMARKID_CUBE"));
        //        return count;
        //    }
        //    catch (Exception ex)
        //    {
        //        return 0;
        //    }
        //    finally
        //    {
        //        
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
        //    OESDBManager dbManager = new OESDBManager();
        //    int count = 0;
        //    try
        //    {
        //        sqlConnection.Open();
        //        dbManager.CreateParameters(1);
        //        dynamicParameters.Add(0, "@vchLoginId", vchLoginId);
        //        int userId = Convert.ToInt32(dbManager.ExecuteScalar(CommandType.StoredProcedure, "USERRIGHTS_VALIDATE_CUBE"));
        //        return userId;
        //    }
        //    catch (Exception ex)
        //    {
        //        return 0;
        //    }
        //    finally
        //    {
        //        
        //    }
        //}

        ///// <summary>
        ///// Method to get all the calculated parameter values.
        ///// </summary>
        //public void ValidateShapeParameters(out string errorMsg)
        //{
        //    int paramselected = 0;
        //    errorMsg = "";
        //    try
        //    {
        //        #region Leg value handling
        //        //Set the C and D parameter value if shape code is 061 and 079
        //        if (this.ShapeCode.ToUpper() == "061" || this.ShapeCode.ToUpper() == "079")
        //        {
        //            foreach (ShapeParameter s in this.ShapeParametersList)
        //            {
        //                if (s.ParameterName == "C" || s.ParameterName == "D")
        //                {
        //                    if (this.Diameter != 0)
        //                    {
        //                        //Set the value on the shape parameter grid.
        //                        if (this.Grade.ToUpper().Equals("H"))
        //                        {
        //                            if (s.ParameterValueCab.Equals("0"))
        //                            {
        //                                //Set the value to shape parameter observable collection.
        //                                s.ParameterValueCab = (13 * this.Diameter).ToString().Trim();
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (s.ParameterValueCab.Equals("0"))
        //                            {
        //                                //Set the value to shape parameter observable collection.
        //                                s.ParameterValueCab = (12 * this.Diameter).ToString().Trim();
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        #endregion

        //        #region Height/Offset & Angle
        //        string[] arr = { "OFF-NOR-HEIGHT", "OFF-NOR-OFFSET" };
        //        foreach (ShapeParameter s in this.ShapeParametersList)
        //        {
        //            string nextToAngleParam = string.Empty;
        //            string prevToAngleParam = string.Empty;
        //            double[] arrResult = { 0, 0 };
        //            double[] resultOffset = { 0, 0, 0, 0 };
        //            if (s.AngleType.Equals("ANGLE"))
        //            {
        //                //If angle is preceeding angle check for sequence+2 componet for length type.
        //                var nextAngleParam = (from item1 in this.ShapeParametersList
        //                                      where item1.SequenceNumber == s.SequenceNumber + 2
        //                                      select item1.AngleType).ToList();
        //                if (nextAngleParam.Count > 0)
        //                {
        //                    nextToAngleParam = nextAngleParam[0].ToString();
        //                }
        //                //If angle is succeding angle check for sequence+2 componet for length type.
        //                var prevAngleParam = (from item1 in this.ShapeParametersList
        //                                      where item1.SequenceNumber == s.SequenceNumber - 1
        //                                      select item1.AngleType).ToList();
        //                if (prevAngleParam.Count > 0)
        //                {
        //                    prevToAngleParam = prevAngleParam[0].ToString();
        //                }
        //            }
        //            if ((s.AngleType.Equals("ANGLE") && Array.IndexOf(arr, nextToAngleParam) < 0 && Array.IndexOf(arr, prevToAngleParam) < 0) || s.AngleType.Equals("HEIGHT") || s.AngleType.Equals("OFFSET"))
        //            {
        //                #region If param is Angle/Height/Offset- Calculate and Set values
        //                //Get the formula to calculate angle or height value.
        //                var formula = (from item1 in this.ShapeParametersList
        //                               where item1.SequenceNumber == s.SequenceNumber
        //                               select item1).ToList();
        //                //Calculate the result from the formula
        //                if (formula.Count > 0)
        //                {
        //                    //Calculate the value from the first formula.
        //                    if (formula[0].HeghtAngleFormula != "0")
        //                    {
        //                        arrResult[0] = CalculateFormulaString(this.ShapeParametersList, formula[0].HeghtAngleFormula, "HEIGHT", this.Diameter, this.PinSizeID);
        //                        //Section for iteration method
        //                        if (s.AngleType.Equals("HEIGHT"))
        //                        {
        //                            var formulaGreaterThan90 = (from item1 in this.ShapeParametersList
        //                                                        where item1.SequenceNumber == s.SequenceNumber - 2
        //                                                        select item1).ToList();
        //                            List<valuePair> valueList = new List<valuePair>();
        //                            foreach (ShapeParameter sParam in this.ShapeParametersList)
        //                            {
        //                                if (sParam.ParameterName != "" && sParam.ParameterName != string.Empty && sParam.ParameterName != null)
        //                                {
        //                                    if (!sParam.AngleType.Equals("ESPLICE") && !sParam.AngleType.Equals("NSPLICE"))
        //                                    {
        //                                        valueList.Add(new valuePair(sParam.ParameterName, Convert.ToDouble(sParam.ParameterValueCab)));
        //                                    }
        //                                }
        //                            }
        //                            var valuePair = (from item1 in valueList
        //                                             where item1._key == formulaGreaterThan90[0].ParameterName
        //                                             select item1).ToList();
        //                            if (formulaGreaterThan90.Count > 0)
        //                            {
        //                                if (formulaGreaterThan90[0].HeghtAngleFormula.Contains("$") && formulaGreaterThan90[0].HeghtAngleFormula.Contains("@"))
        //                                {
        //                                    //Iteration method to get the correct angle value					
        //                                    double resultAngle = 0;
        //                                    double minHtDiff = 0;
        //                                    double approxRes = arrResult[0];

        //                                    //Added for issue where during iteration angle goes below 90
        //                                    double ParameterValueCab = 0;

        //                                    if (string.IsNullOrEmpty(formulaGreaterThan90[0].ParameterValueCab))
        //                                        ParameterValueCab = 0;
        //                                    else
        //                                        ParameterValueCab = Convert.ToDouble(formulaGreaterThan90[0].ParameterValueCab);

        //                                    resultAngle = approxRes;
        //                                    minHtDiff = Math.Abs(Convert.ToInt32(s.ParameterValueCab.ToString()) - CalculateFormulaString(valueList, formulaGreaterThan90[0].HeghtAngleFormula, "HEIGHT", this.Diameter, this.PinSizeID));
        //                                    //end


        //                                    approxRes = approxRes - 20;
        //                                    double[,] arrHtAng = new double[9, 2];
        //                                    for (int count = 0; count < 9; count++)
        //                                    {
        //                                        if (valuePair.Count > 0)
        //                                        {
        //                                            valuePair[0]._value = Convert.ToInt32(approxRes + (count * 5));
        //                                        }
        //                                        arrHtAng[count, 0] = Convert.ToInt32(approxRes + (count * 5));
        //                                        arrHtAng[count, 1] = Math.Abs(Convert.ToInt32(s.ParameterValueCab.ToString()) - CalculateFormulaString(valueList, formulaGreaterThan90[0].HeghtAngleFormula, "HEIGHT", this.Diameter, this.PinSizeID));
        //                                        if (count > 0)
        //                                        {
        //                                            // if (arrHtAng[count, 1] < minHtDiff && arrHtAng[count, 0] > 90)//Modified for issue where during iteration angle goes below 90
        //                                            if (arrHtAng[count, 1] < minHtDiff && (ParameterValueCab < 90 || (ParameterValueCab > 90 && arrHtAng[count, 0] > 90)))
        //                                            {
        //                                                resultAngle = arrHtAng[count, 0];
        //                                                minHtDiff = arrHtAng[count, 1];
        //                                            }
        //                                        }
        //                                        else
        //                                        {
        //                                            // if (arrHtAng[count, 0] > 90)//Added for issue where during iteration angle goes below 90
        //                                            if (ParameterValueCab < 90 || (ParameterValueCab > 90 && arrHtAng[count, 0] > 90))
        //                                            {
        //                                                resultAngle = arrHtAng[count, 0];
        //                                                minHtDiff = arrHtAng[count, 1];
        //                                            }
        //                                        }
        //                                    }
        //                                    Array.Clear(arrHtAng, 0, arrHtAng.Length);

        //                                    approxRes = resultAngle;
        //                                    resultAngle = 0;
        //                                    arrHtAng = new double[11, 2];
        //                                    for (int count = 0; count < 11; count++)
        //                                    {
        //                                        if (valuePair.Count > 0)
        //                                        {
        //                                            valuePair[0]._value = Convert.ToInt32(approxRes - 5 + count);
        //                                        }
        //                                        arrHtAng[count, 0] = Convert.ToInt32(approxRes - 5 + count);
        //                                        arrHtAng[count, 1] = Math.Abs(Convert.ToInt32(s.ParameterValueCab.ToString()) - CalculateFormulaString(valueList, formulaGreaterThan90[0].HeghtAngleFormula, "HEIGHT", this.Diameter, this.PinSizeID));
        //                                        if (count > 0)
        //                                        {
        //                                            //if (arrHtAng[count, 1] < minHtDiff && arrHtAng[count, 0] > 90)//Modified for issue where during iteration angle goes below 90
        //                                            if (arrHtAng[count, 1] < minHtDiff && (ParameterValueCab < 90 || (ParameterValueCab > 90 && arrHtAng[count, 0] > 90)))
        //                                            {
        //                                                resultAngle = arrHtAng[count, 0];
        //                                                minHtDiff = arrHtAng[count, 1];
        //                                            }
        //                                        }
        //                                        else
        //                                        {
        //                                            //if (arrHtAng[count, 0] > 90)//Added for issue where during iteration angle goes below 90
        //                                            if (ParameterValueCab < 90 || (ParameterValueCab > 90 && arrHtAng[count, 0] > 90))
        //                                            {
        //                                                resultAngle = arrHtAng[count, 0];
        //                                                minHtDiff = arrHtAng[count, 1];
        //                                            }
        //                                        }
        //                                    }
        //                                    Array.Clear(arrHtAng, 0, arrHtAng.Length);
        //                                    arrResult[0] = resultAngle;
        //                                    //Iteration method to get the correct angle value
        //                                }
        //                            }
        //                        }
        //                    }
        //                    //Calculate the value from the second formula.
        //                    if (formula[0].HeghtSuceedAngleFormula != "0")
        //                    {
        //                        arrResult[1] = CalculateFormulaString(this.ShapeParametersList, formula[0].HeghtSuceedAngleFormula, "HEIGHT", this.Diameter, this.PinSizeID);
        //                        //Section for iteration method
        //                        if (s.AngleType.Equals("HEIGHT"))
        //                        {
        //                            var formulaGreaterThan90 = (from item1 in this.ShapeParametersList
        //                                                        where item1.SequenceNumber == s.SequenceNumber + 1
        //                                                        select item1).ToList();
        //                            List<valuePair> valueList = new List<valuePair>();
        //                            foreach (ShapeParameter sParam in this.ShapeParametersList)
        //                            {
        //                                if (sParam.ParameterName != "" && sParam.ParameterName != string.Empty && sParam.ParameterName != null)
        //                                {
        //                                    if (!sParam.AngleType.Equals("ESPLICE") && !sParam.AngleType.Equals("NSPLICE"))
        //                                    {
        //                                        valueList.Add(new valuePair(sParam.ParameterName, Convert.ToDouble(sParam.ParameterValueCab)));
        //                                    }
        //                                }
        //                            }
        //                            var valuePair = (from item1 in valueList
        //                                             where item1._key == formulaGreaterThan90[0].ParameterName
        //                                             select item1).ToList();
        //                            if (formulaGreaterThan90.Count > 0)
        //                            {
        //                                if (formulaGreaterThan90[0].HeghtSuceedAngleFormula.Contains("$") && formulaGreaterThan90[0].HeghtSuceedAngleFormula.Contains("@"))
        //                                {
        //                                    //Iteration method to get the correct angle value					
        //                                    double resultAngle = 0;
        //                                    double minHtDiff = 0;
        //                                    double approxRes = arrResult[1];

        //                                    //Added for issue where during iteration angle goes below 90
        //                                    double ParameterValueCab = 0;

        //                                    if (string.IsNullOrEmpty(formulaGreaterThan90[0].ParameterValueCab))
        //                                        ParameterValueCab = 0;
        //                                    else
        //                                        ParameterValueCab = Convert.ToDouble(formulaGreaterThan90[0].ParameterValueCab);

        //                                    resultAngle = approxRes;
        //                                    minHtDiff = Math.Abs(Convert.ToInt32(s.ParameterValueCab.ToString()) - CalculateFormulaString(valueList, formulaGreaterThan90[0].HeghtSuceedAngleFormula, "HEIGHT", this.Diameter, this.PinSizeID));
        //                                    //end

        //                                    approxRes = approxRes - 20;
        //                                    double[,] arrHtAng = new double[9, 2];
        //                                    for (int count = 0; count < 9; count++)
        //                                    {
        //                                        if (valuePair.Count > 0)
        //                                        {
        //                                            valuePair[0]._value = Convert.ToInt32(approxRes + (count * 5));
        //                                        }
        //                                        arrHtAng[count, 0] = Convert.ToInt32(approxRes + (count * 5));
        //                                        arrHtAng[count, 1] = Math.Abs(Convert.ToInt32(s.ParameterValueCab.ToString()) - CalculateFormulaString(valueList, formulaGreaterThan90[0].HeghtSuceedAngleFormula, "HEIGHT", this.Diameter, this.PinSizeID));
        //                                        if (count > 0)
        //                                        {
        //                                            //if (arrHtAng[count, 1] < minHtDiff && arrHtAng[count, 0] > 90)//Modified for issue where during iteration angle goes below 90
        //                                            if (arrHtAng[count, 1] < minHtDiff && (ParameterValueCab < 90 || (ParameterValueCab > 90 && arrHtAng[count, 0] > 90)))
        //                                            {
        //                                                resultAngle = arrHtAng[count, 0];
        //                                                minHtDiff = arrHtAng[count, 1];
        //                                            }
        //                                        }
        //                                        else
        //                                        {
        //                                            //if (arrHtAng[count, 0] > 90)//Added for issue where during iteration angle goes below 90
        //                                            if (ParameterValueCab < 90 || (ParameterValueCab > 90 && arrHtAng[count, 0] > 90))
        //                                            {
        //                                                resultAngle = arrHtAng[count, 0];
        //                                                minHtDiff = arrHtAng[count, 1];
        //                                            }
        //                                        }
        //                                    }
        //                                    Array.Clear(arrHtAng, 0, arrHtAng.Length);

        //                                    approxRes = resultAngle;
        //                                    resultAngle = 0;
        //                                    arrHtAng = new double[11, 2];
        //                                    for (int count = 0; count < 11; count++)
        //                                    {
        //                                        if (valuePair.Count > 0)
        //                                        {
        //                                            valuePair[0]._value = Convert.ToInt32(approxRes - 5 + count);
        //                                        }
        //                                        arrHtAng[count, 0] = Convert.ToInt32(approxRes - 5 + count);
        //                                        arrHtAng[count, 1] = Math.Abs(Convert.ToInt32(s.ParameterValueCab.ToString()) - CalculateFormulaString(valueList, formulaGreaterThan90[0].HeghtSuceedAngleFormula, "HEIGHT", this.Diameter, this.PinSizeID));
        //                                        if (count > 0)
        //                                        {
        //                                            //if (arrHtAng[count, 1] < minHtDiff && arrHtAng[count, 0] > 90)//Modified for issue where during iteration angle goes below 90
        //                                            if (arrHtAng[count, 1] < minHtDiff && (ParameterValueCab < 90 || (ParameterValueCab > 90 && arrHtAng[count, 0] > 90)))
        //                                            {
        //                                                resultAngle = arrHtAng[count, 0];
        //                                                minHtDiff = arrHtAng[count, 1];
        //                                            }
        //                                        }
        //                                        else
        //                                        {
        //                                            //if (arrHtAng[count, 0] > 90)//Added for issue where during iteration angle goes below 90
        //                                            if (ParameterValueCab < 90 || (ParameterValueCab > 90 && arrHtAng[count, 0] > 90))
        //                                            {
        //                                                resultAngle = arrHtAng[count, 0];
        //                                                minHtDiff = arrHtAng[count, 1];
        //                                            }
        //                                        }
        //                                    }
        //                                    Array.Clear(arrHtAng, 0, arrHtAng.Length);
        //                                    arrResult[1] = resultAngle;
        //                                    //Iteration method to get the correct angle value
        //                                }
        //                            }
        //                        }
        //                    }

        //                    //Calculate the value from the first formula.
        //                    if (formula[0].OffsetAngleFormula != "0")
        //                    {
        //                        arrResult[0] = CalculateFormulaString(this.ShapeParametersList, formula[0].OffsetAngleFormula, "OFFSET", this.Diameter, this.PinSizeID);
        //                    }
        //                    //Calculate the value from the second formula.
        //                    if (formula[0].OffsetSuceedAngleFormula != "0")
        //                    {
        //                        arrResult[1] = CalculateFormulaString(this.ShapeParametersList, formula[0].OffsetSuceedAngleFormula, "OFFSET", this.Diameter, this.PinSizeID);
        //                    }

        //                    if (formula[0].HeghtAngleFormula != "0" || formula[0].OffsetAngleFormula != "0")
        //                    {
        //                        if (s.AngleType.Equals("ANGLE") || s.AngleType.Equals("HEIGHT") || s.AngleType.Equals("OFFSET"))
        //                        {
        //                            for (int resultCount = 0; resultCount < arrResult.Length; resultCount++)
        //                            {
        //                                if (arrResult[resultCount] != 0)
        //                                {
        //                                    if ((nextToAngleParam.ToUpper().Equals("HEIGHT") && !prevToAngleParam.ToUpper().Equals("HEIGHT")) || (nextToAngleParam.ToUpper().Equals("OFFSET") && !prevToAngleParam.ToUpper().Equals("OFFSET")))
        //                                    {
        //                                        if (resultCount == 0)
        //                                        {
        //                                            paramselected = s.SequenceNumber + 2;
        //                                        }
        //                                        else if (resultCount == 1)
        //                                        {
        //                                            paramselected = s.SequenceNumber + 3;
        //                                        }
        //                                    }
        //                                    else if (prevToAngleParam.ToUpper().Equals("HEIGHT") || prevToAngleParam.ToUpper().Equals("OFFSET"))
        //                                    {
        //                                        if (resultCount == 0)
        //                                        {
        //                                            if (s.SequenceNumber != 4)
        //                                            {
        //                                                paramselected = s.SequenceNumber - 3;
        //                                                if (this.ShapeParametersList.FirstOrDefault(x => x.SequenceNumber == paramselected).AngleType.Equals("COUPLER_LENGTH"))
        //                                                {
        //                                                    paramselected = s.SequenceNumber - 1;
        //                                                }
        //                                            }
        //                                            else
        //                                            {
        //                                                paramselected = s.SequenceNumber - 1;
        //                                            }
        //                                        }
        //                                        else if (resultCount == 1)
        //                                        {
        //                                            paramselected = s.SequenceNumber - 1;
        //                                        }
        //                                    }
        //                                    else if (s.AngleType.Equals("HEIGHT") || s.AngleType.Equals("OFFSET"))
        //                                    {
        //                                        if (resultCount == 0)
        //                                        {
        //                                            if (s.SequenceNumber != 3)
        //                                            {
        //                                                paramselected = s.SequenceNumber - 2;
        //                                                if (this.ShapeParametersList.FirstOrDefault(x => x.SequenceNumber == paramselected).AngleType.Equals("COUPLER_LENGTH"))
        //                                                {
        //                                                    paramselected = s.SequenceNumber + 1;
        //                                                }
        //                                            }
        //                                            else
        //                                            {
        //                                                paramselected = s.SequenceNumber + 1;
        //                                            }
        //                                        }
        //                                        else if (resultCount == 1)
        //                                        {
        //                                            paramselected = s.SequenceNumber + 1;
        //                                        }
        //                                    }
        //                                    foreach (ShapeParameter sParam in this.ShapeParametersList)
        //                                    {
        //                                        if (sParam.SequenceNumber == paramselected)
        //                                        {
        //                                            if (sParam.ParameterValueCab.Equals("0") || (sParam.EditFlag == false && sParam.VisibleFlag == false))
        //                                            {
        //                                                sParam.ParameterValueCab = Convert.ToInt32(arrResult[resultCount]).ToString();
        //                                            }
        //                                            break;
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                #endregion
        //            }
        //            else if ((s.AngleType.Equals("ANGLE") && Array.IndexOf(arr, nextToAngleParam) >= 0) || (s.AngleType.Equals("ANGLE") && Array.IndexOf(arr, nextToAngleParam) >= 0) || Array.IndexOf(arr, s.AngleType) >= 0)
        //            {
        //                #region if param is offset+ normal calculate height
        //                //Get the formula to calculate angle or height value.
        //                var formula = (from item1 in this.ShapeParametersList
        //                               where item1.SequenceNumber == s.SequenceNumber
        //                               select item1).ToList();

        //                if (formula.Count > 0)
        //                {
        //                    //Calculate the value from the first formula.
        //                    if (formula[0].HeghtAngleFormula != "0")
        //                    {
        //                        resultOffset[0] = CalculateFormulaString(this.ShapeParametersList, formula[0].HeghtAngleFormula, "HEIGHT", this.Diameter, this.PinSizeID);
        //                    }
        //                    //Calculate the value from the second formula.
        //                    if (formula[0].HeghtSuceedAngleFormula != "0")
        //                    {
        //                        resultOffset[1] = CalculateFormulaString(this.ShapeParametersList, formula[0].HeghtSuceedAngleFormula, "HEIGHT", this.Diameter, this.PinSizeID);
        //                    }
        //                    //Calculate the value from the first formula.
        //                    if (formula[0].OffsetAngleFormula != "0")
        //                    {
        //                        resultOffset[2] = CalculateFormulaString(this.ShapeParametersList, formula[0].OffsetAngleFormula, "OFFSET", this.Diameter, this.PinSizeID);
        //                    }
        //                    //Calculate the value from the second formula.
        //                    if (formula[0].OffsetSuceedAngleFormula != "0")
        //                    {
        //                        resultOffset[3] = CalculateFormulaString(this.ShapeParametersList, formula[0].OffsetSuceedAngleFormula, "OFFSET", this.Diameter, this.PinSizeID);
        //                    }

        //                    if (formula[0].HeghtAngleFormula != "0" || formula[0].OffsetAngleFormula != "0")
        //                    {
        //                        if (s.AngleType.Equals("ANGLE") || s.AngleType.Equals("OFF-NOR-HEIGHT") || s.AngleType.Equals("OFF-NOR-OFFSET"))
        //                        {
        //                            for (int resultCount = 0; resultCount < resultOffset.Length; resultCount++)
        //                            {
        //                                if (resultOffset[resultCount] != 0)
        //                                {
        //                                    if (nextToAngleParam.ToUpper().Equals("OFF-NOR-HEIGHT"))
        //                                    {
        //                                        if (resultCount == 0)
        //                                        {
        //                                            paramselected = s.SequenceNumber + 2;
        //                                        }
        //                                        else if (resultCount == 1)
        //                                        {
        //                                            paramselected = s.SequenceNumber + 4;
        //                                        }
        //                                        else if (resultCount == 2)
        //                                        {
        //                                            paramselected = s.SequenceNumber + 3;
        //                                        }
        //                                    }
        //                                    else if (prevToAngleParam.ToUpper().Equals("OFF-NOR-OFFSET"))
        //                                    {
        //                                        if (resultCount == 0)
        //                                        {
        //                                            paramselected = s.SequenceNumber - 4;
        //                                        }
        //                                        else if (resultCount == 1)
        //                                        {
        //                                            paramselected = s.SequenceNumber - 2;
        //                                        }
        //                                        else if (resultCount == 3)
        //                                        {
        //                                            paramselected = s.SequenceNumber - 1;
        //                                        }
        //                                    }
        //                                    else if (s.AngleType.Equals("OFF-NOR-HEIGHT"))
        //                                    {
        //                                        if (resultCount == 0)
        //                                        {
        //                                            paramselected = s.SequenceNumber - 2;
        //                                        }
        //                                        else if (resultCount == 1)
        //                                        {
        //                                            paramselected = s.SequenceNumber + 2;
        //                                        }
        //                                        else if (resultCount == 2)
        //                                        {
        //                                            paramselected = s.SequenceNumber + 1;
        //                                        }
        //                                    }
        //                                    else if (s.AngleType.Equals("OFF-NOR-OFFSET"))
        //                                    {
        //                                        if (resultCount == 0)
        //                                        {
        //                                            paramselected = s.SequenceNumber - 3;
        //                                        }
        //                                        else if (resultCount == 1)
        //                                        {
        //                                            paramselected = s.SequenceNumber + 1;
        //                                        }
        //                                        else if (resultCount == 2)
        //                                        {
        //                                            paramselected = s.SequenceNumber - 1;
        //                                        }
        //                                    }
        //                                    foreach (ShapeParameter sParam in this.ShapeParametersList)
        //                                    {
        //                                        if (sParam.SequenceNumber == paramselected)
        //                                        {
        //                                            if (sParam.ParameterValueCab.Equals("0") || (sParam.EditFlag == false && sParam.VisibleFlag == false))
        //                                            {
        //                                                sParam.ParameterValueCab = Convert.ToInt32(resultOffset[resultCount]).ToString();
        //                                            }
        //                                            break;
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                #endregion
        //            }
        //            Array.Clear(arrResult, 0, arrResult.Length);
        //            Array.Clear(arrResult, 0, arrResult.Length);
        //        }
        //        #endregion

        //        #region Custom Formula Evaluation
        //        //Check for any formula.
        //        var custFormula = (from item1 in this.ShapeParametersList
        //                           where item1.CustFormula != "" && item1.CustFormula != string.Empty && item1.CustFormula != "0"
        //                           select item1).ToList();

        //        if (custFormula.Count > 0)
        //        {
        //            //Loop through to get all the formulas.
        //            for (int count = 0; count < custFormula.Count; count++)
        //            {
        //                string CFormula = custFormula[count].CustFormula.ToString();
        //                if (CFormula != "0")
        //                {
        //                    int resultCust = GetCalculatedVal(this.ShapeParametersList, CFormula, this.Diameter);

        //                    foreach (ShapeParameter sParam in this.ShapeParametersList)
        //                    {
        //                        if (sParam.SequenceNumber == Convert.ToInt32(custFormula[count].SequenceNumber))
        //                        {
        //                            if (sParam.ParameterValueCab.Equals("0") || (sParam.EditFlag == false && sParam.VisibleFlag == false))
        //                            {
        //                                sParam.ParameterValueCab = Convert.ToInt32(resultCust).ToString();
        //                            }
        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        #endregion

        //        #region Coupler validation.
        //        foreach (ShapeParameter s in this.ShapeParametersList)
        //        {
        //            if (s.AngleType == "ESPLICE")
        //            {
        //                string[] arrEsplice = { "ESCO", "ESCN", "ESCS", "EECO", "EECN", "EECS", "EBCS", "EESO", "EESN", "EESS", "ESSO", "ESSN", "ESSS", "ELCS", "ELSS" };

        //                //Get the coupler related info in list object
        //                var assembly = Assembly.GetExecutingAssembly();
        //                Stream stream = assembly.GetManifestResourceStream(this.GetType(), "CouplerMaster.xml");
        //                XDocument doc1 = XDocument.Load(stream);
        //                List<CouplerMaster> lstCoupler = (from c in doc1.Descendants("Coupler")
        //                                                  select new CouplerMaster
        //                                                  {
        //                                                      Type = c.Element("Type").Value,
        //                                                      CouplerMaterial = c.Element("CouplerMaterial").Value,
        //                                                      ThreadMaterial = c.Element("ThreadMaterial").Value,
        //                                                      LockNutMaterial = c.Element("LockNutMaterial").Value,
        //                                                      ParamA = c.Element("ParamA").Value,
        //                                                      ParamB = c.Element("ParamB").Value,
        //                                                      ParamC = c.Element("ParamC").Value,
        //                                                      CouplerDesc = c.Element("CouplerDesc").Value,
        //                                                      IntIndex = c.Element("IntIndex").Value,
        //                                                  }).ToList<CouplerMaster>();

        //                //Get the coupler master values by the coupler type selected
        //                var couplerMasterVals = (from c in lstCoupler where c.Type.Equals(s.ParameterValueCab.ToString().Trim() + this.Diameter) select c).ToList();
        //                if (couplerMasterVals.Count > 0)
        //                {
        //                    int newCouplerLengthVal = 0;
        //                    string nxtCoupLength = (s.SequenceNumber + 1).ToString();
        //                    var nxtCoupLenName = (from item1 in this.ShapeParametersList
        //                                          where item1.SequenceNumber == Convert.ToInt32(nxtCoupLength)
        //                                          select item1).ToList();
        //                    //Check if the shape has start as coupler or end.
        //                    var maxPosition = (from item2 in this.ShapeParametersList
        //                                       where item2.VisibleFlag == true && item2.EditFlag == true
        //                                       select item2.SequenceNumber).Max();
        //                    var minPosition = (from item3 in this.ShapeParametersList
        //                                       where item3.VisibleFlag == true && item3.EditFlag == true
        //                                       select item3.SequenceNumber).Min();

        //                    //Select the index to set the values.
        //                    if (nxtCoupLenName.Count > 0)
        //                    {
        //                        //Coupler can either be at start or at the end. Determine the length value accordingly.
        //                        int lenValEntered = 0;
        //                        if (Convert.ToInt32(nxtCoupLength) == Convert.ToInt32(maxPosition))
        //                        {
        //                            lenValEntered = Convert.ToInt32(nxtCoupLength) - 2;
        //                        }
        //                        if (s.SequenceNumber == Convert.ToInt32(minPosition))
        //                        {
        //                            lenValEntered = Convert.ToInt32(nxtCoupLength) + 1;
        //                        }

        //                        var lengthVal = (from item1 in this.ShapeParametersList
        //                                         where item1.SequenceNumber == lenValEntered
        //                                         select item1).ToList();
        //                        if (lengthVal.Count > 0)
        //                        {
        //                            string nextCoupParamVal = lengthVal[0].ParameterValueCab.ToString().Trim();
        //                            newCouplerLengthVal = Convert.ToInt32(nextCoupParamVal) - Convert.ToInt32(couplerMasterVals[0].ParamB) + Convert.ToInt32(couplerMasterVals[0].ParamA) + Convert.ToInt32(couplerMasterVals[0].ParamC);
        //                        }
        //                        else
        //                        {
        //                            newCouplerLengthVal = 0;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        //If double ended coupler.
        //                        if (s.SequenceNumber - 3 > 0)
        //                        {
        //                            var espliceCheck = (from item1 in this.ShapeParametersList
        //                                                where item1.SequenceNumber == Convert.ToInt32(s.SequenceNumber - 3)
        //                                                select item1.AngleType).ToList();
        //                            if (espliceCheck.Count > 0)
        //                            {
        //                                if (espliceCheck[0].ToUpper() == "ESPLICE")
        //                                {
        //                                    nxtCoupLenName = null;
        //                                    nxtCoupLenName = (from item1 in this.ShapeParametersList
        //                                                      where item1.SequenceNumber == Convert.ToInt32(s.SequenceNumber - 2)
        //                                                      select item1).ToList();
        //                                    if (nxtCoupLenName.Count > 0)
        //                                    {
        //                                        int lenValEntered = 0;
        //                                        lenValEntered = s.SequenceNumber - 2;
        //                                        var lengthVal = (from item1 in this.ShapeParametersList
        //                                                         where item1.SequenceNumber == lenValEntered
        //                                                         select item1).ToList();
        //                                        if (lengthVal.Count > 0)
        //                                        {
        //                                            string nextCoupParamVal = lengthVal[0].ParameterValueCab.ToString().Trim();
        //                                            newCouplerLengthVal = Convert.ToInt32(nextCoupParamVal) - Convert.ToInt32(couplerMasterVals[0].ParamB) + Convert.ToInt32(couplerMasterVals[0].ParamA) + Convert.ToInt32(couplerMasterVals[0].ParamC);
        //                                            nxtCoupLength = (s.SequenceNumber - 2).ToString();
        //                                        }
        //                                        else
        //                                        {
        //                                            newCouplerLengthVal = 0;
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    //Set the value in shape parameter list.
        //                    foreach (ShapeParameter sParam in this.ShapeParametersList)
        //                    {
        //                        if (sParam.SequenceNumber == Convert.ToInt32(nxtCoupLength))
        //                        {
        //                            sParam.ParameterValueCab = newCouplerLengthVal.ToString();
        //                            break;
        //                        }
        //                    }
        //                }
        //                lstCoupler.Clear();
        //                doc1 = null;
        //                stream.Dispose();
        //            }
        //        }
        //        #endregion

        //        #region symmetric component handling.
        //        //get the sequence no for the symmetric component and assign the values.
        //        foreach (ShapeParameter s in this.ShapeParametersList)
        //        {
        //            var symSeqList = (from item1 in this.ShapeParametersList where item1.symmetricIndex == s.ParameterName select item1.SequenceNumber).ToList();
        //            if (symSeqList.Count > 0)
        //            {
        //                foreach (ShapeParameter sParam in this.ShapeParametersList)
        //                {
        //                    if (sParam.SequenceNumber == symSeqList[0])
        //                    {
        //                        sParam.ParameterValueCab = Convert.ToInt32(s.ParameterValueCab).ToString();
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        errorMsg = "Error in shape code " + this.ShapeCode + ":" + ex.Message;
        //        throw ex;
        //    }
        //}

        ///// <summary>
        ///// Method to get Coupler refix with dia from the integer formed in UI.
        ///// </summary>
        ///// <param name="couplerVal"></param>
        ///// <returns></returns>
        //public string GetCoupler(string couplerVal)
        //{
        //    string couplerWithDia = "";
        //    int couplerIndex = Convert.ToInt32(couplerVal.Substring(0, 2));
        //    string dia = couplerVal.Substring(2, 2);
        //    try
        //    {
        //        switch (couplerIndex)
        //        {
        //            case 10:
        //                couplerWithDia = "ESCO" + dia;
        //                break;
        //            case 11:
        //                couplerWithDia = "ESCN" + dia;
        //                break;
        //            case 12:
        //                couplerWithDia = "ESCS" + dia;
        //                break;
        //            case 13:
        //                couplerWithDia = "EECO" + dia;
        //                break;
        //            case 14:
        //                couplerWithDia = "EECN" + dia;
        //                break;
        //            case 15:
        //                couplerWithDia = "EECS" + dia;
        //                break;
        //            case 16:
        //                couplerWithDia = "EBCS" + dia;
        //                break;
        //            case 17:
        //                couplerWithDia = "EESO" + dia;
        //                break;
        //            case 18:
        //                couplerWithDia = "EESN" + dia;
        //                break;
        //            case 19:
        //                couplerWithDia = "EESS" + dia;
        //                break;
        //            case 20:
        //                couplerWithDia = "ESSO" + dia;
        //                break;
        //            case 21:
        //                couplerWithDia = "ESSN" + dia;
        //                break;
        //            case 22:
        //                couplerWithDia = "ESSS" + dia;
        //                break;
        //            case 23:
        //                couplerWithDia = "ELCS" + dia;
        //                break;
        //            case 24:
        //                couplerWithDia = "ELSS" + dia;
        //                break;
        //            case 25:
        //                couplerWithDia = "DEC" + dia;
        //                break;
        //            case 26:
        //                couplerWithDia = "DES" + dia;
        //                break;
        //            case 27:
        //                couplerWithDia = "DSC" + dia;
        //                break;
        //            case 28:
        //                couplerWithDia = "DSS" + dia;
        //                break;
        //            case 29:
        //                couplerWithDia = "DLC" + dia;
        //                break;
        //            case 30:
        //                couplerWithDia = "DLN" + dia;
        //                break;
        //            case 31:
        //                couplerWithDia = "DNEC" + dia;
        //                break;
        //            case 32:
        //                couplerWithDia = "DNES" + dia;
        //                break;
        //            case 33:
        //                couplerWithDia = "DNSC" + dia;
        //                break;
        //            case 34:
        //                couplerWithDia = "DNSS" + dia;
        //                break;
        //            case 35:
        //                couplerWithDia = "DNLC" + dia;
        //                break;
        //            case 36:
        //                couplerWithDia = "DNLN" + dia;
        //                break;
        //            default:
        //                couplerWithDia = "";
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return "";
        //    }
        //    return couplerWithDia;
        //}

        ///// <summary>
        ///// Method to execute arithmatic parser.
        ///// </summary>
        ///// <param name="shapeparam"></param>
        ///// <param name="formula"></param>
        ///// <param name="command"></param>
        ///// <param name="dia"></param>
        ///// <param name="pin"></param>
        ///// <returns></returns>
        //public int CalculateFormulaString(List<ShapeParameter> shapeparam, string formula, string command, int dia, int pin)
        //{
        //    int result = 0;
        //    double pinRadius = 0;
        //    try
        //    {
        //        pinRadius = Convert.ToDouble(pin / 2.0);
        //        int editParamsCount = (from item in shapeparam
        //                               where item.ParameterName != "" && item.ParameterName != string.Empty && item.ParameterName != null
        //                               select item).Count();


        //        MathParserWrapper objParse = new MathParserWrapper();
        //        List<valuePair> valueList = new List<valuePair>();
        //        //Create the value pair.
        //        foreach (ShapeParameter shape in shapeparam)
        //        {
        //            if (shape.ParameterName != "" && shape.ParameterName != string.Empty && shape.ParameterName != null)
        //            {
        //                if (!shape.AngleType.Equals("ESPLICE") && !shape.AngleType.Equals("NSPLICE"))
        //                {
        //                    valueList.Add(new valuePair(shape.ParameterName, Convert.ToDouble(shape.ParameterValueCab)));
        //                }
        //            }
        //        }

        //        //Add the diameter
        //        valueList.Add(new valuePair("$", dia));
        //        //Add Pin radius
        //        valueList.Add(new valuePair("@", pinRadius));
        //        //Get the result.
        //        result = Convert.ToInt32(objParse.GetCalculatedValue(formula, valueList, false));
        //        objParse = null;
        //    }
        //    catch (Exception ex)
        //    {
        //        result = 0;
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// Method to execute arithmatic parser.
        ///// </summary>
        ///// <param name="valueList"></param>
        ///// <param name="formula"></param>
        ///// <param name="command"></param>
        ///// <param name="dia"></param>
        ///// <param name="pin"></param>
        ///// <returns></returns>
        //public int CalculateFormulaString(List<valuePair> valueList, string formula, string command, int dia, int pin)
        //{
        //    int result = 0;
        //    double pinRadius = 0;
        //    try
        //    {
        //        pinRadius = (Convert.ToDouble(pin) / 2);
        //        MathParserWrapper objParse = new MathParserWrapper();
        //        //Add the diameter
        //        valueList.Add(new valuePair("$", dia));
        //        //Add Pin radius
        //        valueList.Add(new valuePair("@", pinRadius));
        //        //Get the result.
        //        result = Convert.ToInt32(objParse.GetCalculatedValue(formula, valueList, false));
        //        objParse = null;
        //    }
        //    catch (Exception ex)
        //    {
        //        result = 0;
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// Method to calculate custom formulas
        ///// </summary>
        ///// <param name="shapeparam"></param>
        ///// <param name="formula"></param>
        ///// <param name="dia"></param>
        ///// <returns></returns>
        //public int GetCalculatedVal(List<ShapeParameter> shapeparam, string formula, int dia)
        //{
        //    int result = 0;
        //    try
        //    {
        //        if (!formula.ToUpper().Contains("SIN") && !formula.ToUpper().Contains("COS") && !formula.ToUpper().Contains("TAN") && !formula.ToUpper().Contains("$") && !formula.ToUpper().Contains("@"))
        //        {
        //            int count = (from param in shapeparam
        //                         where param.ParameterName != "" && param.ParameterName != string.Empty
        //                         select param).Count();
        //            string[,] arr = new string[count + 2, 2];
        //            int j = 0;
        //            shapeparam = shapeparam.OrderBy(O => O.ParameterName).ToList();
        //            for (int i = 0; i <= shapeparam.Count - 1; i++)
        //            {
        //                if (shapeparam[i].ParameterName != "" && shapeparam[i].ParameterName != string.Empty)
        //                {
        //                    arr[j, 0] = shapeparam[i].ParameterName.ToString();
        //                    arr[j, 1] = shapeparam[i].ParameterValueCab.ToString();
        //                    j++;
        //                }
        //            }
        //            if (dia != null)
        //            {
        //                arr[count, 0] = "$";
        //                arr[count, 1] = dia.ToString();
        //            }
        //            if (this.PinSizeID != null)
        //            {
        //                arr[count + 1, 0] = "@";
        //                arr[count + 1, 1] = (this.PinSizeID / 2).ToString();
        //            }
        //            //Call math parser class to execute the formula.
        //            MathParser objMath = new MathParser();
        //            result = Convert.ToInt32(objMath.Evaluate(formula, arr));
        //            objMath = null;
        //            Array.Clear(arr, 0, arr.Length);
        //        }
        //        else
        //        {
        //            double pinRadius = this.PinSizeID / 2;
        //            ExpressionParser.MathParserWrapper objParse = new ExpressionParser.MathParserWrapper();
        //            List<valuePair> valueList = new List<valuePair>();
        //            //Create the value pair.
        //            foreach (ShapeParameter shape in shapeparam)
        //            {
        //                if (shape.ParameterName != "" && shape.ParameterName != string.Empty && shape.ParameterName != null)
        //                {
        //                    valueList.Add(new valuePair(shape.ParameterName, Convert.ToDouble(shape.ParameterValueCab)));
        //                }
        //            }

        //            //Add the diameter
        //            valueList.Add(new valuePair("$", dia));
        //            //Add Pin radius
        //            valueList.Add(new valuePair("@", pinRadius));
        //            //Get the result.
        //            result = Convert.ToInt32(objParse.GetCalculatedValue(formula, valueList, false));
        //            objParse = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// Get the pin for straight bar and other shape codes based on the pin type in OES
        ///// </summary>
        ///// <param name="grade"></param>
        ///// <param name="dia"></param>
        ///// <param name="pinTypeId"></param>
        ///// <param name="productTypeId"></param>
        ///// <param name="shapeCode"></param>
        ///// <param name="errorMsg"></param>
        ///// <param name="pin"></param>
        ///// <param name="minLength"></param>
        ///// <param name="minHookLen"></param>
        ///// <param name="minHookHt"></param>
        //public void GetPinByPinType(string grade, int dia, int pinTypeId, int productTypeId, string shapeCode, out string errorMsg, out int pin, out int minLength, out int minHookLen, out int minHookHt)
        //{
        //    OESDBManager dbManager = new OESDBManager();
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        errorMsg = "";
        //        pin = 0;
        //        minLength = 0;
        //        minHookLen = 0;
        //        minHookHt = 0;

        //        sqlConnection.Open();
        //        dbManager.CreateParameters(5);
        //        dynamicParameters.Add(0, "@INTDIA", dia);
        //        dynamicParameters.Add(1, "@GRADE", grade);
        //        dynamicParameters.Add(2, "@INTPRODUCTTYPEID", productTypeId);
        //        dynamicParameters.Add(3, "@SHAPECODE", shapeCode.Trim());
        //        dynamicParameters.Add(4, "@PINTYPE", pinTypeId);
        //        ds = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "USP_GET_PIN_BYSHAPE_OES");
        //        if (ds.Tables.Count != 0)
        //        {
        //            foreach (DataRowView drCABPin in ds.Tables[0].DefaultView)
        //            {
        //                pin = Convert.ToInt32(drCABPin[0].ToString());
        //                minLength = Convert.ToInt32(drCABPin[1].ToString());
        //                minHookLen = Convert.ToInt32(drCABPin[2].ToString());
        //                minHookHt = Convert.ToInt32(drCABPin[3].ToString());
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        errorMsg = ex.Message.ToString();
        //        pin = 0;
        //        minLength = 0;
        //        minHookLen = 0;
        //        minHookHt = 0;
        //    }
        //    finally
        //    {
        //        
        //        ds.Dispose();
        //    }
        //}

        ///// <summary>
        ///// Method to get the group mark id for Copy BBS.
        ///// </summary>
        ///// <param name="ProjectID"></param>
        ///// <param name="WBSElementID"></param>
        ///// <param name="StructureElementName"></param>
        ///// <param name="GroupMarkingName"></param>
        ///// <param name="UserId"></param>
        ///// <returns></returns>
        public int GetGroupMarkID_CopyBBS(GetGMId_CopyBBSdto obj)
        {
            int groupMarkId;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTPROJECTID", obj.ProjectID);
                    dynamicParameters.Add("@INTWBSELEMENTID",obj.WBSElementID);
                    dynamicParameters.Add("@VCHSTRUCTUREELEMENTNAME",obj.StructureElementName);
                    dynamicParameters.Add("@VCHGROUPMARKINGNAME",obj.GroupMarkingName);
                    dynamicParameters.Add("@INTUSERNAME",obj.UserId);
                    groupMarkId = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.GET_GROUPMARKIDCAB_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();

                }
               
                return groupMarkId;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
               // 
            }
        }

        ///// <summary>
        ///// Method to get the BBS no from projectid.
        ///// </summary>
        ///// <param name="prijectId"></param>
        ///// <param name="enteredText"></param>
        ///// <returns></returns>
        public List<LoadSORDto>GetBBS(int projectId, string enteredText)
        {
            
            DataSet ds = new DataSet();
            try
            {
                IEnumerable<LoadSORDto> loadSORDtos;
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@PROJECTID", projectId);
                    dynamicParameters.Add("@BBSNo", enteredText);
                    loadSORDtos = sqlConnection.Query<LoadSORDto>(SystemConstants.GET_BBS_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return loadSORDtos.ToList();

                }
              
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                
            }
        }

        ///// <summary>
        ///// Method to check if the bbs is from arma plus or from CAB Data Entry. CAB Data Entry if 1, Arma plus 0.
        ///// </summary>
        ///// <param name="bbsSource"></param>
        ///// <returns></returns>
        public bool CheckBbsSource(string bbsSource)
        {
            
            bool boolOutput = false;
            int res = 0;
            DataSet dsChkBbsSource = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@BBSSOURCE", bbsSource.Trim());
                    res = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.USP_CHECK_BBSSOURCE_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);

                    sqlConnection.Close();

                    if(res == 1)
                    {
                        boolOutput=true;

                    }
                    else
                    {
                        boolOutput=false;
                    }


                    //if (dsChkBbsSource.Tables.Count != 0)
                    //{
                    //    foreach (DataRowView drCABProductMarkID in dsChkBbsSource.Tables[0].DefaultView)
                    //    {
                    //        boolOutput = Convert.ToBoolean(drCABProductMarkID[0].ToString());
                    //        break;
                    //    }
                    //}

                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
             
            }
            return boolOutput;
        }



        ////Added by Siddhi for COPY-BBS Support CR
        ////public bool CheckBbsSource(string bbsSource, string strElem) //MODIFIED BY SIDDHI
        ////{
        ////    OESDBManager dbManager = new OESDBManager();
        ////    bool boolOutput = false;
        ////    DataSet dsChkBbsSource = new DataSet();
        ////    try
        ////    {
        ////        sqlConnection.Open();
        ////        dbManager.CreateParameters(2);
        ////        dynamicParameters.Add(0, "@BBSSOURCE", bbsSource.Trim());
        ////        dynamicParameters.Add(1, "@strElem", strElem.Trim());
        ////        dsChkBbsSource = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "USP_CHECK_BBSSOURCE_CUBE");
        ////        if (dsChkBbsSource.Tables.Count != 0)
        ////        {
        ////            foreach (DataRowView drCABProductMarkID in dsChkBbsSource.Tables[0].DefaultView)
        ////            {
        ////                boolOutput = Convert.ToBoolean(drCABProductMarkID[0].ToString());
        ////                break;
        ////            }
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw ex;
        ////    }
        ////    finally
        ////    {
        ////        
        ////        dsChkBbsSource.Dispose();
        ////    }
        ////    return boolOutput;
        ////} 

        ///// <summary>
        ///// Method to copy BBS.
        ///// </summary>
        ///// <param name="bbsSource"></param>
        ///// <param name="bbsTarget"></param>
        ///// <param name="pojId"></param>
        ///// <param name="wbsId"></param>
        ///// <param name="vchStructureElementType"></param>
        ///// <returns></returns>
        //public int CopyBBS(string bbsSource, string bbsTarget, int pojId, int wbsId, string vchStructureElementType)
        //{
        //    OESDBManager dbManager = new OESDBManager();
        //    int intOutput = 0;
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        sqlConnection.Open();
        //        dbManager.CreateParameters(6);
        //        dynamicParameters.Add(0, "@BBSSOURCE", bbsSource.Trim());
        //        dynamicParameters.Add(1, "@BBSTARGET", bbsTarget.Trim());
        //        dynamicParameters.Add(2, "@INTPROJECTID", pojId);
        //        dynamicParameters.Add(3, "@INTWBSELEMENTID", wbsId);
        //        dynamicParameters.Add(4, "@VCHSTRUCTUREELEMENTNAME", vchStructureElementType);
        //        dynamicParameters.Add(5, "@ROWCOUNT", 100);
        //        ds = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "COPY_BBS_INSERT_CUBE");
        //        if (ds.Tables.Count != 0)
        //        {
        //            foreach (DataRowView drCABProductMarkID in ds.Tables[0].DefaultView)
        //            {
        //                intOutput = Convert.ToInt32(drCABProductMarkID[0].ToString());
        //                break;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        
        //    }
        //    return intOutput;
        //}

        ///// <summary>
        ///// Method to get the source and destination ids for BBS copy
        ///// </summary>
        ///// <param name="bbsSource"></param>
        ///// <param name="bbsTarget"></param>
        ///// <param name="wbsId"></param>
        ///// <param name="vchStructureElementType"></param>
        ///// <param name="pojId"></param>
        ///// <returns></returns>
        public List<GetCopyBBSUIDDto> GetCopyBBSUID(string bbsSource, string bbsTarget, int wbsId, string vchStructureElementType, int pojId)
        {
            
            DataSet ds = new DataSet();
            try
            {
                IEnumerable<GetCopyBBSUIDDto>getCopyBBSUIDs;
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@BBSSOURCE", bbsSource.Trim());
                    dynamicParameters.Add("@BBSTARGET", bbsTarget.Trim());
                    dynamicParameters.Add("@INTWBSELEMENTID", wbsId);
                    dynamicParameters.Add("@VCHSTRUCTUREELEMENTNAME", vchStructureElementType);
                    dynamicParameters.Add("@INTPROJECTID", pojId);
                    getCopyBBSUIDs = sqlConnection.Query<GetCopyBBSUIDDto>(SystemConstants.GET_COPYBBS_SOURCE_TARGET_UID, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return getCopyBBSUIDs.ToList();

                }
            
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //
            }
            
        }

        ///// <summary>
        ///// Method to insert productmarking details and select data for trans header and detail insert.
        ///// </summary>
        ///// <param name="seIdSource"></param>
        ///// <param name="seIdTarget"></param>
        ///// <param name="groupMarkId"></param>
        ///// <returns></returns>
        public List<InsertProductMarkCopyBBSDto>InsertProductMarkCopyBBS(int seIdSource, int seIdTarget, int groupMarkId)
        {
            
            DataSet ds = new DataSet();
            try
            {
                IEnumerable<InsertProductMarkCopyBBSDto>insertProductMarks;
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@SEIDSOURCE", seIdSource);
                    dynamicParameters.Add("@SEIDTARGET", seIdTarget);
                    dynamicParameters.Add("@INTGROUPMARKID", groupMarkId);
                    insertProductMarks = sqlConnection.Query<InsertProductMarkCopyBBSDto>(SystemConstants.COPY_BBS_INSERT_PRODUCTMARK, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return insertProductMarks.ToList();

                }
                
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
              //  
            }
            //return ds;
        }

        ///// <summary>
        ///// Method to insert trans details.
        ///// </summary>
        ///// <param name="sourceTransId"></param>
        ///// <param name="seIdTarget"></param>
        ///// <returns></returns>
        public int InsertTransdetailsCopyBBS(int sourceTransId, int seIdTarget)
        {
            int Id = 0;
           
            DataSet ds = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@SOURCESHAPETRANSHEADERID", sourceTransId);
                    dynamicParameters.Add("@SEIDTARGET", seIdTarget);
                    Id = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.COPY_BBS_INSERT_TRANSDETAILS, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return Id;

                }
               
            
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                
            }
          //  return ds;
        }

        ///// <summary>
        ///// Method to get the available accessory.
        ///// </summary>
        ///// <param name="sourceTransId"></param>
        ///// <param name="seIdTarget"></param>
        ///// <returns></returns>
        public List<InsertProductMarkCopyBBSDto> GetAccessoryCopyBBS(int seIdSource)
        {
            
            DataSet ds = new DataSet();
            IEnumerable<InsertProductMarkCopyBBSDto> getAccessory;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@SEIDSOURCE", seIdSource);
                    getAccessory = sqlConnection.Query<InsertProductMarkCopyBBSDto>(SystemConstants.COPY_BBS_GET_ACCESSORY, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                }
                   

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            return getAccessory.ToList();
        }

        ///// <summary>
        ///// Method to insert accessory details.
        ///// </summary>
        ///// <param name="seIdSource"></param>
        ///// <param name="seIdTarget"></param>
        ///// <param name="groupMarkId"></param>
        ///// <param name="prodmarkName"></param>
        ///// <param name="accProdMarkName"></param>
        ///// <returns></returns>
        public int InsertAccessoryCopyBBS(InsertAcSCopyBBSDto acSCopyBBSDto)
        {
            
            int outPut = 0;
            try
            {

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@SEIDSOURCE", acSCopyBBSDto.seIdSource);
                    dynamicParameters.Add("@SEIDTARGET", acSCopyBBSDto.seIdTarget);
                    dynamicParameters.Add("@INTGROUPMARKID", acSCopyBBSDto.groupMarkId);
                    dynamicParameters.Add("@VCHCABPRODUCTMARKNAME", acSCopyBBSDto.prodmarkName);
                    dynamicParameters.Add("@VCHACCPRODUCTMARKINGNAME", acSCopyBBSDto.accProdMarkName);
                    outPut = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.COPY_BBS_INSERT_ACCESSORY, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                }

                return outPut;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            return outPut;
        }
        //#endregion
    }
}
