namespace Domain.Models
{
    public record struct Result<T>
    {
        public T[]? Values { get; set; }
    }
}
