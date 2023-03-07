using AutoMapper;
using Capstone_API.DTO.Task.Request;
using Capstone_API.DTO.Task.Response;
using Capstone_API.Models;
using Capstone_API.Results;
using Capstone_API.Service.Interface;
using Capstone_API.UOW_Repositories.UnitOfWork;

namespace Capstone_API.Service.Implement
{
    public class TaskService : ITaskService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TaskService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public GenericResult<List<GetAllTaskAssignResponse>> GetAll(GetAllTaskAssignRequest request)
        {
            try
            {
                var data = _unitOfWork.TaskRepository.GetAll().Where(item =>
                    request.LecturerIds != null && item.LecturerId != null && request.LecturerIds.Contains((int)item.LecturerId)
                    && request.ClassIds != null && item.ClassId != null && request.ClassIds.Contains((int)item.ClassId)
                    && request.SubjectIds != null && item.SubjectId != null && request.SubjectIds.Contains((int)item.SubjectId)
                    && request.RoomId != null && item.Room1Id != null && item.Room2Id != null && (request.RoomId.Contains((int)item.Room1Id) || request.RoomId.Contains((int)item.Room2Id)));

                List<GetAllTaskAssignResponse> taskAssigns = _mapper.Map<List<GetAllTaskAssignResponse>>(data);
                return new GenericResult<List<GetAllTaskAssignResponse>>(taskAssigns, true);
            }

            catch (Exception ex)
            {
                return new GenericResult<List<GetAllTaskAssignResponse>>($"{ex.Message}: {ex.InnerException?.Message}");
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


    }
}
