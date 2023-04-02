using Capstone_API.DTO.Subject.Request;
using Capstone_API.DTO.Subject.Response;
using Capstone_API.Results;

namespace Capstone_API.Service.Interface
{
    public interface ISubjectService
    {
        GenericResult<IEnumerable<SubjectResponse>> GetAll();
        GenericResult<SubjectResponse> GetOneSubject(int Id);
        GenericResult<SubjectResponse> CreateSubject(SubjectRequest request);
        ResponseResult UpdateSubject(SubjectResponse request);
        ResponseResult DeleteSubject(int id);
    }
}
