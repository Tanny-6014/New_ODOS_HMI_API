namespace ShapeCodeService.Dtos
{
    public class getFormulasDto
    {
        public int LibraryId { get; set; }
        public int Id { get; set; }
        public string FormulaName { get; set; }
        public string Formula { get; set; }
        public int Fk_shapeId { get; set; }

           
    }
}
