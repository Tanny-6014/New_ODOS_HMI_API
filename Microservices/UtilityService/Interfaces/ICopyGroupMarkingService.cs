

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
    
    public interface ICopyGroupMarkingService
    {
        
        void DoWork();

        
        List<GetRevisionAndParamValuesDto> GetSourceParamValues(GroupMark groupMark, out string errorMessage);

        
        List<Groupmarking_Name> GetGroupMarkName(GroupMark groupMark, out string errorMessage);

        
        string CheckDestParameterSetBySourceParameterSet(int structureElementId, int SourceParameterSetId, int DestParameterSetId, int SourceProjectId, int DestinationProjectId, out string errorMessage);

        
        int CopyGM_DestinationGM_Check(GroupMark groupMark, out string errorMessage);

        
        int CreateDestinationParameterSet(GroupMark groupMark, int sourceProjectId, int destProjectId, int sourceParameterSetId, int destParameterSetId);

        
        bool CopyGroupMarking(GroupMark groupMark, string productType, int sourceProjectId, int destProjectId, int sourceParameterSetId, int destParameterSetId, int sourceGroupMarkId, string destGroupMarkName, string copyFrom, string wbsElements,bool IsParameterSetCreationRequired,int IsGroupMarkRevision, out string errorMessage, out int destRevisionNo);
    }
}
