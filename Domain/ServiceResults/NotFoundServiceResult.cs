namespace Domain.ServiceResults;

public interface INotFoundServiceResult
{
    public string Message { get; }
}

public record NotFoundServiceResult(string Message)
    : NotFoundServiceResult<object>(Message)
{

}

public record NotFoundServiceResult<T>(string Message)
    : ServiceResult<T>((T?)null),
      INotFoundServiceResult
    where T : class
{
}
