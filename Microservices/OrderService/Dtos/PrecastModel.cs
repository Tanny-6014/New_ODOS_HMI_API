using OrderService.Models;

namespace OrderService.Dtos

{

    public class PrecastModel

    {

        public string CustomerCode { get; set; }

        public string ProjectCode { get; set; }

        public int JobID { get; set; }

        public PreCastDetailsModel StdPrecastDetail { get; set; }

    }

}



