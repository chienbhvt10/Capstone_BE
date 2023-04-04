namespace Capstone_API.DTO.Task.Request
{
    public class SwapRoomDTO
    {
        public int Id { get; set; }
        public int RoomId { get; set; }

        public SwapRoomDTO()
        {

        }

        public SwapRoomDTO(int id, int roomId)
        {
            Id = id;
            RoomId = roomId;
        }
    }
}
