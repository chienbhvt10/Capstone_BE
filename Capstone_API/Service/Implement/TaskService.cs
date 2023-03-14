using AutoMapper;
using Capstone_API.DTO.Task.Fetch;
using Capstone_API.DTO.Task.Request;
using Capstone_API.DTO.Task.Response;
using Capstone_API.Models;
using Capstone_API.Results;
using Capstone_API.Service.Interface;
using Capstone_API.UOW_Repositories.UnitOfWork;
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


        public ResponseResult TimeTableModify(TaskModifyRequest request)
        {
            try
            {
                TaskAssign taskAssign = _mapper.Map<TaskAssign>(request);
                _unitOfWork.TaskRepository.Update(taskAssign);
                _unitOfWork.Complete();
                return new ResponseResult("Modify task successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

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
                                 SubjectName = D.Name,
                                 RoomId = E.Id,
                                 Status = (bool)B.Status ? "" : "",
                                 SemesterId = B.SemesterId ?? 0,
                                 RoomName = E.Name ?? "",
                                 IsAssign = (B.Id == null) ? 0 : 1,
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
            var data = GetTaskResponses().GroupBy(item => item.LecturerId);
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
                            SubjectName = data.SubjectName,
                            RoomId = data.RoomId ?? 0,
                            RoomName = data.RoomName,
                            Status = data.Status,
                            IsAssign = data.IsAssign ?? 0
                        }).ToList(),
                    Total = group.Where(item => item.IsAssign != 0).Count(),
                }).ToList();
            return result;
        }

        public GenericResult<TimeSlotInfoResponse> GetAllTaskNotAssign()
        {
            try
            {
                var data = _unitOfWork.TaskRepository.MappingTaskData()
                    .Where(item => item.LecturerId == null)
                    .GroupBy(item => item.TimeSlotId);
                var result = data.Select(group => group.OrderBy(item => item?.TimeSlot?.OrderNumber).Select(data =>
                            new TimeSlotInfo
                            {
                                TaskId = data.Id,
                                TimeSlotId = (int)data.TimeSlotId,
                                TimeSlotName = data.TimeSlot.Name,
                                ClassId = (int)data.ClassId,
                                ClassName = data.Class.Name,
                                SubjectId = (int)data.SubjectId,
                                SubjectName = data.Subject.Name,
                                RoomId = (int)data.Room1Id,
                                RoomName = data.Room1.Name,
                                Status = data.Status != null ? "" : "",
                            }).ToList()).ToList();

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

        public GenericResult<QueryDataByLecturerAndTimeSlot> GetATask(int taskId)
        {
            try
            {
                var result = GetTaskResponses().Where(item => item.TaskId == taskId).FirstOrDefault();
                return new GenericResult<QueryDataByLecturerAndTimeSlot>(result, true);
            }
            catch (Exception ex)
            {
                return new GenericResult<QueryDataByLecturerAndTimeSlot>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }


    }
}
