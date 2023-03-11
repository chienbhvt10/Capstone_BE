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

        public GenericResult<List<ExecuteResponse>> GetAll(GetAllTaskAssignRequest request)
        {
            try
            {
                var querySearchData = _unitOfWork.TaskRepository.MappingTaskData().Where(item =>
                    request.LecturerIds != null && item.LecturerId != null && request.LecturerIds.Contains((int)item.LecturerId)
                    && request.ClassIds != null && item.ClassId != null && request.ClassIds.Contains((int)item.ClassId)
                    && request.SubjectIds != null && item.SubjectId != null && request.SubjectIds.Contains((int)item.SubjectId)
                    && request.RoomId != null && item.Room1Id != null && item.Room2Id != null && (request.RoomId.Contains((int)item.Room1Id) || request.RoomId.Contains((int)item.Room2Id)));
                var mappingData = MappingAllTaskToResponseData(querySearchData);

                return new GenericResult<List<ExecuteResponse>>(mappingData, true);
            }

            catch (Exception ex)
            {
                return new GenericResult<List<ExecuteResponse>>($"{ex.Message}: {ex.InnerException?.Message}");
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

        public async Task<GenericResult<List<ExecuteResponse>>> GetSchedule(int executeId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"http://localhost:3001/{executeId}");
                var json = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new GenericResult<List<ExecuteResponse>>("Fetch fail");
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
                        var taskAssign = await _unitOfWork.TaskRepository.FindAsync((item) => item.Id == data.taskID && item.SemesterId == executeSemesterId);
                        if (taskAssign != null)
                        {
                            taskAssign.LecturerId = data.instructorID;
                            _unitOfWork.TaskRepository.Update(taskAssign);
                            await _unitOfWork.CompleteAsync();
                        }
                    }
                }
                var mappingData = MappingAllTaskToResponseData(_unitOfWork.TaskRepository.MappingTaskData()).Where(item => item.LecturerId != 0).ToList();

                return new GenericResult<List<ExecuteResponse>>(mappingData, true);
            }
            catch (Exception ex)
            {
                return new GenericResult<List<ExecuteResponse>>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
        public List<ExecuteResponse> MappingAllTaskToResponseData(IEnumerable<TaskAssign> taskAssigns)
        {
            return taskAssigns.Select(item => new ExecuteResponse()
            {
                LecturerId = item.Lecturer != null ? item.Lecturer.Id : 0,
                LecturerName = item.Lecturer != null ? item.Lecturer.ShortName : "",
                Tasks = item.Lecturer != null ? item.Lecturer.TaskAssigns.Select(taskAssign => new TaskOfLecturer()
                {
                    TaskId = taskAssign.Id,
                    ClassOfTask = new ClassOfTask()
                    {
                        ClassId = taskAssign.Class != null ? taskAssign.Class.Id : 0,
                        ClassName = taskAssign.Class != null ? taskAssign.Class.Name : ""
                    },
                    TimeSlotOfTask = new TimeSlotOfTask()
                    {
                        TimeSlotCode = taskAssign.TimeSlot != null ? taskAssign.TimeSlot.Name : "",
                        TimeSlotId = taskAssign.TimeSlot != null ? taskAssign.TimeSlot.Id : 0,
                        TimeSlotOrder = (taskAssign.TimeSlot != null ? (int)taskAssign.TimeSlot.OrderNumber : 0)
                    },
                    RoomOfTask = null,
                    Subject = new SubjectOfTask()
                    {
                        SubjectId = taskAssign.Subject != null ? taskAssign.Subject.Id : 0,
                        SubjectName = taskAssign.Subject != null ? taskAssign.Subject.Name : null
                    },
                }).ToList() : null
            }).ToList();
        }

    }
}
