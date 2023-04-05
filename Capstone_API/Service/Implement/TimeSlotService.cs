﻿using AutoMapper;
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

        public List<GetSegmentDTO> ResponseTimeSlotSegment()
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
                                 DayId = d.Id,
                                 Day = d.Name
                             } into A
                             join B in (
                                 from segment in context.TimeSlotSegments
                                 select new
                                 {
                                     SegmentId = segment.Id,
                                     TimeSlotId = segment.SlotId,
                                     DayId = segment.DayOfWeek,
                                     segment.Segment
                                 }
                             ) on new { A.TimeSlotId, A.DayId } equals new { TimeSlotId = B.TimeSlotId ?? 0, DayId = B.DayId ?? 0 } into AB
                             from B in AB.DefaultIfEmpty()
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

        public GenericResult<List<GetSegmentResponseDTO>> GetTimeSlotSegment()
        {
            try
            {
                var data = ResponseTimeSlotSegment().OrderBy(item => item.TimeSlotId).GroupBy(item => item.TimeSlotId);
                var result = data.Select(group => new GetSegmentResponseDTO()
                {
                    TimeSlotId = group.Key,
                    TimeSlotName = group.First().TimeSlotName,
                    AmorPm = group.First().AmorPm ?? 0,
                    SemesterId = group.First().SemesterId ?? 0,
                    SlotSegments = group.OrderBy(item => item.DayId).Select(item => new SlotSegment()
                    {
                        SlotId = item.TimeSlotId,
                        Day = item.Day,
                        DayId = item.DayId ?? 0,
                        SegmentId = item.SegmentId ?? 0,
                        Segment = item.Segment ?? 0,
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
                if (request.SegmentId != 0 && _unitOfWork.TimeSlotSegmentRepository?.GetByCondition(item => item.SlotId == request.SlotId).Count() > 0)
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

        public GenericResult<IEnumerable<TimeSlotResponse>> GetAll()
        {
            try
            {
                var timeSlot = _unitOfWork.TimeSlotRepository.GetAll();
                var timeSlotsViewModel = _mapper.Map<IEnumerable<TimeSlotResponse>>(timeSlot);
                return new GenericResult<IEnumerable<TimeSlotResponse>>(timeSlotsViewModel, true);
            }
            catch (Exception ex)
            {
                return new GenericResult<IEnumerable<TimeSlotResponse>>($"{ex.Message}: {ex.InnerException?.Message}");
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
                };
                _unitOfWork.TimeSlotRepository.Add(timeSlot);
                _unitOfWork.Complete();

                if (request.Segments?.Count > 0)
                {
                    foreach (var item in request.Segments)
                    {
                        var segment = new TimeSlotSegment()
                        {
                            SlotId = timeSlot.Id,
                            DayOfWeek = item.Day,
                            Segment = item.Segment
                        };
                        _unitOfWork.TimeSlotSegmentRepository.Add(segment);
                        _unitOfWork.Complete();
                    }
                }
                return new ResponseResult();
            }
            catch (Exception ex)
            {
                return new ResponseResult($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        public ResponseResult DeleteTimeSlot(int id)
        {
            try
            {
                var timeSlot = _unitOfWork.TimeSlotRepository.Find(id) ?? throw new ArgumentException("Timeslot does not exist");
                _unitOfWork.TimeSlotSegmentRepository.DeleteByCondition(item => item.SlotId != id, true);
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

        #endregion
    }
}
