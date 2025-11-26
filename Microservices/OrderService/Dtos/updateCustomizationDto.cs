namespace OrderService.Dtos
{


    public class updateCustomizationDto
    {
        public string CustomerCode { get; set; }
        public string ProjectCode { get; set; }
        public bool Template { get; set; }
        public int JobId { get; set; }
        public int CageId { get; set; }

        public string main_bar_ct { get; set; }

        public string CustomizeBarsJSON { get; set; }
    }
}
