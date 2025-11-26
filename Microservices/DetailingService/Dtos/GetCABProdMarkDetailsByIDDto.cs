namespace DetailingService.Dtos
{
    public class GetCABProdMarkDetailsByIDDto
    {
        public int INTCABPRODUCTMARKID { get; set; }
        public string VCHCABPRODUCTMARKNAME{ get; set; }
            public int INTSEDETAILINGID{ get; set; }
            public int INTPRODUCTCODEID{ get; set; }    
            public int INTMEMBERQTY{ get; set; }
            public int INTSHAPECODE{ get; set; }    
            public int INTPINSIZEID{ get; set; }
            public int NUMINVOICELENGTH{ get; set; }    
            public int NUMPRODUCTIONLENGTH{ get; set; } 
            public int NUMINVOICEWEIGHT{ get; set; }    
            public int NUMPRODUCTIONWEIGHT{ get; set; }
            public int INTSTATUS{ get; set; }
            public string VCHDESCRIPT{ get; set; }
            public int INTDIAMETER{ get; set; }
            public string GRADE{ get; set; } //char
            public string PAGE_NUMBER{ get; set; } 
            public string SHAPECODE{ get; set; }
            public int INTSHAPEID{ get; set; }  
            public bool BITVARIESBAR{ get; set; } 
            public string status{ get; set; }
    }
}
