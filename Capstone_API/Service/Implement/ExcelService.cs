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
                RemoveTableBeforeImport(semesterId);

                var classes = new List<Class>();
                var rooms = new List<Room>();
                var subjects = new List<Subject>();
                var timeSlots = new List<TimeSlot>();
                var buildings = new List<Building>();
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
                            var buildingTemp = new Building();

                            var clasFind = classes.Find(clas => clas.Name.Equals(className));
                            var roomFind = rooms.Find(room => room.Name.Equals(roomName));
                            var subjectFind = subjects.Find(sub => sub.Code.Equals(subjectCode) || sub.Department.Equals(subjectDepartment));
                            var timeSlotFind = timeSlots.Find(ts => ts.Name.Equals(timeSlotName));
                            var shortNameBuilding = new string(roomName.Take(2).ToArray()).Trim();
                            var buildingFind = buildings.Find(b => b.ShortName.Equals(shortNameBuilding));


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
                            else if (timeSlots.Count() > 0 && timeSlotFind == null && !timeSlotName.Equals(string.Empty))
                            {
                                timeSlotTemp.Name = timeSlotName;
                                timeSlotTemp.Slot1 = timeSlotSlot1;
                                timeSlotTemp.Slot2 = timeSlotSlot2;
                                timeSlots.Add(timeSlotTemp);
                                _unitOfWork.TimeSlotRepository.Add(timeSlotTemp);
                            };

                            if (buildings.Count() == 0)
                            {
                                if (shortNameBuilding.Length > 0)
                                {
                                    buildingTemp.ShortName = shortNameBuilding;
                                    buildings.Add(buildingTemp);
                                    _unitOfWork.BuildingRepository.Add(buildingTemp);
                                }
                            }
                            else if (buildings.Count() > 0 && buildingFind == null)
                            {
                                if (shortNameBuilding.Length > 0)
                                {
                                    buildingTemp.ShortName = shortNameBuilding;
                                    buildings.Add(buildingTemp);
                                    _unitOfWork.BuildingRepository.Add(buildingTemp);
                                }
                            };
                            _unitOfWork.Complete();

                            var taskAssign = new TaskAssign()
                            {
                                ClassId = clasFind == null ? (clasTemp.Id == 0 ? null : clasTemp.Id) : (clasFind.Id == 0 ? null : clasFind.Id),
                                SubjectId = subjectFind == null ? (subjectTemp.Id == 0 ? null : subjectTemp.Id) : (subjectFind?.Id == 0 ? null : subjectFind?.Id),
                                TimeSlotId = timeSlotFind == null ? (timeSlotTemp.Id == 0 ? null : timeSlotTemp.Id) : (timeSlotFind?.Id == 0 ? null : timeSlotFind?.Id),
                                Room1Id = roomFind == null ? (roomTemp.Id == 0 ? null : roomTemp.Id) : (roomFind?.Id == 0 ? null : roomFind?.Id),
                            };

                            _unitOfWork.TaskRepository.Add(taskAssign);
                            _unitOfWork.Complete();
                        }
                    }
                }

                GenerateAreaDistanceDefault();
                GenerateSubjectPreferenceLevelDefault();
                GenerateTimeSlotConstainDefault();
                GenerateTimeSlotPreferenceLevelDefault();
                GenerateLecturerQuotaDefault();

                return new ResponseResult("Import excel and add new default data success", true);
            }
            catch (Exception ex)
            {
                return new ResponseResult($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        public void RemoveTableBeforeImport(int semesterId)
        {
            _unitOfWork.SubjectPreferenceLevelRepository.DeleteByCondition(item => item.SemesterId != semesterId, true);
            _unitOfWork.SlotPreferenceLevelRepository.DeleteByCondition(item => item.SemesterId != semesterId, true);
            _unitOfWork.AreaSlotWeightRepository.DeleteByCondition(item => item.SemesterId != semesterId, true);
            _unitOfWork.LecturerQuotaRepository.DeleteByCondition(item => item.SemesterId != semesterId, true);
            _unitOfWork.TimeSlotConflictRepository.DeleteByCondition(item => item.SemesterId != semesterId, true);
            _unitOfWork.TimeSlotCompatibilityRepository.DeleteByCondition(item => item.SemesterId != semesterId, true);
            _unitOfWork.DistanceRepository.DeleteByCondition(item => item.SemesterId != semesterId, true);

            _unitOfWork.BuildingRepository.DeleteByCondition(item => item.SemesterId != semesterId, true);
            _unitOfWork.TaskRepository.DeleteByCondition(item => item.SemesterId != semesterId, true);
            _unitOfWork.RoomRepository.DeleteByCondition(item => item.SemesterId != semesterId, true);
            _unitOfWork.ClassRepository.DeleteByCondition(item => item.SemesterId != semesterId, true);
            _unitOfWork.SubjectRepository.DeleteByCondition(item => item.SemesterId != semesterId, true);
            _unitOfWork.TimeSlotRepository.DeleteByCondition(item => item.SemesterId != semesterId, true);
            _unitOfWork.Complete();
        }

        public void GenerateSubjectPreferenceLevelDefault()
        {
            var subject = _unitOfWork.SubjectRepository.GetAll().ToList();
            var lecturers = _unitOfWork.LecturerRepository.GetAll().ToList();
            List<SubjectPreferenceLevel> subjectPreferenceLevels = new List<SubjectPreferenceLevel>();
            foreach (var item in lecturers)
            {
                foreach (var item2 in subject)
                {
                    var subjectPreferenceLevel = new SubjectPreferenceLevel()
                    {
                        LecturerId = item.Id,
                        SubjectId = item2.Id,
                        PreferenceLevel = 0
                    };
                    subjectPreferenceLevels.Add(subjectPreferenceLevel);
                }
            }

            _unitOfWork.SubjectPreferenceLevelRepository.AddRange(subjectPreferenceLevels);
            _unitOfWork.Complete();
        }

        public void GenerateTimeSlotPreferenceLevelDefault()
        {
            var timeslot = _unitOfWork.TimeSlotRepository.GetAll().ToList();
            var lecturers = _unitOfWork.LecturerRepository.GetAll().ToList();
            foreach (var item in lecturers)
            {
                foreach (var item2 in timeslot)
                {
                    var slotPreferenceLevel = new SlotPreferenceLevel()
                    {
                        LecturerId = item.Id,
                        SlotId = item2.Id,
                        PreferenceLevel = 0
                    };
                    _unitOfWork.SlotPreferenceLevelRepository.Add(slotPreferenceLevel);
                    _unitOfWork.Complete();
                }
            }
        }

        public void GenerateTimeSlotConstainDefault()
        {
            var timeslot = _unitOfWork.TimeSlotRepository.GetAll().ToList();
            var timeslot2 = _unitOfWork.TimeSlotRepository.GetAll().ToList();
            List<TimeSlotConflict> timeSlotConflicts = new();
            List<TimeSlotCompatibility> timeSlotCompatibilities = new();
            List<AreaSlotWeight> areaSlotWeights = new();

            foreach (var item in timeslot)
            {
                foreach (var item2 in timeslot2)
                {
                    TimeSlotConflict timeSlotConflict = new()
                    {
                        SlotId = item.Id,
                        ConflictSlotId = item2.Id,
                        Conflict = false
                    };
                    timeSlotConflicts.Add(timeSlotConflict);
                    TimeSlotCompatibility timeSlotCompatibility = new()
                    {
                        SlotId = item.Id,
                        CompatibilitySlotId = item2.Id,
                        CompatibilityLevel = 0
                    };
                    timeSlotCompatibilities.Add(timeSlotCompatibility);
                    AreaSlotWeight areaSlotWeight = new()
                    {
                        SlotId = item.Id,
                        AreaSlotId = item2.Id,
                        AreaSlotWeight1 = 0
                    };
                    areaSlotWeights.Add(areaSlotWeight);
                }
            }
            _unitOfWork.TimeSlotConflictRepository.AddRange(timeSlotConflicts);
            _unitOfWork.TimeSlotCompatibilityRepository.AddRange(timeSlotCompatibilities);
            _unitOfWork.AreaSlotWeightRepository.AddRange(areaSlotWeights);
            _unitOfWork.Complete();
        }

        public void GenerateAreaDistanceDefault()
        {
            var building = _unitOfWork.BuildingRepository.GetAll().ToList();
            var building2 = _unitOfWork.BuildingRepository.GetAll().ToList();
            List<Distance> distances = new();

            foreach (var item in building)
            {
                foreach (var item2 in building2)
                {
                    Distance distance = new()
                    {
                        Building1Id = item.Id,
                        Building2Id = item2.Id,
                        DistanceBetween = 0,
                    };
                    distances.Add(distance);

                }
            }
            _unitOfWork.DistanceRepository.AddRange(distances);
            _unitOfWork.Complete();
        }

        public void GenerateLecturerQuotaDefault()
        {
            var lecturers = _unitOfWork.LecturerRepository.GetAll().ToList();
            List<LecturerQuotum> lecturerQuotaList = new();
            foreach (var item in lecturers)
            {
                var lecturerQuota = new LecturerQuotum()
                {
                    LecturerId = item.Id,
                    Quota = 8
                };
                lecturerQuotaList.Add(lecturerQuota);
            }
            _unitOfWork.LecturerQuotaRepository.AddRange(lecturerQuotaList);
            _unitOfWork.Complete();
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
