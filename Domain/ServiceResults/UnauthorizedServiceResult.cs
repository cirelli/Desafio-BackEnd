namespace Domain.ServiceResults;

public interface IUnauthorizedServiceResult
{
}

public record UnauthorizedServiceResult()
    : UnauthorizedServiceResult<object>()
{

}

public record UnauthorizedServiceResult<T>()
    : ServiceResult<T>((T?)null),
      IUnauthorizedServiceResult
    where T : class
{
}
