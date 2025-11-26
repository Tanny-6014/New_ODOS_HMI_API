namespace OrderService.Dtos
{
    public class GetContractlistDto
    {
        public string CustomerCode{ get; set; } 
        public string ProjectCode{ get; set; } 
        public string ProdType{ get; set; }
        public string ProdTypeL2{ get; set; } 
        public int? OrderNumber{ get; set; }
        public string StructureElement { get; set; }
    }
}
