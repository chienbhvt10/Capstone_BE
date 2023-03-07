using Capstone_API.DTO.Subject.Response;
using Capstone_API.Results;

namespace Capstone_API.Service.Interface
{
    public interface ISubjectService
    {
        GenericResult<IEnumerable<SubjectResponse>> GetAll();
        GenericResult<SubjectResponse> GetOneSubject(int Id);
        ResponseResult CreateSubject(SubjectResponse request);
        ResponseResult UpdateSubject(SubjectResponse request);
        ResponseResult DeleteSubject(int id);
    }
}
