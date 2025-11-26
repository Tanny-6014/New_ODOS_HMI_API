using Dapper;
using DetailingService.Constants;
using DetailingService.Context;
using DetailingService.Dtos;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DetailingService.Repositories
{
    public class BeamProduct
    {
        //private string connectionString = "Server=nsprddb10\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";
        private string connectionString = "Server=NSQADB5\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=MES_USER;Password=Mes@123;Connection Timeout=36000000";


        #region "Variables"
        public int ProductMarkID { get; set; }
        public int StructureMarkID { get; set; }
        public string ProductMarkingName { get; set; }
        public int BeamWidth { get; set; }
        public int BeamDepth { get; set; }
        public int BeamSlope { get; set; }
        public int CageWidth { get; set; }
        public int CageDepth { get; set; }
        public int CageSlope { get; set; }
        public int ProductCodeId { get; set; }
        public int MWSpacing { get; set; }
        public int CWSpacing { get; set; }
        public int ShapeCodeId { get; set; }
        public int Quantity { get; set; }
        public int InvoiceMWLength { get; set; }
        public int InvoiceCWLength { get; set; }
        public int InvoiceMWQty { get; set; }
        public int InvoiceCWQty { get; set; }
        public int InvoiceMWWeight { get; set; }
        public int InvoiceCWWeight { get; set; }
        public double InvoiceArea { get; set; }
        public int MO1 { get; set; }
        public int MO2 { get; set; }
        public int CO1 { get; set; }
        public int CO2 { get; set; }
        public double InvoiceWeight { get; set; }
        public double TheoraticalWeight { get; set; }
        public int ProductionArea { get; set; }
        public int ProductionMO1 { get; set; }
        public int ProductionMO2 { get; set; }
        public int ProductionCO1 { get; set; }
        public int ProductionCO2 { get; set; }
        public int ProductionMWLength { get; set; }
        public int ProductionCWLength { get; set; }
        public int ProductionMWWeight { get; set; }
        public int ProductionCWWeight { get; set; }
        public double ProductionWeight { get; set; }
        public string ShapeCode { get; set; }
        public string Parameters { get; set; }
        public int NoofLinks { get; set; }
        public int PinSize { get; set; }
        public int GenerationStatus { get; set; }
        public string ParamValues { get; set; }
        public string BOMDrawingPath { get; set; }
        public string MWBVBSString { get; set; }
        public string CWBVBSString { get; set; }
        public string BOMIndicator { get; set; }
        public ShapeParameterCollection ShapeParam { get; set; }
        //public List<ShapeParameter> ShapeParameters { get; set; }
        public string MWPitch { get; set; }
        public string CWPitch { get; set; }
        public int MWFlag { get; set; }
        public int CWFlag { get; set; }

        public int ProductValidator { get; set; }

        #endregion

        public BeamProduct()
        {
        }
      

        # region "Product Save"

        public bool Save()
        {
            bool isSuccess = false;
            object intProductMarkId = null;
            string ProductGenerationOutput = null;

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intProductMarkId", ProductMarkID);
                    dynamicParameters.Add("@intStructureMarkId", StructureMarkID);
                    dynamicParameters.Add("@vchProductMarkingName", ProductMarkingName);
                    dynamicParameters.Add("@intProductCode", ProductCodeId);
                    dynamicParameters.Add("@numCageWidth", CageWidth);
                    dynamicParameters.Add("@numCageDepth", CageDepth);
                    dynamicParameters.Add("@numCageSlope", CageSlope);
                    dynamicParameters.Add("@numBeamWidth", BeamWidth);
                    dynamicParameters.Add("@numBeamDepth", BeamDepth);
                    dynamicParameters.Add("@numInvoiceMWLength", InvoiceMWLength);
                    dynamicParameters.Add("@numInvoiceCWLength", InvoiceCWLength);
                    dynamicParameters.Add("@intShapeCodeId", ShapeCodeId);
                    dynamicParameters.Add("@intInvoiceMWQty", InvoiceMWQty);
                    dynamicParameters.Add("@intInvoiceCWQty", InvoiceCWQty);
                    dynamicParameters.Add("@numInvoiceMWWeight", InvoiceMWWeight);
                    dynamicParameters.Add("@numInvoiceCWWeight", InvoiceCWWeight);
                    dynamicParameters.Add("@numInvoiceArea", InvoiceArea);
                    dynamicParameters.Add("@numProductionArea", ProductionArea);
                    dynamicParameters.Add("@numInvoiceWeight", InvoiceWeight);
                    dynamicParameters.Add("@numTheoraticalWeight", InvoiceWeight);
                    dynamicParameters.Add("@intMemberQty", Quantity);
                    dynamicParameters.Add("@intMO1", MO1);
                    dynamicParameters.Add("@intMO2", MO2);
                    dynamicParameters.Add("@intCO1", CO1);
                    dynamicParameters.Add("@intCO2", CO2);
                    dynamicParameters.Add("@intProductionMO1", ProductionMO1);
                    dynamicParameters.Add("@intProductionMO2", ProductionMO2);
                    dynamicParameters.Add("@intProductionCO1", ProductionCO1);
                    dynamicParameters.Add("@intProductionCO2", ProductionCO2);
                    dynamicParameters.Add("@numProductionMWLength", ProductionMWLength);
                    dynamicParameters.Add("@numProductionCWLength", ProductionCWLength);
                    dynamicParameters.Add("@numProductionMWWeight", ProductionMWWeight);
                    dynamicParameters.Add("@numProductionCWWeight", ProductionCWWeight);
                    dynamicParameters.Add("@numProductionWeight", ProductionWeight);
                    dynamicParameters.Add("@intPinSizeId", PinSize);
                    dynamicParameters.Add("@tntGenerationStatus", 0);
                    dynamicParameters.Add("@numMWSpacing", MWSpacing);
                    dynamicParameters.Add("@numCWSpacing", CWSpacing);
                    dynamicParameters.Add("@ParamValues", ParamValues);
                    dynamicParameters.Add("@nvchBOMDrawingPath", BOMDrawingPath);
                    dynamicParameters.Add("@nvchMWBVBSString", MWBVBSString);
                    dynamicParameters.Add("@nvchCWBVBSString", CWBVBSString);
                    object syncLock = new object();

                    lock (syncLock)
                    {
                        // intProductMarkId = dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_ProductMarkingDetailsInsert");
                        //  ProductGenerationOutput = Convert.ToString((dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_ProductMarkingDetailsInsert")));
                        ProductGenerationOutput = sqlConnection.QueryFirstOrDefault<string>(SystemConstant.ProductMarkingDetailsInsert, dynamicParameters, commandType: CommandType.StoredProcedure);
                        if (ProductGenerationOutput.Contains(","))
                        {
                            string[] Output = ProductGenerationOutput.Split(',');
                            intProductMarkId = Convert.ToInt32(Output[0]);
                            ProductValidator = Convert.ToInt16(Output[1]);
                        }
                    }
                    ProductMarkID = Convert.ToInt32(intProductMarkId);
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isSuccess;
        }

        # endregion

        # region "Get Product"

        public List<BeamProduct> ProductMarkingByGroupMarkId_Get(int StructureMarkId)
        {

            List<BeamProduct> listProductMarkForBeam = new List<BeamProduct> { };
            DataSet dsProductMarkForBeam = new DataSet();
            IEnumerable<BeamProductByStructureMarkIdDto> beamProductByStructureMarkIdDto;


            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@STRUCTUREMARKID", StructureMarkId);
                    //dbManager.AddParameters(1, "@INTPROJECTID", ProjectId);
                    //dbManager.AddParameters(2, "@INSEDETAILINGID", SeDetailingId);
                    // dsProductMarkForBeam = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_ProductMarkingByGroupMarkId_Get");
                    beamProductByStructureMarkIdDto = sqlConnection.Query<BeamProductByStructureMarkIdDto>(SystemConstant.ProductMarkingByGroupMarkId_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (beamProductByStructureMarkIdDto.Count() > 0)
                    {
                        DataTable dt = ConvertToDataTable.ToDataTable(beamProductByStructureMarkIdDto);
                        dsProductMarkForBeam.Tables.Add(dt);
                    }

                    //ShapeParameter objShapeParam = new ShapeParameter();
                    ShapeParameterCollection shapeParameterCollection = new ShapeParameterCollection();
                    ShapeParameter listShapeParameterForBeam = new ShapeParameter { };
                    //  if (IndexusDistributionCache.SharedCache.Get("ShapeparamCache") == null)
                    // {
                    shapeParameterCollection = shapeParameterCollection.ShapeParameterForBeam_Get();

                    //}
                    //else
                    //{
                    //    shapeParameterCollection = (ShapeParameterCollection)IndexusDistributionCache.SharedCache.Get("ShapeparamCache");
                    //}
                    ShapeParameterCollection strucParamCollFilter = new ShapeParameterCollection();
                    if (dsProductMarkForBeam != null && dsProductMarkForBeam.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBeam in dsProductMarkForBeam.Tables[0].DefaultView)
                        {
                            foreach (ShapeParameter shapeParamCollection in shapeParameterCollection)
                            {
                                if (shapeParamCollection.ShapeId == Convert.ToInt32(drvBeam["INTSHAPECODEID"]))
                                {
                                    strucParamCollFilter.Add(shapeParamCollection);
                                }
                            }

                            BeamProduct productMark = new BeamProduct
                            {
                                ProductMarkID = Convert.ToInt32(drvBeam["INTPRODUCTMARKID"]),
                                StructureMarkID = Convert.ToInt32(drvBeam["INTSTRUCTUREMARKID"]),
                                ProductMarkingName = Convert.ToString(drvBeam["VCHPRODUCTMARKINGNAME"]),
                                BeamWidth = Convert.ToInt32(drvBeam["NUMBEAMWIDTH"]),
                                BeamDepth = Convert.ToInt32(drvBeam["NUMBEAMDEPTH"]),
                                BeamSlope = Convert.ToInt32(drvBeam["NUMBEAMSLOPE"]),
                                CageWidth = Convert.ToInt32(drvBeam["NUMCAGEWIDTH"]),
                                CageDepth = Convert.ToInt32(drvBeam["NUMCAGEDEPTH"]),
                                CageSlope = Convert.ToInt32(drvBeam["NUMCAGESLOPE"]),
                                ProductCodeId = Convert.ToInt32(drvBeam["INTPRODUCTCODE"]),
                                MWSpacing = Convert.ToInt32(drvBeam["NUMMWSPACING"]),
                                CWSpacing = Convert.ToInt32(drvBeam["NUMCWSPACING"].ToString()),
                                ShapeCodeId = Convert.ToInt32(drvBeam["INTSHAPECODEID"]),
                                Quantity = Convert.ToInt32(drvBeam["INTMEMBERQTY"]),
                                InvoiceMWLength = Convert.ToInt32(drvBeam["NUMINVOICEMWLENGTH"]),
                                InvoiceCWLength = Convert.ToInt32(drvBeam["NUMINVOICECWLENGTH"]),
                                InvoiceMWQty = Convert.ToInt32(drvBeam["INTINVOICEMWQTY"]),
                                InvoiceCWQty = Convert.ToInt32(drvBeam["INTINVOICECWQTY"]),
                                InvoiceMWWeight = Convert.ToInt32(drvBeam["NUMINVOICEMWWEIGHT"]),
                                InvoiceCWWeight = Convert.ToInt32(drvBeam["NUMINVOICECWWEIGHT"]),
                                InvoiceArea = Convert.ToInt32(drvBeam["NUMINVOICEAREA"]),
                                MO1 = Convert.ToInt32(drvBeam["INTMO1"]),
                                MO2 = Convert.ToInt32(drvBeam["INTMO2"]),
                                CO1 = Convert.ToInt32(drvBeam["INTCO1"]),
                                CO2 = Convert.ToInt32(drvBeam["INTCO2"]),
                                InvoiceWeight = Convert.ToDouble(drvBeam["NUMINVOICEWEIGHT"]),
                                TheoraticalWeight = Convert.ToDouble(drvBeam["NUMTHEORATICALWEIGHT"]),
                                ProductionArea = Convert.ToInt32(drvBeam["NUMPRODUCTIONAREA"]),
                                ProductionMO1 = Convert.ToInt32(drvBeam["INTPRODUCTIONMO1"]),
                                ProductionMO2 = Convert.ToInt32(drvBeam["INTPRODUCTIONMO2"]),
                                ProductionCO1 = Convert.ToInt32(drvBeam["INTPRODUCTIONCO1"]),
                                ProductionCO2 = Convert.ToInt32(drvBeam["INTPRODUCTIONCO2"]),
                                ProductionMWLength = Convert.ToInt32(drvBeam["NUMPRODUCTIONMWLENGTH"]),
                                ProductionCWLength = Convert.ToInt32(drvBeam["NUMPRODUCTIONCWLENGTH"]),
                                ProductionMWWeight = Convert.ToInt32(drvBeam["NUMPRODUCTIONMWWEIGHT"]),
                                ProductionCWWeight = Convert.ToInt32(drvBeam["NUMPRODUCTIONCWWEIGHT"]),
                                ProductionWeight = Convert.ToInt32(drvBeam["NUMPRODUCTIONWEIGHT"]),
                                ShapeCode = Convert.ToString(drvBeam["CHRSHAPECODE"]),
                                //Parameters = Convert.ToDouble(drvBeam["DECWEIGHTAREA"]),
                                //NoofLinks = Convert.ToDouble(drvBeam["DECWEIGTHPERMETERRUN"]),
                                PinSize = Convert.ToInt32(drvBeam["INTPINSIZEID"]),
                                GenerationStatus = Convert.ToInt32(drvBeam["TNTGENERATIONSTATUS"]),
                                ParamValues = Convert.ToString(drvBeam["PARAMVALUES"]),
                                BOMDrawingPath = Convert.ToString(drvBeam["NVCHBOMDRAWINGPATH"]),
                                MWBVBSString = Convert.ToString(drvBeam["NVCHMWBVBSSTRING"]),
                                CWBVBSString = Convert.ToString(drvBeam["NVCHCWBVBSSTRING"]),
                                BOMIndicator = drvBeam["BOMIND"].ToString(),
                                ShapeParam = strucParamCollFilter,
                                ProductValidator = Convert.ToInt32(drvBeam["INTPRODUCTVALIDATOR"])
                            };
                            listProductMarkForBeam.Add(productMark);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listProductMarkForBeam;

        }

        public string ParamValuesByProductMarkId_Get(int ProductMarkId, string StructureElementType)
        {

            string paramValues = "";
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@ProductMarkId", ProductMarkId);
                    dynamicParameters.Add("@StructureElementType", StructureElementType);
                    // paramValues = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_ParamValuesByProductMarkId_Get"));
                    paramValues = sqlConnection.QueryFirstOrDefault<string>(SystemConstant.ParamValuesByProductMarkId_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return paramValues;
        }

        #endregion

        # region "Product Delete"

        public bool DeleteProductmark(int StructureMarkId)
        {
            bool isSuccess = false;
            // DBManager dbManager = new DBManager();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTSTRUCTUREMARKID", StructureMarkId);
                    //  dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_ProductmarkByStructId_Delete");
                    sqlConnection.Query<int>(SystemConstant.ProductmarkByStructId_Delete, dynamicParameters, commandType: CommandType.StoredProcedure);

                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isSuccess;
        }

        #endregion
    }
}
