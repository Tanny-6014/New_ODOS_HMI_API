namespace DetailingService.Dtos
{
    public class BeamProductMarkingFormulaForShapeDto
    {

        public int Id { get; set; }
        public int Fk_libraryId { get; set; }
        public string FormulaName { get; set; }
        public string Formula { get; set; }

        public int NoOfCW { get; set; }

     
    }
}
