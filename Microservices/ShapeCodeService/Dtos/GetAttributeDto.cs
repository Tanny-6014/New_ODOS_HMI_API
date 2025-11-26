namespace ShapeCodeService.Dtos
{
    public class GetAttributeDto
    {
        public int Id { get; set; }
        public int Fk_ShapeId { get; set; }
        public string Attribute{get;set;}
        public string Type { get;set;}
        public string ValidationConstraint { get;set;}


    }
}
	