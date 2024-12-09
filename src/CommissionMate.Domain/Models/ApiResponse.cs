namespace Domain.Models
{
    public class ApiResponse<T>
    {
        public Result<T> Result { get; set; }

        public MetaData MetaData { get; set; }
    }
}
