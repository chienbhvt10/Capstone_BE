using Capstone_API.DTO.Excel;
using Capstone_API.Results;

namespace Capstone_API.Service.Interface
{
    public interface IExcelService
    {
        ResponseResult TaskImportExcel();
        Task<GenericResult<IEnumerable<TaskAssignImportDTO>>> ImportTimetable(IFormFile file, CancellationToken cancellationToken);
        GenericResult<string> ExportInImportFormat(IHttpContextAccessor _httpContextAccessor, IEnumerable<ExportInImportFormatDTO> exportItems);

    }
}
