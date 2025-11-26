namespace OrderService.Dtos
{
    public class OrderAssignmentRequestDto
    {
        //public List<Ordersdata> OrderdataList { get; set; }
        public List<string> OrderRequestNo { get; set; }
        public string AssignedTo { get; set; }

    }

    //public class Ordersdata
    //{
    //    public string OrderRequestNo { get; set; }
    //    public int ExtraLength { get; set; }

    //}
}
