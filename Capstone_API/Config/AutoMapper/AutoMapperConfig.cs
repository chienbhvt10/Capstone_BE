using AutoMapper;
using Capstone_API.DTO.Class.Response;
using Capstone_API.DTO.Distance.Request;
using Capstone_API.DTO.Distance.Response;
using Capstone_API.DTO.Lecturer.Response;
using Capstone_API.DTO.PreferenceLevel.Request;
using Capstone_API.DTO.PreferenceLevel.Response;
using Capstone_API.DTO.Subject.Response;
using Capstone_API.DTO.Task.Fetch;
using Capstone_API.DTO.Task.Request;
using Capstone_API.DTO.Task.Response;
using Capstone_API.DTO.TimeSlot.Response;
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
            CreateMap<TimeSlotInfo, QueryDataByLecturerAndTimeSlot>().ReverseMap();

            // Lecturer Mapper
            CreateMap<Lecturer, LecturerResponse>().ReverseMap();

            // Subject Mapper
            CreateMap<Subject, SubjectResponse>().ReverseMap();

            // Timeslot Mapper
            CreateMap<TimeSlot, TimeSlotResponse>().ReverseMap();
            CreateMap<TimeSlotCompatibility, GetTimeSlotCompatibilityDTO>().ReverseMap();
            CreateMap<TimeSlotConflict, GetTimeSlotConflictDTO>().ReverseMap();
            CreateMap<AreaSlotWeight, GetAreaSlotWeightDTO>().ReverseMap();

            // Room Mapper
            CreateMap<Room, RoomResponse>().ReverseMap();

            // Class Mapper
            CreateMap<Class, ClassResponse>().ReverseMap();

            // Execute Mapper
            CreateMap<ExecuteInfo, ExecuteInfoResponse>().ReverseMap();

            // Preference Mapper
            CreateMap<SubjectPreferenceLevel, GetSubjectPreferenceLevelDTO>().ReverseMap();
            CreateMap<SubjectPreferenceLevel, UpdateSubjectPreferenceLevelDTO>().ReverseMap();

            // Distance Mapper

            CreateMap<Building, CreateBuildingDTO>().ReverseMap();
            CreateMap<Building, BuildingResponse>().ReverseMap();


        }
    }
}
