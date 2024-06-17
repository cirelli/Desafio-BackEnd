namespace Services;

public abstract class BaseService()
{
    private const string NotFoundMsg = "Not found!";

    protected static NotFoundServiceResult NotFound() => new(NotFoundMsg);
    protected static NotFoundServiceResult<T> NotFound<T>()
        where T : class
        => new(NotFoundMsg);
}
