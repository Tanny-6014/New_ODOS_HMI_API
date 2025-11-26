using Dapper;
using SAP_API.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

//using SharedCache.WinServiceCommon.Provider.Cache;


namespace WBSService.Repositories
{
    public class ShapeCode
    {
        private string connectionString = "Server=NSQADB5\\MSSQL2022.natsteel.corp;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=MES_USER;Password=Mes@123";

        #region Variables

        public int ShapeID { get; set; }
        public string ShapeCodeName { get; set; }
        //public ShapeParameterCollection ShapeParam { get; set; }
        public string MeshShapeGroup { get; set; }
        public bool BendIndicator { get; set; }
        public bool CreepDeductAtMO1 { get; set; }
        public bool CreepDeductAtCO1 { get; set; }
        public string MOCO { get; set; }
        public bool ShapePopUp { get; set; }
        public bool IsCapping { get; set; }
        public int NoOfBends { get; set; }
        public int MWBendPosition { get; set; }
        public int CWBendPosition { get; set; }
        public int NoOfMWBend { get; set; }
        public int NoOfCWBend { get; set; }
        public string MWbvbsString { get; set; }
        public string CWbvbsString { get; set; }
        //public List<ShapeParameter> ShapeParameterList { get; set; }
        #endregion

        public ShapeCode()
        {
            //ShapeID = 9;

        }


        //#region "Shape Code for Beam"
        //public List<ShapeCode> ShapeCodeForBeam_Get()
        //{
        //    //DBManager dbManager = new DBManager();
        //    List<ShapeCode> listShapeCodeForBeam = new List<ShapeCode> { };
        //    DataSet dsShapeCodeForBeam = new DataSet();
        //    IEnumerable<BeamShapeCodeDto> beamShapeCodeDto;
        //    try
        //    {
        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {
        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();

        //            //dsShapeCodeForBeam = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_ShapeCodeForBeam_Get");

        //            beamShapeCodeDto = sqlConnection.Query<BeamShapeCodeDto>(SystemConstant.ShapeCodeForBeam_Get, dynamicParameters, commandType: CommandType.StoredProcedure);


        //            if (beamShapeCodeDto.Count() > 0)
        //            {
        //                DataTable dt = ConvertToDataTable.ToDataTable(beamShapeCodeDto);
        //                dsShapeCodeForBeam.Tables.Add(dt);
        //            }

        //            ShapeParameterCollection shapeParameterCollection = new ShapeParameterCollection();
        //            ShapeParameter listShapeParameterForBeam = new ShapeParameter { };
        //            // if (IndexusDistributionCache.SharedCache.Get("ShapeparamCache") == null)
        //            // {
        //            shapeParameterCollection = shapeParameterCollection.ShapeParameterForBeam_Get();
        //            //}
        //            //else
        //            //{
        //            // shapeParameterCollection = (ShapeParameterCollection)IndexusDistributionCache.SharedCache.Get("ShapeparamCache");
        //            // }


        //            if (dsShapeCodeForBeam != null && dsShapeCodeForBeam.Tables.Count != 0)
        //            {
        //                foreach (DataRowView drvBeam in dsShapeCodeForBeam.Tables[0].DefaultView)
        //                {
        //                    ShapeParameterCollection strucParamCollFilter = new ShapeParameterCollection();
        //                    foreach (ShapeParameter shapeParamCollection in shapeParameterCollection)
        //                    {
        //                        if (shapeParamCollection.ShapeId == Convert.ToInt32(drvBeam["INTSHAPEID"]))
        //                        {
        //                            strucParamCollFilter.Add(shapeParamCollection);
        //                        }
        //                    }

        //                    ShapeCode shapecode = new ShapeCode
        //                    {
        //                        ShapeID = Convert.ToInt32(drvBeam["INTSHAPEID"]),
        //                        ShapeCodeName = drvBeam["VCHSHAPECODE"].ToString(),
        //                        ShapeParam = strucParamCollFilter,
        //                        ShapePopUp = Convert.ToBoolean(drvBeam["BITSHAPEPOPUP"]),
        //                        IsCapping = Convert.ToBoolean(drvBeam["BITISCAPPING"]),
        //                        NoOfBends = Convert.ToInt32(drvBeam["NOOFBENDS"]),
        //                        MWBendPosition = Convert.ToInt32(drvBeam["INTMWBENDPOSITION"]),
        //                        CWBendPosition = Convert.ToInt32(drvBeam["INTCWBENDPOSITION"]),
        //                        NoOfMWBend = Convert.ToInt32(drvBeam["INTNOOFMWBEND"]),
        //                        NoOfCWBend = Convert.ToInt32(drvBeam["INTNOOFCWBEND"]),
        //                        MWbvbsString = Convert.ToString(drvBeam["VCHMWBVBSTEMPLATE"]),
        //                        CWbvbsString = Convert.ToString(drvBeam["VCHCWBVBSTEMPLATE"])
        //                    };
        //                    listShapeCodeForBeam.Add(shapecode);

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return listShapeCodeForBeam;

        //}

        //public List<ShapeCode> FilterShapeCodeForBeam_Get(string enteredText)
        //{

        //    List<ShapeCode> listShapeCodeForBeam = new List<ShapeCode> { };

        //    DataSet dsShapeCodeForBeam = new DataSet();

        //    try
        //    {

        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {
        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            dynamicParameters.Add("@CHRSHAPETEXT", enteredText);
        //            // dsShapeCodeForBeam = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_FilterShapeCodeForBeam_Get");
        //            dsShapeCodeForBeam = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.FilterShapeCodeForBeam_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

        //            ShapeParameterCollection shapeParameterCollection = new ShapeParameterCollection();
        //            ShapeParameter listShapeParameterForBeam = new ShapeParameter { };
        //            //if (IndexusDistributionCache.SharedCache.Get("ShapeparamCache") == null)
        //            //{
        //            shapeParameterCollection = shapeParameterCollection.ShapeParameterForBeam_Get();

        //            //}
        //            //else
        //            //{
        //            // shapeParameterCollection = (ShapeParameterCollection)IndexusDistributionCache.SharedCache.Get("ShapeparamCache");
        //            //}


        //            if (dsShapeCodeForBeam != null && dsShapeCodeForBeam.Tables.Count != 0)
        //            {
        //                foreach (DataRowView drvBeam in dsShapeCodeForBeam.Tables[0].DefaultView)
        //                {
        //                    ShapeParameterCollection strucParamCollFilter = new ShapeParameterCollection();
        //                    foreach (ShapeParameter shapeParamCollection in shapeParameterCollection)
        //                    {
        //                        if (shapeParamCollection.ShapeId == Convert.ToInt32(drvBeam["INTSHAPEID"]))
        //                        {

        //                            strucParamCollFilter.Add(shapeParamCollection);
        //                        }
        //                    }

        //                    ShapeCode shapecode = new ShapeCode
        //                    {
        //                        ShapeID = Convert.ToInt32(drvBeam["INTSHAPEID"]),
        //                        ShapeCodeName = drvBeam["VCHSHAPECODE"].ToString(),
        //                        ShapeParam = strucParamCollFilter,
        //                        ShapePopUp = Convert.ToBoolean(drvBeam["BITSHAPEPOPUP"]),
        //                        IsCapping = Convert.ToBoolean(drvBeam["BITISCAPPING"]),
        //                        NoOfBends = Convert.ToInt32(drvBeam["NOOFBENDS"]),
        //                        MWBendPosition = Convert.ToInt32(drvBeam["INTMWBENDPOSITION"]),
        //                        CWBendPosition = Convert.ToInt32(drvBeam["INTCWBENDPOSITION"]),
        //                        NoOfMWBend = Convert.ToInt32(drvBeam["INTNOOFMWBEND"]),
        //                        NoOfCWBend = Convert.ToInt32(drvBeam["INTNOOFCWBEND"]),
        //                        MWbvbsString = Convert.ToString(drvBeam["VCHMWBVBSTEMPLATE"]),
        //                        CWbvbsString = Convert.ToString(drvBeam["VCHCWBVBSTEMPLATE"])
        //                    };
        //                    listShapeCodeForBeam.Add(shapecode);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return listShapeCodeForBeam;

        //}
        //# endregion        

        //# region "Shape Code for Column"
        //public List<ShapeCode> ColumnShapeCode_Get(string enteredText)
        //{

        //    List<ShapeCode> listColumnShapeCode = new List<ShapeCode> { };

        //    DataSet dsColumnShapeCode = new DataSet();

        //    try
        //    {

        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {
        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            dynamicParameters.Add("@CHRSHAPETEXT", enteredText);
        //            //dsColumnShapeCode = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_ColumnShapeCode_Get");
        //            dsColumnShapeCode = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.ColumnShapeCode_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

        //            ShapeParameterCollection shapeParameterCollection = new ShapeParameterCollection();

        //            //if (IndexusDistributionCache.SharedCache.Get("ColumnShapeparamCache") == null)
        //            // {
        //            shapeParameterCollection = shapeParameterCollection.ColumnShapeParameter_Get();

        //            //}
        //            //else
        //            //{
        //            //    shapeParameterCollection = (ShapeParameterCollection)IndexusDistributionCache.SharedCache.Get("ColumnShapeparamCache");
        //            //}

        //            if (dsColumnShapeCode != null && dsColumnShapeCode.Tables.Count != 0)
        //            {

        //                foreach (DataRowView drvBeam in dsColumnShapeCode.Tables[0].DefaultView)
        //                {
        //                    ShapeParameterCollection strucParamCollFilter = new ShapeParameterCollection();
        //                    foreach (ShapeParameter shapeParamCollection in shapeParameterCollection)
        //                    {
        //                        if (shapeParamCollection.ShapeId == Convert.ToInt32(drvBeam["INTSHAPEID"]))
        //                        {

        //                            strucParamCollFilter.Add(shapeParamCollection);
        //                        }
        //                    }

        //                    ShapeCode shapecode = new ShapeCode
        //                    {
        //                        ShapeID = Convert.ToInt32(drvBeam["INTSHAPEID"]),
        //                        ShapeCodeName = drvBeam["VCHSHAPECODE"].ToString(),
        //                        ShapeParam = strucParamCollFilter,
        //                        ShapePopUp = Convert.ToBoolean(drvBeam["BITSHAPEPOPUP"]),
        //                        IsCapping = Convert.ToBoolean(drvBeam["BITISCAPPING"])
        //                    };
        //                    listColumnShapeCode.Add(shapecode);

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return listColumnShapeCode;

        //}

        //public List<ShapeCode> ColumnShapeCodeBind_Get()
        //{

        //    List<ShapeCode> listColumnShapeCode = new List<ShapeCode> { };

        //    DataSet dsColumnShapeCode = new DataSet();

        //    IEnumerable<ColumnShapeCodeBindDto> columnShapeCodeBindDto;

        //    try
        //    {

        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {
        //            sqlConnection.Open();
        //            // var dynamicParameters = new DynamicParameters();
        //            //dsColumnShapeCode = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_ColumnShapeCodeBind_Get");

        //            columnShapeCodeBindDto = sqlConnection.Query<ColumnShapeCodeBindDto>(SystemConstant.ColumnShapeCodeBind_Get, commandType: CommandType.StoredProcedure);

        //            if (columnShapeCodeBindDto.Count() > 0)
        //            {
        //                DataTable dt = ConvertToDataTable.ToDataTable(columnShapeCodeBindDto);
        //                dsColumnShapeCode.Tables.Add(dt);
        //            }
        //            ShapeParameterCollection shapeParameterCollection = new ShapeParameterCollection();

        //            // if (IndexusDistributionCache.SharedCache.Get("ColumnShapeparamCache") == null)
        //            //{
        //            shapeParameterCollection = shapeParameterCollection.ColumnShapeParameter_Get();

        //            // }
        //            //else
        //            //{
        //            //    shapeParameterCollection = (ShapeParameterCollection)IndexusDistributionCache.SharedCache.Get("ColumnShapeparamCache");
        //            //}

        //            if (dsColumnShapeCode != null && dsColumnShapeCode.Tables.Count != 0)
        //            {
        //                foreach (DataRowView drvBeam in dsColumnShapeCode.Tables[0].DefaultView)
        //                {
        //                    ShapeParameterCollection strucParamCollFilter = new ShapeParameterCollection();
        //                    foreach (ShapeParameter shapeParamCollection in shapeParameterCollection)
        //                    {
        //                        if (shapeParamCollection.ShapeId == Convert.ToInt32(drvBeam["INTSHAPEID"]))
        //                        {

        //                            strucParamCollFilter.Add(shapeParamCollection);
        //                        }
        //                    }

        //                    ShapeCode shapecode = new ShapeCode
        //                    {
        //                        ShapeID = Convert.ToInt32(drvBeam["INTSHAPEID"]),
        //                        ShapeCodeName = drvBeam["VCHSHAPECODE"].ToString(),
        //                        ShapeParam = strucParamCollFilter,
        //                        ShapePopUp = Convert.ToBoolean(drvBeam["BITSHAPEPOPUP"]),
        //                        IsCapping = Convert.ToBoolean(drvBeam["BITISCAPPING"]),
        //                        NoOfBends = Convert.ToInt32(drvBeam["NOOFBENDS"]),
        //                        MWBendPosition = Convert.ToInt32(drvBeam["INTMWBENDPOSITION"]),
        //                        CWBendPosition = Convert.ToInt32(drvBeam["INTCWBENDPOSITION"]),
        //                        NoOfMWBend = Convert.ToInt32(drvBeam["INTNOOFMWBEND"]),
        //                        NoOfCWBend = Convert.ToInt32(drvBeam["INTNOOFCWBEND"]),
        //                        MWbvbsString = Convert.ToString(drvBeam["VCHMWBVBSTEMPLATE"]),
        //                        CWbvbsString = Convert.ToString(drvBeam["VCHCWBVBSTEMPLATE"])
        //                    };
        //                    listColumnShapeCode.Add(shapecode);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return listColumnShapeCode;

        //}
        //# endregion

        //# region "Shape Code for Slab"
        //public List<ShapeCode> SlabShapeCode_Get()
        //{

        //    List<ShapeCode> listSlabShapeCode = new List<ShapeCode> { };
        //    IEnumerable<SlabShapeCodeDto> slabShapeCodeDtos;
        //    DataSet dsSlabShapeCode = new DataSet();

        //    try
        //    {

        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {
        //            sqlConnection.Open();

        //            slabShapeCodeDtos = sqlConnection.Query<SlabShapeCodeDto>(SystemConstant.SlabShapeCode_Get, commandType: CommandType.StoredProcedure);


        //            if (slabShapeCodeDtos.Count() > 0)
        //            {
        //                DataTable dt = ConvertToDataTable.ToDataTable(slabShapeCodeDtos);
        //                dsSlabShapeCode.Tables.Add(dt);

        //            }

        //            ShapeParameterCollection SlabshapeParameterCollection = new ShapeParameterCollection();
        //            SlabshapeParameterCollection = SlabshapeParameterCollection.SlabShapeParameter_Get();

        //            if (dsSlabShapeCode != null && dsSlabShapeCode.Tables.Count != 0)
        //            {
        //                foreach (DataRowView drvBeam in dsSlabShapeCode.Tables[0].DefaultView)
        //                {
        //                    ShapeParameterCollection strucParamCollFilter = new ShapeParameterCollection();

        //                    foreach (ShapeParameter shapeParamCollection in SlabshapeParameterCollection)
        //                    {
        //                        if (shapeParamCollection.ShapeId == Convert.ToInt32(drvBeam["INTSHAPEID"]))
        //                        {

        //                            strucParamCollFilter.Add(shapeParamCollection);
        //                        }
        //                    }

        //                    ShapeCode shapecode = new ShapeCode
        //                    {
        //                        ShapeID = Convert.ToInt32(drvBeam["INTSHAPEID"]),
        //                        MeshShapeGroup = drvBeam["VCHMESHSHAPEGROUP"].ToString(),
        //                        ShapeCodeName = drvBeam["VCHSHAPECODE"].ToString(),
        //                        ShapeParam = strucParamCollFilter,
        //                        BendIndicator = Convert.ToBoolean(drvBeam["BITBENDINDICATOR"]),
        //                        CreepDeductAtMO1 = Convert.ToBoolean(drvBeam["BITCREEPDEDUCTATMO1"]),
        //                        CreepDeductAtCO1 = Convert.ToBoolean(drvBeam["BITCREEPDEDUCTATCO1"]),
        //                        MOCO = drvBeam["CHRMOCO"].ToString(),
        //                        ShapePopUp = Convert.ToBoolean(drvBeam["BITSHAPEPOPUP"]),
        //                        NoOfBends = Convert.ToInt32(drvBeam["NOOFBENDS"]),
        //                        MWBendPosition = Convert.ToInt32(drvBeam["INTMWBENDPOSITION"]),
        //                        CWBendPosition = Convert.ToInt32(drvBeam["INTCWBENDPOSITION"]),
        //                        NoOfMWBend = Convert.ToInt32(drvBeam["INTNOOFMWBEND"]),
        //                        NoOfCWBend = Convert.ToInt32(drvBeam["INTNOOFCWBEND"]),
        //                        MWbvbsString = Convert.ToString(drvBeam["VCHMWBVBSTEMPLATE"]),
        //                        CWbvbsString = Convert.ToString(drvBeam["VCHCWBVBSTEMPLATE"])
        //                    };
        //                    listSlabShapeCode.Add(shapecode);

        //                }


        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return listSlabShapeCode;
        //}

        //public List<ShapeCode> SlabShapeCodeFilter_Get(string enteredText)
        //{

        //    List<ShapeCode> listSlabShapeCode = new List<ShapeCode> { };

        //    DataSet dsSlabShapeCode = new DataSet();

        //    try
        //    {

        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {
        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            dynamicParameters.Add("@VCHENTEREDTEXT", enteredText);
        //            //dsSlabShapeCode = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_SlabShapeCodeFilter_Get");
        //            dsSlabShapeCode = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.SlabShapeCodeFilter_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

        //            ShapeParameterCollection SlabshapeParameterCollection = new ShapeParameterCollection();
        //            //if (IndexusDistributionCache.SharedCache.Get("SlabShapeparamCache") == null)
        //            // {
        //            SlabshapeParameterCollection = SlabshapeParameterCollection.SlabShapeParameter_Get();

        //            //}
        //            //else
        //            //{
        //            //    SlabshapeParameterCollection = (ShapeParameterCollection)IndexusDistributionCache.SharedCache.Get("SlabShapeparamCache");
        //            //}


        //            if (dsSlabShapeCode != null && dsSlabShapeCode.Tables.Count != 0)
        //            {
        //                foreach (DataRowView drvBeam in dsSlabShapeCode.Tables[0].DefaultView)
        //                {
        //                    ShapeParameterCollection strucParamCollFilter = new ShapeParameterCollection();
        //                    foreach (ShapeParameter shapeParamCollection in SlabshapeParameterCollection)
        //                    {
        //                        if (shapeParamCollection.ShapeId == Convert.ToInt32(drvBeam["INTSHAPEID"]))
        //                        {

        //                            strucParamCollFilter.Add(shapeParamCollection);
        //                        }
        //                    }

        //                    ShapeCode shapecode = new ShapeCode
        //                    {
        //                        ShapeID = Convert.ToInt32(drvBeam["INTSHAPEID"]),
        //                        MeshShapeGroup = drvBeam["VCHMESHSHAPEGROUP"].ToString(),
        //                        ShapeCodeName = drvBeam["VCHSHAPECODE"].ToString(),
        //                        ShapeParam = strucParamCollFilter,
        //                        BendIndicator = Convert.ToBoolean(drvBeam["BITBENDINDICATOR"]),
        //                        CreepDeductAtMO1 = Convert.ToBoolean(drvBeam["BITCREEPDEDUCTATMO1"]),
        //                        CreepDeductAtCO1 = Convert.ToBoolean(drvBeam["BITCREEPDEDUCTATCO1"]),
        //                        MOCO = drvBeam["CHRMOCO"].ToString(),
        //                        ShapePopUp = Convert.ToBoolean(drvBeam["BITSHAPEPOPUP"]),
        //                        NoOfBends = Convert.ToInt32(drvBeam["NOOFBENDS"]),
        //                        MWBendPosition = Convert.ToInt32(drvBeam["INTMWBENDPOSITION"]),
        //                        CWBendPosition = Convert.ToInt32(drvBeam["INTCWBENDPOSITION"]),
        //                        NoOfMWBend = Convert.ToInt32(drvBeam["INTNOOFMWBEND"]),
        //                        NoOfCWBend = Convert.ToInt32(drvBeam["INTNOOFCWBEND"]),
        //                        MWbvbsString = Convert.ToString(drvBeam["VCHMWBVBSTEMPLATE"]),
        //                        CWbvbsString = Convert.ToString(drvBeam["VCHCWBVBSTEMPLATE"])

        //                    };
        //                    listSlabShapeCode.Add(shapecode);

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return listSlabShapeCode;

        //}
        //#endregion

        //#region "Shape Code for Carpet"
        //public List<ShapeCode> CarpetShapeCode_Get()
        //{

        //    List<ShapeCode> listCarpetShapeCode = new List<ShapeCode> { };

        //    DataSet dsCarpetShapeCode = new DataSet();

        //    try
        //    {

        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {
        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            // dsCarpetShapeCode = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_SlabShapeCode_Get");
        //            dsCarpetShapeCode = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.SlabShapeCode_Get, commandType: CommandType.StoredProcedure);

        //            ShapeParameterCollection CarpetshapeParameterCollection = new ShapeParameterCollection();
        //            // if (IndexusDistributionCache.SharedCache.Get("CarpetShapeparamCache") == null)
        //            // {
        //            CarpetshapeParameterCollection = CarpetshapeParameterCollection.CarpetShapeParameter_Get();

        //            //}
        //            //else
        //            //{
        //            //    CarpetshapeParameterCollection = (ShapeParameterCollection)IndexusDistributionCache.SharedCache.Get("CarpetShapeparamCache");
        //            //}


        //            if (dsCarpetShapeCode != null && dsCarpetShapeCode.Tables.Count != 0)
        //            {
        //                foreach (DataRowView drvBeam in dsCarpetShapeCode.Tables[0].DefaultView)
        //                {
        //                    ShapeParameterCollection strucParamCollFilter = new ShapeParameterCollection();
        //                    foreach (ShapeParameter shapeParamCollection in CarpetshapeParameterCollection)
        //                    {
        //                        if (shapeParamCollection.ShapeId == Convert.ToInt32(drvBeam["INTSHAPEID"]))
        //                        {

        //                            strucParamCollFilter.Add(shapeParamCollection);
        //                        }
        //                    }

        //                    ShapeCode shapecode = new ShapeCode
        //                    {
        //                        ShapeID = Convert.ToInt32(drvBeam["INTSHAPEID"]),
        //                        ShapeCodeName = drvBeam["VCHSHAPECODE"].ToString(),
        //                        ShapeParam = strucParamCollFilter,
        //                        BendIndicator = Convert.ToBoolean(drvBeam["BITBENDINDICATOR"]),
        //                        CreepDeductAtMO1 = Convert.ToBoolean(drvBeam["BITCREEPDEDUCTATMO1"]),
        //                        CreepDeductAtCO1 = Convert.ToBoolean(drvBeam["BITCREEPDEDUCTATCO1"]),
        //                        MOCO = drvBeam["CHRMOCO"].ToString(),
        //                        MeshShapeGroup = drvBeam["VCHMESHSHAPEGROUP"].ToString(),
        //                        ShapePopUp = Convert.ToBoolean(drvBeam["BITSHAPEPOPUP"]),
        //                        NoOfBends = Convert.ToInt32(drvBeam["NOOFBENDS"]),
        //                        MWBendPosition = Convert.ToInt32(drvBeam["INTMWBENDPOSITION"]),
        //                        CWBendPosition = Convert.ToInt32(drvBeam["INTCWBENDPOSITION"]),
        //                        NoOfMWBend = Convert.ToInt32(drvBeam["INTNOOFMWBEND"]),
        //                        NoOfCWBend = Convert.ToInt32(drvBeam["INTNOOFCWBEND"]),
        //                        MWbvbsString = Convert.ToString(drvBeam["VCHMWBVBSTEMPLATE"]),
        //                        CWbvbsString = Convert.ToString(drvBeam["VCHCWBVBSTEMPLATE"])
        //                    };
        //                    listCarpetShapeCode.Add(shapecode);

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return listCarpetShapeCode;
        //}

        //public List<ShapeCode> CarpetShapeCodeFilter_Get(string enteredText)
        //{

        //    List<ShapeCode> listCarpetShapeCode = new List<ShapeCode> { };

        //    DataSet dsCarpetShapeCode = new DataSet();

        //    try
        //    {

        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {
        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            dynamicParameters.Add("@VCHENTEREDTEXT", enteredText);
        //            // dsCarpetShapeCode = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_SlabShapeCodeFilter_Get");
        //            dsCarpetShapeCode = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.SlabShapeCodeFilter_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

        //            ShapeParameterCollection CarpetshapeParameterCollection = new ShapeParameterCollection();
        //            //if (IndexusDistributionCache.SharedCache.Get("CarpetShapeparamCache") == null)
        //            // {
        //            CarpetshapeParameterCollection = CarpetshapeParameterCollection.CarpetShapeParameter_Get();

        //            //}
        //            //else
        //            //{
        //            //    CarpetshapeParameterCollection = (ShapeParameterCollection)IndexusDistributionCache.SharedCache.Get("CarpetShapeparamCache");
        //            //}


        //            if (dsCarpetShapeCode != null && dsCarpetShapeCode.Tables.Count != 0)
        //            {
        //                foreach (DataRowView drvBeam in dsCarpetShapeCode.Tables[0].DefaultView)
        //                {
        //                    ShapeParameterCollection strucParamCollFilter = new ShapeParameterCollection();
        //                    foreach (ShapeParameter shapeParamCollection in CarpetshapeParameterCollection)
        //                    {
        //                        if (shapeParamCollection.ShapeId == Convert.ToInt32(drvBeam["INTSHAPEID"]))
        //                        {

        //                            strucParamCollFilter.Add(shapeParamCollection);
        //                        }
        //                    }

        //                    ShapeCode shapecode = new ShapeCode
        //                    {
        //                        ShapeID = Convert.ToInt32(drvBeam["INTSHAPEID"]),
        //                        ShapeCodeName = drvBeam["VCHSHAPECODE"].ToString(),
        //                        ShapeParam = strucParamCollFilter,
        //                        BendIndicator = Convert.ToBoolean(drvBeam["BITBENDINDICATOR"]),
        //                        CreepDeductAtMO1 = Convert.ToBoolean(drvBeam["BITCREEPDEDUCTATMO1"]),
        //                        CreepDeductAtCO1 = Convert.ToBoolean(drvBeam["BITCREEPDEDUCTATCO1"]),
        //                        MOCO = drvBeam["CHRMOCO"].ToString(),
        //                        MeshShapeGroup = drvBeam["VCHMESHSHAPEGROUP"].ToString(),
        //                        ShapePopUp = Convert.ToBoolean(drvBeam["BITSHAPEPOPUP"]),
        //                        NoOfBends = Convert.ToInt32(drvBeam["NOOFBENDS"]),
        //                        MWBendPosition = Convert.ToInt32(drvBeam["INTMWBENDPOSITION"]),
        //                        CWBendPosition = Convert.ToInt32(drvBeam["INTCWBENDPOSITION"]),
        //                        NoOfMWBend = Convert.ToInt32(drvBeam["INTNOOFMWBEND"]),
        //                        NoOfCWBend = Convert.ToInt32(drvBeam["INTNOOFCWBEND"]),
        //                        MWbvbsString = Convert.ToString(drvBeam["VCHMWBVBSTEMPLATE"]),
        //                        CWbvbsString = Convert.ToString(drvBeam["VCHCWBVBSTEMPLATE"])
        //                    };
        //                    listCarpetShapeCode.Add(shapecode);

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return listCarpetShapeCode;

        //}
        //#endregion

        //#region"Capping Shape Code"
        //public List<ShapeCode> CapShapeCode_Get(string enteredText)
        //{

        //    List<ShapeCode> listCapShapeCode = new List<ShapeCode> { };
        //    DataSet dsCapShapeCode = new DataSet();
        //    try
        //    {

        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {
        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            dynamicParameters.Add("@VCHENTEREDTEXT", enteredText);
        //            //  dsCapShapeCode = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_ShapeCode_Get");
        //            dsCapShapeCode = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.ShapeCode_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

        //            if (dsCapShapeCode != null && dsCapShapeCode.Tables.Count != 0)
        //            {
        //                foreach (DataRowView drvBeam in dsCapShapeCode.Tables[0].DefaultView)
        //                {
        //                    ShapeCode shapeCode = new ShapeCode
        //                    {
        //                        ShapeID = Convert.ToInt32(drvBeam["intShapeId"]),
        //                        ShapeCodeName = drvBeam["chrShapeCode"].ToString()
        //                    };
        //                    listCapShapeCode.Add(shapeCode);
        //                }
        //            }
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return listCapShapeCode;

        //}

        public List<ShapeCode> ShapeCodeNoFilter_Get()
        {

            List<ShapeCode> listCapShapeCode = new List<ShapeCode> { };
            DataSet dsCapShapeCode = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    //  dsCapShapeCode = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_ShapeCodeNoFilter_Get");
                    //dsCapShapeCode = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.usp_ShapeCodeNoFilter_Get, commandType: CommandType.StoredProcedure);
                    var dataAdapter = new SqlDataAdapter();

                    dataAdapter.SelectCommand = new SqlCommand(SystemConstant.usp_ShapeCodeNoFilter_Get, sqlConnection);

                    dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                    dataAdapter.SelectCommand.Parameters.AddRange(dynamicParameters.ParameterNames.Select(name => new SqlParameter(name, dynamicParameters.Get<object>(name))).ToArray());

                    dataAdapter.Fill(dsCapShapeCode);
                    if (dsCapShapeCode != null && dsCapShapeCode.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBeam in dsCapShapeCode.Tables[0].DefaultView)
                        {
                            ShapeCode shapeCode = new ShapeCode
                            {
                                ShapeID = Convert.ToInt32(drvBeam["intShapeId"]),
                                ShapeCodeName = drvBeam["chrShapeCode"].ToString()
                            };
                            listCapShapeCode.Add(shapeCode);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return listCapShapeCode;

        }


        //public List<ShapeCode> CLinkShapeCode_Get(string enteredText)
        //{

        //    List<ShapeCode> listCLinkShapeCode = new List<ShapeCode> { };
        //    DataSet dsCLinkShapeCode = new DataSet();
        //    try
        //    {
        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {
        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            dynamicParameters.Add("@VCHENTEREDTEXT", enteredText);
        //            //dsCLinkShapeCode = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_ShapeCode_Get_Clink");
        //            dsCLinkShapeCode = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.ShapeCode_Get_Clink, dynamicParameters, commandType: CommandType.StoredProcedure);

        //            if (dsCLinkShapeCode != null && dsCLinkShapeCode.Tables.Count != 0)
        //            {
        //                foreach (DataRowView drvBeam in dsCLinkShapeCode.Tables[0].DefaultView)
        //                {
        //                    ShapeCode shapeCode = new ShapeCode
        //                    {
        //                        ShapeID = Convert.ToInt32(drvBeam["intShapeId"]),
        //                        ShapeCodeName = drvBeam["vchShapeCode"].ToString()
        //                    };
        //                    listCLinkShapeCode.Add(shapeCode);
        //                }
        //            }
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return listCLinkShapeCode;

        //}

        //public List<ShapeCode> ClinkShapeCodeNoFilter_Get()
        //{

        //    List<ShapeCode> listCLinkShapeCode = new List<ShapeCode> { };
        //    DataSet dsCLinkShapeCode = new DataSet();
        //    try
        //    {

        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {
        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            //dsCLinkShapeCode = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_ShapeCodeNoFilter_Get_Clink");
        //            dsCLinkShapeCode = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.ShapeCodeNoFilter_Get_Clink, commandType: CommandType.StoredProcedure);

        //            if (dsCLinkShapeCode != null && dsCLinkShapeCode.Tables.Count != 0)
        //            {
        //                foreach (DataRowView drvBeam in dsCLinkShapeCode.Tables[0].DefaultView)
        //                {
        //                    ShapeCode shapeCode = new ShapeCode
        //                    {
        //                        ShapeID = Convert.ToInt32(drvBeam["intShapeId"]),
        //                        ShapeCodeName = drvBeam["vchShapeCode"].ToString()
        //                    };
        //                    listCLinkShapeCode.Add(shapeCode);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return listCLinkShapeCode;

        //}

        //#endregion

        //#region "Shape Code and shape parameter details for Irregular Spacing"

        //public List<ShapeCode> ShapeCodeDetailsforIrregularSpacing_Get(int ProductMarkId, string StructureElementType)
        //{
        //    try
        //    {

        //        List<ShapeCode> listShapeCode = new List<ShapeCode> { };
        //        DataSet dsShapeCode = new DataSet();
        //        //  IEnumerable<ShapeDetailIrregularSpacDto> detailIrregularSpacDtos;
        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {
        //            sqlConnection.Open();
        //            SqlCommand cmd = new SqlCommand(SystemConstant.ShapeCodeDetailsByProductMarkId_Get, sqlConnection);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.Add(new SqlParameter("@ProductMarkId", ProductMarkId));
        //            cmd.Parameters.Add(new SqlParameter("@StructureElementType", StructureElementType));

        //            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        //            adapter.Fill(dsShapeCode);


        //            //var dynamicParameters = new DynamicParameters();
        //            //dynamicParameters.Add("@ProductMarkId", ProductMarkId);
        //            //dynamicParameters.Add("@StructureElementType", StructureElementType);
        //            ////dsShapeCode = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_ShapeCodeDetailsByProductMarkId_Get");
        //            //detailIrregularSpacDtos = sqlConnection.Query<ShapeDetailIrregularSpacDto>(SystemConstant.ShapeCodeDetailsByProductMarkId_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

        //            //if (detailIrregularSpacDtos.Count() > 0)
        //            //{
        //            //    DataTable dt = ConvertToDataTable.ToDataTable(detailIrregularSpacDtos);
        //            //    dsShapeCode.Tables.Add(dt);
        //            //}
        //            if (dsShapeCode != null && dsShapeCode.Tables.Count != 0)
        //            {
        //                if (StructureElementType == "Beam")
        //                {
        //                    foreach (DataRowView drvShapeCode in dsShapeCode.Tables[0].DefaultView)
        //                    {
        //                        ShapeParameterList = new List<ShapeParameter>();
        //                        ShapeCode shapecode = new ShapeCode
        //                        {
        //                            ShapeID = Convert.ToInt32(drvShapeCode["INTSHAPEID"]),
        //                            ShapeCodeName = drvShapeCode["VCHSHAPECODE"].ToString(),
        //                            //ShapeParam = strucParamCollFilter,
        //                            ShapePopUp = Convert.ToBoolean(drvShapeCode["BITSHAPEPOPUP"]),
        //                            IsCapping = Convert.ToBoolean(drvShapeCode["BITISCAPPING"]),
        //                            NoOfBends = Convert.ToInt32(drvShapeCode["NOOFBENDS"]),
        //                            MWBendPosition = Convert.ToInt32(drvShapeCode["INTMWBENDPOSITION"]),
        //                            CWBendPosition = Convert.ToInt32(drvShapeCode["INTCWBENDPOSITION"]),
        //                            NoOfMWBend = Convert.ToInt32(drvShapeCode["INTNOOFMWBEND"]),
        //                            NoOfCWBend = Convert.ToInt32(drvShapeCode["INTNOOFCWBEND"]),
        //                            MWbvbsString = Convert.ToString(drvShapeCode["VCHMWBVBSTEMPLATE"]),
        //                            CWbvbsString = Convert.ToString(drvShapeCode["VCHCWBVBSTEMPLATE"])
        //                        };
        //                        listShapeCode.Add(shapecode);

        //                        foreach (DataRowView drvShapeParameter in dsShapeCode.Tables[1].DefaultView)
        //                        {
        //                            if ((!DBNull.Value.Equals(drvShapeParameter["INTSHAPEID"])) && (Convert.ToInt32(drvShapeParameter["INTSHAPEID"]) == shapecode.ShapeID))
        //                            {
        //                                ShapeParameter shapeparam = new ShapeParameter
        //                                {

        //                                    ShapeId = Convert.ToInt32(drvShapeParameter["INTSHAPEID"]),
        //                                    ParameterName = Convert.ToString(drvShapeParameter["CHRPARAMNAME"]),
        //                                    CriticalIndiacator = Convert.ToString(drvShapeParameter["CHRCRITICALIND"]),
        //                                    SequenceNumber = Convert.ToInt16(drvShapeParameter["INTPARAMSEQ"]),
        //                                    ParameterValueCab = "0",// Convert.ToInt32(drvShapeParameter["DEFAULTPARAMVALUE"])
        //                                    ShapeCodeImage = Convert.ToString(drvShapeParameter["CHRSHAPECODE"]),
        //                                    EditFlag = Convert.ToBoolean(drvShapeParameter["BITEDIT"]),
        //                                    VisibleFlag = Convert.ToBoolean(drvShapeParameter["BITVISIBLE"]),
        //                                    WireType = Convert.ToString(drvShapeParameter["CHRWIRETYPE"]),
        //                                    AngleType = Convert.ToString(drvShapeParameter["CHRANGLETYPE"]),
        //                                    AngleDir = Convert.ToInt32(drvShapeParameter["INTANGLEDIR"])
        //                                };
        //                                ShapeParameterList.Add(shapeparam);
        //                            }
        //                            shapecode.ShapeParameterList = ShapeParameterList;
        //                        }
        //                    }
        //                }
        //                else if (StructureElementType == "Column")
        //                {
        //                    foreach (DataRowView drvShapeCode in dsShapeCode.Tables[0].DefaultView)
        //                    {
        //                        ShapeParameterList = new List<ShapeParameter>();
        //                        ShapeCode shapecode = new ShapeCode
        //                        {
        //                            ShapeID = Convert.ToInt32(drvShapeCode["INTSHAPEID"]),
        //                            ShapeCodeName = drvShapeCode["VCHSHAPECODE"].ToString(),
        //                            ShapePopUp = Convert.ToBoolean(drvShapeCode["BITSHAPEPOPUP"]),
        //                            IsCapping = Convert.ToBoolean(drvShapeCode["BITISCAPPING"]),
        //                            NoOfBends = Convert.ToInt32(drvShapeCode["NOOFBENDS"]),
        //                            MWBendPosition = Convert.ToInt32(drvShapeCode["INTMWBENDPOSITION"]),
        //                            CWBendPosition = Convert.ToInt32(drvShapeCode["INTCWBENDPOSITION"]),
        //                            NoOfMWBend = Convert.ToInt32(drvShapeCode["INTNOOFMWBEND"]),
        //                            NoOfCWBend = Convert.ToInt32(drvShapeCode["INTNOOFCWBEND"]),
        //                            MWbvbsString = Convert.ToString(drvShapeCode["VCHMWBVBSTEMPLATE"]),
        //                            CWbvbsString = Convert.ToString(drvShapeCode["VCHCWBVBSTEMPLATE"])
        //                        };
        //                        listShapeCode.Add(shapecode);

        //                        foreach (DataRowView drvShapeParameter in dsShapeCode.Tables[1].DefaultView)
        //                        {
        //                            if ((!DBNull.Value.Equals(drvShapeParameter["INTSHAPEID"])) && (Convert.ToInt32(drvShapeParameter["INTSHAPEID"]) == shapecode.ShapeID))
        //                            {
        //                                ShapeParameter shapeparam = new ShapeParameter
        //                                {

        //                                    ShapeId = Convert.ToInt32(drvShapeParameter["INTSHAPEID"]),
        //                                    ParameterName = Convert.ToString(drvShapeParameter["CHRPARAMNAME"]),
        //                                    CriticalIndiacator = Convert.ToString(drvShapeParameter["CHRCRITICALIND"]),
        //                                    SequenceNumber = Convert.ToInt16(drvShapeParameter["INTPARAMSEQ"]),
        //                                    ParameterValueCab = "0",//Convert.ToInt32(drvShapeParameter["DEFAULTPARAMVALUE"]),
        //                                    ShapeCodeImage = Convert.ToString(drvShapeParameter["CHRSHAPECODE"]),
        //                                    WireType = Convert.ToString(drvShapeParameter["CHRWIRETYPE"]),
        //                                    AngleType = Convert.ToString(drvShapeParameter["CHRANGLETYPE"]),
        //                                    EditFlag = Convert.ToBoolean(drvShapeParameter["BITEDIT"]),
        //                                    VisibleFlag = Convert.ToBoolean(drvShapeParameter["BITVISIBLE"])
        //                                };
        //                                ShapeParameterList.Add(shapeparam);
        //                            }
        //                            shapecode.ShapeParameterList = ShapeParameterList;
        //                        }
        //                    }
        //                }
        //                else if (StructureElementType == "Drain")
        //                {
        //                    // Get Shape details for Drainage Module.
        //                }
        //                else
        //                {
        //                    foreach (DataRowView drvShapeCode in dsShapeCode.Tables[0].DefaultView)
        //                    {
        //                        ShapeParameterList = new List<ShapeParameter>();
        //                        ShapeCode shapecode = new ShapeCode
        //                        {
        //                            ShapeID = Convert.ToInt32(drvShapeCode["INTSHAPEID"]),
        //                            ShapeCodeName = drvShapeCode["VCHSHAPECODE"].ToString(),
        //                            BendIndicator = Convert.ToBoolean(drvShapeCode["BITBENDINDICATOR"]),
        //                            CreepDeductAtMO1 = Convert.ToBoolean(drvShapeCode["BITCREEPDEDUCTATMO1"]),
        //                            CreepDeductAtCO1 = Convert.ToBoolean(drvShapeCode["BITCREEPDEDUCTATCO1"]),
        //                            MOCO = drvShapeCode["CHRMOCO"].ToString(),
        //                            MeshShapeGroup = drvShapeCode["VCHMESHSHAPEGROUP"].ToString(),
        //                            ShapePopUp = Convert.ToBoolean(drvShapeCode["BITSHAPEPOPUP"]),
        //                            NoOfBends = Convert.ToInt32(drvShapeCode["NOOFBENDS"]),
        //                            MWBendPosition = Convert.ToInt32(drvShapeCode["INTMWBENDPOSITION"]),
        //                            CWBendPosition = Convert.ToInt32(drvShapeCode["INTCWBENDPOSITION"]),
        //                            NoOfMWBend = Convert.ToInt32(drvShapeCode["INTNOOFMWBEND"]),
        //                            NoOfCWBend = Convert.ToInt32(drvShapeCode["INTNOOFCWBEND"]),
        //                            MWbvbsString = Convert.ToString(drvShapeCode["VCHMWBVBSTEMPLATE"]),
        //                            CWbvbsString = Convert.ToString(drvShapeCode["VCHCWBVBSTEMPLATE"])
        //                        };
        //                        listShapeCode.Add(shapecode);

        //                        foreach (DataRowView drvShapeParameter in dsShapeCode.Tables[1].DefaultView)
        //                        {
        //                            if ((!DBNull.Value.Equals(drvShapeParameter["INTSHAPEID"])) && (Convert.ToInt32(drvShapeParameter["INTSHAPEID"]) == shapecode.ShapeID))
        //                            {
        //                                ShapeParameter shapeparam = new ShapeParameter
        //                                {

        //                                    ShapeId = Convert.ToInt32(drvShapeParameter["INTSHAPEID"]),
        //                                    ParameterName = Convert.ToString(drvShapeParameter["CHRPARAMNAME"]),
        //                                    CriticalIndiacator = Convert.ToString(drvShapeParameter["CHRCRITICALIND"]),
        //                                    SequenceNumber = Convert.ToInt16(drvShapeParameter["INTPARAMSEQ"]),
        //                                    ParameterValueCab = Convert.ToString(drvShapeParameter["DEFAULTPARAMVALUE"]),//To set default value for shape parameters 07/06/2012 --Surendar. S
        //                                    ShapeCodeImage = Convert.ToString(drvShapeParameter["CHRSHAPECODE"]),
        //                                    EditFlag = Convert.ToBoolean(drvShapeParameter["BITEDIT"]),
        //                                    AngleType = Convert.ToString(drvShapeParameter["CHRANGLETYPE"]),
        //                                    VisibleFlag = Convert.ToBoolean(drvShapeParameter["BITVISIBLE"]),
        //                                    WireType = Convert.ToString(drvShapeParameter["CHRWIRETYPE"]),
        //                                    OHDtls = Convert.ToString(drvShapeParameter["BITOHDTLS"]),
        //                                    EvenMo1 = Convert.ToInt32(drvShapeParameter["SITEVENMO1"]),//To set default value for over hang shape parameters 07/06/2012 --Surendar. S
        //                                    EvenMo2 = Convert.ToInt32(drvShapeParameter["SITEVENMO2"]),
        //                                    OddMo1 = Convert.ToInt32(drvShapeParameter["SITODDMO1"]),
        //                                    OddMo2 = Convert.ToInt32(drvShapeParameter["SITODDMO2"]),
        //                                    EvenCo1 = Convert.ToInt32(drvShapeParameter["SITEVENCO1"]),
        //                                    EvenCo2 = Convert.ToInt32(drvShapeParameter["SITEVENCO2"]),
        //                                    OddCo1 = Convert.ToInt32(drvShapeParameter["SITODDCO1"]),
        //                                    OddCo2 = Convert.ToInt32(drvShapeParameter["SITODDCO2"]),
        //                                    OHIndicator = Convert.ToBoolean(drvShapeParameter["BITDEFAULTOHINDICATOR"]),
        //                                    AngleDir = Convert.ToInt32(drvShapeParameter["INTANGLEDIR"])
        //                                };
        //                                ShapeParameterList.Add(shapeparam);
        //                            }
        //                            shapecode.ShapeParameterList = ShapeParameterList;
        //                        }
        //                    }
        //                }

        //            }
        //            return listShapeCode;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //#endregion

        //#region Shape Code for Cab
        ///// <summary>
        ///// Method to filter shape code based on entered text.
        ///// </summary>
        ///// <param name="enteredText"></param>
        ///// <returns></returns>
        //public List<ShapeCode> FilterShapeCodeForCab_Get(string enteredText)
        //{
        //    IEnumerable<BeamShapeCodeDto> shapeCodeDtos;
        //    List<ShapeCode> listShapeCodeForCab = new List<ShapeCode> { };
        //    DataSet dsShapeCodeForCab = new DataSet();

        //    try
        //    {
        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {
        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            dynamicParameters.Add("@CHRSHAPETEXT", enteredText);
        //            shapeCodeDtos = sqlConnection.Query<BeamShapeCodeDto>(SystemConstant.FILTERSHAPECODEFORCAB_GET, dynamicParameters, commandType: CommandType.StoredProcedure);


        //            if (shapeCodeDtos.Count() > 0)
        //            {
        //                DataTable dt = ConvertToDataTable.ToDataTable(shapeCodeDtos);
        //                dsShapeCodeForCab.Tables.Add(dt);

        //            }

        //            ShapeParameterCollection shapeParameterCollection = new ShapeParameterCollection();

        //            shapeParameterCollection = shapeParameterCollection.ShapeParameterForCab_Get(enteredText);

        //            if (dsShapeCodeForCab != null && dsShapeCodeForCab.Tables.Count != 0)
        //            {
        //                foreach (DataRowView drvBeam in dsShapeCodeForCab.Tables[0].DefaultView)
        //                {
        //                    ShapeParameterCollection strucParamCollFilter = new ShapeParameterCollection();

        //                    foreach (ShapeParameter shapeParamCollection in shapeParameterCollection)
        //                    {
        //                        if (shapeParamCollection.ShapeId == Convert.ToInt32(drvBeam["INTSHAPEID"]))
        //                        {
        //                            strucParamCollFilter.Add(shapeParamCollection);
        //                        }
        //                    }

        //                    ShapeCode shapecode = new ShapeCode
        //                    {
        //                        ShapeID = Convert.ToInt32(drvBeam["INTSHAPEID"]),
        //                        ShapeCodeName = drvBeam["VCHSHAPECODE"].ToString(),
        //                        ShapeParam = strucParamCollFilter,
        //                        ShapePopUp = Convert.ToBoolean(drvBeam["BITSHAPEPOPUP"]),
        //                        IsCapping = Convert.ToBoolean(drvBeam["BITISCAPPING"]),
        //                        NoOfBends = Convert.ToInt32(drvBeam["NOOFBENDS"]),
        //                        MWBendPosition = Convert.ToInt32(drvBeam["INTMWBENDPOSITION"]),
        //                        CWBendPosition = Convert.ToInt32(drvBeam["INTCWBENDPOSITION"]),
        //                        NoOfMWBend = Convert.ToInt32(drvBeam["INTNOOFMWBEND"]),
        //                        NoOfCWBend = Convert.ToInt32(drvBeam["INTNOOFCWBEND"]),
        //                        MWbvbsString = Convert.ToString(drvBeam["VCHMWBVBSTEMPLATE"]),
        //                        CWbvbsString = Convert.ToString(drvBeam["VCHCWBVBSTEMPLATE"])
        //                    };
        //                    listShapeCodeForCab.Add(shapecode);
        //                }
        //            }

        //            return listShapeCodeForCab;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return listShapeCodeForCab;

        //}


        ////START:ADDED BY SIDDHI FOR CAB MODULE ENHANCEMENT CR 13/12/2018
        //public List<ShapeCode> AllShapeCodeForCab_Get()
        //{

        //    List<ShapeCode> listShapeCodeForCab = new List<ShapeCode> { };
        //    DataSet dsShapeCodeForCab = new DataSet();
        //    ShapeParameterCollection shapeParameterCollection;
        //    try
        //    {
        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {
        //            sqlConnection.Open();

        //            //dsShapeCodeForCab = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "allcabshapecodes_get");
        //            dsShapeCodeForCab = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.allcabshapecodes_get, commandType: CommandType.StoredProcedure);

        //            if (dsShapeCodeForCab != null && dsShapeCodeForCab.Tables.Count != 0)
        //            {
        //                foreach (DataRow item in dsShapeCodeForCab.Tables[0].Rows)
        //                {
        //                    shapeParameterCollection = new ShapeParameterCollection();
        //                    shapeParameterCollection = shapeParameterCollection.ShapeParameterForCab_Get(item["VCHSHAPECODE"].ToString());
        //                    ShapeParameterCollection strucParamCollFilter = new ShapeParameterCollection();
        //                    foreach (ShapeParameter shapeParamCollection in shapeParameterCollection)
        //                    {
        //                        if (shapeParamCollection.ShapeId == Convert.ToInt32(item["INTSHAPEID"]))
        //                        {
        //                            strucParamCollFilter.Add(shapeParamCollection);
        //                        }
        //                    }

        //                    ShapeCode shapecode = new ShapeCode
        //                    {
        //                        ShapeID = Convert.ToInt32(item["INTSHAPEID"]),
        //                        ShapeCodeName = item["VCHSHAPECODE"].ToString(),
        //                        ShapeParam = strucParamCollFilter,
        //                        ShapePopUp = Convert.ToBoolean(item["BITSHAPEPOPUP"]),
        //                        IsCapping = Convert.ToBoolean(item["BITISCAPPING"]),
        //                        NoOfBends = Convert.ToInt32(item["NOOFBENDS"]),
        //                        MWBendPosition = Convert.ToInt32(item["INTMWBENDPOSITION"]),
        //                        CWBendPosition = Convert.ToInt32(item["INTCWBENDPOSITION"]),
        //                        NoOfMWBend = Convert.ToInt32(item["INTNOOFMWBEND"]),
        //                        NoOfCWBend = Convert.ToInt32(item["INTNOOFCWBEND"]),
        //                        MWbvbsString = Convert.ToString(item["VCHMWBVBSTEMPLATE"]),
        //                        CWbvbsString = Convert.ToString(item["VCHCWBVBSTEMPLATE"])
        //                    };
        //                    listShapeCodeForCab.Add(shapecode);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return listShapeCodeForCab;
        //}
        ////END




        ///// <summary>
        ///// Method to filter shape code based on entered text during edit mode.
        ///// </summary>
        ///// <param name="enteredText"></param>
        ///// <param name="cabProdmarkId"></param>
        ///// <returns></returns>
        //public List<ShapeCode> FilterShapeCodeForCabEdit_Get(string enteredText, int cabProdmarkId)
        //{

        //    List<ShapeCode> listShapeCodeForCab = new List<ShapeCode> { };
        //    DataSet dsShapeCodeForCab = new DataSet();
        //    ShapeParameterCollection shapeParameterCollection = new ShapeParameterCollection();
        //    try
        //    {
        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {
        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            dynamicParameters.Add("@CHRSHAPETEXT", enteredText);
        //            //  dsShapeCodeForCab = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "USP_FILTERSHAPECODEFORCAB_GET");
        //            dsShapeCodeForCab = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.FILTERSHAPECODEFORCAB_GET, dynamicParameters, commandType: CommandType.StoredProcedure);

        //            //   if (IndexusDistributionCache.SharedCache.Get("CabShapeparamCache") == null)
        //            /// {
        //            shapeParameterCollection = shapeParameterCollection.ShapeParameterForCabEdit_Get(cabProdmarkId);
        //            //}
        //            //else
        //            //{
        //            //    shapeParameterCollection = (ShapeParameterCollection)IndexusDistributionCache.SharedCache.Get("CabShapeparamCache");
        //            //}

        //            if (dsShapeCodeForCab != null && dsShapeCodeForCab.Tables.Count != 0)
        //            {
        //                foreach (DataRowView drvBeam in dsShapeCodeForCab.Tables[0].DefaultView)
        //                {
        //                    ShapeParameterCollection strucParamCollFilter = new ShapeParameterCollection();
        //                    foreach (ShapeParameter shapeParamCollection in shapeParameterCollection)
        //                    {
        //                        if (shapeParamCollection.ShapeId == Convert.ToInt32(drvBeam["INTSHAPEID"]))
        //                        {
        //                            strucParamCollFilter.Add(shapeParamCollection);
        //                        }
        //                    }

        //                    ShapeCode shapecode = new ShapeCode
        //                    {
        //                        ShapeID = Convert.ToInt32(drvBeam["INTSHAPEID"]),
        //                        ShapeCodeName = drvBeam["VCHSHAPECODE"].ToString(),
        //                        ShapeParam = strucParamCollFilter,
        //                        ShapePopUp = Convert.ToBoolean(drvBeam["BITSHAPEPOPUP"]),
        //                        IsCapping = Convert.ToBoolean(drvBeam["BITISCAPPING"]),
        //                        NoOfBends = Convert.ToInt32(drvBeam["NOOFBENDS"]),
        //                        MWBendPosition = Convert.ToInt32(drvBeam["INTMWBENDPOSITION"]),
        //                        CWBendPosition = Convert.ToInt32(drvBeam["INTCWBENDPOSITION"]),
        //                        NoOfMWBend = Convert.ToInt32(drvBeam["INTNOOFMWBEND"]),
        //                        NoOfCWBend = Convert.ToInt32(drvBeam["INTNOOFCWBEND"]),
        //                        MWbvbsString = Convert.ToString(drvBeam["VCHMWBVBSTEMPLATE"]),
        //                        CWbvbsString = Convert.ToString(drvBeam["VCHCWBVBSTEMPLATE"])
        //                    };
        //                    listShapeCodeForCab.Add(shapecode);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return listShapeCodeForCab;
        //}

        ///// <summary>
        ///// Method to filter shape code based on entered text during variable length calcullation.
        ///// </summary>
        ///// <param name="enteredText"></param>
        ///// <param name="cabProdmarkId"></param>
        ///// <param name="seDetailingId"></param>
        ///// <param name="barMark"></param>
        ///// <returns></returns>
        //public List<ShapeCode> GetShapeCodeAndParam(string enteredText, int cabProdmarkId, int seDetailingId, string barMark)
        //{

        //    List<ShapeCode> listShapeCodeForCab = new List<ShapeCode> { };
        //    DataSet dsShapeCodeForCab = new DataSet();
        //    ShapeParameterCollection shapeParameterCollection = new ShapeParameterCollection();
        //    try
        //    {
        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {
        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            dynamicParameters.Add("@CHRSHAPETEXT", enteredText);
        //            // dsShapeCodeForCab = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "USP_FILTERSHAPECODEFORCAB_GET");
        //            dsShapeCodeForCab = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.FILTERSHAPECODEFORCAB_GET, dynamicParameters, commandType: CommandType.StoredProcedure);

        //            //    if (IndexusDistributionCache.SharedCache.Get("CabShapeparamCache") == null)
        //            //  {
        //            if (cabProdmarkId != 0)
        //            {
        //                shapeParameterCollection = shapeParameterCollection.ShapeParameterForCabEdit_Get(cabProdmarkId);
        //            }
        //            else
        //            {
        //                shapeParameterCollection = shapeParameterCollection.ShapeParameterForCabEdit_Get(seDetailingId, barMark);
        //            }
        //            //}
        //            //else
        //            //{
        //            //    shapeParameterCollection = (ShapeParameterCollection)IndexusDistributionCache.SharedCache.Get("CabShapeparamCache");
        //            //}

        //            if (dsShapeCodeForCab != null && dsShapeCodeForCab.Tables.Count != 0)
        //            {
        //                foreach (DataRowView drvBeam in dsShapeCodeForCab.Tables[0].DefaultView)
        //                {
        //                    ShapeParameterCollection strucParamCollFilter = new ShapeParameterCollection();
        //                    foreach (ShapeParameter shapeParamCollection in shapeParameterCollection)
        //                    {
        //                        if (shapeParamCollection.ShapeId == Convert.ToInt32(drvBeam["INTSHAPEID"]))
        //                        {
        //                            strucParamCollFilter.Add(shapeParamCollection);
        //                        }
        //                    }

        //                    ShapeCode shapecode = new ShapeCode
        //                    {
        //                        ShapeID = Convert.ToInt32(drvBeam["INTSHAPEID"]),
        //                        ShapeCodeName = drvBeam["VCHSHAPECODE"].ToString(),
        //                        ShapeParam = strucParamCollFilter,
        //                        ShapePopUp = Convert.ToBoolean(drvBeam["BITSHAPEPOPUP"]),
        //                        IsCapping = Convert.ToBoolean(drvBeam["BITISCAPPING"]),
        //                        NoOfBends = Convert.ToInt32(drvBeam["NOOFBENDS"]),
        //                        MWBendPosition = Convert.ToInt32(drvBeam["INTMWBENDPOSITION"]),
        //                        CWBendPosition = Convert.ToInt32(drvBeam["INTCWBENDPOSITION"]),
        //                        NoOfMWBend = Convert.ToInt32(drvBeam["INTNOOFMWBEND"]),
        //                        NoOfCWBend = Convert.ToInt32(drvBeam["INTNOOFCWBEND"]),
        //                        MWbvbsString = Convert.ToString(drvBeam["VCHMWBVBSTEMPLATE"]),
        //                        CWbvbsString = Convert.ToString(drvBeam["VCHCWBVBSTEMPLATE"])
        //                    };
        //                    listShapeCodeForCab.Add(shapecode);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return listShapeCodeForCab;
        //}

        ///// <summary>
        ///// Method to filter shape code based on entered text during edit mode.
        ///// </summary>
        ///// <param name="enteredText"></param>
        ///// <param name="shapeId"></param>
        ///// <returns></returns>
        //public List<ShapeCode> FilterShapeCodeCabEdit_Get(string enteredText)
        //{

        //    List<ShapeCode> listShapeCodeForCab = new List<ShapeCode> { };
        //    DataSet dsShapeCodeForCab = new DataSet();
        //    ShapeParameterCollection shapeParameterCollection = new ShapeParameterCollection();
        //    try
        //    {
        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {
        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            dynamicParameters.Add("@CHRSHAPETEXT", enteredText);
        //            //dsShapeCodeForCab = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "USP_FILTERSHAPECODEFORCAB_GET");
        //            dsShapeCodeForCab = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.FILTERSHAPECODEFORCAB_GET, dynamicParameters, commandType: CommandType.StoredProcedure);

        //            //if (IndexusDistributionCache.SharedCache.Get("CabShapeparamCache") == null)
        //            //{
        //            shapeParameterCollection = shapeParameterCollection.ShapeParameterCabEdit_Get(enteredText);
        //            //}
        //            //else
        //            //{
        //            //    shapeParameterCollection = (ShapeParameterCollection)IndexusDistributionCache.SharedCache.Get("CabShapeparamCache");
        //            //}

        //            if (dsShapeCodeForCab != null && dsShapeCodeForCab.Tables.Count != 0)
        //            {
        //                foreach (DataRowView drvBeam in dsShapeCodeForCab.Tables[0].DefaultView)
        //                {
        //                    ShapeParameterCollection strucParamCollFilter = new ShapeParameterCollection();
        //                    foreach (ShapeParameter shapeParamCollection in shapeParameterCollection)
        //                    {
        //                        if (shapeParamCollection.ShapeId == Convert.ToInt32(drvBeam["INTSHAPEID"]))
        //                        {
        //                            strucParamCollFilter.Add(shapeParamCollection);
        //                        }
        //                    }

        //                    ShapeCode shapecode = new ShapeCode
        //                    {
        //                        ShapeID = Convert.ToInt32(drvBeam["INTSHAPEID"]),
        //                        ShapeCodeName = drvBeam["VCHSHAPECODE"].ToString(),
        //                        ShapeParam = strucParamCollFilter,
        //                        ShapePopUp = Convert.ToBoolean(drvBeam["BITSHAPEPOPUP"]),
        //                        IsCapping = Convert.ToBoolean(drvBeam["BITISCAPPING"]),
        //                        NoOfBends = Convert.ToInt32(drvBeam["NOOFBENDS"]),
        //                        MWBendPosition = Convert.ToInt32(drvBeam["INTMWBENDPOSITION"]),
        //                        CWBendPosition = Convert.ToInt32(drvBeam["INTCWBENDPOSITION"]),
        //                        NoOfMWBend = Convert.ToInt32(drvBeam["INTNOOFMWBEND"]),
        //                        NoOfCWBend = Convert.ToInt32(drvBeam["INTNOOFCWBEND"]),
        //                        MWbvbsString = Convert.ToString(drvBeam["VCHMWBVBSTEMPLATE"]),
        //                        CWbvbsString = Convert.ToString(drvBeam["VCHCWBVBSTEMPLATE"])
        //                    };
        //                    listShapeCodeForCab.Add(shapecode);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return listShapeCodeForCab;
        //}
        //#endregion

    }
}
