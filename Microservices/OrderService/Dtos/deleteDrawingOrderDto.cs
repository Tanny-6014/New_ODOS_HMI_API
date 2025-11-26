namespace OrderService.Dtos
{
    public class deleteDrawingOrderDto
    {
       public int OrderNuber { get; set; }
        public String StructureElement { get; set; }
        public string ProductType { get; set; }
        public string ScheduledProd { get; set; }
        public string CustomerCode { get; set; }
        public string ProjectCode { get; set; }
        public int DrawingID { get; set; }
        public int Revision { get; set; }
    }
}
