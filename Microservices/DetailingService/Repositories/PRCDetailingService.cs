using DetailingService.Interfaces;
using System.Data;

namespace DetailingService.Repositories
{
    public class PRCDetailingService : IPRCDetailingService
    {
       
        public void DoWork()
        {
        }

        #region "Load Header Details"

        public List<BeamDetailinfo> GetCustomerContractProjectByGroupMarkId(int GroupMarkId, out List<GroupMark> listHeaderDetails, out string errorMessage)        // Get PRC Detailing Header Details By Group Mark Id
        {
            List<BeamDetailinfo> listBeamDetailInfo = new List<BeamDetailinfo>();
            BeamDetailinfo objBeamDetailInfo = new BeamDetailinfo();
            listHeaderDetails = null;
            errorMessage = "";
            try
            {
                objBeamDetailInfo.intGroupMarkId = GroupMarkId;
                listBeamDetailInfo = objBeamDetailInfo.GetCustContProjByGroupMarkID(out listHeaderDetails);
            }
            catch (Exception ex)
            {
               // ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
            }
            finally
            {
                objBeamDetailInfo = null;
            }
            return listBeamDetailInfo;
        }

        public List<BeamDetailinfo> GetCustomerContractProjectByProjectId(int ProjectId, out string errorMessage)            // Get PRC Detailing Header Details By Project Id
        {
            List<BeamDetailinfo> listBeamDetailInfo = new List<BeamDetailinfo>();
            BeamDetailinfo objBeamDetailInfo = new BeamDetailinfo();
            errorMessage = "";
            try
            {
                listBeamDetailInfo = objBeamDetailInfo.GetCustContProjByProjectID(ProjectId);

            }
            catch (Exception ex)
            {
               // ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
            }
            finally
            {
                objBeamDetailInfo = null;
            }
            return listBeamDetailInfo;
        }

        #endregion

        #region "Load Header Default Values"

        public List<GroupMark> GetHeaderValuesByGroupMarkId(int intGroupMarkId, out string errorMessage)
        {
            List<GroupMark> listHeaderDetails = new List<GroupMark>();
            GroupMark objGroupMark = new GroupMark();
            errorMessage = "";
            try
            {
                listHeaderDetails = objGroupMark.GetPRCHeaderValuesByGroupMarkID(intGroupMarkId);

            }
            catch (Exception ex)
            {
               // ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
            }
            finally
            {
                objGroupMark = null;
            }
            return listHeaderDetails;
        }

        public List<GroupMark> GetPRCDefaultValuesByStructureElementId(int intStructureElementId, out string errorMessage)   // Get PRC Default By Structure Element Id
        {
            List<GroupMark> listPRCDefaultValues = new List<GroupMark>();
            GroupMark objGroupMark = new GroupMark();
            errorMessage = "";
            try
            {
                listPRCDefaultValues = objGroupMark.GetPRCDefaultValuesByStructureElementId(intStructureElementId);

            }
            catch (Exception ex)
            {
               // ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
            }
            finally
            {
                objGroupMark = null;
            }
            return listPRCDefaultValues;
        }

        #endregion

        #region "Load and save WBF info"

        //public List<WBSInfoClass> GetWBFInfoByGroupMarkId(int GroupMarkId, out string errorMessage)                          // Get WBF Info By Group Mark Id
        //{
        //    List<WBSInfoClass> listWBSInfo = new List<WBSInfoClass>();
        //    WBSInfoClass objWBSInfo = new WBSInfoClass();
        //    errorMessage = "";
        //    try
        //    {
        //        listWBSInfo = objWBSInfo.WBSInfoByGroupID_Get(GroupMarkId);

        //    }
        //    catch (Exception ex)
        //    {
        //       // ExceptionLog.LogException(ex, UserName);
        //        errorMessage = ex.Message;
        //    }
        //    finally
        //    {
        //        objWBSInfo = null;
        //    }
        //    return listWBSInfo;
        //}

        //public List<WBSInfoClass> GetWBFInfoByElementId(string strWBSValueParameter, out string errorMessage)                // Get WBF Info By Element Id
        //{
        //    List<WBSInfoClass> listWBSInfo = new List<WBSInfoClass>();
        //    WBSInfoClass objWBSInfo = new WBSInfoClass();
        //    errorMessage = "";
        //    try
        //    {
        //        listWBSInfo = objWBSInfo.WBSInfoByElementId_Get(strWBSValueParameter);
        //    }
        //    catch (Exception ex)
        //    {
        //       // ExceptionLog.LogException(ex, UserName);
        //        errorMessage = ex.Message;
        //    }
        //    finally
        //    {
        //        objWBSInfo = null;
        //    }
        //    return listWBSInfo;
        //}

        public bool SaveWBSInfo(string InputValueParameter, int GroupMarkId, int GroupRevNumber, out string errorMessage)    // To Save the WBS Info
        {
            bool isSuccuess = false;
            GroupMark objGroupMark = new GroupMark();
            errorMessage = "";
            try
            {
                string[] split = InputValueParameter.Split(',');
                objGroupMark.GroupMarkId = GroupMarkId;
                objGroupMark.GroupRevisionNumber = GroupRevNumber;

                foreach (string item in split)
                {
                    objGroupMark.WBSElementId = Convert.ToInt32(item);
                    isSuccuess = objGroupMark.SaveWBSDetail();
                }
            }
            catch (Exception ex)
            {
               // ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
            }
            finally
            {
                objGroupMark = null;
            }
            return isSuccuess;
        }

        #endregion

        #region "Validate Posted GM"

        public int ValidatedPostedGM(int GroupMarkId, out string errorMessage)                                               // Validate Posted Group Marking 
        {
            int intRecordCount = 0;
            BeamDetailinfo objBeamDetailInfo = new BeamDetailinfo();
            errorMessage = "";
            try
            {
                objBeamDetailInfo.intGroupMarkId = Convert.ToInt32(GroupMarkId);
                intRecordCount = objBeamDetailInfo.PostedBeamByGroupmark_Get();
            }
            catch (Exception ex)
            {
               // ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
            }
            finally
            {
                objBeamDetailInfo = null;
            }
            return intRecordCount;
        }

        #endregion

        #region "Load GM details"

        public List<BeamDetailinfo> GetGroupMarkingDetailsByGroupMarkingId(int GroupMarkId, out string errorMessage)         // Get Group Marking Details By Group Marking Id
        {
            List<BeamDetailinfo> listBeamDetailInfo = new List<BeamDetailinfo>();
            BeamDetailinfo objBeamDetailInfo = new BeamDetailinfo();
            errorMessage = "";
            try
            {
                objBeamDetailInfo.intGroupMarkId = GroupMarkId;
                listBeamDetailInfo = objBeamDetailInfo.GroupMarkingDetailsByGroupID_Get();

            }
            catch (Exception ex)
            {
               // ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
            }
            finally
            {
                objBeamDetailInfo = null;
            }
            return listBeamDetailInfo;
        }

        #endregion

        #region "Load SAP material"

        public List<SAPMaterial> GetSAPMaterialByStructureElementId(int intStructureElementId, out string errorMessage)      // Get SAP Material By Structure Element Id
        {
            SAPMaterial objSAPMAterial = new SAPMaterial();
            List<SAPMaterial> listSAPMaterial = new List<SAPMaterial>();
            errorMessage = "";

            try
            {
                listSAPMaterial = objSAPMAterial.GetSAPMaterialforPRCByStructureElementId(intStructureElementId);
            }
            catch (Exception ex)
            {
               // ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
            }
            finally
            {
                objSAPMAterial = null;
            }
            return listSAPMaterial;
        }

        #endregion

        #region "Save GM"

        public List<GroupMark> SaveGroupMarkDetails(GroupMark groupMark,int? Selector_id, out string errorMessage)                            // To Save a Group Marking Details
        {
            List<GroupMark> listSaveGroupMark = new List<GroupMark>();
            errorMessage = "";
            try
            {
                listSaveGroupMark = groupMark.SavePRCGroupMarkingDetails(Selector_id);

            }
            catch (Exception ex)
            {
               // ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
            }
            return listSaveGroupMark;
        }

        #endregion

        #region "Filter GM Names"

        public List<GroupMark> FilterGroupMarkName(string enteredText, int ProjectId, int StructureElementTypeId, out string errorMessage)
        {
            List<GroupMark> listGroupMark = new List<GroupMark>();
            GroupMark objGroupMark = new GroupMark();
            errorMessage = "";
            try
            {
                listGroupMark = objGroupMark.FilterGroupMarkNameForPRC_Get(enteredText, ProjectId, StructureElementTypeId);
            }
            catch (Exception ex)
            {
               // ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
            }
            finally
            {
                objGroupMark = null;
            }
            return listGroupMark;
        }

        #endregion

        #region "Load Transport details"

        public List<ParameterSet> GetAllTransport(out string errorMessage)         // To Get the Transport
        {
            List<ParameterSet> listGetTransport = new List<ParameterSet>();
            ParameterSet objParameterSet = new ParameterSet();
            errorMessage = "";
            try
            {
                listGetTransport = objParameterSet.GetTransport();
            }
            catch (Exception ex)
            {
               // ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
            }
            finally
            {
                objParameterSet = null;
            }
            return listGetTransport;
        }

        #endregion

        #region start for dwall prc posted report
        public bool insertcabproductmark(int groupmarkid, out string errorMessage)
        {
            bool isSuccuess = false;
            errorMessage = "";
            ShapeParameter objshape = new ShapeParameter();
            try
            {
                objshape.insertcabproductmark(groupmarkid);
            }
            catch (Exception ex)
            {
               // ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
            }
            return isSuccuess;
        }
        //public bool insertshape(int groupmarkid, out string errorMessage)
        //{
        //    bool isSuccuess = false;
        //    errorMessage = "";
        //    ShapeParameter objshape = new ShapeParameter();
        //    ImageBuilderDWALL objimage = new ImageBuilderDWALL();
        //    DataSet dtcabproductmark = new DataSet();
        //    try
        //    {
        //        dtcabproductmark = objshape.getcabproductmark(groupmarkid);
        //        foreach (DataTable dt in dtcabproductmark.Tables)
        //        {
        //            foreach (DataRow dr in dt.Rows)
        //            {
        //                string shapecode = Convert.ToString(dr["intshapecode"]);
        //                int productmark = Convert.ToInt32(dr["intcabproductmarkid"]);
        //                objimage.CreateImage(shapecode, productmark);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //       // ExceptionLog.LogException(ex, UserName);
        //        errorMessage = ex.Message;
        //    }
        //    return isSuccuess;
        //}
       
        
        #endregion
    }
}
