using OrderService.Dtos;
namespace OrderService.Interfaces
{
    public interface IPrecastService
    {
        public List<getPrecastDto> GetPrecastDetails();
        public int InsertPrecastDetails(getPrecastDto getPrecast);
        public int UpdatePrecastDetails(getPrecastDto getPrecast);

        public bool DeletePrecastDetailsById(int PrecastID);

        public List<string> GetDistinctBarShapeCodes();
        public int UpdateBarShapeCodeDetails(getBarShapeCodeDto getPrecast);

        public int UpdatePrecastFlag(int PostheaderID, int flag);
        public Task<bool?> GetIsPrecast(string customerCode, string projectCode);
    }
}
