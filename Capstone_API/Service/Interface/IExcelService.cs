using Capstone_API.Results;
using Microsoft.AspNetCore.Mvc;

namespace Capstone_API.Service.Interface
{
    public interface IExcelService
    {
        ResponseResult TaskImportExcel();
        Task<ResponseResult> ImportTimetable(IFormFile file, CancellationToken cancellationToken);
        FileStreamResult ExportInImportFormat(IHttpContextAccessor _httpContextAccessor);

    }
}
