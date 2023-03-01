using AutoMapper;
using Capstone_API.DTO.Lecturer;
using Capstone_API.Models;
using Capstone_API.Results;
using Capstone_API.Service.Interface;
using Capstone_API.UOW_Repositories.UnitOfWork;
using UTA.T2.MusicLibrary.Service.Results;

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

        public GenericResult<IEnumerable<LecturerDTO>> GetAll()
        {
            try
            {
                var lecturers = _unitOfWork.LecturerRepository.GetAll();
                var lecturersViewModel = _mapper.Map<IEnumerable<LecturerDTO>>(lecturers);
                return new GenericResult<IEnumerable<LecturerDTO>>(lecturersViewModel);
            }
            catch (Exception ex)
            {
                return new GenericResult<IEnumerable<LecturerDTO>>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        public GenericResult<LecturerDTO> GetOneLecturer(int Id)
        {
            try
            {
                var lecturer = _unitOfWork.LecturerRepository.GetById(Id);
                var lecturerViewModel = _mapper.Map<LecturerDTO>(lecturer);

                return new GenericResult<LecturerDTO>(lecturerViewModel);
            }
            catch (Exception ex)
            {
                return new GenericResult<LecturerDTO>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        public ResponseResult CreateLecturer(LecturerDTO request)
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

        public ResponseResult UpdateLecturer(LecturerDTO request)
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
