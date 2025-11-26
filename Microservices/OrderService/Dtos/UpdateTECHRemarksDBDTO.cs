namespace OrderService.Dtos
{
    public class UpdateTECHRemarksDBDTO
    {

        public List<int> OrderNumber { get; set; }
        public List<string> StructureElement { get; set; }
        public List<string> ProductType { get; set; }
        public List<string> ScheduledProd { get; set; }
        public List<string> SORNo { get; set; }
        public string TECHRemarks { get; set; }
    }
}
