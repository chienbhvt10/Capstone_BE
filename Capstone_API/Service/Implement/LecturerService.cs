using AutoMapper;
using Capstone_API.DTO.Lecturer.Response;
using Capstone_API.Models;
using Capstone_API.Results;
using Capstone_API.Service.Interface;
using Capstone_API.UOW_Repositories.UnitOfWork;

namespace Capstone_API.Service.Implement
{
    public class LecturerService : ILecturerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public LecturerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public GenericResult<IEnumerable<LecturerResponse>> GetAll()
        {
            try
            {
                var lecturers = _unitOfWork.LecturerRepository.GetAll();
                var lecturersViewModel = _mapper.Map<IEnumerable<LecturerResponse>>(lecturers);
                return new GenericResult<IEnumerable<LecturerResponse>>(lecturersViewModel, true);
            }
            catch (Exception ex)
            {
                return new GenericResult<IEnumerable<LecturerResponse>>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        public GenericResult<LecturerResponse> GetOneLecturer(int Id)
        {
            try
            {
                var lecturer = _unitOfWork.LecturerRepository.GetById(Id);
                var lecturerViewModel = _mapper.Map<LecturerResponse>(lecturer);

                return new GenericResult<LecturerResponse>(lecturerViewModel, true);
            }
            catch (Exception ex)
            {
                return new GenericResult<LecturerResponse>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        public ResponseResult CreateLecturer(LecturerResponse request)
        {
            try
            {
                var lecturer = _mapper.Map<Lecturer>(request);
                _unitOfWork.LecturerRepository.Add(lecturer);
                _unitOfWork.Complete();
                return new ResponseResult("Create successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        public ResponseResult UpdateLecturer(LecturerResponse request)
        {
            try
            {
                var lecturer = _mapper.Map<Lecturer>(request);
                _unitOfWork.LecturerRepository.Update(lecturer);
                _unitOfWork.Complete();
                return new ResponseResult("Update successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        // Đang lỗi
        public ResponseResult DeleteLecturer(int id)
        {
            try
            {
                var lecturer = _unitOfWork.LecturerRepository.Find(id);

                if (lecturer == null)
                    throw new ArgumentException("Lecturer does not exist");

                _unitOfWork.LecturerRepository.Delete(lecturer, isHardDeleted: true);
                _unitOfWork.Complete();
                return new ResponseResult("Delete successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
    }
}
