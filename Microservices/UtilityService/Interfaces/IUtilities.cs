using UtilityService.Dtos;
//using UtilityService.Repositories; 

namespace UtilityService.Interface
{
    public interface IUtilities
    {
        //public  System.Data.DataSet GetStorey(UtilitiesInfo obj);
        //public  System.Data.DataSet GetHeaderBlock(UtilitiesInfo obj);
        //public  System.Data.DataSet GetWBS1(UtilitiesInfo obj);
        //public  int InsertCopyEstimation(UtilitiesInfo obj);
        //public  System.Data.DataSet GetWBSId(UtilitiesInfo obj);
        //public  System.Data.DataSet GetCustomer();
        //public  System.Data.DataSet GetSapContractConvert();
        //public  int InsertSapContractConvert(UtilitiesInfo obj);
        //public  System.Data.DataSet GetSapProjectConvert();
        //public  int InsertSapProjectConvert(UtilitiesInfo obj);
        //public  System.Data.DataSet GetContract(UtilitiesInfo obj);
        
        //List<UtilitiesInfo> CopyProjectParameterSetGet(int ProjectId, int ProductTypeId, int MeshFlag, string SElement);
        Task<IEnumerable<getCopyParameterSetDto>> CopyProjectParameterSetGet(int ProjectId, int ProductTypeId, int MeshFlag, string SElement);

        //public  System.Data.DataSet CopyProjectParamLabelGet(UtilitiesInfo obj);
        //public  int InsertCopyProjectParameter(UtilitiesInfo obj);
        //public  int InsertCopyDetailing(UtilitiesInfo obj);
        //public  System.Data.DataSet CopyParamGet_Get(UtilitiesInfo obj);
        //public  System.Data.DataSet GetGroupmarkingRevQuan(UtilitiesInfo obj);
        //public  System.Data.DataSet GetCopyGroupmarkingRevision(UtilitiesInfo obj);
        //public  System.Data.DataSet GetCopySCTB(UtilitiesInfo obj);
        //public  System.Data.DataSet GetDestParamDrop(UtilitiesInfo obj);
        //public  System.Data.DataSet GetNonEstimatedStorey(UtilitiesInfo obj);
        //public  System.Data.DataSet GetEstimatedStorey(UtilitiesInfo obj);
        //public  System.Data.DataSet GetNonEstimatedStoreyDest(UtilitiesInfo obj);
        //public  System.Data.DataSet GetEstimatedStoreyDest(UtilitiesInfo obj);
        //public  System.Data.DataSet GetEstimatedWBS1(UtilitiesInfo obj);
        //public  System.Data.DataSet GetSourceWBS3(UtilitiesInfo obj);
        //public  System.Data.DataSet GetDestinationWBS3(UtilitiesInfo obj);
        //public  System.Data.DataSet GetCopyWBSId(UtilitiesInfo obj);
        //Task<int> InsertCopyDetailing(UtilitiesInfo obj);

        Task<int> InsertCopyProjectParameter(InsertCopyProjectParamDto obj);
    }

}

