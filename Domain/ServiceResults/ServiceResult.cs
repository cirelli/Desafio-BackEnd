namespace Domain.ServiceResults;

public record ServiceResult
{
}

public record ServiceResult<T>(T? Value)
    : ServiceResult
    where T : class
{
}