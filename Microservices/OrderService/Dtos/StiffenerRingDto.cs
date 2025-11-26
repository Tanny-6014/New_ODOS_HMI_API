namespace OrderService.Dtos
{
    public class StiffenerRingDto
    {
        public string CustomerCode { get; set; }
        public string ProjectCode { get; set; }
        public bool Template { get; set; }
        public int JobId { get; set; }
        public int CageId { get; set; }
        public int Sr1Location { get; set; }
        public int Sr2Location { get; set; }
        public int Sr3Location { get; set; }
        public int Sr4Location { get; set; }
        public int Sr5Location { get; set; }
        public int NoOfSr { get; set; }
        public int LminTop { get; set; }
        public int LminEnd { get; set; }
        public int rings_start { get; set; }
        public int no_of_sr { get; set; }
    }
}
