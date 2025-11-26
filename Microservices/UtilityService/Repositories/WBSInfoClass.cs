
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using BWIR.DAL;
using System.Data;
//using SharedCache.WinServiceCommon.Provider.Cache;
using System.Configuration;
using Dapper;
using Microsoft.Data.SqlClient;
using UtilityService.Constants;
using UtilityService.Dtos;


namespace UtilityService.Repositories
{
  
    public class WBSInfoClass
    {
        private object dbManager;
        private string connectionString = "Server=NSQADB5\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=MES_USER;Password=Mes@123;Connection Timeout=36000000";
        //private string connectionString = "Server=nsprddb10\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";

        #region Properties
        public int WBSElementId { get; set; }
        public string Block { get; set; }
        public string Storey { get; set; }
        public string Part { get; set; }
        public string SAPProjectCode { get; set; }
        public int ProjectId { get; set; }

        public int WBSTypeId { get; set; }
        public string WBSType { get; set; }
        public string WBSDescription { get; set; }

        public string BBSNo { get; set; }
        public string BBSDesc { get; set; }

        public bool IsSelected { get; set; }
        public string BBSNoCellColor { get; set; }

        public int PostHeaderId { get; set; }
        public int StructureElementId { get; set; }
        public int ProductTypeId { get; set; }
        public int UserId { get; set; }


        public int PostedQuantity { get; set; }
        public double PostedTonnage { get; set; }
        public string Modular { get; set; } // Modular 31014
        #endregion



        //public WBSInfoClass(int WBSElementId,string Block,string Storey,string Part)
        //{

        //}

        public WBSInfoClass()
        {
        }

        public List<WBSInfoClass> WBSInfoByGroupID_Get(int GroupMarkId)
        {
            // DBManager dbManager = new DBManager();
            List<WBSInfoClass> listWBSInfo = new List<WBSInfoClass> { };

            DataSet dsWBSINFOByGroupID = new DataSet();

            try
            {
                
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTGROUPMARKID",GroupMarkId);

                    dsWBSINFOByGroupID = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.WBSINFOByGroupID_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                   
                }

                if (dsWBSINFOByGroupID != null && dsWBSINFOByGroupID.Tables.Count != 0)
                {
                    foreach (DataRowView drvBeam in dsWBSINFOByGroupID.Tables[0].DefaultView)
                    {

                        WBSInfoClass wbsInfo = new WBSInfoClass
                        {
                            WBSElementId = Convert.ToInt32(drvBeam["INTWBSELEMENTID"]),
                            Block = Convert.ToString(drvBeam["BLOCK"]),
                            Storey = Convert.ToString(drvBeam["STOREY"]),
                            Part = Convert.ToString(drvBeam["PART"])
                        };

                        listWBSInfo.Add(wbsInfo);


                    }
                    // IndexusDistributionCache.SharedCache.Add(strCacheWBSInfo, listWBSInfo, DateTime.Today.AddMinutes(30));
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
              //  dbManager.Dispose();
            }
            return listWBSInfo;

        }

        public List<WBSInfoClass> WBSInfoByElementId_Get(string InputValueParameter)
        {
            //DBManager dbManager = new DBManager();
            List<WBSInfoClass> listWBSInfo = new List<WBSInfoClass> { };

            DataSet dsWBSINFOByGroupID = new DataSet();

            try
            {               
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@vchInputValueParameter", InputValueParameter);

                    dsWBSINFOByGroupID = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.BCDWBSList_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                }

                if (dsWBSINFOByGroupID != null && dsWBSINFOByGroupID.Tables.Count != 0)
                {
                    foreach (DataRowView drvBeam in dsWBSINFOByGroupID.Tables[0].DefaultView)
                    {

                        WBSInfoClass wbsInfo = new WBSInfoClass
                        {
                            WBSElementId = Convert.ToInt32(drvBeam["intWBSElementId"]),
                            Block = Convert.ToString(drvBeam["vchWBS1"]),
                            Storey = Convert.ToString(drvBeam["vchWBS2"]),
                            Part = Convert.ToString(drvBeam["vchWBS3"])
                        };

                        listWBSInfo.Add(wbsInfo);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // dbManager.Dispose();
            }
            return listWBSInfo;

        }

        public List<WBSInfoClass> GetWBSType()
        {
            //DBManager dbManager = new DBManager();
            List<WBSInfoClass> listGetWBSType = new List<WBSInfoClass> { };
            DataSet dsGetWBSType = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    //dynamicParameters.Add("@vchInputValueParameter", InputValueParameter);

                    dsGetWBSType = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.WBSType_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    
                }

                if (dsGetWBSType != null && dsGetWBSType.Tables.Count != 0)
                {
                    foreach (DataRowView drvWBSType in dsGetWBSType.Tables[0].DefaultView)
                    {
                        WBSInfoClass wbs = new WBSInfoClass
                        {
                            WBSTypeId = Convert.ToInt32(drvWBSType["INTWBSTYPEID"].ToString()),
                            WBSType = drvWBSType["VCHWBSTYPE"].ToString(),
                            WBSDescription = drvWBSType["VCHDESCRIPTION"].ToString()
                        };
                        listGetWBSType.Add(wbs);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
               // dbManager.Dispose();
            }
            return listGetWBSType;
        }

        public List<GetWBS1>GetWBS1(string ProjectCode, string structureElement, string productType)
        {

            List<WBSInfoClass> GetWBS1_list = new List<WBSInfoClass>();
            DataSet dsGetWBS1 = new DataSet();
            IEnumerable<GetWBS1> GetWBS1_IEnumerable;

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@PROJECTCODE", ProjectCode);
                    dynamicParameters.Add("@STRUC_ELEM_TYPE", structureElement.Trim());
                    dynamicParameters.Add("@PROD_TYPE", productType.Trim());

                    GetWBS1_IEnumerable = sqlConnection.Query<GetWBS1>(SystemConstant.USP_WBS1_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    return GetWBS1_IEnumerable.ToList();
                }

                //if (dsGetWBS1 != null && dsGetWBS1.Tables.Count != 0)
                //{
                //    foreach (DataRowView drvWBS1 in dsGetWBS1.Tables[0].DefaultView)
                //    {
                //        WBSInfoClass wbs = new WBSInfoClass
                //        {
                //            Block = drvWBS1["WBS1"].ToString()
                //        };
                //        listGetWBS1.Add(wbs);
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // dbManager.Dispose();
            }
        }
       
        public List<GetWBS2> GetWBS2(string ProjectCode,string structureElement, string productType,string WBS1)
        {
            //DBManager dbManager = new DBManager();
            List<WBSInfoClass> listGetWBS2 = new List<WBSInfoClass> { };
            DataSet dsGetWBS2 = new DataSet();
            IEnumerable<GetWBS2> getWBS2s;
            try
            {

                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@PROJECTCODE",ProjectCode);
                    dynamicParameters.Add("@STRUC_ELEM_TYPE", structureElement.Trim());
                    dynamicParameters.Add("@PROD_TYPE", productType.Trim());
                    dynamicParameters.Add("@WBS1",WBS1.Trim());



                    getWBS2s = sqlConnection.Query<GetWBS2>(SystemConstant.USP_WBS2_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    return getWBS2s.ToList();
                }
                   
           
                //if (dsGetWBS2 != null && dsGetWBS2.Tables.Count != 0)
                //{
                //    foreach (DataRowView drvWBS2 in dsGetWBS2.Tables[0].DefaultView)
                //    {
                //        WBSInfoClass wbs = new WBSInfoClass
                //        {
                //            Storey = drvWBS2["WBS2"].ToString(),
                //            WBSElementId = Convert.ToInt32(drvWBS2["NDS_WBSELEMENT_ID"]),
                //            BBSNo = drvWBS2["VCHBBSNO"].ToString(),
                //            BBSDesc = drvWBS2["BBS_DESC"].ToString()
                //        };
                //        listGetWBS2.Add(wbs);
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
               // dbManager.Dispose();
            }
        }

        public List<GetWBS3> GetWBS3(string ProjectCode,string structureElement, string productType,string WBS1,string WBS2)
        {

            List<GetWBS3> GetWBS3_list = new List<GetWBS3>();
            IEnumerable<GetWBS3> GetWBS3_IEnumerable;
            DataSet dsGetWBS3 = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@PROJECTCODE",ProjectCode);
                    dynamicParameters.Add("@STRUC_ELEM_TYPE", structureElement.Trim());
                    dynamicParameters.Add("@PROD_TYPE", productType.Trim());
                    dynamicParameters.Add("@WBS1",WBS1.Trim());
                    dynamicParameters.Add("@WBS2",WBS2.Trim());


                    GetWBS3_IEnumerable = sqlConnection.Query<GetWBS3>(SystemConstant.USP_WBS3_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    return GetWBS3_IEnumerable.ToList();

                }

                //if (dsGetWBS3 != null && dsGetWBS3.Tables.Count != 0)
                //{
                //    foreach (DataRowView drvWBS3 in dsGetWBS3.Tables[0].DefaultView)
                //    {
                //        WBSInfoClass wbs = new WBSInfoClass
                //        {
                //            Part = drvWBS3["WBS3"].ToString(),
                //            WBSElementId = Convert.ToInt32(drvWBS3["NDS_WBSELEMENT_ID"]),
                //            BBSNo = drvWBS3["VCHBBSNO"].ToString(),
                //            BBSDesc = drvWBS3["BBS_DESC"].ToString()
                //        };
                //        listGetWBS3.Add(wbs);
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //// dbManager.Dispose();
            }
           
        }

        public List<GetWBS1> GetCopyWBS1(int StructureElementId, int ProductTypeId, int ProjectId,int WBSTypeId)
        {
            //DBManager dbManager = new DBManager();
            List<WBSInfoClass> listGetWBS1 = new List<WBSInfoClass> { };
            List<GetWBS1> GetWBS3_list = new List<GetWBS1>();
            IEnumerable<GetWBS1> GetWBS1_IEnumerable;
            DataSet dsGetWBS1 = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTPROJECTID", ProjectId);
                    dynamicParameters.Add("@INTSTRUCTUREELEMENTTYPEID", StructureElementId);
                    dynamicParameters.Add("@SITPRODUCTTYPEID", ProductTypeId);
                    dynamicParameters.Add("@INTWBSTYPEID", WBSTypeId);



                    GetWBS1_IEnumerable = sqlConnection.Query<GetWBS1>(SystemConstant.CopyWBS1_Get, dynamicParameters, commandType: CommandType.StoredProcedure);


                }

                //if (dsGetWBS1 != null && dsGetWBS1.Tables.Count != 0)
                //{
                //    foreach (DataRowView drvWBS1 in dsGetWBS1.Tables[0].DefaultView)
                //    {
                //        WBSInfoClass wbs = new WBSInfoClass
                //        {
                //            Block = drvWBS1["VCHWBS1"].ToString()
                //        };
                //        listGetWBS1.Add(wbs);
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
               // dbManager.Dispose();
            }
            return GetWBS1_IEnumerable.ToList();
        }

        public List<GetWBS2> GetCopyWBS2(int StructureElementId, int ProductTypeId, int ProjectId, int WBSTypeId,string Block)
        {
            //DBManager dbManager = new DBManager();
            List<WBSInfoClass> listGetWBS2 = new List<WBSInfoClass> { };
            List<GetWBS2> GetWBS3_list = new List<GetWBS2>();
            IEnumerable<GetWBS2> GetWBS2_IEnumerable;
            DataSet dsGetWBS2 = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTPROJECTID", ProjectId);
                    dynamicParameters.Add("@INTSTRUCTUREELEMENTTYPEID", StructureElementId);
                    dynamicParameters.Add("@SITPRODUCTTYPEID", ProductTypeId);
                    dynamicParameters.Add("@INTWBSTYPEID", WBSTypeId);
                    //dynamicParameters.Add("@WBS2", Storey);
                    dynamicParameters.Add("@VCHWBS1", Block);

                    GetWBS2_IEnumerable = sqlConnection.Query<GetWBS2>(SystemConstant.CopyWBS2_Get, dynamicParameters, commandType: CommandType.StoredProcedure);


                }
                
             
                //if (dsGetWBS2 != null && dsGetWBS2.Tables.Count != 0)
                //{
                //    foreach (DataRowView drvWBS2 in dsGetWBS2.Tables[0].DefaultView)
                //    {
                //        WBSInfoClass wbs = new WBSInfoClass
                //        {
                //            Storey = drvWBS2["VCHWBS2"].ToString()
                //        };
                //        listGetWBS2.Add(wbs);
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
               // dbManager.Dispose();
            }
            return GetWBS2_IEnumerable.ToList();
        }

        public List<GetWBS3> GetCopyWBS3(int StructureElementId, int ProductTypeId, int ProjectId, int WBSTypeId,string Block,string Storey)
        {
            //DBManager dbManager = new DBManager();

            //List<WBSInfoClass> listGetWBS3 = new List<WBSInfoClass> { };

            List<GetWBS3> GetWBS3_list = new List<GetWBS3>();
            IEnumerable<GetWBS3> GetWBS3_IEnumerable;
            DataSet dsGetWBS3 = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@PROJECTID", ProjectId);
                    dynamicParameters.Add("@STRUCTUREELEMENTID", StructureElementId);
                    dynamicParameters.Add("@PRODUCTTYPEID", ProductTypeId);
                    dynamicParameters.Add("@WBS1", Block);
                    dynamicParameters.Add("@WBS2", Storey);


                    GetWBS3_IEnumerable = sqlConnection.Query<GetWBS3>(SystemConstant.CopyWBS3_Get, dynamicParameters, commandType: CommandType.StoredProcedure);


                }
               

               
                //if (dsGetWBS3 != null && dsGetWBS3.Tables.Count != 0)
                //{
                //    foreach (DataRowView drvWBS3 in dsGetWBS3.Tables[0].DefaultView)
                //    {
                //        WBSInfoClass wbs = new WBSInfoClass
                //        {
                //            Part = drvWBS3["VCHWBS3"].ToString(),
                //            WBSElementId = Convert.ToInt32(drvWBS3["INTWBSELEMENTID"])
                //        };
                //        listGetWBS3.Add(wbs);
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
               // dbManager.Dispose();
            }
            return GetWBS3_IEnumerable.ToList();
        }


        public List<GetBBSNoDto> GetBBSNoAndBBSDesc(int structureElementId, int productTypeId)
        {
            //DBManager dbManager = new DBManager();
            List<GetBBSNoDto> listGetBBSNoAndDesc = new List<GetBBSNoDto> { };
            DataSet dsBBSNo = new DataSet();
            IEnumerable<GetBBSNoDto> getBBSNoDtos;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@PROJECTID",ProjectId);
                    dynamicParameters.Add("@STRUCTUREELEMENTTYPEID", structureElementId);
                    dynamicParameters.Add("@PRODUCTTYPEID", productTypeId);
                    dynamicParameters.Add("@WBSTYPEID",WBSTypeId);
                    dynamicParameters.Add("@WBS1",Block);
                    dynamicParameters.Add("@WBS2",Storey);
                    dynamicParameters.Add("@WBS3",Part);

                    getBBSNoDtos = sqlConnection.Query<GetBBSNoDto>(SystemConstant.CopyWBS_BBSNoAndBBSDesc_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                   listGetBBSNoAndDesc = getBBSNoDtos.ToList();

                }
                //if (dsBBSNo != null && dsBBSNo.Tables.Count != 0)
                //{
                //    foreach (DataRowView drvBBSNo in dsBBSNo.Tables[0].DefaultView)
                //    {
                //        WBSInfoClass wbs = new WBSInfoClass
                //        {
                //            BBSNo = drvBBSNo["VCHBBSNO"].ToString(),
                //            BBSDesc = drvBBSNo["BBS_DESC"].ToString(),
                //            PostHeaderId = Convert.ToInt32(drvBBSNo["INTPOSTHEADERID"]),
                //            Modular = drvBBSNo["Modular"].ToString(), //Modular 31014
                //        };
                //        listGetBBSNoAndDesc.Add(wbs);
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listGetBBSNoAndDesc;
        }
        public List<GetDestinationWBS_Insert> GetDestWBSDetails(int structureElementId, int productTypeId, string storeyFrom, string storeyTo)
        {
         
            List<WBSInfoClass> listGetDestWBSDetails = new List<WBSInfoClass> { };
            IEnumerable<GetDestinationWBS_Insert> dsWBSDetails;
            try
            {

                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@PROJECTID", this.ProjectId);
                    dynamicParameters.Add("@STRUCTUREELEMENTTYPEID", structureElementId);
                    dynamicParameters.Add("@PRODUCTTYPEID", productTypeId);
                    dynamicParameters.Add("@WBSTYPEID", 1);

                    dynamicParameters.Add("@WBS1", this.Block);
                    dynamicParameters.Add("@WBS2FROM", storeyFrom);
                    dynamicParameters.Add("@WBS2TO", storeyTo);
                    dynamicParameters.Add("@WBS3", this.Part);



                    dsWBSDetails = sqlConnection.Query<GetDestinationWBS_Insert>(SystemConstant.CopyWBS_DestWBSDetails_Get, dynamicParameters, commandType: CommandType.StoredProcedure);


                }

              
                //if (dsWBSDetails != null && dsWBSDetails.Tables.Count != 0)
                //{
                //    foreach (DataRowView drvWBSDetails in dsWBSDetails.Tables[0].DefaultView)
                //    {
                //        WBSInfoClass wbs = new WBSInfoClass
                //        {
                //            WBSElementId = Convert.ToInt32(drvWBSDetails["INTWBSELEMENTID"].ToString()),
                //            Block = drvWBSDetails["VCHWBS1"].ToString(),
                //            Storey = drvWBSDetails["VCHWBS2"].ToString(),
                //            Part = drvWBSDetails["VCHWBS3"].ToString(),
                //            IsSelected = true
                //        };
                //        listGetDestWBSDetails.Add(wbs);
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsWBSDetails.ToList();
        }

        public List<ValidateBBSDetailsDto> BBSNo_Validate(string WBSElements, string BBSNos, string BBSDescriptions, out int IsSuccess)
        {
            //DBManager dbManager = new DBManager();
            List<ValidateBBSDetailsDto> listBBSValidate = new List<ValidateBBSDetailsDto> { };
            DataSet dsBBSValidate = new DataSet();

            IEnumerable<ValidateBBSDetailsDto> validateBBs;
            try
            {
                IsSuccess = 0;

                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@WBSELEMENTIDS", WBSElements);
                    dynamicParameters.Add("@BBSNOS", BBSNos);
                    dynamicParameters.Add("@DESTPROJECID", this.ProjectId);
                    dynamicParameters.Add("@STRUCTUREELEMENTTYPEID", this.StructureElementId);
                    dynamicParameters.Add("@PRODUCTTYPEID", this.ProductTypeId);
                    dynamicParameters.Add("@BBSDESCS", BBSDescriptions);


                    validateBBs =sqlConnection.Query<ValidateBBSDetailsDto>(SystemConstant.CopyWBS_BBSNo_Validate, dynamicParameters, commandType: CommandType.StoredProcedure);
                    
                    return validateBBs.ToList();

                }
             
               
                //if (dsBBSValidate != null && dsBBSValidate.Tables.Count != 0)
                //{
                //    foreach (DataRowView drvWBSDetails in dsBBSValidate.Tables[0].DefaultView)
                //    {
                //        WBSInfoClass wbs = new WBSInfoClass
                //        {
                //            WBSElementId = Convert.ToInt32(drvWBSDetails["WBSELEMENTID"].ToString()),
                //            Block = drvWBSDetails["WBS1"].ToString(),
                //            Storey = drvWBSDetails["WBS2"].ToString(),
                //            Part = drvWBSDetails["WBS3"].ToString(),
                //            BBSNo = drvWBSDetails["BBSNO"].ToString(),
                //            BBSDesc = drvWBSDetails["BBSDESC"].ToString(),
                //            BBSNoCellColor = drvWBSDetails["BBSNOFLAG"].ToString(),
                //        };
                //        listBBSValidate.Add(wbs);
                //    }

                //    foreach (DataRowView drvWBSDetails in dsBBSValidate.Tables[1].DefaultView)
                //    {
                //        IsSuccess = Convert.ToInt32(drvWBSDetails["OUTPUT"].ToString());
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
           

        }

        public bool CopyWBSDetailing(string WBSElements, string BBSNos, string BBSDescriptions)
        {

            string returnValue =string.Empty;
            
            WBSInfoClass slabStructure = new WBSInfoClass();
           
            bool isSuccess = false;
           
            DataSet dsCopyWBS = new DataSet(); //Added for CHG0031776
            string ERRORLOG; //Added for CHG0031776
            try
            {

              
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@DESTPROJECTID", this.ProjectId);
                    dynamicParameters.Add("@STRUCTUREELEMENTTYPEID", this.StructureElementId);
                    dynamicParameters.Add("@PRODUCTTYPEID", this.ProductTypeId);
                    dynamicParameters.Add("@WBSTYPEID", this.WBSTypeId);
                    dynamicParameters.Add("@SOURCEPOSTHEADERID", this.PostHeaderId);
                    dynamicParameters.Add("@DESTWBSELEMENTIDS",WBSElements);
                    dynamicParameters.Add("@BBSNOS", BBSNos);
                    dynamicParameters.Add("@BBSDESCS", BBSDescriptions);
                    dynamicParameters.Add("@USERID",this.UserId);


                    returnValue = sqlConnection.Query<string>(SystemConstant.CopyWBS_CopyWBSDetailing, dynamicParameters, commandType: CommandType.StoredProcedure).SingleOrDefault();


                    if (dsCopyWBS != null && dsCopyWBS.Tables.Count != 0)
                    {
                        for (int i = 0; i < dsCopyWBS.Tables.Count; i++)
                        {
                            if (dsCopyWBS.Tables[i].Columns.Contains("ERRORLOG"))
                            {

                                foreach (DataRowView drvCopyGM in dsCopyWBS.Tables[i].DefaultView)
                                {
                                    ERRORLOG = drvCopyGM["ERRORLOG"].ToString();
                                    isSuccess = false;
                                }
                            }
                            else
                            {
                                isSuccess = true;
                            }
                        }

                    }

                    else
                    {
                        //if (!string.IsNullOrEmpty(returnValue))
                        //{
                            isSuccess = true;
                        //}
                        //else

                        //{ isSuccess = false; }

                    }


                }
               // return isSuccess;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isSuccess;
        }

        public int GetUserIDByName(string name)
        {
            int Output = 0;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@UserName", name);

                    Output = sqlConnection.QueryFirstOrDefault<int>(SystemConstant.GetUserID, dynamicParameters, commandType: CommandType.StoredProcedure);
                    //Output = dynamicParameters.Get<int>("@Output");
                    sqlConnection.Close();

                }

                return Output;


            }
            catch (Exception e)
            {
                return Output;
            }

        }

        public List<WBSInfoClass> GetPostedQuantityandTonnageByPostHeaderId(int structureElementId, int productTypeId, string storeyFrom, out List<WBSInfoClass>destinationPostedDetailsList)
        {
            
            DataSet dsBBSNo = new DataSet();
            destinationPostedDetailsList = new List<WBSInfoClass>{ };
            List<WBSInfoClass> listpostedquantity = new List<WBSInfoClass> { };

            try
            {


                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();

                    SqlCommand cmd = new SqlCommand(SystemConstant.CopyWBS_PostedTonnageandQuantityByWBSDetails_Get, sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@PROJECTID", ProjectId));
                    cmd.Parameters.Add(new SqlParameter("@STRUCTUREELEMENTTYPEID", structureElementId));
                    cmd.Parameters.Add(new SqlParameter("@PRODUCTTYPEID", productTypeId));
                    cmd.Parameters.Add(new SqlParameter("@WBSTYPEID", WBSTypeId));
                    cmd.Parameters.Add(new SqlParameter("@WBS1", Block));
                    cmd.Parameters.Add(new SqlParameter("@WBS2", storeyFrom));
                    cmd.Parameters.Add(new SqlParameter("@WBS3", Part));
                    cmd.Parameters.Add(new SqlParameter("@POSTHEADERID", PostHeaderId));

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dsBBSNo);

                    //var dynamicParameters = new DynamicParameters();
                    //dynamicParameters.Add("@PROJECTID",ProjectId);
                    //dynamicParameters.Add("@STRUCTUREELEMENTTYPEID", structureElementId);
                    //dynamicParameters.Add("@PRODUCTTYPEID", productTypeId);
                    //dynamicParameters.Add("@WBSTYPEID",WBSTypeId);
                    //dynamicParameters.Add("@WBS1",Block);
                    //dynamicParameters.Add("@WBS2", storeyFrom);
                    //dynamicParameters.Add("@WBS3",Part);
                    //dynamicParameters.Add("@POSTHEADERID",PostHeaderId);


                    //getPostedQuantityTonnageDto = sqlConnection.Query<GetPostedQuantityTonnageDto>(SystemConstant.CopyWBS_PostedTonnageandQuantityByWBSDetails_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    if (dsBBSNo != null && dsBBSNo.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBBSNo in dsBBSNo.Tables[0].DefaultView)
                        {
                            WBSInfoClass objGetSourcePostedDetails = new WBSInfoClass
                             {
                                Block = Convert.ToString(drvBBSNo["WBS1"]),
                                Storey = Convert.ToString(drvBBSNo["WBS2"]),
                                Part = Convert.ToString(drvBBSNo["WBS3"]),
                                PostedQuantity = Convert.ToInt32(drvBBSNo["INTPOSTEDQTY"]),
                                PostedTonnage = Convert.ToDouble(drvBBSNo["NUMPOSTEDWEIGHT"]),
                                PostHeaderId = Convert.ToInt32(drvBBSNo["INTPOSTHEADERID"])
                             };
                            listpostedquantity.Add(objGetSourcePostedDetails);
                        }
                        foreach (DataRowView drvBBSNo in dsBBSNo.Tables[1].DefaultView)
                        {
                            WBSInfoClass objGetDestinationPostedDetails = new WBSInfoClass
                            {
                                Block = Convert.ToString(drvBBSNo["WBS1"]),
                                Storey = Convert.ToString(drvBBSNo["WBS2"]),
                                Part = Convert.ToString(drvBBSNo["WBS3"]),
                                PostedQuantity = Convert.ToInt32(drvBBSNo["INTPOSTEDQTY"]),
                                PostedTonnage = Convert.ToDouble(drvBBSNo["NUMPOSTEDWEIGHT"]),
                                PostHeaderId = Convert.ToInt32(drvBBSNo["INTPOSTHEADERID"])
                            };
                            listpostedquantity.Add(objGetDestinationPostedDetails);

                        }
                    }

                    sqlConnection.Close();

                }

            }


            catch (Exception ex)
            {
                throw ex;
            }

            return listpostedquantity;
        }

    }
}
