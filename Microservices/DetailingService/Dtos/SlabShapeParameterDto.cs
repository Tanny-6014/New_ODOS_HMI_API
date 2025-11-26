namespace DetailingService.Dtos
{
    public class SlabShapeParameterDto
    {
        public int INTSHAPEID { get; set; }
        public string CHRPARAMNAME { get; set; }
       public string CHRCRITICALIND { get; set; }
        public int INTPARAMSEQ { get; set; }
        public string CHRSHAPECODE { get; set; }
        public int BITEDIT { get; set; }
        public string CHRANGLETYPE { get; set; }

        public int BITVISIBLE { get; set; }
        public string CHRWIRETYPE { get; set; }
        public string BITOHDTLS { get; set; }

        public string DEFAULTPARAMVALUE { get; set; }

        public int SITEVENMO1 { get; set; }
        public int SITEVENMO2 { get; set; }
        public int SITODDMO1 { get; set; }
        public int SITODDMO2 { get; set; }
        public int SITEVENCO1 { get; set; }
        public int SITEVENCO2 { get; set; }
        public int SITODDCO1 { get; set; }
        public int SITODDCO2 { get; set; }

        public int BITDEFAULTOHINDICATOR { get; set; }
        public int INTANGLEDIR { get; set; }

    }
}
