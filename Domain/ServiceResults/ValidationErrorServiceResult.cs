namespace Domain.ServiceResults;

public interface IValidationErrorServiceResult
{
    public IEnumerable<KeyValuePair<string, string>> Errors { get; }
}

public record ValidationErrorServiceResult<T>(IEnumerable<KeyValuePair<string, string>> Errors)
    : ServiceResult<T>((T?)null),
      IValidationErrorServiceResult
    where T : class
{
    public ValidationErrorServiceResult(string key, string value)
        : this([new KeyValuePair<string, string>(key, value)])
    {
        
    }
}
