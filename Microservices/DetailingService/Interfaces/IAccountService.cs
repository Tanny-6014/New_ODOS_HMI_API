using DetailingService.Repositories;
using DetailingService.Dtos;

namespace DetailingService.Interfaces
{
    public interface IAccountService
    {

        List<DetailingFormDto> GetDetailingForm(string Username);

    }
}
