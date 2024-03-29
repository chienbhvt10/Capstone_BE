﻿using Capstone_API.DTO.CommonRequest;
using Capstone_API.Results;
using Microsoft.AspNetCore.Mvc;

namespace Capstone_API.Service.Interface
{
    public interface IExcelService
    {
        FileStreamResult ExportGroupByLecturers(int userId, IHttpContextAccessor _httpContextAccessor);
        Task<ResponseResult> ImportTimetable(GetAllRequest request, IFormFile file, CancellationToken cancellationToken);
        FileStreamResult ExportInImportFormat(int userId, IHttpContextAccessor _httpContextAccessor);
        Task<ResponseResult> ImportTimetableResult(GetAllRequest request, IFormFile file, CancellationToken cancellationToken);

    }
}
