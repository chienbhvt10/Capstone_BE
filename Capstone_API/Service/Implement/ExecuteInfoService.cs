using AutoMapper;
using Capstone_API.DTO.Task.Fetch;
using Capstone_API.Models;
using Capstone_API.Results;
using Capstone_API.Service.Interface;
using Capstone_API.UOW_Repositories.UnitOfWork;
using System.Text;
using System.Text.Json;

namespace Capstone_API.Service.Implement
{
    public class ExecuteInfoService : IExecuteInfoService
    {
        private readonly HttpClient _httpClient;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ExecuteInfoService(IUnitOfWork unitOfWork, IMapper mapper, HttpClient httpClient)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpClient = httpClient;
        }
        public GenericResult<List<ExecuteInfoResponse>> GetAll(int semesterId)
        {
            try
            {
                var executeInfo = _unitOfWork.ExecuteInfoRepository.GetAll().Where(item => item.SemesterId == semesterId);
                var executeInfoViewModel = _mapper.Map<List<ExecuteInfoResponse>>(executeInfo).OrderByDescending(item => item.ExecuteTime).ToList();
                return new GenericResult<List<ExecuteInfoResponse>>(executeInfoViewModel, true);
            }
            catch (Exception ex)
            {
                return new GenericResult<List<ExecuteInfoResponse>>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        public ResponseResult CreateExecuteInfo(ExecuteInfoResponse request)
        {
            try
            {
                var executeInfo = _mapper.Map<ExecuteInfo>(request);
                _unitOfWork.ExecuteInfoRepository.Add(executeInfo);
                _unitOfWork.Complete();
                return new ResponseResult("Create successfully", true);
            }
            catch (Exception ex)
            {
                return new ResponseResult($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        #region Fetch Scheduler
        public void UpdateTaskByExecuteData(IEnumerable<ExecuteData>? executeData)
        {

            if (executeData != null)
            {
                foreach (var data in executeData)
                {
                    var taskAssign = _unitOfWork.TaskRepository.Find((item) => item.Id == Convert.ToInt32(data.taskId));
                    if (taskAssign != null)
                    {
                        if (data.instructorId != null)
                        {
                            taskAssign.LecturerId = Convert.ToInt32(data.instructorId);
                            _unitOfWork.TaskRepository.Update(taskAssign);
                            _unitOfWork.Complete();
                        }
                        else
                        {
                            taskAssign.LecturerId = null;
                            _unitOfWork.TaskRepository.Update(taskAssign);
                            _unitOfWork.Complete();
                        }
                    }
                }
            }
        }
        public async Task<ResponseResult> GetSchedule(string executeId)
        {
            try
            {
                var fetchRequest = new FetchDataByExecuteIdRequest() { sessionHash = executeId, solutionNo = 1, token = "" };
                var jsonRequest = JsonSerializer.Serialize(fetchRequest);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"https://localhost:7000/ATTASAPI/get", content);
                var json = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new ResponseResult("Fetch fail");
                }
                var dataFetch = JsonSerializer.Deserialize<DataFetch>(json);

                var fetchDataByExcecuteIdResponse = new FetchDataByExcecuteIdResponse()
                {
                    data = dataFetch,
                    code = 200,
                    message = "success",
                    success = true
                };
                IEnumerable<ExecuteData>? executeData = dataFetch?.results;

                UpdateTaskByExecuteData(executeData);

                return new ResponseResult("Fetch data success", true);
            }
            catch (Exception ex)
            {
                return new ResponseResult($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        #endregion

        #region Execute

        public void SaveExecuteId(ExecuteResponse result)
        {

            var executeInfo = new ExecuteInfo()
            {
                ExecuteId = result.sessionId,
                ExecuteTime = DateTime.Now,
                SemesterId = _unitOfWork.SemesterInfoRepository.GetAll().FirstOrDefault(item => item.IsNow == true)?.Id
            };
            _unitOfWork.ExecuteInfoRepository.Add(executeInfo);
            _unitOfWork.Complete();
        }

        public async Task<GenericResult<ExecuteResponse>> Execute(SettingRequest request)
        {
            try
            {
                ExecuteFetchRequest executeFetchRequest = GetExecuteFetchRequest(request);

                var jsonRequest = JsonSerializer.Serialize(executeFetchRequest);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"https://localhost:7000/ATTASAPI/excecute", content);

                var json = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new GenericResult<ExecuteResponse>("Fetch fail");
                }
                var result = JsonSerializer.Deserialize<ExecuteResponse>(json);

                if (result != null && result.sessionId != null && !result.sessionId.Equals(""))
                {
                    SaveExecuteId(result);
                }

                return new GenericResult<ExecuteResponse>(result, true);
            }
            catch (Exception ex)
            {
                return new GenericResult<ExecuteResponse>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        #region GetExecuteFetchRequest
        public ExecuteFetchRequest GetExecuteFetchRequest(SettingRequest request)
        {
            int currentSemesterId = _unitOfWork.SemesterInfoRepository.GetAll().FirstOrDefault(item => item.IsNow == true)?.Id ?? 0;
            List<int> InstructorQuota = GetInstructorQuota(currentSemesterId);
            List<int> InstructorMinQuota = GetInstructorMinQuota(currentSemesterId);
            List<int?> PatternCost = GetPatternCost(currentSemesterId);
            List<List<int>>? SlotConflict = GetSlotConflict(currentSemesterId);
            List<List<int?>>? AreaSlotCoefficient = GetAreaSlotCoefficient(currentSemesterId);
            List<List<int?>>? InstructorSubject = GetInstructorSubject(currentSemesterId);
            List<List<int?>>? InstructorSlot = GetInstructorSlot(currentSemesterId);
            List<List<int?>>? AreaDistance = GetAreaDistance();
            List<List<int>>? Slotday = GetSlotDays(currentSemesterId);
            List<List<int>>? SlotTimes = GetSlotTimes(currentSemesterId);

            List<Building> Buildings = _unitOfWork.BuildingRepository.GetAll().ToList();
            List<SlotFetchRequest> Slots = GetSlotFetchRequest(currentSemesterId);
            List<DayOfWeekFetchRequest> DayOfWeeks = GetDayOfWeekFetchRequest();
            List<SubjectFetchRequest> Subjects = GetSubjectFetchRequest(currentSemesterId);
            List<InstructorFetchRequest> Instructors = GetInstructorFetchRequest(currentSemesterId);
            List<TaskFetchRequest> Tasks = GetTaskFetchRequest(Slots, Subjects, currentSemesterId);
            List<List<int>>? SlotSegments = GetSlotSegments(Slots, DayOfWeeks, currentSemesterId);
            List<TaskPreAssignFetchRequest> PreAssign = GetTaskPreAssignFetchRequest(Instructors, Tasks, currentSemesterId);

            var slotPerDay = _unitOfWork.NumSegmentsRepository.GetAll().Where(item => item.SemesterId == currentSemesterId).FirstOrDefault();

            ExecuteFetchRequest executeFetchRequest = new()
            {
                Token = "",
                SessionHash = "",
                BackupInstructor = -1,
                NumTasks = Tasks.Count,
                NumInstructors = Instructors.Count,
                NumSlots = Slots.Count,
                NumDays = _unitOfWork.DayOfWeeksRepository.GetAll().Count(),
                NumTimes = 2,
                NumSegments = slotPerDay != null ? (slotPerDay.NumberOfSlots ?? 0) : 4,
                NumSegmentRules = SlotSegments.Count,
                NumSubjects = Subjects.Count,
                NumAreas = Buildings.Count,
                Setting = request,
                SlotConflict = SlotConflict,
                AreaSlotCoefficient = AreaSlotCoefficient,
                InstructorSubject = InstructorSubject,
                InstructorSlot = InstructorSlot,
                AreaDistance = AreaDistance,
                InstructorQuota = InstructorQuota,
                Slots = Slots,
                Instructors = Instructors,
                Tasks = Tasks,
                PreAssign = PreAssign,
                SlotDay = Slotday,
                InstructorMinQuota = InstructorMinQuota,
                PatternCost = PatternCost,
                SlotSegment = SlotSegments,
                SlotTime = SlotTimes

            };
            return executeFetchRequest;
        }

        #endregion

        #region GetSlotTimes
        // Timeslot AM or PM
        private List<List<int>> GetSlotTimes(int currentSemesterid)
        {
            List<List<int>> slotTimes = new();
            List<TimeSlot> timeSlots =
                _unitOfWork.TimeSlotRepository.GetAll()
                    .Where(item => item.SemesterId == currentSemesterid)
                    .OrderBy(item => item.Id)
                    .ToList();
            foreach (var item in timeSlots)
            {
                if (item.AmorPm == 0)
                {
                    slotTimes.Add(new List<int>() { 0, 1 });
                };
                if (item.AmorPm == 1)
                {
                    slotTimes.Add(new List<int>() { 1, 0 });
                };
            }
            return slotTimes;
        }

        #endregion

        #region GetSlotSegments
        // slotsegment info with slot order, day order, segment order
        private List<List<int>> GetSlotSegments(List<SlotFetchRequest> Slots, List<DayOfWeekFetchRequest> dayOfWeeks, int currentSemesterid)
        {
            List<List<int>> slotSegments = new();
            List<TimeSlotSegment> timeSlotSegments =
                _unitOfWork.TimeSlotSegmentRepository.GetAll()
                    .Where(item => item.SemesterId == currentSemesterid)
                    .OrderBy(item => item.Id)
                    .ToList();

            foreach (var timeSlotSegment in timeSlotSegments)
            {
                if (timeSlotSegment.Segment > 0)
                {
                    slotSegments.Add(new List<int>()
                    {
                        Slots.Where(i => Convert.ToInt32(i.Id) == timeSlotSegment.SlotId).Select(item => item.Order).FirstOrDefault(),
                        dayOfWeeks.Where(i => Convert.ToInt32(i.Id) == timeSlotSegment.DayOfWeek).Select(item => item.Order).FirstOrDefault(),
                        timeSlotSegment.Segment.HasValue && timeSlotSegment.Segment.Value > 0 ? timeSlotSegment.Segment.Value - 1: 0
                    });
                }
            }
            return slotSegments;
        }

        #endregion

        #region GetSlotDays
        // Timeslot in what day on weeks
        private List<List<int>> GetSlotDays(int currentSemesterid)
        {
            List<List<int>> slotDays =
                _unitOfWork.TimeSlotSegmentRepository.GetAll()
                .Where(item => item.SlotId != null && item.SemesterId == currentSemesterid)
                .OrderBy(item => item.SlotId).GroupBy(item => item.SlotId)
                .Select(gr => gr.OrderBy(item => item.DayOfWeek)
                                .Select(item => item.Segment != 0 ? 1 : 0)
                                .ToList())
                .ToList();
            return slotDays;
        }

        #endregion

        #region GetPatternCost
        private int CountZerosBetweenOnes(int n)
        {
            string binaryN = Convert.ToString(n, 2);
            int count = 0;
            int? lastOneIndex = null;
            for (int i = 0; i < binaryN.Length; i++)
            {
                if (binaryN[i] == '1')
                {
                    if (lastOneIndex != null)
                    {
                        count += i - lastOneIndex.Value - 1;
                    }
                    lastOneIndex = i;
                }
            }
            return count;
        }
        // calculate number of number 0 between number 1 in binarynumber 2^NumberOfSegment similar to space between 2 timeslot
        private List<int?> GetPatternCost(int currentSemesterid)
        {
            var slotPerDay = _unitOfWork.NumSegmentsRepository.GetAll().Where(item => item.SemesterId == currentSemesterid).FirstOrDefault();
            var combineParrentCost = new List<int?>();
            var binaryTarget = Convert.ToInt32(Math.Pow(2, Convert.ToDouble(slotPerDay != null ? slotPerDay.NumberOfSlots : 4)));
            for (int i = 0; i < binaryTarget; i++)
            {
                combineParrentCost.Add(CountZerosBetweenOnes(i));
            }
            return combineParrentCost;
        }

        #endregion

        #region GetAreaDistance
        private List<List<int?>> GetAreaDistance()
        {
            return _unitOfWork.DistanceRepository.GetAll()
                                        .OrderBy(item => item.Building1Id)
                                        .GroupBy(item => item.Building1Id)
                                        .Select(item => item.Select(item => item.DistanceBetween)
                                        .ToList()).ToList();
        }
        #endregion

        #region GetTaskFetchRequest
        private List<TaskFetchRequest> GetTaskFetchRequest(List<SlotFetchRequest> Slots, List<SubjectFetchRequest> Subjects, int currentSemesterid)
        {
            List<TaskFetchRequest>? Tasks = new();

            int count = 0;
            List<TaskAssign> taskAssigns = _unitOfWork.TaskRepository.GetAll()
                .Where(item => item.SemesterId == currentSemesterid)
                .OrderBy(item => item.Id)
                .ToList();

            foreach (var task in taskAssigns)
            {
                Tasks.Add(new TaskFetchRequest()
                {
                    Id = task.Id.ToString(),
                    Order = count,
                    SlotOrder = Slots.Where(i => Convert.ToInt32(i.Id) == task.TimeSlotId).Select(item => item.Order).FirstOrDefault(),
                    SubjectOrder = Subjects.Where(i => Convert.ToInt32(i.Id) == task.SubjectId).Select(item => item.Order).FirstOrDefault(),
                });
                count++;
            }

            return Tasks;
        }
        #endregion

        #region GetTaskPreAssignFetchRequest
        private List<TaskPreAssignFetchRequest> GetTaskPreAssignFetchRequest(List<InstructorFetchRequest> Instructors, List<TaskFetchRequest> Tasks, int currentSemesterid)
        {
            List<TaskPreAssignFetchRequest>? PreAssign = _unitOfWork.TaskRepository.GetAll()
                .Where(item => item.LecturerId != null
                                && (item.PreAssign ?? false)
                                && item.SemesterId == currentSemesterid)
                .Select(item => new TaskPreAssignFetchRequest()
                {
                    InstructorOrder =
                        Instructors.Where(i => Convert.ToInt32(i.Id) == item.LecturerId)
                                    .Select(item => item.Order)
                                    .FirstOrDefault(),
                    TaskOrder =
                        Tasks.Where(t => Convert.ToInt32(t.Id) == item.Id)
                              .Select(item => item.Order)
                              .FirstOrDefault(),
                    Type = 1
                }).ToList();
            return PreAssign;
        }
        #endregion

        #region GetSubjectFetchRequest
        private List<SubjectFetchRequest> GetSubjectFetchRequest(int currentSemesterid)
        {
            List<SubjectFetchRequest>? Subjects = new();

            int count = 0;
            List<Subject> subjects =
                _unitOfWork.SubjectRepository.GetAll()
                .Where(item => item.SemesterId == currentSemesterid)
                .OrderBy(item => item.Id)
                .ToList();
            foreach (var lecturer in subjects)
            {
                Subjects.Add(new SubjectFetchRequest()
                {
                    Id = lecturer.Id.ToString(),
                    Order = count
                });
                count++;
            }

            return Subjects;
        }
        #endregion

        #region GetDayOfWeekFetchRequest
        private List<DayOfWeekFetchRequest> GetDayOfWeekFetchRequest()
        {
            List<DayOfWeekFetchRequest>? DayOfWeeks = new();

            int count = 0;
            List<Models.DayOfWeek> dayOfWeeks = _unitOfWork.DayOfWeeksRepository.GetAll().OrderBy(item => item.Id).ToList();
            foreach (var dayOfWeek in dayOfWeeks)
            {
                DayOfWeeks.Add(new DayOfWeekFetchRequest()
                {
                    Id = dayOfWeek.Id,
                    Order = count
                });
                count++;
            }

            return DayOfWeeks;
        }
        #endregion

        #region GetInstructorFetchRequest
        private List<InstructorFetchRequest> GetInstructorFetchRequest(int currentSemesterid)
        {
            List<InstructorFetchRequest>? Instructors = new();

            int count = 0;
            List<Lecturer> lecturers = _unitOfWork.LecturerRepository.GetAll()
                                        .Where(item => item.SemesterId == currentSemesterid)
                                        .OrderBy(item => item.Id)
                                        .ToList();
            foreach (var lecturer in lecturers)
            {
                Instructors.Add(new InstructorFetchRequest()
                {
                    Id = lecturer.Id.ToString(),
                    Order = count
                });
                count++;
            }

            return Instructors;
        }
        #endregion

        #region GetSlotFetchRequest
        private List<SlotFetchRequest> GetSlotFetchRequest(int currentSemesterid)
        {
            List<SlotFetchRequest>? Slots = new();
            int count = 0;
            List<TimeSlot> timeSlots = _unitOfWork.TimeSlotRepository.GetAll()
                                        .Where(item => item.SemesterId == currentSemesterid)
                                        .OrderBy(item => item.Id)
                                        .ToList();
            foreach (var slot in timeSlots)
            {
                Slots.Add(new SlotFetchRequest()
                {
                    Id = slot.Id.ToString(),
                    Order = count
                });
                count++;
            }

            return Slots;
        }
        #endregion

        #region GetInstructorSlot
        private List<List<int?>> GetInstructorSlot(int currentSemesterid)
        {
            return _unitOfWork.SlotPreferenceLevelRepository.GetAll()
                            .Where(item => item.SemesterId == currentSemesterid && item.LecturerId != null)
                            .OrderBy(item => item.LecturerId)
                            .GroupBy(item => item.LecturerId)
                            .Select(item => item.Select(item => item.PreferenceLevel)
                            .ToList()).ToList();
        }
        #endregion

        #region GetInstructorSubject
        private List<List<int?>> GetInstructorSubject(int currentSemesterid)
        {
            return _unitOfWork.SubjectPreferenceLevelRepository.GetAll()
                            .Where(item => item.SemesterId == currentSemesterid && item.LecturerId != null)
                            .OrderBy(item => item.LecturerId)
                            .GroupBy(item => item.LecturerId)
                            .Select(item => item.Select(item => item.PreferenceLevel)
                            .ToList()).ToList();
        }
        #endregion

        #region GetInstructorQuota
        private List<int> GetInstructorQuota(int currentSemesterid)
        {
            return _unitOfWork.LecturerRepository.GetAll()
                            .Where(item => item.SemesterId == currentSemesterid)
                            .OrderBy(item => item.Id)
                            .Select(item => item.Quota ?? 4)
                            .ToList();
        }
        #endregion

        #region GetInstructorMinQuota

        // Lecturer must be assign atleast number of quota
        private List<int> GetInstructorMinQuota(int currentSemesterid)
        {
            return _unitOfWork.LecturerRepository.GetAll()
                            .Where(item => item.SemesterId == currentSemesterid)
                           .OrderBy(item => item.Id)
                           .Select(item => item.MinQuota ?? 0)
                           .ToList();
        }

        #endregion

        #region GetAreaSlotCoefficient
        private List<List<int?>> GetAreaSlotCoefficient(int currentSemesterid)
        {
            return _unitOfWork.AreaSlotWeightRepository.GetAll()
                            .Where(item => item.SemesterId == currentSemesterid)
                            .OrderBy(item => item.SlotId)
                            .GroupBy(item => item.SlotId)
                            .Select(item => item.Select(item => item.AreaSlotWeight1)
                            .ToList()).ToList();
        }
        #endregion

        #region GetSlotConflict
        private List<List<int>> GetSlotConflict(int currentSemesterid)
        {
            return _unitOfWork.TimeSlotConflictRepository.GetAll()
                            .Where(item => item.SemesterId == currentSemesterid)
                            .OrderBy(item => item.SlotId)
                            .GroupBy(item => item.SlotId)
                            .Select(item => item.Select(item => (bool)item.Conflict ? 1 : 0)
                            .ToList()).ToList();
        }
        #endregion

        #endregion
    }
}
