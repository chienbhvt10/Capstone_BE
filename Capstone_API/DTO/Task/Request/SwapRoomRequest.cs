namespace Capstone_API.DTO.Task.Request
{
    public class SwapRoomRequest
    {
        public int Id { get; set; }
        public int RoomId { get; set; }

        public SwapRoomRequest()
        {

        }

        public SwapRoomRequest(int id, int roomId)
        {
            Id = id;
            RoomId = roomId;
        }
    }
}
