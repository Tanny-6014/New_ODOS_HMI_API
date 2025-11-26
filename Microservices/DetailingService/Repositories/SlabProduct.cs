

using Dapper;
using DetailingService.Constants;
using DetailingService.Context;
using DetailingService.Dtos;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SharedCache.WinServiceCommon.Provider.Cache;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DetailingService.Repositories
{
    public class SlabProduct
    {
        private DetailingApplicationContext _dbContext;
        private readonly IConfiguration _configuration;
        // private string connectionString;

        private string connectionString = "Server=NSQADB5\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=MES_USER;Password=Mes@123;Connection Timeout=3600";
    //private string connectionString = "Server=nsprddb10\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";

        public int ProductMarkId { get; set; }
        public int StructureMarkId { get; set; }
        public int ProductCodeId { get; set; }
        public string ProductMarkingName { get; set; }
        public string ShapeParam { get; set; }
        public int MWLength { get; set; }
        public int CWLength { get; set; }
        public int NoOfMainWire { get; set; }
        public int NoOfCrossWire { get; set; }
        public int ProductionMWLength { get; set; }
        public int ProductionCWLength { get; set; }
        public int ProductionMWQty { get; set; }
        public int ProductionCWQty { get; set; }
        public int MemberQty { get; set; }
        public int MO1 { get; set; }
        public int MO2 { get; set; }
        public int CO1 { get; set; }
        public int CO2 { get; set; }
        public int ProductionMO1 { get; set; }
        public int ProductionMO2 { get; set; }
        public int ProductionCO1 { get; set; }
        public int ProductionCO2 { get; set; }
        public int MWSpacing { get; set; }
        public int CWSpacing { get; set; }
        //public int ProductionMWLength { get; set; }
        //public int ProductionCWLength { get; set; }
        public int PinSize { get; set; }
        public double TheoraticalWeight { get; set; }
        public double InvoiceArea { get; set; }
        public double NetWeight { get; set; }
        public double ProductionWeight { get; set; }
        public ShapeCode shapecode { get; set; }

        public double EnvelopeLength { get; set; }
        public double EnvelopeWidth { get; set; }
        public double EnvelopeHeight { get; set; }
        public bool ProduceIndicator { get; set; }
        public string BOMIndicator { get; set; }
        public string ParamValues { get; set; }
        public string BOMDrawingPath { get; set; }
        public string MWBVBSString { get; set; }
        public string CWBVBSString { get; set; }
        public double InvoiceMWWeight { get; set; }
        public double InvoiceCWWeight { get; set; }
        public double ProductionMWWeight { get; set; }
        public double ProductionCWWeight { get; set; }
        public int Deleteflag { get; set; }
        public int ProductValidator { get; set; }
        public string MWPitch { get; set; }
        public string CWPitch { get; set; }
        public int MWFlag { get; set; }
        public int CWFlag { get; set; }
        public bool BendingCheckInd { get; set; }
        public SlabProduct()
        {



            StructureMarkId = 4343089;
            ProductCodeId = 3834;
            ProductMarkingName = "test";
            ShapeParam = "test";
            MWLength = 2;
            CWLength = 2;
            NoOfMainWire = 2;
            NoOfCrossWire = 2;
            ProductionMWLength = 2;
            ProductionCWLength = 2;
            ProductionMWQty = 2;
            ProductionCWQty = 2;
            MemberQty = 2;
            MO1 = 2;
            MO2 = 2;
            CO1 = 2;
            CO2 = 2;
            ProductionMO1 = 2;
            ProductionMO2 = 2;
            ProductionCO1 = 2;
            ProductionCO2 = 2;
            MWSpacing = 2;
            CWSpacing = 2;
            ProductMarkId = 5704879;
            TheoraticalWeight = 2;
            InvoiceArea = 2;
            NetWeight = 2;
            ProductionWeight = 2;

            EnvelopeLength = 2;
            EnvelopeWidth = 2;
            EnvelopeHeight = 2;
            ProduceIndicator = true;



            BOMIndicator = "Test";
            ParamValues = "Test";
            BOMDrawingPath = "Test";
            MWBVBSString = "BF2D@Gl3700 @w0@";
            CWBVBSString = "BF2D@Gl2600@w0@";
            InvoiceMWWeight = 2;
            InvoiceCWWeight = 2;
            ProductionMWWeight = 3;
            ProductionCWWeight = 3;
            Deleteflag = 0;
            ProductValidator = 0;



            MWPitch = "String";

            CWPitch = "String";
            MWFlag = 0;
            CWFlag = 0;
            BendingCheckInd = true;






        }
        public SlabProduct(DetailingApplicationContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;

            connectionString = _configuration.GetConnectionString(SystemConstant.DefaultDBConnection);
        }
     
        public List<SlabProduct> SlabProductByStructureMarkId_Get(int StructureMarkId, int StructureElementId)
        {
           // DBManager dbManager = new DBManager();
            List<SlabProduct> SlabProductList = new List<SlabProduct>();
            DataSet dsSlabProductMark = new DataSet();
            IEnumerable<SlabProductMarkDto> SlabProductMark;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTSTRUCTUREMARKID", StructureMarkId);
                    dynamicParameters.Add("@INTSTRUCTUREELEMENTTYPEID", StructureElementId);
                    SlabProductMark = sqlConnection.Query<SlabProductMarkDto>(SystemConstant.SlabProductByStructureMarkId_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    //Vanita
                    if (SlabProductMark.Count() > 0)
                    {
                        DataTable dt = ConvertToDataTable.ToDataTable(SlabProductMark);
                        dsSlabProductMark.Tables.Add(dt);

                        if (dsSlabProductMark != null && dsSlabProductMark.Tables.Count != 0)
                        {
                            foreach (DataRowView drvBeam in dsSlabProductMark.Tables[0].DefaultView)
                            {
                                ShapeCode objShapeCode = new ShapeCode();
                                objShapeCode.ShapeID = Convert.ToInt32(drvBeam["INTSHAPEID"]);
                                objShapeCode.ShapeCodeName = Convert.ToString(drvBeam["CHRSHAPECODE"]);
                                objShapeCode.MeshShapeGroup = Convert.ToString(drvBeam["MESHSHAPE"]);
                                objShapeCode.BendIndicator = Convert.ToBoolean(drvBeam["BITBENDINDICATOR"]);
                                objShapeCode.CreepDeductAtMO1 = Convert.ToBoolean(drvBeam["BITCREEPDEDUCTATMO1"]);
                                objShapeCode.CreepDeductAtCO1 = Convert.ToBoolean(drvBeam["BITCREEPDEDUCTATCO1"]);
                                objShapeCode.MOCO = drvBeam["CHRMOCO"].ToString();
                                objShapeCode.NoOfBends = Convert.ToInt32(drvBeam["NOOFBENDS"]);
                                objShapeCode.MWBendPosition = Convert.ToInt32(drvBeam["INTMWBENDPOSITION"]);
                                objShapeCode.CWBendPosition = Convert.ToInt32(drvBeam["INTCWBENDPOSITION"]);
                                objShapeCode.NoOfMWBend = Convert.ToInt32(drvBeam["INTNOOFMWBEND"]);
                                objShapeCode.NoOfCWBend = Convert.ToInt32(drvBeam["INTNOOFCWBEND"]);

                                ShapeParameterCollection shapeParameterCollection = new ShapeParameterCollection();
                                ShapeParameter listSlabShapeParameter = new ShapeParameter { };
                               
                                 shapeParameterCollection = shapeParameterCollection.SlabShapeParameter_Get();


                                ShapeParameterCollection strucParamCollFilter = new ShapeParameterCollection();



                                foreach (ShapeParameter shapeParamCollection in shapeParameterCollection)
                                {
                                    if (shapeParamCollection.ShapeId == Convert.ToInt32(drvBeam["INTSHAPEID"]))
                                    {

                                        strucParamCollFilter.Add(shapeParamCollection);
                                    }

                                }

                                objShapeCode.ShapeParam = strucParamCollFilter;

                                if (drvBeam["PRODUCEINDICATOR"].ToString().Trim().ToUpper() == "YES")
                                {
                                    ProduceIndicator = true;
                                }
                                else if (drvBeam["PRODUCEINDICATOR"].ToString().Trim().ToUpper() == "NO")
                                {
                                    ProduceIndicator = false;
                                }

                                SlabProduct slabProduct = new SlabProduct
                                {
                                    ProductMarkId = Convert.ToInt32(drvBeam["INTPRODUCTMARKID"]),
                                    StructureMarkId = Convert.ToInt32(drvBeam["INTSTRUCTUREMARKID"]),
                                    ProductCodeId = Convert.ToInt32(drvBeam["INTPRODUCTCODEID"]),
                                    ProductMarkingName = Convert.ToString(drvBeam["VCHPRODUCTMARKINGNAME"]),
                                    ShapeParam = Convert.ToString(drvBeam["PARAMVALUES"]),
                                    MWLength = Convert.ToInt32(drvBeam["MHMWLENGTH"]),
                                    CWLength = Convert.ToInt32(drvBeam["MHCWLENGTH"]),
                                    NoOfMainWire = Convert.ToInt32(drvBeam["MAINWIRENO"]),
                                    NoOfCrossWire = Convert.ToInt32(drvBeam["CROSSWIRENO"]),
                                    ProductionMWLength = Convert.ToInt32(drvBeam["NUMPRODUCTIONMWLENGTH"]),
                                    ProductionCWLength = Convert.ToInt32(drvBeam["NUMPRODUCTIONCWLENGTH"]),
                                    ProductionMWQty = Convert.ToInt32(drvBeam["MHNOMW"]),
                                    ProductionCWQty = Convert.ToInt32(drvBeam["MHNOCW"]),
                                    MemberQty = Convert.ToInt32(drvBeam["SWQUANTITY"]),
                                    MO1 = Convert.ToInt32(drvBeam["MHMO1"]),
                                    MO2 = Convert.ToInt32(drvBeam["MHMO2"]),
                                    CO1 = Convert.ToInt32(drvBeam["MHCO1"]),
                                    CO2 = Convert.ToInt32(drvBeam["MHCO2"]),
                                    ProductionMO1 = Convert.ToInt32(drvBeam["MHPRODMO1"]),
                                    ProductionMO2 = Convert.ToInt32(drvBeam["MHPRODMO2"]),
                                    ProductionCO1 = Convert.ToInt32(drvBeam["MHPRODCO1"]),
                                    ProductionCO2 = Convert.ToInt32(drvBeam["MHPRODCO2"]),
                                    MWSpacing = Convert.ToInt32(drvBeam["INTMWSPACING"]),
                                    CWSpacing = Convert.ToInt32(drvBeam["INTCWSPACING"]),
                                    PinSize = Convert.ToInt32(drvBeam["MHPINDIA"]),
                                    TheoraticalWeight = Convert.ToDouble(drvBeam["NUMTHEORATICALWEIGHT"]),
                                    InvoiceArea = Convert.ToDouble(drvBeam["NUMINVOICEAREA"]),
                                    NetWeight = Convert.ToDouble(drvBeam["NUMNETWEIGHT"]),
                                    ProductionWeight = Convert.ToDouble(drvBeam["NUMPRODUCTIONWEIGHT"]),
                                    shapecode = objShapeCode,
                                    EnvelopeLength = Convert.ToDouble(drvBeam["INTENVELOPLENGTH"]),
                                    EnvelopeWidth = Convert.ToDouble(drvBeam["INTENVELOPWIDTH"]),
                                    EnvelopeHeight = Convert.ToDouble(drvBeam["INTENVELOPHEIGHT"]),
                                    ProduceIndicator = ProduceIndicator,
                                    BOMIndicator = Convert.ToString(drvBeam["BOMIND"]),
                                    ParamValues = Convert.ToString(drvBeam["PARAMVALUES"]),
                                    MWBVBSString = Convert.ToString(drvBeam["NVCHMWBVBSSTRING"]),
                                    CWBVBSString = Convert.ToString(drvBeam["NVCHCWBVBSSTRING"]),
                                    ProductValidator = Convert.ToInt32(drvBeam["INTPRODUCTVALIDATOR"]),
                                    //BendingCheckInd = Convert.ToBoolean(drvBeam["BENDINGCHECK"])
                                };
                                SlabProductList.Add(slabProduct);
                            }

                        }
                    } 
                }
            }
             catch (Exception ex)
            {

                throw ex;
            }
            return SlabProductList;
        }

        public bool Save(int UserId, int StructureElementId)
        {
            bool isSuccess = false;
            string ProductGenerationOutput = null;
            object intProductMarkId = null;
           // DBManager dbManager = new DBManager();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intStructureMarkId", StructureMarkId);
                    dynamicParameters.Add("@intProductCodeId", ProductCodeId);
                    dynamicParameters.Add("@vchProductMarkingName", ProductMarkingName);
                    dynamicParameters.Add("@intShapeCodeId", shapecode.ShapeID);
                    dynamicParameters.Add("@numInvoiceMWLength", MWLength);
                    dynamicParameters.Add("@numInvoiceCWLength", CWLength);
                    dynamicParameters.Add("@intInvoiceMainQty", NoOfMainWire);
                    dynamicParameters.Add("@intInvoiceCrossQty", NoOfCrossWire);
                    dynamicParameters.Add("@numProductionMWLength", ProductionMWLength);
                    dynamicParameters.Add("@numProductionCWLength", ProductionCWLength);
                    dynamicParameters.Add( "@intProductionMainQty", ProductionMWQty);
                    dynamicParameters.Add( "@intProductionCrossQty", ProductionCWQty);
                    dynamicParameters.Add( "@INTINVOICEMO1", MO1);
                    dynamicParameters.Add( "@INTINVOICEMO2", MO2);
                    dynamicParameters.Add( "@INTINVOICECO1", CO1);
                    dynamicParameters.Add( "@INTINVOICECO2", CO2);
                    dynamicParameters.Add( "@INTPRODUCTIONMO1", ProductionMO1);
                    dynamicParameters.Add( "@INTPRODUCTIONMO2", ProductionMO2);
                    dynamicParameters.Add( "@INTPRODUCTIONCO1", ProductionCO1);
                    dynamicParameters.Add( "@INTPRODUCTIONCO2", ProductionCO2);
                    dynamicParameters.Add( "@INTMWSPACING", MWSpacing);
                    dynamicParameters.Add( "@INTCWSPACING", CWSpacing);
                    dynamicParameters.Add( "@SITPINSIZE", PinSize);
                    dynamicParameters.Add( "@XMLRESULT", "");
                    dynamicParameters.Add( "@NUMINVOICEMWWEIGHT", InvoiceMWWeight);
                    dynamicParameters.Add( "@NUMINVOICECWWEIGHT", InvoiceCWWeight);
                    dynamicParameters.Add( "@NUMINVOICEAREA", InvoiceArea);
                    dynamicParameters.Add( "@NUMTHEORATICALWEIGHT", TheoraticalWeight);
                    dynamicParameters.Add( "@NUMNETWEIGHT", NetWeight);
                    dynamicParameters.Add( "@INTMEMBERQTY", MemberQty);
                    dynamicParameters.Add( "@NUMPRODUCTIONMWWEIGHT", ProductionMWWeight);
                    dynamicParameters.Add( "@NUMPRODUCTIONCWWEIGHT", ProductionCWWeight);
                    dynamicParameters.Add( "@NUMPRODUCTIONWEIGHT", ProductionWeight);
                    dynamicParameters.Add( "@PARAMVALUES", ParamValues);
                    dynamicParameters.Add( "@INTENVELOPLENGTH", EnvelopeLength);
                    dynamicParameters.Add( "@INTENVELOPWIDTH", EnvelopeWidth);
                    dynamicParameters.Add( "@INTENVELOPHEIGHT", EnvelopeHeight);
                    dynamicParameters.Add( "@INTSTRUCTUREELEMENTID", StructureElementId);
                    dynamicParameters.Add( "@INTPRODUCTMARKID", ProductMarkId);
                    dynamicParameters.Add( "@INTUSERID", UserId);
                    if (ProduceIndicator == true)
                    {
                        dynamicParameters.Add( "@CHPRODUCEINDICATOR", "Yes");
                    }
                    else
                    {
                        dynamicParameters.Add( "@CHPRODUCEINDICATOR", "No");
                    }
                    dynamicParameters.Add( "@BOMDrawingPath", BOMDrawingPath);
                    dynamicParameters.Add( "@NVCHMWBVBSSTRING", MWBVBSString);
                    dynamicParameters.Add( "@NVCHCWBVBSSTRING", CWBVBSString);
                    //if (MWPitch == string.Empty || MWPitch == "")
                    //{
                    //    dynamicParameters.Add((44, "@MWPITCH", DBNull.Value);
                    //}
                    //else
                    //{
                    //    dynamicParameters.Add((44, "@MWPITCH", MWPitch);
                    //}
                    //dbManager.AddParameters(45, "@MWFLAG", MWFlag);
                    //if (CWPitch == string.Empty || CWPitch == "")
                    //{
                    //    dynamicParameters.Add((46, "@CWPITCH", DBNull.Value);
                    //}
                    //else
                    //{
                    //    dynamicParameters.Add((46, "@CWPITCH", CWPitch);
                    //}
                    //dbManager.AddParameters(47, "@CWFLAG", CWFlag);
                    //if (BendingCheckInd == true)
                    //{
                    //    dynamicParameters.Add((48, "@BENDINGCHECK", 1);
                    //}
                    //else
                    //{
                    //    dynamicParameters.Add((48, "@BENDINGCHECK", 0);
                    //}

                    //object intProductMarkId = dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_SlabProductMarkingDetails_InsUpd");
                    // ProductGenerationOutput = Convert.ToString((dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_SlabProductMarkingDetails_InsUpd")));
                    ProductGenerationOutput = sqlConnection.QueryFirstOrDefault<string>(SystemConstant.SlabProductMarkingDetails_InsUpd, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();


                    if (ProductGenerationOutput.Contains(","))
                    {
                        string[] Output = ProductGenerationOutput.Split(',');
                        intProductMarkId = Convert.ToInt32(Output[0]);
                        ProductValidator = Convert.ToInt16(Output[1]);
                    }

                    ProductMarkId = Convert.ToInt32(intProductMarkId);
                }
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return isSuccess;
        }

        public bool DeleteSlabProductMark(int ProductMarkId, out string errorMessage)
        {
            bool isSuccess = false;
         
            object Postedvalidate = null;
             errorMessage = "";
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@PRODUCTMARKID", ProductMarkId);
                    dynamicParameters.Add("@DELETEFLAG", 0);
                    Postedvalidate = sqlConnection.QueryFirstOrDefault<bool>(SystemConstant.SlabProductMark_Delete, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    if (Convert.ToInt32(Postedvalidate) == 0)
                    {
                       //isSuccess = false;
                        errorMessage = "POSTED";
                    }
                    else if (Convert.ToInt32(Postedvalidate) == 1)
                    {
                        isSuccess = true;
                    }
                    else
                    {
                        throw new Exception("Error in deleting Slab Product");
                    }
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
          
            return isSuccess;
        }

        public bool DeleteSlabProductMarkByStructureMarkId(int StructureMarkId,out string errorMessage)
        {
            bool isSuccess = false;
           // DBManager dbManager = new DBManager();
            object Postedvalidate = null;
            errorMessage = "";
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTSTRUCTUREMARKID", StructureMarkId);
                  //  Postedvalidate = dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_SlabProductMarkByStructureMarkId_Delete");
                    Postedvalidate = sqlConnection.QueryFirstOrDefault<bool>(SystemConstant.SlabProductMarkByStructureMarkId_Delete, dynamicParameters, commandType: CommandType.StoredProcedure);


                    if (Convert.ToInt32(Postedvalidate) == 0)
                    {
                        errorMessage = "POSTED";
                    }
                    else if (Convert.ToInt32(Postedvalidate) == 1)
                    {
                        isSuccess = true;
                    }
                    else
                    {
                        throw new Exception("Error in deleting Slab Product");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
          
            return isSuccess;
        }

        public List<SlabProduct> GetOverHang(int ParamSetNumber, int ProjectId, int StructureElementId, int ProductTypeId, SlabProduct slabproddto)
        {
           // DBManager dbManager = new DBManager();
            List<SlabProduct> OverHangList = new List<SlabProduct>();
            DataSet dsOverHang = new DataSet();
            IEnumerable<listGetOverHangDto> listGetOverHangDto;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@decMWLength", ProductionMWLength);
                    dynamicParameters.Add("@decCWLength", ProductionCWLength);
                    dynamicParameters.Add("@intMWSpace", MWSpacing);
                    dynamicParameters.Add("@intCWSpace", CWSpacing);
                    dynamicParameters.Add("@tntParamSetNumber", ParamSetNumber);
                    dynamicParameters.Add("@intProjectId", ProjectId);
                    dynamicParameters.Add("@intStructureElementId", StructureElementId);
                    dynamicParameters.Add("@sitProductTypeId", ProductTypeId);
                    //dsOverHang = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_OverHang_Get");
                   
                    listGetOverHangDto = sqlConnection.Query<listGetOverHangDto>(SystemConstant.OverHang_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (listGetOverHangDto.Count() > 0)
                    {
                        DataTable dt = ConvertToDataTable.ToDataTable(listGetOverHangDto);
                        dsOverHang.Tables.Add(dt);
                    }


                    if (dsOverHang != null && dsOverHang.Tables.Count != 0)
                    {
                        foreach (DataRowView drvOverHang in dsOverHang.Tables[0].DefaultView)
                        {
                            SlabProduct slabProduct = new SlabProduct
                            {
                                MO1 = Convert.ToInt32(drvOverHang["MO1"]),
                                MO2 = Convert.ToInt32(drvOverHang["MO2"]),
                                CO1 = Convert.ToInt32(drvOverHang["CO1"]),
                                CO2 = Convert.ToInt32(drvOverHang["CO2"])
                            };
                            OverHangList.Add(slabProduct);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return OverHangList;
        }

        //added by vidya 

        public bool DeleteSlabProductMark(out string errorMessage)
        {
            bool isSuccess = false;
           
            object Postedvalidate = null;
            errorMessage = "";
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@PRODUCTMARKID", ProductMarkId);
                    dynamicParameters.Add("@DELETEFLAG", 0);
                    Postedvalidate = sqlConnection.QueryFirstOrDefault<bool>(SystemConstant.SlabProductMark_Delete, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    if (Convert.ToInt32(Postedvalidate) == 0)
                    {
                        //isSuccess = false;
                        errorMessage = "POSTED";
                    }
                    else if (Convert.ToInt32(Postedvalidate) == 1)
                    {
                        isSuccess = true;
                    }
                    else
                    {
                        throw new Exception("Error in deleting Slab Product");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
            return isSuccess;
        }

    }

}