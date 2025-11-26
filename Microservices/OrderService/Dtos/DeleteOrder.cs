namespace OrderService.Dtos
{
    public class DeleteOrder
    {
        public List<string> pCustomerCode { get; set; }

        public List<string> pProjectCode { get; set; }

        public List<int> pOrderNo { get; set; }

        public string pOrderStatus { get; set; }

        public string UserName { get; set; }
    }
}
