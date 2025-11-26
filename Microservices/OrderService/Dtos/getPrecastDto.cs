namespace OrderService.Dtos
{
    public class getPrecastDto
    {
        public int Precast_ID { get; set; }
        public string CustomerCode { get; set; }
        public string ProjectCode { get; set; }
        public string ComponentMarking { get; set; }
        public string Block { get; set; }

        public string Level { get; set; }
        public int Qty { get; set; }
        public string Remark { get; set; }
        public int PageNo { get; set; }
        public string StructureElement { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }



    }
}
 												