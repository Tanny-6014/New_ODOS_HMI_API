using DetailingService.Dtos;
using DetailingService.Repositories;
namespace DetailingService.Interfaces

{
    public interface IAccessories
    {

     
        List<SAPMaterial> GetSAPMaterialDetails(string sapMaterialCode, out string errorMessage);
        
        List<Accessory> GetCABItems(int intSEDetailingID, out string errorMessage);
       
        List<Accessory> GetACCProductMarkDetailsBySEDetailingID(int intSEDetailingID, out string errorMessage);
   
        int InsUpdACCProdMarkDetails(Accessory accessoryItem, out string errorMessage);
       
        string GetCustomerIP(out string errorMessage);
      
        bool DeleteACCProductMarkDetail(int AccProductMarkID);

        //Task<IEnumerable<GetGroupMarkListDto>> GetMeshDetailingListAsync(int ProjectID);
    }

}
