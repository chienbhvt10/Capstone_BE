using AutoMapper;
using Capstone_API.DTO.Distance;
using Capstone_API.DTO.Distance.Request;
using Capstone_API.DTO.Distance.Response;
using Capstone_API.Models;
using Capstone_API.Results;
using Capstone_API.Service.Interface;
using Capstone_API.UOW_Repositories.UnitOfWork;

namespace Capstone_API.Service.Implement
{
    public class DistanceService : IDistanceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DistanceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public GenericResult<List<RoomResponse>> GetAllRoom()
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
        public GenericResult<List<BuildingResponse>> GetAllBuilding()
        {
            try
            {
                var rooms = _unitOfWork.BuildingRepository.GetAll();
                var roomsViewModel = _mapper.Map<List<BuildingResponse>>(rooms);
                return new GenericResult<List<BuildingResponse>>(roomsViewModel.ToList(), true);
            }
            catch (Exception ex)
            {
                return new GenericResult<List<BuildingResponse>>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
        public GenericResult<List<DistanceResponse>> GetAllDistance()
        {
            try
            {
                var rooms = DistanceByBuilding1IsKey();
                var roomsViewModel = _mapper.Map<List<DistanceResponse>>(rooms);
                return new GenericResult<List<DistanceResponse>>(roomsViewModel.ToList(), true);
            }
            catch (Exception ex)
            {
                return new GenericResult<List<DistanceResponse>>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
        public IEnumerable<DistanceResponse> DistanceByBuilding1IsKey()
        {
            var data = _unitOfWork.DistanceRepository.MappingDistanceData()
                .OrderBy(item => item.Building1Id).GroupBy(item => item.Building1Id);

            var result = data.Select(group =>
                new DistanceResponse
                {
                    BuildingId = group.First().Building1Id ?? 0,
                    SemesterId = group.First().SemesterId ?? 0,
                    BuildingName = group.First().Building1?.ShortName ?? "",
                    BuildingDistances = group.OrderBy(item => item.Building2Id).Select(data =>
                        new BuildingDistance
                        {
                            Id = data.Id,
                            BuildingDistanceId = data.Building2Id ?? 0,
                            DistanceBetween = data.DistanceBetween ?? 0
                        }).ToList(),
                }).ToList();
            return result;
        }

        public ResponseResult UpdateDistance(UpdateDistanceDTO request)
        {
            try
            {
                var distance = _unitOfWork.DistanceRepository.Find(item => item.Id == request.DistanceId);
                distance.DistanceBetween = request.DistanceBetween;
                _unitOfWork.DistanceRepository.Update(distance);
                _unitOfWork.Complete();
                return new ResponseResult("Update successfully", true);
            }
            catch (Exception ex)
            {
                return new ResponseResult($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        // need create distance between
        public ResponseResult CreateBuilding(CreateBuildingDTO request)
        {
            try
            {
                var building = _mapper.Map<Building>(request);
                _unitOfWork.BuildingRepository.Add(building);
                _unitOfWork.Complete();
                return new ResponseResult("Create successfully", true);
            }
            catch (Exception ex)
            {
                return new ResponseResult($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        // update api

        // delete api 
        // need delete distance between
    }
}
