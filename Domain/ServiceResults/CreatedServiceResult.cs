using Domain.Entities;

namespace Domain.ServiceResults;

public interface ICreatedServiceResult
{
    public string RouteName { get; }
    public object RouteValues { get; }
    public object? Value { get; }
}

public record CreatedServiceResult<T>
    : SuccessServiceResult<T>,
      ICreatedServiceResult
    where T : BaseEntity
{
    public CreatedServiceResult(string createdRouteName, SuccessServiceResult<T> result)
        : base(result.Value)
    {
        RouteName = createdRouteName;
        RouteValues = new { id = result.Value?.Id };
    }

    public string RouteName { get; }
    public object RouteValues { get; }
    object? ICreatedServiceResult.Value { get => Value; }
}
