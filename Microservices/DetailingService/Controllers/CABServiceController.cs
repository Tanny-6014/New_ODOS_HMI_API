using AutoMapper;
using Dapper;
using DetailingService.Dtos;
using DetailingService.Interfaces;
using DetailingService.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Diagnostics.Metrics;
using System.Net.Http;
using System.Security.Cryptography.Xml;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Web;
using System.Runtime.ConstrainedExecution;
using System.Data.SqlClient;
using static Dapper.SqlMapper;
//using Microsoft.Data.SqlClient;

namespace DetailingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CABServiceController : ControllerBase
    {
        private readonly ICABService _cABService;

        SqlConnection cnNDS = new SqlConnection();
        SqlTransaction osqlTransNDS;

        public CABServiceController( ICABService cABService)
        {
            
            _cABService = cABService;

        }


       
        [HttpPost]
        [Route("/ValidateAndInsertTCT")]
        public async Task<IActionResult> ValidateAndInsertTCT([FromBody] InsertToDbTCTDto[] insertToDbTCT)
        {
            List<CABItem> cABs;
            string errorMessage = "";
            int GMId = 0;
            string InserFlag = "";
            bool SaveFlag = false;
            int totalcount = insertToDbTCT.Length;
            int counter = 0;
            int ProductmarkID = 0;
            try
            {
                foreach (InsertToDbTCTDto item in insertToDbTCT)
                {
                    CABItem cABItem = new CABItem();
                    //cABItem.GroupRevisionNumber = 0;
                    cABItem.CABProductMarkName = item.CABProductMarkName;
                    cABItem.intSEDetailingId = item.SEDetailingID;
                    cABItem.GroupMarkId = item.GroupMarkId;
                    cABItem.Quantity = item.MemberQty;
                    cABItem.ShapeCode = item.ShapeCode;
                    cABItem.PinSizeID = item.PinSizeID;
                    cABItem.InvoiceLength = item.InvoiceLength;
                    cABItem.ProductionLength = item.ProductionLength;
                    cABItem.InvoiceWeight = item.InvoiceWeight;
                    cABItem.ProductionWeight = item.ProductionWeight;
                    cABItem.Grade = item.Grade;
                    cABItem.Diameter = item.Diameter;
                    cABItem.Coupler1Standard = item.Coupler1Standard;
                    cABItem.ShapeType = item.ShapeType;
                    cABItem.ShapeGroup = item.ShapeGroup;
                    cABItem.Coupler1Type = item.Coupler1Type;
                    cABItem.Coupler1 = item.Coupler1;
                    cABItem.Coupler1Standard = item.Coupler1Standard;

                    cABItem.Coupler2Type = item.Coupler2Type;
                    cABItem.Coupler2 = item.Coupler2;
                    cABItem.Coupler2Standard = item.Coupler2Standard;
                    cABItem.Status = item.Status;
                    cABItem.CustomerRemarks = item.CustomerRemarks;
                    cABItem.ShapeImage = item.ShapeImage;
                    cABItem.BVBS = item.BVBS;
                    cABItem.PageNumber = item.PageNumber;
                    cABItem.DescScript = item.DescScript;
                    cABItem.EnvLength = item.EnvLength;
                    cABItem.EnvWidth = item.EnvWidth;
                    cABItem.EnvHeight = item.EnvHeight;
                    cABItem.NoOfBends = item.NoOfBends;
                    cABItem.IsVariableBar = Convert.ToBoolean(item.IsVariableBar);

                    var result = _cABService.InsertToDbTCT(cABItem,false,out ProductmarkID);
                    SaveFlag = true;
                    counter++;
                    if (counter == totalcount)
                    {
                        return Ok(result);
                    }

                }
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }



        }


        [HttpPost]
        [Route("/ValidateAndInsertTCTById2/{isValidated}")]
        public async Task<IActionResult> ValidateAndInsertTCT([FromBody] InsertToDbTCTDto[] insertToDbTCT, bool isValidated)
        {
            List<CABItem> cABs;
            string errorMessage = "";
            int GMId = 0;
            string InserFlag = "";
            bool SaveFlag = false;
            int totalcount = insertToDbTCT.Length;
            int counter = 0;
            try
            {
                foreach (InsertToDbTCTDto item in insertToDbTCT)
                {


                    CABItem cABItem = new CABItem();
                    //cABItem.GroupRevisionNumber = 0;
                    cABItem.CABProductMarkName = item.CABProductMarkName;
                    cABItem.intSEDetailingId = item.SEDetailingID;
                    cABItem.GroupMarkId = item.GroupMarkId;
                    cABItem.Quantity = item.MemberQty;
                    cABItem.ShapeCode = item.ShapeCode;
                    cABItem.PinSizeID = item.PinSizeID;
                    cABItem.InvoiceLength = item.InvoiceLength;
                    cABItem.ProductionLength = item.ProductionLength;
                    cABItem.InvoiceWeight = item.InvoiceWeight;
                    cABItem.ProductionWeight = item.ProductionWeight;
                    cABItem.Grade = item.Grade;
                    cABItem.Diameter = item.Diameter;
                    cABItem.Coupler1Standard = item.Coupler1Standard;
                    cABItem.ShapeType = item.ShapeType;
                    cABItem.ShapeGroup = item.ShapeGroup;
                    cABItem.Coupler1Type = item.Coupler1Type;
                    cABItem.Coupler1 = item.Coupler1;
                    cABItem.Coupler1Standard = item.Coupler1Standard;

                    cABItem.Coupler2Type = item.Coupler2Type;
                    cABItem.Coupler2 = item.Coupler2;
                    cABItem.Coupler2Standard = item.Coupler2Standard;
                    cABItem.Status = item.Status;
                    cABItem.CustomerRemarks = item.CustomerRemarks;
                    cABItem.ShapeImage = item.ShapeImage;
                    cABItem.BVBS = item.BVBS;
                    cABItem.PageNumber = item.PageNumber;
                    cABItem.DescScript = item.DescScript;
                    cABItem.EnvLength = item.EnvLength;
                    cABItem.EnvWidth = item.EnvWidth;
                    cABItem.EnvHeight = item.EnvHeight;
                    cABItem.NoOfBends = item.NoOfBends;
                    cABItem.IsVariableBar = Convert.ToBoolean(item.IsVariableBar);



                    var result = _cABService.InsertToDbTCT(cABItem, isValidated, out errorMessage);
                    SaveFlag = true;
                    counter++;
                    if (counter == totalcount)
                    {
                        return Ok(result);
                    }

                }
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }


        [HttpGet]
        [Route("/ValidateAndSave/{groupMarkId}")]
        public async Task<IActionResult> ValidateAndSave(int groupMarkId)
        {
            // bool SaveFlag = false;
            string errorMsg = "";
            try
            {
                var result = _cABService.ValidateAndSave(groupMarkId, out errorMsg);

                if (errorMsg == "")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(errorMsg);
                }

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }



        }

        //Core Cage Shape Code
        [HttpGet]
        [Route("/FilterShapeCode_cab/{enteredText}")]
        public async Task<IActionResult> FilterShapeCode(string enteredText)
        {
            string errorMessage = "";
            List<ShapeCode> shapeCodes = _cABService.FilterShapeCode(enteredText, out errorMessage);
            var result = shapeCodes;
            if (errorMessage == "")
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(errorMessage);
            }
        }

        //[HttpGet]
        //[Route("/ShapeParameterForCab_Get/{enteredText}")]
        //public async Task<IActionResult> ShapeParameterForCab_Get(string enteredText)
        //{
        //    string errorMessage = "";
        //    ShapeParameterCollection ShapeparameterList = _cABService.ShapeParameterForCab_Get(enteredText, out errorMessage);


          
        //    List<ShapeParameterCollection> ShapeparameterList_new = ShapeparameterList.Cast<ShapeParameterCollection>().ToList();

           
        //    var result = ShapeparameterList_new;
        //    if (errorMessage == "")
        //    {
        //        return Ok(result);
        //    }
        //    else
        //    {
        //        return BadRequest(errorMessage);
        //    }
        //}


        

        [HttpPost]
        [Route("/InsertProductMarkInLine")]
        public async Task<IActionResult> InsertProductMarkInLine([FromBody] InsertProductMarkInLineDto insertProductMarkIn, int SEDetailingID, string barMarkStart)
        {
            string errorMessage = "";

            bool SaveFlag = false;
            try
            {
                CABItem objCab = new CABItem
                {


                };

                var result = _cABService.InsertProductMarkInLine(insertProductMarkIn, SEDetailingID, barMarkStart, out errorMessage);
                SaveFlag = true;
                return Ok(result);


            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }



        [HttpPost]
        [Route("/InsertBarMark/{SEDetailingID}/{intGroupMarkID}")]
        public async Task<IActionResult> InsertBarMark([FromBody] OrderDetailsListModelsDto insertProductMark, int SEDetailingID,int intGroupMarkID)
            {
            string errorMessage = "";
            int ProductMarkId = 0;
            DataTable ldtPin;
            DataTable ldtValidator;
            DataTable ldtCouplerType;
            DataTable ldtCouplerShape;

            string lSQL;
            SqlDataAdapter adoAdp;
            SqlCommand adoCmd;
            SqlDataReader adoRst;
            bool SaveFlag = false;
            string strComDesc, strBarmark;
            string strGrade="";
            string lCouplerSeries = "S";
            
            try
            {

                string lShapeCode = insertProductMark.BarShapeCode;
                if (lShapeCode == null)
                {
                    lShapeCode = "";
                }
                if (lShapeCode != "")
                {
                    while (lShapeCode.Length < 3)
                    {
                        lShapeCode = "0" + lShapeCode;
                    }

                    insertProductMark.BarShapeCode = lShapeCode;
                }

                if (insertProductMark.BarMark == null)
                {
                    insertProductMark.BarMark = "";
                }
                if (insertProductMark.BarMark.IndexOf(",") >= 0)
                {
                    insertProductMark.BarMark = insertProductMark.BarMark.Replace(",", "");
                }
                CABItem objCab = new CABItem();

                objCab.CABProductMarkName = insertProductMark.BarMark;
                objCab.CABProductMarkID = insertProductMark.CabProductMarkID;
                objCab.intSEDetailingId = SEDetailingID;
                objCab.GroupMarkId = intGroupMarkID;
                objCab.MemberQty = insertProductMark.BarMemberQty;
                objCab.Quantity = insertProductMark.BarTotalQty;
                objCab.ShapeCode = insertProductMark.BarShapeCode.ToString();
                objCab.PinSizeID = insertProductMark.PinSize;
                //objCab.InvoiceLength = insertProductMark.NUMINVOICELENGTH;
                //objCab.ProductionLength = insertProductMark.NUMPRODUCTIONLENGTH;
                //objCab.InvoiceWeight = insertProductMark.NUMINVOICEWEIGHT;
                //objCab.ProductionWeight = insertProductMark.NUMPRODUCTIONWEIGHT;
                ////////////////////////-----------------/////////////////

                objCab.Grade = insertProductMark.BarType;
                objCab.Diameter = insertProductMark.BarSize;
                //objCab.Coupler1Standard = insertProductMark.BARSTANDARD;
                //objCab.ShapeType = insertProductMark.VCHSHAPETYPE;
                //objCab.ShapeGroup = insertProductMark.VCHSHAPEGROUP;
                //objCab.Coupler1Type = insertProductMark.COUPLERTYPE1;
                //objCab.Coupler1 = insertProductMark.COUPLERMATERIAL1;
                //objCab.Coupler1Standard = insertProductMark.COUPLERSTANDARD1;
                //objCab.Coupler2Type = insertProductMark.COUPLERTYPE2;
                //objCab.Coupler2 = insertProductMark.COUPLERMATERIAL2;
                //objCab.Coupler2Standard = insertProductMark.COUPLERSTANDARD2;
                objCab.Status = "1";
                objCab.CustomerRemarks = insertProductMark.ElementMark;
                //objCab.ShapeImage = insertProductMark.VCHSHAPEIMAGE;
                //objCab.BVBS = insertProductMark.VCHCAB_BVBS;
                //objCab.PageNumber = insertProductMark.VCHPAGENUMBER;
                objCab.CommercialDesc = insertProductMark.ElementMark;
                objCab.DescScript = insertProductMark.ElementMark;
                //objCab.EnvLength = insertProductMark.NUMENVLENGTH;
                //objCab.EnvWidth = insertProductMark.NUMENVWIDTH;
                //objCab.EnvHeight = insertProductMark.NUMENVHEIGHT;
                //objCab.NoOfBends = insertProductMark.INTNOOFBENDS;
                objCab.IsVariableBar = false;
                objCab.CustomerRemarks = insertProductMark.ElementMark;
                objCab.CommercialDesc = insertProductMark.ElementMark; 
                //objCab.Shape = insertProductMark.Shape;
                //objCab.ShapeParametersList = insertProductMark/*.*/Shape.ShapeParam;



                List<ShapeCode> shapeCodes = _cABService.FilterShapeCode(objCab.ShapeCode, out errorMessage);

                objCab.Shape = shapeCodes[0];

                 objCab.ShapeParametersList = new List<ShapeParameter>();


                foreach (var shapeParam in objCab.Shape.ShapeParam)
                {
                    var options = new JsonSerializerOptions()
                    {
                        NumberHandling = JsonNumberHandling.AllowReadingFromString |
                                 JsonNumberHandling.WriteAsString
                    };

               //  ShapeParameter shapeParam1 = new ShapeParameter();
                    string denemeJson = JsonSerializer.Serialize(shapeParam, options);
                    // Assuming shapeParam is an instance of DetailingService.Repositories.ShapeParameterDetailingService.Repositories.ShapeParameter shapeParam = new DetailingService.Repositories.ShapeParameter();
                    // Serialize shapeParam to a JSON stringstring denemeJson = JsonSerializer.Serialize(shapeParam, options);
                    // string denemeJson = JsonSerializer.Serialize<JsonElement>((JsonElement)shapeParam, options);



                    // deserialize

                    ShapeParameter shapeParamObj = JsonSerializer.Deserialize<ShapeParameter>(denemeJson, options);



                    objCab.ShapeParametersList.Add(shapeParamObj);
                }

                cnNDS.ConnectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=120";


        //Added by ajit
        //get Coupler Type
        //lSQL = "SELECT ShapeCode, CouplerType FROM dbo.OESCouplerType WHERE CouplerSeries = '" + lCouplerSeries + "' ";
        lSQL = "SELECT ShapeCode, CouplerType FROM dbo.OESCouplerType WHERE CouplerSeries = ' ' OR CouplerSeries = 'S' ";
                adoCmd = new SqlCommand(lSQL, cnNDS);
                adoCmd.Transaction = osqlTransNDS;
                adoCmd.CommandTimeout = 1200;
                adoAdp = new SqlDataAdapter(adoCmd);
                ldtCouplerType = new DataTable();
                adoAdp.Fill(ldtCouplerType);

                //get Coupler Type
                //lSQL = "SELECT shapeCode, CouplerParameters FROM dbo.OESShapeReference WHERE shapeStatus = 'Active' and CouplerParameters > ''  ";
                lSQL = "SELECT chrShapeCode as shapeCode, chrParamName as CouplerParameters " +
                    "FROM dbo.ShapeMaster_cube M, dbo.ShapeParamDetails_cube P " +
                    "WHERE P.intShapeID = M.intShapeID " +
                    "AND (chrAngleType = 'N' OR chrAngleType = 'E') " +
                    "ORDER BY chrShapeCode, chrParamName ";
                adoCmd = new SqlCommand(lSQL, cnNDS);
                adoCmd.Transaction = osqlTransNDS;
                adoCmd.CommandTimeout = 1200;
                adoAdp = new SqlDataAdapter(adoCmd);
                ldtCouplerShape = new DataTable();
                adoAdp.Fill(ldtCouplerShape);

                // get coupler type for coupler shape
                string lShapeType = lShapeCode.Substring(0, 1);
                if (lShapeType == "C" ||
                lShapeType == "P" ||
                lShapeType == "N" ||
                lShapeType == "S" ||
                lShapeType == "H" ||
                lShapeType == "I" ||
                lShapeType == "J" ||
                lShapeType == "K" ||
                lShapeType == "L")
                {
                    var ldRows = ldtCouplerShape.Select("ShapeCode = '" + lShapeCode + "' ");
                    if (ldRows.Length > 0)
                    {
                        for (int j = 0; j < ldRows.Count(); j++)
                        {
                            string lType = "";
                            if (j == 0)
                            {
                                string lVarType = lShapeType;
                                if (lVarType == "L") lVarType = lShapeCode.Substring(0, 2);
                                var ldTypeRows = ldtCouplerType.Select("ShapeCode = '" + lVarType + "' ");
                                if (ldTypeRows.Length > 0) lType = (string)ldTypeRows[0].ItemArray[1];
                            }
                            if (j == 1)
                            {
                                string lVarType = lShapeCode.Substring(lShapeCode.Trim().Length - 1, 1);
                                if (lVarType == "L") lVarType = lShapeCode.Substring(lShapeCode.Trim().Length - 2, 2);
                                var ldTypeRows = ldtCouplerType.Select("ShapeCode = '" + lVarType + "' ");
                                if (ldTypeRows.Length > 0) lType = (string)ldTypeRows[0].ItemArray[1];
                            }
                            if (lType != "")
                            {
                                if (strGrade == "X")
                                {
                                    if (lCouplerSeries == "N" || lCouplerSeries == "S")
                                    {
                                        lType = lType.Substring(0, 3) + 'X';
                                    }
                                    else
                                    {
                                        lType = 'X' + lType.Substring(1);
                                    }
                                }
                                switch (((string)ldRows[j].ItemArray[1]).Trim())
                                {
                                    case "A":
                                        insertProductMark.A = lType;
                                        break;
                                    case "B":
                                        insertProductMark.B = lType;
                                        break;
                                    case "C":
                                        insertProductMark.C = lType;
                                        break;
                                    case "D":
                                        insertProductMark.D = lType;
                                        break;
                                    case "E":
                                        insertProductMark.E = lType;
                                        break;
                                    case "F":
                                        insertProductMark.F = lType;
                                        break;
                                    case "G":
                                        insertProductMark.G = lType;
                                        break;
                                    case "H":
                                        insertProductMark.H = lType;
                                        break;
                                    case "I":
                                        insertProductMark.I = lType;
                                        break;
                                    case "J":
                                        insertProductMark.J = lType;
                                        break;
                                    case "K":
                                        insertProductMark.K = lType;
                                        break;
                                    case "L":
                                        insertProductMark.L = lType;
                                        break;
                                    case "M":
                                        insertProductMark.M = lType;
                                        break;
                                    case "N":
                                        insertProductMark.N = lType;
                                        break;
                                    case "O":
                                        insertProductMark.O = lType;
                                        break;
                                    case "P":
                                        insertProductMark.P = lType;
                                        break;
                                    case "Q":
                                        insertProductMark.Q = lType;
                                        break;
                                    case "R":
                                        insertProductMark.R = lType;
                                        break;
                                    case "S":
                                        insertProductMark.S = lType;
                                        break;
                                    case "T":
                                        insertProductMark.T = lType;
                                        break;
                                    case "U":
                                        insertProductMark.U = lType;
                                        break;
                                    case "V":
                                        insertProductMark.V = lType;
                                        break;
                                    case "W":
                                        insertProductMark.W = lType;
                                        break;
                                    case "X":
                                        insertProductMark.X = lType;
                                        break;
                                    case "Y":
                                        insertProductMark.Y = lType;
                                        break;
                                    case "Z":
                                        insertProductMark.Z = lType;
                                        break;
                                }
                            }
                        }
                    }
                }

                for (int j = 0; j < objCab.ShapeParametersList.Count(); j++)
                {
                    if (objCab.ShapeParametersList[j].EditFlag == true || (objCab.ShapeCode.Trim().ToUpper() == "R84" && objCab.ShapeParametersList[j].ParameterName.Trim().ToUpper() == "R"))
                    {
                        
                        switch (objCab.ShapeParametersList[j].ParameterName.Trim().ToUpper())
                        {
                            case "A":
                                objCab.ShapeParametersList[j].ParameterValueCab = insertProductMark.A;
                                break;
                            case "B":
                                objCab.ShapeParametersList[j].ParameterValueCab = insertProductMark.B;
                                break;
                            case "C":
                                objCab.ShapeParametersList[j].ParameterValueCab = insertProductMark.C;
                                break;
                            case "D":
                                objCab.ShapeParametersList[j].ParameterValueCab = insertProductMark.D;
                                break;
                            case "E":
                                objCab.ShapeParametersList[j].ParameterValueCab = insertProductMark.E;
                                break;
                            case "F":
                                objCab.ShapeParametersList[j].ParameterValueCab = insertProductMark.F;
                                break;
                            case "G":
                                objCab.ShapeParametersList[j].ParameterValueCab = insertProductMark.G;
                                break;
                            case "H":
                                objCab.ShapeParametersList[j].ParameterValueCab = insertProductMark.H;
                                break;
                            case "I":
                                objCab.ShapeParametersList[j].ParameterValueCab = insertProductMark.I;
                                break;
                            case "J":
                                objCab.ShapeParametersList[j].ParameterValueCab = insertProductMark.J;
                                break;
                            case "K":
                                objCab.ShapeParametersList[j].ParameterValueCab = insertProductMark.K;
                                break;
                            case "L":
                                objCab.ShapeParametersList[j].ParameterValueCab = insertProductMark.L;
                                break;
                            case "M":
                                objCab.ShapeParametersList[j].ParameterValueCab = insertProductMark.M;
                                break;
                            case "N":
                                objCab.ShapeParametersList[j].ParameterValueCab = insertProductMark.N;
                                break;
                            case "O":
                                objCab.ShapeParametersList[j].ParameterValueCab = insertProductMark.O;
                                break;
                            case "P":
                                objCab.ShapeParametersList[j].ParameterValueCab = insertProductMark.P;
                                break;
                            case "Q":
                                objCab.ShapeParametersList[j].ParameterValueCab = insertProductMark.Q;
                                break;
                            case "R":
                                if (objCab.ShapeCode.Trim().ToUpper() == "R84")
                                {
                                    int liVar;
                                    if (int.TryParse(insertProductMark.C, out liVar))
                                    {
                                        objCab.ShapeParametersList[j].ParameterValueCab = (liVar / 2).ToString();
                                    }
                                    else
                                    {
                                        objCab.ShapeParametersList[j].ParameterValueCab = insertProductMark.R;
                                    }
                                }
                                else
                                {
                                    objCab.ShapeParametersList[j].ParameterValueCab = insertProductMark.R;
                                }
                               
                                break;
                            case "S":
                                objCab.ShapeParametersList[j].ParameterValueCab = insertProductMark.S;
                                break;
                            case "T":
                                objCab.ShapeParametersList[j].ParameterValueCab = insertProductMark.T;
                                break;
                            case "U":
                                objCab.ShapeParametersList[j].ParameterValueCab = insertProductMark.U;
                                break;
                            case "V":
                                objCab.ShapeParametersList[j].ParameterValueCab = insertProductMark.V;
                                break;
                            case "W":
                                objCab.ShapeParametersList[j].ParameterValueCab = insertProductMark.W;
                                break;
                            case "X":
                                objCab.ShapeParametersList[j].ParameterValueCab = insertProductMark.X;
                                break;
                            case "Y":
                                objCab.ShapeParametersList[j].ParameterValueCab = insertProductMark.Y;
                                break;
                            case "Z":
                                objCab.ShapeParametersList[j].ParameterValueCab = insertProductMark.Z;
                                break;
                        }
                    }
                    //ndsParList.Add(objCab.ShapeParametersList[j]);
                }


                if (objCab.CABProductMarkID == null || objCab.CABProductMarkID == 0)

                {
                    var result = _cABService.InsertToDbTCT(objCab,false,out ProductMarkId);
                    InsertBarmarkReturn returnResult = new InsertBarmarkReturn();
                    returnResult.Issuccess = result;
                    returnResult.ProductmarkId = ProductMarkId;

                    SaveFlag = true;
                    return Ok(returnResult);
                }
                else
                {
                    int minLen = 0;
                    int minHookLen = 0;
                    int minHookHt = 0;
                    bool flag = false;
                    var result = _cABService.UpdateCABProductMarking(objCab, out errorMessage, SEDetailingID, out minLen, out minHookLen, out minHookHt, out flag);
                    
                    InsertBarmarkReturn returnResult = new InsertBarmarkReturn();
                    returnResult.Issuccess = flag;
                    returnResult.ProductmarkId = insertProductMark.CabProductMarkID;

                    SaveFlag = true;
                    return Ok(returnResult);
                }
                //SaveFlag = true;
                //counter++;
                //if (counter == totalcount)
                //{
                //    return Ok(result);
                //}

                //var result = _cABService.InsertProductMark(objCab, SEDetailingID, out errorMessage);
                //Commented by Pankaj


               


            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }
        [HttpPost]
        [Route("/InsertProductMark/{SEDetailingID}")]
        public async Task<IActionResult> InsertProductMark([FromBody] InsertProductMarkDto insertProductMark, int SEDetailingID)
        {
            string errorMessage = "";

            bool SaveFlag = false;
            int ProductmarkID = 0;
            try
            {
                CABItem objCab = new CABItem();

                objCab.CABProductMarkName = insertProductMark.VCHCABPRODUCTMARKNAME;
                objCab.SEDetailingID = SEDetailingID;
                objCab.GroupMarkId = insertProductMark.INTGROUPMARKID;
                objCab.MemberQty = insertProductMark.INTMEMBERQTY;
                objCab.ShapeCode = insertProductMark.INTSHAPECODE.ToString();
                objCab.PinSizeID = insertProductMark.INTPINSIZEID;
                objCab.InvoiceLength = insertProductMark.NUMINVOICELENGTH;
                objCab.ProductionLength = insertProductMark.NUMPRODUCTIONLENGTH;
                objCab.InvoiceWeight = insertProductMark.NUMINVOICEWEIGHT;
                objCab.ProductionWeight = insertProductMark.NUMPRODUCTIONWEIGHT;
                ////////////////////////-----------------/////////////////

                objCab.Grade = insertProductMark.GRADE;
                objCab.Diameter = insertProductMark.INTDIAMETER;
                objCab.Coupler1Standard = insertProductMark.BARSTANDARD;
                objCab.ShapeType = insertProductMark.VCHSHAPETYPE;
                objCab.ShapeGroup = insertProductMark.VCHSHAPEGROUP;
                objCab.Coupler1Type = insertProductMark.COUPLERTYPE1;
                objCab.Coupler1 = insertProductMark.COUPLERMATERIAL1;
                objCab.Coupler1Standard = insertProductMark.COUPLERSTANDARD1;
                objCab.Coupler2Type = insertProductMark.COUPLERTYPE2;
                objCab.Coupler2 = insertProductMark.COUPLERMATERIAL2;
                objCab.Coupler2Standard = insertProductMark.COUPLERSTANDARD2;
                objCab.Status = insertProductMark.INTSTATUS;
                objCab.CustomerRemarks = insertProductMark.VCHCUSTREMARKS;
                objCab.ShapeImage = insertProductMark.VCHSHAPEIMAGE;
                objCab.BVBS = insertProductMark.VCHCAB_BVBS;
                objCab.PageNumber = insertProductMark.VCHPAGENUMBER;
                objCab.CommercialDesc = insertProductMark.VCHCOMMDESCRIPT;
                objCab.EnvLength = insertProductMark.NUMENVLENGTH;
                objCab.EnvWidth = insertProductMark.NUMENVWIDTH;
                objCab.EnvHeight = insertProductMark.NUMENVHEIGHT;
                objCab.NoOfBends = insertProductMark.INTNOOFBENDS;
                objCab.IsVariableBar = insertProductMark.BITVARIESBAR;
                objCab.Shape = insertProductMark.Shape;
                //objCab.ShapeParametersList = insertProductMark/*.*/Shape.ShapeParam;
               


                //   List<ShapeCode> shapeCodes =  _cABService.FilterShapeCode(objCab.ShapeCode,out errorMessage);

                // objCab.Shape = shapeCodes[0];

                var result = _cABService.InsertToDbTCT(objCab,false,out ProductmarkID);
                //SaveFlag = true;
                //counter++;
                //if (counter == totalcount)
                //{
                //    return Ok(result);
                //}

                //var result = _cABService.InsertProductMark(objCab, SEDetailingID, out errorMessage);
                //Commented by Pankaj


                SaveFlag = true;
                return Ok(result);


            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }

        [HttpGet]
        [Route("/GetCABProductMarkingDetailsByID/{SEDetailingID}")]
        public async Task<IActionResult> GetCABProductMarkingDetailsBySEDetailingID(int SEDetailingID)



        {
           


            
            string errorMessage = "";
            List<CABItem> getCABProdMarkDetails = _cABService.GetCABProductMarkingDetailsBySEDetailingID(SEDetailingID, ref errorMessage);

            List<OrderDetailsListModelsDto> orderDetailsListModelsDtos = new List<OrderDetailsListModelsDto>();

            foreach (var objCab in getCABProdMarkDetails)
            {
                OrderDetailsListModelsDto insertProductMark = new OrderDetailsListModelsDto();
              

                insertProductMark.BarMark = objCab.CABProductMarkName;
                objCab.SEDetailingID = SEDetailingID;
                insertProductMark.CabProductMarkID = objCab.CABProductMarkID;
                
                //objCab.GroupMarkId = intGroupMarkID;
                insertProductMark.BarMemberQty = 1;//objCab.MemberQty;
                insertProductMark.BarEachQty = objCab.Quantity;
                insertProductMark.BarTotalQty = objCab.Quantity;
                insertProductMark.BarShapeCode = objCab.ShapeCode;
                insertProductMark.PinSize = objCab.PinSizeID;
                insertProductMark.BarLength= objCab.InvoiceLength.ToString();
                //insertProductMark.NUMPRODUCTIONLENGTH= objCab.ProductionLength;
                insertProductMark.BarWeight=Convert.ToDecimal(objCab.InvoiceWeight);
                //insertProductMark.NUMPRODUCTIONWEIGHT=objCab.ProductionWeight;
                ////////////////////////-----------------/////////////////

                insertProductMark.BarType = objCab.Grade;
                insertProductMark.BarSize = objCab.Diameter;
                insertProductMark.ElementMark = objCab.DescScript;
                //objCab.Coupler1Standard = insertProductMark.BARSTANDARD;
                //objCab.ShapeType = insertProductMark.VCHSHAPETYPE;
                //objCab.ShapeGroup = insertProductMark.VCHSHAPEGROUP;
                //objCab.Coupler1Type = insertProductMark.COUPLERTYPE1;
                //objCab.Coupler1 = insertProductMark.COUPLERMATERIAL1;
                //objCab.Coupler1Standard = insertProductMark.COUPLERSTANDARD1;
                //objCab.Coupler2Type = insertProductMark.COUPLERTYPE2;
                //objCab.Coupler2 = insertProductMark.COUPLERMATERIAL2;
                //objCab.Coupler2Standard = insertProductMark.COUPLERSTANDARD2;
                //insertProductMark.sta = "";
                //objCab.CustomerRemarks = insertProductMark.VCHCUSTREMARKS;
                //objCab.ShapeImage = insertProductMark.VCHSHAPEIMAGE;
                //objCab.BVBS = insertProductMark.VCHCAB_BVBS;
                //objCab.PageNumber = insertProductMark.VCHPAGENUMBER;
                insertProductMark.ElementMark= objCab.DescScript;
                //objCab.EnvLength = insertProductMark.NUMENVLENGTH;
                //objCab.EnvWidth = insertProductMark.NUMENVWIDTH;
                //objCab.EnvHeight = insertProductMark.NUMENVHEIGHT;
                //objCab.NoOfBends = insertProductMark.INTNOOFBENDS;
                //insertProductMark.va = false;
                //objCab.Shape = insertProductMark.Shape;
                //objCab.ShapeParametersList = insertProductMark/*.*/Shape.ShapeParam;



                //List<ShapeCode> shapeCodes = _cABService.FilterShapeCode(objCab.ShapeCode, out errorMessage);

                //objCab.Shape = shapeCodes[0];

                objCab.ShapeParametersList = new List<ShapeParameter>();

                foreach (var shapeParam in objCab.Shape.ShapeParam)
                {
                    var options = new JsonSerializerOptions()
                    {
                        NumberHandling = JsonNumberHandling.AllowReadingFromString |
                                 JsonNumberHandling.WriteAsString
                    };

                    //  ShapeParameter shapeParam1 = new ShapeParameter();
                    string denemeJson = JsonSerializer.Serialize(shapeParam, options);
                    // Assuming shapeParam is an instance of DetailingService.Repositories.ShapeParameterDetailingService.Repositories.ShapeParameter shapeParam = new DetailingService.Repositories.ShapeParameter();
                    // Serialize shapeParam to a JSON stringstring denemeJson = JsonSerializer.Serialize(shapeParam, options);
                    // string denemeJson = JsonSerializer.Serialize<JsonElement>((JsonElement)shapeParam, options);



                    // deserialize

                    ShapeParameter shapeParamObj = JsonSerializer.Deserialize<ShapeParameter>(denemeJson, options);

                    

                    objCab.ShapeParametersList.Add(shapeParamObj);

                }

                var parameterValues = string.Empty;
                for (int j = 0; j < objCab.ShapeParametersList.Count(); j++)
                {

                    {
                        switch (objCab.ShapeParametersList[j].ParameterName.Trim().ToUpper())
                        {
                            case "A":

                                insertProductMark.A = objCab.ShapeParametersList[j].ParameterValueCab.ToString();
                                parameterValues = objCab.ShapeParametersList[j].ParameterName.Trim().ToUpper();
                                break;
                            case "B":
                                insertProductMark.B = objCab.ShapeParametersList[j].ParameterValueCab.ToString();
                                parameterValues = parameterValues +","+ objCab.ShapeParametersList[j].ParameterName.Trim().ToUpper();
                                break;
                            case "C":
                                insertProductMark.C = objCab.ShapeParametersList[j].ParameterValueCab.ToString();
                                parameterValues = parameterValues + "," + objCab.ShapeParametersList[j].ParameterName.Trim().ToUpper();

                                break;
                            case "D":
                                insertProductMark.D = objCab.ShapeParametersList[j].ParameterValueCab.ToString();
                                parameterValues = parameterValues + "," + objCab.ShapeParametersList[j].ParameterName.Trim().ToUpper();

                                break;
                            case "E":
                                insertProductMark.E = objCab.ShapeParametersList[j].ParameterValueCab.ToString();
                                parameterValues = parameterValues + "," + objCab.ShapeParametersList[j].ParameterName.Trim().ToUpper();

                                break;
                            case "F":
                                insertProductMark.F = objCab.ShapeParametersList[j].ParameterValueCab.ToString();
                                parameterValues = parameterValues + "," + objCab.ShapeParametersList[j].ParameterName.Trim().ToUpper();

                                break;
                            case "G":
                                insertProductMark.G = objCab.ShapeParametersList[j].ParameterValueCab.ToString();
                                parameterValues = parameterValues + "," + objCab.ShapeParametersList[j].ParameterName.Trim().ToUpper();

                                break;
                            case "H":
                                insertProductMark.H = objCab.ShapeParametersList[j].ParameterValueCab.ToString();
                                parameterValues = parameterValues + "," + objCab.ShapeParametersList[j].ParameterName.Trim().ToUpper();

                                break;
                            case "I":
                                insertProductMark.I = objCab.ShapeParametersList[j].ParameterValueCab.ToString();
                                parameterValues = parameterValues + "," + objCab.ShapeParametersList[j].ParameterName.Trim().ToUpper();

                                break;
                            case "J":
                                insertProductMark.J = objCab.ShapeParametersList[j].ParameterValueCab.ToString();
                                parameterValues = parameterValues + "," + objCab.ShapeParametersList[j].ParameterName.Trim().ToUpper();

                                break;
                            case "K":
                                insertProductMark.K = objCab.ShapeParametersList[j].ParameterValueCab.ToString();
                                parameterValues = parameterValues + "," + objCab.ShapeParametersList[j].ParameterName.Trim().ToUpper();

                                break;
                            case "L":
                                insertProductMark.L = objCab.ShapeParametersList[j].ParameterValueCab.ToString();
                                parameterValues = parameterValues + "," + objCab.ShapeParametersList[j].ParameterName.Trim().ToUpper();

                                break;
                            case "M":
                                insertProductMark.M = objCab.ShapeParametersList[j].ParameterValueCab.ToString();
                                parameterValues = parameterValues + "," + objCab.ShapeParametersList[j].ParameterName.Trim().ToUpper();

                                break;
                            case "N":
                                insertProductMark.N = objCab.ShapeParametersList[j].ParameterValueCab.ToString();
                                parameterValues = parameterValues + "," + objCab.ShapeParametersList[j].ParameterName.Trim().ToUpper();

                                break;
                            case "O":
                                insertProductMark.O = objCab.ShapeParametersList[j].ParameterValueCab.ToString();
                                parameterValues = parameterValues + "," + objCab.ShapeParametersList[j].ParameterName.Trim().ToUpper();

                                break;
                            case "P":
                                insertProductMark.P = objCab.ShapeParametersList[j].ParameterValueCab.ToString();
                                parameterValues = parameterValues + "," + objCab.ShapeParametersList[j].ParameterName.Trim().ToUpper();

                                break;
                            case "Q":
                                insertProductMark.Q = objCab.ShapeParametersList[j].ParameterValueCab.ToString();
                                parameterValues = parameterValues + "," + objCab.ShapeParametersList[j].ParameterName.Trim().ToUpper();

                                break;
                            case "R":
                                insertProductMark.R = objCab.ShapeParametersList[j].ParameterValueCab.ToString();
                                parameterValues = parameterValues + "," + objCab.ShapeParametersList[j].ParameterName.Trim().ToUpper();

                                break;
                            case "S":
                                insertProductMark.S = objCab.ShapeParametersList[j].ParameterValueCab.ToString();
                                parameterValues = parameterValues + "," + objCab.ShapeParametersList[j].ParameterName.Trim().ToUpper();

                                break;
                            case "T":
                                insertProductMark.T = objCab.ShapeParametersList[j].ParameterValueCab.ToString();
                                parameterValues = parameterValues +","+ objCab.ShapeParametersList[j].ParameterName.Trim().ToUpper();

                                break;
                            case "U":
                                insertProductMark.U = objCab.ShapeParametersList[j].ParameterValueCab.ToString();
                                parameterValues = parameterValues + "," + objCab.ShapeParametersList[j].ParameterName.Trim().ToUpper();

                                break;
                            case "V":
                                insertProductMark.V = objCab.ShapeParametersList[j].ParameterValueCab.ToString();
                                parameterValues = parameterValues + "," + objCab.ShapeParametersList[j].ParameterName.Trim().ToUpper();

                                break;
                            case "W":
                                insertProductMark.W = objCab.ShapeParametersList[j].ParameterValueCab.ToString();
                                parameterValues = parameterValues + "," + objCab.ShapeParametersList[j].ParameterName.Trim().ToUpper();

                                break;
                            case "X":
                                insertProductMark.X = objCab.ShapeParametersList[j].ParameterValueCab.ToString();
                                parameterValues = parameterValues + "," + objCab.ShapeParametersList[j].ParameterName.Trim().ToUpper();

                                break;
                            case "Y":
                                insertProductMark.Y = objCab.ShapeParametersList[j].ParameterValueCab.ToString();
                                parameterValues = parameterValues + "," + objCab.ShapeParametersList[j].ParameterName.Trim().ToUpper();

                                break;
                            case "Z":
                                insertProductMark.Z = objCab.ShapeParametersList[j].ParameterValueCab.ToString();
                                parameterValues = parameterValues + "," + objCab.ShapeParametersList[j].ParameterName.Trim().ToUpper();

                                break;
                        }
                    }
                  
                }

                insertProductMark.shapeParameters = parameterValues;
                orderDetailsListModelsDtos.Add(insertProductMark);

            }

            var result = orderDetailsListModelsDtos;
            if (errorMessage == "")
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(errorMessage);
            }
        }



        [HttpPost]
        [Route("/UpdateCabProductMark/{SEDetailingID}")]
        public async Task<IActionResult> UpdateCabProductMark(CABItem objCabproductMarking,  int SEDetailingID)
        {
            string errorMessage = "";
            try
            {
                int minLen=0;
                int minHookLen=0;
                int minHookHt=0;
                bool flag = false;

                var result = _cABService.UpdateCABProductMarking(objCabproductMarking, out errorMessage, SEDetailingID, out minLen, out minHookLen, out minHookHt, out flag);

                if (errorMessage!="")
                {
                    return BadRequest(errorMessage);

                }
                return Ok(result);



               }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("/UpdateBarMark/{SEDetailingID}")]
        public Task<IActionResult> UpdateBarMark([FromBody] OrderDetailsListModelsDto UpdateBarkMark, int SEDetailingID)
        {
            string errorMessage = "";
            try
            {
                int minLen = 0;
                int minHookLen = 0;
                int minHookHt = 0;



                CABItem objCab = new CABItem();


                objCab.CABProductMarkName = UpdateBarkMark.BarMark;
                objCab.CABProductMarkID = UpdateBarkMark.BarID;
                objCab.SEDetailingID = SEDetailingID;
                //objCab.GroupMarkId = UpdateBarkMark;
                objCab.MemberQty = UpdateBarkMark.BarMemberQty;
                objCab.ShapeCode = UpdateBarkMark.BarShapeCode.ToString();
                objCab.PinSizeID = UpdateBarkMark.PinSize;
           
                ////////////////////////-----------------/////////////////

                objCab.Grade = UpdateBarkMark.BarType;
                objCab.Diameter = UpdateBarkMark.BarSize;
               
                objCab.Status = "";
            
                objCab.IsVariableBar = false;
               



                List<ShapeCode> shapeCodes = _cABService.FilterShapeCode(objCab.ShapeCode, out errorMessage);

                objCab.Shape = shapeCodes[0];

                objCab.ShapeParametersList = new List<ShapeParameter>();

                foreach (var shapeParam in objCab.Shape.ShapeParam)
                {
                    var options = new JsonSerializerOptions()
                    {
                        NumberHandling = JsonNumberHandling.AllowReadingFromString |
                                 JsonNumberHandling.WriteAsString
                    };
                    string denemeJson = JsonSerializer.Serialize<JsonElement>((JsonElement)shapeParam, options);



                    // deserialize

                    ShapeParameter shapeParamObj = JsonSerializer.Deserialize<ShapeParameter>(denemeJson, options);



                    objCab.ShapeParametersList.Add(shapeParamObj);
                }


                for (int j = 0; j < objCab.ShapeParametersList.Count(); j++)
                {

                    {
                        switch (objCab.ShapeParametersList[j].ParameterName.Trim().ToUpper())
                        {
                            case "A":
                                objCab.ShapeParametersList[j].ParameterValue = Convert.ToInt32(UpdateBarkMark.A);
                                break;
                            case "B":
                                objCab.ShapeParametersList[j].ParameterValue = Convert.ToInt32(UpdateBarkMark.B);
                                break;
                            case "C":
                                objCab.ShapeParametersList[j].ParameterValue = Convert.ToInt32(UpdateBarkMark.C);
                                break;
                            case "D":
                                objCab.ShapeParametersList[j].ParameterValue = Convert.ToInt32(UpdateBarkMark.D);
                                break;
                            case "E":
                                objCab.ShapeParametersList[j].ParameterValue = Convert.ToInt32(UpdateBarkMark.E);
                                break;
                            case "F":
                                objCab.ShapeParametersList[j].ParameterValue = Convert.ToInt32(UpdateBarkMark.F);
                                break;
                            case "G":
                                objCab.ShapeParametersList[j].ParameterValue = Convert.ToInt32(UpdateBarkMark.G);
                                break;
                            case "H":
                                objCab.ShapeParametersList[j].ParameterValue = Convert.ToInt32(UpdateBarkMark.H);
                                break;
                            case "I":
                                objCab.ShapeParametersList[j].ParameterValue = Convert.ToInt32(UpdateBarkMark.I);
                                break;
                            case "J":
                                objCab.ShapeParametersList[j].ParameterValue = Convert.ToInt32(UpdateBarkMark.J);
                                break;
                            case "K":
                                objCab.ShapeParametersList[j].ParameterValue = Convert.ToInt32(UpdateBarkMark.K);
                                break;
                            case "L":
                                objCab.ShapeParametersList[j].ParameterValue = Convert.ToInt32(UpdateBarkMark.L);
                                break;
                            case "M":
                                objCab.ShapeParametersList[j].ParameterValue = Convert.ToInt32(UpdateBarkMark.M);
                                break;
                            case "N":
                                objCab.ShapeParametersList[j].ParameterValue = Convert.ToInt32(UpdateBarkMark.N);
                                break;
                            case "O":
                                objCab.ShapeParametersList[j].ParameterValue = Convert.ToInt32(UpdateBarkMark.O);
                                break;
                            case "P":
                                objCab.ShapeParametersList[j].ParameterValue = Convert.ToInt32(UpdateBarkMark.P);
                                break;
                            case "Q":
                                objCab.ShapeParametersList[j].ParameterValue = Convert.ToInt32(UpdateBarkMark.Q);
                                break;
                            case "R":
                                objCab.ShapeParametersList[j].ParameterValue = Convert.ToInt32(UpdateBarkMark.R);
                                break;
                            case "S":
                                objCab.ShapeParametersList[j].ParameterValue = Convert.ToInt32(UpdateBarkMark.S);
                                break;
                            case "T":
                                objCab.ShapeParametersList[j].ParameterValue = Convert.ToInt32(UpdateBarkMark.T);
                                break;
                            case "U":
                                objCab.ShapeParametersList[j].ParameterValue = Convert.ToInt32(UpdateBarkMark.U);
                                break;
                            case "V":
                                objCab.ShapeParametersList[j].ParameterValue = Convert.ToInt32(UpdateBarkMark.V);
                                break;
                            case "W":
                                objCab.ShapeParametersList[j].ParameterValue = Convert.ToInt32(UpdateBarkMark.W);
                                break;
                            case "X":
                                objCab.ShapeParametersList[j].ParameterValue = Convert.ToInt32(UpdateBarkMark.X);
                                break;
                            case "Y":
                                objCab.ShapeParametersList[j].ParameterValue = Convert.ToInt32(UpdateBarkMark.Y);
                                break;
                            case "Z":
                                objCab.ShapeParametersList[j].ParameterValue = Convert.ToInt32(UpdateBarkMark.Z);
                                break;
                        }
                    }
                    //ndsParList.Add(objCab.ShapeParametersList[j]);
                }
                bool flag = false;
            
                var result = _cABService.UpdateCABProductMarking(objCab, out errorMessage, SEDetailingID, out minLen, out minHookLen, out minHookHt, out flag);

                if (errorMessage != "")
                {
                    return Task.FromResult<IActionResult>(BadRequest(errorMessage));

                }
                return Task.FromResult<IActionResult>(Ok(result));



            }
            catch (Exception ex)
            {
                return Task.FromResult<IActionResult>(BadRequest(ex));
            }
        }


        [HttpDelete]
        [Route("/DeleteCABProductMarkingDetailsByID/{SEDetailingID}/{CABProductMarkID}/{CABProductMarkName}")]
        public async Task<IActionResult> DeleteCABProductMarkingDetailsByID(int SEDetailingID, int CABProductMarkID, string CABProductMarkName)
        {

            CABItem objCabMarkingDetail = new CABItem();
            string errorMsg = "";



            objCabMarkingDetail.CABProductMarkID = CABProductMarkID;
            objCabMarkingDetail.SEDetailingID = SEDetailingID;
            objCabMarkingDetail.CABProductMarkName = CABProductMarkName;


            var DeleteCabResult =  _cABService.DeleteCabMarkingDetail(objCabMarkingDetail,out errorMsg);
            return Ok(DeleteCabResult);



        }

    }


}

