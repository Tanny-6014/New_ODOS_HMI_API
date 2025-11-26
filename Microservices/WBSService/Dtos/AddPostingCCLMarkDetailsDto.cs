namespace WBSService.Dtos
{
    public class AddPostingCCLMarkDetailsDto
    {
        public int WBSElementID { get; set; }
        public int ProductCodeID { get; set; }
        public int Width { get; set; }
        public int Depth { get; set; }
        public int MWLength { get; set; }
        public int CWLength{ get; set; }
        public int Qty { get; set; }
        public int RevNo { get; set; }
        public string? AddFlag  { get; set; }
        public string? ShapeId{ get; set; }
        public int MO1 { get; set; }
        public int MO2 { get; set; }
        public int CO1 { get; set; }
        public int CO2 { get; set; }
        public int StructElementID { get; set; }
        public int ProductTypeL1Id { get; set; }
        public int UserId { get; set; }
        public int PostHeaderID { get; set; }

    }
}
