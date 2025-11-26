namespace WBSService.Dtos
{
    public class GetCapProductCodeDto
    {
        public int? INTPRODUCTCODEID { get; set; }
        public string? VCHPRODUCTCODE { get; set; }
        public string? VCHPRODUCTDESCRIPTION { get; set; }
        public string CHRSTATUSCODE { get; set; }
        public int TNTSTATUSID { get; set; }
        public string? VCHPRODUCTTYPE { get; set; }
        
    }
}
