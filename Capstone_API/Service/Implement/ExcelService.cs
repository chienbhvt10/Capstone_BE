using Capstone_API.DTO.Excel;
using Capstone_API.Results;
using Capstone_API.Service.Interface;
using OfficeOpenXml;

namespace Capstone_API.Service.Implement
{
    public class ExcelService : IExcelService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ExcelService(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public GenericResult<string> ExportInImportFormat(IHttpContextAccessor _httpContextAccessor, IEnumerable<ExportInImportFormatDTO> exportItems)
        {
            try
            {
                string folder = _hostingEnvironment.WebRootPath;
                string excelName = $"Timetable-{DateTime.Now:yyyyMMddHHmmssfff}.xlsx";
                string downloadUrl = string.Format("{0}://{1}/{2}", _httpContextAccessor.HttpContext?.Request.Scheme, _httpContextAccessor.HttpContext?.Request.Host, excelName);
                FileInfo file = new(Path.Combine(folder, excelName));
                if (file.Exists)
                {
                    file.Delete();
                    file = new FileInfo(Path.Combine(folder, excelName));
                }

                using (var package = new ExcelPackage(file))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(exportItems, true);
                    package.Save();
                }
                return new GenericResult<string>(downloadUrl, true);
            }
            catch (Exception ex)
            {
                return new GenericResult<string>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }



        public async Task<GenericResult<IEnumerable<TaskAssignImportDTO>>> ImportTimetable(IFormFile file, CancellationToken cancellationToken)
        {
            if (file == null || file.Length <= 0)
            {
                return new GenericResult<IEnumerable<TaskAssignImportDTO>>("formfile is empty");
            }

            if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return new GenericResult<IEnumerable<TaskAssignImportDTO>>("Not Support file extension");
            }

            var list = new List<TaskAssignImportDTO>();

            try
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream, cancellationToken);

                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++)
                        {
                            list.Add(new TaskAssignImportDTO
                            {
                                ClassName = worksheet.Cells[row, 1].Value?.ToString()?.Trim() ?? string.Empty,
                                SubjectName = worksheet.Cells[row, 2].Value?.ToString()?.Trim() ?? string.Empty,
                                Department = worksheet.Cells[row, 3].Value?.ToString()?.Trim() ?? string.Empty,
                                TimeSlot = worksheet.Cells[row, 4].Value?.ToString()?.Trim() ?? string.Empty,
                                Slot1 = worksheet.Cells[row, 5].Value?.ToString()?.Trim() ?? string.Empty,
                                Slot2 = worksheet.Cells[row, 6].Value?.ToString()?.Trim() ?? string.Empty,
                                Room1 = worksheet.Cells[row, 7].Value?.ToString()?.Trim() ?? string.Empty,
                                Room2 = worksheet.Cells[row, 8].Value?.ToString()?.Trim() ?? string.Empty,
                                Status = worksheet.Cells[row, 9].Value?.ToString()?.Trim() ?? string.Empty,
                            });
                        }
                    }
                }

                return new GenericResult<IEnumerable<TaskAssignImportDTO>>(list, true);
            }
            catch (Exception ex)
            {
                return new GenericResult<IEnumerable<TaskAssignImportDTO>>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        public ResponseResult TaskImportExcel()
        {
            try
            {
                return new ResponseResult();
            }
            catch (Exception ex)
            {
                return new ResponseResult($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
    }
}
