using DetailingService.Dtos;
using DetailingService.Repositories;
using System.Data;

namespace DetailingService.Interfaces
{
    public interface IGroupMarkDal
    {
        //Task<IEnumerable<GetGroupMarkListDto>> GetGroupMarkListing(GroupMarkInfo objGroupMarkInfo);
        //public int InsertBeamExcel(GroupMarkInfo objGroupMarkInfo);
        //public int InsertColumnExcel(GroupMarkInfo objGroupMarkInfo);
        //public int InsertExcel(GroupMarkInfo objGroupMarkInfo);
        public List<GroupMarkInfo> GetProductType(int intProductType,int intGroupMarkId);
        //public int DelGroupMark(GroupMarkInfo objGroupMarkInfo);
        //public DataSet GetBeamExportToTxt(GroupMarkInfo objGroupMarkInfo);
        //public DataSet GetSlabExportToTxt(GroupMarkInfo objGroupMarkInfo);
       
        public int UpdateGMINPostGM(GroupMarkInfo objGroupMarkInfo);
       
        //public int ImportParameterCheck(GroupMarkInfo objGroupMarkInfo);
        //// added for ship to party
        //public abstract DataSet GetGroupMarkListing_new(GroupMarkInfo objGroupMarkInfo);
        //// Added for Core Cage
        //public List<ProductCode> GetProductCodeIdForGroupMark(int GroupMarkId);
       
        List<GetPostedGMDto> GetPostedGroupMark(int intGroupMarkId);
        List<GetReleasedGMDto> GetReleasedGroupMark(int intGroupMarkId);

        public GetGmDetailsForCall CallStructMarkListbyGMID(int GMID);



    }





}