
using DrainService.Repositories;

namespace DrainService.Interfaces
{
    
        public interface IBorePileParameterSetService
        {
           
            

           
            List<BorePileParameterSet> GetParameterSetDetails(int ProjectId, out string errorMessage);

           
            int InsertParameterSet(int ProjectId, int ProductTypeId, int UserId, out string errorMessage);

           
            List<BorePileParameterSet> UpdateParameterSet(int ProjectId, BorePileParameterSet borePilePS, out string errorMessage);

           
            //List<ParameterSet> GetTransport(out string errorMessage);

           
            List<BorePileParameterSet> GetProductType(out String errorMessage);
        }

    
}
