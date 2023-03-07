using AutoMapper;
using Capstone_API.DTO.Lecturer.Response;
using Capstone_API.DTO.Task.Request;
using Capstone_API.DTO.Task.Response;
using Capstone_API.Models;

namespace Capstone_API.Config.AutoMapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            // TaskMapper
            CreateMap<TaskAssign, SwapLecturerRequest>().ReverseMap();
            CreateMap<TaskAssign, SwapRoomRequest>().ReverseMap();
            CreateMap<TaskAssign, TaskModifyRequest>().ReverseMap();
            CreateMap<TaskAssign, GetAllTaskAssignResponse>().ReverseMap();
            CreateMap<TaskAssign, GetAllTaskAssignRequest>().ReverseMap();

            // Lecturer Mapper
            CreateMap<Lecturer, LecturerResponse>().ReverseMap();

            // Subject Mapper

        }
    }
}
