using Capstone_API.Results;
using Microsoft.AspNetCore.Mvc;

namespace Capstone_API.Service.Interface
{
    public interface IExcelService
    {
        FileStreamResult ExportGroupByLecturers(IHttpContextAccessor _httpContextAccessor);
        Task<ResponseResult> ImportTimetable(IFormFile file, CancellationToken cancellationToken);
        FileStreamResult ExportInImportFormat(IHttpContextAccessor _httpContextAccessor);

    }
}
