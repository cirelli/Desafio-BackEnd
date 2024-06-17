namespace Domain.ServiceResults;

public interface IConflictServiceResult
{
    public string Message { get; }
}

public record ConflictServiceResult<T>(string Message)
    : ServiceResult<T>((T?)null),
      IConflictServiceResult
    where T : class
{

}
