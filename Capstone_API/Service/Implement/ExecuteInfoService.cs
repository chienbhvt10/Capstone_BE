using AutoMapper;
using Capstone_API.DTO.Task.Fetch;
using Capstone_API.Models;
using Capstone_API.Results;
using Capstone_API.Service.Interface;
using Capstone_API.UOW_Repositories.UnitOfWork;

namespace Capstone_API.Service.Implement
{
    public class ExecuteInfoService : IExecuteInfoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ExecuteInfoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public GenericResult<List<ExecuteInfoResponse>> GetAll()
        {
            try
            {
                var executeInfo = _unitOfWork.ExecuteInfoRepository.GetAll();
                var executeInfoViewModel = _mapper.Map<List<ExecuteInfoResponse>>(executeInfo).ToList();
                return new GenericResult<List<ExecuteInfoResponse>>(executeInfoViewModel, true);
            }
            catch (Exception ex)
            {
                return new GenericResult<List<ExecuteInfoResponse>>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        public ResponseResult CreateExecuteInfo(ExecuteInfoResponse request)
        {
            try
            {
                var executeInfo = _mapper.Map<ExecuteInfo>(request);
                _unitOfWork.ExecuteInfoRepository.Add(executeInfo);
                _unitOfWork.Complete();
                return new ResponseResult("Create successfully", true);
            }
            catch (Exception ex)
            {
                return new ResponseResult($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
    }
}
