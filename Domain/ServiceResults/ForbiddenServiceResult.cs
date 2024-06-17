namespace Domain.ServiceResults;

public interface IForbiddenServiceResult
{
}

public record ForbiddenServiceResult()
    : UnauthorizedServiceResult<object>()
{

}

public record ForbiddenServiceResult<T>()
    : ServiceResult<T>((T?)null),
      IForbiddenServiceResult
    where T : class
{
}
