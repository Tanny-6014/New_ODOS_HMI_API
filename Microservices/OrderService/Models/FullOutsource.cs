namespace OrderService.Models
{
    public class FullOutsource
    {

        public string order_no { get; set; }
        public string order_request_no { get; set; }
        public string cust_code { get; set; }
        public string Cust_Name { get; set; }
        public string Contract_No { get; set; }
        public string Proj_No { get; set; }
        public string Proj_Name { get; set; }
        public string Cust_Po_No { get; set; }
        public int Order_Pieces { get; set; }
        public string cust_order_date { get; set; }

    }
}
