namespace Api.Controllers;

public abstract class BaseController : ControllerBase
{
    protected ActionResult HandleServiceResult<T>(IMapper? mapper,
                                                  ServiceResult result)
        where T : class
    {
        return result switch
        {
            IForbiddenServiceResult _ => Forbid(),
            IInvalidServiceResult r => BadRequest(r.Message),
            IValidationErrorServiceResult r => InvalidResult(r),
            IConflictServiceResult r => Conflict(r.Message),
            ICreatedServiceResult r => CreatedResult<T>(mapper, r),
            SuccessServiceResult _ => NoContent(),
            ISuccessServiceResult r => Ok(r.Value),
            INotFoundServiceResult r => NotFound(r.Message),
            IUnauthorizedServiceResult _ => Unauthorized(),
            _ => throw new Exception("Unknown type of ServiceResult")
        };
    }

    protected ActionResult HandleServiceResult(ServiceResult result)
        => HandleServiceResult<object>(null, result);

    private BadRequestObjectResult InvalidResult(IValidationErrorServiceResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(error.Key, error.Value);
        }

        return BadRequest(ModelState);
    }

    private CreatedAtRouteResult CreatedResult<T>(IMapper? mapper,
                                                  ICreatedServiceResult result)
    {
        object? value = null;
        if (result.Value is not null)
        {
            value = Map<T>(mapper, result.Value);
        }

        return CreatedAtRoute(result.RouteName, result.RouteValues, value);
    }

    private static object? Map<T>(IMapper? mapper, object value)
    {
        if (mapper is not null && typeof(T) != typeof(object))
        {
            return mapper.Map<T>(value);
        }

        return value;
    }
}
