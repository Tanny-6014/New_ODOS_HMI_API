namespace ShapeCodeService.Dtos
{
    public class ShapeSurchageDto
    {
        public int? ID { get; set; }
        public int? ShapeCode_Id { get; set; }
        public string? ShapeCode { get; set; }
        public decimal? Bar_Dia { get; set; }
        public decimal? Invoice_Length { get; set; }
        public string? Surcharge { get; set; }
        public int? Surchage_Code { get; set; }
        public int? Condition_Id { get; set; }
        public string? Dia_Condition { get; set; }
        public string? User_Id { get; set; }
        public string Updated_Date { get; set; }
    }
}
