using DetailingService.Repositories;

namespace DetailingService.Dtos
{
    public class BeamStructureMarkListDto
    {
        public int INTPARENTSTRUCTUREMARKID { get; set; }
        public int INTSTRUCTUREMARKID { get; set; }
        public int TNTSTRUCTUREREVNO { get; set; }
        public int INTGROUPMARKID { get; set; }
        public int TNTGROUPREVNO { get; set; }
        public int INTCONSIGNMENTTYPE { get; set; }
        public string VCHSTRUCTUREMARKINGNAME { get; set; }
        public int TNTPARAMSETNUMBER { get; set; }
        public int INTSHAPETRANSHEADERID { get; set; }
        public int INTMEMBERQTY { get; set; }
        public int BITSIMILARSTRUCTURE { get; set; }
        public string VCHSIMILARTO { get; set; }
        public int NUMBEAMWIDTH { get; set; }
        public int NUMBEAMDEPTH { get; set; }
        
        public int NUMBEAMSLOPE { get; set; }
        public int NUMCAGEWIDTH { get; set; }
        public int NUMCAGEDEPTH { get; set; }
        public int NUMCAGESLOPE { get; set; }
        public int INTCLEARSPAN { get; set; }
        public int INTTOTALSTIRRUPS { get; set; }
        public int INTBEAMPRODUCTCODEID { get; set; }
        public int INTBEAMSHAPEID { get; set; }
        public int BITISCAPPING { get; set; }
        public int INTCAPPINGPRODUCTCODEID { get; set; }
        public int INTCAPPINGSHAPECODEID { get; set; }
        public int NUMAREA { get; set; }
        public int INTTOTALQTY { get; set; }
        public int INTTOTALBEND { get; set; }
        public int NUMTHEORETICTONNAGE { get; set; }
        public int NUMNETTONNAGE { get; set; }
        public int BITBENDINGCHECK { get; set; }
        public string BENDCHECK { get; set; }
        public int BITMACHINECHECK { get; set; }
        public string MACHINECHECK { get; set; }
        public int BITTRANSPORTCHECK { get; set; }
        public string TRANSPORTCHECK { get; set; }
        public int BITCOAT { get; set; }
        public string COATCHECK { get; set; }
        public string VCHDRAWINGREFERENCE { get; set; }
        public string SIMILARBEAM { get; set; }
        public string ISCAP { get; set; }
        public string SIMILARSTRUCTURE { get; set; }
        public string BEAMPRODUCT { get; set; }
        public string BEAMSHAPE { get; set; }
        public string CAPPRODUCT { get; set; }
        public string CAPSHAPE { get; set; }
        public string CHRDRAWINGVERSION { get; set; }
        public string VCHDRAWINGREMARKS { get; set; }
        public string PARENTSTRUCTUREMARKNAME { get; set; }
        public int TNTSTATUSID { get; set; }
        public int INTSAPMATERIALCODEID { get; set; }
        public int INTPARAMETERSET { get; set; }
        public string VCHTACTONCONFIGURATIONSTATE { get; set; }      
        public int PINSIZE { get; set; }
        public string PRODUCEINDICATOR { get; set; }

 }
}
