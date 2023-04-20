using Capstone_API.DTO.CommonRequest;
using Capstone_API.DTO.Semester.Request;
using Capstone_API.DTO.Semester.Response;
using Capstone_API.Results;

namespace Capstone_API.Service.Interface
{
    public interface ISemesterService
    {
        GenericResult<List<SemesterResponse>> GetAll(GetAllRequest request);
        GenericResult<SemesterResponse> GetOneSemester(int Id);
        GenericResult<SemesterResponse> CreateSemester(SemesterRequest request);
        ResponseResult UpdateSemester(SemesterResponse request);
        ResponseResult DeleteSemester(int id);
    }
}
