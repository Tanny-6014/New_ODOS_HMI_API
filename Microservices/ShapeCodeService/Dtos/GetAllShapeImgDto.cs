namespace ShapeCodeService.Dtos
{
    public class GetAllShapeImgDto
    {
        public string CSM_SHAPE_ID { get; set; } 
        public int CSM_ACT_INACTIVE { get; set; }   
        public byte[] CSM_SHAPE_IMAGE { get; set; }
    }
}
