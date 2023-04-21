using Capstone_API.DTO.CommonRequest;
using Capstone_API.DTO.Excel;
using Capstone_API.DTO.Task.Response;
using Capstone_API.Models;
using Capstone_API.Results;
using Capstone_API.Service.Interface;
using Capstone_API.UOW_Repositories.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

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

        public Statistic GetStatistic(GetAllRequest request)
        {
            var currentAssignedTask = _unitOfWork.TaskRepository
                .GetByCondition(item =>
                    item.SemesterId == request.SemesterId
                    && item.DepartmentHeadId == item.DepartmentHeadId
                    && item.LecturerId != null).ToList();

            var currentNotAssignedTask = _unitOfWork.TaskRepository
                .GetByCondition(item =>
                    item.SemesterId == request.SemesterId
                    && item.DepartmentHeadId == item.DepartmentHeadId
                    && item.LecturerId == null).ToList();

            var countGroupByTimeslot = currentAssignedTask
                .OrderBy(item => item.TimeSlotId)
                .GroupBy(item => item.TimeSlotId).Select(gr => gr.Count()).ToList();

            var allTask = _unitOfWork.TaskRepository
                .GetByCondition(item =>
                    item.SemesterId == request.SemesterId
                    && item.DepartmentHeadId == item.DepartmentHeadId).ToList();

            var countAllGroupByTimeslot = allTask
                .OrderBy(item => item.TimeSlotId)
                .GroupBy(item => item.TimeSlotId).Select(gr => gr.Count()).ToList();

            return new Statistic()
            {
                AssignedCount = currentAssignedTask.Count,
                NotAssignedCount = currentNotAssignedTask.Count,
                CountGroupByTimeSlot = countGroupByTimeslot,
                CountAllGroupByTimeSlot = countAllGroupByTimeslot,
                TotalTask = allTask.Count
            };
        }

        public FileStreamResult ExportGroupByLecturers(int userId, IHttpContextAccessor _httpContextAccessor)
        {
            try
            {
                var currentSemester = _unitOfWork.SemesterInfoRepository.GetByCondition(item => item.DepartmentHeadId == userId && item.IsNow == true).FirstOrDefault();
                var request = new GetAllRequest()
                {
                    DepartmentHeadId = userId,
                    SemesterId = currentSemester?.Id
                };

                var statistic = GetStatistic(request);

                var exportItems = _taskService.GetTaskResponseByLecturerKey(request).ToList();
                string excelName = $"{DateTime.Now:yyyy-MM-dd}-Group-by-lecturers-{DateTime.Now:HHmmss}.xlsx";
                var excelPackage = new ExcelPackage();
                var worksheet = excelPackage.Workbook.Worksheets.Add("Group by Lecturer");

                var timeslots = _unitOfWork.TimeSlotRepository
                    .GetByCondition(item =>
                        item.SemesterId == currentSemester?.Id
                        && item.DepartmentHeadId == userId)
                    .ToList();

                // Header Columns
                worksheet.Column(1).Width = 20;
                worksheet.Column(timeslots.Count + 2).Width = 20;
                worksheet.Cells[1, 1].Value = "";
                worksheet.Cells[1, 1].Style.Font.Color.SetColor(Color.White);
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.Font.Size = 11;
                worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(Color.White);

                for (int i = 2; i <= timeslots.Count + 1; i++)
                {
                    worksheet.Column(i).Width = 28;
                    worksheet.Cells[1, i].Value = timeslots[i - 2].Name;
                    worksheet.Cells[1, i].Style.Font.Color.SetColor(Color.White);
                    worksheet.Cells[1, i].Style.Font.Bold = true;
                    worksheet.Cells[1, i].Style.Font.Size = 11;
                    worksheet.Cells[1, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, i].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(60, 162, 255));
                    worksheet.Cells[1, i].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }
                worksheet.Cells[1, timeslots.Count + 2].Value = "Assigned Task";
                worksheet.Cells[1, timeslots.Count + 2].Style.Font.Color.SetColor(Color.Black);
                worksheet.Cells[1, timeslots.Count + 2].Style.Font.Bold = true;
                worksheet.Cells[1, timeslots.Count + 2].Style.Font.Size = 11;
                worksheet.Cells[1, timeslots.Count + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[1, timeslots.Count + 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, timeslots.Count + 2].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(247, 113, 147));
                worksheet.Cells[1, timeslots.Count + 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                // Data
                for (int i = 2; i <= exportItems.Count; i++)
                {
                    worksheet.Cells[i, 1].Value = exportItems[i - 2].LecturerName;
                    worksheet.Cells[i, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[i, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[i, 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(247, 113, 147));
                    worksheet.Cells[i, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[i, exportItems[0].TimeSlotInfos.Count() + 2].Value = exportItems?[i - 2].Total;
                    worksheet.Cells[i, exportItems[0].TimeSlotInfos.Count() + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[i, exportItems[0].TimeSlotInfos.Count() + 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[i, exportItems[0].TimeSlotInfos.Count() + 2].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(247, 113, 147));
                    worksheet.Cells[i, exportItems[0].TimeSlotInfos.Count() + 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    for (int j = 2; j <= exportItems[i - 2].TimeSlotInfos?.Count + 1; j++)
                    {
                        var timeSlotInfo = exportItems[i - 2].TimeSlotInfos?[j - 2];
                        if (timeSlotInfo?.TaskId > 0)
                        {
                            worksheet.Cells[i, j].Value = timeSlotInfo?.ClassName + "." + timeSlotInfo?.SubjectCode + "." + timeSlotInfo?.RoomName;
                            worksheet.Cells[i, j].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells[i, j].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[i, j].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(252, 255, 181));
                        }
                        worksheet.Cells[i, j].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    }
                }
                // Count Assigned row
                for (int i = 2; i <= timeslots.Count + 1; i++)
                {
                    worksheet.Cells[exportItems.Count + 1, i].Value = statistic.CountGroupByTimeSlot?[i - 2];
                    worksheet.Cells[exportItems.Count + 1, i].Style.Font.Color.SetColor(Color.Black);
                    worksheet.Cells[exportItems.Count + 1, i].Style.Font.Bold = true;
                    worksheet.Cells[exportItems.Count + 1, i].Style.Font.Size = 11;
                    worksheet.Cells[exportItems.Count + 1, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[exportItems.Count + 1, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[exportItems.Count + 1, i].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(247, 113, 147));
                    worksheet.Cells[exportItems.Count + 1, i].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }

                worksheet.Cells[exportItems.Count + 1, 1].Value = "Assigned task";
                worksheet.Cells[exportItems.Count + 1, 1].Style.Font.Color.SetColor(Color.Black);
                worksheet.Cells[exportItems.Count + 1, 1].Style.Font.Bold = true;
                worksheet.Cells[exportItems.Count + 1, 1].Style.Font.Size = 11;
                worksheet.Cells[exportItems.Count + 1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[exportItems.Count + 1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[exportItems.Count + 1, 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(247, 113, 147));
                worksheet.Cells[exportItems.Count + 1, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                worksheet.Cells[exportItems.Count + 1, timeslots.Count + 2].Value = statistic.AssignedCount;
                worksheet.Cells[exportItems.Count + 1, timeslots.Count + 2].Style.Font.Color.SetColor(Color.Black);
                worksheet.Cells[exportItems.Count + 1, timeslots.Count + 2].Style.Font.Bold = true;
                worksheet.Cells[exportItems.Count + 1, timeslots.Count + 2].Style.Font.Size = 11;
                worksheet.Cells[exportItems.Count + 1, timeslots.Count + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[exportItems.Count + 1, timeslots.Count + 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[exportItems.Count + 1, timeslots.Count + 2].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(247, 113, 147));
                worksheet.Cells[exportItems.Count + 1, timeslots.Count + 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                // Total row
                for (int i = 2; i <= timeslots.Count + 1; i++)
                {
                    worksheet.Cells[exportItems.Count + 2, i].Value = statistic.CountAllGroupByTimeSlot?[i - 2];
                    worksheet.Cells[exportItems.Count + 2, i].Style.Font.Color.SetColor(Color.Black);
                    worksheet.Cells[exportItems.Count + 2, i].Style.Font.Bold = true;
                    worksheet.Cells[exportItems.Count + 2, i].Style.Font.Size = 11;
                    worksheet.Cells[exportItems.Count + 2, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[exportItems.Count + 2, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[exportItems.Count + 2, i].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(60, 162, 255));
                    worksheet.Cells[exportItems.Count + 2, i].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }

                worksheet.Cells[exportItems.Count + 2, 1].Value = "Total Task";
                worksheet.Cells[exportItems.Count + 2, 1].Style.Font.Color.SetColor(Color.Black);
                worksheet.Cells[exportItems.Count + 2, 1].Style.Font.Bold = true;
                worksheet.Cells[exportItems.Count + 2, 1].Style.Font.Size = 11;
                worksheet.Cells[exportItems.Count + 2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[exportItems.Count + 2, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[exportItems.Count + 2, 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(60, 162, 255));
                worksheet.Cells[exportItems.Count + 2, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                worksheet.Cells[exportItems.Count + 2, timeslots.Count + 2].Value = statistic.TotalTask;
                worksheet.Cells[exportItems.Count + 2, timeslots.Count + 2].Style.Font.Color.SetColor(Color.Black);
                worksheet.Cells[exportItems.Count + 2, timeslots.Count + 2].Style.Font.Bold = true;
                worksheet.Cells[exportItems.Count + 2, timeslots.Count + 2].Style.Font.Size = 11;
                worksheet.Cells[exportItems.Count + 2, timeslots.Count + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[exportItems.Count + 2, timeslots.Count + 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[exportItems.Count + 2, timeslots.Count + 2].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(60, 162, 255));
                worksheet.Cells[exportItems.Count + 2, timeslots.Count + 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                // Not assigned count
                worksheet.Cells[exportItems.Count + 3, timeslots.Count + 1].Value = "Not Assigned Task";
                worksheet.Cells[exportItems.Count + 3, timeslots.Count + 1].Style.Font.Color.SetColor(Color.Black);
                worksheet.Cells[exportItems.Count + 3, timeslots.Count + 1].Style.Font.Bold = true;
                worksheet.Cells[exportItems.Count + 3, timeslots.Count + 1].Style.Font.Size = 11;
                worksheet.Cells[exportItems.Count + 3, timeslots.Count + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[exportItems.Count + 3, timeslots.Count + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[exportItems.Count + 3, timeslots.Count + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(60, 162, 255));
                worksheet.Cells[exportItems.Count + 3, timeslots.Count + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                worksheet.Cells[exportItems.Count + 3, timeslots.Count + 2].Value = statistic.NotAssignedCount;
                worksheet.Cells[exportItems.Count + 3, timeslots.Count + 2].Style.Font.Color.SetColor(Color.Black);
                worksheet.Cells[exportItems.Count + 3, timeslots.Count + 2].Style.Font.Bold = true;
                worksheet.Cells[exportItems.Count + 3, timeslots.Count + 2].Style.Font.Size = 11;
                worksheet.Cells[exportItems.Count + 3, timeslots.Count + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[exportItems.Count + 3, timeslots.Count + 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[exportItems.Count + 3, timeslots.Count + 2].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(252, 255, 181));
                worksheet.Cells[exportItems.Count + 3, timeslots.Count + 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);

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

        public FileStreamResult ExportInImportFormat(int userId, IHttpContextAccessor _httpContextAccessor)
        {
            try
            {
                var currentSemester = _unitOfWork.SemesterInfoRepository.GetByCondition(item => item.DepartmentHeadId == userId && item.IsNow == true).FirstOrDefault();
                var currentUser = _unitOfWork.UserRepository.GetById(userId);
                var department = _unitOfWork.DepartmentRepository.GetByCondition(
                            item => item.Id == currentUser?.DepartmentId).FirstOrDefault()?.Department1 ?? "";

                var exportItems = _unitOfWork.TaskRepository.MappingTaskData()
                    .Where(item => item.DepartmentHeadId == userId && item.SemesterId == currentSemester?.Id).ToList();
                List<ExportInImportFormatDTO> listExport = new();
                foreach (var item in exportItems)
                {
                    var Segments = _unitOfWork.TimeSlotSegmentRepository
                        .GetByCondition(tss => tss.SlotId == item.TimeSlot?.Id && tss.Segment > 0)
                        .OrderBy(item => item.Segment);
                    var DaySlot1Id = Segments.Take(1).ToList().FirstOrDefault()?.DayOfWeek ?? 0;
                    var DaySlot1Name = _unitOfWork.DayOfWeeksRepository.GetById(DaySlot1Id)?.Name ?? "";
                    var DaySlot2Id = Segments.Skip(1).Take(1).ToList().FirstOrDefault()?.DayOfWeek ?? 0;
                    var DaySlot2Name = _unitOfWork.DayOfWeeksRepository.GetById(DaySlot2Id)?.Name ?? "";

                    listExport.Add(new ExportInImportFormatDTO()
                    {
                        Dept = department,
                        Class = item.Class?.Name,
                        Subject = item.Subject?.Code,
                        TimeSlot = item.TimeSlot?.Name,
                        Slot1 = DaySlot1Name,
                        Slot2 = DaySlot2Name,
                        Room = item.Room1?.Name,
                        Status = item.Lecturer?.ShortName != null ? "" : "NOT_ASSIGNED",
                        Lecturer = item.Lecturer?.ShortName,

                    });
                }
                string excelName = $"{DateTime.Now:yyyy-MM-dd}-Import-format-{DateTime.Now:HHmmss}.xlsx";
                var excelPackage = new ExcelPackage();
                var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells.LoadFromCollection(listExport, true);

                for (int i = 1; i <= new ExportInImportFormatDTO().GetType().GetProperties().Length; i++)
                {
                    worksheet.Column(i).Width = 15;
                    worksheet.Cells[1, i].Style.Font.Color.SetColor(Color.White);
                    worksheet.Cells[1, i].Style.Font.Bold = true;
                    worksheet.Cells[1, i].Style.Font.Size = 11;
                    var customColor = Color.FromArgb(60, 162, 255);
                    worksheet.Cells[1, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, i].Style.Fill.BackgroundColor.SetColor(customColor);
                    worksheet.Cells[1, i].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[1, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

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

        public async Task<ResponseResult> ImportTimetable(GetAllRequest request, IFormFile file, CancellationToken cancellationToken)
        {
            if (file == null || file.Length <= 0)
            {
                return new ResponseResult("Formfile is empty");
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

                var semesterId = _unitOfWork.SemesterInfoRepository.GetAll().FirstOrDefault(item => item.IsNow == true && item.DepartmentHeadId == request.DepartmentHeadId)?.Id;

                if (semesterId == null)
                {
                    return new ResponseResult("Please insert current semester first");
                }
                var timeSlots = _unitOfWork.TimeSlotRepository.GetAll().Where(item => item.DepartmentHeadId == request.DepartmentHeadId && item.SemesterId == semesterId).ToList();
                if (timeSlots.Count == 0)
                {
                    return new ResponseResult("Please insert timeslot data first");
                }
                var subjects = _unitOfWork.SubjectRepository.GetAll().Where(item => item.DepartmentHeadId == request.DepartmentHeadId && item.SemesterId == semesterId).ToList();
                if (subjects.Count == 0)
                {
                    return new ResponseResult("Please insert subject data first");
                }
                _unitOfWork.TaskRepository.DeleteByCondition(item => item.SemesterId == semesterId && item.DepartmentHeadId == request.DepartmentHeadId, true);
                _unitOfWork.ExecuteInfoRepository.DeleteByCondition(item => item.SemesterId == semesterId && item.DepartmentHeadId == request.DepartmentHeadId, true);
                _unitOfWork.ClassRepository.DeleteByCondition(item => item.SemesterId == semesterId && item.DepartmentHeadId == request.DepartmentHeadId, true);
                _unitOfWork.RoomRepository.DeleteByCondition(item => item.SemesterId == semesterId && item.DepartmentHeadId == request.DepartmentHeadId, true);
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
                            var subjectFind = subjects.FirstOrDefault(sub => sub.Code.Trim().Equals(subjectCode));
                            var timeSlotFind = timeSlots.FirstOrDefault(ts => ts.Name.Trim().Equals(timeSlotName));


                            if (classes.Count() == 0)
                            {
                                clasTemp.Name = className;
                                clasTemp.SemesterId = semesterId;
                                clasTemp.DepartmentHeadId = request.DepartmentHeadId;
                                classes.Add(clasTemp);
                                _unitOfWork.ClassRepository.Add(clasTemp);
                            }
                            else if (classes.Count() > 0 && clasFind == null)
                            {
                                clasTemp.Name = className;
                                clasTemp.SemesterId = semesterId;
                                clasTemp.DepartmentHeadId = request.DepartmentHeadId;
                                classes.Add(clasTemp);
                                _unitOfWork.ClassRepository.Add(clasTemp);
                            }

                            if (rooms.Count() == 0)
                            {
                                roomTemp.Name = roomName;
                                roomTemp.SemesterId = semesterId;
                                roomTemp.DepartmentHeadId = request.DepartmentHeadId;
                                rooms.Add(roomTemp);
                                _unitOfWork.RoomRepository.Add(roomTemp);
                            }
                            else if (rooms.Count() > 0 && roomFind == null)
                            {
                                roomTemp.Name = roomName;
                                roomTemp.SemesterId = semesterId;
                                roomTemp.DepartmentHeadId = request.DepartmentHeadId;
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
                                SemesterId = semesterId,
                                DepartmentHeadId = request.DepartmentHeadId
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
