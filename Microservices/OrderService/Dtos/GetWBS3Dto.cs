namespace OrderService.Dtos
{
    public class GetWBS3Dto
    {
        public List<string> StructureEle { get; set; }
        public List<string> ProductType { get; set; }

        public string ProjectCode { get; set; }

        public string WBS1O { get; set; }
        public string WBS2O { get; set; }
        public string WBS3O { get; set; }
        public string WBS1 { get; set; }
        public string WBS2FR { get; set; }
        public string WBS2TO { get; set; }
        public string ScheduledProd { get; set; }
    }
}

