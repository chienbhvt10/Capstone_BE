using Capstone_API.DTO.Excel;
using Capstone_API.Models;
using Capstone_API.Results;
using Capstone_API.Service.Interface;
using Capstone_API.UOW_Repositories.UnitOfWork;
using OfficeOpenXml;

namespace Capstone_API.Service.Implement
{
    public class ExcelService : IExcelService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IUnitOfWork _unitOfWork;

        public ExcelService(IWebHostEnvironment hostingEnvironment, IUnitOfWork unitOfWork)
        {
            _hostingEnvironment = hostingEnvironment;
            _unitOfWork = unitOfWork;
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
            try
            {
                //save executeId and time of semester by executeSemesterId
                // and get executeSemesterId in database
                // this code must be get executeSemesterId in database

                var semesterId = 0;
                _unitOfWork.TaskRepository.DeleteByCondition(item => item.SemesterId != semesterId, true);
                _unitOfWork.RoomRepository.DeleteByCondition(item => item.SemesterId != semesterId, true);
                _unitOfWork.ClassRepository.DeleteByCondition(item => item.SemesterId != semesterId, true);
                _unitOfWork.SubjectRepository.DeleteByCondition(item => item.SemesterId != semesterId, true);
                _unitOfWork.TimeSlotRepository.DeleteByCondition(item => item.SemesterId != semesterId, true);
                _unitOfWork.Complete();

                var classes = new List<Class>();
                var rooms = new List<Room>();
                var subjects = new List<Subject>();
                var timeSlots = new List<TimeSlot>();

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
                            var timeSlotSlot1 = worksheet.Cells[row, 5].Value?.ToString()?.Trim() ?? string.Empty;
                            var timeSlotSlot2 = worksheet.Cells[row, 6].Value?.ToString()?.Trim() ?? string.Empty;
                            var roomName = worksheet.Cells[row, 7].Value?.ToString()?.Trim() ?? string.Empty;

                            var clasTemp = new Class();
                            var roomTemp = new Room();
                            var subjectTemp = new Subject();
                            var timeSlotTemp = new TimeSlot();

                            var clasFind = classes.Find(clas => clas.Name.Equals(className));
                            var roomFind = rooms.Find(room => room.Name.Equals(roomName));
                            var subjectFind = subjects.Find(sub => sub.Code.Equals(subjectCode) || sub.Department.Equals(subjectDepartment));
                            var timeSlotFind = timeSlots.Find(ts => ts.Name.Equals(timeSlotName));

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

                            if (subjects.Count() == 0)
                            {
                                subjectTemp.Code = subjectCode;
                                subjectTemp.Department = subjectDepartment;
                                subjects.Add(subjectTemp);
                                _unitOfWork.SubjectRepository.Add(subjectTemp);
                            }
                            else if (subjects.Count() > 0 && subjectFind == null)
                            {
                                subjectTemp.Code = subjectCode;
                                subjectTemp.Department = subjectDepartment;
                                subjects.Add(subjectTemp);
                                _unitOfWork.SubjectRepository.Add(subjectTemp);
                            };

                            if (timeSlots.Count() == 0)
                            {
                                timeSlotTemp.Name = timeSlotName;
                                timeSlotTemp.Slot1 = timeSlotSlot1;
                                timeSlotTemp.Slot2 = timeSlotSlot2;
                                timeSlots.Add(timeSlotTemp);
                                _unitOfWork.TimeSlotRepository.Add(timeSlotTemp);
                            }
                            else if (timeSlots.Count() > 0 && timeSlotFind == null)
                            {
                                timeSlotTemp.Name = timeSlotName;
                                timeSlotTemp.Slot1 = timeSlotSlot1;
                                timeSlotTemp.Slot2 = timeSlotSlot2;
                                timeSlots.Add(timeSlotTemp);
                                _unitOfWork.TimeSlotRepository.Add(timeSlotTemp);
                            };
                            _unitOfWork.Complete();

                            var taskAssign = new TaskAssign()
                            {
                                ClassId = clasFind == null ? (clasTemp.Id == 0 ? null : clasTemp.Id) : (clasFind.Id == 0 ? null : clasFind.Id),
                                SubjectId = subjectFind == null ? (subjectTemp.Id == 0 ? null : subjectTemp.Id) : (subjectFind?.Id == 0 ? null : subjectFind?.Id),
                                TimeSlotId = timeSlotFind == null ? (timeSlotTemp.Id == 0 ? null : timeSlotTemp.Id) : (timeSlotFind?.Id == 0 ? null : timeSlotFind?.Id),
                                Room1Id = roomFind == null ? (roomTemp.Id == 0 ? null : roomTemp.Id) : (roomFind?.Id == 0 ? null : roomFind?.Id),
                            };
                            subjectTemp.OrderNumber = subjectTemp.Id;
                            timeSlotTemp.OrderNumber = timeSlotTemp.Id;

                            _unitOfWork.TaskRepository.Add(taskAssign);
                            _unitOfWork.Complete();
                        }
                    }
                }

                return new GenericResult<IEnumerable<TaskAssignImportDTO>>();
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
