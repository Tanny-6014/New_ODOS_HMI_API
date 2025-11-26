namespace OrderService.Dtos
{
    public class getBarShapeCodeDto
    {
        public string CustomerCode{ get; set; }
        public string ProjectCode{ get; set; } 
        public int JobID{ get; set; } 
        public int BarID{ get; set; } 
        public int CageID{ get; set; }
        public string BarMark { get; set; }
        public string BarType { get; set; }
        public int BarSize { get; set; }
        public int BarTotalQty { get; set; }
        public string BarShapeCode { get; set; }
        public int A { get; set; }
        public int B { get; set; }
        public int C { get; set; }
        public int D { get; set; }
        public int E { get; set; }
        public int F { get; set; }
        public int G { get; set; }
        public int BarLength { get; set; }
        public decimal BarWeight { get; set; }
        public string Remarks { get; set; }
    }
}       