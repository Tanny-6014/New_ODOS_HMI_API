namespace DrainService.Dtos
{
    public class GetBPMainBarPatternDetailDto
    {
        public int intMainBarPatternID { get; set; }
        public string vchMainBarPatternCode { get; set; }
        public string vchDescription { get; set; }
        public int tntNoBarSize { get; set; }
        public int tntNoBarLength { get; set; }
        public string vchMainBarPatternImage { get; set; }  
        public string vchMainBarPatternImagePath { get; set; }
    }
}
