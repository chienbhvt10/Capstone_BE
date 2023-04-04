namespace Capstone_API.DTO.Task.Request
{
    public class SwapLecturerDTO
    {
        public int Id { get; set; }
        public int LecturerId { get; set; }

        public SwapLecturerDTO()
        {

        }

        public SwapLecturerDTO(int id, int lecturerId)
        {
            Id = id;
            LecturerId = lecturerId;
        }
    }
}
