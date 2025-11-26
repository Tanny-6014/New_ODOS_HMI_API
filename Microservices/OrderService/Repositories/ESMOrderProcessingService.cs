using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using OrderService.Dtos;
using System.Data;
using OrderService.Interfaces;

namespace OrderService.Repositories
{
    public class ESMOrderProcessingService : IESMOrderProcessingService
    {
        string UserName = "" + (new System.Security.Principal.WindowsPrincipal(System.Security.Principal.WindowsIdentity.GetCurrent())).Identity.Name;

        public List<ESMTrackerGenerator>SaveESMTrackingDetails(SaveESMTrackingDetailsDto trackingDetailsDto , out string Resultvalue, out string errorMessage)
        {
            bool isSuccuess = false;
            Resultvalue = "";
            errorMessage = "";
            DataSet dsTrackingDetailsResult = new DataSet();
            ESMTrackerGenerator ESMTracker = new ESMTrackerGenerator();
            List<ESMTrackerGenerator> eSMTrackers = new List<ESMTrackerGenerator>();
            try
            {
                string ProductType = string.Empty;

                if (trackingDetailsDto.ProductTypeId == 4)
                {
                    ProductType = "CAB";
                }
                if (trackingDetailsDto.ProductTypeId == 7)
                {
                    ProductType = "MSH";
                }
                isSuccuess = ESMTracker.GenerateTrackingID(ProductType);
                if (isSuccuess)
                {
                    // isSuccuess = ESMTracker.SaveESMTrackingDetails();
                    eSMTrackers = ESMTracker.SaveESMTrackingDetails(trackingDetailsDto, out  Resultvalue, out  errorMessage);
                }

             
            }
            catch (Exception ex)
            {
                
                errorMessage = ex.Message;
            }
            return eSMTrackers;
           
 
        }

        public List<ESMTrackerGenerator> UpdateESMTrackingDetails(SaveESMTrackingDetailsDto trackingDetailsDto, out string Resultvalue, out string errorMessage)
        {
            bool isSuccuess = false;
            Resultvalue = "";
            errorMessage = "";
            DataSet dsTrackingDetailsResult = new DataSet();
            ESMTrackerGenerator ESMTracker = new ESMTrackerGenerator();
            List<ESMTrackerGenerator> eSMTrackers= new List<ESMTrackerGenerator>();

            try
            {
                string ProductType = string.Empty;

                if (ESMTracker.ProductTypeId == 4)
                {
                    ProductType = "CAB";
                }
                if (ESMTracker.ProductTypeId == 7)
                {
                    ProductType = "MSH";
                }

                eSMTrackers = ESMTracker.UpdateESMTrackingDetails(trackingDetailsDto, out Resultvalue, out errorMessage);

            }
            catch (Exception ex)
            {
                
                errorMessage = ex.Message;
            }
            return eSMTrackers;
        }

        public List<ESMTrackerGenerator> GetESMTrackingDetails(int structureElementTypeId,int productTypeId, int projectId, out string errorMessage)
        {
            List<ESMTrackerGenerator> listESMTrackingDetails = new List<ESMTrackerGenerator>();
            ESMTrackerGenerator objGetESMTracker = new ESMTrackerGenerator();
            errorMessage = "";
            try
            {
                objGetESMTracker.StructureElementTypeId = structureElementTypeId;
                objGetESMTracker.ProjectId = projectId;
                objGetESMTracker.ProductTypeId = productTypeId;
                listESMTrackingDetails = objGetESMTracker.GetESMTrackingDetails();
                return listESMTrackingDetails;
            }
            catch (Exception ex)
            {
                
                errorMessage = ex.Message;
                return null;
            }
            finally
            {
                //listESMTrackingDetails = null;
                //objGetESMTracker = null;
            }
        }

        public bool GenerateBBSNo(int projectId, int structureElementTypeId, int productTypeId, out string Resultvalue, out string errorMessage)
        {
            ESMTrackerGenerator objGenerateBBSNo = new ESMTrackerGenerator();
            bool isSuccuess = false;
            Resultvalue = "";
            errorMessage = "";
            try
            {
                objGenerateBBSNo.ProjectId = projectId;
                objGenerateBBSNo.StructureElementTypeId = structureElementTypeId;
                objGenerateBBSNo.ProductTypeId = productTypeId;
                isSuccuess = objGenerateBBSNo.GenerateBBSNo();
                Resultvalue = objGenerateBBSNo.BBSNO;
            }
            catch (Exception ex)
            {
                
                errorMessage = ex.Message;
            }
            return isSuccuess;
        }


        public bool GetBBSNo(int WBSElementID, out string Resultvalue, out string errorMessage)
        {
            ESMTrackerGenerator objGetBBSNo = new ESMTrackerGenerator();
            bool isSuccuess = false;
            Resultvalue = "";
            errorMessage = "";
            try
            {
                objGetBBSNo.WBSElementId = WBSElementID;
                isSuccuess = objGetBBSNo.GetBBSNo();
                Resultvalue = objGetBBSNo.BBSNO;
            }
            catch (Exception ex)
            {
                
                errorMessage = ex.Message;
            }
            return isSuccuess; throw new NotImplementedException();
        }


        public List<ESMTrackerGenerator> GetESMTrackingDetailsByTrackNum(string TrackNum, out string errorMessage)
        {
            List<ESMTrackerGenerator> listESMTrackingDetails = new List<ESMTrackerGenerator>();
            ESMTrackerGenerator objGetESMTracker = new ESMTrackerGenerator();
            errorMessage = "";
            try
            {
                objGetESMTracker.TrakingId = TrackNum;
                listESMTrackingDetails = objGetESMTracker.GetESMTrackingDetailsByTrackNum();
                return listESMTrackingDetails;
            }
            catch (Exception ex)
            {
                
                errorMessage = ex.Message;
                return null;
            }
            //finally
            //{
            //    listESMTrackingDetails = null;
            //    objGetESMTracker = null;
            //}
        }
    }
}
