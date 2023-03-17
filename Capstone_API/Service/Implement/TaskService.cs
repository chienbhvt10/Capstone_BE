﻿using AutoMapper;
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
                query ??= _mapper.Map<QueryDataByLecturerAndTimeSlot>(GetTasksNotAssign2().Where(item => item.TaskId == taskId).FirstOrDefault());

                return new GenericResult<QueryDataByLecturerAndTimeSlot>(query, true);
            }
            catch (Exception ex)
            {
                return new GenericResult<QueryDataByLecturerAndTimeSlot>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
        #endregion

        #region SearchTask
        public GenericResult<List<QueryDataByLecturerAndTimeSlot>> SearchTask(GetAllTaskAssignRequest request)
        {
            try
            {
                var querySearchData = GetTaskResponses().Where(item =>
                    request.LecturerIds != null && item?.LecturerId != null && request.LecturerIds.Contains((int)item.LecturerId)
                    && request.ClassIds != null && item?.ClassId != null && request.ClassIds.Contains((int)item.ClassId)
                    && request.SubjectIds != null && item?.SubjectId != null && request.SubjectIds.Contains((int)item.SubjectId)
                    && request.RoomId != null && item?.RoomId != null && (request.RoomId.Contains((int)item.RoomId))).ToList();

                return new GenericResult<List<QueryDataByLecturerAndTimeSlot>>(querySearchData, true);
            }

            catch (Exception ex)
            {
                return new GenericResult<List<QueryDataByLecturerAndTimeSlot>>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
        #endregion

        #region RequestLecturerConfirm
        public ResponseResult RequestLecturerConfirm()
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
        #endregion

        #region SwapLecturer
        public ResponseResult SwapLecturer(SwapLecturerRequest request)
        {
            try
            {
                TaskAssign taskAssign = _mapper.Map<TaskAssign>(request);
                _unitOfWork.TaskRepository.Update(taskAssign);
                _unitOfWork.Complete();
                return new ResponseResult("Swap lecturer successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
        #endregion

        #region SwapRoom
        public ResponseResult SwapRoom(SwapRoomRequest request)
        {
            try
            {
                TaskAssign taskAssign = _mapper.Map<TaskAssign>(request);
                _unitOfWork.TaskRepository.Update(taskAssign);
                _unitOfWork.Complete();
                return new ResponseResult("Swap room successfully");

            }
            catch (Exception ex)
            {
                return new ResponseResult($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
        #endregion

        #region TimeTableModify

        public ResponseResult TimeTableModify(TaskModifyRequest request)
        {
            try
            {
                var taskDupplicate = _unitOfWork.TaskRepository.GetAll()
                    .FirstOrDefault(item => item.LecturerId == request.LecturerId && item.TimeSlotId == request.TimeSlotId);
                var taskFind = _unitOfWork.TaskRepository.Find(request.TaskId);
                if (taskDupplicate != null)
                {
                    int tmpId = (int)taskDupplicate.LecturerId;
                    taskDupplicate.LecturerId = taskFind.LecturerId;
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

        public ResponseResult LockAndUnLockTask(LockAndUnLockTaskRequest request)
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

        #region GetSchedule
        public async Task<GenericResult<List<ResponseTaskByLecturerIsKey>>> GetSchedule(int executeId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"http://localhost:3001/{executeId}");
                var json = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new GenericResult<List<ResponseTaskByLecturerIsKey>>("Fetch fail");
                }
                var result = JsonSerializer.Deserialize<FetchDataByExcecuteIdResponse>(json);

                var dataFetch = result?.data;
                ExecuteData[]? executeData = dataFetch?.data;

                // save executeId and time of semester by executeSemesterId
                // and get executeSemesterId in database
                // this code must be get executeSemesterId in database
                var executeSemesterId = 0;

                if (executeData != null)
                {
                    foreach (var data in executeData)
                    {
                        var taskAssign = await _unitOfWork.TaskRepository.FindAsync((item) => item.Id == data.taskID && item.SemesterId != executeSemesterId);
                        if (taskAssign != null)
                        {
                            taskAssign.LecturerId = data.instructorID;
                            _unitOfWork.TaskRepository.Update(taskAssign);
                            await _unitOfWork.CompleteAsync();
                        }
                    }
                }
                var mappingData = ResponseTaskByLecturerIsKey().ToList();
                return new GenericResult<List<ResponseTaskByLecturerIsKey>>(mappingData, true);
            }
            catch (Exception ex)
            {
                return new GenericResult<List<ResponseTaskByLecturerIsKey>>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
        #endregion

        #region GetTaskAssigned
        public IEnumerable<QueryDataByLecturerAndTimeSlot> GetTaskResponses()
        {
            try
            {
                var context = _unitOfWork.Context;
                var result = from A in (
                                    from l in context.Lecturers
                                    from ts in context.TimeSlots
                                    select new
                                    { LecturerId = l.Id, LecturerName = l.Name, TimeSlotId = ts.Id, TimeSlotName = ts.Name, TimeSlotOrder = ts.OrderNumber }
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
                                 TimeSlotOrder = A.TimeSlotOrder,
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
            catch (Exception ex)
            {
                return null;

            }
        }
        public IEnumerable<ResponseTaskByLecturerIsKey> ResponseTaskByLecturerIsKey()
        {
            var data = GetTaskResponses().OrderBy(item => item.LecturerId).GroupBy(item => item.LecturerId);
            var result = data.Select(group =>
                new ResponseTaskByLecturerIsKey
                {
                    LecturerId = group.Key,
                    SemesterId = group.First().SemesterId ?? 0,
                    LecturerName = group.First().LecturerName,
                    TimeSlotInfos = group.OrderBy(item => item.TimeSlotOrder).Select(data =>
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
        #endregion

        #region GetTasksNotAssigned
        public List<List<TimeSlotInfo>> GetTasksNotAssign()
        {
            var data = _unitOfWork.TaskRepository.MappingTaskData()
                    .Where(item => item.LecturerId == null)
                    .OrderBy(item => item.TimeSlot?.OrderNumber)
                    .GroupBy(item => item.TimeSlotId);
            var result = data.Select(group => group.Select(data =>
                        new TimeSlotInfo
                        {
                            TaskId = data.Id,
                            TimeSlotId = (int)data.TimeSlotId,
                            TimeSlotName = data.TimeSlot.Name,
                            ClassId = (int)data.ClassId,
                            ClassName = data.Class.Name,
                            SubjectId = (int)data.SubjectId,
                            SubjectCode = data.Subject.Code,
                            SubjectName = data.Subject.Name,
                            RoomId = (int)data.Room1Id,
                            RoomName = data.Room1.Name,
                            Status = data.Status != null ? "" : "",
                        }).ToList()).ToList();
            return result;
        }

        public List<TimeSlotInfo> GetTasksNotAssign2()
        {
            var data = _unitOfWork.TaskRepository.MappingTaskData()
                    .Where(item => item.LecturerId == null);
            var result = data.Select(data =>
                        new TimeSlotInfo
                        {
                            TaskId = data.Id,
                            TimeSlotId = (int)data.TimeSlotId,
                            TimeSlotName = data.TimeSlot.Name,
                            ClassId = (int)data.ClassId,
                            ClassName = data.Class.Name,
                            SubjectId = (int)data.SubjectId,
                            SubjectCode = data.Subject.Code,
                            SubjectName = data.Subject.Name,
                            RoomId = (int)data.Room1Id,
                            RoomName = data.Room1.Name,
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
        public async Task<GenericResult<int>> Execute(SettingRequest request)
        {
            try
            {

                ExecuteFetchRequest executeFetchRequest = GetExecuteFetchRequest(request);

                var jsonRequest = JsonSerializer.Serialize(executeFetchRequest);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"https://localhost:7062/ATTASAPI/excecute", content);

                var json = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new GenericResult<int>("Fetch fail");
                }
                var result = JsonSerializer.Deserialize<FetchExcecuteResponse>(json);
                //var result = JsonSerializer.Deserialize<int>(json);

                var dataFetch = result?.data ?? 0;
                return new GenericResult<int>(dataFetch, true);
            }
            catch (Exception ex)
            {
                return new GenericResult<int>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        #region GetExecuteFetchRequest
        public ExecuteFetchRequest GetExecuteFetchRequest(SettingRequest request)
        {
            List<List<int>>? SlotConflict = GetSlotConflict();
            List<List<int?>>? SlotCompatibility = GetSlotCompatibility();
            List<List<int?>>? AreaSlotCoefficient = GetAreaSlotCoefficient();
            List<List<int?>>? InstructorSubject = GetInstructorSubject();
            List<List<int?>>? InstructorSlot = GetInstructorSlot();
            List<List<int?>>? AreaDistance = GetAreaDistance();

            List<Building> Buildings = _unitOfWork.BuildingRepository.GetAll().ToList();
            List<int?> InstructorQuota = GetInstructorQuota();
            List<SlotFetchRequest> Slots = GetSlotFetchRequest();
            List<SubjectFetchRequest> Subjects = GetSubjectFetchRequest();
            List<InstructorFetchRequest> Instructors = GetInstructorFetchRequest();
            List<TaskFetchRequest> Tasks = GetTaskFetchRequest(Slots, Subjects);
            List<TaskPreAssignFetchRequest> PreAssign = GetTaskPreAssignFetchRequest(Instructors, Tasks);

            ExecuteFetchRequest executeFetchRequest = new()
            {
                BackupInstructor = 0,
                NumAreas = Buildings.Count(),
                NumInstructors = Instructors.Count(),
                NumSlots = Slots.Count(),
                NumSubjects = Subjects.Count(),
                NumTasks = Tasks.Count(),
                Setting = request,
                SlotConflict = SlotConflict,
                SlotCompability = SlotCompatibility,
                AreaSlotCoefficient = AreaSlotCoefficient,
                InstructorSubject = InstructorSubject,
                InstructorSlot = InstructorSlot,
                AreaDistance = AreaDistance,
                InstructorQuota = InstructorQuota,
                Slots = Slots,
                Instructors = Instructors,
                Tasks = Tasks,
                PreAssign = PreAssign
            };
            return executeFetchRequest;
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
        private List<int?> GetInstructorQuota()
        {
            return _unitOfWork.LecturerQuotaRepository.GetAll()
                            .OrderBy(item => item.LecturerId)
                            .Select(item => item.Quota)
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

        #region GetSlotCompatibility
        private List<List<int?>> GetSlotCompatibility()
        {
            return _unitOfWork.TimeSlotCompatibilityRepository.GetAll()
                            .OrderBy(item => item.SlotId)
                            .GroupBy(item => item.SlotId)
                            .Select(item => item.Select(item => item.CompatibilityLevel)
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
