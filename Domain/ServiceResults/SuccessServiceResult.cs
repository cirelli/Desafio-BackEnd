namespace Domain.ServiceResults;

public interface ISuccessServiceResult
{
    public object? Value { get; }
}

public record SuccessServiceResult()
    : ServiceResult;

public record SuccessServiceResult<T>(T? Value)
    : ServiceResult<T>(Value),
      ISuccessServiceResult
    where T : class
{
    object? ISuccessServiceResult.Value { get => Value; }
}
