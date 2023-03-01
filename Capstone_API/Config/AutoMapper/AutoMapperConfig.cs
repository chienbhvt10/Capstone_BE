using AutoMapper;
using Capstone_API.DTO.Lecturer;
using Capstone_API.DTO.Task;
using Capstone_API.Models;

namespace Capstone_API.Config.AutoMapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            // TaskMapper
            CreateMap<TaskAssign, SwapLecturerOfTaskDTO>().ReverseMap();
            CreateMap<TaskAssign, SwapRoomOfTaskDTO>().ReverseMap();
            CreateMap<TaskAssign, TaskModifyDTO>().ReverseMap();

            // Lecturer Mapper
            CreateMap<Lecturer, LecturerDTO>().ReverseMap();

            // Subject Mapper

        }
    }
}
