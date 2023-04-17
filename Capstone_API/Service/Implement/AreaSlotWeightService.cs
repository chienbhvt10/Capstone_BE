using AutoMapper;
using Capstone_API.DTO.CommonRequest;
using Capstone_API.DTO.PreferenceLevel.Request;
using Capstone_API.DTO.TimeSlot.Response;
using Capstone_API.Results;
using Capstone_API.Service.Interface;
using Capstone_API.UOW_Repositories.UnitOfWork;

namespace Capstone_API.Service.Implement
{
    public class AreaSlotWeightService : IAreaSlotWeightService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AreaSlotWeightService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public GenericResult<List<GetAreaSlotWeightDTO>> GetAll(GetAllRequest request)
        {
            try
            {
                var currentSemester = _unitOfWork.SemesterInfoRepository
                    .GetAll().FirstOrDefault(item => item.IsNow == true)?.Id ?? 0;

                var query = AreaSlotWeightByTimeSlotIsKey(currentSemester, request.DepartmentHeadId);
                var areaTimeSlotWeightViewModel = _mapper.Map<IEnumerable<GetAreaSlotWeightDTO>>(query).ToList();

                return new GenericResult<List<GetAreaSlotWeightDTO>>(areaTimeSlotWeightViewModel, true);

            }
            catch (Exception ex)
            {
                return new GenericResult<List<GetAreaSlotWeightDTO>>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        public IEnumerable<GetAreaSlotWeightDTO> AreaSlotWeightByTimeSlotIsKey(int semesterId, int departmentHeadId)
        {
            var data = _unitOfWork.AreaSlotWeightRepository.TimeSlotData()
                .Where(item => item.SemesterId == semesterId && item.DepartmentHeadId == departmentHeadId)
                .OrderBy(item => item.SlotId).GroupBy(item => item.SlotId);

            var result = data.Select(group =>
                new GetAreaSlotWeightDTO
                {
                    TimeSlotId = group.First().SlotId ?? 0,
                    SemesterId = group.First().SemesterId ?? 0,
                    TimeSlotName = group.First().Slot?.Name ?? "",
                    AreaSlotWeightInfos = group.OrderBy(item => item.AreaSlotId).Select(data =>
                        new AreaSlotWeightInfo
                        {
                            SlotWeightId = data.Id,
                            SlotWeight = data.AreaSlotWeight1 ?? 0,
                            TimeSlotId = data.SlotId ?? 0
                        }).ToList(),
                }).ToList();
            return result;
        }

        public ResponseResult UpdateAreaTimeSlotWeight(UpdateAreaTimeSlotWeight request)
        {
            try
            {
                var areaSlotWeight = _unitOfWork.AreaSlotWeightRepository.Find(item => item.Id == request.SlotWeightId);
                areaSlotWeight.AreaSlotWeight1 = request.SlotWeight;
                _unitOfWork.AreaSlotWeightRepository.Update(areaSlotWeight);
                _unitOfWork.Complete();
                return new ResponseResult("Update successfully", true);
            }
            catch (Exception ex)
            {
                return new ResponseResult($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
    }
}
