namespace Domain.Dtos;

public record Pagination()
{
    public int Offset { get; set; } = 0;
    public int? Limit { get; set; }
    public string? OrderBy { get; set; }
    public bool Asc { get; set; } = true;
}

public record FilteredPagination<T>() : Pagination where T : IFilter, new()
{
    public T Filters { get; set; } = new();
}