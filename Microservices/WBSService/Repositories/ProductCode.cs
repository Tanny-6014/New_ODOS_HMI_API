using WBSService.Constants;
using Dapper;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace WBSService.Repositories
{
    public class ProductCode
    {
        private readonly IConfiguration _configuration;
        //  private string connectionString;

        private string connectionString = "Server=NSQADB5\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=NDSWebApps;Password=NDS4DBAdmin*";
        //private string connectionString = "Server=nsprddb10\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";

        #region "Properties"
        public int ProductCodeId { get; set; }
        public string ProductCodeName { get; set; }
        public Int32 StructureElementTypeId { get; set; }
        public Int32 ProductTypeId { get; set; }
        public Int32 MainWireDia { get; set; }
        public Int32 MainWireSpacing { get; set; }
        public Int32 CrossWireSpacing { get; set; }
        public double WeightArea { get; set; }
        public double WeightPerMeterRun { get; set; }
        public Int32 MinLinkFactor { get; set; }
        public Int32 MaxLinkFactor { get; set; }
        public double CwDia { get; set; }
        public double CwWeightPerMeterRun { get; set; }
        public double DECMWLength { get; set; }

        public Int32 SAPMaterialCodeId { get; set; }

        public string SAPMaterialCode { get; set; }




        # endregion
        public ProductCode()
        {
        }
        public ProductCode( IConfiguration configuration)
        {
           
            _configuration = configuration;

            connectionString = _configuration.GetConnectionString(SystemConstant.DefaultDBConnection);
        }

        ////public ProductCode(int productCodeId, string productCodeName, int structureElementTypeId, int productTypeId, int mainWireDia, int mainWireSpacing, double weightArea, double weightPerMeterRun, int minLinkFactor, int maxLinkFactor, double cwDia, double cwWeightPerMeterRun)
        ////{
        ////    this.ProductCodeId = productCodeId;
        ////    this.ProductCodeName = productCodeName;
        ////    this.StructureElementTypeId = structureElementTypeId;
        ////    this.ProductTypeId = productTypeId;
        ////    this.MainWireDia = mainWireDia;
        ////    this.MainWireSpacing = mainWireSpacing;
        ////    this.WeightArea = weightArea;
        ////    this.WeightPerMeterRun = weightPerMeterRun;
        ////    this.MinLinkFactor = minLinkFactor;
        ////    this.MaxLinkFactor = maxLinkFactor;
        ////    this.CwDia = cwDia;
        ////    this.CwWeightPerMeterRun = cwWeightPerMeterRun;
        ////}

        //#region "Beam Product Code"
        //public List<ProductCode> ProductCodeForBeam_Get(int StructureElementTypeId, int ProductTyprId)
        //{
        //    //
        //    List<ProductCode> listProductCodeForBeam = new List<ProductCode> { };
        //    DataSet dsProductCodeForBeam = new DataSet();
        //    IEnumerable<BeamProductCodeListDto> beamProductCodeListDto;
        //    try
        //    {
        //        //string ProductCodeCache = "ProductCodeCache" + StructureElementTypeId.ToString() + ProductTyprId.ToString();
        //        //if (IndexusDistributionCache.SharedCache.Get(ProductCodeCache) == null)
        //        //{//Hardcoded value//
        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {
        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            dynamicParameters.Add("@INTSTRUCTUREELEMENTTYPEID", StructureElementTypeId);
        //            dynamicParameters.Add("@SITPRODUCTTYPEID", ProductTyprId);
        //            // dsProductCodeForBeam = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_ProductCodeForBeam_Get");
        //            beamProductCodeListDto = sqlConnection.Query<BeamProductCodeListDto>(SystemConstant.ProductCodeForBeam_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

        //            if (beamProductCodeListDto.Count() > 0)
        //            {
        //                DataTable dt = ConvertToDataTable.ToDataTable(beamProductCodeListDto);
        //                dsProductCodeForBeam.Tables.Add(dt);
        //            }

        //            if (dsProductCodeForBeam != null && dsProductCodeForBeam.Tables.Count != 0)
        //            {
        //                foreach (DataRowView drvBeam in dsProductCodeForBeam.Tables[0].DefaultView)
        //                {
        //                    ProductCode productCode = new ProductCode
        //                    {
        //                        ProductCodeId = Convert.ToInt32(drvBeam["INTPRODUCTCODEID"]),
        //                        ProductCodeName = drvBeam["VCHPRODUCTCODE"].ToString(),
        //                        MainWireDia = Convert.ToInt32(drvBeam["DECMWDIAMETER"]),
        //                        MainWireSpacing = Convert.ToInt32(drvBeam["INTMWSPACE"]),
        //                        WeightArea = Convert.ToDouble(drvBeam["DECWEIGHTAREA"]),
        //                        WeightPerMeterRun = Convert.ToDouble(drvBeam["DECWEIGTHPERMETERRUN"]),
        //                        MinLinkFactor = Convert.ToInt32(drvBeam["INTMINLINKFACTOR"]),
        //                        MaxLinkFactor = Convert.ToInt32(drvBeam["INTMAXLINKFACTOR"]),
        //                        CwDia = Convert.ToDouble(drvBeam["DECCWDIAMETER"]),
        //                        CwWeightPerMeterRun = Convert.ToDouble(drvBeam["DECCWWEIGTHPERMETERRUN"])
        //                    };
        //                    listProductCodeForBeam.Add(productCode);
        //                }
        //                //IndexusDistributionCache.SharedCache.Add(ProductCodeCache, listProductCodeForBeam, DateTime.Today.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings.Get("cacheTimeOut"))));
        //                //}
        //                //else
        //                //{
        //                //    listProductCodeForBeam = IndexusDistributionCache.SharedCache.Get(ProductCodeCache) as List<ProductCode>;
        //                //}
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return listProductCodeForBeam;

        //}
        //public List<ProductCode> FilterProductCodeForBeam_Get(int StructureElementTypeId, int ProductTyprId, string enteredText)
        //{
        //    //
        //    List<ProductCode> listProductCodeForBeam = new List<ProductCode> { };
        //    DataSet dsProductCodeForBeam = new DataSet();
        //    try
        //    {
        //        string ProductCodeCache = "ProductCodeCache" + StructureElementTypeId.ToString() + ProductTyprId.ToString();
        //        //if (IndexusDistributionCache.SharedCache.Get(ProductCodeCache) == null)
        //        //{//Hardcoded value//
        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {
        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            dynamicParameters.Add("@INTSTRUCTUREELEMENTTYPEID", StructureElementTypeId);
        //            dynamicParameters.Add("@SITPRODUCTTYPEID", ProductTyprId);
        //            dynamicParameters.Add("@VCHENTEREDTEXT", enteredText);
        //            //dsProductCodeForBeam = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_FilterProductCodeForBeam_Get");
        //            dsProductCodeForBeam = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.FilterProductCodeForBeam_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

        //            if (dsProductCodeForBeam != null && dsProductCodeForBeam.Tables.Count != 0)
        //            {
        //                foreach (DataRowView drvBeam in dsProductCodeForBeam.Tables[0].DefaultView)
        //                {
        //                    ProductCode productCode = new ProductCode
        //                    {
        //                        ProductCodeId = Convert.ToInt32(drvBeam["INTPRODUCTCODEID"]),
        //                        ProductCodeName = drvBeam["VCHPRODUCTCODE"].ToString(),
        //                        MainWireDia = Convert.ToInt32(drvBeam["DECMWDIAMETER"]),
        //                        MainWireSpacing = Convert.ToInt32(drvBeam["INTMWSPACE"]),
        //                        WeightArea = Convert.ToDouble(drvBeam["DECWEIGHTAREA"]),
        //                        WeightPerMeterRun = Convert.ToDouble(drvBeam["DECWEIGTHPERMETERRUN"]),
        //                        MinLinkFactor = Convert.ToInt32(drvBeam["INTMINLINKFACTOR"]),
        //                        MaxLinkFactor = Convert.ToInt32(drvBeam["INTMAXLINKFACTOR"]),
        //                        CwDia = Convert.ToDouble(drvBeam["DECCWDIAMETER"]),
        //                        CwWeightPerMeterRun = Convert.ToDouble(drvBeam["DECCWWEIGTHPERMETERRUN"])
        //                    };
        //                    listProductCodeForBeam.Add(productCode);
        //                }
        //                // IndexusDistributionCache.SharedCache.Add(ProductCodeCache, listProductCodeForBeam, DateTime.Today.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings.Get("cacheTimeOut"))));

        //                //}
        //                //else
        //                //{
        //                //    listProductCodeForBeam = IndexusDistributionCache.SharedCache.Get(ProductCodeCache) as List<ProductCode>;

        //                //}
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return listProductCodeForBeam;

        //}
        //# endregion

        //#region "Beam CAP Product Code"
        //public List<ProductCode> CapProductForBeam_Get()
        //{
        //    //
        //    List<ProductCode> listCapProductCodeForBeam = new List<ProductCode> { };
        //    DataSet dsCapProductForBeam = new DataSet();
        //    IEnumerable<BeamCapProductDto> beamCapProductDto;
        //    try
        //    {
        //        //if (IndexusDistributionCache.SharedCache.Get("CacheCAPProuctCode") == null)
        //        //{
        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {
        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            // dsCapProductForBeam = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_CapProductForBeam_Get");
        //            beamCapProductDto = sqlConnection.Query<BeamCapProductDto>(SystemConstant.CapProductForBeam_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

        //            if (beamCapProductDto.Count() > 0)
        //            {
        //                DataTable dt = ConvertToDataTable.ToDataTable(beamCapProductDto);
        //                dsCapProductForBeam.Tables.Add(dt);
        //            }

        //            if (dsCapProductForBeam != null && dsCapProductForBeam.Tables.Count != 0)
        //            {
        //                foreach (DataRowView drvBeam in dsCapProductForBeam.Tables[0].DefaultView)
        //                {
        //                    ProductCode productCode = new ProductCode
        //                    {
        //                        ProductCodeId = Convert.ToInt32(drvBeam["INTPRODUCTCODEID"]),
        //                        ProductCodeName = drvBeam["VCHPRODUCTCODE"].ToString()
        //                    };
        //                    listCapProductCodeForBeam.Add(productCode);
        //                }
        //                //    IndexusDistributionCache.SharedCache.Add("CacheCAPProuctCode", listCapProductCodeForBeam, DateTime.Today.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings.Get("cacheTimeOut"))));
        //                //}
        //                //else
        //                //{
        //                //    listCapProductCodeForBeam = IndexusDistributionCache.SharedCache.Get("CacheCAPProuctCode") as List<ProductCode>;
        //                //}
        //            }
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return listCapProductCodeForBeam;

        //}
        //public List<ProductCode> FilterCapProductForBeam_Get(string enteredText)
        //{
        //    //DBManager dbManager = new DBManager();
        //    List<ProductCode> listCapProductCodeForBeam = new List<ProductCode> { };
        //    DataSet dsCapProductForBeam = new DataSet();
        //    try
        //    {
        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {
        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            dynamicParameters.Add("@VCHENTEREDTEXT", enteredText);
        //            //dsCapProductForBeam = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_FilterCapProductForBeam_Get");
        //            dsCapProductForBeam = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.FilterCapProductForBeam_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

        //            if (dsCapProductForBeam != null && dsCapProductForBeam.Tables.Count != 0)
        //            {
        //                foreach (DataRowView drvBeam in dsCapProductForBeam.Tables[0].DefaultView)
        //                {
        //                    ProductCode productCode = new ProductCode
        //                    {
        //                        ProductCodeId = Convert.ToInt32(drvBeam["INTPRODUCTCODEID"]),
        //                        ProductCodeName = drvBeam["VCHPRODUCTCODE"].ToString()
        //                    };
        //                    listCapProductCodeForBeam.Add(productCode);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return listCapProductCodeForBeam;

        //}
        //# endregion

        //#region "Column Product Code"
        //public List<ProductCode> ColumnProductCode(int StructureElementTypeId, int ProductTypeId, string enteredText)
        //{

        //    List<ProductCode> listColumnProductCode = new List<ProductCode> { };
        //    DataSet dsColumnProductCode = new DataSet();
        //    IEnumerable<ColumnProductCodeBindDto> columnProductCodeBindDto;
        //    try
        //    {
        //        //string ColumnProductCodeCache = "ColumnProductCodeCache" + StructureElementTypeId.ToString() + ProductTypeId.ToString();
        //        //if (IndexusDistributionCache.SharedCache.Get(ColumnProductCodeCache) == null)
        //        //{

        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {
        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            dynamicParameters.Add("@INTSTRUCTUREELEMENTTYPEID", StructureElementTypeId);
        //            dynamicParameters.Add("@SITPRODUCTTYPEID", ProductTypeId);
        //            dynamicParameters.Add("@VCHENTEREDTEXT", enteredText);
        //            // dsColumnProductCode = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_ColumnProductCode_Get");
        //            columnProductCodeBindDto = sqlConnection.Query<ColumnProductCodeBindDto>(SystemConstant.ColumnProductCode_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
        //            if (columnProductCodeBindDto.Count() > 0)
        //            {
        //                DataTable dt = ConvertToDataTable.ToDataTable(columnProductCodeBindDto);
        //                dsColumnProductCode.Tables.Add(dt);
        //            }
        //            if (dsColumnProductCode != null && dsColumnProductCode.Tables.Count != 0)
        //            {
        //                foreach (DataRowView drvBeam in dsColumnProductCode.Tables[0].DefaultView)
        //                {
        //                    ProductCode productCode = new ProductCode
        //                    {
        //                        ProductCodeId = Convert.ToInt32(drvBeam["INTPRODUCTCODEID"]),
        //                        ProductCodeName = drvBeam["VCHPRODUCTCODE"].ToString(),
        //                        MainWireDia = Convert.ToInt32(drvBeam["DECMWDIAMETER"]),
        //                        MainWireSpacing = Convert.ToInt32(drvBeam["INTMWSPACE"]),
        //                        WeightArea = Convert.ToDouble(drvBeam["DECWEIGHTAREA"]),
        //                        WeightPerMeterRun = Convert.ToDouble(drvBeam["DECWEIGTHPERMETERRUN"]),
        //                        MinLinkFactor = Convert.ToInt32(drvBeam["INTMINLINKFACTOR"]),
        //                        MaxLinkFactor = Convert.ToInt32(drvBeam["INTMAXLINKFACTOR"]),
        //                        CwDia = Convert.ToDouble(drvBeam["DECCWDIAMETER"]),
        //                        CwWeightPerMeterRun = Convert.ToDouble(drvBeam["DECCWWEIGTHPERMETERRUN"])
        //                    };
        //                    listColumnProductCode.Add(productCode);
        //                }
        //                //    IndexusDistributionCache.SharedCache.Add(ColumnProductCodeCache, listColumnProductCode, DateTime.Today.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings.Get("cacheTimeOut"))));
        //                //}
        //                //else
        //                //{
        //                //    listColumnProductCode = IndexusDistributionCache.SharedCache.Get(ColumnProductCodeCache) as List<ProductCode>;
        //                //}
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return listColumnProductCode;

        //}
        //public List<ProductCode> ColumnProductCodeBind(int StructureElementTypeId, int ProductTypeId)
        //{
        //    //
        //    List<ProductCode> listColumnProductCode = new List<ProductCode> { };
        //    DataSet dsColumnProductCode = new DataSet();
        //    IEnumerable<ColumnProductCodeBindDto> columnProductCodeBindDto;
        //    try
        //    {

        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {
        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            dynamicParameters.Add("@INTSTRUCTUREELEMENTTYPEID", StructureElementTypeId);
        //            dynamicParameters.Add("@SITPRODUCTTYPEID", ProductTypeId);
        //            //dsColumnProductCode = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_ColumnProductCodeBind_Get");
        //            columnProductCodeBindDto = sqlConnection.Query<ColumnProductCodeBindDto>(SystemConstant.ColumnProductCodeBind_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

        //            if (columnProductCodeBindDto.Count() > 0)
        //            {
        //                DataTable dt = ConvertToDataTable.ToDataTable(columnProductCodeBindDto);
        //                dsColumnProductCode.Tables.Add(dt);

        //                if (dsColumnProductCode != null && dsColumnProductCode.Tables.Count != 0)
        //                {
        //                    foreach (DataRowView drvBeam in dsColumnProductCode.Tables[0].DefaultView)
        //                    {
        //                        ProductCode productCode = new ProductCode
        //                        {
        //                            ProductCodeId = Convert.ToInt32(drvBeam["INTPRODUCTCODEID"]),
        //                            ProductCodeName = drvBeam["VCHPRODUCTCODE"].ToString(),
        //                            MainWireDia = Convert.ToInt32(drvBeam["DECMWDIAMETER"]),
        //                            MainWireSpacing = Convert.ToInt32(drvBeam["INTMWSPACE"]),
        //                            WeightArea = Convert.ToDouble(drvBeam["DECWEIGHTAREA"]),
        //                            WeightPerMeterRun = Convert.ToDouble(drvBeam["DECWEIGTHPERMETERRUN"]),
        //                            MinLinkFactor = Convert.ToInt32(drvBeam["INTMINLINKFACTOR"]),
        //                            MaxLinkFactor = Convert.ToInt32(drvBeam["INTMAXLINKFACTOR"]),
        //                            CwDia = Convert.ToDouble(drvBeam["DECCWDIAMETER"]),
        //                            CwWeightPerMeterRun = Convert.ToDouble(drvBeam["DECCWWEIGTHPERMETERRUN"])
        //                        };
        //                        listColumnProductCode.Add(productCode);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return listColumnProductCode;

        //}
        //# endregion

        //#region "Column CLink Product Code"
        //public List<ProductCode> ColumnCLinkProductLength_Get(string enteredText)
        //{

        //    List<ProductCode> listColumnClinkProductCodelength = new List<ProductCode> { };
        //    DataSet dsColumnClinkProductCodelength = new DataSet();
        //    IEnumerable<ColumnClinkProductDto> columnClinkProductDto;

        //    try
        //    {

        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {
        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            dynamicParameters.Add("@VCHPRODUCTCODETEXT", enteredText);
        //            //dsColumnClinkProductCodelength = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_ColumnCLinkProduct_Get");
        //            columnClinkProductDto = sqlConnection.Query<ColumnClinkProductDto>(SystemConstant.ColumnCLinkProduct_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

        //            if (columnClinkProductDto.Count() > 0)
        //            {
        //                DataTable dt = ConvertToDataTable.ToDataTable(columnClinkProductDto);
        //                dsColumnClinkProductCodelength.Tables.Add(dt);
        //            }
        //            if (dsColumnClinkProductCodelength != null && dsColumnClinkProductCodelength.Tables.Count != 0)
        //            {
        //                foreach (DataRowView drvBeam in dsColumnClinkProductCodelength.Tables[0].DefaultView)
        //                {

        //                    ProductCode productCode = new ProductCode
        //                    {
        //                        ProductCodeId = Convert.ToInt32(drvBeam["INTPRODUCTCODEID"]),
        //                        ProductCodeName = drvBeam["VCHPRODUCTCODE"].ToString()
        //                    };

        //                    listColumnClinkProductCodelength.Add(productCode);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return listColumnClinkProductCodelength;

        //}
        //public List<ProductCode> ColumnCLinkProducttLengthBind_Get()
        //{

        //    List<ProductCode> listColumnClinkProductCodelength = new List<ProductCode> { };

        //    DataSet dsColumnClinkProductCodelength = new DataSet();

        //    try
        //    {

        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {
        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();

        //            // dsColumnClinkProductCodelength = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_ColumnCLinkProductBind_Get");
        //            dsColumnClinkProductCodelength = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.ColumnCLinkProductBind_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

        //            if (dsColumnClinkProductCodelength != null && dsColumnClinkProductCodelength.Tables.Count != 0)
        //            {
        //                foreach (DataRowView drvBeam in dsColumnClinkProductCodelength.Tables[0].DefaultView)
        //                {

        //                    ProductCode productCode = new ProductCode
        //                    {
        //                        ProductCodeId = Convert.ToInt32(drvBeam["INTPRODUCTCODEID"]),
        //                        ProductCodeName = drvBeam["VCHPRODUCTCODE"].ToString()
        //                    };

        //                    listColumnClinkProductCodelength.Add(productCode);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return listColumnClinkProductCodelength;

        //}
        //# endregion

        //# region "Slab Product Code"
        //public List<ProductCode> SlabProductCode()
        //{

        //    List<ProductCode> listSlabProductCode = new List<ProductCode> { };
        //    DataSet dsSlabProductCode = new DataSet();
        //    IEnumerable<SlabProductCodeDto> SlabProductCodeDto;
        //    try
        //    {
        //        //string ColumnProductCodeCache = "listSlabProductCode" ;
        //        //if (IndexusDistributionCache.SharedCache.Get(ColumnProductCodeCache) == null)
        //        //{

        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {
        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();

        //            // dsSlabProductCode = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_SlabProductCode_Get");
        //            SlabProductCodeDto = sqlConnection.Query<SlabProductCodeDto>(SystemConstant.SlabProductCode_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

        //            if (SlabProductCodeDto.Count() > 0)
        //            {
        //                DataTable dt = ConvertToDataTable.ToDataTable(SlabProductCodeDto);
        //                dsSlabProductCode.Tables.Add(dt);

        //                if (dsSlabProductCode != null && dsSlabProductCode.Tables.Count != 0)
        //                {
        //                    foreach (DataRowView drvBeam in dsSlabProductCode.Tables[0].DefaultView)
        //                    {
        //                        ProductCode productCode = new ProductCode
        //                        {
        //                            ProductCodeId = Convert.ToInt32(drvBeam["INTPRODUCTCODEID"]),
        //                            ProductCodeName = drvBeam["VCHPRODUCTCODE"].ToString(),
        //                            MainWireSpacing = Convert.ToInt32(drvBeam["INTMWSPACE"]),
        //                            CrossWireSpacing = Convert.ToInt32(drvBeam["INTCWSPACE"]),
        //                            MainWireDia = Convert.ToInt32(drvBeam["DECMWDIAMETER"]),
        //                            CwDia = Convert.ToDouble(drvBeam["DECCWDIAMETER"]),
        //                            WeightArea = Convert.ToDouble(drvBeam["DECWEIGHTAREA"]),
        //                            WeightPerMeterRun = Convert.ToDouble(drvBeam["DECWEIGTHPERMETERRUN"]),
        //                            CwWeightPerMeterRun = Convert.ToDouble(drvBeam["DECCWWEIGTHPERMETERRUN"])
        //                        };
        //                        listSlabProductCode.Add(productCode);
        //                    }
        //                    //    IndexusDistributionCache.SharedCache.Add(ColumnProductCodeCache, listSlabProductCode, DateTime.Today.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings.Get("cacheTimeOut"))));
        //                    //}
        //                    //else
        //                    //{
        //                    //    listSlabProductCode = IndexusDistributionCache.SharedCache.Get(ColumnProductCodeCache) as List<ProductCode>;
        //                    //}
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return listSlabProductCode;
        //}
        //public List<ProductCode> SlabProductCodeFilter(string enteredText)
        //{

        //    List<ProductCode> listSlabProductCode = new List<ProductCode> { };
        //    DataSet dsSlabProductCode = new DataSet();
        //    IEnumerable<SlabProductCodeDto> productCodes;
        //    try


        //    {
        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {
        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            dynamicParameters.Add("@VCHENTEREDTEXT", enteredText);
        //            // dsSlabProductCode = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_SlabProductCodeFilter_Get");
        //            productCodes = sqlConnection.Query<SlabProductCodeDto>(SystemConstant.SlabProductCodeFilter_Get, dynamicParameters, commandType: CommandType.StoredProcedure);


        //            if (productCodes.Count() > 0)
        //            {
        //                DataTable dt = ConvertToDataTable.ToDataTable(productCodes);
        //                dsSlabProductCode.Tables.Add(dt);
        //            }

        //            if (dsSlabProductCode != null && dsSlabProductCode.Tables.Count != 0)
        //            {
        //                foreach (DataRowView drvBeam in dsSlabProductCode.Tables[0].DefaultView)
        //                {
        //                    ProductCode productCode = new ProductCode
        //                    {
        //                        ProductCodeId = Convert.ToInt32(drvBeam["INTPRODUCTCODEID"]),
        //                        ProductCodeName = drvBeam["VCHPRODUCTCODE"].ToString(),
        //                        MainWireSpacing = Convert.ToInt32(drvBeam["INTMWSPACE"]),
        //                        CrossWireSpacing = Convert.ToInt32(drvBeam["INTCWSPACE"]),
        //                        MainWireDia = Convert.ToInt32(drvBeam["DECMWDIAMETER"]),
        //                        CwDia = Convert.ToDouble(drvBeam["DECCWDIAMETER"]),
        //                        WeightArea = Convert.ToDouble(drvBeam["DECWEIGHTAREA"]),
        //                        WeightPerMeterRun = Convert.ToDouble(drvBeam["DECWEIGTHPERMETERRUN"]),
        //                        CwWeightPerMeterRun = Convert.ToDouble(drvBeam["DECCWWEIGTHPERMETERRUN"]),
        //                        SAPMaterialCodeId = Convert.ToInt32(drvBeam["intSAPMaterialCodeId"]),
        //                        SAPMaterialCode = drvBeam["vchMaterialNumber"].ToString(),

        //                    };
        //                    listSlabProductCode.Add(productCode);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return listSlabProductCode;
        //}
        //# endregion

        //// CARPET Anuran  
        //# region "CARPET Product Code" 

        //// Modified for CoreCage
        //public List<ProductCode> CarpetProductCode()
        //{

        //    List<ProductCode> listCarpetProductCode = new List<ProductCode> { };
        //    DataSet dsCarpetProductCode = new DataSet();
        //    try
        //    {
        //        //string ColumnProductCodeCache = "listSlabProductCode" ;
        //        //if (IndexusDistributionCache.SharedCache.Get(ColumnProductCodeCache) == null)
        //        //{
        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {
        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            // dsCarpetProductCode = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_CarpetProductCode_Get");
        //            dsCarpetProductCode = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.CarpetProductCode_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

        //            if (dsCarpetProductCode != null && dsCarpetProductCode.Tables.Count != 0)
        //            {
        //                foreach (DataRowView drvBeam in dsCarpetProductCode.Tables[0].DefaultView)
        //                {
        //                    ProductCode productCode = new ProductCode
        //                    {
        //                        ProductCodeId = Convert.ToInt32(drvBeam["INTPRODUCTCODEID"]),
        //                        ProductCodeName = drvBeam["VCHPRODUCTCODE"].ToString(),
        //                        MainWireSpacing = Convert.ToInt32(drvBeam["INTMWSPACE"]),
        //                        CrossWireSpacing = Convert.ToInt32(drvBeam["INTCWSPACE"]),
        //                        MainWireDia = Convert.ToInt32(drvBeam["DECMWDIAMETER"]),
        //                        CwDia = Convert.ToDouble(drvBeam["DECCWDIAMETER"]),
        //                        WeightArea = Convert.ToDouble(drvBeam["DECWEIGHTAREA"]),
        //                        WeightPerMeterRun = Convert.ToDouble(drvBeam["DECWEIGTHPERMETERRUN"]),
        //                        CwWeightPerMeterRun = Convert.ToDouble(drvBeam["DECCWWEIGTHPERMETERRUN"]),
        //                        DECMWLength = Convert.ToDouble(drvBeam["DECMWLENGTH"]),
        //                        // CARPET
        //                        //For Core Cage
        //                        StructureElementTypeId = Convert.ToInt32(drvBeam["INTSTRUCTUREELEMENTTYPEID"])

        //                    };

        //                    listCarpetProductCode.Add(productCode);
        //                }
        //                //    IndexusDistributionCache.SharedCache.Add(ColumnProductCodeCache, listSlabProductCode, DateTime.Today.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings.Get("cacheTimeOut"))));
        //                //}
        //                //else
        //                //{
        //                //    listSlabProductCode = IndexusDistributionCache.SharedCache.Get(ColumnProductCodeCache) as List<ProductCode>;
        //                //}
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return listCarpetProductCode;
        //}

        //public List<ProductCode> CarpetProductCodeFilter(string enteredText)
        //{

        //    List<ProductCode> listCarpetProductCode = new List<ProductCode> { };
        //    DataSet dsCarpetProductCode = new DataSet();
        //    try
        //    {
        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {
        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            dynamicParameters.Add("@VCHENTEREDTEXT", enteredText);
        //            // dsCarpetProductCode = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_CarpetProductCodeFilter_Get");
        //            dsCarpetProductCode = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.CarpetProductCodeFilter_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

        //            if (dsCarpetProductCode != null && dsCarpetProductCode.Tables.Count != 0)
        //            {
        //                foreach (DataRowView drvBeam in dsCarpetProductCode.Tables[0].DefaultView)
        //                {
        //                    ProductCode productCode = new ProductCode
        //                    {
        //                        ProductCodeId = Convert.ToInt32(drvBeam["INTPRODUCTCODEID"]),
        //                        ProductCodeName = drvBeam["VCHPRODUCTCODE"].ToString(),
        //                        MainWireSpacing = Convert.ToInt32(drvBeam["INTMWSPACE"]),
        //                        CrossWireSpacing = Convert.ToInt32(drvBeam["INTCWSPACE"]),
        //                        MainWireDia = Convert.ToInt32(drvBeam["DECMWDIAMETER"]),
        //                        CwDia = Convert.ToDouble(drvBeam["DECCWDIAMETER"]),
        //                        WeightArea = Convert.ToDouble(drvBeam["DECWEIGHTAREA"]),
        //                        WeightPerMeterRun = Convert.ToDouble(drvBeam["DECWEIGTHPERMETERRUN"]),
        //                        CwWeightPerMeterRun = Convert.ToDouble(drvBeam["DECCWWEIGTHPERMETERRUN"]),
        //                        DECMWLength = Convert.ToDouble(drvBeam["DECMWLENGTH"])
        //                        // CARPET

        //                    };
        //                    listCarpetProductCode.Add(productCode);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return listCarpetProductCode;
        //}
        //# endregion

        //# region "Product Code for Capping"
        //public List<ProductCode> CapProductCode_Get(string enteredText)
        //{

        //    List<ProductCode> listCapProductCode = new List<ProductCode> { };
        //    DataSet dsCapProductCode = new DataSet();
        //    try
        //    {
        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {
        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            dynamicParameters.Add("@VCHENTEREDTEXT", enteredText);
        //            //dsCapProductCode = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_CapProductCode_Cache_Get");
        //            dsCapProductCode = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.CapProductCode_Cache_Get, dynamicParameters, commandType: CommandType.StoredProcedure);


        //            if (dsCapProductCode != null && dsCapProductCode.Tables.Count != 0)
        //            {
        //                foreach (DataRowView drvBeam in dsCapProductCode.Tables[0].DefaultView)
        //                {
        //                    ProductCode productCode = new ProductCode
        //                    {
        //                        ProductCodeId = Convert.ToInt32(drvBeam["INTPRODUCTCODEID"]),
        //                        ProductCodeName = drvBeam["VCHPRODUCTCODE"].ToString()
        //                    };
        //                    listCapProductCode.Add(productCode);
        //                }
        //            }
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return listCapProductCode;

        //}

        public List<ProductCode> CapProductCodeNoFilter_Get()
        {

            List<ProductCode> listCapProductCode = new List<ProductCode> { };
            DataSet dsCapProductCode = new DataSet();
            try
            {

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();

                    //dsCapProductCode = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_CapProductCodeNoFilter_Cache_Get");
                    dsCapProductCode = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.usp_CapProductCodeNoFilter_Cache_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (dsCapProductCode != null && dsCapProductCode.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBeam in dsCapProductCode.Tables[0].DefaultView)
                        {
                            ProductCode productCode = new ProductCode
                            {
                                ProductCodeId = Convert.ToInt32(drvBeam["INTPRODUCTCODEID"]),
                                ProductCodeName = drvBeam["VCHPRODUCTCODE"].ToString()
                            };
                            listCapProductCode.Add(productCode);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return listCapProductCode;

        }

        //public List<ProductCode> ClinkProductCode_Get(string enteredText)
        //{

        //    List<ProductCode> listClinkProductCode = new List<ProductCode> { };
        //    DataSet dsClinkProductCode = new DataSet();
        //    try
        //    {
        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {
        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            dynamicParameters.Add("@VCHENTEREDTEXT", enteredText);
        //            //dsClinkProductCode = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_ClinkProductCode_Get");
        //            dsClinkProductCode = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.ClinkProductCode_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

        //            if (dsClinkProductCode != null && dsClinkProductCode.Tables.Count != 0)
        //            {
        //                foreach (DataRowView drvBeam in dsClinkProductCode.Tables[0].DefaultView)
        //                {
        //                    ProductCode productCode = new ProductCode
        //                    {
        //                        ProductCodeId = Convert.ToInt32(drvBeam["INTPRODUCTCODEID"]),
        //                        ProductCodeName = drvBeam["VCHPRODUCTCODE"].ToString()
        //                    };
        //                    listClinkProductCode.Add(productCode);
        //                }
        //            }
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return listClinkProductCode;

        //}

        //public List<ProductCode> CLinkProductCodeNoFilter_Get()
        //{

        //    List<ProductCode> listCLinkProductCode = new List<ProductCode> { };
        //    DataSet dsCLinkProductCode = new DataSet();
        //    try
        //    {
        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {
        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            // dsCLinkProductCode = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_ClinkProductCodeNoFilter_Get");
        //            dsCLinkProductCode = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.ClinkProductCodeNoFilter_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

        //            if (dsCLinkProductCode != null && dsCLinkProductCode.Tables.Count != 0)
        //            {
        //                foreach (DataRowView drvBeam in dsCLinkProductCode.Tables[0].DefaultView)
        //                {
        //                    ProductCode productCode = new ProductCode
        //                    {
        //                        ProductCodeId = Convert.ToInt32(drvBeam["INTPRODUCTCODEID"]),
        //                        ProductCodeName = drvBeam["VCHPRODUCTCODE"].ToString()
        //                    };
        //                    listCLinkProductCode.Add(productCode);
        //                }
        //            }
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return listCLinkProductCode;

        //}

       // #endregion


    }
}
