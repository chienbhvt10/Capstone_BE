namespace Capstone_API.DTO.Task
{
    public class SwapLecturerOfTaskDTO
    {
        public int Id { get; set; }
        public int LecturerId { get; set; }

        public SwapLecturerOfTaskDTO()
        {

        }

        public SwapLecturerOfTaskDTO(int id, int lecturerId)
        {
            Id = id;
            LecturerId = lecturerId;
        }
    }
}
