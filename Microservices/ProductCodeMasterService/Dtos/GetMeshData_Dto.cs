namespace ProductCodeMasterService.Dtos
{
    public class GetMeshData_Dto
    {
        public string MainWireSpace { get; set; }
        public string CrossWireSpace { get; set; }
      
        public string MMType { get; set; }
        public string CMType { get; set; }
        public string M_DIA { get; set; }
        public string C_DIA { get; set; }
        public string DRP_M_ProdCodeName { get; set; }
        public string DRP_C_ProdCodeName { get; set; }
        public string DRP_M_ProdCodeID { get; set; }
        public string DRP_C_ProdCodeID { get; set; }
        public string MaterialDescription { get; set; }
        public string M_Grade { get; set; }
        public string C_Grade { get; set; }

        public decimal? M_WeightRun { get; set; }

        public decimal? C_WeightRun { get; set; }





    }
}
