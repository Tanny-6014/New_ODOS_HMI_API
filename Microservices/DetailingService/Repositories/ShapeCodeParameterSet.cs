
using Dapper;
using DetailingService.Constants;
using DetailingService.Context;
using DetailingService.Dtos;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DetailingService.Repositories
{
    public class ShapeCodeParameterSet
    {
        //private string connectionString = "Server=nsprddb10\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";
        private string connectionString = "Server=NSQADB5\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=MES_USER;Password=Mes@123;Connection Timeout=36000000";


        public int ParameterSetValue { get; set; }
        public int ParameterSetNumber { get; set; }
        public int TransportModeId { get; set; }
        public string TransportMode { get; set; }
        public string TransportModeDesc { get; set; }
        public int Gap1 { get; set; }
        public int Gap2 { get; set; }
        public int TopCover { get; set; }
        public int BottomCover { get; set; }
        public int LeftCover { get; set; }
        public int RightCover { get; set; }
        public int Hook { get; set; }
        public int Leg { get; set; }
        public string Description { get; set; }
        public int? MaxMWLength { get; set; }
        public int? MaxCWLength { get; set; }
        public int MinMWLength { get; set; }
        public int MinCWLength { get; set; }
        public int MachineMaxMWLength { get; set; }
        public int MachineMaxCWLength { get; set; }
        public int MWLap { get; set; }
        public int CWLap { get; set; }
        public ProductCode productCode { get; set; }
        public int TransportMaxLength { get; set; }
        public int TransportMaxWidth { get; set; }
        public int TransportMaxHeight { get; set; }
        public int TransportMaxWeight { get; set; }
        public bool ISStandard { get; set; }
        public int? MinMo1 { get; set; }
        public int? MinMo2 { get; set; }
        public int? MinCo1 { get; set; }
        public int? MinCo2 { get; set; }
        public int StructureElementType { get; set; }


        private DetailingApplicationContext _dbContext;
        private readonly IConfiguration _configuration;
        //private string connectionString;

        public ShapeCodeParameterSet()
        {

        }
        public ShapeCodeParameterSet(DetailingApplicationContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;

            connectionString = _configuration.GetConnectionString(SystemConstant.DefaultDBConnection);
        }

        public List<ShapeCodeParameterSet> ParameterSetByProjectID_Get(int GroupMarkId)
        {
           // DBManager dbManager = new DBManager();
            List<ShapeCodeParameterSet> listParameterSet = new List<ShapeCodeParameterSet> { };

            DataSet dsParameterSetByProjectID = new DataSet();
            //ParameterSet Cache based on the GroupMarkId
            string strGroupMarkCache = "CacheShapeCodeParameterSet" + GroupMarkId.ToString();
            try
            {
                //if (IndexusDistributionCache.SharedCache.Get(strGroupMarkCache) == null)
                //{//Hardcoded value//
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTGROUPMARKID", GroupMarkId);
                    // dsParameterSetByProjectID = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_ParameterSetByGroupMarkID_Get");
                    dsParameterSetByProjectID = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.ParameterSetByGroupMarkID_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (dsParameterSetByProjectID != null && dsParameterSetByProjectID.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBeam in dsParameterSetByProjectID.Tables[0].DefaultView)
                        {

                            if (drvBeam["CHRSTANDARDCP"].ToString().Trim().ToUpper() == "Y")
                            {
                                ISStandard = true;
                            }
                            else
                            {
                                ISStandard = false;
                            }

                            ShapeCodeParameterSet parameterSet = new ShapeCodeParameterSet
                            {
                                ParameterSetValue = Convert.ToInt32(drvBeam["INTPARAMETESET"]),
                                ParameterSetNumber = Convert.ToInt32(drvBeam["TNTPARAMSETNUMBER"]),
                                TransportModeId = Convert.ToInt32(drvBeam["TNTTRANSPORTMODEID"]),
                                TransportMode = Convert.ToString(drvBeam["VCHTRANSPORTMODE"]),
                                TransportModeDesc = Convert.ToString(drvBeam["VCHTRANSPORTDESCRIPTION"]),
                                Gap1 = Convert.ToInt32(drvBeam["SITGAP1"]),
                                Gap2 = Convert.ToInt32(drvBeam["SITGAP2"]),
                                TopCover = Convert.ToInt32(drvBeam["SITTOPCOVER"]),
                                BottomCover = Convert.ToInt32(drvBeam["SITBOTTOMCOVER"]),
                                LeftCover = Convert.ToInt32(drvBeam["SITLEFTCOVER"]),
                                RightCover = Convert.ToInt32(drvBeam["SITRIGHTCOVER"]),
                                Hook = Convert.ToInt32(drvBeam["SITHOOK"]),
                                Leg = Convert.ToInt32(drvBeam["SITLEG"]),
                                Description = "Top:" + drvBeam["SITTOPCOVER"].ToString() + "; Bot:" + drvBeam["SITBOTTOMCOVER"].ToString() + "; Left: " + drvBeam["SITLEFTCOVER"].ToString() + "; Right: " + drvBeam["SITRIGHTCOVER"].ToString() + ";Leg: " + drvBeam["SITLEG"].ToString() + ";Hook: " + drvBeam["SITHOOK"].ToString(),
                                ISStandard = ISStandard
                            };

                            listParameterSet.Add(parameterSet);


                        }
                        //IndexusDistributionCache.SharedCache.Add(strGroupMarkCache, listParameterSet, DateTime.Today.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings.Get("cacheTimeOut"))));

                        //}
                        //else
                        //{
                        //    dbManager.Open();
                        //    listParameterSet = IndexusDistributionCache.SharedCache.Get(strGroupMarkCache) as List<ShapeCodeParameterSet>;

                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
            return listParameterSet;

        }

        public List<ShapeCodeParameterSet> ParameterSetByProjectProductTypeId_Get(int ProjectId, int ProductTypeId)
        {
            
            List<ShapeCodeParameterSet> listParameterSet = new List<ShapeCodeParameterSet> { };

            DataSet dsParameterSetByProjectID = new DataSet();
            //ParameterSet Cache based on the GroupMarkId
            IEnumerable<BeamShapeCodeParameterSetDto> beamShapeCodeParameterSetDto;
          
            try
            {
                string strParamSet = "strParamSet" + ProjectId.ToString() + ProductTypeId.ToString();
                //if (IndexusDistributionCache.SharedCache.Get(strParamSet) == null)
                //{//Hardcoded value//
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTPROJECTID", ProjectId);
                    dynamicParameters.Add("@SITPRODUCTTYPEID", ProductTypeId);
                    //dsParameterSetByProjectID = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_ParameterSetByProjectProductTypeId_Get");
                    beamShapeCodeParameterSetDto =sqlConnection.Query<BeamShapeCodeParameterSetDto>(SystemConstant.ParameterSetByProjectProductTypeId_Get, dynamicParameters, commandType: CommandType.StoredProcedure);


                    if (beamShapeCodeParameterSetDto.Count() > 0)
                    {
                        DataTable dt = ConvertToDataTable.ToDataTable(beamShapeCodeParameterSetDto);
                        dsParameterSetByProjectID.Tables.Add(dt);
                    }

                    if (dsParameterSetByProjectID != null && dsParameterSetByProjectID.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBeam in dsParameterSetByProjectID.Tables[0].DefaultView)
                        {
                            if (drvBeam["CHRSTANDARDCP"].ToString().Trim().ToUpper() == "Y")
                            {
                                ISStandard = true;
                            }
                            else
                            {
                                ISStandard = false;
                            }
                            ShapeCodeParameterSet parameterSet = new ShapeCodeParameterSet
                            {
                                ParameterSetValue = Convert.ToInt32(drvBeam["INTPARAMETESET"]),
                                ParameterSetNumber = Convert.ToInt32(drvBeam["TNTPARAMSETNUMBER"]),
                                TransportModeId = Convert.ToInt32(drvBeam["TNTTRANSPORTMODEID"]),
                                TransportMode = Convert.ToString(drvBeam["VCHTRANSPORTMODE"]),
                                TransportModeDesc = Convert.ToString(drvBeam["VCHTRANSPORTDESCRIPTION"]),
                                Gap1 = Convert.ToInt32(drvBeam["SITGAP1"]),
                                Gap2 = Convert.ToInt32(drvBeam["SITGAP2"]),
                                TopCover = Convert.ToInt32(drvBeam["SITTOPCOVER"]),
                                BottomCover = Convert.ToInt32(drvBeam["SITBOTTOMCOVER"]),
                                LeftCover = Convert.ToInt32(drvBeam["SITLEFTCOVER"]),
                                RightCover = Convert.ToInt32(drvBeam["SITRIGHTCOVER"]),
                                Hook = Convert.ToInt32(drvBeam["SITHOOK"]),
                                Leg = Convert.ToInt32(drvBeam["SITLEG"]),
                                Description = "Top:" + drvBeam["SITTOPCOVER"].ToString() + "; Bot:" + drvBeam["SITBOTTOMCOVER"].ToString() + "; Left: " + drvBeam["SITLEFTCOVER"].ToString() + "; Right: " + drvBeam["SITRIGHTCOVER"].ToString() + ";Leg: " + drvBeam["SITLEG"].ToString() + ";Hook: " + drvBeam["SITHOOK"].ToString(),
                            };

                            listParameterSet.Add(parameterSet);


                        }
                        //    IndexusDistributionCache.SharedCache.Add(strParamSet, listParameterSet, DateTime.Today.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings.Get("cacheTimeOut"))));

                        //}
                        //else
                        //{
                        //    dbManager.Open();
                        //    listParameterSet = IndexusDistributionCache.SharedCache.Get(strParamSet) as List<ShapeCodeParameterSet>;

                        //}
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
            return listParameterSet;

        }

        public List<ShapeCodeParameterSet> ColumnParameterSetbyProjIdProdType(int ProjectId, int ProductTypeId)
        {
            //DBManager dbManager = new DBManager();
            List<ShapeCodeParameterSet> listColumnParameterSet = new List<ShapeCodeParameterSet> { };

            DataSet dsColumnParameterSetbyProjIdProdType = new DataSet();
            //ParameterSet Cache based on the GroupMarkId
            IEnumerable<ColumnShapeCodeParameterSetDto> columnShapeCodeParameterSetDto;

            try
            {

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTPROJECTID", ProjectId);
                    dynamicParameters.Add("@SITPRODUCTTYPEID", ProductTypeId);
                    //dsColumnParameterSetbyProjIdProdType = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_ParameterSetbyProjIdProdType_Get");
                    columnShapeCodeParameterSetDto = sqlConnection.Query<ColumnShapeCodeParameterSetDto>(SystemConstant.ParameterSetbyProjIdProdType_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                     //Vanita
                    if (columnShapeCodeParameterSetDto.Count() > 0)
                    {
                        DataTable dt = ConvertToDataTable.ToDataTable(columnShapeCodeParameterSetDto);
                        dsColumnParameterSetbyProjIdProdType.Tables.Add(dt);
                    }

                        if (dsColumnParameterSetbyProjIdProdType != null && dsColumnParameterSetbyProjIdProdType.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBeam in dsColumnParameterSetbyProjIdProdType.Tables[0].DefaultView)
                        {

                            ShapeCodeParameterSet parameterSet = new ShapeCodeParameterSet
                            {
                                ParameterSetValue = Convert.ToInt32(drvBeam["INTPARAMETESET"]),
                                ParameterSetNumber = Convert.ToInt32(drvBeam["TNTPARAMSETNUMBER"]),
                                TransportModeId = Convert.ToInt32(drvBeam["TNTTRANSPORTMODEID"]),
                                TransportMode = Convert.ToString(drvBeam["VCHTRANSPORTMODE"]),
                               TransportModeDesc = Convert.ToString(drvBeam["VCHTRANSPORTDESCRIPTION"]),
                                Gap1 = Convert.ToInt32(drvBeam["SITGAP1"]),
                                Gap2 = Convert.ToInt32(drvBeam["SITGAP2"]),
                                TopCover = Convert.ToInt32(drvBeam["SITTOPCOVER"]),
                                BottomCover = Convert.ToInt32(drvBeam["SITBOTTOMCOVER"]),
                                LeftCover = Convert.ToInt32(drvBeam["SITLEFTCOVER"]),
                                RightCover = Convert.ToInt32(drvBeam["SITRIGHTCOVER"]),
                                Hook = Convert.ToInt32(drvBeam["SITHOOK"]),
                                Leg = Convert.ToInt32(drvBeam["SITLEG"]),
                                Description = "Top:" + drvBeam["SITTOPCOVER"].ToString() + "; Bot:" + drvBeam["SITBOTTOMCOVER"].ToString() + "; Left: " + drvBeam["SITLEFTCOVER"].ToString() + "; Right: " + drvBeam["SITRIGHTCOVER"].ToString() + ";Leg: " + drvBeam["SITLEG"].ToString(),
                            };
                            listColumnParameterSet.Add(parameterSet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
            return listColumnParameterSet;

        }

        public List<ShapeCodeParameterSet> SlabParameterSetbyProjIdProdType(int ProjectId, int ProductTypeId)
        {

            List<ShapeCodeParameterSet> listSlabParameterSet = new List<ShapeCodeParameterSet> { };
            DataSet dsSlabParameterSetbyProjIdProdType = new DataSet();
            IEnumerable<ShapeCodeParameterSetDto> shapeCodeParameterSetDtos;

            //ParameterSet Cache based on the GroupMarkId

            try
            {

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTPROJECTID", ProjectId);
                    dynamicParameters.Add("@SITPRODUCTTYPEID", ProductTypeId);
                    //dsSlabParameterSetbyProjIdProdType = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_SlabParameterSetbyProjIdProdType_Get");
                    shapeCodeParameterSetDtos = sqlConnection.Query<ShapeCodeParameterSetDto>(SystemConstant.SlabParameterSetbyProjIdProdType_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                   
                    if (shapeCodeParameterSetDtos.Count() > 0)
                    {
                        DataTable dt = ConvertToDataTable.ToDataTable(shapeCodeParameterSetDtos);
                        dsSlabParameterSetbyProjIdProdType.Tables.Add(dt);
                    }

                    if (dsSlabParameterSetbyProjIdProdType != null && dsSlabParameterSetbyProjIdProdType.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBeam in dsSlabParameterSetbyProjIdProdType.Tables[0].DefaultView)
                        {
                            
                            ShapeCodeParameterSet parameterSet = new ShapeCodeParameterSet
                            {
                                ParameterSetValue = Convert.ToInt32(drvBeam["INTPARAMETESET"]),
                                ParameterSetNumber = Convert.ToInt32(drvBeam["TNTPARAMSETNUMBER"]),
                                TransportModeId = Convert.ToInt32(drvBeam["TNTTRANSPORTMODEID"]),
                                TransportMaxLength = Convert.ToInt32(drvBeam["TANSPORTMAXLENGTH"]),
                                TransportMaxWidth = Convert.ToInt32(drvBeam["TRANSPORTMAXWIDTH"]),
                                TransportMaxHeight = Convert.ToInt32(drvBeam["TRANSPORTMAXHEIGHT"]),
                                TransportMaxWeight = Convert.ToInt32(drvBeam["TRANSPORTMAXWEIGHT"]),
                                TransportMode = Convert.ToString(drvBeam["VCHTRANSPORTMODE"]),
                                TransportModeDesc = Convert.ToString(drvBeam["VCHTRANSPORTDESCRIPTION"]),
                                MaxMWLength = Convert.ToInt32(drvBeam["MAXMWLENGTH"]),
                                MaxCWLength = Convert.ToInt32(drvBeam["MAXCWLENGTH"]),
                                MachineMaxMWLength = Convert.ToInt32(drvBeam["MACHINEMAXMWLENGTH"]),
                                MachineMaxCWLength = Convert.ToInt32(drvBeam["MACHINEMAXCWLENGTH"]),
                                Description = Convert.ToString(drvBeam["VCHDESCRIPTION"]),
                                MinMo1 = Convert.ToInt32(drvBeam["MINMO1"]),
                                MinMo2 = Convert.ToInt32(drvBeam["MINMO2"]),
                                MinCo1 = Convert.ToInt32(drvBeam["MINCO1"]),
                                MinCo2 = Convert.ToInt32(drvBeam["MINCO2"]),
                                MinMWLength = Convert.ToInt32(drvBeam["MINMWLENGTH"]),
                                MinCWLength = Convert.ToInt32(drvBeam["MINCWLENGTH"])
                            };
                            listSlabParameterSet.Add(parameterSet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
            return listSlabParameterSet;

        }

        // CARPET anuran
        public List<ShapeCodeParameterSet> CarpetParameterSetbyProjIdProdType(int ProjectId, int ProductTypeId)
        {
          
            List<ShapeCodeParameterSet> listCarpetParameterSet = new List<ShapeCodeParameterSet> { };

            DataSet dsCarpetParameterSetbyProjIdProdType = new DataSet();
            IEnumerable<ShapeCodeParameterSetDto> shapeCodeParameterSetDtos;

            try
            {

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                   
                    SqlCommand cmd = new SqlCommand(SystemConstant.SlabParameterSetbyProjIdProdType_Get_CAR, sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@INTPROJECTID", ProjectId));
                    cmd.Parameters.Add(new SqlParameter("@SITPRODUCTTYPEID", ProductTypeId));

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dsCarpetParameterSetbyProjIdProdType);


                    if (dsCarpetParameterSetbyProjIdProdType != null && dsCarpetParameterSetbyProjIdProdType.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBeam in dsCarpetParameterSetbyProjIdProdType.Tables[0].DefaultView)
                        {

                            ShapeCodeParameterSet parameterSet = new ShapeCodeParameterSet
                            {
                                ParameterSetNumber = Convert.ToInt32(drvBeam["TNTPARAMSETNUMBER"]),
                                ParameterSetValue = Convert.ToInt32(drvBeam["INTPARAMETESET"]),
                                Description = Convert.ToString(drvBeam["VCHDESCRIPTION"]),
                                TransportModeId = Convert.ToInt32(drvBeam["TNTTRANSPORTMODEID"]),
                                TransportMode = Convert.ToString(drvBeam["VCHTRANSPORTMODE"]),
                                TransportModeDesc = Convert.ToString(drvBeam["VCHTRANSPORTDESCRIPTION"]),
                                MaxMWLength = Convert.ToInt32(drvBeam["MAXMWLENGTH"]),
                                MaxCWLength = Convert.ToInt32(drvBeam["MAXCWLENGTH"]),
                                MachineMaxMWLength = Convert.ToInt32(drvBeam["MACHINEMAXMWLENGTH"]),
                                MachineMaxCWLength = Convert.ToInt32(drvBeam["MACHINEMAXCWLENGTH"]),
                                MinMWLength = Convert.ToInt32(drvBeam["MINMWLENGTH"]),
                                MinCWLength = Convert.ToInt32(drvBeam["MINCWLENGTH"]),
                                TransportMaxLength = Convert.ToInt32(drvBeam["TANSPORTMAXLENGTH"]),
                                TransportMaxWidth = Convert.ToInt32(drvBeam["TRANSPORTMAXWIDTH"]),
                                TransportMaxHeight = Convert.ToInt32(drvBeam["TRANSPORTMAXHEIGHT"]),
                                TransportMaxWeight = Convert.ToInt32(drvBeam["TRANSPORTMAXWEIGHT"]),

                                MinMo1 = Convert.ToInt32(drvBeam["MINMO1"]),
                                MinMo2 = Convert.ToInt32(drvBeam["MINMO2"]),
                                MinCo1 = Convert.ToInt32(drvBeam["MINCO1"]),
                                MinCo2 = Convert.ToInt32(drvBeam["MINCO2"])

                            };
                            listCarpetParameterSet.Add(parameterSet);
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listCarpetParameterSet;
            //return shapeCodeParameterSetDtos.ToList();

        }

        public List<ShapeCodeParameterSet> ProjectParamLap_Get(int productCodeId,int tntparametersetnumber)
        {
            //DBManager dbManager = new DBManager();
            List<ShapeCodeParameterSet> listProjectParameterSet = new List<ShapeCodeParameterSet> { };
            DataSet dsrojectParameterSet = new DataSet();
            IEnumerable<ProjectParameterSetDto> ProjectParameterSetList;
            try
            {
                string strParamSet = "strProjParamSet";
                // (IndexusDistributionCache.SharedCache.Get(strParamSet) == null)
                //{
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    // dsrojectParameterSet = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_ProjectParamLap_Get");
                    //ProjectParameterSetList = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.ProjectParamLap_Get, commandType: CommandType.StoredProcedure);
                    dynamicParameters.Add("@INTPRODUCTCODEID", productCodeId);
                    dynamicParameters.Add("@TNTPARAMSETNUMBER", tntparametersetnumber);
                    ProjectParameterSetList = sqlConnection.Query<ProjectParameterSetDto>(SystemConstant.ProjectParamLap_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (ProjectParameterSetList.Count() > 0)
                    {
                        DataTable dt = ConvertToDataTable.ToDataTable(ProjectParameterSetList);
                        dsrojectParameterSet.Tables.Add(dt);

                    }
                    if (dsrojectParameterSet != null && dsrojectParameterSet.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBeam in dsrojectParameterSet.Tables[0].DefaultView)
                        {
                            ProductCode ObjProductCode = new ProductCode();
                            ObjProductCode.ProductCodeId = Convert.ToInt32(drvBeam["INTPRODUCTCODEID"]);
                            ObjProductCode.ProductCodeName = Convert.ToString(drvBeam["VCHPRODUCTCODE"]);
                            ShapeCodeParameterSet parameterSet = new ShapeCodeParameterSet
                            {
                                ParameterSetNumber = Convert.ToInt32(drvBeam["TNTPARAMSETNUMBER"]),
                                MWLap = Convert.ToInt32(drvBeam["SITMWLAP"]),
                                CWLap = Convert.ToInt32(drvBeam["SITCWLAP"]),
                                productCode = ObjProductCode,
                                StructureElementType = Convert.ToInt32(drvBeam["INTSTRUCTUREELEMENTTYPEID"])
                            };
                            listProjectParameterSet.Add(parameterSet);
                        }
                    }
                    //}
                    //else
                    //{
                    //    listProjectParameterSet = IndexusDistributionCache.SharedCache.Get(strParamSet) as List<ShapeCodeParameterSet>;

                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return listProjectParameterSet;

        }
    }
}
