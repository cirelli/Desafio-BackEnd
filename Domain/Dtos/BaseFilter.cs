namespace Domain.Dtos;

public interface IFilter
{

}

public record BaseFilter : IFilter
{
    public string? Search { get; set; }
}
