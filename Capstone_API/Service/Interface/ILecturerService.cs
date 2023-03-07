using Capstone_API.DTO.Lecturer.Response;
using Capstone_API.Results;

namespace Capstone_API.Service.Interface
{
    public interface ILecturerService
    {
        GenericResult<IEnumerable<LecturerResponse>> GetAll();
        GenericResult<LecturerResponse> GetOneLecturer(int Id);
        ResponseResult CreateLecturer(LecturerResponse request);
        ResponseResult UpdateLecturer(LecturerResponse request);
        ResponseResult DeleteLecturer(int id);

    }
}
