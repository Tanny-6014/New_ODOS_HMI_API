namespace DetailingService.Dtos
{
    public class CABShapeParameterDto
    {
        public int ? INTSHAPEID { get; set; }
        public string CHRPARAMNAME { get; set; }
        public string CHRCRITICALIND { get; set; }
        public int INTPARAMSEQ { get; set; }
        public string CHRSHAPECODE { get; set; }
        public bool BITEDIT { get; set; }
        public bool BITVISIBLE { get; set; }
        public string CHRANGLETYPE { get; set; }
        public string CHRWIRETYPE { get; set; }
        public int INTANGLEDIR { get; set; }
        public int PARAMETERVALUE { get; set; }
        public string SYMMETRYINDEX { get; set; }
        public string HEIGHTANGLEFORMULA { get; set; }
        public string HEIGHTSUCEEDANGLEFORMULA { get; set; }
        public int INTXCOORD { get; set; }
        public int INTYCOORD { get; set; }
        public int INTZCOORD { get; set; }
        public string VCHCUSTOMFORMULA { get; set; }
        public string VCHOFFSETANGLEFORMULA { get; set; }
        public string OFFSETSUCEEDANGLEFORMULA { get; set; }
        public string CSD_INPUT_TYPE { get; set; }
        public string CSD_VIEW { get; set; }
        public int RoundOffRange { get; set; }
    }
}
 						