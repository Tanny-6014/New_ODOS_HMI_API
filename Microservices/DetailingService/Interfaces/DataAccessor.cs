using Tacton.Configurator.Interfaces;

namespace DetailingService.Interfaces
{
    public interface DataAccessor
    {
        AttributeSet GetData(string datakey);
        ModelFile GetModel(string modelkey);
    }
}
