namespace OrderService.Dtos
{
    public class setCAB2Dto
    { 
            public setSubBarType BarType { get; set; }
            public float SumWeight { get; set; }
        
    }

    public class setSubBarType
    {
        public string BarType { get; set;}
        public int BarSize { get; set; }
    }
}
