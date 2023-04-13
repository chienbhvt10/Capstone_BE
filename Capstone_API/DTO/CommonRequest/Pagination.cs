namespace Capstone_API.DTO.CommonRequest
{
    public class Pagination
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItem { get; set; }
        public int TotalPage { get; set; }
    }
}
