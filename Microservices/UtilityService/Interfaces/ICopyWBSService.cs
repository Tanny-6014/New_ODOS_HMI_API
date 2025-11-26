
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using UtilityService.Dtos;
using UtilityService.Repositories;

namespace UtilityService.Interface
{
   
    public interface ICopyWBSService
    {

        List<GetBBSNoDto> GetBBSNoAndBBSDesc(WBSInfoClass wBSInfoClass, int structureElementId, int productTypeId);

        List<GetDestinationWBS_Insert> GetDestinationWBSDetails(WBSInfoClass wbsInfo, int structureElementId, int productTypeId, string storeyFrom, string storeyTo, out string errorMessage);

        List<ValidateBBSDetailsDto> ValidateBBSDetails(WBSInfoClass wbsInfo, string WBSElements, string BBSNos, string BBSDescriptions, out string errorMessage);

        bool CopyWBSDetailing(WBSInfoClass wbsInfo, string WBSElements, string BBSNos, string BBSDescriptions, out string errorMessage);

        //WBSInfoClass GetPostedQuantityandTonnageByWBSDetails(WBSInfoClass wbsInfo,int structureElementId, int productTypeId, string storeyFrom, out WBSInfoClass destinationWBSInfo);
        List<WBSInfoClass> GetPostedQuantityandTonnageByWBSDetails(WBSInfoClass wbsInfo, int structureElementId, int productTypeId, string storeyFrom, out List<WBSInfoClass> destinationWBSInfo, out string errorMessage);

        public List<GetWBS1> GetWBS1(string ProjectCode,string structureElement, string productType);

        public List<GetWBS2> GetWBS2(string ProjectCode,string structureElement, string productType,string WBS1);

        public List<GetWBS3> GetWBS3(string ProjectCode,string structureElement, string productType, string WBS1, string WBS2);

        public List<GetWBS1> GetCopyWBS1(int StructureElementId, int ProductTypeId, int ProjectId, int WBSTypeId);
        public List<GetWBS2> GetCopyWBS2(int StructureElementId, int ProductTypeId, int ProjectId, int WBSTypeId, string Block);
        public List<GetWBS3> GetCopyWBS3(int StructureElementId, int ProductTypeId, int ProjectId, int WBSTypeId, string Block, string Storey);




    }
}
