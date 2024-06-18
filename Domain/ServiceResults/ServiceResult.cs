namespace Domain.ServiceResults;

public interface IServiceResult
{
    bool IsSuccess { get; init; }

    ServiceResult? Result { get; }
}

public record ServiceResult
    : IServiceResult
{
    public virtual bool IsSuccess { get; init; }

    public ServiceResult? Result => this;
}

public record ServiceResult<T>
    : IServiceResult
{
    public virtual bool IsSuccess { get; init; }

    public ServiceResult? Result { get; set; }

    public T? Value { get; set; }

    public ServiceResult(T value)
    {
        Value = value;
    }

    public ServiceResult(ServiceResult result)
    {
        Result = result;
    }

    public static implicit operator ServiceResult<T>(ServiceResult result)
    {
        return new ServiceResult<T>(result);
    }
}