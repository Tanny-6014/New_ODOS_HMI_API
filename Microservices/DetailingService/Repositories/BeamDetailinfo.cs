using Dapper;
using DetailingService.Constants;
using DetailingService.Context;
using DetailingService.Dtos;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DetailingService.Repositories
{
    public class BeamDetailinfo
    {
        private DetailingApplicationContext _dbContext;
        private readonly IConfiguration _configuration;
        //private string connectionString;
        //private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";
        private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";

        # region "Variables"

        public Int32 intGroupMarkId { get; set; }
        public Int32 intProjectId { get; set; }
        public Int32 intTransportModeId { get; set; }
        public Int32 intStructureElementTypeId { get; set; }
        public Int32 sitProducttypeId { get; set; }
        public Int32 SeDetailing { get; set; }
        public Int32 WBSTypeId { get; set; }
        public Int32 CustomerCode { get; set; }
        public Int32 ContractCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerNumber { get; set; }
        public string ContractName { get; set; }
        public string ContractDescription { get; set; }
        public string ProjectCode { get; set; }
        public string StructureElementType { get; set; }
        public string ProductType { get; set; }
        public string GroupMarkingName { get; set; }
        public Int32 BitParentFlag { get; set; }
        public string Projectname { get; set; }
        public Int32 GroupRevNo { get; set; }
        public string Remarks { get; set; }
        public Int32 Parameterset { get; set; }
        public string TransportMode { get; set; }
        public string CopyFrom { get; set; }
        public Int32 ParameterSetNumber { get; set; }
        public Int32 ProjectTypeId { get; set; }
        public Int32 IsCABOnly { get; set; }
        public GroupMark GroupMark { get; set; }
        public SAPMaterial SAPMaterial { get; set; }
        public string SideForCode { get; set; }
        #endregion

        public BeamDetailinfo()
        {

        }
        public BeamDetailinfo(DetailingApplicationContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;

            connectionString = _configuration.GetConnectionString(SystemConstant.DefaultDBConnection);
        }

        # region "Group Mark"

        public List<BeamDetailinfo> CustContProjByGroupID_Get()
        {
            
            DataSet dsCustContProjByGroupID = new DataSet();
            List<BeamDetailinfo> listCustomerProjbyGroupId = new List<BeamDetailinfo> { };
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTGROUPMARKID", this.intGroupMarkId);
                    // dsCustContProjByGroupID = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_CustContProjByGroupID_Get");
                    dsCustContProjByGroupID = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.CustContProjByGroupID_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (dsCustContProjByGroupID != null && dsCustContProjByGroupID.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBeam in dsCustContProjByGroupID.Tables[0].DefaultView)
                        {
                            BeamDetailinfo beamDetailInfo = new BeamDetailinfo
                            {
                                CustomerName = drvBeam["VCHCUSTOMERNAME"].ToString(),
                                ContractName = drvBeam["VCHCONTRACTNUMBER"].ToString(),
                                ProjectCode = drvBeam["VCHPROJECTCODE"].ToString(),
                                StructureElementType = drvBeam["VCHSTRUCTUREELEMENTTYPE"].ToString(),
                                ProductType = drvBeam["VCHPRODUCTTYPE"].ToString(),
                                intStructureElementTypeId = Convert.ToInt32(drvBeam["INTSTRUCTUREELEMENTTYPEID"]),
                                sitProducttypeId = Convert.ToInt32(drvBeam["SITPRODUCTTYPEID"]),
                                intProjectId = Convert.ToInt32(drvBeam["INTPROJECTID"]),
                                SeDetailing = Convert.ToInt32(drvBeam["INTSEDETAILINGID"]),
                                GroupMarkingName = drvBeam["VCHGROUPMARKINGNAME"].ToString(),
                                WBSTypeId = Convert.ToInt32(drvBeam["INTWBSTYPEID"]),
                                CustomerCode = Convert.ToInt32(drvBeam["INTCUSTOMERCODE"]),
                                ContractCode = Convert.ToInt32(drvBeam["INTCONTRACTID"]),
                                BitParentFlag = Convert.ToInt32(drvBeam["BITPARENTFLAG"]),
                                Projectname = drvBeam["VCHPROJECTNAME"].ToString(),
                                ProjectTypeId = Convert.ToInt32(drvBeam["TNTPROJECTTYPEID"]),
                                intTransportModeId = Convert.ToInt32(drvBeam["TNTTRANSPORTMODEID"])
                            };
                            listCustomerProjbyGroupId.Add(beamDetailInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
            return listCustomerProjbyGroupId;
        }

        //changes by vidya 
        public List<BeamDetailinfo> CustContProjID_Get(int ProjectId)
        {
            List<BeamDetailinfo> beamDetailinfos = new List<BeamDetailinfo> { };
            IEnumerable<PopulateHeaderDto> populateHeaderDtos;
            DataSet dsCustContProjID = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTROJECTID", ProjectId);
                    populateHeaderDtos = sqlConnection.Query<PopulateHeaderDto>(SystemConstant.CustContProjID_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (populateHeaderDtos.Count() > 0)
                    {
                        DataTable dt = ConvertToDataTable.ToDataTable(populateHeaderDtos);
                        dsCustContProjID.Tables.Add(dt);

                        if (dsCustContProjID != null && dsCustContProjID.Tables.Count != 0)
                        {
                            foreach (DataRowView drvBeam in dsCustContProjID.Tables[0].DefaultView)
                            {
                                BeamDetailinfo productCode = new BeamDetailinfo
                                {
                                    CustomerCode = Convert.ToInt32(drvBeam["INTCUSTOMERCODE"]),
                                    CustomerName = drvBeam["VCHCUSTOMERNAME"].ToString(),
                                    CustomerNumber = drvBeam["VCHCUSTOMERNO"].ToString(),
                                    ContractCode = Convert.ToInt32(drvBeam["INTCONTRACTID"]),
                                    ContractName = drvBeam["VCHCONTRACTNUMBER"].ToString(),
                                    ContractDescription = drvBeam["VCHNDSCONTRACTDESCRIPTION"].ToString(),
                                    intProjectId = Convert.ToInt32(drvBeam["INTPROJECTID"]),
                                    ProjectCode= drvBeam["VCHPROJECTCODE"].ToString(),
                                    Projectname = drvBeam["VCHPROJECTNAME"].ToString(),

                                };
                                beamDetailinfos.Add(productCode);
                            }
                                         
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return beamDetailinfos;
        }

        public List<BeamDetailinfo> GroupMarkingDetailsByGroupID_Get()
        {
            
            DataSet dsGroupmarkingDetailsByGroupID = new DataSet();
            List<BeamDetailinfo> listGroupmarkingDetailsByGroupID = new List<BeamDetailinfo>();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTGROUPMARKID", this.intGroupMarkId);
                    // dsGroupmarkingDetailsByGroupID = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_GroupmarkingDetailsByGroupID_Get");
                    dsGroupmarkingDetailsByGroupID = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.GroupmarkingDetailsByGroupID_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (dsGroupmarkingDetailsByGroupID != null && dsGroupmarkingDetailsByGroupID.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBeam in dsGroupmarkingDetailsByGroupID.Tables[0].DefaultView)
                        {
                            BeamDetailinfo beamDetailInfo = new BeamDetailinfo
                            {
                                GroupMarkingName = drvBeam["VCHGROUPMARKINGNAME"].ToString(),
                                GroupRevNo = Convert.ToInt32(drvBeam["TNTGROUPREVNO"]),
                                Remarks = drvBeam["VCHREMARKS"].ToString(),
                                Parameterset = Convert.ToInt32(drvBeam["INTPARAMETESET"]),
                                TransportMode = drvBeam["VCHTRANSPORTMODE"].ToString(),
                                CopyFrom = drvBeam["VCHCOPYFROM"].ToString(),
                                ParameterSetNumber = Convert.ToInt32(drvBeam["TNTPARAMSETNUMBER"]),
                                BitParentFlag = Convert.ToInt32(drvBeam["BITPARENTFLAG"]),
                                StructureElementType = Convert.ToString(drvBeam["VCHSTRUCTUREELEMENTTYPE"]),
                                SideForCode = Convert.ToString(drvBeam["SIDEFORCODE"])
                            };
                            listGroupmarkingDetailsByGroupID.Add(beamDetailInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
            return listGroupmarkingDetailsByGroupID;
        }



        public List<BeamDetailinfo> GroupMarkingDetailsByGroupIDCARPET_Get() // FOR CARPET
        {
            
            DataSet dsGroupmarkingDetailsByGroupID = new DataSet();
            List<BeamDetailinfo> listGroupmarkingDetailsByGroupID = new List<BeamDetailinfo>();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTGROUPMARKID", this.intGroupMarkId);
                    // dsGroupmarkingDetailsByGroupID = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_GroupmarkingDetailsByGroupIDCARPET_Get");
                    dsGroupmarkingDetailsByGroupID = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.GroupmarkingDetailsByGroupIDCARPET_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (dsGroupmarkingDetailsByGroupID != null && dsGroupmarkingDetailsByGroupID.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBeam in dsGroupmarkingDetailsByGroupID.Tables[0].DefaultView)
                        {
                            BeamDetailinfo beamDetailInfo = new BeamDetailinfo
                            {
                                GroupMarkingName = drvBeam["VCHGROUPMARKINGNAME"].ToString(),
                                GroupRevNo = Convert.ToInt32(drvBeam["TNTGROUPREVNO"]),
                                Remarks = drvBeam["VCHREMARKS"].ToString(),
                                Parameterset = Convert.ToInt32(drvBeam["INTPARAMETESET"]),
                                TransportMode = drvBeam["VCHTRANSPORTMODE"].ToString(),
                                CopyFrom = drvBeam["VCHCOPYFROM"].ToString(),
                                ParameterSetNumber = Convert.ToInt32(drvBeam["TNTPARAMSETNUMBER"]),
                                BitParentFlag = Convert.ToInt32(drvBeam["BITPARENTFLAG"]),
                                StructureElementType = Convert.ToString(drvBeam["VCHSTRUCTUREELEMENTTYPE"]),
                                SideForCode = Convert.ToString(drvBeam["SIDEFORCODE"])
                            };
                            listGroupmarkingDetailsByGroupID.Add(beamDetailInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
            return listGroupmarkingDetailsByGroupID;
        }

        public int PostedBeamByGroupmark_Get()
        {
            
            int intPostedBeamByGroupmark;
            object output = null;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTGROUPMARKID", this.intGroupMarkId);
                   // intPostedBeamByGroupmark = sqlConnection.QueryFirstOrDefault<int>(SystemConstant.PostedBeamByGroupmark_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    //Convert.ToInt32(dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_PostedBeamByGroupmark_Get"));
                    dynamic result = sqlConnection.Query<int>(SystemConstant.PostedBeamByGroupmark_Get, dynamicParameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
                    output = result;

                    intPostedBeamByGroupmark = Convert.ToInt32(output);
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            
            return intPostedBeamByGroupmark;
        }

        public int GroupMarkPostedValidate_Get(int SeDetailingId, out string errorMessage)
        {

            int intPostedBeamByGroupmark;
            errorMessage = "";
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTSEDETAILINGID", SeDetailingId);
                    intPostedBeamByGroupmark = sqlConnection.QueryFirstOrDefault<int>(SystemConstant.GroupMarkPostedValidate_Get, dynamicParameters, commandType: CommandType.StoredProcedure);



                    //Convert.ToInt32(dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_GroupMarkPostedValidate_Get"));
                    if (Convert.ToInt32(intPostedBeamByGroupmark) == 0)
                    {
                        errorMessage = "POSTED";
                    }
                    else if (Convert.ToInt32(intPostedBeamByGroupmark) == 1)
                    {
                        intPostedBeamByGroupmark = Convert.ToInt32(intPostedBeamByGroupmark);
                    }
                    else
                    {
                        throw new Exception("error");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return intPostedBeamByGroupmark;
        }

        public int GroupMarkPPosting_Regenerate_CARPET(int StructureMarkId, out string errorMessage)
        {
            
            int intPostedBeamByGroupmark;
            errorMessage = "";
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@STRUCTUREELEMENTTYPEID", this.intStructureElementTypeId);
                    dynamicParameters.Add("@INTSTRUCTUREMARKID", StructureMarkId);
                    //intPostedBeamByGroupmark = Convert.ToInt32(dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_GroupMarkPPosting_Regenerate_CARPET"));
                    intPostedBeamByGroupmark = sqlConnection.QueryFirstOrDefault<int>(SystemConstant.GroupMarkPPosting_Regenerate_CARPET, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (Convert.ToInt32(intPostedBeamByGroupmark) == 0)
                    {
                        errorMessage = "POSTED";
                    }
                    else if (Convert.ToInt32(intPostedBeamByGroupmark) == 1)
                    {
                        intPostedBeamByGroupmark = Convert.ToInt32(intPostedBeamByGroupmark);
                    }
                    else
                    {
                        throw new Exception("error");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }          
            return intPostedBeamByGroupmark;
        }

        public int GroupMarkPPosting_Regenerate(int StructureMarkId, out string errorMessage)
        {
            
            int intPostedBeamByGroupmark;
            errorMessage = "";
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@STRUCTUREELEMENTTYPEID", this.intStructureElementTypeId);
                    dynamicParameters.Add("@INTSTRUCTUREMARKID", StructureMarkId);

                    //intPostedBeamByGroupmark = Convert.ToInt32(dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_GroupMarkPPosting_Regenerate"));
                    intPostedBeamByGroupmark = sqlConnection.QueryFirstOrDefault<int>(SystemConstant.GroupMarkPPosting_Regenerate, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (Convert.ToInt32(intPostedBeamByGroupmark) == 0)
                    {
                        errorMessage = "POSTED";
                    }
                    else if (Convert.ToInt32(intPostedBeamByGroupmark) == 1)
                    {
                        intPostedBeamByGroupmark = Convert.ToInt32(intPostedBeamByGroupmark);
                    }
                    else
                    {
                        throw new Exception("error");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
            return intPostedBeamByGroupmark;
        }

        public string WBSAttachedWithGroupMarking_Get()
        {
            
            string PostedWBS = "";
            DataSet dsPostedWBS = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTGROUPMARKID", this.intGroupMarkId);
                    dynamicParameters.Add("@INTSTRUCTUREELEMENTTYPEID", this.intStructureElementTypeId);
                    dynamicParameters.Add("@SITPRODUCTTYPEID", this.sitProducttypeId);
                    //dsPostedWBS = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_WBSAttachedWithGroupMarking_Get");
                    dsPostedWBS = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.WBSAttachedWithGroupMarking_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (dsPostedWBS != null && dsPostedWBS.Tables.Count != 0)
                    {
                        if (dsPostedWBS.Tables[0].Rows.Count > 0)
                        {
                            PostedWBS = "This GroupMark is posted with the following WBS :" + "\n";
                            foreach (DataRowView drvPostedWBS in dsPostedWBS.Tables[0].DefaultView)
                            {
                                if (Convert.ToString(drvPostedWBS["vchWBS1"]) != "")
                                {
                                    PostedWBS += Convert.ToString(drvPostedWBS["vchWBS1"]);
                                }
                                if (Convert.ToString(drvPostedWBS["vchWBS2"]) != "")
                                {
                                    PostedWBS += " - " + Convert.ToString(drvPostedWBS["vchWBS2"]);
                                }
                                if (Convert.ToString(drvPostedWBS["vchWBS3"]) != "")
                                {
                                    PostedWBS += " - " + Convert.ToString(drvPostedWBS["vchWBS3"]);
                                }
                                if (Convert.ToString(drvPostedWBS["vchWBS4"]) != "")
                                {
                                    PostedWBS += " - " + Convert.ToString(drvPostedWBS["vchWBS4"]);
                                }
                                if (Convert.ToString(drvPostedWBS["vchWBS5"]) != "")
                                {
                                    PostedWBS += " - " + Convert.ToString(drvPostedWBS["vchWBS5"]);
                                }
                                PostedWBS += "\n";
                            }
                        }
                        else
                        {
                            PostedWBS = "This GroupMark is not posted with any WBS";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return PostedWBS;
        }

        public string UnPostedWBSAttachedWithGroupMarking_Get()
        {
            
            string PostedWBS = "";
            DataSet dsPostedWBS = new DataSet();
            IEnumerable<BeamDetailinfo> beamDetailinfos; //new IEnumerable<List<ProjectMaster>>();

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTGROUPMARKID", this.intGroupMarkId);
                    //dsPostedWBS = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "Usp_unpostedwbsattachedwithgroupmarking_get");
                    beamDetailinfos = sqlConnection.Query<BeamDetailinfo>(SystemConstant.unpostedwbsattachedwithgroupmarking_get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (dsPostedWBS != null && dsPostedWBS.Tables.Count != 0)
                    {
                        if (dsPostedWBS.Tables[0].Rows.Count > 0)
                        {
                            PostedWBS = "This GroupMark is attached with the following WBS :";
                            foreach (DataRowView drvPostedWBS in dsPostedWBS.Tables[0].DefaultView)
                            {
                                if (Convert.ToString(drvPostedWBS["vchWBS1"]) != "")
                                {
                                    PostedWBS += Convert.ToString(drvPostedWBS["vchWBS1"]);
                                }
                                if (Convert.ToString(drvPostedWBS["vchWBS2"]) != "")
                                {
                                    PostedWBS += " - " + Convert.ToString(drvPostedWBS["vchWBS2"]);
                                }
                                if (Convert.ToString(drvPostedWBS["vchWBS3"]) != "")
                                {
                                    PostedWBS += " - " + Convert.ToString(drvPostedWBS["vchWBS3"]);
                                }
                                if (Convert.ToString(drvPostedWBS["vchWBS4"]) != "")
                                {
                                    PostedWBS += " - " + Convert.ToString(drvPostedWBS["vchWBS4"]);
                                }
                                if (Convert.ToString(drvPostedWBS["vchWBS5"]) != "")
                                {
                                    PostedWBS += " - " + Convert.ToString(drvPostedWBS["vchWBS5"]);
                                }
                                PostedWBS += "/";
                            }
                        }
                        else
                        {
                            PostedWBS = "This GroupMark is not posted with any WBS";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
            return PostedWBS;
        }

        #endregion

        #region "CAB Detailing"

        public List<BeamDetailinfo> CABCustContProjByGroupID_Get()
        {
            
            DataSet dsCABCustContProjByGroupID = new DataSet();
            List<BeamDetailinfo> listCABCustomerProjbyGroupId = new List<BeamDetailinfo> { };
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTGROUPMARKID", this.intGroupMarkId);
                    // dsCABCustContProjByGroupID = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_CABCustContProjByGroupID_Get");
                    dsCABCustContProjByGroupID = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.CABCustContProjByGroupID_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (dsCABCustContProjByGroupID != null && dsCABCustContProjByGroupID.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBeam in dsCABCustContProjByGroupID.Tables[0].DefaultView)
                        {
                            if (Convert.ToInt32(drvBeam["INTSEDETAILINGID"]) != 0)
                            {
                                BeamDetailinfo beamDetailInfo = new BeamDetailinfo
                                {
                                    CustomerName = drvBeam["VCHCUSTOMERNAME"].ToString(),
                                    ContractName = drvBeam["VCHCONTRACTNUMBER"].ToString(),
                                    ProjectCode = drvBeam["VCHPROJECTCODE"].ToString(),
                                    StructureElementType = drvBeam["VCHSTRUCTUREELEMENTTYPE"].ToString(),
                                    ProductType = drvBeam["VCHPRODUCTTYPE"].ToString(),
                                    intStructureElementTypeId = Convert.ToInt32(drvBeam["INTSTRUCTUREELEMENTTYPEID"]),
                                    sitProducttypeId = Convert.ToInt32(drvBeam["SITPRODUCTTYPEID"]),
                                    intProjectId = Convert.ToInt32(drvBeam["INTPROJECTID"]),
                                    SeDetailing = Convert.ToInt32(drvBeam["INTSEDETAILINGID"]),
                                    GroupMarkingName = drvBeam["VCHGROUPMARKINGNAME"].ToString(),
                                    WBSTypeId = Convert.ToInt32(drvBeam["INTWBSTYPEID"]),
                                    CustomerCode = Convert.ToInt32(drvBeam["INTCUSTOMERCODE"]),
                                    ContractCode = Convert.ToInt32(drvBeam["INTCONTRACTID"]),
                                    BitParentFlag = Convert.ToInt32(drvBeam["BITPARENTFLAG"]),
                                    Projectname = drvBeam["VCHPROJECTNAME"].ToString(),
                                    TransportMode = drvBeam["VCHTRANSPORTMODE"].ToString()
                                };
                                listCABCustomerProjbyGroupId.Add(beamDetailInfo);
                            }
                            else
                            {
                                BeamDetailinfo beamDetailInfo = new BeamDetailinfo
                                {
                                    SeDetailing = Convert.ToInt32(drvBeam["INTSEDETAILINGID"])
                                };
                                listCABCustomerProjbyGroupId.Add(beamDetailInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }           
            return listCABCustomerProjbyGroupId;

        }

        public List<BeamDetailinfo> CABGroupMarkingDetailsByGroupID_Get()
        {
            
            DataSet dsGroupmarkingDetailsByGroupID = new DataSet();
            List<BeamDetailinfo> listCABGroupmarkingDetailsByGroupID = new List<BeamDetailinfo>();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTGROUPMARKID", this.intGroupMarkId);
                    // dsGroupmarkingDetailsByGroupID = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_CABGroupmarkingDetailsByGroupID_Get");
                    dsGroupmarkingDetailsByGroupID = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.CABGroupmarkingDetailsByGroupID_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (dsGroupmarkingDetailsByGroupID != null && dsGroupmarkingDetailsByGroupID.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBeam in dsGroupmarkingDetailsByGroupID.Tables[0].DefaultView)
                        {
                            BeamDetailinfo beamDetailInfo = new BeamDetailinfo
                            {
                                GroupMarkingName = drvBeam["VCHGROUPMARKINGNAME"].ToString(),
                                GroupRevNo = Convert.ToInt32(drvBeam["TNTGROUPREVNO"]),
                                Remarks = drvBeam["VCHREMARKS"].ToString(),
                                CopyFrom = drvBeam["VCHCOPYFROM"].ToString(),
                                BitParentFlag = Convert.ToInt32(drvBeam["BITPARENTFLAG"]),
                                StructureElementType = Convert.ToString(drvBeam["VCHSTRUCTUREELEMENTTYPE"])
                            };
                            listCABGroupmarkingDetailsByGroupID.Add(beamDetailInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }           

            return listCABGroupmarkingDetailsByGroupID;
        }

        #endregion

        #region "PRC Detailing"

        public List<BeamDetailinfo> GetCustContProjByProjectID(int intProjectID)        // To Get PRC Header & Group Mark Details By Project Id
        {
            List<BeamDetailinfo> listBeamDetailingInfo = new List<BeamDetailinfo>();
            
            DataSet dsCustContProj = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTROJECTID", intProjectID);
                    // dsCustContProj = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_CustContProjID_Get");
                    dsCustContProj = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.CustContProjID_Get, dynamicParameters, commandType: CommandType.StoredProcedure);


                    if (dsCustContProj != null && dsCustContProj.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBeam in dsCustContProj.Tables[0].DefaultView)
                        {
                            BeamDetailinfo beamDetailInfo = new BeamDetailinfo
                            {
                                CustomerName = drvBeam["VCHCUSTOMERNAME"].ToString(),
                                ContractName = drvBeam["VCHCONTRACTNUMBER"].ToString(),
                                ProjectCode = drvBeam["VCHPROJECTCODE"].ToString(),
                                intProjectId = Convert.ToInt32(drvBeam["INTPROJECTID"]),
                                CustomerCode = Convert.ToInt32(drvBeam["INTCUSTOMERCODE"]),
                                ContractCode = Convert.ToInt32(drvBeam["INTCONTRACTID"]),
                                Projectname = drvBeam["VCHPROJECTNAME"].ToString()
                            };
                            listBeamDetailingInfo.Add(beamDetailInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listBeamDetailingInfo;
        }

        public List<BeamDetailinfo> GetCustContProjByGroupMarkID(out List<GroupMark> listHeaderDetailsByGroupId)                      // To Get PRC Header & Group Mark Details By Group Mark Id 
        {
            
            DataSet dsCustContProjByGroupID = new DataSet();
            listHeaderDetailsByGroupId = new List<GroupMark> { };
            List<BeamDetailinfo> listCustomerProjbyGroupId = new List<BeamDetailinfo> { };
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();                   

                    SqlCommand cmd = new SqlCommand(SystemConstant.GroupMarkingHeaderDetailsByGroupMarkIdForPRC_Get, sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@INTGROUPMARKID", this.intGroupMarkId));

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dsCustContProjByGroupID);
                    
                    if (dsCustContProjByGroupID != null && dsCustContProjByGroupID.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBeam in dsCustContProjByGroupID.Tables[0].DefaultView)
                        {
                            BeamDetailinfo beamDetailInfo = new BeamDetailinfo
                            {
                                CustomerName = drvBeam["VCHCUSTOMERNAME"].ToString(),
                                ContractName = drvBeam["VCHCONTRACTNUMBER"].ToString(),
                                ProjectCode = drvBeam["VCHPROJECTCODE"].ToString(),
                                StructureElementType = drvBeam["VCHSTRUCTUREELEMENTTYPE"].ToString(),
                                ProductType = drvBeam["VCHPRODUCTTYPE"].ToString(),
                                intStructureElementTypeId = Convert.ToInt32(drvBeam["INTSTRUCTUREELEMENTTYPEID"]),
                                sitProducttypeId = Convert.ToInt32(drvBeam["SITPRODUCTTYPEID"]),
                                intProjectId = Convert.ToInt32(drvBeam["INTPROJECTID"]),
                                SeDetailing = Convert.ToInt32(drvBeam["INTSEDETAILINGID"]),
                                GroupMarkingName = drvBeam["VCHGROUPMARKINGNAME"].ToString(),
                                WBSTypeId = Convert.ToInt32(drvBeam["INTWBSTYPEID"]),
                                CustomerCode = Convert.ToInt32(drvBeam["INTCUSTOMERCODE"]),
                                ContractCode = Convert.ToInt32(drvBeam["INTCONTRACTID"]),
                                BitParentFlag = Convert.ToInt32(drvBeam["BITPARENTFLAG"]),
                                Projectname = drvBeam["VCHPROJECTNAME"].ToString(),
                                Remarks = drvBeam["VCHREMARKS"].ToString(),
                                intTransportModeId = Convert.ToInt32(drvBeam["TNTTRANSPORTMODEID"]),
                                ProjectTypeId = Convert.ToInt32(drvBeam["TNTPROJECTTYPEID"]),
                                IsCABOnly = Convert.ToInt32(drvBeam["ISONLYCAB"])
                            };
                            listCustomerProjbyGroupId.Add(beamDetailInfo);
                        }

                        foreach (DataRowView drvBeam in dsCustContProjByGroupID.Tables[1].DefaultView)
                        {
                            SAPMaterial sapMaterial = new SAPMaterial();
                            {
                                sapMaterial.MaterialCodeID = Convert.ToInt32(drvBeam["INTSAPMATERIALCODEID"]);
                            }
                            SAPMaterial = sapMaterial;
                            GroupMark groupMark = new GroupMark
                            {
                                DefaultWidth = Convert.ToInt32(drvBeam["WIDTH"]),
                                DefaultLength = Convert.ToInt32(drvBeam["LENGTH"]),
                                DefaultDepth = Convert.ToInt32(drvBeam["DEPTH"]),
                                AssemblyInd = Convert.ToInt32(drvBeam["ASSEMBLYINDICATOR"]),
                                ParentStructureMarkId = Convert.ToInt32(drvBeam["INTSTRUCTUREMARKID"]),
                                SAPMaterial = sapMaterial
                            };
                            listHeaderDetailsByGroupId.Add(groupMark);
                        }
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
            return listCustomerProjbyGroupId;

        }

        #endregion

    }
}
