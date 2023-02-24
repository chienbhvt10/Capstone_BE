namespace Exercise.Data.Entities
{
    public class Class // Đặt tên như này có ngày nó lỗi đéo biết tại sao :v
    {

        public int Id { get; set; }
        public string? Name { get; set; }

        public Class(int id, string? name)
        {
            Id = id;
            Name = name;
        }
        public Class()
        {
        }

    }
}