using Capstone_API.DTO.CommonRequest;

namespace Capstone_API.DTO.PreferenceLevel.Request
{
    public class GetSlotPreferenceRequest
    {
        public string? Lecturer { get; set; }
        public GetAllRequest? GetAllRequest { get; set; }
        public PaginationRequest? Pagination { get; set; }
    }
}
