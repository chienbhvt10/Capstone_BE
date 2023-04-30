﻿using Capstone_API.DTO.CommonRequest;
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

        public void GetStatisticOfTaskBySubject() { }
        public Statistic GetStatisticOfTaskByLecturer(GetAllRequest request)
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

                var statistic = GetStatisticOfTaskByLecturer(request);

                var exportLecturerStatisticItems = _taskService.GetTaskResponseByLecturerKey(request).ToList();
                var exportSubjectsStatisticItems = _taskService.GetTaskResponseBySubjectIsKey(request).ToList();

                string excelName = $"{DateTime.Now:yyyy-MM-dd}-Group-by-lecturers-{DateTime.Now:HHmmss}.xlsx";
                using (var excelPackage = new ExcelPackage())
                {

                    var worksheet = excelPackage.Workbook.Worksheets.Add("Group by Lecturer");

                    var timeslots = _unitOfWork.TimeSlotRepository
                        .GetByCondition(item =>
                            item.SemesterId == currentSemester?.Id
                            && item.DepartmentHeadId == userId)
                        .ToList();

                    #region Statistic Lecturer + Slot

                    #region HeaderColumn
                    worksheet.Column(1).Width = 20;
                    worksheet.Column(timeslots.Count + 2).Width = 20;
                    worksheet.Cells[1, 1].Value = "Lecturer/Slot";
                    worksheet.Cells[1, 1].Style.Font.Color.SetColor(Color.Black);
                    worksheet.Cells[1, 1].Style.Font.Bold = true;
                    worksheet.Cells[1, 1].Style.Font.Size = 11;
                    worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(252, 255, 181));

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
                    #endregion

                    #region Data Row
                    for (int i = 2; i <= exportLecturerStatisticItems.Count; i++)
                    {
                        // lecturer column
                        worksheet.Cells[i, 1].Value = exportLecturerStatisticItems[i - 2].LecturerName;
                        worksheet.Cells[i, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[i, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[i, 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(247, 113, 147));
                        worksheet.Cells[i, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        // total column
                        worksheet.Cells[i, exportLecturerStatisticItems[0].TimeSlotInfos.Count() + 2].Value = exportLecturerStatisticItems?[i - 2].Total;
                        worksheet.Cells[i, exportLecturerStatisticItems[0].TimeSlotInfos.Count() + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[i, exportLecturerStatisticItems[0].TimeSlotInfos.Count() + 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[i, exportLecturerStatisticItems[0].TimeSlotInfos.Count() + 2].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(247, 113, 147));
                        worksheet.Cells[i, exportLecturerStatisticItems[0].TimeSlotInfos.Count() + 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        // other timeslot column
                        for (int j = 2; j <= exportLecturerStatisticItems[i - 2].TimeSlotInfos?.Count + 1; j++)
                        {
                            var timeSlotInfo = exportLecturerStatisticItems[i - 2].TimeSlotInfos?[j - 2];
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
                    #endregion

                    #region Count Assigned Row
                    for (int i = 2; i <= timeslots.Count + 1; i++)
                    {
                        worksheet.Cells[exportLecturerStatisticItems.Count + 1, i].Value = statistic.CountGroupByTimeSlot?[i - 2];
                        worksheet.Cells[exportLecturerStatisticItems.Count + 1, i].Style.Font.Color.SetColor(Color.Black);
                        worksheet.Cells[exportLecturerStatisticItems.Count + 1, i].Style.Font.Bold = true;
                        worksheet.Cells[exportLecturerStatisticItems.Count + 1, i].Style.Font.Size = 11;
                        worksheet.Cells[exportLecturerStatisticItems.Count + 1, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[exportLecturerStatisticItems.Count + 1, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[exportLecturerStatisticItems.Count + 1, i].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(247, 113, 147));
                        worksheet.Cells[exportLecturerStatisticItems.Count + 1, i].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    }

                    worksheet.Cells[exportLecturerStatisticItems.Count + 1, 1].Value = "Assigned task";
                    worksheet.Cells[exportLecturerStatisticItems.Count + 1, 1].Style.Font.Color.SetColor(Color.Black);
                    worksheet.Cells[exportLecturerStatisticItems.Count + 1, 1].Style.Font.Bold = true;
                    worksheet.Cells[exportLecturerStatisticItems.Count + 1, 1].Style.Font.Size = 11;
                    worksheet.Cells[exportLecturerStatisticItems.Count + 1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[exportLecturerStatisticItems.Count + 1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[exportLecturerStatisticItems.Count + 1, 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(247, 113, 147));
                    worksheet.Cells[exportLecturerStatisticItems.Count + 1, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    worksheet.Cells[exportLecturerStatisticItems.Count + 1, timeslots.Count + 2].Value = statistic.AssignedCount;
                    worksheet.Cells[exportLecturerStatisticItems.Count + 1, timeslots.Count + 2].Style.Font.Color.SetColor(Color.Black);
                    worksheet.Cells[exportLecturerStatisticItems.Count + 1, timeslots.Count + 2].Style.Font.Bold = true;
                    worksheet.Cells[exportLecturerStatisticItems.Count + 1, timeslots.Count + 2].Style.Font.Size = 11;
                    worksheet.Cells[exportLecturerStatisticItems.Count + 1, timeslots.Count + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[exportLecturerStatisticItems.Count + 1, timeslots.Count + 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[exportLecturerStatisticItems.Count + 1, timeslots.Count + 2].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(247, 113, 147));
                    worksheet.Cells[exportLecturerStatisticItems.Count + 1, timeslots.Count + 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    #endregion

                    #region Total Row
                    for (int i = 2; i <= timeslots.Count + 1; i++)
                    {
                        worksheet.Cells[exportLecturerStatisticItems.Count + 2, i].Value = statistic.CountAllGroupByTimeSlot?[i - 2];
                        worksheet.Cells[exportLecturerStatisticItems.Count + 2, i].Style.Font.Color.SetColor(Color.Black);
                        worksheet.Cells[exportLecturerStatisticItems.Count + 2, i].Style.Font.Bold = true;
                        worksheet.Cells[exportLecturerStatisticItems.Count + 2, i].Style.Font.Size = 11;
                        worksheet.Cells[exportLecturerStatisticItems.Count + 2, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[exportLecturerStatisticItems.Count + 2, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[exportLecturerStatisticItems.Count + 2, i].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(60, 162, 255));
                        worksheet.Cells[exportLecturerStatisticItems.Count + 2, i].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    }

                    worksheet.Cells[exportLecturerStatisticItems.Count + 2, 1].Value = "Total Task";
                    worksheet.Cells[exportLecturerStatisticItems.Count + 2, 1].Style.Font.Color.SetColor(Color.Black);
                    worksheet.Cells[exportLecturerStatisticItems.Count + 2, 1].Style.Font.Bold = true;
                    worksheet.Cells[exportLecturerStatisticItems.Count + 2, 1].Style.Font.Size = 11;
                    worksheet.Cells[exportLecturerStatisticItems.Count + 2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[exportLecturerStatisticItems.Count + 2, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[exportLecturerStatisticItems.Count + 2, 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(60, 162, 255));
                    worksheet.Cells[exportLecturerStatisticItems.Count + 2, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    worksheet.Cells[exportLecturerStatisticItems.Count + 2, timeslots.Count + 2].Value = statistic.TotalTask;
                    worksheet.Cells[exportLecturerStatisticItems.Count + 2, timeslots.Count + 2].Style.Font.Color.SetColor(Color.Black);
                    worksheet.Cells[exportLecturerStatisticItems.Count + 2, timeslots.Count + 2].Style.Font.Bold = true;
                    worksheet.Cells[exportLecturerStatisticItems.Count + 2, timeslots.Count + 2].Style.Font.Size = 11;
                    worksheet.Cells[exportLecturerStatisticItems.Count + 2, timeslots.Count + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[exportLecturerStatisticItems.Count + 2, timeslots.Count + 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[exportLecturerStatisticItems.Count + 2, timeslots.Count + 2].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(60, 162, 255));
                    worksheet.Cells[exportLecturerStatisticItems.Count + 2, timeslots.Count + 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    #endregion

                    #region Not Assigned Count
                    worksheet.Cells[exportLecturerStatisticItems.Count + 3, timeslots.Count + 1].Value = "Not Assigned Task";
                    worksheet.Cells[exportLecturerStatisticItems.Count + 3, timeslots.Count + 1].Style.Font.Color.SetColor(Color.Black);
                    worksheet.Cells[exportLecturerStatisticItems.Count + 3, timeslots.Count + 1].Style.Font.Bold = true;
                    worksheet.Cells[exportLecturerStatisticItems.Count + 3, timeslots.Count + 1].Style.Font.Size = 11;
                    worksheet.Cells[exportLecturerStatisticItems.Count + 3, timeslots.Count + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[exportLecturerStatisticItems.Count + 3, timeslots.Count + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[exportLecturerStatisticItems.Count + 3, timeslots.Count + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(60, 162, 255));
                    worksheet.Cells[exportLecturerStatisticItems.Count + 3, timeslots.Count + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    worksheet.Cells[exportLecturerStatisticItems.Count + 3, timeslots.Count + 2].Value = statistic.NotAssignedCount;
                    worksheet.Cells[exportLecturerStatisticItems.Count + 3, timeslots.Count + 2].Style.Font.Color.SetColor(Color.Black);
                    worksheet.Cells[exportLecturerStatisticItems.Count + 3, timeslots.Count + 2].Style.Font.Bold = true;
                    worksheet.Cells[exportLecturerStatisticItems.Count + 3, timeslots.Count + 2].Style.Font.Size = 11;
                    worksheet.Cells[exportLecturerStatisticItems.Count + 3, timeslots.Count + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[exportLecturerStatisticItems.Count + 3, timeslots.Count + 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[exportLecturerStatisticItems.Count + 3, timeslots.Count + 2].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(252, 255, 181));
                    worksheet.Cells[exportLecturerStatisticItems.Count + 3, timeslots.Count + 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    #endregion

                    #endregion

                    #region Statistic Subject + Slot

                    #region Header Column 
                    worksheet.Column(1).Width = 20;
                    worksheet.Column(timeslots.Count + 2).Width = 20;
                    worksheet.Cells[exportLecturerStatisticItems.Count + 5, 1].Value = "Subject/Slot";
                    worksheet.Cells[exportLecturerStatisticItems.Count + 5, 1].Style.Font.Color.SetColor(Color.Black);
                    worksheet.Cells[exportLecturerStatisticItems.Count + 5, 1].Style.Font.Bold = true;
                    worksheet.Cells[exportLecturerStatisticItems.Count + 5, 1].Style.Font.Size = 11;
                    worksheet.Cells[exportLecturerStatisticItems.Count + 5, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[exportLecturerStatisticItems.Count + 5, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[exportLecturerStatisticItems.Count + 5, 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(252, 255, 181));
                    worksheet.Cells[exportLecturerStatisticItems.Count + 5, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    for (int i = 2; i <= timeslots.Count + 1; i++)
                    {
                        worksheet.Column(i).Width = 28;
                        worksheet.Cells[exportLecturerStatisticItems.Count + 5, i].Value = timeslots[i - 2].Name;
                        worksheet.Cells[exportLecturerStatisticItems.Count + 5, i].Style.Font.Color.SetColor(Color.White);
                        worksheet.Cells[exportLecturerStatisticItems.Count + 5, i].Style.Font.Bold = true;
                        worksheet.Cells[exportLecturerStatisticItems.Count + 5, i].Style.Font.Size = 11;
                        worksheet.Cells[exportLecturerStatisticItems.Count + 5, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[exportLecturerStatisticItems.Count + 5, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[exportLecturerStatisticItems.Count + 5, i].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(60, 162, 255));
                        worksheet.Cells[exportLecturerStatisticItems.Count + 5, i].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    }
                    worksheet.Cells[exportLecturerStatisticItems.Count + 5, timeslots.Count + 2].Value = "Assigned Task";
                    worksheet.Cells[exportLecturerStatisticItems.Count + 5, timeslots.Count + 2].Style.Font.Color.SetColor(Color.Black);
                    worksheet.Cells[exportLecturerStatisticItems.Count + 5, timeslots.Count + 2].Style.Font.Bold = true;
                    worksheet.Cells[exportLecturerStatisticItems.Count + 5, timeslots.Count + 2].Style.Font.Size = 11;
                    worksheet.Cells[exportLecturerStatisticItems.Count + 5, timeslots.Count + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[exportLecturerStatisticItems.Count + 5, timeslots.Count + 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[exportLecturerStatisticItems.Count + 5, timeslots.Count + 2].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(247, 113, 147));
                    worksheet.Cells[exportLecturerStatisticItems.Count + 5, timeslots.Count + 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    #endregion

                    #region Data Row

                    for (int i = 0; i < exportSubjectsStatisticItems.Count; i++)
                    {
                        for (int j = 0; j < exportSubjectsStatisticItems[i].TimeSlotInfos?.Count; j++)
                        {
                            for (int k = 0; k < timeslots.Count; k++)
                            {
                                if (exportSubjectsStatisticItems?[i].TimeSlotInfos?[j][0].TimeSlotId == timeslots[k].Id)
                                {
                                    for (int l = 0; l < exportSubjectsStatisticItems?[i]?.TimeSlotInfos?[j]?.Count; l++)
                                    {

                                        //worksheet.Cells[exportLecturerStatisticItems.Count + 6 + i, j].Value = timeSlotInfo?.ClassName + "." + timeSlotInfo?.RoomName;
                                        worksheet.Cells[exportLecturerStatisticItems.Count + 6 + i, j].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        worksheet.Cells[exportLecturerStatisticItems.Count + 6 + i, j].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        worksheet.Cells[exportLecturerStatisticItems.Count + 6 + i, j].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(252, 255, 181));
                                    }
                                }

                            }
                            // subject column
                            worksheet.Cells[exportLecturerStatisticItems.Count + 6 + i, 1].Value = exportSubjectsStatisticItems[i].SubjectCode;
                            worksheet.Cells[exportLecturerStatisticItems.Count + 6 + i, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells[exportLecturerStatisticItems.Count + 6 + i, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[exportLecturerStatisticItems.Count + 6 + i, 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(247, 113, 147));
                            worksheet.Cells[exportLecturerStatisticItems.Count + 6 + i, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            // total column
                            worksheet.Cells[exportLecturerStatisticItems.Count + 6 + i, exportSubjectsStatisticItems[0].TimeSlotInfos.Count() + 2].Value = exportSubjectsStatisticItems[i].Total;
                            worksheet.Cells[exportLecturerStatisticItems.Count + 6 + i, exportSubjectsStatisticItems[0].TimeSlotInfos.Count() + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells[exportLecturerStatisticItems.Count + 6 + i, exportSubjectsStatisticItems[0].TimeSlotInfos.Count() + 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[exportLecturerStatisticItems.Count + 6 + i, exportSubjectsStatisticItems[0].TimeSlotInfos.Count() + 2].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(247, 113, 147));
                            worksheet.Cells[exportLecturerStatisticItems.Count + 6 + i, exportSubjectsStatisticItems[0].TimeSlotInfos.Count() + 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            // other timeslot column

                            var timeSlotInfo = exportLecturerStatisticItems[i].TimeSlotInfos?[j - 2];
                            if (timeSlotInfo.TimeSlotId == timeslots[i].Id)
                            {

                            }
                            worksheet.Cells[exportLecturerStatisticItems.Count + 6 + i, j].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        }
                    }

                    #endregion

                    #endregion
                    excelPackage.Save();
                    var stream = new MemoryStream(excelPackage.GetAsByteArray());
                    return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = excelName
                    };
                }
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

                string excelName = $"{DateTime.Now:yyyy-MM-dd}-Import-format-{DateTime.Now:HHmmss}.xlsx";
                using (var excelPackage = new ExcelPackage())
                {
                    var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

                    worksheet.Column(1).Width = 15;
                    worksheet.Cells[1, 1].Value = "Class";
                    worksheet.Cells[1, 1].Style.Font.Color.SetColor(Color.White);
                    worksheet.Cells[1, 1].Style.Font.Bold = true;
                    worksheet.Cells[1, 1].Style.Font.Size = 11;
                    worksheet.Cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(60, 162, 255));
                    worksheet.Cells[1, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet.Column(2).Width = 15;
                    worksheet.Cells[1, 2].Value = "Subject";
                    worksheet.Cells[1, 2].Style.Font.Color.SetColor(Color.White);
                    worksheet.Cells[1, 2].Style.Font.Bold = true;
                    worksheet.Cells[1, 2].Style.Font.Size = 11;
                    worksheet.Cells[1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, 2].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(60, 162, 255));
                    worksheet.Cells[1, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet.Column(3).Width = 15;
                    worksheet.Cells[1, 3].Value = "Dept";
                    worksheet.Cells[1, 3].Style.Font.Color.SetColor(Color.White);
                    worksheet.Cells[1, 3].Style.Font.Bold = true;
                    worksheet.Cells[1, 3].Style.Font.Size = 11;
                    worksheet.Cells[1, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, 3].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(60, 162, 255));
                    worksheet.Cells[1, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[1, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet.Column(4).Width = 15;
                    worksheet.Cells[1, 4].Value = "Slot";
                    worksheet.Cells[1, 4].Style.Font.Color.SetColor(Color.White);
                    worksheet.Cells[1, 4].Style.Font.Bold = true;
                    worksheet.Cells[1, 4].Style.Font.Size = 11;
                    worksheet.Cells[1, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, 4].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(60, 162, 255));
                    worksheet.Cells[1, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[1, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet.Column(5).Width = 15;
                    worksheet.Cells[1, 5].Value = "Slot1";
                    worksheet.Cells[1, 5].Style.Font.Color.SetColor(Color.White);
                    worksheet.Cells[1, 5].Style.Font.Bold = true;
                    worksheet.Cells[1, 5].Style.Font.Size = 11;
                    worksheet.Cells[1, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(60, 162, 255));
                    worksheet.Cells[1, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[1, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet.Column(6).Width = 15;
                    worksheet.Cells[1, 6].Value = "Slot2";
                    worksheet.Cells[1, 6].Style.Font.Color.SetColor(Color.White);
                    worksheet.Cells[1, 6].Style.Font.Bold = true;
                    worksheet.Cells[1, 6].Style.Font.Size = 11;
                    worksheet.Cells[1, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, 6].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(60, 162, 255));
                    worksheet.Cells[1, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[1, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                    worksheet.Column(7).Width = 15;
                    worksheet.Cells[1, 7].Value = "Room";
                    worksheet.Cells[1, 7].Style.Font.Color.SetColor(Color.White);
                    worksheet.Cells[1, 7].Style.Font.Bold = true;
                    worksheet.Cells[1, 7].Style.Font.Size = 11;
                    worksheet.Cells[1, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, 7].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(60, 162, 255));
                    worksheet.Cells[1, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[1, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet.Column(8).Width = 15;
                    worksheet.Cells[1, 8].Value = "Room2";
                    worksheet.Cells[1, 8].Style.Font.Color.SetColor(Color.White);
                    worksheet.Cells[1, 8].Style.Font.Bold = true;
                    worksheet.Cells[1, 8].Style.Font.Size = 11;
                    worksheet.Cells[1, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, 8].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(60, 162, 255));
                    worksheet.Cells[1, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[1, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet.Column(9).Width = 20;
                    worksheet.Cells[1, 9].Value = "Note";
                    worksheet.Cells[1, 9].Style.Font.Color.SetColor(Color.White);
                    worksheet.Cells[1, 9].Style.Font.Bold = true;
                    worksheet.Cells[1, 9].Style.Font.Size = 11;
                    worksheet.Cells[1, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, 9].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(60, 162, 255));
                    worksheet.Cells[1, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[1, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet.Column(10).Width = 15;
                    worksheet.Cells[1, 10].Value = "Note";
                    worksheet.Cells[1, 10].Style.Font.Color.SetColor(Color.White);
                    worksheet.Cells[1, 10].Style.Font.Bold = true;
                    worksheet.Cells[1, 10].Style.Font.Size = 11;
                    worksheet.Cells[1, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, 10].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(60, 162, 255));
                    worksheet.Cells[1, 10].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[1, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    #region Data Row

                    for (int i = 2; i <= exportItems.Count + 1; i++)
                    {
                        var Segments = _unitOfWork.TimeSlotSegmentRepository
                                       .GetByCondition(tss => tss.SlotId == exportItems[i - 2].TimeSlot?.Id && tss.Segment > 0)
                                       .OrderBy(item => item.Segment);
                        var DaySlot1Id = Segments.Take(1).ToList().FirstOrDefault()?.DayOfWeek ?? 0;
                        var DaySlot1Name = _unitOfWork.DayOfWeeksRepository.GetById(DaySlot1Id)?.Name ?? "";
                        var DaySlot2Id = Segments.Skip(1).Take(1).ToList().FirstOrDefault()?.DayOfWeek ?? 0;
                        var DaySlot2Name = _unitOfWork.DayOfWeeksRepository.GetById(DaySlot2Id)?.Name ?? "";

                        worksheet.Cells[i, 1].Value = exportItems[i - 2].Class?.Name ?? "";
                        worksheet.Cells[i, 2].Value = exportItems[i - 2].Subject?.Code ?? "";
                        worksheet.Cells[i, 3].Value = department;
                        worksheet.Cells[i, 4].Value = exportItems[i - 2].TimeSlot?.Name ?? "";
                        worksheet.Cells[i, 5].Value = DaySlot1Name;
                        worksheet.Cells[i, 6].Value = DaySlot2Name;
                        worksheet.Cells[i, 7].Value = exportItems[i - 2].Room1?.Name ?? "";
                        worksheet.Cells[i, 8].Value = "";
                        worksheet.Cells[i, 9].Value = exportItems[i - 2].Lecturer?.Email ?? "";
                        worksheet.Cells[i, 10].Value = exportItems[i - 2].Lecturer?.Email != null ? "" : "NOT_ASSIGNED";
                    }

                    #endregion
                    //////
                    excelPackage.Save();
                    var stream = new MemoryStream(excelPackage.GetAsByteArray());
                    return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = excelName
                    };
                }
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
                        var checkClassHeaderColumn = worksheet.Cells[1, 1].Value?.ToString()?.ToLower() != "class";
                        var checkSujectHeaderColumn = worksheet.Cells[1, 2].Value?.ToString()?.ToLower() != "subject";
                        var checkDeptHeaderColumn = worksheet.Cells[1, 3].Value?.ToString()?.ToLower() != "dept";
                        var checkSlotHeaderColumn = worksheet.Cells[1, 4].Value?.ToString()?.ToLower() != "slot";
                        var checkSlot1HeaderColumn = worksheet.Cells[1, 5].Value?.ToString()?.ToLower() != "slot1";
                        var checkSlot2HeaderColumn = worksheet.Cells[1, 6].Value?.ToString()?.ToLower() != "slot2";
                        var checkRoomHeaderColumn = worksheet.Cells[1, 7].Value?.ToString()?.ToLower() != "room";
                        var checkRoom2HeaderColumn = worksheet.Cells[1, 8].Value?.ToString()?.ToLower() != "room2";
                        var checkNoteHeaderColumn = worksheet.Cells[1, 9].Value?.ToString()?.ToLower() != "note";
                        var checkNote2HeaderColumn = worksheet.Cells[1, 10].Value?.ToString()?.ToLower() != "note";

                        if (checkClassHeaderColumn
                            || checkSujectHeaderColumn
                            || checkDeptHeaderColumn
                            || checkSlotHeaderColumn
                            || checkSlot1HeaderColumn
                            || checkSlot2HeaderColumn
                            || checkRoomHeaderColumn
                            || checkRoom2HeaderColumn
                            || checkNoteHeaderColumn
                            || checkNote2HeaderColumn)
                        {
                            return new ResponseResult("Please import correct format of the excel file from fap-timetable");
                        }



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

        public async Task<ResponseResult> ImportTimetableResult(GetAllRequest request, IFormFile file, CancellationToken cancellationToken)
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
                    return new ResponseResult("Please insert current semesters first");
                }
                var timeSlots = _unitOfWork.TimeSlotRepository.GetAll().Where(item => item.DepartmentHeadId == request.DepartmentHeadId && item.SemesterId == semesterId).ToList();
                if (timeSlots.Count == 0)
                {
                    return new ResponseResult("Please insert timeslots data first");
                }
                var subjects = _unitOfWork.SubjectRepository.GetAll().Where(item => item.DepartmentHeadId == request.DepartmentHeadId && item.SemesterId == semesterId).ToList();
                if (subjects.Count == 0)
                {
                    return new ResponseResult("Please insert subjects data first");
                }

                var lecturers = _unitOfWork.LecturerRepository.GetAll().Where(item => item.DepartmentHeadId == request.DepartmentHeadId && item.SemesterId == semesterId).ToList();
                if (lecturers.Count == 0)
                {
                    return new ResponseResult("Please insert lecturers data first");
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
                        var checkClassHeaderColumn = worksheet.Cells[1, 1].Value?.ToString()?.ToLower() != "class";
                        var checkSujectHeaderColumn = worksheet.Cells[1, 2].Value?.ToString()?.ToLower() != "subject";
                        var checkDeptHeaderColumn = worksheet.Cells[1, 3].Value?.ToString()?.ToLower() != "dept";
                        var checkSlotHeaderColumn = worksheet.Cells[1, 4].Value?.ToString()?.ToLower() != "slot";
                        var checkSlot1HeaderColumn = worksheet.Cells[1, 5].Value?.ToString()?.ToLower() != "slot1";
                        var checkSlot2HeaderColumn = worksheet.Cells[1, 6].Value?.ToString()?.ToLower() != "slot2";
                        var checkRoomHeaderColumn = worksheet.Cells[1, 7].Value?.ToString()?.ToLower() != "room";
                        var checkRoom2HeaderColumn = worksheet.Cells[1, 8].Value?.ToString()?.ToLower() != "room2";
                        var checkNoteHeaderColumn = worksheet.Cells[1, 9].Value?.ToString()?.ToLower() != "note";
                        var checkNote2HeaderColumn = worksheet.Cells[1, 10].Value?.ToString()?.ToLower() != "note";

                        if (checkClassHeaderColumn
                            || checkSujectHeaderColumn
                            || checkDeptHeaderColumn
                            || checkSlotHeaderColumn
                            || checkSlot1HeaderColumn
                            || checkSlot2HeaderColumn
                            || checkRoomHeaderColumn
                            || checkRoom2HeaderColumn
                            || checkNoteHeaderColumn
                            || checkNote2HeaderColumn)
                        {
                            return new ResponseResult("Please import correct format of the excel file from fap-timetable");
                        }

                        var rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++)
                        {
                            var className = worksheet.Cells[row, 1].Value?.ToString()?.Trim() ?? string.Empty;
                            var subjectCode = worksheet.Cells[row, 2].Value?.ToString()?.Trim() ?? string.Empty;
                            var subjectDepartment = worksheet.Cells[row, 3].Value?.ToString()?.Trim() ?? string.Empty;
                            var timeSlotName = worksheet.Cells[row, 4].Value?.ToString()?.Trim() ?? string.Empty;
                            var roomName = worksheet.Cells[row, 7].Value?.ToString()?.Trim() ?? string.Empty;
                            var lecturerEmail = worksheet.Cells[row, 9].Value?.ToString()?.Trim() ?? string.Empty;


                            var clasTemp = new Class();
                            var roomTemp = new Room();

                            var shortNameBuilding = new string(roomName.Take(2).ToArray()).Trim();
                            var clasFind = classes.Find(clas => clas.Name.Equals(className));
                            var roomFind = rooms.Find(room => room.Name.Equals(roomName));
                            var subjectFind = subjects.FirstOrDefault(sub => sub.Code.Trim().Equals(subjectCode));
                            var timeSlotFind = timeSlots.FirstOrDefault(ts => ts.Name.Trim().Equals(timeSlotName));
                            var lecturerFind = lecturers.FirstOrDefault(ts => ts.Email.ToLower().Trim().Equals(lecturerEmail.ToLower()));

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
                                LecturerId = lecturerFind?.Id == 0 ? 0 : lecturerFind?.Id,
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
