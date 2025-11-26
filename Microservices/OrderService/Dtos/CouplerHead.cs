using OrderService.Models;

namespace OrderService.Dtos
{
    public class CouplerHead
    {

        public string CustomerCode { get; set; }

        public string ProjectCode { get; set; }

        public int JobID { get; set; }

        public List<StdProdDetailsModels> StdProdDetails { get; set; }




    }
}
