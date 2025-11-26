namespace OrderService.Models
{
    public class OrderItemDet
    {
        public string order_no { get; set; }
        public string item_no { get; set; }
        public string shape_code { get; set; }
        public string TotalWeight { get; set; }
        public string NoofCuts { get; set; }
        public string NoofBends { get; set; }
        public string NoofPieces { get; set; }
        public string MRun { get; set; }
        public string Score { get; set; }
        public string SumofWeight { get; set; }
        public string DefaultRoute { get; set; }

        public string ProductType { get; set; }
        public string cab_dia { get; set; }
        public string cab_cut_length { get; set; }
        public string product_marking { get; set; }
        
        public string cab_bvbs { get; set; }
    }
}
