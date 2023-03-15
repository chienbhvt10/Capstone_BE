using AutoMapper;
using Capstone_API.DTO.Distance.Response;
using Capstone_API.Results;
using Capstone_API.Service.Interface;
using Capstone_API.UOW_Repositories.UnitOfWork;

namespace Capstone_API.Service.Implement
{
    public class DistanceService : IDistanceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DistanceService(IUnitOfWork unitOfWork, IMapper mapper, HttpClient httpClient)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public GenericResult<List<RoomResponse>> GetAll()
        {
            try
            {
                var rooms = _unitOfWork.RoomRepository.GetAll();
                var roomsViewModel = _mapper.Map<List<RoomResponse>>(rooms);
                return new GenericResult<List<RoomResponse>>(roomsViewModel.ToList(), true);
            }
            catch (Exception ex)
            {
                return new GenericResult<List<RoomResponse>>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
    }
}
