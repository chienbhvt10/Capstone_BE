using Capstone_API.DTO.Class.Response;
using Capstone_API.Results;

namespace Capstone_API.Service.Interface
{
    public interface IClassService
    {
        GenericResult<IEnumerable<ClassResponse>> GetAll();
    }
}
