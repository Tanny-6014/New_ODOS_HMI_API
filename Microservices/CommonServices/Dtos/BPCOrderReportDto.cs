namespace CommonServices.Dtos
{
    public class BPCOrderReportDto
    {
        public string OrderNumber { get; set; } 
        public int  JobID { get; set; }  
        public string Customer_Name { get; set; }
        public string CustomerCode { get; set; }
        public string Project_Name { get; set; }
        public string ProjectCode { get; set; }
        public int cage_qty { get; set; }
        public decimal cage_weight{ get; set; }
        public string pile_type { get; set; }
        public int pile_dia{ get; set; }
        public int cage_dia { get; set; }
        public string main_bar_type { get; set; }  
        public string MB_Count { get; set; } 
        public string main_bar_shape { get; set; }
        public string main_bar_grade { get; set; }
         public string main_bar_dia { get; set; }
         public string cage_length { get; set; }
        public string spiral_link_type { get; set; }
          public string spiral_link_grade { get; set; }
          public string spiral_link_dia { get; set; }
          public string spiral_link_spacing { get; set; }
          public int  lap_length { get; set; }
          public int  end_length { get; set; }
          public string cage_location { get; set; }
         public string UpdateBy { get; set; }
         public string   UpdateDate { get; set; }
          public decimal TotalWeight { get; set; } 
           public string OrderStatus { get; set; }
    }
}
