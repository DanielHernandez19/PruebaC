using PruebaC.DTOs.Request;
using PruebaC.DTOs.Response;
using System.Threading.Tasks;

namespace PruebaC.Interfaces
{
    public interface IEmpleadoService
    {
        Task<int> Add(EmpleadoDto model);
        Task<int> Update(EmpleadoDto model);
        Task<List<EmpleadoResponseDto>> GetById(int id);
        Task<List<EmpleadoResponseDto>> GetAll();
        Task<int> Delete(int id);
    }
}
