using AutoMapper;
using Capstone_API.DTO.Task.Fetch;
using Capstone_API.DTO.Task.Request;
using Capstone_API.DTO.Task.Response;
using Capstone_API.Models;
using Capstone_API.Results;
using Capstone_API.Service.Interface;
using Capstone_API.UOW_Repositories.UnitOfWork;
using System.Text;
using System.Text.Json;

namespace Capstone_API.Service.Implement
{
    public class TaskService : ITaskService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;

        public TaskService(IUnitOfWork unitOfWork, IMapper mapper, HttpClient httpClient)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpClient = httpClient;
        }

        #region GetATask
        public GenericResult<QueryDataByLecturerAndTimeSlot> GetATask(int taskId)
        {
            try
            {
                var query = GetTaskResponses().Where(item => item.TaskId == taskId).FirstOrDefault();
                query ??= _mapper.Map<QueryDataByLecturerAndTimeSlot>(GetTasksNotAssignForDetail().Where(item => item.TaskId == taskId).FirstOrDefault());

                return new GenericResult<QueryDataByLecturerAndTimeSlot>(query, true);
            }
            catch (Exception ex)
            {
                return new GenericResult<QueryDataByLecturerAndTimeSlot>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
        #endregion

        #region SearchTask
        public GenericResult<SearchDTO> SearchTask(DTO.Task.Request.GetAllTaskAssignDTO request)
        {
            try
            {
                List<List<TimeSlotInfo>> querySearchDataNotAssign = GetTasksNotAssign();
                IEnumerable<ResponseTaskByLecturerIsKey> querySearchDataAssign = GetTaskResponseByLecturerKey();
                if (request.SemesterId != 0)
                {
                    querySearchDataAssign = querySearchDataAssign.Where(item => item.SemesterId == request.SemesterId);
                }
                if (request.LecturerIds.Count != 0)
                {
                    querySearchDataAssign = querySearchDataAssign.Where(item =>
                    request.LecturerIds.Count != 0 && request.LecturerIds.Contains(item.LecturerId));
                }
                if (request.ClassIds.Count != 0)
                {
                    querySearchDataAssign = querySearchDataAssign.Select(innerItem => new ResponseTaskByLecturerIsKey()
                    {
                        LecturerId = innerItem.LecturerId,
                        SemesterId = innerItem.SemesterId,
                        LecturerName = innerItem.LecturerName,
                        Total = innerItem.Total,
                        TimeSlotInfos = innerItem.TimeSlotInfos?.Select(item => new TimeSlotInfo()
                        {
                            ClassId = request.ClassIds.Contains(item.ClassId) ? item.ClassId : 0,
                            ClassName = request.ClassIds.Contains(item.ClassId) ? item.ClassName : "",
                            IsAssign = request.ClassIds.Contains(item.ClassId) ? item.IsAssign : 0,
                            PreAssign = request.ClassIds.Contains(item.ClassId) && item.PreAssign,
                            RoomId = request.ClassIds.Contains(item.ClassId) ? item.RoomId : 0,
                            RoomName = request.ClassIds.Contains(item.ClassId) ? item.RoomName : "",
                            Status = request.ClassIds.Contains(item.ClassId) ? item.Status : "",
                            SubjectCode = request.ClassIds.Contains(item.ClassId) ? item.SubjectCode : "",
                            SubjectId = request.ClassIds.Contains(item.ClassId) ? item.SubjectId : 0,
                            SubjectName = request.ClassIds.Contains(item.ClassId) ? item.SubjectName : "",
                            TaskId = request.ClassIds.Contains(item.ClassId) ? item.TaskId : 0,
                            TimeSlotId = request.ClassIds.Contains(item.ClassId) ? item.TimeSlotId : 0,
                            TimeSlotName = request.ClassIds.Contains(item.ClassId) ? item.TimeSlotName : "",
                        }).ToList()
                    });
                    querySearchDataNotAssign = querySearchDataNotAssign.Select(innerItem => innerItem.Where(item => request.ClassIds.Contains(item.ClassId)).ToList()).ToList();
                }
                if (request.SubjectIds.Count != 0)
                {
                    querySearchDataAssign = querySearchDataAssign.Select(innerItem => new ResponseTaskByLecturerIsKey()
                    {
                        LecturerId = innerItem.LecturerId,
                        SemesterId = innerItem.SemesterId,
                        LecturerName = innerItem.LecturerName,
                        Total = innerItem.Total,
                        TimeSlotInfos = innerItem.TimeSlotInfos?.Select(item => new TimeSlotInfo()
                        {
                            ClassId = request.SubjectIds.Contains(item.SubjectId) ? item.ClassId : 0,
                            ClassName = request.SubjectIds.Contains(item.SubjectId) ? item.ClassName : "",
                            IsAssign = request.SubjectIds.Contains(item.SubjectId) ? item.IsAssign : 0,
                            PreAssign = request.SubjectIds.Contains(item.SubjectId) && item.PreAssign,
                            RoomId = request.SubjectIds.Contains(item.SubjectId) ? item.RoomId : 0,
                            RoomName = request.SubjectIds.Contains(item.SubjectId) ? item.RoomName : "",
                            Status = request.SubjectIds.Contains(item.SubjectId) ? item.Status : "",
                            SubjectCode = request.SubjectIds.Contains(item.SubjectId) ? item.SubjectCode : "",
                            SubjectId = request.SubjectIds.Contains(item.SubjectId) ? item.SubjectId : 0,
                            SubjectName = request.SubjectIds.Contains(item.SubjectId) ? item.SubjectName : "",
                            TaskId = request.SubjectIds.Contains(item.SubjectId) ? item.TaskId : 0,
                            TimeSlotId = request.SubjectIds.Contains(item.SubjectId) ? item.TimeSlotId : 0,
                            TimeSlotName = request.SubjectIds.Contains(item.SubjectId) ? item.TimeSlotName : "",
                        }).ToList()
                    });
                    querySearchDataNotAssign = querySearchDataNotAssign.Select(innerItem => innerItem.Where(item => request.SubjectIds.Contains(item.SubjectId)).ToList()).ToList();
                }
                if (request.RoomId.Count != 0)
                {
                    querySearchDataAssign = querySearchDataAssign.Select(innerItem => new ResponseTaskByLecturerIsKey()
                    {
                        LecturerId = innerItem.LecturerId,
                        SemesterId = innerItem.SemesterId,
                        LecturerName = innerItem.LecturerName,
                        Total = innerItem.Total,
                        TimeSlotInfos = innerItem.TimeSlotInfos?.Select(item => new TimeSlotInfo()
                        {
                            ClassId = request.RoomId.Contains(item.RoomId) ? item.ClassId : 0,
                            ClassName = request.RoomId.Contains(item.RoomId) ? item.ClassName : "",
                            IsAssign = request.RoomId.Contains(item.RoomId) ? item.IsAssign : 0,
                            PreAssign = request.RoomId.Contains(item.RoomId) && item.PreAssign,
                            RoomId = request.RoomId.Contains(item.RoomId) ? item.RoomId : 0,
                            RoomName = request.RoomId.Contains(item.RoomId) ? item.RoomName : "",
                            Status = request.RoomId.Contains(item.RoomId) ? item.Status : "",
                            SubjectCode = request.RoomId.Contains(item.RoomId) ? item.SubjectCode : "",
                            SubjectId = request.RoomId.Contains(item.RoomId) ? item.SubjectId : 0,
                            SubjectName = request.RoomId.Contains(item.RoomId) ? item.SubjectName : "",
                            TaskId = request.RoomId.Contains(item.RoomId) ? item.TaskId : 0,
                            TimeSlotId = request.RoomId.Contains(item.RoomId) ? item.TimeSlotId : 0,
                            TimeSlotName = request.RoomId.Contains(item.RoomId) ? item.TimeSlotName : "",
                        }).ToList()
                    });
                    querySearchDataNotAssign = querySearchDataNotAssign.Select(innerItem => innerItem.Where(item => request.RoomId.Contains(item.RoomId)).ToList()).ToList();
                }

                return new GenericResult<SearchDTO>(new SearchDTO()
                {
                    DataAssign = querySearchDataAssign.ToList(),
                    DataNotAssign = new TimeSlotInfoResponse()
                    {
                        Total = _unitOfWork.TaskRepository.MappingTaskData().Where(item => item.LecturerId == null).Count(),
                        TimeSlotInfos = querySearchDataNotAssign
                    }

                }
                , true);
            }

            catch (Exception ex)
            {
                return new GenericResult<SearchDTO>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
        #endregion

        #region TimeTableModify

        public ResponseResult TimeTableModify(TaskModifyDTO request)
        {
            try
            {
                // check if request lecturer already have task at the same timeslot, convert two task of lecturer together
                var taskDupplicate = _unitOfWork.TaskRepository.GetAll()
                    .FirstOrDefault(item => item.LecturerId == request.LecturerId && item.TimeSlotId == request.TimeSlotId);
                var taskFind = _unitOfWork.TaskRepository.Find(request.TaskId);
                if (taskDupplicate != null)
                {
                    int tmpId = taskDupplicate.LecturerId ?? 0;
                    taskDupplicate.LecturerId = taskFind.LecturerId;
                    taskFind.Room1Id = request.RoomId;
                    taskFind.LecturerId = tmpId;
                    _unitOfWork.TaskRepository.Update(taskFind);
                    _unitOfWork.TaskRepository.Update(taskDupplicate);
                }
                else
                {
                    taskFind.Room1Id = request.RoomId;
                    taskFind.LecturerId = request.LecturerId;
                    _unitOfWork.TaskRepository.Update(taskFind);
                }
                _unitOfWork.Complete();
                return new ResponseResult();
            }
            catch (Exception ex)
            {
                return new ResponseResult($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        public ResponseResult LockAndUnLockTask(LockAndUnLockTaskDTO request)
        {
            try
            {
                var taskFind = _unitOfWork.TaskRepository.Find(request.TaskId);
                var isPreAssign = taskFind.PreAssign != null && (bool)taskFind.PreAssign;
                taskFind.PreAssign = !isPreAssign;
                taskFind.LecturerId ??= request.LecturerId;

                _unitOfWork.TaskRepository.Update(taskFind);
                _unitOfWork.Complete();
                return new ResponseResult();
            }
            catch (Exception ex)
            {
                return new ResponseResult($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        public ResponseResult UnLockAllTask()
        {
            try
            {
                var tasksFind = _unitOfWork.TaskRepository.GetAll().Where(item => item.PreAssign == true);
                foreach (var task in tasksFind)
                {
                    task.PreAssign = false;
                }

                _unitOfWork.TaskRepository.UpdateRange(tasksFind);
                _unitOfWork.Complete();
                return new ResponseResult();
            }
            catch (Exception ex)
            {
                return new ResponseResult($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
        #endregion

        #region Fetch Scheduler
        public async Task UpdateTaskByExecuteData(List<ExecuteData>? executeData)
        {
            var executeSemesterId = 0;

            if (executeData != null)
            {
                foreach (var data in executeData)
                {
                    var taskAssign = await _unitOfWork.TaskRepository.FindAsync((item) => item.Id == Convert.ToInt32(data.taskId) && item.SemesterId != executeSemesterId);
                    if (taskAssign != null && data.instructorId != null)
                    {
                        taskAssign.LecturerId = Convert.ToInt32(data.instructorId);
                        _unitOfWork.TaskRepository.Update(taskAssign);
                        await _unitOfWork.CompleteAsync();
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
                List<ExecuteData>? executeData = dataFetch?.results;

                await UpdateTaskByExecuteData(executeData);
                return new ResponseResult("Fetch data success", true);
            }
            catch (Exception ex)
            {
                return new ResponseResult($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        #endregion

        #region GetTaskAssigned
        public IEnumerable<QueryDataByLecturerAndTimeSlot> GetTaskResponses()
        {

            var context = _unitOfWork.Context;
            var result = from A in (
                                from l in context.Lecturers
                                from ts in context.TimeSlots
                                select new
                                { LecturerId = l.Id, LecturerName = l.ShortName, TimeSlotId = ts.Id, TimeSlotName = ts.Name }
                            )
                         join B in context.TaskAssigns
                             on new { A.TimeSlotId, A.LecturerId } equals new { TimeSlotId = B.TimeSlotId ?? 0, LecturerId = B.LecturerId ?? 0 } into AB
                         from B in AB.DefaultIfEmpty()
                         join C in context.Classes
                         on B.ClassId equals C.Id into BC
                         from C in BC.DefaultIfEmpty()
                         join D in context.Subjects
                         on B.SubjectId equals D.Id into BD
                         from D in BD.DefaultIfEmpty()
                         join E in context.Rooms
                         on B.Room1Id equals E.Id into BE
                         from E in BE.DefaultIfEmpty()
                         select new QueryDataByLecturerAndTimeSlot()
                         {
                             TaskId = B.Id,
                             LecturerId = A.LecturerId,
                             LecturerName = A.LecturerName,
                             TimeSlotId = A.TimeSlotId,
                             TimeSlotName = A.TimeSlotName,
                             ClassId = C.Id,
                             ClassName = C.Name,
                             SubjectId = D.Id,
                             SubjectCode = D.Code,
                             SubjectName = D.Name,
                             RoomId = E.Id,
                             Status = (bool)B.Status ? "" : "",
                             SemesterId = B.SemesterId ?? 0,
                             RoomName = E.Name ?? "",
                             IsAssign = (B.Id == null) ? 0 : 1,
                             PreAssign = (bool)B.PreAssign ? true : false
                         };
            return result;

        }
        public List<ResponseTaskByLecturerIsKey> GetTaskResponseByLecturerKey()
        {

            var data = GetTaskResponses().OrderBy(item => item.LecturerId).GroupBy(item => item.LecturerId);
            var result = data.Select(group =>
                new ResponseTaskByLecturerIsKey
                {
                    LecturerId = group.Key,
                    SemesterId = group.First().SemesterId ?? 0,
                    LecturerName = group.First().LecturerName,
                    TimeSlotInfos = group.OrderBy(item => item.TimeSlotId).Select(data =>
                        new TimeSlotInfo
                        {
                            TaskId = data.TaskId ?? 0,
                            TimeSlotId = data.TimeSlotId,
                            TimeSlotName = data.TimeSlotName,
                            ClassId = data.ClassId ?? 0,
                            ClassName = data.ClassName,
                            SubjectId = data.SubjectId ?? 0,
                            SubjectCode = data.SubjectCode,
                            SubjectName = data.SubjectName,
                            RoomId = data.RoomId ?? 0,
                            RoomName = data.RoomName,
                            Status = data.Status,
                            IsAssign = data.IsAssign ?? 0,
                            PreAssign = (bool)data.PreAssign ? true : false
                        }).ToList(),
                    Total = group.Where(item => item.IsAssign != 0).Count(),
                }).ToList();
            return result;

        }

        public GenericResult<List<ResponseTaskByLecturerIsKey>> GetTaskAssigned()
        {
            try
            {
                var data = GetTaskResponseByLecturerKey();
                return new GenericResult<List<ResponseTaskByLecturerIsKey>>(data, true);
            }
            catch (Exception ex)
            {
                return new GenericResult<List<ResponseTaskByLecturerIsKey>>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
        #endregion

        #region GetTasksNotAssigned
        public List<List<TimeSlotInfo>> GetTasksNotAssign()
        {
            var data = _unitOfWork.TaskRepository.MappingTaskData()
                    .Where(item => item.LecturerId == null)
                    .OrderBy(item => item.TimeSlot?.Id)
                    .GroupBy(item => item.TimeSlotId);
            var result = data.Select(group => group.Select(data =>
                        new TimeSlotInfo
                        {
                            TaskId = data.Id,
                            TimeSlotId = data.TimeSlotId ?? 0,
                            TimeSlotName = data.TimeSlot?.Name ?? "",
                            ClassId = data.ClassId ?? 0,
                            ClassName = data.Class?.Name ?? "",
                            SubjectId = data.SubjectId ?? 0,
                            SubjectCode = data.Subject?.Code ?? "",
                            SubjectName = data.Subject?.Name ?? "",
                            RoomId = data.Room1Id ?? 0,
                            RoomName = data.Room1?.Name ?? "",
                            Status = data.Status != null ? "" : "",
                        }).ToList()).ToList();
            return result;
        }

        public List<TimeSlotInfo> GetTasksNotAssignForDetail()
        {
            var data = _unitOfWork.TaskRepository.MappingTaskData()
                    .Where(item => item.LecturerId == null);
            var result = data.Select(data =>
                        new TimeSlotInfo
                        {
                            TaskId = data.Id,
                            TimeSlotId = data.TimeSlotId ?? 0,
                            TimeSlotName = data.TimeSlot?.Name ?? "",
                            ClassId = data.ClassId ?? 0,
                            ClassName = data.Class?.Name ?? "",
                            SubjectId = data.SubjectId ?? 0,
                            SubjectCode = data.Subject?.Code ?? "",
                            SubjectName = data.Subject?.Name ?? "",
                            RoomId = data.Room1Id ?? 0,
                            RoomName = data.Room1?.Name ?? "",
                            Status = data.Status != null ? "" : "",
                        }).ToList();
            return result;
        }

        public GenericResult<TimeSlotInfoResponse> GetAllTaskNotAssign()
        {
            try
            {
                var result = GetTasksNotAssign();

                var total = new TimeSlotInfoResponse()
                {
                    Total = _unitOfWork.TaskRepository.MappingTaskData()
                    .Where(item => item.LecturerId == null).Count(),
                    TimeSlotInfos = result
                };
                return new GenericResult<TimeSlotInfoResponse>(total, true);
            }
            catch (Exception ex)
            {
                return new GenericResult<TimeSlotInfoResponse>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
        #endregion

        #region Execute

        public void SaveExecuteId(ExecuteResponse result)
        {

            var executeInfo = new ExecuteInfo()
            {
                ExecuteId = result.sessionId,
                ExecuteTime = DateTime.Now

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
            List<int> InstructorQuota = GetInstructorQuota();
            List<int> InstructorMinQuota = GetInstructorMinQuota();
            List<int?> PatternCost = GetPatternCost();
            List<List<int>>? SlotConflict = GetSlotConflict();
            List<List<int?>>? AreaSlotCoefficient = GetAreaSlotCoefficient();
            List<List<int?>>? InstructorSubject = GetInstructorSubject();
            List<List<int?>>? InstructorSlot = GetInstructorSlot();
            List<List<int?>>? AreaDistance = GetAreaDistance();
            List<List<int>>? Slotday = GetSlotDays();
            List<List<int>>? SlotTimes = GetSlotTimes();

            List<Building> Buildings = _unitOfWork.BuildingRepository.GetAll().ToList();
            List<SlotFetchRequest> Slots = GetSlotFetchRequest();
            List<DayOfWeekFetchRequest> DayOfWeeks = GetDayOfWeekFetchRequest();
            List<SubjectFetchRequest> Subjects = GetSubjectFetchRequest();
            List<InstructorFetchRequest> Instructors = GetInstructorFetchRequest();
            List<TaskFetchRequest> Tasks = GetTaskFetchRequest(Slots, Subjects);
            List<List<int>>? SlotSegments = GetSlotSegments(Slots, DayOfWeeks);
            List<TaskPreAssignFetchRequest> PreAssign = GetTaskPreAssignFetchRequest(Instructors, Tasks);

            var slotPerDay = _unitOfWork.NumSegmentsRepository.GetAll().FirstOrDefault();
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
        private List<List<int>> GetSlotTimes()
        {
            List<List<int>> slotTimes = new();
            List<TimeSlot> timeSlots = _unitOfWork.TimeSlotRepository.GetAll().OrderBy(item => item.Id).ToList();
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
        private List<List<int>> GetSlotSegments(List<SlotFetchRequest> Slots, List<DayOfWeekFetchRequest> dayOfWeeks)
        {
            List<List<int>> slotSegments = new();
            List<TimeSlotSegment> timeSlotSegments = _unitOfWork.TimeSlotSegmentRepository.GetAll().OrderBy(item => item.Id).ToList();

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
        private List<List<int>> GetSlotDays()
        {
            List<List<int>> slotDays =
                _unitOfWork.TimeSlotSegmentRepository.GetAll().Where(item => item.SlotId != null)
                .OrderBy(item => item.SlotId).GroupBy(item => item.SlotId)
                .Select(gr => gr.Select(item =>
                    item.Segment != 0 ? 1 : 0
                ).ToList()).ToList();
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
        private List<int?> GetPatternCost()
        {
            var slotPerDay = _unitOfWork.NumSegmentsRepository.GetAll().FirstOrDefault();
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
        private List<TaskFetchRequest> GetTaskFetchRequest(List<SlotFetchRequest> Slots, List<SubjectFetchRequest> Subjects)
        {
            List<TaskFetchRequest>? Tasks = new();

            int count = 0;
            List<TaskAssign> taskAssigns = _unitOfWork.TaskRepository.GetAll().OrderBy(item => item.Id).ToList();
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
        private List<TaskPreAssignFetchRequest> GetTaskPreAssignFetchRequest(List<InstructorFetchRequest> Instructors, List<TaskFetchRequest> Tasks)
        {
            List<TaskPreAssignFetchRequest>? PreAssign = _unitOfWork.TaskRepository.GetAll()
                .Where(item => item.LecturerId != null && (item.PreAssign ?? false))
                .Select(item => new TaskPreAssignFetchRequest()
                {
                    InstructorOrder = Instructors.Where(i => Convert.ToInt32(i.Id) == item.LecturerId).Select(item => item.Order).FirstOrDefault(),
                    TaskOrder = Tasks.Where(t => Convert.ToInt32(t.Id) == item.Id).Select(item => item.Order).FirstOrDefault(),
                    Type = 1
                }).ToList();
            return PreAssign;
        }
        #endregion

        #region GetSubjectFetchRequest
        private List<SubjectFetchRequest> GetSubjectFetchRequest()
        {
            List<SubjectFetchRequest>? Subjects = new();

            int count = 0;
            List<Subject> subjects = _unitOfWork.SubjectRepository.GetAll().OrderBy(item => item.Id).ToList();
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
        private List<InstructorFetchRequest> GetInstructorFetchRequest()
        {
            List<InstructorFetchRequest>? Instructors = new();

            int count = 0;
            List<Lecturer> lecturers = _unitOfWork.LecturerRepository.GetAll().OrderBy(item => item.Id).ToList();
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
        private List<SlotFetchRequest> GetSlotFetchRequest()
        {
            List<SlotFetchRequest>? Slots = new();
            int count = 0;
            List<TimeSlot> timeSlots = _unitOfWork.TimeSlotRepository.GetAll().OrderBy(item => item.Id).ToList();
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
        private List<List<int?>> GetInstructorSlot()
        {
            return _unitOfWork.SlotPreferenceLevelRepository.GetAll()
                            .OrderBy(item => item.LecturerId)
                            .GroupBy(item => item.LecturerId)
                            .Select(item => item.Select(item => item.PreferenceLevel)
                            .ToList()).ToList();
        }
        #endregion

        #region GetInstructorSubject
        private List<List<int?>> GetInstructorSubject()
        {
            return _unitOfWork.SubjectPreferenceLevelRepository.GetAll()
                            .OrderBy(item => item.LecturerId)
                            .GroupBy(item => item.LecturerId)
                            .Select(item => item.Select(item => item.PreferenceLevel)
                            .ToList()).ToList();
        }
        #endregion

        #region GetInstructorQuota
        private List<int> GetInstructorQuota()
        {
            return _unitOfWork.LecturerRepository.GetAll()
                            .OrderBy(item => item.Id)
                            .Select(item => item.Quota ?? 4)
                            .ToList();
        }
        #endregion

        #region GetInstructorMinQuota

        // Lecturer must be assign atleast number of quota
        private List<int> GetInstructorMinQuota()
        {
            return _unitOfWork.LecturerRepository.GetAll()
                           .OrderBy(item => item.Id)
                           .Select(item => item.MinQuota ?? 0)
                           .ToList();
        }

        #endregion

        #region GetAreaSlotCoefficient
        private List<List<int?>> GetAreaSlotCoefficient()
        {
            return _unitOfWork.AreaSlotWeightRepository.GetAll()
                            .OrderBy(item => item.SlotId)
                            .GroupBy(item => item.SlotId)
                            .Select(item => item.Select(item => item.AreaSlotWeight1)
                            .ToList()).ToList();
        }
        #endregion

        #region GetSlotConflict
        private List<List<int>> GetSlotConflict()
        {
            return _unitOfWork.TimeSlotConflictRepository.GetAll()
                            .OrderBy(item => item.SlotId)
                            .GroupBy(item => item.SlotId)
                            .Select(item => item.Select(item => (bool)item.Conflict ? 1 : 0)
                            .ToList()).ToList();
        }
        #endregion

        #endregion

    }
}
