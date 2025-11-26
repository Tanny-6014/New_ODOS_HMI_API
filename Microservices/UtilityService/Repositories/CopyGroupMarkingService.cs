

using System.Data;
using System.Transactions;
using UtilityService.Dtos;
using UtilityService.Interface;

namespace UtilityService.Repositories
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CopyGroupMarkingService" in code, svc and config file together.
    
    public class CopyGroupMarkingService : ICopyGroupMarkingService
    {
       // string UserName = "" + (new System.Security.Principal.WindowsPrincipal(System.Security.Principal.WindowsIdentity.GetCurrent())).Identity.Name;

        public CopyGroupMarkingService()
        { }
        public void DoWork()
        {
        }
        
        public List<GetRevisionAndParamValuesDto> GetSourceParamValues(GroupMark groupMark, out string errorMessage)
        {
            List<GetRevisionAndParamValuesDto> groupMarkList = new List<GetRevisionAndParamValuesDto>();
            
            errorMessage = "";
            try
            {
                groupMarkList = groupMark.GetRevisionAndParameterValuesByGroupMarkName();
                return groupMarkList;
            }
            catch (Exception ex)
            {
                
                errorMessage = ex.Message;
                return null;
            }
            finally
            {
                //groupMarkList = null;
            }
        }

        public List<Groupmarking_Name> GetGroupMarkName(GroupMark groupMark, out string errorMessage)
        {
            List<Groupmarking_Name> groupMarkList = new List<Groupmarking_Name>();
            errorMessage = "";
            try
            {
                groupMarkList = groupMark.GetGroupMarkNameByProjId();
                return groupMarkList;
            }
            catch (Exception ex)
            {
                
                errorMessage = ex.Message;
                return null;
            }
            finally
            {
                //groupMarkList = null;
            }
        }
       
        public string CheckDestParameterSetBySourceParameterSet(int structureElementId, int SourceParameterSetId, int DestParameterSetId, int SourceProjectId, int DestinationProjectId, out string errorMessage)
        {
            string Output = null;
            errorMessage = "";
            GroupMark groupMark = new GroupMark();
            try
            {
                Output = groupMark.CheckDestinationParameterSet(structureElementId, SourceParameterSetId, DestParameterSetId, SourceProjectId, DestinationProjectId);
                return Output;
            }
            catch (Exception ex)
            {
                
                errorMessage = ex.Message;
                return null;
            }
            finally
            {
                groupMark = null;
            }
        }

        public int CopyGM_DestinationGM_Check(GroupMark groupMark, out string errorMessage)
        {
            int Output = 0;
            errorMessage = "";
            try
            {
                Output = groupMark.CopyGM_DestinationGM_Check();
                return Output;
            }
            catch (Exception ex)
            {
                
                errorMessage = ex.Message;
                return 0;
            }
            finally
            {
                groupMark = null;
            }
        }

        public int CreateDestinationParameterSet(GroupMark groupMark, int sourceProjectId, int destProjectId, int sourceParameterSetId, int destParameterSetId)
        {
            int destParameterSet;
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    destParameterSet = groupMark.CopyGM_CreateDestinationParameterSet(sourceProjectId, destProjectId, sourceParameterSetId, destParameterSetId);
                    ts.Complete();
                }
                return destParameterSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                groupMark = null;
            }
        }

        public int CreateNewDestinationParameterSet(GroupMark groupMark, int sourceProjectId, int destProjectId, int sourceParameterSetId)
        {
            int destParameterSet;
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    destParameterSet = groupMark.CopyGM_NewCreateDestinationParameterSet(sourceProjectId, destProjectId, sourceParameterSetId);
                    ts.Complete();
                }
                return destParameterSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                groupMark = null;
            }
        }

        public bool CopyGroupMarking(GroupMark groupMark, string productType, int sourceProjectId, int destProjectId, int sourceParameterSetId, int destParameterSetId, int sourceGroupMarkId, string destGroupMarkName, string copyFrom, string wbsElements,bool IsParameterSetCreationRequired,int IsGroupMarkRevision, out string errorMessage, out int destRevisionNumber)
        {
            bool IsCopyGMSuccess = false;
            string destGroupMarkResult=null;
            int destGroupMarkId = 0;
            int destRevisonNo = 0;
            int armaValidate = 0;
            int updatePostedGMforRevisedGM = 0;
            //string wbsElements = null;
            string statusLog = "";
            errorMessage = "";
            destRevisionNumber = 0;
            try
            {
               
              //  wbsElements = groupMark.CopyGM_GetCopyWBSElementId(wbs1, wbs2);
                if ((productType == "PRC") || (productType == "BPC"))
                {
                    /* Validate the Source NDS & Arma+ Quantity. */
                    armaValidate = groupMark.CopyGM_ArmaValidator(groupMark.GroupMarkId);
                    //armaValidate = 1;
                    if (armaValidate == 1)
                    {
                        statusLog = "ODOS and Arma+ Quantity Matched for Souce GroupMarking";
                        
                        if (IsParameterSetCreationRequired == true)
                        {
                            /* Create Destination Parameter Set */
                            /*Changed CreateDestinationParameterSet to CreateNewDestinationParameterSet for CHG30031332 */
                            destParameterSetId = CreateNewDestinationParameterSet(groupMark, sourceProjectId, destProjectId, sourceParameterSetId);
                            destGroupMarkResult = groupMark.CopyGM_CopyGroupMarking_New(sourceProjectId, destProjectId, sourceGroupMarkId, destGroupMarkName, sourceParameterSetId, destParameterSetId, copyFrom, wbsElements, IsGroupMarkRevision);

                            statusLog = "Destination Paramter Id : " + destParameterSetId + " is created for the Project " + destProjectId;
                            //StatusLog.LogStatus(statusLog, UserName, "Copy Group Marking");
                        }
                        /* If source & destination are Same project */
                        if (sourceProjectId == destProjectId)
                        {
                            /* Copy Group Marking */
                            destGroupMarkResult = groupMark.CopyGM_CopyGroupMarking(sourceProjectId, destProjectId, sourceGroupMarkId, destGroupMarkName, sourceParameterSetId, sourceParameterSetId, copyFrom, wbsElements, IsGroupMarkRevision);
                            if (destGroupMarkResult.Contains(","))
                            {
                                string[] Output = destGroupMarkResult.Split(',');
                                destGroupMarkId = Convert.ToInt32(Output[0]);
                                destRevisonNo = Convert.ToInt32(Output[1]);
                            }
                            statusLog = "Group Mark Copied Successfully, Group Mark Id : " + destGroupMarkId + " is created for the Project " + destProjectId;
                            //StatusLog.LogStatus(statusLog, UserName, "Copy Group Marking");
                        }
                        else
                        {
                            /* Copy Group Marking */
                            
                            if (IsParameterSetCreationRequired != true)
                            {

                                destGroupMarkResult = groupMark.CopyGM_CopyGroupMarking(sourceProjectId, destProjectId, sourceGroupMarkId, destGroupMarkName, sourceParameterSetId, destParameterSetId, copyFrom, wbsElements, IsGroupMarkRevision);

                            }
                           

                            if (destGroupMarkResult != null)
                            {

                                if (destGroupMarkResult.Contains(","))
                                {
                                    string[] Output = destGroupMarkResult.Split(',');
                                    destGroupMarkId = Convert.ToInt32(Output[0]);
                                    destRevisonNo = Convert.ToInt32(Output[1]);
                                }
                                statusLog = "Group Mark Copied Successfully, Group Mark Id : " + destGroupMarkId + " is created for the Project " + destProjectId;
                                
                            }
                            else 
                            {

                                errorMessage = "Cannot Copy GroupMark as the Source and Destination Parameter is Different";

                            }             

                        }

                        /* Validate the Destination NSA & Arma Quantity */
                        armaValidate = groupMark.CopyGM_ArmaValidator(destGroupMarkId);
                        //armaValidate = 1;
                        if (armaValidate == 1)
                        {
                            statusLog = "ODOS and Arma+ Quantity Matched for Destination GroupMarking";
                            //StatusLog.LogStatus(statusLog, UserName, "Copy Group Marking");
                            if (IsGroupMarkRevision == 1)
                            {
                                if (destGroupMarkResult != null)//Added for CR-CHG30031332
                                {
                                    if (destGroupMarkResult.Contains(","))
                                    {
                                        string[] Output = destGroupMarkResult.Split(',');
                                        destGroupMarkId = Convert.ToInt32(Output[0]);
                                        destRevisonNo = Convert.ToInt32(Output[1]);
                                    }

                                    /* If the GM's posted, Update the Post Header Details to latest Group Mark details. */
                                    updatePostedGMforRevisedGM = groupMark.CopyGM_UpdatePostedGMforRevisedGM(sourceGroupMarkId, destGroupMarkId, destRevisonNo);
                                    if (updatePostedGMforRevisedGM == 1)
                                    {
                                        IsCopyGMSuccess = true;
                                        statusLog = "Posted GroupMark Revision Updated Successfully.";
                                        //StatusLog.LogStatus(statusLog, UserName, "Copy Group Marking");
                                    }
                                    else if (updatePostedGMforRevisedGM == 2)
                                    {
                                        IsCopyGMSuccess = true;
                                        statusLog = "Posted GroupMark Revision Updated Successfully.";
                                    }
                                    else
                                    {
                                        errorMessage = "Error while update the Revision Number.";
                                        statusLog = errorMessage;
                                        throw new Exception(errorMessage);
                                    }
                                }
                                else
                                {
                                    IsCopyGMSuccess = false;
                                }
                            }
                            else
                            {
                                if (destGroupMarkResult != null)
                                {
                                    IsCopyGMSuccess = true;
                                }
                                else
                                {
                                    IsCopyGMSuccess = false;
                                }
                            }
                            if (IsCopyGMSuccess != false)
                            {
                                IsCopyGMSuccess = true;
                            }
                            else
                            {
                                IsCopyGMSuccess = false;
                            }

                        }
                        else
                        {
                            errorMessage = "ODOS and Arma+ Quantity Mismatched for Destination GroupMarking, Copy Fail.";
                            statusLog = errorMessage;
                            throw new Exception(errorMessage);
                        }
                    }
                    else
                    {
                        errorMessage = "ODOS and Arma+ Quantity Mismatched for Souce GroupMarking, Copy Fail.";
                        statusLog = errorMessage;
                        
                        throw new Exception(errorMessage);
                    }
                }
                else
                {
                    
                    if (IsParameterSetCreationRequired == true)
                    {

                        /* Create Destination Parameter Set */
                        /*Changed CreateDestinationParameterSet to CreateNewDestinationParameterSet  for CHG30031332 */
                        destParameterSetId = CreateNewDestinationParameterSet(groupMark, sourceProjectId, destProjectId, sourceParameterSetId);
                        //, destGroupMarkName, copyFrom, wbsElements);
                        destGroupMarkResult = groupMark.CopyGM_CopyGroupMarking_New(sourceProjectId, destProjectId, sourceGroupMarkId, destGroupMarkName, sourceParameterSetId, destParameterSetId, copyFrom, wbsElements, IsGroupMarkRevision);
                        /* End Change for CHG30031332 */

                        statusLog = "Destination Paramter Id : " + destParameterSetId + " is created for the Project " + destProjectId;
                        //StatusLog.LogStatus(statusLog, UserName, "Copy Group Marking");
                    }
                    /* If source & destination are Same project */
                    if (sourceProjectId == destProjectId)
                    {
                        /* Copy Group Marking */
                        destGroupMarkResult = groupMark.CopyGM_CopyGroupMarking(sourceProjectId, destProjectId, sourceGroupMarkId, destGroupMarkName, sourceParameterSetId, sourceParameterSetId, copyFrom, wbsElements, IsGroupMarkRevision);
                        if (destGroupMarkResult.Contains(","))
                        {
                            string[] Output = destGroupMarkResult.Split(',');
                            destGroupMarkId = Convert.ToInt32(Output[0]);
                            destRevisonNo = Convert.ToInt32(Output[1]);

                        }
                        statusLog = "Group Mark Copied Successfully, Group Mark Id : " + destGroupMarkId + " is created for the Project " + destProjectId;
                    }
                    else
                    {
                        /* Copy Group Marking */

                        if (IsParameterSetCreationRequired != true)
                        {

                            destGroupMarkResult = groupMark.CopyGM_CopyGroupMarking(sourceProjectId, destProjectId, sourceGroupMarkId, destGroupMarkName, sourceParameterSetId, destParameterSetId, copyFrom, wbsElements, IsGroupMarkRevision);
                        }
                        
                        if (destGroupMarkResult != null)
                        {
                            if (destGroupMarkResult.Contains(","))
                            {
                                string[] Output = destGroupMarkResult.Split(',');
                                destGroupMarkId = Convert.ToInt32(Output[0]);
                                destRevisonNo = Convert.ToInt32(Output[1]);
                            }

                            statusLog = "Group Mark Copied Successfully, Group Mark Id : " + destGroupMarkId + " is created for the Project " + destProjectId;
                            
                        }

                        else 
                        {

                            errorMessage = "Cannot Copy GroupMark as the Source and Destination Parameter is Different";

                        }

                    }

                    if (IsGroupMarkRevision == 1)
                    {
                        if (destGroupMarkResult != null)
                        {
                            if (destGroupMarkResult.Contains(","))
                            {
                                string[] Output = destGroupMarkResult.Split(',');
                                destGroupMarkId = Convert.ToInt32(Output[0]);
                                destRevisonNo = Convert.ToInt32(Output[1]);
                            }
                            /* If the GM's posted, Update the Poste Header Details to latest Group Mark details. */
                            updatePostedGMforRevisedGM = groupMark.CopyGM_UpdatePostedGMforRevisedGM(sourceGroupMarkId, destGroupMarkId, destRevisonNo);
                            if (updatePostedGMforRevisedGM == 1)
                            {
                                IsCopyGMSuccess = true;
                                statusLog = "Posted GroupMark Revision Updated Successfully.";
                               
                            }
                            else if (updatePostedGMforRevisedGM == 2)
                            {
                                IsCopyGMSuccess = true;
                                statusLog = "Posted GroupMark Revision Updated Successfully.";
                               
                            }
                            else
                            {
                                errorMessage = "Error while update the Revision Number.";
                                statusLog = errorMessage;
                              
                                throw new Exception(errorMessage);
                            }
                        }
                        else
                        {
                            IsCopyGMSuccess = false;
                        }
                    }
                   
                    else
                    {
                        if (destGroupMarkResult != null)
                        {
                            IsCopyGMSuccess = true;
                        }
                        else
                        {
                            IsCopyGMSuccess = false;
                        }
                    }
                    if (IsCopyGMSuccess != false)
                    {
                        IsCopyGMSuccess = true;
                    }
                    else
                    {
                        IsCopyGMSuccess = false;
                    }

                  
                }
                destRevisionNumber = destRevisonNo;
                return IsCopyGMSuccess;
            }
            catch (Exception ex)
            {
                
                errorMessage = ex.Message;
                return false;
            }
            finally
            {
                //groupMark = null;
            }
        }
    }
}
