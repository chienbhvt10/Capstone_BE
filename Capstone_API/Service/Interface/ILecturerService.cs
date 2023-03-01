using Capstone_API.DTO.Lecturer;
using Capstone_API.Results;
using UTA.T2.MusicLibrary.Service.Results;

namespace Capstone_API.Service.Interface
{
    public interface ILecturerService
    {
        GenericResult<IEnumerable<LecturerDTO>> GetAll();
        GenericResult<LecturerDTO> GetOneLecturer(int Id);

        ResponseResult CreateLecturer(LecturerDTO request);
        ResponseResult UpdateLecturer(LecturerDTO request);
        ResponseResult DeleteLecturer(int id);

    }
}
