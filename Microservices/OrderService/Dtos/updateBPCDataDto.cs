using OrderService.Models;

namespace OrderService.Dtos
{
    public class updateBPCDataDto
    {
      //public  List<BPCDetailsProcModels> pBPCLoadDatas { get; set; }
        public string CustomerCode { get; set; }
        public string ProjectCode { get; set; }
        public int JobID { get; set; }
        public int cage_id { get; set; }
        public int load_id { get; set; }
        public int load_qty { get; set; }
        public DateTime required_date { get; set; }    //yyyy-mm-dd
        public string int_remarks { get; set; }
        public string ext_remarks { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
