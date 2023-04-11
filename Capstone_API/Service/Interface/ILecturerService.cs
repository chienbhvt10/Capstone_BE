using Capstone_API.DTO.Lecturer.Request;
using Capstone_API.DTO.Lecturer.Response;
using Capstone_API.Results;

namespace Capstone_API.Service.Interface
{
    public interface ILecturerService
    {
        GenericResult<List<LecturerResponse>> GetAll(GetLecturerDTO request);
        GenericResult<LecturerResponse> GetOneLecturer(int Id);
        GenericResult<LecturerResponse> CreateLecturer(LecturerRequest request);
        ResponseResult UpdateLecturer(LecturerResponse request);
        ResponseResult DeleteLecturer(int id);

    }
}
