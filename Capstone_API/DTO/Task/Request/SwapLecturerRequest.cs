namespace Capstone_API.DTO.Task.Request
{
    public class SwapLecturerRequest
    {
        public int Id { get; set; }
        public int LecturerId { get; set; }

        public SwapLecturerRequest()
        {

        }

        public SwapLecturerRequest(int id, int lecturerId)
        {
            Id = id;
            LecturerId = lecturerId;
        }
    }
}
