using Dapper;
using OrderService.Constants;
using OrderService.Dtos;
using OrderService.NDSPosting;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace OrderService.Repositories
{
    public class ESMTrackerGenerator
    {
        //private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";
        private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";


        #region Properties
        public string TrakingId { get; set; }

        public int ProjectId { get; set; }
        public int ContractNo { get; set; }

        public string StructureElement { get; set; }
        public int StructureElementTypeId { get; set; }

        public int ProductTypeId { get; set; }
        public string ProductType { get; set; }

        public int PostHeaderId { get; set; }
        public int WBSElementId { get; set; }


        public string WBS1 { get; set; }
        public string WBS2 { get; set; }
        public string WBS3 { get; set; }
        public string WBS4 { get; set; }
        public string WBS5 { get; set; }

        public string BBSNO { get; set; }
        public string BBSSDesc { get; set; }
        public string BBSDescriptionString { get; set; }

        public string OrdDate { get; set; }
        public string PONumber { get; set; }
        public string ReqDate { get; set; }
        public string RevReqDate { get; set; }
        public string IntRemark { get; set; }
        public string ExtRemark { get; set; }
        // public string SOR { get; set; }

        public int InsertedById { get; set; }
        public string InsertedBy { get; set; }
        public string InsertedDate { get; set; }

        public int UpdatedById { get; set; }
        public string UpdatedBy { get; set; }
        public string UpodateddDate { get; set; }

        public string Result { get; set; }
        //public DataTable ResultTable { get; set; }
        public string StatusCode { get; set; }
        public string SalesOrderId { get; set; }
        public NDSStatus NDSStatus { get; set; }
        public int ReturnValue { get; set; }
        public bool Select { get; set; }

        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public string ProdDate { get; set; }
        public string OrderType { get; set; }
        public string Location { get; set; }
        public double OverDelTolerance { get; set; }
        public double UnderDelTolerance { get; set; }
        public string ContactPerson { get; set; }
        public string ReleasedStatus { get; set; }

        //Start: Added by Siddhi for Estimated weight field addition
        public string EstimatedWeight { get; set; }
        //End

        #endregion

        /// <summary>
        /// GenerateESMTracker() function is used to generate unique Tracking Id to track ESM order during ESM Order Processing
        /// </summary>
        /// <returns></returns>
        public bool GenerateTrackingID(string ProductType)
        {
            bool isSuccess = false;
            
            try
            {
                
                if (ProductType == "CAB") //CAB ==4
                {
                    using (var sqlConnection = new SqlConnection(connectionString))
                    {
                            sqlConnection.Open();
                            var dynamicParameters = new DynamicParameters();
                            dynamicParameters.Add("@CompanyCode", "ESM");
                            dynamicParameters.Add("@doc_type", "TOC");
                            //dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_GroupMarkingParamset_Update");
                            var a= sqlConnection.Query<string>(SystemConstant.spGetDocNo_ESM, dynamicParameters, commandType: CommandType.StoredProcedure);
                            this.TrakingId = a.FirstOrDefault();
                    }
               
                }
                if (ProductType == "MSH") // MSH ==7
                {

                    using (var sqlConnection = new SqlConnection(connectionString))
                        {
                            sqlConnection.Open();
                            var dynamicParameters = new DynamicParameters();
                            dynamicParameters.Add("@CompanyCode", "ESM");
                            dynamicParameters.Add("@doc_type", "TOM");
                        var a = sqlConnection.Query<string>(SystemConstant.spGetDocNo_ESM, dynamicParameters, commandType: CommandType.StoredProcedure);

                        this.TrakingId = a.FirstOrDefault();
                        //var a =sqlConnection.QueryFirstOrDefault(SystemConstant.spGetDocNo_ESM, dynamicParameters, commandType: CommandType.StoredProcedure);
                        
                    }
                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
               
            }
            return isSuccess;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        public List<ESMTrackerGenerator> SaveESMTrackingDetails(SaveESMTrackingDetailsDto trackingDetailsDto, out string Resultvalue, out string errorMessage)
        {
            Resultvalue = null;
            errorMessage = null;
            //bool isSuccess = false;
            trackingDetailsDto.TrakingId=this.TrakingId;
            DataSet dsTrackingDetailsResult = new DataSet();
            List<ESMTrackerGenerator> listGroupMark = new List<ESMTrackerGenerator>();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();

                    SqlCommand cmd = new SqlCommand(SystemConstant.usp_ESMTrackingDetails_Insert, sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Track_Ord_no", TrakingId));
                    cmd.Parameters.Add(new SqlParameter("@PROJ_ID", trackingDetailsDto.ProjectId));
                    cmd.Parameters.Add(new SqlParameter("@CONTR_NO", trackingDetailsDto.ContractNo));
                    cmd.Parameters.Add(new SqlParameter("@PO_NUM", trackingDetailsDto.PONumber));
                    cmd.Parameters.Add(new SqlParameter("@INTWBSELEMENTID", trackingDetailsDto.WBSElementId));
                    cmd.Parameters.Add(new SqlParameter("@INTSTRUCTUREELEMENTTYPEID", trackingDetailsDto.StructureElementTypeId));
                    cmd.Parameters.Add(new SqlParameter("@INTPRODUCTTYPEID", trackingDetailsDto.ProductTypeId));
                    cmd.Parameters.Add(new SqlParameter("@BBS_NO", trackingDetailsDto.BBSNO));
                    cmd.Parameters.Add(new SqlParameter("@BBS_DESC", trackingDetailsDto.BBSSDesc));
                    cmd.Parameters.Add(new SqlParameter("@REQ_DELI_DATE", trackingDetailsDto.ReqDate.ToString()));
                    cmd.Parameters.Add(new SqlParameter("@INT_REMARK", trackingDetailsDto.IntRemark));
                    cmd.Parameters.Add(new SqlParameter("@EXT_REMARK", trackingDetailsDto.ExtRemark));
                    cmd.Parameters.Add(new SqlParameter("@PO_DATE", trackingDetailsDto.OrdDate.ToString()));
                    cmd.Parameters.Add(new SqlParameter("@PROD_DATE", trackingDetailsDto.ProdDate.ToString()));
                    cmd.Parameters.Add(new SqlParameter("@ORD_TYPE", trackingDetailsDto.OrderType));
                    cmd.Parameters.Add(new SqlParameter("@LOCATION", trackingDetailsDto.Location));
                    cmd.Parameters.Add(new SqlParameter("@OVER_DEL_DELAY", trackingDetailsDto.OverDelTolerance));
                    cmd.Parameters.Add(new SqlParameter("@UNDER_DEL_DELAY", trackingDetailsDto.UnderDelTolerance));
                    cmd.Parameters.Add(new SqlParameter("@SITE_CONTACT_PERSON", trackingDetailsDto.ContactPerson));
                    cmd.Parameters.Add(new SqlParameter("@ESTIMATED_WEIGHT", trackingDetailsDto.EstimatedWeight));

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dsTrackingDetailsResult);

                    if (dsTrackingDetailsResult != null && dsTrackingDetailsResult.Tables.Count != 0)
                    {
                        foreach (DataRowView drvTracking in dsTrackingDetailsResult.Tables[0].DefaultView)
                        {

                            ESMTrackerGenerator groupMark = new ESMTrackerGenerator
                            {
                                ErrorMessage = Convert.ToString(drvTracking["ErrorMessage"]),
                                Result = Convert.ToString(drvTracking["SuccessMessage"]),

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
            finally
            {
               
            }
            // return isSuccess;
            return listGroupMark;
            
        }

        public List<ESMTrackerGenerator>UpdateESMTrackingDetails(SaveESMTrackingDetailsDto trackingDetailsDto, out string Resultvalue, out string errorMessage)
        {
            //bool isSuccess = false;
            Resultvalue = null;
            errorMessage = null;
            DataSet dsTrackingDetailsResult = new DataSet();
            List<ESMTrackerGenerator> listGroupMark = new List<ESMTrackerGenerator>();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();

                    SqlCommand cmd = new SqlCommand(SystemConstant.usp_ESMTrackingDetails_Update, sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Track_Ord_no", trackingDetailsDto.TrakingId));
                    cmd.Parameters.Add(new SqlParameter("@PROJ_ID", trackingDetailsDto.ProjectId));
                    cmd.Parameters.Add(new SqlParameter("@CONTR_NO", trackingDetailsDto.ContractNo));
                    cmd.Parameters.Add(new SqlParameter("@PO_NUM", trackingDetailsDto.PONumber));
                    cmd.Parameters.Add(new SqlParameter("@INTWBSELEMENTID", trackingDetailsDto.WBSElementId));
                    cmd.Parameters.Add(new SqlParameter("@INTSTRUCTUREELEMENTTYPEID", trackingDetailsDto.StructureElementTypeId));
                    cmd.Parameters.Add(new SqlParameter("@INTPRODUCTTYPEID", trackingDetailsDto.ProductTypeId));
                    cmd.Parameters.Add(new SqlParameter("@BBS_NO", trackingDetailsDto.BBSNO));
                    cmd.Parameters.Add(new SqlParameter("@BBS_DESC", trackingDetailsDto.BBSSDesc));
                    cmd.Parameters.Add(new SqlParameter("@REQ_DELI_DATE", trackingDetailsDto.ReqDate.ToString()));
                    cmd.Parameters.Add(new SqlParameter("@INT_REMARK", trackingDetailsDto.IntRemark));
                    cmd.Parameters.Add(new SqlParameter("@EXT_REMARK", trackingDetailsDto.ExtRemark));
                    cmd.Parameters.Add(new SqlParameter("@PO_DATE", trackingDetailsDto.OrdDate.ToString()));
                    cmd.Parameters.Add(new SqlParameter("@PROD_DATE", trackingDetailsDto.ProdDate.ToString()));
                    cmd.Parameters.Add(new SqlParameter("@ORD_TYPE", trackingDetailsDto.OrderType));
                    cmd.Parameters.Add(new SqlParameter("@LOCATION", trackingDetailsDto.Location));
                    cmd.Parameters.Add(new SqlParameter("@OVER_DEL_DELAY", trackingDetailsDto.OverDelTolerance));
                    cmd.Parameters.Add(new SqlParameter("@UNDER_DEL_DELAY", trackingDetailsDto.UnderDelTolerance));
                    cmd.Parameters.Add(new SqlParameter("@SITE_CONTACT_PERSON", trackingDetailsDto.ContactPerson));
                    cmd.Parameters.Add(new SqlParameter("@ESTIMATED_WEIGHT", trackingDetailsDto.EstimatedWeight));

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dsTrackingDetailsResult);


                    if (dsTrackingDetailsResult != null && dsTrackingDetailsResult.Tables.Count != 0)
                    {
                        foreach (DataRowView drvTracking in dsTrackingDetailsResult.Tables[0].DefaultView)
                        {
                            ESMTrackerGenerator groupMark = new ESMTrackerGenerator
                            {
                                ErrorMessage = Convert.ToString(drvTracking["ErrorMessage"]),
                                Result = Convert.ToString(drvTracking["SuccessMessage"]),

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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ESMTrackerGenerator> GetESMTrackingDetails()
        {
            List<ESMTrackerGenerator> listGetTrackingDetails = new List<ESMTrackerGenerator> { };
            DataSet dsGetTrackingDetails = new DataSet();
            try
            {
               IEnumerable<GetESMTrackingDetails> getESMTrackingDetails; //not used
               
               using (var sqlConnection = new SqlConnection(connectionString))
               {

                    sqlConnection.Open();
                    
                    SqlCommand cmd = new SqlCommand(SystemConstant.usp_TrackingDetails_Get_ESM, sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@INTPROJECID", this.ProjectId));
                    cmd.Parameters.Add(new SqlParameter("@STRUCTUREELEMENTTYPEID", this.StructureElementTypeId));
                    cmd.Parameters.Add(new SqlParameter("@SITPRODUCTTYPEID", this.ProductTypeId));

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dsGetTrackingDetails);


                    if (dsGetTrackingDetails != null && dsGetTrackingDetails.Tables.Count != 0)
                    {
                        foreach (DataRowView drvTrackingDetails in dsGetTrackingDetails.Tables[0].DefaultView)
                        {
                            ESMTrackerGenerator ESMTracker = new ESMTrackerGenerator
                            {
                                // PostHeaderId = Convert.ToInt32(drvTrackingDetails["INTPOSTHEADERID"]),
                                // WBSElementId = Convert.ToInt32(drvTrackingDetails["INTWBSELEMENTID"]),
                                TrakingId = Convert.ToString(drvTrackingDetails["ORD_REQ_NO"]),
                                WBS1 = Convert.ToString(drvTrackingDetails["VCHWBS1"]),
                                WBS2 = Convert.ToString(drvTrackingDetails["VCHWBS2"]),
                                WBS3 = Convert.ToString(drvTrackingDetails["VCHWBS3"]),
                                BBSNO = Convert.ToString(drvTrackingDetails["VCHBBSNO"]),
                                BBSSDesc = Convert.ToString(drvTrackingDetails["BBS_DESC"]),
                                Select = false,
                                ProjectId = this.ProjectId,
                                StructureElement = Convert.ToString(drvTrackingDetails["StructureElement"]),
                                ProductType = Convert.ToString(drvTrackingDetails["ProductType"]),
                                // StatusCode = Convert.ToString(drvTrackingDetails["CHRSTATUSCODE"]),
                                // SalesOrderId = Convert.ToString(drvTrackingDetails["IDENTITY_NO"]),
                                //ReqDeliveryDate = Convert.ToString(drvTrackingDetails["REQDATE"])
                                PONumber = Convert.ToString(drvTrackingDetails["PONumber"]),
                                ReqDate = Convert.ToString(drvTrackingDetails["ReqDate"])
                            };
                            listGetTrackingDetails.Add(ESMTracker);
                        }
                    }

                }
              
             
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        
            return listGetTrackingDetails;
        }

        public List<ESMTrackerGenerator> GetESMTrackingDetailsByTrackNum()
        {

            List<ESMTrackerGenerator> listGetTrackingDetails = new List<ESMTrackerGenerator> { };
            DataSet dsGetTrackingDetails = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    //var dynamicParameters = new DynamicParameters();

                    //dynamicParameters.Add("@TrackingNumber", this.TrakingId);
                    //dsGetTrackingDetails = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.usp_TrackingDetailsByID_Get_ESM, dynamicParameters, commandType: CommandType.StoredProcedure);
                    //sqlConnection.Close();

                    SqlCommand cmd = new SqlCommand(SystemConstant.usp_TrackingDetailsByID_Get_ESM, sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@TrackingNumber", this.TrakingId));
                 

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dsGetTrackingDetails);

                    if (dsGetTrackingDetails != null && dsGetTrackingDetails.Tables.Count != 0)
                    {
                        foreach (DataRowView drvTrackingDetails in dsGetTrackingDetails.Tables[0].DefaultView)
                        {
                            ESMTrackerGenerator ESMTracker = new ESMTrackerGenerator
                            {
                                TrakingId = Convert.ToString(drvTrackingDetails["ORD_REQ_NO"]),
                                WBS1 = Convert.ToString(drvTrackingDetails["WBS1"]),
                                WBS2 = Convert.ToString(drvTrackingDetails["WBS2"]),
                                WBS3 = Convert.ToString(drvTrackingDetails["WBS3"]),
                                BBSNO = Convert.ToString(drvTrackingDetails["BBS_NO"]),
                                BBSSDesc = Convert.ToString(drvTrackingDetails["BBS_DESC"]),
                                PONumber = Convert.ToString(drvTrackingDetails["PO_NUM"]),
                                ReqDate = Convert.ToString(drvTrackingDetails["REQ_DELI_DATE"]),
                                OrdDate = Convert.ToString(drvTrackingDetails["ORD_DATE"]),
                                ProdDate = Convert.ToString(drvTrackingDetails["DtProdDate"]),
                                Location = Convert.ToString(drvTrackingDetails["vchLocation"]),
                                OverDelTolerance = Convert.ToDouble(drvTrackingDetails["OverTolerance"]),
                                UnderDelTolerance = Convert.ToDouble(drvTrackingDetails["UnderTolerance"]),
                                ContactPerson = Convert.ToString(drvTrackingDetails["vchContactPerson"]),
                                OrderType = Convert.ToString(drvTrackingDetails["vchOrdType"]),
                                IntRemark = Convert.ToString(drvTrackingDetails["vchRemark"]),
                                ReleasedStatus = Convert.ToString(drvTrackingDetails["vchReleaseStatus"]),
                                //  WBSElementId = Convert.ToInt32(drvTrackingDetails["intWBSElementId"]),
                                //StructureElement = Convert.ToString(drvTrackingDetails["STRUC_ELEM_TYPE"]),
                                //ProductType = Convert.ToString(drvTrackingDetails["PROD_TYPE"]),
                                EstimatedWeight = Convert.ToString(drvTrackingDetails["EstimatedWeight"]),//added by siddhi for estimated weight
                                Select = true
                            };
                            listGetTrackingDetails.Add(ESMTracker);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listGetTrackingDetails;
        }

        public bool GenerateBBSNo()
        {
            bool isSuccess = false;
           
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTPROJECTID", this.ProjectId);
                    dynamicParameters.Add("@INTSTRUCTUREELEMENTTYPEID", this.StructureElementTypeId);
                    dynamicParameters.Add("@INTPRODUCTTYPEID", this.ProductTypeId);
                    
                    this.BBSNO = sqlConnection.QueryFirstOrDefault(SystemConstant.usp_PostingBBSNumberGeneration_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                   
                    isSuccess = true;
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        
            return isSuccess;
        }

        ///
        public bool GetBBSNo()
        {

            bool isSuccess = false;
           
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@WBSELEMENTID", this.WBSElementId);
                   
                    this.BBSNO = sqlConnection.QueryFirstOrDefault(SystemConstant.usp_ESM_BBSNumber_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

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
