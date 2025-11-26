
using ShapeCodeService.Dtos;
using ShapeCodeService.Models;

namespace ShapeCodeService.Interfaces
{
    public interface IShapeSurchage
    {
        Task<List<Shapesurchage>> GetShapeSurchageListAsync();
        Task<List<ShapeCodes>> GetShapeCodesAsync();
        Task<bool> CheckduplicateShapeGroupAsync(string ShapeSurchangeName);
        Task<List<Shapesurchage>> AddShapeSurchageAsync(List<Shapesurchage> shapesurchage);

        Task <Shapesurchage> UpdateShapeSurchageAsync(Shapesurchage shapesurchage);

        Task<int> DeleteShapeSurchageAsync(int id);
        Task<IEnumerable<SurchargeDropdown>> GetSurchargesAsync();

    }
}
