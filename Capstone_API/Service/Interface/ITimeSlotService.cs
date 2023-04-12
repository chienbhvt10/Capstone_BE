using Capstone_API.DTO.TimeSlot.Request;
using Capstone_API.DTO.TimeSlot.Response;
using Capstone_API.Results;

namespace Capstone_API.Service.Interface
{
    public interface ITimeSlotService
    {
        GenericResult<List<TimeSlotResponse>> GetAll(int semesterId);
        GenericResult<List<GetSegmentResponseDTO>> GetTimeSlotSegment(int semesterId);
        ResponseResult CreateTimeSlot(CreateTimeSlotDTO request);
        ResponseResult DeleteTimeSlot(int id);
        ResponseResult DeleteTimeSlotSegment(int id);
        ResponseResult UpdateTimeslot(UpdateTimeSlotDTO request);
        GenericResult<TimeSlotSegmentDTO> UpdateTimeslotSegment(TimeSlotSegmentDTO request);
        GenericResult<TimeSlotSegmentDTO> CreateTimeSlotSegment(TimeSlotSegmentDTO request);
    }
}
