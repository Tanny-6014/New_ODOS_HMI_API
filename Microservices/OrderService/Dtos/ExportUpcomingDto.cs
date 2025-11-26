namespace OrderService.Dtos
{
    public class ExportUpcomingDto
    {
      
        public List<string> pColumnsID { get; set; }
        public List<string> pColumnName { get; set; }
        public List<double> pColumnSize { get; set; }
        public string UserName { get; set; }

        public string CustomerCode { get; set; }
        public List<string> ProjectCode { get; set; }

    }
}
