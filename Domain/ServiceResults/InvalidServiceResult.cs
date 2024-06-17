namespace Domain.ServiceResults;

public interface IInvalidServiceResult
{
    public string Message { get; }
}

public record InvalidServiceResult<T>(string Message)
    : ServiceResult<T>((T?)null),
      IInvalidServiceResult
    where T : class;
