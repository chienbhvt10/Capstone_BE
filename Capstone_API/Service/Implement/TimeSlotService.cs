using AutoMapper;
using Capstone_API.DTO.CommonRequest;
using Capstone_API.DTO.TimeSlot.Request;
using Capstone_API.DTO.TimeSlot.Response;
using Capstone_API.Models;
using Capstone_API.Results;
using Capstone_API.Service.Interface;
using Capstone_API.UOW_Repositories.UnitOfWork;

namespace Capstone_API.Service.Implement
{
    public class TimeSlotService : ITimeSlotService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TimeSlotService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }



        #region TimeSlot Segment 

        public List<GetSegmentDTO> ResponseTimeSlotSegment(GetAllRequest request)
        {
            try
            {
                var context = _unitOfWork.Context;
                var result = from t in context.TimeSlots
                             from d in context.DayOfWeeks
                             select new
                             {
                                 TimeSlotId = t.Id,
                                 t.AmorPm,
                                 t.Name,
                                 t.SemesterId,
                                 t.DepartmentHeadId,
                                 DayId = d.Id,
                                 Day = d.Name,

                             } into A
                             join B in (
                                 from segment in context.TimeSlotSegments
                                 select new
                                 {
                                     SegmentId = segment.Id,
                                     TimeSlotId = segment.SlotId,
                                     DayId = segment.DayOfWeek,
                                     segment.Segment,
                                 }
                             ) on new { A.TimeSlotId, A.DayId } equals new { TimeSlotId = B.TimeSlotId ?? 0, DayId = B.DayId ?? 0 } into AB
                             from B in AB.DefaultIfEmpty()
                             where A.DepartmentHeadId == request.SemesterId && A.DepartmentHeadId == request.DepartmentHeadId
                             select new GetSegmentDTO
                             {
                                 TimeSlotId = A.TimeSlotId,
                                 AmorPm = A.AmorPm ?? 0,
                                 TimeSlotName = A.Name ?? "",
                                 SemesterId = A.SemesterId ?? 0,
                                 DayId = A.DayId,
                                 Day = A.Day ?? "",
                                 SegmentId = B.SegmentId,
                                 Segment = B.Segment ?? 0
                             };
                return result.ToList();
            }
            catch (Exception ex)
            {
                return null;

            }
        }

        public GenericResult<List<GetSegmentResponseDTO>> GetTimeSlotSegment(GetAllRequest request)
        {
            try
            {
                var data = ResponseTimeSlotSegment(request)
                    .OrderBy(item => item.TimeSlotId)
                    .GroupBy(item => item.TimeSlotId);
                var result = data.Select(group => new GetSegmentResponseDTO()
                {
                    TimeSlotId = group.Key,
                    TimeSlotName = group.First().TimeSlotName,
                    AmorPm = group.First().AmorPm ?? 0,
                    SemesterId = group.First().SemesterId ?? 0,
                    SegmentByDays = group.OrderBy(item => item.DayId).GroupBy(item => item.DayId)
                    .Select(item => new SegmentByDay()
                    {
                        DayId = item.Key ?? 0,
                        SlotSegments = item.Select(item => new SlotSegment()
                        {
                            SlotId = item.TimeSlotId,
                            Day = item.Day,
                            SegmentId = item.SegmentId ?? 0,
                            Segment = item.Segment ?? 0,
                        }).ToList()
                    }).ToList()
                });
                return new GenericResult<List<GetSegmentResponseDTO>>(result.ToList(), true);
            }
            catch (Exception ex)
            {
                return new GenericResult<List<GetSegmentResponseDTO>>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        public ResponseResult DeleteTimeSlotSegment(int id)
        {
            try
            {
                var timeSlot = _unitOfWork.TimeSlotSegmentRepository.Find(id) ?? throw new ArgumentException("Timeslot segment does not exist");
                _unitOfWork.TimeSlotSegmentRepository.Delete(timeSlot, isHardDeleted: true);
                _unitOfWork.Complete();
                return new ResponseResult("Delete successfully", true);
            }
            catch (Exception ex)
            {
                return new ResponseResult($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        public GenericResult<TimeSlotSegmentDTO> CreateTimeSlotSegment(TimeSlotSegmentDTO request)
        {
            try
            {
                if (request.SegmentId != 0 &&
                    _unitOfWork.TimeSlotSegmentRepository?
                    .GetByCondition(item => item.SlotId == request.SlotId).Count() > 0)
                {
                    return UpdateTimeslotSegment(request);
                }
                var timeSlotSegment = _mapper.Map<TimeSlotSegment>(request);
                _unitOfWork.TimeSlotSegmentRepository?.Add(timeSlotSegment);
                _unitOfWork.Complete();
                var timeSlotSegment2 = new TimeSlotSegmentDTO()
                {
                    DayOfWeek = timeSlotSegment.DayOfWeek,
                    Segment = timeSlotSegment.Segment,
                    SegmentId = timeSlotSegment.Id,
                    SemesterId = timeSlotSegment.SemesterId,
                    SlotId = timeSlotSegment.SlotId
                };

                return new GenericResult<TimeSlotSegmentDTO>(timeSlotSegment2, true);
            }
            catch (Exception ex)
            {
                return new GenericResult<TimeSlotSegmentDTO>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        public GenericResult<TimeSlotSegmentDTO> UpdateTimeslotSegment(TimeSlotSegmentDTO request)
        {
            try
            {
                var timeSlotSegment = _unitOfWork.TimeSlotSegmentRepository.GetById(request.SegmentId);
                if (timeSlotSegment != null)
                {
                    timeSlotSegment.Segment = request.Segment;
                    _unitOfWork.TimeSlotSegmentRepository.Update(timeSlotSegment);
                    _unitOfWork.Complete();
                    var timeSlotSegment2 = _mapper.Map<TimeSlotSegmentDTO>(timeSlotSegment);
                    return new GenericResult<TimeSlotSegmentDTO>(timeSlotSegment2, true);
                }
                return new GenericResult<TimeSlotSegmentDTO>("Cannot find timeslot segment");

            }
            catch (Exception ex)
            {
                return new GenericResult<TimeSlotSegmentDTO>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
        #endregion

        #region TimeSlot 

        public GenericResult<List<TimeSlotResponse>> GetAll(GetAllRequest request)
        {
            try
            {
                var timeSlot = _unitOfWork.TimeSlotRepository.GetAll()
                    .Where(item => item.SemesterId == request.SemesterId
                    && item.DepartmentHeadId == request.DepartmentHeadId).ToList();
                var timeSlotsViewModel = _mapper.Map<List<TimeSlotResponse>>(timeSlot);
                return new GenericResult<List<TimeSlotResponse>>(timeSlotsViewModel, true);
            }
            catch (Exception ex)
            {
                return new GenericResult<List<TimeSlotResponse>>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        #region Create Timeslot

        public void CreateAreaSlotWeightForNewTimeSlot(TimeSlot timeSlot, int semesterId)
        {
            List<AreaSlotWeight> newAreaSlotWeights = new();
            foreach (var item in _unitOfWork.TimeSlotRepository.GetAll().Where(item => item.SemesterId == semesterId))
            {
                if (item.Id == timeSlot.Id)
                {
                    newAreaSlotWeights.Add(new AreaSlotWeight()
                    {
                        AreaSlotId = timeSlot.Id,
                        SlotId = item.Id,
                        AreaSlotWeight1 = 0,
                        SemesterId = timeSlot.SemesterId,
                        DepartmentHeadId = timeSlot.DepartmentHeadId
                    });
                }
                if (item.Id != timeSlot.Id)
                {
                    newAreaSlotWeights.Add(new AreaSlotWeight()
                    {
                        AreaSlotId = timeSlot.Id,
                        SlotId = item.Id,
                        AreaSlotWeight1 = 0,
                        SemesterId = timeSlot.SemesterId,
                        DepartmentHeadId = timeSlot.DepartmentHeadId
                    });
                    newAreaSlotWeights.Add(new AreaSlotWeight()
                    {
                        AreaSlotId = item.Id,
                        SlotId = timeSlot.Id,
                        AreaSlotWeight1 = 0,
                        SemesterId = timeSlot.SemesterId,
                        DepartmentHeadId = timeSlot.DepartmentHeadId
                    });
                }
            }
            _unitOfWork.AreaSlotWeightRepository.AddRange(newAreaSlotWeights);
            _unitOfWork.Complete();
        }

        public void CreateTimeSlotConflictForNewTimeSlot(TimeSlot timeSlot, int semesterId)
        {
            List<TimeSlotConflict> timeSlotConflicts = new();
            foreach (var item in _unitOfWork.TimeSlotRepository.GetAll().Where(item => item.SemesterId == semesterId))
            {
                if (item.Id == timeSlot.Id)
                {
                    timeSlotConflicts.Add(new TimeSlotConflict()
                    {
                        SlotId = item.Id,
                        ConflictSlotId = timeSlot.Id,
                        Conflict = true,
                        SemesterId = timeSlot.SemesterId,
                        DepartmentHeadId = timeSlot.DepartmentHeadId
                    });
                }
                if (item.Id != timeSlot.Id)
                {
                    timeSlotConflicts.Add(new TimeSlotConflict()
                    {
                        SlotId = timeSlot.Id,
                        ConflictSlotId = item.Id,
                        Conflict = false,
                        SemesterId = timeSlot.SemesterId,
                        DepartmentHeadId = timeSlot.DepartmentHeadId
                    });
                    timeSlotConflicts.Add(new TimeSlotConflict()
                    {
                        SlotId = item.Id,
                        ConflictSlotId = timeSlot.Id,
                        Conflict = false,
                        SemesterId = timeSlot.SemesterId,
                        DepartmentHeadId = timeSlot.DepartmentHeadId
                    });
                }
            }
            _unitOfWork.TimeSlotConflictRepository.AddRange(timeSlotConflicts);
            _unitOfWork.Complete();
        }

        public void CreateSlotPreferenceForNewTimeSlot(TimeSlot timeSlot, int semesterId)
        {
            List<SlotPreferenceLevel> slotPreferenceLevels = new();
            foreach (var item in _unitOfWork.LecturerRepository.GetAll().Where(item => item.SemesterId == semesterId))
            {
                slotPreferenceLevels.Add(new SlotPreferenceLevel()
                {
                    SlotId = timeSlot.Id,
                    LecturerId = item.Id,
                    PreferenceLevel = 0,
                    SemesterId = timeSlot.SemesterId,
                    DepartmentHeadId = timeSlot.DepartmentHeadId
                });
            }
            _unitOfWork.SlotPreferenceLevelRepository.AddRange(slotPreferenceLevels);
            _unitOfWork.Complete();
        }

        public void CreateTimeSlotSegment(CreateTimeSlotDTO request, TimeSlot timeSlot, int semesterId)
        {
            if (request.Segments?.Count > 0)
            {
                foreach (var item in request.Segments)
                {
                    var segment = new TimeSlotSegment()
                    {
                        SlotId = timeSlot.Id,
                        DayOfWeek = item.Day,
                        Segment = item.Segment,
                        SemesterId = semesterId,
                        DepartmentHeadId = timeSlot.DepartmentHeadId
                    };
                    _unitOfWork.TimeSlotSegmentRepository.Add(segment);
                    _unitOfWork.Complete();
                }
            }
        }

        public ResponseResult CreateTimeSlot(CreateTimeSlotDTO request)
        {
            try
            {
                var timeSlot = new TimeSlot()
                {
                    AmorPm = request.DaySession,
                    Name = request.Name,
                    SemesterId = request.SemesterId,
                    DepartmentHeadId = request.DepartmentHeadId
                };
                _unitOfWork.TimeSlotRepository.Add(timeSlot);
                _unitOfWork.Complete();

                CreateTimeSlotSegment(request, timeSlot, request.SemesterId);
                CreateAreaSlotWeightForNewTimeSlot(timeSlot, request.SemesterId);
                CreateTimeSlotConflictForNewTimeSlot(timeSlot, request.SemesterId);
                CreateSlotPreferenceForNewTimeSlot(timeSlot, request.SemesterId);
                return new ResponseResult();
            }
            catch (Exception ex)
            {
                return new ResponseResult($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        #endregion

        // delete delete areaslotweight, timeslotconflict, timeslot preference level first
        public ResponseResult DeleteTimeSlot(int id)
        {
            try
            {
                _unitOfWork.TimeSlotConflictRepository.DeleteByCondition(item => item.SlotId == id || item.ConflictSlotId == id, true);
                _unitOfWork.AreaSlotWeightRepository.DeleteByCondition(item => item.SlotId == id || item.AreaSlotId == id, true);
                _unitOfWork.SlotPreferenceLevelRepository.DeleteByCondition(item => item.SlotId == id, true);
                _unitOfWork.TimeSlotSegmentRepository.DeleteByCondition(item => item.SlotId != id, true);
                var taskContainThisDeleteTimeSlot = _unitOfWork.TaskRepository.GetAll().Where(item => item.TimeSlotId == id).ToList();

                foreach (var item in taskContainThisDeleteTimeSlot)
                {
                    item.TimeSlotId = 0;
                }

                _unitOfWork.TaskRepository.UpdateRange(taskContainThisDeleteTimeSlot);
                _unitOfWork.Complete();

                var timeSlot = _unitOfWork.TimeSlotRepository.Find(id) ?? throw new ArgumentException("Timeslot does not exist");
                _unitOfWork.TimeSlotRepository.Delete(timeSlot, isHardDeleted: true);
                _unitOfWork.Complete();
                return new ResponseResult("Delete successfully", true);
            }
            catch (Exception ex)
            {
                return new ResponseResult($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        public ResponseResult UpdateTimeslot(UpdateTimeSlotDTO request)
        {
            try
            {
                var timeSlot = _unitOfWork.TimeSlotRepository.GetById(request.Id);
                if (timeSlot != null)
                {
                    timeSlot.AmorPm = request.AmorPm;
                    _unitOfWork.TimeSlotRepository.Update(timeSlot);
                    _unitOfWork.Complete();
                }
                return new ResponseResult("Update successfully", true);
            }
            catch (Exception ex)
            {
                return new ResponseResult($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        /// <summary>
        ///  Dang loi vcl
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        #region CopyData
        public List<TimeSlot> CopyTimeSlotData(ReUseRequest request)
        {
            var fromTimeSlotData = _unitOfWork.TimeSlotRepository.GetAll().Where(item => item.SemesterId == request.FromSemesterId);
            List<TimeSlot> newTimeSlot = new();
            foreach (var item in fromTimeSlotData)
            {
                newTimeSlot.Add(new TimeSlot()
                {
                    Name = item.Name,
                    AmorPm = item.AmorPm,
                    SemesterId = request.ToSemesterId
                });
            }
            _unitOfWork.TimeSlotRepository.AddRange(newTimeSlot);
            _unitOfWork.Complete();
            return newTimeSlot;
        }

        public void CopyTimeSlotConflictData(ReUseRequest request)
        {
            var fromTimeSlotConflictData = _unitOfWork.TimeSlotConflictRepository.GetAll().Where(item => item.SemesterId == request.FromSemesterId);
            List<TimeSlotConflict> newTimeSlotConflict = new();
            foreach (var item in fromTimeSlotConflictData)
            {
                newTimeSlotConflict.Add(new TimeSlotConflict()
                {
                    Conflict = item.Conflict,
                    SlotId = item.SlotId,
                    ConflictSlotId = item.ConflictSlotId,
                    SemesterId = request.ToSemesterId
                });
            }
            _unitOfWork.TimeSlotConflictRepository.AddRange(newTimeSlotConflict);
            _unitOfWork.Complete();
        }

        public void CopyTimeSlotWeightData(ReUseRequest request)
        {
            var fromTimeSlotWeightData = _unitOfWork.AreaSlotWeightRepository.GetAll().Where(item => item.SemesterId == request.FromSemesterId);
            List<AreaSlotWeight> newAreaSlotWeight = new();

            foreach (var item in fromTimeSlotWeightData)
            {
                newAreaSlotWeight.Add(new AreaSlotWeight()
                {
                    AreaSlotId = item.AreaSlotId,
                    SlotId = item.SlotId,
                    AreaSlotWeight1 = item.AreaSlotWeight1,
                    SemesterId = request.ToSemesterId
                });
            }
            _unitOfWork.AreaSlotWeightRepository.AddRange(newAreaSlotWeight);
            _unitOfWork.Complete();
        }

        public void CopyTimeSlotSegmentData(ReUseRequest request)
        {
            var fromTimeSlotSegmentData = _unitOfWork.TimeSlotSegmentRepository.GetAll().Where(item => item.SemesterId == request.FromSemesterId);
            List<TimeSlotSegment> newSlotSegment = new();

            foreach (var item in fromTimeSlotSegmentData)
            {
                newSlotSegment.Add(new TimeSlotSegment()
                {
                    DayOfWeek = item.DayOfWeek,
                    SlotId = item.SlotId,
                    Segment = item.Segment,
                    SemesterId = request.ToSemesterId
                });
            }
            _unitOfWork.TimeSlotSegmentRepository.AddRange(newSlotSegment);
            _unitOfWork.Complete();
        }


        public ResponseResult ReUseDataFromASemester(ReUseRequest request)
        {
            try
            {

                CopyTimeSlotData(request);
                CopyTimeSlotSegmentData(request);
                CopyTimeSlotConflictData(request);
                CopyTimeSlotWeightData(request);
                return new ResponseResult("Reuse data successfully", true);
            }
            catch (Exception ex)
            {
                return new ResponseResult($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
        #endregion
        #endregion
    }
}
