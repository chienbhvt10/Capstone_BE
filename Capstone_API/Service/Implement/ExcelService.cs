using Capstone_API.DTO.Excel;
using Capstone_API.Models;
using Capstone_API.Results;
using Capstone_API.Service.Interface;
using Capstone_API.UOW_Repositories.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace Capstone_API.Service.Implement
{
    public class ExcelService : IExcelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITaskService _taskService;
        public ExcelService(IUnitOfWork unitOfWork, ITaskService taskService)
        {
            _unitOfWork = unitOfWork;
            _taskService = taskService;
        }

        public FileStreamResult ExportGroupByLecturers(IHttpContextAccessor _httpContextAccessor)
        {
            try
            {
                var exportItems = _taskService.GetTaskResponseByLecturerKey();
                string excelName = $"Timetable-{DateTime.Now:yyyyMMddHHmmssfff}.xlsx";
                var excelPackage = new ExcelPackage();
                var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells.LoadFromCollection(exportItems, true);
                excelPackage.Save();
                var stream = new MemoryStream(excelPackage.GetAsByteArray());
                return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = excelName
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public FileStreamResult ExportInImportFormat(IHttpContextAccessor _httpContextAccessor)
        {
            try
            {
                var exportItems = _unitOfWork.TaskRepository.MappingTaskData().Select(item => new ExportInImportFormatDTO()
                {
                    Class = item.Class?.Name,
                    Subject = item.Subject?.Code,
                    Dept = item.Subject?.Department,
                    TimeSlot = item.TimeSlot?.Name,
                    Room = item.Room1?.Name,
                    Status = item.Lecturer?.ShortName != null ? "ASSIGNED" : "NOT_ASSIGNED",
                    Lecturer = item.Lecturer?.ShortName,

                });
                string excelName = $"Timetable-{DateTime.Now:yyyyMMddHHmmssfff}.xlsx";
                var excelPackage = new ExcelPackage();
                var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells.LoadFromCollection(exportItems, true);
                excelPackage.Save();
                var stream = new MemoryStream(excelPackage.GetAsByteArray());
                return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = excelName
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ResponseResult> ImportTimetable(IFormFile file, CancellationToken cancellationToken)
        {
            if (file == null || file.Length <= 0)
            {
                return new ResponseResult("formfile is empty");
            }

            if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return new ResponseResult("Not Support file extension");
            }
            try
            {
                //save executeId and time of semester by executeSemesterId
                // and get executeSemesterId in database
                // this code must be get executeSemesterId in database

                var semesterId = 0;
                _unitOfWork.TaskRepository.DeleteByCondition(item => item.SemesterId != semesterId, true);
                _unitOfWork.ExecuteInfoRepository.DeleteByCondition(item => item.SemesterId != semesterId, true);
                _unitOfWork.ClassRepository.DeleteByCondition(item => item.SemesterId != semesterId, true);
                _unitOfWork.RoomRepository.DeleteByCondition(item => item.SemesterId != semesterId, true);
                _unitOfWork.Complete();

                var classes = new List<Class>();
                var rooms = new List<Room>();
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream, cancellationToken);

                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++)
                        {
                            var className = worksheet.Cells[row, 1].Value?.ToString()?.Trim() ?? string.Empty;
                            var subjectCode = worksheet.Cells[row, 2].Value?.ToString()?.Trim() ?? string.Empty;
                            var subjectDepartment = worksheet.Cells[row, 3].Value?.ToString()?.Trim() ?? string.Empty;
                            var timeSlotName = worksheet.Cells[row, 4].Value?.ToString()?.Trim() ?? string.Empty;
                            var roomName = worksheet.Cells[row, 7].Value?.ToString()?.Trim() ?? string.Empty;

                            var clasTemp = new Class();
                            var roomTemp = new Room();

                            var shortNameBuilding = new string(roomName.Take(2).ToArray()).Trim();
                            var clasFind = classes.Find(clas => clas.Name.Equals(className));
                            var roomFind = rooms.Find(room => room.Name.Equals(roomName));
                            var subjectFind = _unitOfWork.SubjectRepository.GetAll().FirstOrDefault(sub => sub.Code.Trim().Equals(subjectCode) && sub.Department.Trim().Equals(subjectDepartment));
                            var timeSlotFind = _unitOfWork.TimeSlotRepository.GetAll().FirstOrDefault(ts => ts.Name.Trim().Equals(timeSlotName));


                            if (classes.Count() == 0)
                            {
                                clasTemp.Name = className;
                                classes.Add(clasTemp);
                                _unitOfWork.ClassRepository.Add(clasTemp);
                            }
                            else if (classes.Count() > 0 && clasFind == null)
                            {
                                clasTemp.Name = className;
                                classes.Add(clasTemp);
                                _unitOfWork.ClassRepository.Add(clasTemp);
                            }

                            if (rooms.Count() == 0)
                            {
                                roomTemp.Name = roomName;
                                rooms.Add(roomTemp);
                                _unitOfWork.RoomRepository.Add(roomTemp);
                            }
                            else if (rooms.Count() > 0 && roomFind == null)
                            {
                                roomTemp.Name = roomName;
                                rooms.Add(roomTemp);
                                _unitOfWork.RoomRepository.Add(roomTemp);
                            };

                            _unitOfWork.Complete();

                            var taskAssign = new TaskAssign()
                            {
                                ClassId = clasFind == null ? (clasTemp.Id == 0 ? 0 : clasTemp.Id) : (clasFind.Id == 0 ? 0 : clasFind.Id),
                                SubjectId = subjectFind?.Id == 0 ? 0 : subjectFind?.Id,
                                TimeSlotId = timeSlotFind?.Id == 0 ? 0 : timeSlotFind?.Id,
                                Room1Id = roomFind == null ? (roomTemp.Id == 0 ? 0 : roomTemp.Id) : (roomFind?.Id == 0 ? 0 : roomFind?.Id),
                            };

                            _unitOfWork.TaskRepository.Add(taskAssign);
                            _unitOfWork.Complete();
                        }
                    }
                }

                return new ResponseResult("Import excel and add new default data success", true);
            }
            catch (Exception ex)
            {
                return new ResponseResult($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }


    }
}
