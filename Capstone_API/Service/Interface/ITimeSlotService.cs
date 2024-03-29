﻿using Capstone_API.DTO.CommonRequest;
using Capstone_API.DTO.TimeSlot.Request;
using Capstone_API.DTO.TimeSlot.Response;
using Capstone_API.Results;

namespace Capstone_API.Service.Interface
{
    public interface ITimeSlotService
    {
        GenericResult<List<TimeSlotResponse>> GetAll(GetAllRequest request);
        GenericResult<List<GetSegmentResponseDTO>> GetTimeSlotSegment(GetAllRequest request);
        ResponseResult CreateTimeSlot(CreateTimeSlotDTO request);
        ResponseResult DeleteTimeSlot(int id);
        ResponseResult DeleteTimeSlotSegment(int id);
        ResponseResult UpdateTimeslot(UpdateTimeSlotDTO request);
        GenericResult<TimeSlotSegmentDTO> UpdateTimeslotSegment(TimeSlotSegmentDTO request);
        GenericResult<TimeSlotSegmentDTO> CreateTimeSlotSegment(TimeSlotSegmentDTO request);
        ResponseResult ReUseDataFromASemester(ReUseRequest request);
    }
}
