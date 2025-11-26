namespace DetailingService.Dtos
{
    public class GetGroupMarkListDto
    {
        
         public int INTGROUPMARKID { get; set; }

        public int TNTGROUPREVNO { get; set; }
       
       
        public int INTSEDETAILINGID { get; set; }

        public int INTPROJECTID { get; set; }
        public int INTSTRUCTUREELEMENTTYPEID { get; set; }
        public int SITPRODUCTTYPEID { get; set; }
        public string VCHSTRUCTUREELEMENTTYPE { get; set; }
        public string VCHPRODUCTTYPE { get; set; }
        public string VCHGROUPMARKINGNAME { get; set; }
        public int INTPARAMETESET { get; set; }
        public int TNTPARAMSETNUMBER { get; set; }
        public string DATCREATEDDATE { get; set; }
        public string VCHSTATUS { get; set; }
        public string ISMODULAR { get; set; }
        public int POSTED { get; set; }
        public string SIDEFORCODE { get; set; }
        public int ISCABDE { get; set; }

//        INTGROUPMARKID TNTGROUPREVNO   INTPROJECTID INTSTRUCTUREELEMENTTYPEID   VCHSTRUCTUREELEMENTTYPE SITPRODUCTTYPEID    INTCONSIGNMENTTYPE VCHGROUPMARKINGNAME INTPARAMETESET TNTPARAMSETNUMBER   TNTSTATUSID DATCREATEDDATE  VCHPRODUCTTYPE VCHSTATUS   ISMODULAR POSTED  SIDEFORCODE ISCABDE
//11071	0	7710	1	Beam	10	1	TEST-B-5	0	0	1	04/01/2010	PRC Active  NULL	0	0	1
   
 
}
}
