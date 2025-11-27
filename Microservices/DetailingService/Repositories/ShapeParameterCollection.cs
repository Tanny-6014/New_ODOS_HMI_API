using System.Collections;
using System.Data;
using DetailingService.Constants;
using Dapper;
using DetailingService.Dtos;
using Microsoft.Data.SqlClient;
using static Dapper.SqlMapper;
using SharedCache.WinServiceCommon.Provider.Cache;
using Microsoft.AspNetCore.Components;

namespace DetailingService.Repositories
{
    public class ShapeParameterCollection : CollectionBase
    {
        //private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";
        private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";

        public ShapeParameterCollection()
        {

        }


        public ShapeParameter this[int index]
        {
            get { return ((ShapeParameter)List[index]); }
            set { List[index] = value; }
        }

        public int Add(ShapeParameter value)
        {
            return (List.Add(value));
        }

        public int IndexOf(ShapeParameter value)
        {
            return (List.IndexOf(value));
        }

        public void Insert(int index, ShapeParameter value)
        {
            List.Insert(index, value);
        }

        public void Remove(ShapeParameter value)
        {
            List.Remove(value);
        }

        public bool Contains(ShapeParameter value)
        {
            // If value is not of type Issue, this will return false.
            return (List.Contains(value));
        }

        //protected  void OnInsert(int index, Object value)
        //{

        //    if (value.GetType() != Type.GetType("NatSteel.NDS.BLL.ShapeParameter"))669y
        //        throw new ArgumentException("value must be of type ShapeParameter.", "value");
        //}

        //protected  void OnRemove(int index, Object value)
        //{

        //    if (value.GetType() != Type.GetType("NatSteel.NDS.BLL.ShapeParameter"))
        //        throw new ArgumentException("value must be of type ShapeParameter.", "value");
        //}

        //protected  void OnSet(int index, Object oldValue, Object newValue)
        //{

        //    if (newValue.GetType() != Type.GetType("NatSteel.NDS.BLL.ShapeParameter"))
        //        throw new ArgumentException("newValue must be of type ShapeParameter.", "newValue");
        //}

        //protected  void OnValidate(Object value)
        //{

        //    if (value.GetType() != Type.GetType("NatSteel.NDS.BLL.ShapeParameter"))
        //        throw new ArgumentException("value must be of type ShapeParameter.");
        //}

        //private DetailingApplicationContext _dbContext;
        //private readonly IConfiguration _configuration;
        //private string connectionString;
        //public ShapeParameterCollection(DetailingApplicationContext dbContext, IConfiguration configuration)
        //{
        //    _dbContext = dbContext;
        //    _configuration = configuration;

        //    connectionString = _configuration.GetConnectionString(SystemConstant.DefaultDBConnection);
        //}
        #region "Shape Parameter for Beam"
        public ShapeParameterCollection ShapeParameterForBeam_Get()
        {

            ShapeParameterCollection listShapeParameterForBeam = new ShapeParameterCollection();

            DataSet dsShapeParameterForBeam = new DataSet();
            IEnumerable<BeamShapeParameterDto> beamShapeParameterDto;

            try
            {
                // if (IndexusDistributionCache.SharedCache.Get("ShapeparamCache") == null)
                //{
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    //dsShapeParameterForBeam = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_ShapeParameterForBeam_Get");
                    beamShapeParameterDto = sqlConnection.Query<BeamShapeParameterDto>(SystemConstant.ShapeParameterForBeam_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                   
                    if (beamShapeParameterDto.Count() > 0)
                    {
                        DataTable dt = ConvertToDataTable.ToDataTable(beamShapeParameterDto);
                        dsShapeParameterForBeam.Tables.Add(dt);
                    }

                    if (dsShapeParameterForBeam != null && dsShapeParameterForBeam.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBeam in dsShapeParameterForBeam.Tables[0].DefaultView)
                        {
                            ShapeCode objShapeCode = new ShapeCode();
                            objShapeCode.ShapeID = Convert.ToInt32(drvBeam["INTSHAPEID"]);
                            ShapeParameter shapeparam = new ShapeParameter
                            {

                                ShapeId = Convert.ToInt32(drvBeam["INTSHAPEID"]),
                                ParameterName = Convert.ToString(drvBeam["CHRPARAMNAME"]),
                                CriticalIndiacator = Convert.ToString(drvBeam["CHRCRITICALIND"]),
                                SequenceNumber = Convert.ToInt16(drvBeam["INTPARAMSEQ"]),
                                ParameterValue = 0,// Convert.ToInt32(drvBeam["DEFAULTPARAMVALUE"])
                                ShapeCodeImage = Convert.ToString(drvBeam["CHRSHAPECODE"]),
                                EditFlag = Convert.ToBoolean(drvBeam["BITEDIT"]),
                                VisibleFlag = Convert.ToBoolean(drvBeam["BITVISIBLE"]),
                                WireType = Convert.ToString(drvBeam["CHRWIRETYPE"]),
                                AngleType = Convert.ToString(drvBeam["CHRANGLETYPE"]),
                                AngleDir = Convert.ToInt32(drvBeam["INTANGLEDIR"])
                            };
                            listShapeParameterForBeam.Add(shapeparam);

                        }

                        // IndexusDistributionCache.SharedCache.Add("ShapeparamCache", listShapeParameterForBeam, DateTime.Today.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings.Get("cacheTimeOut"))));
                    }
                }
                //}
                //else
                //{
                //    listShapeParameterForBeam = (ShapeParameterCollection)IndexusDistributionCache.SharedCache.Get("ShapeparamCache");
                //}

                //dbManager.Open();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally
            //{
            //    dbManager.Dispose();
            //}

            return listShapeParameterForBeam;
        }
        #endregion

        #region "Shape Parameter for Column"
        public ShapeParameterCollection ColumnShapeParameter_Get()
        {
            ShapeParameterCollection listColumnShapeParameter = new ShapeParameterCollection();

            DataSet dsColumnShapeParameter = new DataSet();
            IEnumerable<ColumnShapeParameterDto> ColumnShapeParameterDto;

            try
            {
                //if (IndexusDistributionCache.SharedCache.Get("ColumnShapeparamCache") == null)
                {
                    using (var sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();

                        //  dsColumnShapeParameter = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_ColumnShapeParameter_Get");
                        ColumnShapeParameterDto = sqlConnection.Query<ColumnShapeParameterDto>(SystemConstant.ColumnShapeParameter_Get, commandType: CommandType.StoredProcedure);

                        if (ColumnShapeParameterDto.Count() > 0)
                        {
                            DataTable dt = ConvertToDataTable.ToDataTable(ColumnShapeParameterDto);
                            dsColumnShapeParameter.Tables.Add(dt);

                            if (dsColumnShapeParameter != null && dsColumnShapeParameter.Tables.Count != 0)
                            {
                                foreach (DataRowView drvBeam in dsColumnShapeParameter.Tables[0].DefaultView)
                                {
                                    ShapeCode objShapeCode = new ShapeCode();
                                    objShapeCode.ShapeID = Convert.ToInt32(drvBeam["INTSHAPEID"]);
                                    ShapeParameter shapeparam = new ShapeParameter
                                    {

                                        ShapeId = Convert.ToInt32(drvBeam["INTSHAPEID"]),
                                        ParameterName = Convert.ToString(drvBeam["CHRPARAMNAME"]),
                                        CriticalIndiacator = Convert.ToString(drvBeam["CHRCRITICALIND"]),
                                        SequenceNumber = Convert.ToInt16(drvBeam["INTPARAMSEQ"]),
                                        ParameterValue = 0,//Convert.ToInt32(drvBeam["DEFAULTPARAMVALUE"]),
                                        ShapeCodeImage = Convert.ToString(drvBeam["CHRSHAPECODE"]),
                                        EditFlag = Convert.ToBoolean(drvBeam["BITEDIT"]),
                                        VisibleFlag = Convert.ToBoolean(drvBeam["BITVISIBLE"])
                                    };
                                    listColumnShapeParameter.Add(shapeparam);

                                }

                                //IndexusDistributionCache.SharedCache.Add("ColumnShapeparamCache", listColumnShapeParameter, DateTime.Today.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings.Get("cacheTimeOut"))));
                            }
                        }
                        //}
                        //else
                        //{
                        //    listColumnShapeParameter = (ShapeParameterCollection)IndexusDistributionCache.SharedCache.Get("ColumnShapeparamCache");
                        //}

                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listColumnShapeParameter;
        }
        #endregion

        #region "Shape Parameter for Slab"
        public ShapeParameterCollection SlabShapeParameter_Get()
        {

            ShapeParameterCollection listSlabShapeParameter = new ShapeParameterCollection();

            //List<ShapeParameterCollection> listSlabShapeParameter = new List<ShapeParameterCollection>();

            DataSet dsSlabShapeParameter = new DataSet();
            IEnumerable<SlabShapeParameterDto> slabShapeParameterDto; 
            try
            {
                //if (IndexusDistributionCache.SharedCache.Get("SlabShapeparamCache") == null)
                
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    
                    //dsSlabShapeParameter = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_SlabShapeParameter_Get");
                    slabShapeParameterDto = sqlConnection.Query<SlabShapeParameterDto>(SystemConstant.SlabShapeParameter_Get, commandType: CommandType.StoredProcedure);
                    
                    if (slabShapeParameterDto.Count() > 0)
                    {
                        DataTable dt = ConvertToDataTable.ToDataTable(slabShapeParameterDto);
                        dsSlabShapeParameter.Tables.Add(dt);


                        if (dsSlabShapeParameter != null && dsSlabShapeParameter.Tables.Count != 0)
                        {
                        foreach (DataRowView drvBeam in dsSlabShapeParameter.Tables[0].DefaultView)
                        {
                            ShapeCode objShapeCode = new ShapeCode();
                            objShapeCode.ShapeID = Convert.ToInt32(drvBeam["INTSHAPEID"]);
                            ShapeParameter shapeparam = new ShapeParameter
                            {

                                ShapeId = Convert.ToInt32(drvBeam["INTSHAPEID"]),
                                ParameterName = Convert.ToString(drvBeam["CHRPARAMNAME"]),
                                CriticalIndiacator = Convert.ToString(drvBeam["CHRCRITICALIND"]),
                                SequenceNumber = Convert.ToInt16(drvBeam["INTPARAMSEQ"]),
                                ParameterValue = Convert.ToInt32(drvBeam["DEFAULTPARAMVALUE"]),//To set default value for shape parameters 07/06/2012 --Surendar. S
                                ShapeCodeImage = Convert.ToString(drvBeam["CHRSHAPECODE"]),
                                EditFlag = Convert.ToBoolean(drvBeam["BITEDIT"]),
                                AngleType = Convert.ToString(drvBeam["CHRANGLETYPE"]),
                                VisibleFlag = Convert.ToBoolean(drvBeam["BITVISIBLE"]),
                                WireType = Convert.ToString(drvBeam["CHRWIRETYPE"]),
                                OHDtls = Convert.ToString(drvBeam["BITOHDTLS"]),
                                EvenMo1 = Convert.ToInt32(drvBeam["SITEVENMO1"]),//To set default value for over hang shape parameters 07/06/2012 --Surendar. S
                                EvenMo2 = Convert.ToInt32(drvBeam["SITEVENMO2"]),
                                OddMo1 = Convert.ToInt32(drvBeam["SITODDMO1"]),
                                OddMo2 = Convert.ToInt32(drvBeam["SITODDMO2"]),
                                EvenCo1 = Convert.ToInt32(drvBeam["SITEVENCO1"]),
                                EvenCo2 = Convert.ToInt32(drvBeam["SITEVENCO2"]),
                                OddCo1 = Convert.ToInt32(drvBeam["SITODDCO1"]),
                                OddCo2 = Convert.ToInt32(drvBeam["SITODDCO2"]),
                                OHIndicator = Convert.ToBoolean(drvBeam["BITDEFAULTOHINDICATOR"]),
                                AngleDir = Convert.ToInt32(drvBeam["INTANGLEDIR"])


                            };
                                listSlabShapeParameter.Add(shapeparam);


                         }


                            //IndexusDistributionCache.SharedCache.Add("SlabShapeparamCache", listSlabShapeParameter, DateTime.Today.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings.Get("cacheTimeOut"))));
                        }
                    }
                    
                    //}
                    //else
                    //{
                    //    listSlabShapeParameter = (ShapeParameterCollection)IndexusDistributionCache.SharedCache.Get("SlabShapeparamCache");
                    //}

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listSlabShapeParameter;
        }
        #endregion

        // CARPET anuran
        #region "Shape Parameter for CARPET"
        public ShapeParameterCollection CarpetShapeParameter_Get()
        {

            ShapeParameterCollection listCarpetShapeParameter = new ShapeParameterCollection();
            IEnumerable<SlabShapeParameterDto> slabShapeParameterDto;
            DataSet dsCarpetShapeParameter = new DataSet();

            try
            {
                
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    slabShapeParameterDto =sqlConnection.Query<SlabShapeParameterDto>(SystemConstant.SlabShapeParameter_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (slabShapeParameterDto.Count() > 0)
                    {
                        DataTable dt = ConvertToDataTable.ToDataTable(slabShapeParameterDto);
                        dsCarpetShapeParameter.Tables.Add(dt);


                        if (dsCarpetShapeParameter != null && dsCarpetShapeParameter.Tables.Count != 0)
                        {
                            foreach (DataRowView drvBeam in dsCarpetShapeParameter.Tables[0].DefaultView)
                            {
                                ShapeCode objShapeCode = new ShapeCode();
                                objShapeCode.ShapeID = Convert.ToInt32(drvBeam["INTSHAPEID"]);
                                ShapeParameter shapeparam = new ShapeParameter
                                {

                                    ShapeId = Convert.ToInt32(drvBeam["INTSHAPEID"]),
                                    ParameterName = Convert.ToString(drvBeam["CHRPARAMNAME"]),
                                    CriticalIndiacator = Convert.ToString(drvBeam["CHRCRITICALIND"]),
                                    SequenceNumber = Convert.ToInt16(drvBeam["INTPARAMSEQ"]),
                                    ParameterValue = Convert.ToInt32(drvBeam["DEFAULTPARAMVALUE"]),//To set default value for shape parameters 07/06/2012 --Surendar. S
                                    ShapeCodeImage = Convert.ToString(drvBeam["CHRSHAPECODE"]),
                                    EditFlag = Convert.ToBoolean(drvBeam["BITEDIT"]),
                                    AngleType = Convert.ToString(drvBeam["CHRANGLETYPE"]),
                                    VisibleFlag = Convert.ToBoolean(drvBeam["BITVISIBLE"]),
                                    WireType = Convert.ToString(drvBeam["CHRWIRETYPE"]),
                                    OHDtls = Convert.ToString(drvBeam["BITOHDTLS"]),
                                    EvenMo1 = Convert.ToInt32(drvBeam["SITEVENMO1"]),//To set default value for over hang shape parameters 07/06/2012 --Surendar. S
                                    EvenMo2 = Convert.ToInt32(drvBeam["SITEVENMO2"]),
                                    OddMo1 = Convert.ToInt32(drvBeam["SITODDMO1"]),
                                    OddMo2 = Convert.ToInt32(drvBeam["SITODDMO2"]),
                                    EvenCo1 = Convert.ToInt32(drvBeam["SITEVENCO1"]),
                                    EvenCo2 = Convert.ToInt32(drvBeam["SITEVENCO2"]),
                                    OddCo1 = Convert.ToInt32(drvBeam["SITODDCO1"]),
                                    OddCo2 = Convert.ToInt32(drvBeam["SITODDCO2"]),
                                    OHIndicator = Convert.ToBoolean(drvBeam["BITDEFAULTOHINDICATOR"]),
                                    AngleDir = Convert.ToInt32(drvBeam["INTANGLEDIR"])
                                };
                                listCarpetShapeParameter.Add(shapeparam);

                            }

                            //IndexusDistributionCache.SharedCache.Add("CarpetShapeparamCache", listCarpetShapeParameter, DateTime.Today.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings.Get("cacheTimeOut"))));
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listCarpetShapeParameter;
        }
        #endregion

        #region "Shape Parameter for Cab"

        /// <summary>
        /// Method to get shape parameters to populate the CAB product marking grid.
        /// </summary>
        /// <param name="intSEDetailingID"></param>
        /// <returns></returns>
        public ShapeParameterCollection ShapeParameterForCab_Get(int intSEDetailingID)
        {

            ShapeParameterCollection listShapeParameterForCab = new ShapeParameterCollection();
            DataSet dsShapeParameterForCab = new DataSet();
            try
            {
                //if (IndexusDistributionCache.SharedCache.Get("CabShapeparamCache") == null)
                //{
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    //var dynamicParameters = new DynamicParameters();
                    //dynamicParameters.Add("@INTSEDETAILINGID", intSEDetailingID);
                    //dsShapeParameterForCab = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.GET_SHAPEPARAMETER_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);

                    

                    SqlCommand cmd = new SqlCommand(SystemConstant.GET_SHAPEPARAMETER_CUBE, sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@INTSEDETAILINGID", intSEDetailingID));

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dsShapeParameterForCab);
                    
                    if (dsShapeParameterForCab != null && dsShapeParameterForCab.Tables.Count != 0)
                    {
                        foreach (DataRowView drvCab in dsShapeParameterForCab.Tables[0].DefaultView)
                        {
                            ShapeParameter shapeparam = new ShapeParameter
                            {
                                ShapeId = Convert.ToInt32(drvCab["INTSHAPEID"]),
                                ParameterName = Convert.ToString(drvCab["CHRPARAMNAME"]),
                                CriticalIndiacator = Convert.ToString(drvCab["CHRCRITICALIND"]),
                                SequenceNumber = Convert.ToInt16(drvCab["INTPARAMSEQ"]),
                                ParameterValueCab = Convert.ToString(drvCab["PARAMETERVALUE"]),
                                ShapeCodeImage = Convert.ToString(drvCab["CHRSHAPECODE"]),
                                EditFlag = Convert.ToBoolean(drvCab["BITEDIT"]),
                                VisibleFlag = Convert.ToBoolean(drvCab["BITVISIBLE"]),
                                WireType = Convert.ToString(drvCab["CHRWIRETYPE"]),
                                AngleType = Convert.ToString(drvCab["CHRANGLETYPE"]),
                                AngleDir = Convert.ToInt32(drvCab["INTANGLEDIR"]),
                                symmetricIndex = Convert.ToString(drvCab["SYMMETRYINDEX"]),
                                HeghtAngleFormula = Convert.ToString(drvCab["HEIGHTANGLEFORMULA"]),
                                HeghtSuceedAngleFormula = Convert.ToString(drvCab["HEIGHTSUCEEDANGLEFORMULA"]),
                                IntXCord = Convert.ToInt32(drvCab["INTXCOORD"]),
                                IntYCord = Convert.ToInt32(drvCab["INTYCOORD"]),
                                IntZCord = Convert.ToInt32(drvCab["INTZCOORD"]),
                                CustFormula = Convert.ToString(drvCab["VCHCUSTOMFORMULA"]),
                                OffsetAngleFormula = Convert.ToString(drvCab["VCHOFFSETANGLEFORMULA"]),
                                OffsetSuceedAngleFormula = Convert.ToString(drvCab["OFFSETSUCEEDANGLEFORMULA"]),
                                CouplerType = Convert.ToString(drvCab["CSD_INPUT_TYPE"]),
                                ParameterView = Convert.ToString(drvCab["CSD_VIEW"]),
                                RoundOffRange = Convert.ToInt32(drvCab["RoundOffRange"]),
                                ParameterValue = Convert.ToInt32(drvCab["PARAMETERVALUE"])
                            };


                            listShapeParameterForCab.Add(shapeparam);
                        }

                    }
                    
                 
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listShapeParameterForCab;
        }
        public ShapeParameterCollection ShapeParameterForCab_Get1(int Cabproductmarkid)
        {

            ShapeParameterCollection listShapeParameterForCab = new ShapeParameterCollection();
            DataSet dsShapeParameterForCab = new DataSet();
            try
            {
                //if (IndexusDistributionCache.SharedCache.Get("CabShapeparamCache") == null)
                //{
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    //var dynamicParameters = new DynamicParameters();
                    //dynamicParameters.Add("@INTSEDETAILINGID", intSEDetailingID);
                    //dsShapeParameterForCab = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.GET_SHAPEPARAMETER_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);



                    SqlCommand cmd = new SqlCommand(SystemConstant.SHAPEPARAMETERFORCAB_EDIT_CUBE, sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@INTCABPRODUCTMARKID", Cabproductmarkid));

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dsShapeParameterForCab);

                    if (dsShapeParameterForCab != null && dsShapeParameterForCab.Tables.Count != 0)
                    {
                        foreach (DataRowView drvCab in dsShapeParameterForCab.Tables[0].DefaultView)
                        {
                            ShapeParameter shapeparam = new ShapeParameter
                            {
                                ShapeId = Convert.ToInt32(drvCab["INTSHAPEID"]),
                                ParameterName = Convert.ToString(drvCab["CHRPARAMNAME"]),
                                CriticalIndiacator = Convert.ToString(drvCab["CHRCRITICALIND"]),
                                SequenceNumber = Convert.ToInt16(drvCab["INTPARAMSEQ"]),
                                ParameterValueCab = Convert.ToString(drvCab["INTSEGMENTVALUE"]),
                                ShapeCodeImage = Convert.ToString(drvCab["CHRSHAPECODE"]),
                                EditFlag = Convert.ToBoolean(drvCab["BITEDIT"]),
                                VisibleFlag = Convert.ToBoolean(drvCab["BITVISIBLE"]),
                                WireType = Convert.ToString(drvCab["CHRWIRETYPE"]),
                                AngleType = Convert.ToString(drvCab["CHRANGLETYPE"]),
                                AngleDir = Convert.ToInt32(drvCab["INTANGLEDIR"]),
                                symmetricIndex = Convert.ToString(drvCab["SYMMETRYINDEX"]),
                                HeghtAngleFormula = Convert.ToString(drvCab["HEIGHTANGLEFORMULA"]),
                                HeghtSuceedAngleFormula = Convert.ToString(drvCab["HEIGHTSUCEEDANGLEFORMULA"]),
                                IntXCord = Convert.ToInt32(drvCab["INTXCOORD"]),
                                IntYCord = Convert.ToInt32(drvCab["INTYCOORD"]),
                                IntZCord = Convert.ToInt32(drvCab["INTZCOORD"]),
                                CustFormula = Convert.ToString(drvCab["VCHCUSTOMFORMULA"]),
                                OffsetAngleFormula = Convert.ToString(drvCab["VCHOFFSETANGLEFORMULA"]),
                                OffsetSuceedAngleFormula = Convert.ToString(drvCab["OFFSETSUCEEDANGLEFORMULA"]),
                                CouplerType = Convert.ToString(drvCab["CSD_INPUT_TYPE"]),
                                ParameterView = Convert.ToString(drvCab["CSD_VIEW"]),
                                RoundOffRange = Convert.ToInt32(drvCab["RoundOffRange"]),
                              //  ParameterValue = Convert.ToInt32(drvCab["PARAMETERVALUE"])
                            };


                            listShapeParameterForCab.Add(shapeparam);
                        }

                        //IndexusDistributionCache.SharedCache.Add("CabShapeparamCache", listShapeParameterForCab, DateTime.Today.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings.Get("cacheTimeOut"))));
                    }

                    //else
                    //{
                    //    listShapeParameterForCab = (ShapeParameterCollection)IndexusDistributionCache.SharedCache.Get("CabShapeparamCache");
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listShapeParameterForCab;
        }

        /// <summary>
        /// Method to get shape parameters to populate the product marking grid duing double capture.
        /// </summary>
        /// <param name="intSEDetailingID"></param>
        /// <returns></returns>
        //public ShapeParameterCollection Get_ShapeParameterForDoubleCapture(int intSEDetailingID)
        //{
        //    
        //    ShapeParameterCollection listShapeParameterForCab = new ShapeParameterCollection();
        //    DataSet dsShapeParameterForCab = new DataSet();
        //    try
        //    {
        //        if (IndexusDistributionCache.SharedCache.Get("CabShapeparamCache") == null)
        //        {
        //            dbManager.Open();
        //            dbManager.CreateParameters(1);
        //            dynamicParameters.Add(0, "@INTSEDETAILINGID", intSEDetailingID);
        //            dsShapeParameterForCab = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "USP_GET_SHAPEPARAMETER_DOUBLECAP_CUBE");
        //            if (dsShapeParameterForCab != null && dsShapeParameterForCab.Tables.Count != 0)
        //            {
        //                foreach (DataRowView drvCab in dsShapeParameterForCab.Tables[0].DefaultView)
        //                {
        //                    ShapeParameter shapeparam = new ShapeParameter
        //                    {
        //                        ShapeId = Convert.ToInt32(drvCab["INTSHAPEID"]),
        //                        ParameterName = Convert.ToString(drvCab["CHRPARAMNAME"]),
        //                        CriticalIndiacator = Convert.ToString(drvCab["CHRCRITICALIND"]),
        //                        SequenceNumber = Convert.ToInt16(drvCab["INTPARAMSEQ"]),
        //                        ParameterValueCab = Convert.ToString(drvCab["INTSEGMENTVALUE"]),
        //                        ShapeCodeImage = Convert.ToString(drvCab["CHRSHAPECODE"]),
        //                        EditFlag = Convert.ToBoolean(drvCab["BITEDIT"]),
        //                        VisibleFlag = Convert.ToBoolean(drvCab["BITVISIBLE"]),
        //                        WireType = Convert.ToString(drvCab["CHRWIRETYPE"]),
        //                        AngleType = Convert.ToString(drvCab["CHRANGLETYPE"]),
        //                        AngleDir = Convert.ToInt32(drvCab["INTANGLEDIR"]),
        //                        symmetricIndex = Convert.ToString(drvCab["SYMMETRYINDEX"]),
        //                        HeghtAngleFormula = Convert.ToString(drvCab["HEIGHTANGLEFORMULA"]),
        //                        HeghtSuceedAngleFormula = Convert.ToString(drvCab["HEIGHTSUCEEDANGLEFORMULA"]),
        //                        IntXCord = Convert.ToInt32(drvCab["INTXCOORD"]),
        //                        IntYCord = Convert.ToInt32(drvCab["INTYCOORD"]),
        //                        IntZCord = Convert.ToInt32(drvCab["INTZCOORD"]),
        //                        CustFormula = Convert.ToString(drvCab["VCHCUSTOMFORMULA"]),
        //                        OffsetAngleFormula = Convert.ToString(drvCab["VCHOFFSETANGLEFORMULA"]),
        //                        OffsetSuceedAngleFormula = Convert.ToString(drvCab["OFFSETSUCEEDANGLEFORMULA"])
        //                    };
        //                    listShapeParameterForCab.Add(shapeparam);
        //                }
        //                IndexusDistributionCache.SharedCache.Add("CabShapeparamCache", listShapeParameterForCab, DateTime.Today.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings.Get("cacheTimeOut"))));
        //            }
        //        }
        //        else
        //        {
        //            listShapeParameterForCab = (ShapeParameterCollection)IndexusDistributionCache.SharedCache.Get("CabShapeparamCache");
        //        }
        //        dbManager.Open();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //        dsShapeParameterForCab.Dispose();
        //    }
        //    return listShapeParameterForCab;
        //}

        /// <summary>
        /// Method to get the shape parameters for OES.
        /// </summary>
        /// <param name="shapeCode"></param>
        /// <returns></returns>
        //public List<ShapeParameter> GetShapeParamsOES(string shapeCode)
        //{
        //    
        //    List<ShapeParameter> lstShapeParam = new List<ShapeParameter>();
        //    DataSet dsShapeParameterForCab = new DataSet();
        //    try
        //    {
        //        dbManager.Open();
        //        dbManager.CreateParameters(1);
        //        dynamicParameters.Add(0, "@SHAPECODE", shapeCode);
        //        dsShapeParameterForCab = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "USP_SHAPEPARAMETERFORCAB_GET_CUBE");
        //        if (dsShapeParameterForCab != null && dsShapeParameterForCab.Tables.Count != 0)
        //        {
        //            foreach (DataRowView drvCab in dsShapeParameterForCab.Tables[0].DefaultView)
        //            {
        //                ShapeParameter shapeparam = new ShapeParameter
        //                {
        //                    ShapeId = Convert.ToInt32(drvCab["INTSHAPEID"]),
        //                    ParameterName = Convert.ToString(drvCab["CHRPARAMNAME"]),
        //                    CriticalIndiacator = Convert.ToString(drvCab["CHRCRITICALIND"]),
        //                    SequenceNumber = Convert.ToInt16(drvCab["INTPARAMSEQ"]),
        //                    ParameterValueCab = Convert.ToString(drvCab["PARAMETERVALUE"]),
        //                    ShapeCodeImage = Convert.ToString(drvCab["CHRSHAPECODE"]),
        //                    EditFlag = Convert.ToBoolean(drvCab["BITEDIT"]),
        //                    VisibleFlag = Convert.ToBoolean(drvCab["BITVISIBLE"]),
        //                    WireType = Convert.ToString(drvCab["CHRWIRETYPE"]),
        //                    AngleType = Convert.ToString(drvCab["CHRANGLETYPE"]),
        //                    AngleDir = Convert.ToInt32(drvCab["INTANGLEDIR"]),
        //                    symmetricIndex = Convert.ToString(drvCab["SYMMETRYINDEX"]),
        //                    HeghtAngleFormula = Convert.ToString(drvCab["HEIGHTANGLEFORMULA"]),
        //                    HeghtSuceedAngleFormula = Convert.ToString(drvCab["HEIGHTSUCEEDANGLEFORMULA"]),
        //                    IntXCord = Convert.ToInt32(drvCab["INTXCOORD"]),
        //                    IntYCord = Convert.ToInt32(drvCab["INTYCOORD"]),
        //                    IntZCord = Convert.ToInt32(drvCab["INTZCOORD"]),
        //                    CustFormula = Convert.ToString(drvCab["VCHCUSTOMFORMULA"]),
        //                    OffsetAngleFormula = Convert.ToString(drvCab["VCHOFFSETANGLEFORMULA"]),
        //                    OffsetSuceedAngleFormula = Convert.ToString(drvCab["OFFSETSUCEEDANGLEFORMULA"])
        //                };
        //                lstShapeParam.Add(shapeparam);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //        dsShapeParameterForCab.Dispose();
        //    }
        //    return lstShapeParam;
        //}

        /// <summary>
        /// Method to get shape parameters during add mode.
        /// </summary>
        /// <param name="enteredText"></param>
        /// <returns></returns>
        /// 
        //CAB using this method
        public ShapeParameterCollection ShapeParameterForCab_Get(string enteredText)
        {
            

            ShapeParameterCollection listShapeParameterForCab = new ShapeParameterCollection();
            DataSet dsShapeParameterForCab = new DataSet();
            IEnumerable<CABShapeParameterDto> shapeParameterDtos;
            try
            {
               
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@SHAPECODE", enteredText);
                    shapeParameterDtos = sqlConnection.Query<CABShapeParameterDto>(SystemConstant.SHAPEPARAMETERFORCAB_GET_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (shapeParameterDtos.Count() > 0)
                    {
                        DataTable dt = ConvertToDataTable.ToDataTable(shapeParameterDtos);
                        dsShapeParameterForCab.Tables.Add(dt);

                    }
                    if (dsShapeParameterForCab != null && dsShapeParameterForCab.Tables.Count != 0)
                    {
                        
                        foreach (DataRowView drvCab in dsShapeParameterForCab.Tables[0].DefaultView)
                        {
                            ShapeCode objShapeCode = new ShapeCode();
                            //objShapeCode.ShapeID = Convert.ToInt32(drvCab["INTSHAPEID"]);
                            objShapeCode.ShapeCodeName = Convert.ToString(drvCab["CHRSHAPECODE"]);

                            ShapeParameter shapeparam = new ShapeParameter
                            {
                                ShapeId = Convert.ToInt32(drvCab["INTSHAPEID"]),
                                ParameterName = Convert.ToString(drvCab["CHRPARAMNAME"]),
                                CriticalIndiacator = Convert.ToString(drvCab["CHRCRITICALIND"]),
                                SequenceNumber = Convert.ToInt16(drvCab["INTPARAMSEQ"]),
                                ParameterValueCab = Convert.ToString(drvCab["PARAMETERVALUE"]),
                                ShapeCodeImage = Convert.ToString(drvCab["CHRSHAPECODE"]),
                                EditFlag = Convert.ToBoolean(drvCab["BITEDIT"]),
                                VisibleFlag = Convert.ToBoolean(drvCab["BITVISIBLE"]),
                                WireType = Convert.ToString(drvCab["CHRWIRETYPE"]),
                                AngleType = Convert.ToString(drvCab["CHRANGLETYPE"]),
                                AngleDir = Convert.ToInt32(drvCab["INTANGLEDIR"]),
                                symmetricIndex = Convert.ToString(drvCab["SYMMETRYINDEX"]),
                                HeghtAngleFormula = Convert.ToString(drvCab["HEIGHTANGLEFORMULA"]),
                                HeghtSuceedAngleFormula = Convert.ToString(drvCab["HEIGHTSUCEEDANGLEFORMULA"]),
                                IntXCord = Convert.ToInt32(drvCab["INTXCOORD"]),
                                IntYCord = Convert.ToInt32(drvCab["INTYCOORD"]),
                                IntZCord = Convert.ToInt32(drvCab["INTZCOORD"]),
                                CustFormula = Convert.ToString(drvCab["VCHCUSTOMFORMULA"]),
                                OffsetAngleFormula = Convert.ToString(drvCab["VCHOFFSETANGLEFORMULA"]),
                                OffsetSuceedAngleFormula = Convert.ToString(drvCab["OFFSETSUCEEDANGLEFORMULA"]),
                                CouplerType = Convert.ToString(drvCab["CSD_INPUT_TYPE"]),
                                ParameterView = Convert.ToString(drvCab["CSD_VIEW"]),
                                RoundOffRange = Convert.ToInt32(drvCab["RoundOffRange"])
                            };
                            listShapeParameterForCab.Add(shapeparam);
                        }
                        // IndexusDistributionCache.SharedCache.Add("CabShapeparamCache", listShapeParameterForCab, DateTime.Today.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings.Get("cacheTimeOut"))));
                    }
                    
                    //else
                    //{
                    //    listShapeParameterForCab = (ShapeParameterCollection)IndexusDistributionCache.SharedCache.Get("CabShapeparamCache");
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listShapeParameterForCab;
        }

       

        /// <summary>
        /// Method to get shape parameters during edit mode.
        /// </summary>
        /// <param name="cabProdmarkId"></param>
        /// <returns></returns>
        public ShapeParameterCollection ShapeParameterForCabEdit_Get(int cabProdmarkId)
        {

            ShapeParameterCollection listShapeParameterForCab = new ShapeParameterCollection();
            DataSet dsShapeParameterForCab = new DataSet();

            try
            {
                // if (IndexusDistributionCache.SharedCache.Get("CabShapeparamCache") == null)
                // {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTCABPRODUCTMARKID", cabProdmarkId);
                    //dsShapeParameterForCab = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "USP_SHAPEPARAMETERFORCAB_EDIT_CUBE");
                    dsShapeParameterForCab = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.SHAPEPARAMETERFORCAB_EDIT_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);


                    if (dsShapeParameterForCab != null && dsShapeParameterForCab.Tables.Count != 0)
                    {
                        foreach (DataRowView drvCab in dsShapeParameterForCab.Tables[0].DefaultView)
                        {
                            ShapeParameter shapeparam = new ShapeParameter
                            {
                                ShapeId = Convert.ToInt32(drvCab["INTSHAPEID"]),
                                ParameterName = Convert.ToString(drvCab["CHRPARAMNAME"]),
                                CriticalIndiacator = Convert.ToString(drvCab["CHRCRITICALIND"]),
                                SequenceNumber = Convert.ToInt16(drvCab["INTPARAMSEQ"]),
                                ParameterValueCab = Convert.ToString(drvCab["INTSEGMENTVALUE"]),
                                ShapeCodeImage = Convert.ToString(drvCab["CHRSHAPECODE"]),
                                EditFlag = Convert.ToBoolean(drvCab["BITEDIT"]),
                                VisibleFlag = Convert.ToBoolean(drvCab["BITVISIBLE"]),
                                WireType = Convert.ToString(drvCab["CHRWIRETYPE"]),
                                AngleType = Convert.ToString(drvCab["CHRANGLETYPE"]),
                                AngleDir = Convert.ToInt32(drvCab["INTANGLEDIR"]),
                                symmetricIndex = Convert.ToString(drvCab["SYMMETRYINDEX"]),
                                HeghtAngleFormula = Convert.ToString(drvCab["HEIGHTANGLEFORMULA"]),
                                HeghtSuceedAngleFormula = Convert.ToString(drvCab["HEIGHTSUCEEDANGLEFORMULA"]),
                                IntXCord = Convert.ToInt32(drvCab["INTXCOORD"]),
                                IntYCord = Convert.ToInt32(drvCab["INTYCOORD"]),
                                IntZCord = Convert.ToInt32(drvCab["INTZCOORD"]),
                                CustFormula = Convert.ToString(drvCab["VCHCUSTOMFORMULA"]),
                                OffsetAngleFormula = Convert.ToString(drvCab["VCHOFFSETANGLEFORMULA"]),
                                OffsetSuceedAngleFormula = Convert.ToString(drvCab["OFFSETSUCEEDANGLEFORMULA"]),
                                CouplerType = Convert.ToString(drvCab["CSD_INPUT_TYPE"]),
                                ParameterView = Convert.ToString(drvCab["CSD_VIEW"]),
                                RoundOffRange = Convert.ToInt32(drvCab["RoundOffRange"])
                            };
                            listShapeParameterForCab.Add(shapeparam);
                        }
                        //  IndexusDistributionCache.SharedCache.Add("CabShapeparamCache", listShapeParameterForCab, DateTime.Today.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings.Get("cacheTimeOut"))));
                    }
                    // }
                    //else
                    //{
                    //    listShapeParameterForCab = (ShapeParameterCollection)IndexusDistributionCache.SharedCache.Get("CabShapeparamCache");
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listShapeParameterForCab;
        }

        /// <summary>
        /// Method to get shape parameters during variable length.
        /// </summary>
        /// <param name="intSEDetailingID"></param>
        /// <param name="barMark"></param>
        /// <returns></returns>
        public ShapeParameterCollection ShapeParameterForCabEdit_Get(int intSEDetailingID, string barMark)
        {

            ShapeParameterCollection listShapeParameterForCab = new ShapeParameterCollection();
            DataSet dsShapeParameterForCab = new DataSet();

            try
            {
                // if (IndexusDistributionCache.SharedCache.Get("CabShapeparamCache") == null)
                // {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTSEDETAILINGID", intSEDetailingID);
                    dynamicParameters.Add("@BARMARK", barMark);
                    //dsShapeParameterForCab = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "USP_GET_SHAPEPARAMETER_VARLENGTH_CUBE");
                    dsShapeParameterForCab = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.GET_SHAPEPARAMETER_VARLENGTH_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);


                    if (dsShapeParameterForCab != null && dsShapeParameterForCab.Tables.Count != 0)
                    {
                        foreach (DataRowView drvCab in dsShapeParameterForCab.Tables[0].DefaultView)
                        {
                            ShapeParameter shapeparam = new ShapeParameter
                            {
                                ShapeId = Convert.ToInt32(drvCab["INTSHAPEID"]),
                                ParameterName = Convert.ToString(drvCab["CHRPARAMNAME"]),
                                CriticalIndiacator = Convert.ToString(drvCab["CHRCRITICALIND"]),
                                SequenceNumber = Convert.ToInt16(drvCab["INTPARAMSEQ"]),
                                ParameterValueCab = Convert.ToString(drvCab["INTSEGMENTVALUE"]),
                                ShapeCodeImage = Convert.ToString(drvCab["CHRSHAPECODE"]),
                                EditFlag = Convert.ToBoolean(drvCab["BITEDIT"]),
                                VisibleFlag = Convert.ToBoolean(drvCab["BITVISIBLE"]),
                                WireType = Convert.ToString(drvCab["CHRWIRETYPE"]),
                                AngleType = Convert.ToString(drvCab["CHRANGLETYPE"]),
                                AngleDir = Convert.ToInt32(drvCab["INTANGLEDIR"]),
                                symmetricIndex = Convert.ToString(drvCab["SYMMETRYINDEX"]),
                                HeghtAngleFormula = Convert.ToString(drvCab["HEIGHTANGLEFORMULA"]),
                                HeghtSuceedAngleFormula = Convert.ToString(drvCab["HEIGHTSUCEEDANGLEFORMULA"]),
                                IntXCord = Convert.ToInt32(drvCab["INTXCOORD"]),
                                IntYCord = Convert.ToInt32(drvCab["INTYCOORD"]),
                                IntZCord = Convert.ToInt32(drvCab["INTZCOORD"]),
                                CustFormula = Convert.ToString(drvCab["VCHCUSTOMFORMULA"]),
                                OffsetAngleFormula = Convert.ToString(drvCab["VCHOFFSETANGLEFORMULA"]),
                                OffsetSuceedAngleFormula = Convert.ToString(drvCab["OFFSETSUCEEDANGLEFORMULA"]),
                                CouplerType = Convert.ToString(drvCab["CSD_INPUT_TYPE"]),
                                ParameterView = Convert.ToString(drvCab["CSD_VIEW"]),
                                RoundOffRange = Convert.ToInt32(drvCab["RoundOffRange"])
                            };
                            listShapeParameterForCab.Add(shapeparam);
                        }
                        // IndexusDistributionCache.SharedCache.Add("CabShapeparamCache", listShapeParameterForCab, DateTime.Today.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings.Get("cacheTimeOut"))));
                    }
                    //}
                    //else
                    //{
                    //    listShapeParameterForCab = (ShapeParameterCollection)IndexusDistributionCache.SharedCache.Get("CabShapeparamCache");
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listShapeParameterForCab;
        }

        /// <summary>
        /// Method to get shape parameters during edit mode.
        /// </summary>
        /// <param name="shapecode"></param>
        /// <returns></returns>
        public ShapeParameterCollection ShapeParameterCabEdit_Get(string shapecode)
        {

            ShapeParameterCollection listShapeParameterForCab = new ShapeParameterCollection();
            DataSet dsShapeParameterForCab = new DataSet();

            try
            {
                //if (IndexusDistributionCache.SharedCache.Get("CabShapeparamCache") == null)
                //{
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@CHRSHAPECODE", shapecode);
                    //dsShapeParameterForCab = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "USP_SHAPEPARAMFORCAB_GET_CUBE");
                    dsShapeParameterForCab = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.SHAPEPARAMFORCAB_GET_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (dsShapeParameterForCab != null && dsShapeParameterForCab.Tables.Count != 0)
                    {
                        foreach (DataRowView drvCab in dsShapeParameterForCab.Tables[0].DefaultView)
                        {
                            ShapeParameter shapeparam = new ShapeParameter
                            {
                                ShapeId = Convert.ToInt32(drvCab["INTSHAPEID"]),
                                ParameterName = Convert.ToString(drvCab["CHRPARAMNAME"]),
                                CriticalIndiacator = Convert.ToString(drvCab["CHRCRITICALIND"]),
                                SequenceNumber = Convert.ToInt16(drvCab["INTPARAMSEQ"]),
                                ParameterValueCab = Convert.ToString(drvCab["PARAMETERVALUE"]),
                                ShapeCodeImage = Convert.ToString(drvCab["CHRSHAPECODE"]),
                                EditFlag = Convert.ToBoolean(drvCab["BITEDIT"]),
                                VisibleFlag = Convert.ToBoolean(drvCab["BITVISIBLE"]),
                                WireType = Convert.ToString(drvCab["CHRWIRETYPE"]),
                                AngleType = Convert.ToString(drvCab["CHRANGLETYPE"]),
                                AngleDir = Convert.ToInt32(drvCab["INTANGLEDIR"]),
                                symmetricIndex = Convert.ToString(drvCab["SYMMETRYINDEX"]),
                                HeghtAngleFormula = Convert.ToString(drvCab["HEIGHTANGLEFORMULA"]),
                                HeghtSuceedAngleFormula = Convert.ToString(drvCab["HEIGHTSUCEEDANGLEFORMULA"]),
                                IntXCord = Convert.ToInt32(drvCab["INTXCOORD"]),
                                IntYCord = Convert.ToInt32(drvCab["INTYCOORD"]),
                                IntZCord = Convert.ToInt32(drvCab["INTZCOORD"]),
                                CustFormula = Convert.ToString(drvCab["VCHCUSTOMFORMULA"]),
                                OffsetAngleFormula = Convert.ToString(drvCab["VCHOFFSETANGLEFORMULA"]),
                                OffsetSuceedAngleFormula = Convert.ToString(drvCab["OFFSETSUCEEDANGLEFORMULA"]),
                                CouplerType = Convert.ToString(drvCab["CSD_INPUT_TYPE"]),
                                ParameterView = Convert.ToString(drvCab["CSD_VIEW"]),
                                RoundOffRange = Convert.ToInt32(drvCab["RoundOffRange"])

                            };
                            listShapeParameterForCab.Add(shapeparam);
                        }
                        //IndexusDistributionCache.SharedCache.Add("CabShapeparamCache", listShapeParameterForCab, DateTime.Today.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings.Get("cacheTimeOut"))));
                    }
                    //}
                    //else
                    //{
                    //    listShapeParameterForCab = (ShapeParameterCollection)IndexusDistributionCache.SharedCache.Get("CabShapeparamCache");
                    //}
                    //dbManager.Open();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listShapeParameterForCab;
        }
        #endregion


        #region "Shape Parameter for Cab"
        /// <summary>
        /// Method to get the shape parameters for OES.
        /// </summary>
        /// <param name="shapeCode"></param>
        /// <returns></returns>
        public List<ShapeParameter> GetShapeParamsOES(string shapeCode)
        {

            List<ShapeParameter> lstShapeParam = new List<ShapeParameter>();
            DataSet dsShapeParameterForCab = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.Add("@SHAPECODE", shapeCode);
                    // dsShapeParameterForCab = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "USP_SHAPEPARAMETERFORCAB_GET_CUBE");
                    dsShapeParameterForCab = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.SHAPEPARAMETERFORCAB_GET_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (dsShapeParameterForCab != null && dsShapeParameterForCab.Tables.Count != 0)
                    {
                        foreach (DataRowView drvCab in dsShapeParameterForCab.Tables[0].DefaultView)
                        {
                            ShapeParameter shapeparam = new ShapeParameter
                            {
                                ShapeId = Convert.ToInt32(drvCab["INTSHAPEID"]),
                                ParameterName = Convert.ToString(drvCab["CHRPARAMNAME"]),
                                CriticalIndiacator = Convert.ToString(drvCab["CHRCRITICALIND"]),
                                SequenceNumber = Convert.ToInt16(drvCab["INTPARAMSEQ"]),
                                ParameterValueCab = Convert.ToString(drvCab["PARAMETERVALUE"]),
                                ShapeCodeImage = Convert.ToString(drvCab["CHRSHAPECODE"]),
                                EditFlag = Convert.ToBoolean(drvCab["BITEDIT"]),
                                VisibleFlag = Convert.ToBoolean(drvCab["BITVISIBLE"]),
                                WireType = Convert.ToString(drvCab["CHRWIRETYPE"]),
                                AngleType = Convert.ToString(drvCab["CHRANGLETYPE"]),
                                AngleDir = Convert.ToInt32(drvCab["INTANGLEDIR"]),
                                symmetricIndex = Convert.ToString(drvCab["SYMMETRYINDEX"]),
                                HeghtAngleFormula = Convert.ToString(drvCab["HEIGHTANGLEFORMULA"]),
                                HeghtSuceedAngleFormula = Convert.ToString(drvCab["HEIGHTSUCEEDANGLEFORMULA"]),
                                IntXCord = Convert.ToInt32(drvCab["INTXCOORD"]),
                                IntYCord = Convert.ToInt32(drvCab["INTYCOORD"]),
                                IntZCord = Convert.ToInt32(drvCab["INTZCOORD"]),
                                CustFormula = Convert.ToString(drvCab["VCHCUSTOMFORMULA"]),
                                OffsetAngleFormula = Convert.ToString(drvCab["VCHOFFSETANGLEFORMULA"]),
                                OffsetSuceedAngleFormula = Convert.ToString(drvCab["OFFSETSUCEEDANGLEFORMULA"]),
                                CouplerType = Convert.ToString(drvCab["CSD_INPUT_TYPE"]),
                                ParameterView = Convert.ToString(drvCab["CSD_VIEW"])
                            };
                            lstShapeParam.Add(shapeparam);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lstShapeParam;
        }
        #endregion
    }
}
