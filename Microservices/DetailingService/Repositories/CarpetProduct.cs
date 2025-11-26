using Dapper;
using DetailingService.Constants;
using DetailingService.Context;
using DetailingService.Dtos;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using System.Data;

namespace DetailingService.Repositories
{
    public class CarpetProduct
    {
        private DetailingApplicationContext _dbContext;
        private readonly IConfiguration _configuration;



        private string connectionString = "Server=NSQADB5\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=MES_USER;Password=Mes@123;Connection Timeout=3600";
   // private string connectionString = "Server=nsprddb10\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";

        #region Properties

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
        public string ?BOMDrawingPath { get; set; }
        public string MWBVBSString { get; set; }
        public string CWBVBSString { get; set; }
        public double InvoiceMWWeight { get; set; }
        public double InvoiceCWWeight { get; set; }
        public double ProductionMWWeight { get; set; }
        public double ProductionCWWeight { get; set; }
        public int Deleteflag { get; set; }
        public int ProductValidator { get; set; }
        public string ?MWPitch { get; set; }
        public string ?CWPitch { get; set; }
        public int MWFlag { get; set; }
        public int CWFlag { get; set; }
        public bool BendingCheckInd { get; set; }

        public int mainlngtdecrsoffset_a { get; set; } // FOR C,D,E BOM
        public int mainlngtdecrsoffset_b { get; set; } // FOR C,D,E BOM
        public int mainlngtdecrsoffset_c { get; set; } // FOR C,D,E BOM

        public int mainlngtdecrsoffset_d { get; set; } // FOR G BOM (Subhankar)
        public int mainlngtdecrsoffset_e { get; set; } // FOR G BOM (Subhankar)

        public double MwMeterRunVal { get; set; }// Added for E BOM to get MW meter run value 

        #endregion


        public CarpetProduct()
        {

        }
        public CarpetProduct(DetailingApplicationContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;

            connectionString = _configuration.GetConnectionString(SystemConstant.DefaultDBConnection);
        }
        public List<CarpetProduct> CarpetProductByStructureMarkId_Get(int StructureMarkId, int StructureElementId)
        {
           
            List<CarpetProduct> CarpetProductList = new List<CarpetProduct>();
            DataSet dsCarpetProductMark = new DataSet();
            IEnumerable<carpetProductMarkDto> SlabProductMark;

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTSTRUCTUREMARKID", StructureMarkId);
                    dynamicParameters.Add("@INTSTRUCTUREELEMENTTYPEID", StructureElementId);

                    SlabProductMark = sqlConnection.Query<carpetProductMarkDto>(SystemConstant.CarpetProductByStructureMarkId_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (SlabProductMark.Count() > 0)
                    {
                        DataTable dt = ConvertToDataTable.ToDataTable(SlabProductMark);
                        dsCarpetProductMark.Tables.Add(dt);

                        if (dsCarpetProductMark != null && dsCarpetProductMark.Tables.Count != 0)
                        {
                            foreach (DataRowView drvBeam in dsCarpetProductMark.Tables[0].DefaultView)
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
                                ShapeParameter listCarpetShapeParameter = new ShapeParameter { };
                                
                                shapeParameterCollection = shapeParameterCollection.CarpetShapeParameter_Get();

                                
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

                                CarpetProduct CarpetProduct = new CarpetProduct
                                {
                                    ProductMarkId = Convert.ToInt32(drvBeam["INTPRODUCTMARKID"]),
                                    //StructureMarkId = Convert.ToInt32(drvBeam["INTSTRUCTUREMARKID"]),
                                    StructureMarkId = Convert.ToInt32(drvBeam["INTCARPETSTRUCTUREMARKID"]),
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
                                CarpetProductList.Add(CarpetProduct);
                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return CarpetProductList;
        }

        public bool Save(int UserId, int StructureElementId)
        {
            bool isSuccess = false;
            string ProductGenerationOutput = null;
            int intProductMarkId = 0;

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intStructureMarkId", StructureMarkId);
                    dynamicParameters.Add("@intProductCodeId", ProductCodeId);
                    dynamicParameters.Add("@vchProductMarkingName", this.ProductMarkingName);
                    dynamicParameters.Add("@intShapeCodeId", this.shapecode.ShapeID);
                    dynamicParameters.Add("@numInvoiceMWLength", this.MWLength);
                    dynamicParameters.Add("@numInvoiceCWLength", this.CWLength);
                    dynamicParameters.Add("@intInvoiceMainQty", this.NoOfMainWire);
                    dynamicParameters.Add("@intInvoiceCrossQty", this.NoOfCrossWire);
                    dynamicParameters.Add("@numProductionMWLength", this.ProductionMWLength);
                    dynamicParameters.Add("@numProductionCWLength", this.ProductionCWLength);
                    dynamicParameters.Add("@intProductionMainQty", this.ProductionMWQty);
                    dynamicParameters.Add("@intProductionCrossQty", this.ProductionCWQty);
                    dynamicParameters.Add("@INTINVOICEMO1", this.MO1);
                    dynamicParameters.Add("@INTINVOICEMO2", this.MO2);
                    dynamicParameters.Add("@INTINVOICECO1", this.CO1);
                    dynamicParameters.Add("@INTINVOICECO2", this.CO2);
                    dynamicParameters.Add("@INTPRODUCTIONMO1", this.ProductionMO1);
                    dynamicParameters.Add("@INTPRODUCTIONMO2", this.ProductionMO2);
                    dynamicParameters.Add("@INTPRODUCTIONCO1", this.ProductionCO1);
                    dynamicParameters.Add("@INTPRODUCTIONCO2", this.ProductionCO2);
                    dynamicParameters.Add("@INTMWSPACING", this.MWSpacing);
                    dynamicParameters.Add("@INTCWSPACING", this.CWSpacing);
                    dynamicParameters.Add("@SITPINSIZE", this.PinSize);
                    dynamicParameters.Add("@XMLRESULT",null);
                    dynamicParameters.Add("@NUMINVOICEMWWEIGHT", InvoiceMWWeight);
                    dynamicParameters.Add("@NUMINVOICECWWEIGHT", InvoiceCWWeight);
                    dynamicParameters.Add("@NUMINVOICEAREA", this.InvoiceArea);
                    dynamicParameters.Add("@NUMTHEORATICALWEIGHT", this.TheoraticalWeight);
                    dynamicParameters.Add("@NUMNETWEIGHT", this.NetWeight);
                    dynamicParameters.Add("@INTMEMBERQTY", this.MemberQty);
                    dynamicParameters.Add("@NUMPRODUCTIONMWWEIGHT", ProductionMWWeight);
                    dynamicParameters.Add("@NUMPRODUCTIONCWWEIGHT", ProductionCWWeight);
                    dynamicParameters.Add("@NUMPRODUCTIONWEIGHT", this.ProductionWeight);
                    dynamicParameters.Add("@PARAMVALUES", this.ParamValues);
                    dynamicParameters.Add("@INTENVELOPLENGTH", this.EnvelopeLength);
                    dynamicParameters.Add("@INTENVELOPWIDTH", this.EnvelopeWidth);
                    dynamicParameters.Add("@INTENVELOPHEIGHT", this.EnvelopeHeight);
                    dynamicParameters.Add("@INTSTRUCTUREELEMENTID", StructureElementId);
                    dynamicParameters.Add("@INTPRODUCTMARKID", this.ProductMarkId);
                    dynamicParameters.Add("@INTUSERID", UserId);
                    if (ProduceIndicator == true)
                    {
                        dynamicParameters.Add("@CHPRODUCEINDICATOR", "Yes");
                    }
                    else
                    {
                        dynamicParameters.Add("@CHPRODUCEINDICATOR", "No");
                    }
                    dynamicParameters.Add("@BOMDrawingPath", BOMDrawingPath);
                    dynamicParameters.Add("@NVCHMWBVBSSTRING", MWBVBSString);
                    dynamicParameters.Add("@NVCHCWBVBSSTRING", CWBVBSString);

                    //CARPET C_BOM, D_BOM >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    dynamicParameters.Add("@OFFSET_A", this.mainlngtdecrsoffset_a);
                    dynamicParameters.Add("@OFFSET_B", this.mainlngtdecrsoffset_b);
                    dynamicParameters.Add("@OFFSET_C", this.mainlngtdecrsoffset_c);
                    //CARPET C_BOM, D_BOM <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


                    // START CARPET G BOM (Subhankar)>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    dynamicParameters.Add("@OFFSET_D", this.mainlngtdecrsoffset_d);
                    dynamicParameters.Add("@OFFSET_E", this.mainlngtdecrsoffset_e);
                    //END CARPET G BOM (Subhankar)<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<



                    // MW weight added by Subhankar 15th Jan 2015 (Start)

                    double CwMeterRunVal = GetCWMeterRun(ProductCodeId); //Subha 27052015

                    double InvoiceMWWeightVal;
                    double ProductionMWWeightVal;
                    double NetWeightVal;
                    double ProductionWeightVal;

                    //double CwMeterRunVal;


                    if (this.shapecode.ShapeCodeName == "T1" || this.shapecode.ShapeCodeName == "T2" || this.shapecode.ShapeCodeName == "T1K1" || this.shapecode.ShapeCodeName == "T1K2" || this.shapecode.ShapeCodeName == "T2K1" || this.shapecode.ShapeCodeName == "T2K2")
                    {
                        int NoOfCrossWireVal = ((MWLength - MO1 - MO2) / CWSpacing + 1);

                        double CWNetWtVal = (NoOfCrossWireVal * 2 * CWLength * CwMeterRunVal) / 2000;

                        dynamicParameters.Add("@NUMINVOICECWWEIGHT", CWNetWtVal);

                        if (this.NoOfMainWire % 2 == 0)
                        {

                            MwMeterRunVal = GetMWMeterRun(ProductCodeId);
                            InvoiceMWWeightVal = ((this.MWLength - this.mainlngtdecrsoffset_b) * MwMeterRunVal * this.NoOfMainWire / 2 + (this.MWLength - this.mainlngtdecrsoffset_c) * MwMeterRunVal * this.NoOfMainWire / 2) / 1000;
                            ProductionMWWeightVal = ((this.MWLength - this.mainlngtdecrsoffset_b) * MwMeterRunVal * this.NoOfMainWire / 2 + (this.MWLength - this.mainlngtdecrsoffset_c) * MwMeterRunVal * this.NoOfMainWire / 2) / 1000;
                            NetWeightVal = (InvoiceMWWeightVal + CWNetWtVal * 2) / 2;
                            ProductionWeightVal = (ProductionMWWeightVal + CWNetWtVal * 2) / 2;
                            MwMeterRunVal = GetMWMeterRun(ProductCodeId);
                        }
                        else
                        {
                            MwMeterRunVal = GetMWMeterRun(ProductCodeId);
                            InvoiceMWWeightVal = ((this.MWLength - this.mainlngtdecrsoffset_b) * MwMeterRunVal * (this.NoOfMainWire + 1) / 2 + (this.MWLength - this.mainlngtdecrsoffset_c) * MwMeterRunVal * (this.NoOfMainWire - 1) / 2) / 1000;
                            ProductionMWWeightVal = ((this.MWLength - this.mainlngtdecrsoffset_b) * MwMeterRunVal * (this.NoOfMainWire + 1) / 2 + (this.MWLength - this.mainlngtdecrsoffset_c) * MwMeterRunVal * (this.NoOfMainWire - 1) / 2) / 1000;
                            NetWeightVal = (InvoiceMWWeightVal + CWNetWtVal * 2) / 2;
                            ProductionWeightVal = (ProductionMWWeightVal + CWNetWtVal * 2) / 2;


                        }
                        dynamicParameters.Add("@NUMINVOICEMWWEIGHT", InvoiceMWWeightVal);//15th Jan
                        dynamicParameters.Add("@NUMNETWEIGHT", NetWeightVal);//15th Jan
                        dynamicParameters.Add("@NUMPRODUCTIONMWWEIGHT", ProductionMWWeight);//15th Jan
                        dynamicParameters.Add("@NUMPRODUCTIONWEIGHT", ProductionWeightVal);//15th Jan
                    }


                    if (this.shapecode.ShapeCodeName == "T7" || this.shapecode.ShapeCodeName == "T8")
                    {
                        if (this.NoOfMainWire % 2 == 0)
                        {

                            MwMeterRunVal = GetMWMeterRun(ProductCodeId);
                            InvoiceMWWeightVal = ((this.MWLength - this.mainlngtdecrsoffset_b - this.mainlngtdecrsoffset_c) * MwMeterRunVal * this.NoOfMainWire / 2 + this.MWLength * MwMeterRunVal * this.NoOfMainWire / 2) / 1000;
                            ProductionMWWeightVal = ((this.MWLength - this.mainlngtdecrsoffset_b) * MwMeterRunVal * this.NoOfMainWire / 2 + (this.MWLength - this.mainlngtdecrsoffset_c) * MwMeterRunVal * this.NoOfMainWire / 2) / 1000;
                            NetWeightVal = (InvoiceMWWeightVal + this.InvoiceCWWeight * 2) / 2;
                            ProductionWeightVal = (ProductionMWWeightVal + this.InvoiceCWWeight * 2) / 2;
                            MwMeterRunVal = GetMWMeterRun(ProductCodeId);
                            dynamicParameters.Add("@NUMINVOICEMWWEIGHT", InvoiceMWWeightVal);//15th Jan
                            dynamicParameters.Add("@NUMNETWEIGHT", NetWeightVal);//15th Jan
                            dynamicParameters.Add("@NUMPRODUCTIONMWWEIGHT", ProductionMWWeight);//15th Jan
                            dynamicParameters.Add("@NUMPRODUCTIONWEIGHT", ProductionWeightVal);//15th Jan
                        }
                        else
                        {
                            if (this.shapecode.ShapeCodeName == "T7")
                            {
                                MwMeterRunVal = GetMWMeterRun(ProductCodeId);
                                InvoiceMWWeightVal = (this.MWLength * MwMeterRunVal * (this.NoOfMainWire + 1) / 2 + (this.MWLength - this.mainlngtdecrsoffset_b - this.mainlngtdecrsoffset_c) * MwMeterRunVal * (this.NoOfMainWire - 1) / 2) / 1000;
                                ProductionMWWeightVal = (this.MWLength * MwMeterRunVal * (this.NoOfMainWire + 1) / 2 + (this.MWLength - this.mainlngtdecrsoffset_b - this.mainlngtdecrsoffset_c) * MwMeterRunVal * (this.NoOfMainWire - 1) / 2) / 1000;
                                NetWeightVal = (InvoiceMWWeightVal + this.InvoiceCWWeight * 2) / 2;
                                ProductionWeightVal = (ProductionMWWeightVal + this.InvoiceCWWeight * 2) / 2;
                                dynamicParameters.Add("@NUMINVOICEMWWEIGHT", InvoiceMWWeightVal);//15th Jan
                                dynamicParameters.Add("@NUMNETWEIGHT", NetWeightVal);//15th Jan
                                dynamicParameters.Add("@NUMPRODUCTIONMWWEIGHT", ProductionMWWeight);//15th Jan
                                dynamicParameters.Add("@NUMPRODUCTIONWEIGHT", ProductionWeightVal);//15th Jan
                            }
                            else if (this.shapecode.ShapeCodeName == "T8")
                            {
                                MwMeterRunVal = GetMWMeterRun(ProductCodeId);
                                InvoiceMWWeightVal = ((this.MWLength - this.mainlngtdecrsoffset_b - this.mainlngtdecrsoffset_c) * MwMeterRunVal * (this.NoOfMainWire + 1) / 2 + this.MWLength * MwMeterRunVal * (this.NoOfMainWire - 1) / 2) / 1000;
                                ProductionMWWeightVal = ((this.MWLength - this.mainlngtdecrsoffset_b - this.mainlngtdecrsoffset_c) * MwMeterRunVal * (this.NoOfMainWire + 1) / 2 + this.MWLength * MwMeterRunVal * (this.NoOfMainWire - 1) / 2) / 1000;
                                NetWeightVal = (InvoiceMWWeightVal + this.InvoiceCWWeight * 2) / 2;
                                ProductionWeightVal = (ProductionMWWeightVal + this.InvoiceCWWeight * 2) / 2;
                                dynamicParameters.Add("@NUMINVOICEMWWEIGHT", InvoiceMWWeightVal);//15th Jan
                                dynamicParameters.Add("@NUMNETWEIGHT", NetWeightVal);//15th Jan
                                dynamicParameters.Add("@NUMPRODUCTIONMWWEIGHT", ProductionMWWeight);//15th Jan
                                dynamicParameters.Add("@NUMPRODUCTIONWEIGHT", ProductionWeightVal);//15th Jan
                            }

                        }

                    }

                    if (this.shapecode.ShapeCodeName == "T3" || this.shapecode.ShapeCodeName == "T4" || this.shapecode.ShapeCodeName == "T5" || this.shapecode.ShapeCodeName == "T6" || this.shapecode.ShapeCodeName == "T3K1" || this.shapecode.ShapeCodeName == "T3K2")
                    {
                        if (this.NoOfMainWire % 2 == 0)
                        {
                            MwMeterRunVal = GetMWMeterRun(ProductCodeId);
                            InvoiceMWWeightVal = ((this.MWLength - this.mainlngtdecrsoffset_b) * MwMeterRunVal * this.NoOfMainWire / 2 + this.MWLength * MwMeterRunVal * this.NoOfMainWire / 2) / 1000;
                            ProductionMWWeightVal = ((this.MWLength - this.mainlngtdecrsoffset_b) * MwMeterRunVal * this.NoOfMainWire / 2 + this.MWLength * MwMeterRunVal * this.NoOfMainWire / 2) / 1000;

                            NetWeightVal = (InvoiceMWWeightVal + this.InvoiceCWWeight * 2) / 2;
                            ProductionWeightVal = (ProductionMWWeightVal + this.InvoiceCWWeight * 2) / 2;
                            MwMeterRunVal = GetMWMeterRun(ProductCodeId);


                            dynamicParameters.Add("@NUMINVOICEMWWEIGHT", InvoiceMWWeightVal);//15th Jan
                            dynamicParameters.Add("@NUMNETWEIGHT", NetWeightVal);//15th Jan
                            dynamicParameters.Add("@NUMPRODUCTIONMWWEIGHT", ProductionMWWeight);//15th Jan
                            dynamicParameters.Add("@NUMPRODUCTIONWEIGHT", ProductionWeightVal);//15th Jan
                        }
                        else
                        {

                            MwMeterRunVal = GetMWMeterRun(ProductCodeId);
                            CwMeterRunVal = GetCWMeterRun(ProductCodeId);


                            if (this.shapecode.ShapeCodeName == "T3" || this.shapecode.ShapeCodeName == "T5" || this.shapecode.ShapeCodeName == "T3K1" || this.shapecode.ShapeCodeName == "T3K2")
                            {
                                InvoiceMWWeightVal = (this.MWLength * MwMeterRunVal * (this.NoOfMainWire + 1) / 2 + (this.MWLength - this.mainlngtdecrsoffset_b) * MwMeterRunVal * (this.NoOfMainWire - 1) / 2) / 1000;
                                ProductionMWWeightVal = (this.MWLength * MwMeterRunVal * (this.NoOfMainWire + 1) / 2 + (this.MWLength - this.mainlngtdecrsoffset_b) * MwMeterRunVal * (this.NoOfMainWire - 1) / 2) / 1000;
                                NetWeightVal = (InvoiceMWWeightVal + this.InvoiceCWWeight * 2) / 2;
                                ProductionWeightVal = (ProductionMWWeightVal + this.InvoiceCWWeight * 2) / 2;
                                dynamicParameters.Add("@NUMINVOICEMWWEIGHT", InvoiceMWWeightVal);//15th Jan
                                dynamicParameters.Add("@NUMNETWEIGHT", NetWeightVal);//15th Jan
                                dynamicParameters.Add("@NUMPRODUCTIONMWWEIGHT", ProductionMWWeight);//15th Jan
                                dynamicParameters.Add("@NUMPRODUCTIONWEIGHT", ProductionWeightVal);//15th Jan
                            }
                            else if (this.shapecode.ShapeCodeName == "T4" || this.shapecode.ShapeCodeName == "T6")
                            {
                                InvoiceMWWeightVal = ((this.MWLength - this.mainlngtdecrsoffset_b) * MwMeterRunVal * (this.NoOfMainWire + 1) / 2 + this.MWLength * MwMeterRunVal * (this.NoOfMainWire - 1) / 2) / 1000;
                                ProductionMWWeightVal = ((this.MWLength - this.mainlngtdecrsoffset_b) * MwMeterRunVal * (this.NoOfMainWire + 1) / 2 + this.MWLength * MwMeterRunVal * (this.NoOfMainWire - 1) / 2) / 1000;
                                NetWeightVal = (InvoiceMWWeightVal + this.InvoiceCWWeight * 2) / 2;
                                ProductionWeightVal = (ProductionMWWeightVal + this.InvoiceCWWeight * 2) / 2;
                                dynamicParameters.Add("@NUMINVOICEMWWEIGHT", InvoiceMWWeightVal);//15th Jan
                                dynamicParameters.Add("@NUMNETWEIGHT", NetWeightVal);//15th Jan
                                dynamicParameters.Add("@NUMPRODUCTIONMWWEIGHT", ProductionMWWeight);//15th Jan
                                dynamicParameters.Add("@NUMPRODUCTIONWEIGHT", ProductionWeightVal);//15th Jan
                            }




                        }




                    }


                    #region "A1,A2 net wt..."
                    // To assign net wts of A1 and A2 
                    if (this.shapecode.ShapeCodeName == "A1" || this.shapecode.ShapeCodeName == "A2")
                    {

                        MwMeterRunVal = GetMWMeterRun(ProductCodeId);
                        InvoiceMWWeightVal = 0;
                        ProductionMWWeightVal = 0;
                        Double OffSet = 0;
                        OffSet = (double)(this.mainlngtdecrsoffset_b) / (NoOfMainWire - 1);
                        int WireLen = 0;
                        int FactorVal = 0;
                        int TotalMWLen = 0;
                        // WireLen = Convert.ToInt32(Math.Round((Convert.ToDouble(this.mainlngtdecrsoffset_a) + OffSet), 0));


                        for (int IntNoOfMWWire = 0; IntNoOfMWWire < NoOfMainWire; IntNoOfMWWire++)
                        {
                            WireLen = Convert.ToInt32(Math.Round((Convert.ToDouble(this.mainlngtdecrsoffset_a) + OffSet * FactorVal), 0));
                            TotalMWLen = TotalMWLen + WireLen;
                            FactorVal = FactorVal + 1;
                        }

                        InvoiceMWWeightVal = (TotalMWLen * MwMeterRunVal) / 1000;
                        ProductionMWWeightVal = (TotalMWLen * MwMeterRunVal) / 1000;

                        //InvoiceMWWeightVal = ((this.MWLength - this.mainlngtdecrsoffset_b) * MwMeterRunVal * this.NoOfMainWire / 2 + (this.MWLength - this.mainlngtdecrsoffset_c) * MwMeterRunVal * this.NoOfMainWire / 2) / 1000;
                        // ProductionMWWeightVal = ((this.MWLength - this.mainlngtdecrsoffset_b) * MwMeterRunVal * this.NoOfMainWire / 2 + (this.MWLength - this.mainlngtdecrsoffset_c) * MwMeterRunVal * this.NoOfMainWire / 2) / 1000;
                        NetWeightVal = (InvoiceMWWeightVal + this.InvoiceCWWeight * 2) / 2;
                        ProductionWeightVal = (ProductionMWWeightVal + this.InvoiceCWWeight * 2) / 2;

                        dynamicParameters.Add("@NUMINVOICEMWWEIGHT", InvoiceMWWeightVal);//15th Jan
                        dynamicParameters.Add("@NUMNETWEIGHT", NetWeightVal);//15th Jan
                        dynamicParameters.Add("@NUMPRODUCTIONMWWEIGHT", ProductionMWWeight);//15th Jan
                        dynamicParameters.Add("@NUMPRODUCTIONWEIGHT", ProductionWeightVal);//15th Jan
                    }
                    #endregion

                    #region "A3 net wt..."

                    if (this.shapecode.ShapeCodeName == "A3")
                    {
                        int WireLen = 0;
                        int TotalWireLen = 0;
                        int NoOfLoop = 0;
                        int repVal = 0;
                        MwMeterRunVal = GetMWMeterRun(ProductCodeId);
                        WireLen = this.MWLength;
                        NoOfLoop = (this.mainlngtdecrsoffset_b) / (this.mainlngtdecrsoffset_c) + 1;
                        repVal = ((this.CWLength - this.CO1 - this.CO2 + this.MWSpacing) / (this.MWSpacing + this.mainlngtdecrsoffset_d) - 3);


                        for (int cntval = 0; cntval < NoOfLoop; cntval++)
                        {
                            TotalWireLen = TotalWireLen + WireLen;
                            WireLen = WireLen - this.mainlngtdecrsoffset_c;
                        }
                        InvoiceMWWeightVal = (TotalWireLen * MwMeterRunVal * (repVal + 3)) / 1000;
                        ProductionMWWeightVal = (TotalWireLen * MwMeterRunVal * (repVal + 3)) / 1000;

                        NetWeightVal = (InvoiceMWWeightVal + this.InvoiceCWWeight * 2) / 2;
                        ProductionWeightVal = (ProductionMWWeightVal + this.InvoiceCWWeight * 2) / 2;

                        dynamicParameters.Add("@NUMINVOICEMWWEIGHT", InvoiceMWWeightVal);//15th Jan
                        dynamicParameters.Add("@NUMNETWEIGHT", NetWeightVal);//15th Jan
                        dynamicParameters.Add("@NUMPRODUCTIONMWWEIGHT", ProductionMWWeight);//15th Jan
                        dynamicParameters.Add("@NUMPRODUCTIONWEIGHT", ProductionWeightVal);//15th Jan
                    }




                    #endregion

                    #region "P1 net wt..."
                    // To assign net wts of A1 and A2 
                    if (this.shapecode.ShapeCodeName == "P1")
                    {
                        MwMeterRunVal = GetMWMeterRun(ProductCodeId);
                        InvoiceMWWeightVal = 0;
                        ProductionMWWeightVal = 0;
                        Double OffSet = 0;
                        OffSet = (double)(this.mainlngtdecrsoffset_b) / (NoOfMainWire);
                        int TotalMWLen = 0;
                        int PitchTopLine = 0;
                        int PitchLastLine = 0;
                        int RepVal = 0;
                        // WireLen = Convert.ToInt32(Math.Round((Convert.ToDouble(this.mainlngtdecrsoffset_a) + OffSet), 0));
                        PitchTopLine = (this.mainlngtdecrsoffset_d) / (MWSpacing);
                        PitchLastLine = (CWLength - CO1 - CO2 - this.mainlngtdecrsoffset_d - this.mainlngtdecrsoffset_e) / (MWSpacing);
                        RepVal = (this.mainlngtdecrsoffset_e) / (MWSpacing) - 1;

                        TotalMWLen = PitchTopLine * MWLength + PitchLastLine * MWLength + (RepVal + 2) * this.mainlngtdecrsoffset_a + (RepVal + 2) * this.mainlngtdecrsoffset_c;

                        InvoiceMWWeightVal = (TotalMWLen * MwMeterRunVal) / 1000;
                        ProductionMWWeightVal = (TotalMWLen * MwMeterRunVal) / 1000;

                        //InvoiceMWWeightVal = ((this.MWLength - this.mainlngtdecrsoffset_b) * MwMeterRunVal * this.NoOfMainWire / 2 + (this.MWLength - this.mainlngtdecrsoffset_c) * MwMeterRunVal * this.NoOfMainWire / 2) / 1000;
                        // ProductionMWWeightVal = ((this.MWLength - this.mainlngtdecrsoffset_b) * MwMeterRunVal * this.NoOfMainWire / 2 + (this.MWLength - this.mainlngtdecrsoffset_c) * MwMeterRunVal * this.NoOfMainWire / 2) / 1000;
                        NetWeightVal = (InvoiceMWWeightVal + this.InvoiceCWWeight * 2) / 2;
                        ProductionWeightVal = (ProductionMWWeightVal + this.InvoiceCWWeight * 2) / 2;

                        dynamicParameters.Add("@NUMINVOICEMWWEIGHT", InvoiceMWWeightVal);//15th Jan
                        dynamicParameters.Add("@NUMNETWEIGHT", NetWeightVal);//15th Jan
                        dynamicParameters.Add("@NUMPRODUCTIONMWWEIGHT", ProductionMWWeight);//15th Jan
                        dynamicParameters.Add("@NUMPRODUCTIONWEIGHT", ProductionWeightVal);//15th Jan
                    }
                    #endregion






                    // MW weight added by Subhankar 15th Jan 2015 (End)



                    //if (MWPitch == string.Empty || MWPitch == "")
                    //{
                    //    dynamicParameters.Add(44, "@MWPITCH", DBNull.Value);
                    //}
                    //else
                    //{
                    //    dynamicParameters.Add(44, "@MWPITCH", MWPitch);
                    //}
                    //dynamicParameters.Add(45, "@MWFLAG", MWFlag);
                    //if (CWPitch == string.Empty || CWPitch == "")
                    //{
                    //    dynamicParameters.Add(46, "@CWPITCH", DBNull.Value);
                    //}
                    //else
                    //{
                    //    dynamicParameters.Add(46, "@CWPITCH", CWPitch);
                    //}
                    //dynamicParameters.Add(47, "@CWFLAG", CWFlag);
                    //if (BendingCheckInd == true)
                    //{
                    //    dynamicParameters.Add(48, "@BENDINGCHECK", 1);
                    //}
                    //else
                    //{
                    //    dynamicParameters.Add(48, "@BENDINGCHECK", 0);
                    //}

                   // ProductGenerationOutput = Convert.ToString((dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_CarpetProductMarkingDetails_InsUpd")));
                    ProductGenerationOutput = sqlConnection.QueryFirstOrDefault<string>(SystemConstant.CarpetProductMarkingDetails_InsUpd, dynamicParameters, commandType: CommandType.StoredProcedure);


                    if (ProductGenerationOutput.Contains(","))
                    {
                        string[] Output = ProductGenerationOutput.Split(',');
                        intProductMarkId = Convert.ToInt32(Output[0]);
                        this.ProductValidator = Convert.ToInt16(Output[1]);
                    }
                    else if (ProductGenerationOutput.Contains("Please check max NET WEIGHT"))
                    {
                        throw new Exception("Please check max NET WEIGHT.");
                    }

                    this.ProductMarkId = Convert.ToInt32(intProductMarkId);
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
            return isSuccess;
        }

        public bool DeleteCarpetProductMark(out string errorMessage)
        {
            bool isSuccess = false;

            int Postedvalidate = 0;
            errorMessage = "";
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@PRODUCTMARKID", this.ProductMarkId);
                    dynamicParameters.Add("@DELETEFLAG", this.Deleteflag);
                    Postedvalidate = sqlConnection.QueryFirstOrDefault<int>(SystemConstant.CarpetProductMark_Delete, dynamicParameters, commandType: CommandType.StoredProcedure);

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
                        throw new Exception("Error in deleting Carpet Product");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
            return isSuccess;
        }

        public bool DeleteCarpetProductMarkByStructureMarkId(out string errorMessage)
        {
            bool isSuccess = false;

            int Postedvalidate=0;
            errorMessage = "";
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTSTRUCTUREMARKID", this.StructureMarkId);
                    //Postedvalidate = dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_CarpetProductMarkByStructureMarkId_Delete");
                    Postedvalidate =sqlConnection.QueryFirstOrDefault<int>(SystemConstant.CarpetProductMarkByStructureMarkId_Delete, dynamicParameters, commandType: CommandType.StoredProcedure);

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
                        throw new Exception("Error in deleting Carpet Product");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
            return isSuccess;
        }

        public List<CarpetProduct> GetOverHang(int ParamSetNumber, int ProjectId, int StructureElementId, int ProductTypeId)
        {
          //  DBManager dbManager = new DBManager();
            List<CarpetProduct> OverHangList = new List<CarpetProduct>();
            DataSet dsOverHang = new DataSet();
            IEnumerable<listGetOverHangDto> listGetOverHangDto;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@decMWLength", this.ProductionMWLength);
                    dynamicParameters.Add("@decCWLength", this.ProductionCWLength);
                    dynamicParameters.Add("@intMWSpace", this.MWSpacing);
                    dynamicParameters.Add("@intCWSpace", this.CWSpacing);
                    dynamicParameters.Add("@tntParamSetNumber", ParamSetNumber);
                    dynamicParameters.Add("@intProjectId", ProjectId);
                    dynamicParameters.Add("@intStructureElementId", StructureElementId);
                    dynamicParameters.Add("@sitProductTypeId", ProductTypeId);

                    listGetOverHangDto = sqlConnection.Query<listGetOverHangDto>(SystemConstant.OverHang_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (listGetOverHangDto.Count() > 0)
                    {
                        DataTable dt = ConvertToDataTable.ToDataTable(listGetOverHangDto);
                        dsOverHang.Tables.Add(dt);

                        if (dsOverHang != null && dsOverHang.Tables.Count != 0)
                        {
                            foreach (DataRowView drvOverHang in dsOverHang.Tables[0].DefaultView)
                            {
                                CarpetProduct CarpetProduct = new CarpetProduct
                                {
                                    MO1 = Convert.ToInt32(drvOverHang["MO1"]),
                                    MO2 = Convert.ToInt32(drvOverHang["MO2"]),
                                    CO1 = Convert.ToInt32(drvOverHang["CO1"]),
                                    CO2 = Convert.ToInt32(drvOverHang["CO2"])
                                };
                                OverHangList.Add(CarpetProduct);
                            }
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



        //Subhankar for carpet
        public string GetCarpetBOMType(int ProductCodeId, string ShapeCode)
        {
            string BOMTYPE = string.Empty;

            DataSet BOMTypeVal = null;

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@PRODUCTCODEID", ProductCodeId);
                    dynamicParameters.Add("@SHAPECODE", ShapeCode);
                    //BOMTypeVal = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.Carpet_GetBOMType, dynamicParameters, commandType: CommandType.StoredProcedure);
                    // BOMTYPE = BOMTypeVal.Tables[0].Rows[0][0].ToString();

                    BOMTYPE = sqlConnection.QueryFirstOrDefault<string>(SystemConstant.Carpet_GetBOMType, dynamicParameters, commandType: CommandType.StoredProcedure);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           

            return BOMTYPE;
        }

        // Inserting inner spool data for CAB

        //public int InsertUpdCABAccessories(int intSEDetailingId)
        //{
        //    DBManager dbManager = new DBManager();
        //    string strCABProductMarkID = string.Empty;
        //    DataSet dsCABProductMarkID = new DataSet();
        //    int intCABProductMarkID = 0;
        //    int ShapeTransHeaderId = 0;
        //    try
        //    {
        //        dbManager.Open();
        //        dbManager.CreateParameters(30);
        //        dynamicParameters.Add(0, "@VCHCABPRODUCTMARKNAME", this.CABProductMarkName);
        //        dynamicParameters.Add(1, "@INTSEDETAILINGID", intSEDetailingId);
        //        dynamicParameters.Add(2, "@INTMEMBERQTY", this.Quantity);
        //        dynamicParameters.Add(3, "@INTSHAPECODE", this.ShapeCode);
        //        dynamicParameters.Add(4, "@INTPINSIZEID", this.PinSizeID);
        //        dynamicParameters.Add(5, "@NUMINVOICELENGTH", this.InvoiceLength);
        //        dynamicParameters.Add(6, "@NUMPRODUCTIONLENGTH", this.ProductionLength);
        //        dynamicParameters.Add(7, "@NUMINVOICEWEIGHT", this.InvoiceWeight);
        //        dynamicParameters.Add(8, "@NUMPRODUCTIONWEIGHT", this.ProductionWeight);
        //        dynamicParameters.Add(9, "@GRADE", this.Grade);
        //        dynamicParameters.Add(10, "@INTDIAMETER", this.Diameter);
        //        dynamicParameters.Add(11, "@VCHSHAPETYPE", this.ShapeType);
        //        dynamicParameters.Add(12, "@VCHSHAPEGROUP", this.ShapeGroup);
        //        dynamicParameters.Add(13, "@INTSTATUS", this.Status);
        //        dynamicParameters.Add(14, "@VCHCUSTREMARKS", this.CustomerRemarks);
        //        dynamicParameters.Add(15, "@VCHSHAPEIMAGE", this.ShapeImage);
        //        dynamicParameters.Add(16, "@VCHCAB_BVBS", this.BVBS);
        //        dynamicParameters.Add(17, "@VCHPAGENUMBER", this.PageNumber);
        //        dynamicParameters.Add(18, "@VCHCOMMDESCRIPT", this.CommercialDesc);

        //        dynamicParameters.Add(19, "@NUMENVLENGTH", this.EnvLength);
        //        dynamicParameters.Add(20, "@NUMENVWIDTH", this.EnvWidth);
        //        dynamicParameters.Add(21, "@NUMENVHEIGHT", this.EnvHeight);
        //        dynamicParameters.Add(22, "@INTNOOFBENDS", this.NoOfBends);


        //           dynamicParameters.Add(23, "@BARSTANDARD", "");
        //           dynamicParameters.Add(24, "@COUPLERTYPE1", "");
        //           dynamicParameters.Add(25, "@COUPLERMATERIAL1", "");
        //           dynamicParameters.Add(26, "@COUPLERSTANDARD1", "");
        //           dynamicParameters.Add(27, "@COUPLERTYPE2", "");
        //           dynamicParameters.Add(28, "@COUPLERMATERIAL2", "");
        //           dynamicParameters.Add(29, "@COUPLERSTANDARD2", "");

        //        dsCABProductMarkID = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_CAB_ProductMarkingDetails_InsUpd");


        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //    return intCABProductMarkID;
        //}


        /// <summary>
        /// Get Meter Per Run Value by Product Code Id provided
        /// </summary>
        /// <param name="ProductCodeVal"> Product Code Id</param>
        /// <returns> Meter Per Run</returns>

        public double GetMWMeterRun(int ProductCodeVal)
        {
            double MWRunVal = 0.0;


            DataSet MWRunValDS = null;

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@IntProductCodeID", ProductCodeVal);
                    //MWRunValDS = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.ShapeCode_Get_MWRunPerMetValue_ByProdCode_Carpet, dynamicParameters, commandType: CommandType.StoredProcedure);
                    //MWRunVal = Convert.ToDouble(MWRunValDS.Tables[0].Rows[0][0]);

                    MWRunVal = sqlConnection.QueryFirstOrDefault<double>(SystemConstant.ShapeCode_Get_MWRunPerMetValue_ByProdCode_Carpet, dynamicParameters, commandType: CommandType.StoredProcedure);

                }


            }
            catch (Exception ex)
            {
                throw ex;
            }           
            return MWRunVal;
        }



        /// <summary>
        /// Get CW Meter Per Run Value by Product Code Id provided
        /// </summary>
        /// <param name="ProductCodeVal"> Product Code Id</param>
        /// <returns> CW Meter Per Run</returns>

        public double GetCWMeterRun(int ProductCodeVal)
        {
            double CWRunVal = 0.0;
            DataSet CWRunValDS = null;

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@IntProductCodeID", ProductCodeVal);
                    //CWRunValDS = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.ShapeCode_Get_CWRunPerMetValue_ByProdCode_Carpet, dynamicParameters, commandType: CommandType.StoredProcedure);
                    //CWRunVal = Convert.ToDouble(CWRunValDS.Tables[0].Rows[0][0]);

                    CWRunVal =sqlConnection.QueryFirstOrDefault<double>(SystemConstant.ShapeCode_Get_CWRunPerMetValue_ByProdCode_Carpet, dynamicParameters, commandType: CommandType.StoredProcedure);



                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return CWRunVal;
        }

    }
}
