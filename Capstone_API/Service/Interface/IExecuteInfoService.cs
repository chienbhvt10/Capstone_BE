using Capstone_API.DTO.Task.Fetch;
using Capstone_API.Results;

namespace Capstone_API.Service.Interface
{
    public interface IExecuteInfoService
    {
        GenericResult<List<ExecuteInfoResponse>> GetAll(int semesterId);
        ResponseResult CreateExecuteInfo(ExecuteInfoResponse request);
        Task<GenericResult<ExecuteResponse>> Execute(SettingRequest request);
        Task<ResponseResult> GetSchedule(string executeId);

    }
}
