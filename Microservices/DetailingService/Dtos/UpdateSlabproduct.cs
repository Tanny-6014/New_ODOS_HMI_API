using DetailingService.Repositories;

namespace DetailingService.Dtos
{
    public class UpdateSlabproduct
    {
        public SlabProduct slabprod { get; set; }

        public SlabStructure structureMark { get; set; }

        public ShapeCodeParameterSetDto ParameterSet { get; set; }
    }
}
