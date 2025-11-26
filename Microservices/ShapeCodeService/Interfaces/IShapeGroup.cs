using ShapeCodeService.Dtos;
using ShapeCodeService.Models;

namespace ShapeCodeService.Interfaces
{
    public interface IShapeGroup
    {
     Task<List<Shapegroup>> GetShapeGroupListAsync();
     Task<bool> CheckduplicateShapeGroupAsync(string ShapeGroupName);
     Task<Shapegroup> AddShapeGroupAsync(Shapegroup shapegroup);
     Task<int> DeleteShapeGroupAsync(int id);

    }
}
