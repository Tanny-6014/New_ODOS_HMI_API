using Dapper;
using DetailingService.Constants;
using DetailingService.Context;
using DetailingService.Dtos;
using Microsoft.Data.SqlClient;

using System.Data;

namespace DetailingService.Repositories
{
    public class ColumnProduct
    {

        //  private string connectionString;

        //private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";
        private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";

        public int ProductMarkId { get; set; }
        public int StructureMarkId { get; set; }
        public string ProductMarkingName { get; set; }
        public int ProductCodeId { get; set; }
        public int LinkWidth { get; set; }
        public int LinkLength { get; set; }
        public int LinkQty { get; set; }
        public int InvoiceMWQty { get; set; }
        public int InvoiceCWQty { get; set; }
        public double InvoiceMWWeight { get; set; }
        public double InvoiceCWWeight { get; set; }
        public double InvoiceWeight { get; set; }
        public double InvoiceMWLength { get; set; }
        public double InvoiceCWLength { get; set; }
        public int ShapeCodeId { get; set; }
        public int NoofLinks { get; set; }
        public int TotalLinks { get; set; }
        public int MO1 { get; set; }
        public int MO2 { get; set; }
        public int CO1 { get; set; }
        public int CO2 { get; set; }
        public int ProductionMO1 { get; set; }
        public int ProductionMO2 { get; set; }
        public int ProductionCO1 { get; set; }
        public int ProductionCO2 { get; set; }
        public double InvoiceArea { get; set; }
        public double ProductionArea { get; set; }
        public double TheoraticalWeight { get; set; }
        public int ProductionMWLength { get; set; }
        public int ProductionCWLength { get; set; }
        public int ProductionMWQty { get; set; }
        public int ProductionCWQty { get; set; }
        public double ProductionMWWeight { get; set; }
        public double ProductionCWWeight { get; set; }
        public double ProductionWeight { get; set; }
        public int PinSize { get; set; }
        public int MWSpacing { get; set; }
        public int CWSpacing { get; set; }
        public string ParamValues { get; set; }
        public string BOMDrawingPath { get; set; }
        public string MWBVBSString { get; set; }
        public string CWBVBSString { get; set; }
        public ShapeParameterCollection ShapeParam { get; set; }
        public string ShapeCode { get; set; }
        public Int32 Quantity { get; set; }
        public string BOMIndicator { get; set; }
        public string CWSpacingString { get; set; }
        public bool ProduceIndicator { get; set; }
        public bool BendCheck { get; set; }
        public Int32 GenerationStatus { get; set; }

        public int ProductValidator { get; set; }
        public string MWPitch { get; set; }
        public string CWPitch { get; set; }
        public int MWFlag { get; set; }
        public int CWFlag { get; set; }


        
        public ColumnProduct()
        {

        }
       

        //public string Product
        public List<ColumnProduct> ColumnProductByStructureMarkId_Get(int StructureMarkId)
        {
          //  DBManager dbManager = new DBManager();
            List<ColumnProduct> columnProductList = new List<ColumnProduct>();
            DataSet dsProductMarkForColumn = new DataSet();
            IEnumerable<ColumnProductByStructureMarkIdDto> columnProductByStructureMarkIdDto;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTSTRUCTUREMARKID", StructureMarkId);
                    // dsProductMarkForColumn = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_ColumnProductByStructureMarkId_Get");
                    columnProductByStructureMarkIdDto = sqlConnection.Query<ColumnProductByStructureMarkIdDto>(SystemConstant.ColumnProductByStructureMarkId_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    ShapeParameterCollection shapeParameterCollection = new ShapeParameterCollection();
                    //  if (IndexusDistributionCache.SharedCache.Get("ColumnShapeparamCache") == null)
                    // {
                    shapeParameterCollection = shapeParameterCollection.ColumnShapeParameter_Get();

                    //}
                    //else
                    //{
                    //    shapeParameterCollection = (ShapeParameterCollection)IndexusDistributionCache.SharedCache.Get("ColumnShapeparamCache");
                    //}
                    ShapeParameterCollection strucParamCollFilter = new ShapeParameterCollection();

                    if (columnProductByStructureMarkIdDto.Count() > 0)
                    {
                        DataTable dt = ConvertToDataTable.ToDataTable(columnProductByStructureMarkIdDto);
                        dsProductMarkForColumn.Tables.Add(dt);

                        if (dsProductMarkForColumn != null && dsProductMarkForColumn.Tables.Count != 0)
                        {
                            foreach (DataRowView drvBeam in dsProductMarkForColumn.Tables[0].DefaultView)
                            {
                                foreach (ShapeParameter shapeParamCollection in shapeParameterCollection)
                                {
                                    if (shapeParamCollection.ShapeId == Convert.ToInt32(drvBeam["SHAPE"]))
                                    {

                                        strucParamCollFilter.Add(shapeParamCollection);
                                    }
                                }
                                bool boolProduceIndicator = false;
                                if (drvBeam["PRODUCEINDICATOR"].ToString().Trim().ToUpper() == "YES")
                                {
                                    boolProduceIndicator = true;
                                }
                                else if (drvBeam["PRODUCEINDICATOR"].ToString().Trim().ToUpper() == "NO")
                                {
                                    boolProduceIndicator = false;
                                }

                                ColumnProduct columnProduct = new ColumnProduct
                                {
                                    ProductMarkId = Convert.ToInt32(drvBeam["INTPRODUCTMARKID"]),
                                    StructureMarkId = Convert.ToInt32(drvBeam["INTSTRUCTUREMARKID"]),
                                    ProductCodeId = Convert.ToInt32(drvBeam["PRODUCTCODE"]),
                                    Quantity = Convert.ToInt32(drvBeam["LINKQTY"]),
                                    LinkLength = Convert.ToInt32(drvBeam["LINKLENGTH"]),
                                    LinkWidth = Convert.ToInt32(drvBeam["LINKWIDTH"]),
                                    InvoiceMWLength = Convert.ToInt32(drvBeam["INVOICEMWLENGTH"]),
                                    InvoiceCWLength = Convert.ToInt32(drvBeam["INVOICECWLENGTH"]),
                                    ShapeCodeId = Convert.ToInt32(drvBeam["SHAPE"]),
                                    NoofLinks = Convert.ToInt32(drvBeam["NOOFLINKS"]),
                                    BOMIndicator = drvBeam["BOMIND"].ToString(),
                                    ShapeParam = strucParamCollFilter,
                                    ProduceIndicator = boolProduceIndicator,
                                    BendCheck = Convert.ToBoolean(drvBeam["BENDINDICATOR"]),
                                    ParamValues = Convert.ToString(drvBeam["PARAMVALUES"]),
                                    ShapeCode = drvBeam["SHAPECODE"].ToString(),
                                    MO1 = Convert.ToInt32(drvBeam["MO1"]),
                                    MO2 = Convert.ToInt32(drvBeam["MO2"]),
                                    CO1 = Convert.ToInt32(drvBeam["CO1"]),
                                    CO2 = Convert.ToInt32(drvBeam["CO2"]),
                                    ProductionMO1 = Convert.ToInt32(drvBeam["INTPRODUCTIONMO1"]),
                                    ProductionMO2 = Convert.ToInt32(drvBeam["INTPRODUCTIONMO2"]),
                                    ProductionCO1 = Convert.ToInt32(drvBeam["INTPRODUCTIONCO1"]),
                                    ProductionCO2 = Convert.ToInt32(drvBeam["INTPRODUCTIONCO2"]),
                                    ProductValidator = Convert.ToInt32(drvBeam["INTPRODUCTVALIDATOR"])
                                };
                                columnProductList.Add(columnProduct);
                            }
                        }
                    }
                
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return columnProductList; ///---Vanita
        }

        public bool DeleteColumnProductmark(int StructureMarkId)
        {
            int output = 0;
            bool isSuccess = false;
            //DBManager dbManager = new DBManager();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTSTRUCTUREMARKID", StructureMarkId);
                    dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);

                    // dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_ColumnProductmarkByStructId_Delete");
                     sqlConnection.Query<int>(SystemConstant.ColumnProductmarkByStructId_Delete, dynamicParameters, commandType: CommandType.StoredProcedure);
                    output = dynamicParameters.Get<int>("@Output");
                    if (output > 0)
                    {
                        isSuccess = true;

                    }

                    //
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
            return isSuccess;
        }

        public bool Save(int UserId)
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
                    dynamicParameters.Add("@INTSTRUCTUREMARKID", StructureMarkId);
                    dynamicParameters.Add("@VCHPRODUCTMARKINGNAME", ProductMarkingName);
                    dynamicParameters.Add("@NUMLINKWIDTH", this.LinkWidth);
                    dynamicParameters.Add("@NUMLINKLENGTH", this.LinkLength);
                    dynamicParameters.Add("@INTLINKQTY", this.Quantity);
                    dynamicParameters.Add("@INTNOOFLINKS", this.NoofLinks);
                    dynamicParameters.Add("@INTTOTALLINKS", this.TotalLinks);
                    dynamicParameters.Add("@NUMINVOICEMWLENGTH", this.InvoiceMWLength);
                    dynamicParameters.Add("@NUMINVOICECWLENGTH", this.InvoiceCWLength);
                    dynamicParameters.Add("@INTSHAPECODEID", this.ShapeCodeId);
                    dynamicParameters.Add("@INTPRODUCTCODEID", this.ProductCodeId);
                    dynamicParameters.Add("@NUMPRODUCTIONCWQTY", this.ProductionCWQty);
                    dynamicParameters.Add("@NUMPRODUCTIONMWQTY", this.ProductionMWQty);
                    dynamicParameters.Add("@NUMINVOICEMWWEIGHT", this.InvoiceMWWeight);
                    dynamicParameters.Add("@NUMINVOICECWWEIGHT", this.InvoiceCWWeight);
                    dynamicParameters.Add("@NUMPRODUCTIONMWWEIGHT", this.ProductionMWWeight);
                    dynamicParameters.Add("@NUMPRODUCTIONCWWEIGHT", this.ProductionCWWeight);
                    dynamicParameters.Add("@NUMINVOICEWEIGHT", this.InvoiceWeight);
                    dynamicParameters.Add("@NUMPRODUCTIONWEIGHT", this.ProductionWeight);
                    dynamicParameters.Add("@NUMMWSPACING", this.MWSpacing);
                    dynamicParameters.Add("@NUMTHEOWEIGHT", this.TheoraticalWeight);
                    dynamicParameters.Add("@NUMINVOICEAREA", this.InvoiceArea);
                    dynamicParameters.Add("@INTMO1", this.MO1);
                    dynamicParameters.Add("@INTMO2", this.MO2);
                    dynamicParameters.Add("@INTCO1", this.CO1);
                    dynamicParameters.Add("@INTCO2", this.CO2);
                    dynamicParameters.Add("@INTPRODUCTIONMO1", this.ProductionMO1);
                    dynamicParameters.Add("@INTPRODUCTIONMO2", this.ProductionMO2);
                    dynamicParameters.Add("@INTPRODUCTIONCO1", this.ProductionCO1);
                    dynamicParameters.Add("@INTPRODUCTIONCO2", this.ProductionCO2);
                    dynamicParameters.Add("@NUMPRODUCTIONMWLENGTH", this.ProductionMWLength);
                    dynamicParameters.Add("@NUMPRODUCTIONCWLENGTH", this.ProductionCWLength);
                    dynamicParameters.Add("@INTPINSIZEID", this.PinSize);
                    dynamicParameters.Add("@INTUSERUID", UserId);
                    dynamicParameters.Add("@VCHPARAMVALUES", this.ParamValues);
                    dynamicParameters.Add("@VCHCWSPACING", this.CWSpacingString);
                    dynamicParameters.Add("@VCHMWBVBSSTRING", this.MWBVBSString);
                    dynamicParameters.Add("@VCHCWBVBSSTRING", this.CWBVBSString);
                    dynamicParameters.Add("@NVCHBOMDRAWINGPATH", this.BOMDrawingPath);
                    dynamicParameters.Add("@NUMPRODUCTIONAREA", this.ProductionArea);
                    dynamicParameters.Add("@INTINVOICECWQTY", this.InvoiceCWQty);
                    dynamicParameters.Add("@INTINVOICEMWQTY", this.InvoiceMWQty);
                    if (MWPitch == string.Empty || MWPitch == "")
                    {
                        dynamicParameters.Add("@MWPITCH", DBNull.Value);
                    }
                    else
                    {
                        dynamicParameters.Add("@MWPITCH", MWPitch);
                    }

                    dynamicParameters.Add("@MWFLAG", MWFlag);

                    if (CWPitch == string.Empty || CWPitch == "")
                    {
                        dynamicParameters.Add("@CWPITCH", DBNull.Value);
                    }
                    else
                    {
                        dynamicParameters.Add("@CWPITCH", CWPitch);
                    }

                    dynamicParameters.Add("@CWFLAG", CWFlag);
                    //object intProductMarkId = dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_ColumnProductMarkingDetails_Insert");

                  //  ProductGenerationOutput = Convert.ToString((dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_ColumnProductMarkingDetails_Insert")));
                    ProductGenerationOutput = sqlConnection.QueryFirstOrDefault<string>(SystemConstant.ColumnProductMarkingDetails_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);


                    if (ProductGenerationOutput.Contains(","))
                    {
                        string[] Output = ProductGenerationOutput.Split(',');
                        intProductMarkId = Convert.ToInt32(Output[0]);
                        this.ProductValidator = Convert.ToInt16(Output[1]);
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
    }
}
