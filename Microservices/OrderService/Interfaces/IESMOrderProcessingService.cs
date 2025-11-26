using OrderService.Repositories;
using System.ServiceModel;
using OrderService.Dtos;
namespace OrderService.Interfaces
{
    public interface IESMOrderProcessingService
    {


        List<ESMTrackerGenerator>SaveESMTrackingDetails(SaveESMTrackingDetailsDto ESMTracker, out string Resultvalue, out string errorMessage);

        
        List<ESMTrackerGenerator> GetESMTrackingDetails(int structureElementTypeId, int productTypeId, int projectId, out string errorMessage);

        
        bool GenerateBBSNo(int projectId, int structureElementTypeId, int productTypeId, out string Resultvalue, out string errorMessage);

        
        bool GetBBSNo(int WBSElementID, out string Resultvalue, out string errorMessage);

        
        List<ESMTrackerGenerator> GetESMTrackingDetailsByTrackNum(string TrackNum, out string errorMessage);


        //bool UpdateESMTrackingDetails(SaveESMTrackingDetailsDto ESMTracker, out string Resultvalue, out string errorMessage);
        List<ESMTrackerGenerator>UpdateESMTrackingDetails(SaveESMTrackingDetailsDto ESMTracker, out string Resultvalue, out string errorMessage);
    }
}
