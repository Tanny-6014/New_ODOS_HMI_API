namespace DrainService.Dtos
{
    public class SlabValidationFormulaDto
    {
        public int Id { get; set; }



        public int Fk_libraryId { get; set; }



        public string FormulaName { get; set; }



        public string Formula { get; set; }
        public string NoOfCW { get; set; }
    }
}
