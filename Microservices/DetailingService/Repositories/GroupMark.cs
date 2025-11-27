using Dapper;
using DetailingService.Constants;
using DetailingService.Context;
using Microsoft.Data.SqlClient;
using DetailingService.Repositories;
using DetailingService.Interfaces;
using System.Data;
using DetailingService.Dtos;

namespace DetailingService.Repositories
{
    public class GroupMark
    {
        private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";
        //private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";
        #region "Properties"
        public Int32 GroupRevisionNumber { get; set; }
        public Int32 ProjectId { get; set; }
        public Int32 WBSTypeId { get; set; }
        public Int32 StructureElementTypeId { get; set; }
        public Int32 SitProductTypeId { get; set; }
        public Int32 ParameterSetNumber { get; set; }
        public Int32 CreatedUserId { get; set; }
        public Int32 GroupMarkId { get; set; }
        public Int32 WBSElementId { get; set; }

        public int SeDetailId { get; set; }
        public int? DefaultWidth { get; set; }
        public int? MinWidth { get; set; }
        public int? MaxWidth { get; set; }

        public int? DefaultDepth { get; set; }
        public int? MinDepth { get; set; }
        public int? MaxDepth { get; set; }

        public int? DefaultLength { get; set; }
        public int? MinLength { get; set; }
        public int? MaxLength { get; set; }

        public int AssemblyInd { get; set; }
        public int ParentStructureMarkId { get; set; }
        public int IsCABOnly { get; set; }

        public string? Remarks { get; set; }
        public string InsertFlag { get; set; }
        public string CreatedUserName { get; set; }
        public string GroupMarkingName { get; set; }

        public SAPMaterial SAPMaterial { get; set; }
        public ParameterSet transport { get; set; }
        public ParameterSet ParamValues { get; set; }

        public ProductCode ProductCode { get; set; } // Added by AK for Core Cage

        public string SiderForCode { get; set; }

        //added for CABDE by TCS on 04.07.2016
        public int isCABDE { get; set; }

        public int? SelectionId { get; set; }
        //added for CABDE by TCS on 04.07.2016
        #endregion

        public GroupMark()
        {
        }
       

        #region "Save groupmarking details"

        public bool SaveGroupMark()
        {
            int intDuplicate = 0;
            bool isSuccess = false;


            try
            {

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@TNTGROUPREVNO", GroupRevisionNumber);
                    dynamicParameters.Add("@INTPROJECTID", ProjectId);
                    dynamicParameters.Add("@INTWBSTYPEID", WBSTypeId);
                    dynamicParameters.Add("@INTSTRUCTUREELEMENTTYPEID", StructureElementTypeId);
                    dynamicParameters.Add("@SITPRODUCTTYPEID", SitProductTypeId);
                    dynamicParameters.Add("@VCHGROUPMARKINGNAME", GroupMarkingName);
                    dynamicParameters.Add("@TNTPARAMSETNUMBER", ParameterSetNumber);
                    dynamicParameters.Add("@TNTTRANSPORTID", transport == null ? 0 : transport.TransportID);
                    dynamicParameters.Add("@ISONLYCAB", IsCABOnly);
                    dynamicParameters.Add("@VCHREMARKS", Remarks);
                    dynamicParameters.Add("@INTCREATEDUID", CreatedUserId);
                    dynamicParameters.Add("@VCHCREATEDUNAME", CreatedUserName);
                    dynamicParameters.Add("@INTGROUPMARKID", GroupMarkId);
                    dynamicParameters.Add("@SIDERFORCODE", SiderForCode == null ? "" : SiderForCode);

                  

                    dynamic result = sqlConnection.Query<string>(SystemConstant.GroupMarking_Insert_PV, dynamicParameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
                    object objDuplicate = result;
                    this.InsertFlag = objDuplicate.ToString().Substring(0, 1);
                    this.GroupMarkId = Convert.ToInt32(objDuplicate.ToString().Substring(1));
                    if(GroupMarkId==0)
                    {
                        isSuccess = false;
                    }
                    else
                    {
                    isSuccess = true;

                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
          
            return isSuccess;
        }

        public List<GroupMark> SavePRCGroupMarkingDetails(int? Selector_id)                                              // Save PRC Group Marking Details...
        {
            List<GroupMark> listGroupMark = new List<GroupMark>();
          
            DataSet dsGroupMark = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    SqlCommand cmd = new SqlCommand(SystemConstant.GroupMarkingPRCDetails_Insert_PV, sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                   
                    cmd.Parameters.Add(new SqlParameter("@INTGROUPMARKID", this.GroupMarkId));
                    cmd.Parameters.Add(new SqlParameter("@TNTGROUPREVNO", this.GroupRevisionNumber));
                    cmd.Parameters.Add(new SqlParameter("@INTPROJECTID", this.ProjectId));
                    cmd.Parameters.Add(new SqlParameter("@INTWBSTYPEID ", this.WBSTypeId));
                    cmd.Parameters.Add(new SqlParameter("@INTSTRUCTUREELEMENTTYPEID", this.StructureElementTypeId));
                    cmd.Parameters.Add(new SqlParameter("@SITPRODUCTTYPEID", this.SitProductTypeId));
                    cmd.Parameters.Add(new SqlParameter("@VCHGROUPMARKINGNAME", this.GroupMarkingName));
                    cmd.Parameters.Add(new SqlParameter("@TNTPARAMSETNUMBER", this.ParameterSetNumber));
                    cmd.Parameters.Add(new SqlParameter("@VCHREMARKS", this.Remarks));
                    cmd.Parameters.Add(new SqlParameter("@INTCREATEDUID", this.CreatedUserId));
                    cmd.Parameters.Add(new SqlParameter("@VCHCREATEDUNAME", this.CreatedUserName));
                    cmd.Parameters.Add(new SqlParameter("@INTSAPMATERIALCODEID", SAPMaterial.MaterialCodeID));
                    cmd.Parameters.Add(new SqlParameter("@NUMBEAMWIDTH", this.DefaultWidth));
                    cmd.Parameters.Add(new SqlParameter("@NUMBEAMDEPTH", this.DefaultDepth));
                    cmd.Parameters.Add(new SqlParameter("@INTCLEARSPAN", this.DefaultLength));
                    cmd.Parameters.Add(new SqlParameter("@BITASSINDICATOR", this.AssemblyInd));
                    cmd.Parameters.Add(new SqlParameter("@INTSTRUCTUREMARKID", this.ParentStructureMarkId));
                    cmd.Parameters.Add(new SqlParameter("@PRODUCEINDICATOR", "YES"));
                   // cmd.Parameters.Add(new SqlParameter("@TRANSPORTID", 0));
                    cmd.Parameters.Add(new SqlParameter("@ISCABONLY", this.IsCABOnly));
                    cmd.Parameters.Add(new SqlParameter("@SELECTORID", Selector_id));

                    

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dsGroupMark);


                    //dsGroupMark = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_GroupMarkingPRCDetails_Insert_PV")); //modified for ship to party
                   // dsGroupMark = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.GroupMarkingPRCDetails_Insert_PV, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (dsGroupMark != null && dsGroupMark.Tables.Count != 0)
                    {
                        foreach (DataRowView drvPRCGroupMark in dsGroupMark.Tables[0].DefaultView)
                        {
                            GroupMark groupMark = new GroupMark
                            {
                                InsertFlag = drvPRCGroupMark[0].ToString(),
                                GroupMarkId = Convert.ToInt32(drvPRCGroupMark[1]),
                                ParentStructureMarkId = Convert.ToInt32(drvPRCGroupMark[2])
                            };
                            listGroupMark.Add(groupMark);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listGroupMark;
        }

        #endregion

        #region "Save SE Detailing"

        public bool SaveSeDetail()
        {
            bool isSuccess = false;

            try
            {

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add( "@INTGROUPMARKID", this.GroupMarkId);
                    dynamicParameters.Add( "@INTSTRUCTUREELEMENTTYPEID", this.StructureElementTypeId);
                    dynamicParameters.Add( "@SITPRODUCTTYPEID", this.SitProductTypeId);
                    dynamicParameters.Add( "@TNTPARAMSETNUMBER", this.ParameterSetNumber);
                   // object SeDetailId = dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_SeLevelDetail_Insert");
                    object SeDetailId = sqlConnection.QueryFirstOrDefault<object>(SystemConstant.SeLevelDetail_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);

                    this.SeDetailId = Convert.ToInt32(SeDetailId);
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

        #region "Save WBS Details"

        public bool SaveWBSDetail()
        {
            // Int32 intDuplicate = 0;
            bool isSuccess = false;

            try
            {

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intGroupMarkId", GroupMarkId);
                    dynamicParameters.Add("@tntGroupRevNo", GroupRevisionNumber);
                    dynamicParameters.Add("@intWBSElementId", WBSElementId);
                    //dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_WBSDetails_Insert");
                    sqlConnection.QueryFirstOrDefault(SystemConstant.WBSDetails_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);

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

        #region "Save & update GM parameter"

        public bool GroupMarkingParamset_Update(int SeDetailId, int ParamSetNumber)
        {
            // Int32 intDuplicate = 0;
            bool isSuccess = false;

            try
            {

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@SEDETAILINGID", SeDetailId);
                    dynamicParameters.Add("@TNTPARAMSETNUMBER", ParamSetNumber);
                    //dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_GroupMarkingParamset_Update");
                    sqlConnection.QueryFirstOrDefault(SystemConstant.GroupMarkingParamset_Update, dynamicParameters, commandType: CommandType.StoredProcedure);

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

        #region "Filter GM"

        public List<GroupMark> FilterGroupMarkName_Get(string enteredText, int ProjectId, int StrucutreElementTypeId, string ProductType)         // For Beam Detailing               
        {
          
            List<GroupMark> listGroupMarkName = new List<GroupMark> { };

            DataSet dsFilterGroupMarkName = new DataSet();

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add( "@VCHENTEREDTEXT", enteredText);
                    dynamicParameters.Add( "@PROJECTID", ProjectId);
                    dynamicParameters.Add( "@PRODUCTTYPE", ProductType);
                    dynamicParameters.Add( "@STRUCTUREELEMENTTYPEID", StrucutreElementTypeId);
                  //  dsFilterGroupMarkName = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_FilterGroupMarkName_Get");
                    dsFilterGroupMarkName = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.FilterGroupMarkName_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (dsFilterGroupMarkName != null && dsFilterGroupMarkName.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBeam in dsFilterGroupMarkName.Tables[0].DefaultView)
                        {

                            GroupMark groupMark = new GroupMark
                            {
                                GroupMarkingName = drvBeam["VCHGROUPMARKINGNAME"].ToString(),
                                GroupRevisionNumber = Convert.ToInt32(drvBeam["TNTGROUPREVNO"]),
                                Remarks = drvBeam["VCHREMARKS"].ToString(),
                                ParameterSetNumber = Convert.ToInt32(drvBeam["TNTPARAMSETNUMBER"]),
                                SitProductTypeId = Convert.ToInt32(drvBeam["SITPRODUCTTYPEID"]),
                                StructureElementTypeId = Convert.ToInt32(drvBeam["INTSTRUCTUREELEMENTTYPEID"])
                            };

                            listGroupMarkName.Add(groupMark);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
            return listGroupMarkName;

        }

        public List<GroupMark> FilterGroupMarkNameForPRC_Get(string enteredText, int ProjectId, int StrucutreElementTypeId)   // For PRC Detailing                
        {
          
            List<GroupMark> listGroupMarkName = new List<GroupMark> { };

            DataSet dsFilterGroupMarkName = new DataSet();

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add( "@VCHENTEREDTEXT", enteredText);
                    dynamicParameters.Add( "@PROJECTID", ProjectId);
                    dynamicParameters.Add( "@STRUCTUREELEMENTTYPEID", StrucutreElementTypeId);
                  //  dsFilterGroupMarkName = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_FilterGroupMarkNameForPRC_Get");
                    dsFilterGroupMarkName = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.FilterGroupMarkNameForPRC_Get, dynamicParameters, commandType: CommandType.StoredProcedure);


                    if (dsFilterGroupMarkName != null && dsFilterGroupMarkName.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBeam in dsFilterGroupMarkName.Tables[0].DefaultView)
                        {

                            GroupMark groupMark = new GroupMark
                            {
                                GroupMarkingName = drvBeam["VCHGROUPMARKINGNAME"].ToString(),
                                GroupRevisionNumber = Convert.ToInt32(drvBeam["TNTGROUPREVNO"]),
                                Remarks = drvBeam["VCHREMARKS"].ToString(),
                                ParameterSetNumber = Convert.ToInt32(drvBeam["TNTPARAMSETNUMBER"]),
                                SitProductTypeId = Convert.ToInt32(drvBeam["SITPRODUCTTYPEID"]),
                                StructureElementTypeId = Convert.ToInt32(drvBeam["INTSTRUCTUREELEMENTTYPEID"])
                            };

                            listGroupMarkName.Add(groupMark);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
            return listGroupMarkName;

        }

        public List<GroupMark> FilterGroupMarkNameForSideForCode_Get(string enteredText, int ProjectId, int StrucutreElementTypeId, string ProductType)         // For Beam Detailing               
        {
          
            List<GroupMark> listGroupMarkName = new List<GroupMark> { };

            DataSet dsFilterGroupMarkName = new DataSet();

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add( "@VCHENTEREDTEXT", enteredText);
                    dynamicParameters.Add( "@PROJECTID", ProjectId);
                    dynamicParameters.Add( "@PRODUCTTYPE", ProductType);
                    dynamicParameters.Add( "@STRUCTUREELEMENTTYPEID", StrucutreElementTypeId);
                   // dsFilterGroupMarkName = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_FilterGroupMarkNameForSideFor_Get");
                    dsFilterGroupMarkName = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.FilterGroupMarkNameForSideFor_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (dsFilterGroupMarkName != null && dsFilterGroupMarkName.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBeam in dsFilterGroupMarkName.Tables[0].DefaultView)
                        {

                            GroupMark groupMark = new GroupMark
                            {
                                GroupMarkId = Convert.ToInt32(drvBeam["INTGROUPMARKID"]),
                                GroupMarkingName = drvBeam["VCHGROUPMARKINGNAME"].ToString(),
                                GroupRevisionNumber = Convert.ToInt32(drvBeam["TNTGROUPREVNO"]),
                                Remarks = drvBeam["VCHREMARKS"].ToString(),
                                SiderForCode = drvBeam["SIDEFORCODE"].ToString(),
                                ParameterSetNumber = Convert.ToInt32(drvBeam["TNTPARAMSETNUMBER"]),
                                SitProductTypeId = Convert.ToInt32(drvBeam["SITPRODUCTTYPEID"]),
                                StructureElementTypeId = Convert.ToInt32(drvBeam["INTSTRUCTUREELEMENTTYPEID"])
                            };

                            listGroupMarkName.Add(groupMark);
                        }
                    }

                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
           
            return listGroupMarkName;

        }
        #endregion

        #region "Default Values for PRC"

        public List<GroupMark> GetPRCDefaultValuesByStructureElementId(int intStructureElementId)                              // Get PRC DefaultValues By Structure Element Id
        {
          
            List<GroupMark> listGroupMark = new List<GroupMark> { };
            DataSet dsPRCDefaultValues = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTSTRUCTUREELEMENTID", intStructureElementId);
                    //dsPRCDefaultValues = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_PRCDefaultByStructureElementId_Get");
                    dsPRCDefaultValues = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.PRCDefaultByStructureElementId_Get, dynamicParameters, commandType: CommandType.StoredProcedure);



                    if (dsPRCDefaultValues != null && dsPRCDefaultValues.Tables.Count != 0)
                    {
                        foreach (DataRowView drPRC in dsPRCDefaultValues.Tables[0].DefaultView)
                        {
                            GroupMark groupMark = new GroupMark
                            {
                                DefaultDepth = Convert.ToInt32(drPRC["DEPTH"]),
                                MinDepth = Convert.ToInt32(drPRC["INTMINENVELOPHEIGHT"]),
                                MaxDepth = Convert.ToInt32(drPRC["INTMAXENVELOPHEIGHT"]),
                                DefaultLength = Convert.ToInt32(drPRC["LENGTH"]),
                                MinLength = Convert.ToInt32(drPRC["INTMINENVELOPLENGTH"]),
                                MaxLength = Convert.ToInt32(drPRC["INTMAXENVELOPLENGTH"]),
                                DefaultWidth = Convert.ToInt32(drPRC["WIDTH"]),
                                MinWidth = Convert.ToInt32(drPRC["INTMINENVELOPWIDTH"]),
                                MaxWidth = Convert.ToInt32(drPRC["INTMAXENVELOPWIDTH"])
                            };
                            listGroupMark.Add(groupMark);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
            return listGroupMark;
        }

        #endregion

        #region "Get PRC Header Details"

        public List<GroupMark> GetPRCHeaderValuesByGroupMarkID(int intGroupMarkId)
        {
          
            DataSet dsCustContProjByGroupID = new DataSet();
            List<GroupMark> listPRCHeaderbyGroupMarkId = new List<GroupMark> { };
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand cmd = new SqlCommand(SystemConstant.PRCParentHeaderDetailsByGroupMarkID_Get, sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@intGroupMarkingId", intGroupMarkId));                   

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dsCustContProjByGroupID);
                    int MaterialCodeID = 0;



                    if (dsCustContProjByGroupID != null && dsCustContProjByGroupID.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBeam in dsCustContProjByGroupID.Tables[0].DefaultView)
                        {
                           

                            GroupMark groupMark = new GroupMark
                            {
                                GroupMarkId = Convert.ToInt32(drvBeam["INTGROUPMARKID"]),
                                SAPMaterial = new SAPMaterial { MaterialCodeID = drvBeam["INTSAPMATERIALCODEID"].Equals(System.DBNull.Value) ? 0: Convert.ToInt32(drvBeam["INTSAPMATERIALCODEID"]) },
                                DefaultWidth = drvBeam["WIDTH"].Equals(System.DBNull.Value) ? 0: Convert.ToInt32(drvBeam["WIDTH"]),
                                DefaultLength = drvBeam["LENGTH"].Equals(System.DBNull.Value) ? 0:  Convert.ToInt32(drvBeam["LENGTH"]),
                                DefaultDepth = drvBeam["DEPTH"].Equals(System.DBNull.Value) ? 0 : Convert.ToInt32(drvBeam["DEPTH"]),
                                AssemblyInd =  drvBeam["ASSEMBLYINDICATOR"].Equals(System.DBNull.Value) ? 0 : Convert.ToInt32(drvBeam["ASSEMBLYINDICATOR"]),
                                ParentStructureMarkId = drvBeam["INTPARENTSTRUCTUREMARKID"].Equals(System.DBNull.Value) ? 0 : Convert.ToInt32(drvBeam["INTPARENTSTRUCTUREMARKID"]),
                                SelectionId = drvBeam["SelectionId"].Equals(System.DBNull.Value) ? 0 : Convert.ToInt32(drvBeam["SelectionId"])
                            };
                            listPRCHeaderbyGroupMarkId.Add(groupMark);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listPRCHeaderbyGroupMarkId;
        }

        #endregion

        #region "Copy Group Marking..."

        public List<GetRevisionAndParamValuesDto> GetRevisionAndParameterValuesByGroupMarkName()
        {
          
            DataSet dsRevNoParamValuesByGroupMarkName = new DataSet();
            List<GetRevisionAndParamValuesDto> groupMarkList = new List<GetRevisionAndParamValuesDto>();
            IEnumerable<GetRevisionAndParamValuesDto> getRevisionAndParams;           
           
              try
                {
                  using (var sqlConnection = new SqlConnection(connectionString))
                  {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add( "@PROJECTID", this.ProjectId);
                    dynamicParameters.Add( "@STRUCTUREELEMENTID", this.StructureElementTypeId);
                    dynamicParameters.Add( "@PRODUCTTYPEID", this.SitProductTypeId);
                    dynamicParameters.Add( "@VCHGROUPMARKINGNAME", this.GroupMarkingName);
                    // dsRevNoParamValuesByGroupMarkName = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "USP_COPYGMPARAMVALUESBYGROUPMARKNAME_GET_CUBE");
                    getRevisionAndParams = sqlConnection.Query<GetRevisionAndParamValuesDto>(SystemConstant.COPYGMPARAMVALUESBYGROUPMARKNAME_GET_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);

                    groupMarkList = getRevisionAndParams.ToList();

                    //if (groupMarks.Count() > 0)
                    //{
                    //    DataTable dt = ConvertToDataTable.ToDataTable(groupMarks);
                    //    dsRevNoParamValuesByGroupMarkName.Tables.Add(dt);
                    //}
                    //if (dsRevNoParamValuesByGroupMarkName != null && dsRevNoParamValuesByGroupMarkName.Tables.Count != 0)
                    //{
                    //    foreach (DataRowView drvParamValues in dsRevNoParamValuesByGroupMarkName.Tables[0].DefaultView)
                    //    {
                    //        GroupMark groupMark = new GroupMark
                    //        {
                    //            GroupMarkId = Convert.ToInt32(drvParamValues["INTGROUPMARKID"]),
                    //            GroupMarkingName = drvParamValues["VCHGROUPMARKINGNAME"].ToString(),
                    //            GroupRevisionNumber = Convert.ToInt32(drvParamValues["TNTGROUPREVNO"]),
                    //            Remarks = drvParamValues["VCHREMARKS"].ToString(),
                    //            isCABDE = Convert.ToInt32(drvParamValues["CABDEflag"]) //Added for CABDE by TCS on 06.07.2016

                    //        };
                    //        ParameterSet parameterSet = new ParameterSet
                    //        {
                    //            ParameterSetNumber = Convert.ToInt32(drvParamValues["INTPARAMETESET"]),
                    //            ParameterSetId = Convert.ToInt32(drvParamValues["TNTPARAMSETNUMBER"]),
                    //            TopCover = Convert.ToInt32(drvParamValues["SITTOPCOVER"]),
                    //            BottomCover = Convert.ToInt32(drvParamValues["SITBOTTOMCOVER"]),
                    //            LeftCover = Convert.ToInt32(drvParamValues["SITLEFTCOVER"]),
                    //            RightCover = Convert.ToInt32(drvParamValues["SITRIGHTCOVER"])
                    //        };
                    //        groupMark.ParamValues = parameterSet;
                    //        groupMarkList.Add(groupMark);
                    //    }
                    //}


                }
              }
              catch (Exception ex)
              {
                throw ex;
              }
            return groupMarkList;
        }

        public List<Groupmarking_Name> GetGroupMarkNameByProjId()
        {
            DataSet dsGroupMarkDetails = new DataSet();

            IEnumerable<Groupmarking_Name> GetGroupmarkingName;
            List<Groupmarking_Name> groupMarkList = new List<Groupmarking_Name>();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.Add("@INTPROJECTID", ProjectId);
                    dynamicParameters.Add("@INTSTRUCTUREELEMENTTYPEID", StructureElementTypeId);
                    dynamicParameters.Add("@SITPRODUCTTYPEID", SitProductTypeId);
                    // dsGroupMarkDetails = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_CopyGMGroupmarking_Get");
                    GetGroupmarkingName = sqlConnection.Query<Groupmarking_Name>(SystemConstant.CopyGMGroupmarking_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    groupMarkList = GetGroupmarkingName.ToList();
                        
                    //if (GetGroupmarkingName.Count() > 0)
                    //{
                    //    DataTable dt = ConvertToDataTable.ToDataTable(GetGroupmarkingName);
                    //    dsGroupMarkDetails.Tables.Add(dt);
                    //}
                    //foreach (DataRowView drvGroupMarkDetails in dsGroupMarkDetails.Tables[0].DefaultView)
                    //    {
                    //        GroupMark groupMark = new GroupMark
                    //        {
                    //            GroupMarkingName = drvGroupMarkDetails["VCHGROUPMARKINGNAME"].ToString(),
                    //        };
                    //        groupMarkList.Add(groupMark);
                    //    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return groupMarkList;
        }

        public string CheckDestinationParameterSet(int structureElementId, int SourceParameterSetId, int DestParameterSetId, int SourceProjectId, int DestinationProjectId)
        {
            string Output = null;

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@SOURCEPARAMETERSETNUMBER", SourceParameterSetId);
                    dynamicParameters.Add("@DESTPARAMETERSETNUMBER", DestParameterSetId);
                    dynamicParameters.Add("@SOURCEPROJECTID", SourceProjectId);
                    dynamicParameters.Add("@DESTINATIONPROJECTID", DestinationProjectId);
                    dynamicParameters.Add("@STRUCTUREELEMENTID", structureElementId);
                    dynamicParameters.Add("@DESTPARAMETERSETID", 0);
                  //  Output = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_CopyGM_DestParameterSet_Check"));
                    Output = sqlConnection.QueryFirstOrDefault<string>(SystemConstant.CopyGM_DestParameterSet_Check, dynamicParameters, commandType: CommandType.StoredProcedure);

                    return Output;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CopyGM_DestinationGM_Check()
        {
            int Output = 0;

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add( "@PROJECTID", this.ProjectId);
                    dynamicParameters.Add( "@STRUCTUREELEMENTTYPEID", this.StructureElementTypeId);
                    dynamicParameters.Add( "@PRODUCTTYPEID", this.SitProductTypeId);
                    dynamicParameters.Add( "@GROUPMARKNAME", this.GroupMarkingName);
                 //  Output = Convert.ToInt32(dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_CopyGM_DestinationGM_Check_PV")); //modified for ship to party
                    Output = sqlConnection.QueryFirstOrDefault<int>(SystemConstant.CopyGM_DestinationGM_Check_PV, dynamicParameters, commandType: CommandType.StoredProcedure);

                    return Output;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CopyGM_CreateDestinationParameterSet(int sourceProjectId, int destProjectId, int sourceParameterSetId, int destParameterSetId)
        {
            // int destinationParameterSetId;
          
            DataSet destinationParameterSetId = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@STRUCTUREELEMENTTYPEID", this.StructureElementTypeId);
                    dynamicParameters.Add("@PRODUCTTYPEID", this.SitProductTypeId);
                    dynamicParameters.Add("@SOURCEPARAMETERSETNUMBER", sourceParameterSetId);
                    dynamicParameters.Add("@DESTPARAMETERSETNUMBER", destParameterSetId);
                    dynamicParameters.Add("@SOURCEPROJECTID", sourceProjectId);
                    dynamicParameters.Add("@DESTINATIONPROJECTID", destProjectId);
                    dynamicParameters.Add("@USERID", this.CreatedUserId);
                    // dynamicParameters.Add(6, "@DESTPARAMETERSETID", destParameterSetId);

                  //  destinationParameterSetId = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_CopyGM_DestinationParameterSet_Create");
                    destinationParameterSetId = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.CopyGM_DestinationParameterSet_Create, dynamicParameters, commandType: CommandType.StoredProcedure);

                    return destParameterSetId;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
          

        }
        //Added for CHG0031332

        public int CopyGM_NewCreateDestinationParameterSet(int sourceProjectId, int destProjectId, int sourceParameterSetId)
        {
            // int destinationParameterSetId;
          
            int destinationParameterSetId;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@STRUCTUREELEMENTTYPEID", this.StructureElementTypeId);
                    dynamicParameters.Add("@PRODUCTTYPEID", this.SitProductTypeId);
                    dynamicParameters.Add("@SOURCEPARAMETERSETNUMBER", sourceParameterSetId);
                    dynamicParameters.Add("@SOURCEPROJECTID", sourceProjectId);
                    dynamicParameters.Add("@DESTINATIONPROJECTID", destProjectId);
                    dynamicParameters.Add("@USERID", this.CreatedUserId);


                    //destinationParameterSetId = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_CopyGM_DestinationParameterSet_Create_New");
                    destinationParameterSetId = sqlConnection.QueryFirstOrDefault<int>(SystemConstant.CopyGM_DestinationParameterSet_Create_New, dynamicParameters, commandType: CommandType.StoredProcedure);


                    //Convert.ToInt32(dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_CopyGM_DestinationParameterSet_Create_New"));
                    return destinationParameterSetId;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           

        }

        public string CopyGM_GetCopyWBSElementId(string wbs1, string wbs2)
        {
            string wbsElementsIds = null;

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTPROJECTID", this.ProjectId);
                    dynamicParameters.Add("@INTWBSTYPEID", this.WBSTypeId);
                    dynamicParameters.Add("@VCHWBS1", wbs1);
                    dynamicParameters.Add("@VCHWBS2", wbs2);
                    dynamicParameters.Add("@INTSTRUCTUREELEMENTTYPEIDSOURCE", this.StructureElementTypeId);
                    dynamicParameters.Add("@SITPRODUCTTYPEIDSOURCE", this.SitProductTypeId);
                   // wbsElementsIds = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_CopyGM_CopyWBSElementID_Get"));
                    wbsElementsIds = sqlConnection.QueryFirstOrDefault<string>(SystemConstant.CopyGM_CopyWBSElementID_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    return wbsElementsIds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //copy for released
        public string CopyGM_CopyGroupMarking(int sourceProjectId, int destProjectId, int sourceGroupMarkId, string destGroupMarkName, int sourceParameterSet, int destParameterSet, string copyFrom, string wbsElements, int IsGroupMarkRevision)
        {
            string destGroupMark = null;

            DataSet dsCopyGM = new DataSet();
            IEnumerable<GetReleasedGMDto> DestinationGroupMarkDto = new List<GetReleasedGMDto>();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@STRUCTUREELEMENTTYPEID", this.StructureElementTypeId);
                    dynamicParameters.Add("@PRODUCTTYPEID", this.SitProductTypeId);
                    dynamicParameters.Add("@WBSTYPEID", this.WBSTypeId);
                    dynamicParameters.Add("@SOURCEPROJECTID", sourceProjectId);
                    dynamicParameters.Add("@DESTPROJECTID", destProjectId);
                    dynamicParameters.Add("@SOURCEGROUPMARKID", sourceGroupMarkId);
                    dynamicParameters.Add("@DESTGROUPMARKNAME", destGroupMarkName);
                    dynamicParameters.Add("@SOURCEPARAMETERSETID", sourceParameterSet);
                    dynamicParameters.Add("@DESTPARAMETERSETID", destParameterSet.ToString());
                    dynamicParameters.Add("@COPYFROM", copyFrom);
                    dynamicParameters.Add("@WBSELEMENTIDS", wbsElements);
                    dynamicParameters.Add("@ISGROUPMARKREVISION", IsGroupMarkRevision);
                    dynamicParameters.Add("@USERID", this.CreatedUserId);
                    //dsCopyGM = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "[usp_CopyGM_CopyGroupMarking_Insert_CUBE]");
                    DestinationGroupMarkDto = sqlConnection.Query<GetReleasedGMDto>(SystemConstant.CopyGM_CopyGroupMarking_Insert_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);
                    if (DestinationGroupMarkDto.Count() > 0)
                    {
                        DataTable dt = ConvertToDataTable.ToDataTable(DestinationGroupMarkDto);
                        dsCopyGM.Tables.Add(dt);
                    }
                    if (dsCopyGM != null && dsCopyGM.Tables.Count != 0)
                    {
                        for (int i = 0; i < dsCopyGM.Tables.Count; i++)
                        {
                            if (dsCopyGM.Tables[i].Columns.Contains("DESTGM"))
                            {
                                foreach (DataRowView drvCopyGM in dsCopyGM.Tables[i].DefaultView)
                                {
                                    destGroupMark = drvCopyGM["DESTGM"].ToString();
                                }
                            }
                        }

                    }
                   
                    return destGroupMark;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

     //   Added for CHG0031332


        public string CopyGM_CopyGroupMarking_New(int sourceProjectId, int destProjectId, int sourceGroupMarkId, string destGroupMarkName, int sourceParameterSet, int destParameterSet, string copyFrom, string wbsElements, int IsGroupMarkRevision)
        {
            string destGroupMark = null;
          
            DataSet dsCopyGM = new DataSet();
            IEnumerable<GetReleasedGMDto> DestinationGroupMarkDto = new List<GetReleasedGMDto>();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@STRUCTUREELEMENTTYPEID", this.StructureElementTypeId);
                    dynamicParameters.Add("@PRODUCTTYPEID", this.SitProductTypeId);
                    dynamicParameters.Add("@WBSTYPEID", 1);//this.WBSTypeId);
                    dynamicParameters.Add("@SOURCEPROJECTID", sourceProjectId);
                    dynamicParameters.Add("@DESTPROJECTID", destProjectId);
                    dynamicParameters.Add("@SOURCEGROUPMARKID", sourceGroupMarkId);
                    dynamicParameters.Add("@DESTGROUPMARKNAME", destGroupMarkName);
                    dynamicParameters.Add("@SOURCEPARAMETERSETID", sourceParameterSet);
                    dynamicParameters.Add("@DESTPARAMETERSETID", destParameterSet.ToString());
                    dynamicParameters.Add("@COPYFROM", copyFrom);
                    dynamicParameters.Add( "@WBSELEMENTIDS", "I");
                    dynamicParameters.Add( "@ISGROUPMARKREVISION", IsGroupMarkRevision);
                    dynamicParameters.Add( "@USERID", 1);
                    // dsCopyGM = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_CopyGM_CopyGroupMarking_Insert_New_CUBE");
                    DestinationGroupMarkDto =sqlConnection.Query<GetReleasedGMDto>(SystemConstant.CopyGM_CopyGroupMarking_Insert_New_CUBE, dynamicParameters, commandType: CommandType.StoredProcedure);
                    if (DestinationGroupMarkDto.Count() > 0)
                    {
                        DataTable dt = ConvertToDataTable.ToDataTable(DestinationGroupMarkDto);
                        dsCopyGM.Tables.Add(dt);
                    }

                    if (dsCopyGM != null && dsCopyGM.Tables.Count != 0)
                    {
                        for (int i = 0; i < dsCopyGM.Tables.Count; i++)
                        {
                            if (dsCopyGM.Tables[i].Columns.Contains("DESTGM"))
                            {
                                foreach (DataRowView drvCopyGM in dsCopyGM.Tables[i].DefaultView)
                                {
                                    destGroupMark = drvCopyGM["DESTGM"].ToString();
                                }
                            }
                        }

                    }
                    return destGroupMark;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CopyGM_ArmaValidator(int GroupMarkId)
        {
            int result = 0;

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add( "@SOURCEGROUPMARKID", GroupMarkId);
                    dynamicParameters.Add( "@OUTPUT", 0);
                   // result =
                   // .ToInt32(dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_Copy_NDSAndARMA_Qty_Validator"));
                    result = sqlConnection.QueryFirstOrDefault<int>(SystemConstant.Copy_NDSAndARMA_Qty_Validator, dynamicParameters, commandType: CommandType.StoredProcedure);


                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //POsted group mark 
        public int CopyGM_UpdatePostedGMforRevisedGM(int sourceGroupMarkId, int destGroupMarkId, int destGroupMarkRevision)
        {
            int result = 0;

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intgroupmarkid", sourceGroupMarkId);
                    dynamicParameters.Add("@identityGM", destGroupMarkId);
                    dynamicParameters.Add("@tntGroupRevNo", destGroupMarkRevision);
                    dynamicParameters.Add("@Output", 0);
                   // result = Convert.ToInt32(dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_CopyGM_UpdateGM_PostGM"));
                    result = sqlConnection.QueryFirstOrDefault<int>(SystemConstant.CopyGM_UpdateGM_PostGM, dynamicParameters, commandType: CommandType.StoredProcedure);


                    if (result == 2)
                    {
                        result = 1; // Included bze if the GM is Post and again do unpost record will be in PostGroupMark table. 
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
   
    
    }
}
