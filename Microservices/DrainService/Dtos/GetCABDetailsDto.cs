namespace DrainService.Dtos
{
    public class GetCABDetailsDto
    {
        public int intCABProductMarkID { get; set; }
        public string  BARMARK { get; set; }
        public int  intProductCodeId { get; set; }
        public int QUANTITY { get; set; }
        public int CODE { get; set; }
        public int PIN { get; set; }
        public int INVLENGTH { get; set; }
        public int PRODLENGTH { get; set; }
        public int PRODWEIGHT { get; set; }
        public string GRADE { get; set; }
        public string TYPEWORK { get; set; }
        public string BAR_STANDARD { get; set; }
        public int intShapeTransHeaderID { get; set; }
        public int element_id { get; set; }
        public int INVWEIGHT { get; set; }

        //ITEM STANDARD    ITEM1 STANDARD1   ITEM2 STANDARD2   Type1 Type2    Diameter    tntLayer chrGenerationStatus bitAssemblyIndicator bitMachineIndicator SHAPE_GROUP SHAPE_SURCHARGE SHAPE_TYPE Number_of_Ends  COMMENT STATUS  DESCRIPT nvchImagePath   vehicleType
    }
}
