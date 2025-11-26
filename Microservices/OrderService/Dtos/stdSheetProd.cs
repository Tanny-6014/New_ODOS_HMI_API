using OrderService.Models;

namespace OrderService.Dtos
{
    public class stdSheetProd
    {

        public string CustomerCode { get; set; }

        public string ProjectCode { get; set; }

        public int JobID { get; set; }

        public List<StdSheetDetailsModels> StdProdDetails { get; set; }




    }
}

