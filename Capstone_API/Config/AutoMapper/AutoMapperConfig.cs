using AutoMapper;
using Capstone_API.DTO.Auth.Request;
using Capstone_API.DTO.Auth.Response;
using Capstone_API.DTO.Class.Response;
using Capstone_API.DTO.Distance.Request;
using Capstone_API.DTO.Distance.Response;
using Capstone_API.DTO.Lecturer.Request;
using Capstone_API.DTO.Lecturer.Response;
using Capstone_API.DTO.PreferenceLevel.Request;
using Capstone_API.DTO.PreferenceLevel.Response;
using Capstone_API.DTO.Semester.Request;
using Capstone_API.DTO.Semester.Response;
using Capstone_API.DTO.Subject.Request;
using Capstone_API.DTO.Subject.Response;
using Capstone_API.DTO.Task.Fetch;
using Capstone_API.DTO.Task.Request;
using Capstone_API.DTO.Task.Response;
using Capstone_API.DTO.TimeSlot.Request;
using Capstone_API.DTO.TimeSlot.Response;
using Capstone_API.Models;

namespace Capstone_API.Config.AutoMapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            // TaskMapper
            CreateMap<TaskAssign, SwapLecturerDTO>().ReverseMap();
            CreateMap<TaskAssign, SwapRoomDTO>().ReverseMap();
            CreateMap<TaskAssign, TaskModifyDTO>().ReverseMap();
            CreateMap<TaskAssign, DTO.Task.Response.GetAllTaskAssignDTO>().ReverseMap();
            CreateMap<TaskAssign, DTO.Task.Request.GetAllTaskAssignRequest>().ReverseMap();
            CreateMap<TimeSlotInfo, QueryDataByLecturerAndTimeSlot>().ReverseMap();

            // Lecturer Mapper
            CreateMap<Lecturer, LecturerResponse>().ReverseMap();
            CreateMap<Lecturer, CreateLecturerRequest>().ReverseMap();

            // Subject Mapper
            CreateMap<Subject, SubjectResponse>().ReverseMap();
            CreateMap<Subject, SubjectRequest>().ReverseMap();

            // Timeslot Mapper
            CreateMap<TimeSlot, TimeSlotResponse>().ReverseMap();
            CreateMap<TimeSlotConflict, GetTimeSlotConflictDTO>().ReverseMap();
            CreateMap<AreaSlotWeight, GetAreaSlotWeightDTO>().ReverseMap();

            // Room Mapper
            CreateMap<Room, RoomResponse>().ReverseMap();

            // Class Mapper
            CreateMap<Class, ClassResponse>().ReverseMap();

            // Execute Mapper
            CreateMap<ExecuteInfo, ExecuteInfoResponse>().ReverseMap();
            CreateMap<ExecuteInfo, CreateExecuteInfoRequest>().ReverseMap();


            // Preference Mapper
            CreateMap<SubjectPreferenceLevel, GetSubjectPreferenceLevelDTO>().ReverseMap();
            CreateMap<SubjectPreferenceLevel, UpdateSubjectPreferenceLevelDTO>().ReverseMap();

            // Distance Mapper
            CreateMap<Building, CreateBuildingDTO>().ReverseMap();
            CreateMap<Building, UpdateBuildingDTO>().ReverseMap();
            CreateMap<Building, BuildingResponse>().ReverseMap();

            // TimeSlot Mapper
            CreateMap<TimeSlot, UpdateTimeSlotDTO>().ReverseMap();
            CreateMap<TimeSlotSegment, TimeSlotSegmentDTO>().ReverseMap();

            // SemesterInfo Mapper
            CreateMap<SemesterInfo, SemesterRequest>().ReverseMap();
            CreateMap<SemesterInfo, SemesterResponse>().ReverseMap();

            // User Mapper
            CreateMap<User, LoginRequest>().ReverseMap();
            CreateMap<User, LoginResponse>().ReverseMap();

        }
    }
}
