namespace DrainService.Dtos
{
    public class InsertAccessoriesCentralizerDto
    {
        public int intSELevelDetailsID { get; set; }
        public int intGroupMarkId { get; set; }    
        public int intStructureMarkId { get; set; }
        public string vchBarMark { get;set;}        
        public string vchSapMaterialCode { get; set; }         
        public int intQty { get; set; }
    }
}
