namespace OrderService.Dtos
{
    public class UpdateSlabproductOrderDto
    {
        public NDSSlab.SlabProduct slabprod { get; set; }

        public NDSSlab.SlabStructure structureMark { get; set; }

        public NDSSlab.ShapeCodeParameterSet ParameterSet { get; set; }
    }
}
