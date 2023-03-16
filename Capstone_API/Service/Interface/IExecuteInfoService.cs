using Capstone_API.DTO.Task.Fetch;
using Capstone_API.Results;

namespace Capstone_API.Service.Interface
{
    public interface IExecuteInfoService
    {
        GenericResult<List<ExecuteInfoResponse>> GetAll();
        ResponseResult CreateExecuteInfo(ExecuteInfoResponse request);
    }
}
