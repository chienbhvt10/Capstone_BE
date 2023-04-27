using AutoMapper;
using Capstone_API.DTO.CommonRequest;
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
        public GenericResult<List<RoomResponse>> GetAllRoom(GetAllRequest request)
        {
            try
            {
                var rooms = _unitOfWork.RoomRepository.GetAll()
                .Where(item =>
                    item.SemesterId == request.SemesterId
                    && item.DepartmentHeadId == request.DepartmentHeadId);
                var roomsViewModel = _mapper.Map<List<RoomResponse>>(rooms);
                return new GenericResult<List<RoomResponse>>(roomsViewModel.ToList(), true);
            }
            catch (Exception ex)
            {
                return new GenericResult<List<RoomResponse>>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
        public GenericResult<List<BuildingResponse>> GetAllBuilding(GetAllRequest request)
        {
            try
            {
                var rooms = _unitOfWork.BuildingRepository
                    .GetAll().Where(item =>
                    item.SemesterId == request.SemesterId
                    && item.DepartmentHeadId == request.DepartmentHeadId);
                var roomsViewModel = _mapper.Map<List<BuildingResponse>>(rooms);
                return new GenericResult<List<BuildingResponse>>(roomsViewModel.ToList(), true);
            }
            catch (Exception ex)
            {
                return new GenericResult<List<BuildingResponse>>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
        public GenericResult<List<DistanceResponse>> GetAllDistance(GetAllRequest request)
        {
            try
            {
                var rooms = DistanceByBuilding1IsKey(request);
                var roomsViewModel = _mapper.Map<List<DistanceResponse>>(rooms);
                return new GenericResult<List<DistanceResponse>>(roomsViewModel.ToList(), true);
            }
            catch (Exception ex)
            {
                return new GenericResult<List<DistanceResponse>>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
        public IEnumerable<DistanceResponse> DistanceByBuilding1IsKey(GetAllRequest request)
        {
            var data = _unitOfWork.DistanceRepository.MappingDistanceData()
                .Where(item => item.SemesterId == request.SemesterId && item.DepartmentHeadId == request.DepartmentHeadId)
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

        public void CreateDistanceBetweenTwoBuilding(Building building, CreateBuildingDTO request)
        {
            List<Distance> buildingDistance = new();
            var currentBuilding = _unitOfWork.BuildingRepository.GetAll()
                .Where(item => item.SemesterId == request.SemesterId && item.DepartmentHeadId == request.DepartmentHeadId);
            foreach (var item in currentBuilding)
            {
                if (item.Id == building.Id)
                {
                    buildingDistance.Add(new Distance()
                    {
                        Building1Id = building.Id,
                        Building2Id = item.Id,
                        DistanceBetween = 0,
                        DepartmentHeadId = building.DepartmentHeadId,
                        SemesterId = building.SemesterId
                    });
                }
                if (item.Id != building.Id)
                {
                    buildingDistance.Add(new Distance()
                    {
                        Building1Id = building.Id,
                        Building2Id = item.Id,
                        DistanceBetween = 0,
                        DepartmentHeadId = building.DepartmentHeadId,
                        SemesterId = building.SemesterId
                    });
                    buildingDistance.Add(new Distance()
                    {
                        Building1Id = item.Id,
                        Building2Id = building.Id,
                        DistanceBetween = 0,
                        DepartmentHeadId = building.DepartmentHeadId,
                        SemesterId = building.SemesterId
                    });
                }
            }
            _unitOfWork.DistanceRepository.AddRange(buildingDistance);
            _unitOfWork.Complete();
        }
        public ResponseResult CreateBuilding(CreateBuildingDTO request)
        {
            try
            {
                var buildingFind = _unitOfWork.BuildingRepository
                    .GetByCondition(item =>
                        item.SemesterId == request.SemesterId
                        && item.DepartmentHeadId == request.DepartmentHeadId
                        && item.ShortName == request.ShortName
                    ).FirstOrDefault();
                if (buildingFind != null)
                {
                    return new ResponseResult("Building with short name already exist");
                }
                var building = _mapper.Map<Building>(request);
                _unitOfWork.BuildingRepository.Add(building);
                _unitOfWork.Complete();
                CreateDistanceBetweenTwoBuilding(building, request);
                return new ResponseResult("Create successfully", true);
            }
            catch (Exception ex)
            {
                return new ResponseResult($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        public ResponseResult UpdateBuilding(UpdateBuildingDTO request)
        {
            try
            {
                var building = _unitOfWork.BuildingRepository.GetById(request.Id);
                if (building == null)
                {
                    return new ResponseResult("Cannot find this building");
                }
                building.Name = request.Name;
                building.ShortName = request.ShortName;
                _unitOfWork.BuildingRepository.Update(building);
                _unitOfWork.Complete();
                return new ResponseResult("Update successfully", true);
            }
            catch (Exception ex)
            {
                return new ResponseResult($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        public ResponseResult DeleteBuilding(int id)
        {
            try
            {
                _unitOfWork.DistanceRepository.DeleteByCondition(item => item.Building1Id == id || item.Building2Id == id, true);

                var building = _unitOfWork.BuildingRepository.Find(id) ?? throw new ArgumentException("Building does not exist");
                _unitOfWork.BuildingRepository.Delete(building, isHardDeleted: true);
                _unitOfWork.Complete();
                return new ResponseResult("Delete successfully", true);
            }
            catch (Exception ex)
            {
                return new ResponseResult($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }


        public void CopyBuildingData(ReUseRequest request)
        {
            var fromBuildingData = _unitOfWork.BuildingRepository
                .GetAll()
                .Where(item =>
                    item.SemesterId == request.FromSemesterId
                    && item.DepartmentHeadId == request.DepartmentHeadId)
                .ToList();
            List<Building> newBuilding = new();
            foreach (var item in fromBuildingData)
            {
                newBuilding.Add(new Building()
                {
                    ShortName = item.ShortName,
                    Name = item.Name,
                    SemesterId = request.ToSemesterId,
                    DepartmentHeadId = request.DepartmentHeadId
                });
            }
            _unitOfWork.BuildingRepository.AddRange(newBuilding);
            _unitOfWork.Complete();
        }

        public void CopyDistanceData(ReUseRequest request)
        {
            var fromDistanceData = _unitOfWork.DistanceRepository
                .GetAll()
                .Where(item =>
                    item.SemesterId == request.FromSemesterId
                    && item.DepartmentHeadId == request.DepartmentHeadId)
                .ToList();
            List<Distance> newDistance = new();
            foreach (var item in fromDistanceData)
            {
                var building1ShortNameInOldSemester = _unitOfWork.BuildingRepository.GetById(item.Building1Id ?? 0)?.Name;
                var building1tInCurrentSemester = _unitOfWork.BuildingRepository
                    .GetByCondition(item =>
                        item.SemesterId == request.ToSemesterId
                        && item.DepartmentHeadId == request.DepartmentHeadId
                        && item.Name == building1ShortNameInOldSemester).FirstOrDefault();

                var building2ShortNameInOldSemester = _unitOfWork.BuildingRepository.GetById(item.Building2Id ?? 0)?.Name;
                var building2tInCurrentSemester = _unitOfWork.BuildingRepository
                    .GetByCondition(item =>
                        item.SemesterId == request.ToSemesterId
                        && item.DepartmentHeadId == request.DepartmentHeadId
                        && item.Name == building2ShortNameInOldSemester).FirstOrDefault();

                newDistance.Add(new Distance()
                {
                    Building1Id = building1tInCurrentSemester?.Id,
                    Building2Id = building2tInCurrentSemester?.Id,
                    DistanceBetween = item.DistanceBetween,
                    SemesterId = request.ToSemesterId,
                    DepartmentHeadId = request.DepartmentHeadId
                });
            }
            _unitOfWork.DistanceRepository.AddRange(newDistance);
            _unitOfWork.Complete();
        }

        public ResponseResult ReUseDataFromASemester(ReUseRequest request)
        {
            try
            {
                CopyBuildingData(request);
                var buildingCurrentSemesterData = _unitOfWork.BuildingRepository
                    .GetByCondition(item =>
                    item.SemesterId == request.ToSemesterId
                    && item.DepartmentHeadId == request.DepartmentHeadId).ToList();
                if (buildingCurrentSemesterData == null)
                {
                    return new ResponseResult("Cannot find building data in current semester");
                }
                CopyDistanceData(request);
                return new ResponseResult("Reuse data successfully", true);
            }
            catch (Exception ex)
            {
                return new ResponseResult($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
    }
}
