namespace OrderService.Dtos
{
    public class ExportIncomingDto
    {
        public string OrderStatus { get; set; }
        public List<string> pColumnsID { get; set; }
        public List<string> pColumnName { get; set; }
        public List<double> pColumnSize { get; set; }
        public bool Forecast { get; set; }
        public string UserName { get; set; }

    }
}
