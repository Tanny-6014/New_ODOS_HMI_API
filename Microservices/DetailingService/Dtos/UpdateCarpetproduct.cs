using DetailingService.Repositories;

namespace DetailingService.Dtos
{
    public class UpdateCarpetproduct
    {
        public CarpetProduct carpetprod { get; set; }

        public CarpetStructure structureMark { get; set; }

        public ShapeCodeParameterSetDto ParameterSet { get; set; }
    }
}
