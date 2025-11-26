using System;
using System.Collections.Generic;
using System.Data;
using UtilityService.Dtos;
using UtilityService.Context;
using UtilityService.Interface;

using Microsoft.EntityFrameworkCore;
using UtilityService.Constants;
using Microsoft.Data.SqlClient;

using UtilityService.Repositories;


namespace UtilityService.Repositories
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CopyWBSService" in code, svc and config file together.
   
    public class CopyWBSService : ICopyWBSService
    {
        //string UserName = "" + (new System.Security.Principal.WindowsPrincipal(System.Security.Principal.WindowsIdentity.GetCurrent())).Identity.Name;
        string statusLog = "";


        private UtilityServiceContext _dbContext;
        private readonly IConfiguration _configuration;
        private string connectionString;

        public CopyWBSService(UtilityServiceContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;

            connectionString = _configuration.GetConnectionString(SystemConstant.DefaultDBConnection);
        }


        #region "Get Methods"



        //done
        public List<GetBBSNoDto> GetBBSNoAndBBSDesc(WBSInfoClass wBSInfoClass, int structureElementId, int productTypeId)
        {
             
            //List<GetBBSNoDto> wbsInfoList = new List<GetBBSNoDto>();
            List<GetBBSNoDto> ObjwBSInfoClass = new List<GetBBSNoDto>();
            string errorMessage = "";
            try
            {

                ObjwBSInfoClass = wBSInfoClass.GetBBSNoAndBBSDesc(structureElementId, productTypeId);
                return ObjwBSInfoClass;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return null;
            }
            finally
            {
                //ObjwBSInfoClass = null;
            }
           
        }

        public List<GetDestinationWBS_Insert> GetDestinationWBSDetails(WBSInfoClass wbsInfo, int structureElementId, int productTypeId, string storeyFrom, string storeyTo, out string errorMessage)
        {
            List<GetDestinationWBS_Insert> wbsInfoList = new List<GetDestinationWBS_Insert>();
            errorMessage = "";
            try
            {
                wbsInfoList = wbsInfo.GetDestWBSDetails(structureElementId, productTypeId, storeyFrom, storeyTo);
                return wbsInfoList;
            }
            catch (Exception ex)
            {



                errorMessage = ex.Message;
                return null;
            }
            finally
            {
                wbsInfoList = null;
            }
        }


        public List<ValidateBBSDetailsDto> ValidateBBSDetails(WBSInfoClass wbsInfo, string WBSElements, string BBSNos, string BBSDescriptions, out string errorMessage)
        {
            //List<WBSInfoClass> wbsInfoList = new List<WBSInfoClass>();

            List<ValidateBBSDetailsDto> wbsInfoList = new List<ValidateBBSDetailsDto>();

            errorMessage = "";
            int IsSuccess = 0;
            try
            {
                wbsInfoList = wbsInfo.BBSNo_Validate(WBSElements, BBSNos, BBSDescriptions, out IsSuccess);
                if (IsSuccess == 0)
                {
                    WBSElements = "";
                    BBSNos = "";
                    BBSDescriptions = "";
                    foreach (ValidateBBSDetailsDto item in wbsInfoList)
                    {
                        WBSElements = WBSElements + item.WBSELEMENTID + ",";
                        BBSNos = BBSNos + item.BBSNO + ",";
                        BBSDescriptions = BBSDescriptions + item.BBSDESC + ",";
                    }
                   
                    CopyWBSDetailing(wbsInfo, WBSElements, BBSNos,BBSDescriptions, out errorMessage);//Added for CHG0031776





                    //--------------Commentented for CR-CHG0031776-------------
                    // CopyWBSDetailing(wbsInfo, WBSElements, BBSNos, BBSDescriptions);
                    ////statusLog = "WBS Copied Successfully";
                    ////StatusLog.LogStatus(statusLog, UserName, "Copy WBS");
                    ////errorMessage = "Copied";



                    //----------------------end comment -------------------------
                }
                return wbsInfoList;
            }
            catch (Exception ex)
            {



                errorMessage = ex.Message;
                return null;
            }
            finally
            {
               // wbsInfoList = null;
            }
        }

        public List<WBSInfoClass> GetPostedQuantityandTonnageByWBSDetails(WBSInfoClass wbsInfo, int structureElementId, int productTypeId, string storeyFrom, out List<WBSInfoClass>destinationWBSInfo, out string errorMessage)
        {
            
            List<WBSInfoClass> wbsInfoList = new List<WBSInfoClass>();
            destinationWBSInfo = null;
            errorMessage = "";
            try
            {
                
                wbsInfoList = wbsInfo.GetPostedQuantityandTonnageByPostHeaderId(structureElementId, productTypeId, storeyFrom, out destinationWBSInfo);
            }
            catch (Exception ex)
            {

                errorMessage = ex.Message;
                return null;
            }
            finally
            {
                //ObjwBSInfoClass = null;
            }

            return wbsInfoList;
        }

        public List<GetWBS1> GetWBS1(string ProjectCode,string structureElement, string productType)
        {
            List<GetWBS1> ObjwBS1 = new List<GetWBS1>();
            WBSInfoClass wbsInfo = new WBSInfoClass();
            string errorMessage = "";
            try
            {

                ObjwBS1 = wbsInfo.GetWBS1( ProjectCode,structureElement,  productType);
                return ObjwBS1;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return null;
            }
            finally
            {
                //ObjwBSInfoClass = null;
            }
        }

        public List<GetWBS2> GetWBS2(string ProjectCode,string structureElement, string productType,String WBS1)
        {
            List<GetWBS2> ObjwBS2 = new List<GetWBS2>();
            WBSInfoClass wbsInfo = new WBSInfoClass();
            string errorMessage = "";
            try
            {

                ObjwBS2 = wbsInfo.GetWBS2(ProjectCode,structureElement,productType, WBS1);
                return ObjwBS2;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return null;
            }
            finally
            {
                //ObjwBSInfoClass = null;
            }
        }

        public List<GetWBS3> GetWBS3(string ProjectCode,string structureElement, string productType, String WBS1, String WBS2)
        {
            List<GetWBS3> ObjwBS3 = new List<GetWBS3>();
            WBSInfoClass wbsInfo = new WBSInfoClass();
            string errorMessage = "";
            try
            {

                ObjwBS3 = wbsInfo.GetWBS3(ProjectCode,structureElement,  productType, WBS1, WBS2);
                return ObjwBS3;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return null;
            }
            finally
            {
                //ObjwBSInfoClass = null;
            }
        }


        public List<GetWBS1> GetCopyWBS1(int StructureElementId, int ProductTypeId, int ProjectId, int WBSTypeId)
        {
            List<GetWBS1> ObjwBS1 = new List<GetWBS1>();
            WBSInfoClass wbsInfo = new WBSInfoClass();
            string errorMessage = "";
            try
            {

                ObjwBS1 = wbsInfo.GetCopyWBS1(StructureElementId,ProductTypeId,ProjectId,WBSTypeId);
                return ObjwBS1;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return null;
            }
            finally
            {
                //ObjwBSInfoClass = null;
            }
        }
      
        public List<GetWBS2> GetCopyWBS2(int StructureElementId, int ProductTypeId, int ProjectId, int WBSTypeId, string Block)
        {
            List<GetWBS2> ObjwBS2 = new List<GetWBS2>();
            WBSInfoClass wbsInfo = new WBSInfoClass();
            string errorMessage = "";
            try
            {

                ObjwBS2 = wbsInfo.GetCopyWBS2(StructureElementId, ProductTypeId, ProjectId, WBSTypeId, Block);
                return ObjwBS2;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return null;
            }
            finally
            {
                //ObjwBSInfoClass = null;
            }
        }
        
        public List<GetWBS3> GetCopyWBS3(int StructureElementId, int ProductTypeId, int ProjectId, int WBSTypeId, string Block, string Storey)
        {
            List<GetWBS3> ObjwBS3 = new List<GetWBS3>();
            WBSInfoClass wbsInfo = new WBSInfoClass();
            string errorMessage = "";
            try
            {

                ObjwBS3 = wbsInfo.GetCopyWBS3(StructureElementId, ProductTypeId, ProjectId, WBSTypeId, Block, Storey);
                return ObjwBS3;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return null;
            }
            finally
            {
                //ObjwBSInfoClass = null;
            }
        }



        #endregion



        #region "Save"
        public bool CopyWBSDetailing(WBSInfoClass wbsInfo, string WBSElements, string BBSNos, string BBSDescriptions, out string errorMessage)//Added for CHG0031776
        {
            bool isSuccess = false;
            List<ValidateBBSDetailsDto> wbsInfoList = new List<ValidateBBSDetailsDto>();

            errorMessage = "";
            int IsSuccessValidate = 0;

            errorMessage = "";//Added for CHG0031776
            try
            {
                wbsInfoList = wbsInfo.BBSNo_Validate(WBSElements, BBSNos, BBSDescriptions, out IsSuccessValidate);
                if (IsSuccessValidate == 0)
                {
                    WBSElements = "";
                    BBSNos = "";
                    BBSDescriptions = "";
                    foreach (ValidateBBSDetailsDto item in wbsInfoList)
                    {
                        WBSElements = WBSElements + item.WBSELEMENTID + ",";
                        BBSNos = BBSNos + item.BBSNO + ",";
                        BBSDescriptions = BBSDescriptions + item.BBSDESC + ",";
                    }
                    isSuccess = wbsInfo.CopyWBSDetailing(WBSElements, BBSNos, BBSDescriptions);
                }


                 

                if (isSuccess == false)
                {

                   // statusLog = "GMs have revision of source Modular WBS , please post destination WBS";
                    // StatusLog.LogStatus(statusLog, UserName, "Copy WBS");
                    errorMessage = "Cannot Copy";
                }
                else
                {

                    //statusLog = "WBS Copied Successfully";
                    // StatusLog.LogStatus(statusLog, UserName, "Copy WBS");
                    errorMessage = "Copied";
                }
                //End  for  CHG0031776
                return isSuccess;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }


        #endregion


    }
}
