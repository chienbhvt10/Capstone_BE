﻿using AutoMapper;
using Capstone_API.DTO.Task.Request;
using Capstone_API.DTO.Task.Response;
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
                List<List<TimeSlotInfo>> querySearchDataNotAssign = GetTasksNotAssign(request.SemesterId);
                IEnumerable<ResponseTaskByLecturerIsKey> querySearchDataAssign = GetTaskResponseByLecturerKey(request.SemesterId);
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
                var findLecturerSubject = _unitOfWork.SubjectPreferenceLevelRepository.GetAll()
                    .Where(item => item.LecturerId == request.LecturerId && item.SubjectId == request.SubjectId).FirstOrDefault()?.PreferenceLevel;
                if (findLecturerSubject.HasValue && findLecturerSubject.Value == 0)
                {
                    return new ResponseResult("Lecturer cannot assign task with this subject");
                }

                var findLecturerSlot = _unitOfWork.SlotPreferenceLevelRepository.GetAll()
                    .Where(item => item.LecturerId == request.LecturerId && item.SlotId == request.TimeSlotId).FirstOrDefault()?.PreferenceLevel;
                if (findLecturerSlot.HasValue && findLecturerSlot.Value == 0)
                {
                    return new ResponseResult("Lecturer cannot assign task with this timeslot");
                }

                // check if request lecturer already have task at the same timeslot, convert two task of lecturer together
                var taskDupplicateTimeslot = _unitOfWork.TaskRepository.GetAll()
                    .FirstOrDefault(item => item.LecturerId == request.LecturerId && item.TimeSlotId == request.TimeSlotId);
                var taskFind = _unitOfWork.TaskRepository.Find(request.TaskId);

                if (taskDupplicateTimeslot != null)
                {
                    int tmpId = taskDupplicateTimeslot.LecturerId ?? 0;
                    taskDupplicateTimeslot.LecturerId = taskFind.LecturerId;
                    taskDupplicateTimeslot.Room1Id = taskFind.Room1Id;
                    taskFind.Room1Id = request.RoomId;
                    taskFind.LecturerId = tmpId;
                    _unitOfWork.TaskRepository.Update(taskFind);
                    _unitOfWork.TaskRepository.Update(taskDupplicateTimeslot);
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
        public List<ResponseTaskByLecturerIsKey> GetTaskResponseByLecturerKey(int semesterId)
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
                }).OrderBy(item => item.LecturerName).ToList();
            return result;

        }

        public GenericResult<List<ResponseTaskByLecturerIsKey>> GetTaskAssigned(int semesterId)
        {
            try
            {
                var data = GetTaskResponseByLecturerKey(semesterId);
                return new GenericResult<List<ResponseTaskByLecturerIsKey>>(data, true);
            }
            catch (Exception ex)
            {
                return new GenericResult<List<ResponseTaskByLecturerIsKey>>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
        #endregion

        #region GetTasksNotAssigned
        public List<List<TimeSlotInfo>> GetTasksNotAssign(int semesterId)
        {
            var data = _unitOfWork.TaskRepository.MappingTaskData()
                    .Where(item => item.LecturerId == null && item.SemesterId == semesterId)
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

        public GenericResult<TimeSlotInfoResponse> GetAllTaskNotAssign(int semesterId)
        {
            try
            {
                var result = GetTasksNotAssign(semesterId);

                var total = new TimeSlotInfoResponse()
                {
                    Total = _unitOfWork.TaskRepository.MappingTaskData()
                    .Where(item => item.LecturerId == null && item.SemesterId == semesterId).Count(),
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



    }
}
