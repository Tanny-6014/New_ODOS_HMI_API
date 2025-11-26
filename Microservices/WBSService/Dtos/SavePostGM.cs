using WBSService.Model;

namespace WBSService.Dtos
{
    public class SavePostGM
    {
        public PostGroupMark postGroupMarkList { get; set; }
        public int wbsElementId { get; set; }
        public int structureElementTypeId { get; set; }
        public int productTypeId { get; set; }
        public string wbsBBSNo { get; set; }
        public string wbsBBSDesc { get; set; }
        public int userId { get; set; }
        public int postHeaderId { get; set; }
        public int projectId { get; set; }
    }
}
