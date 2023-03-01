namespace Capstone_API.DTO.Task
{
    public class SwapRoomOfTaskDTO
    {
        public int Id { get; set; }
        public int RoomId { get; set; }

        public SwapRoomOfTaskDTO()
        {

        }

        public SwapRoomOfTaskDTO(int id, int roomId)
        {
            Id = id;
            RoomId = roomId;
        }
    }
}
