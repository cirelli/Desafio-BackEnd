using Service = Services.NotificationService;

namespace Api.Controllers;

[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class NotificationController()
    : BaseController
{
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<List<Domain.Entities.Notification>>> GetAll([FromServices] Service service,
                                                                               [FromQuery] Pagination pagination,
                                                                               CancellationToken cancellationToken)
    {
        var result = await service.GetAllAsync(pagination, cancellationToken);
        return HandleServiceResult(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    public async Task<ActionResult<Domain.Entities.Notification>> GetById([FromServices] Service service,
                                                                          Guid id,
                                                                          CancellationToken cancellationToken)
    {
        var result = await service.GetByIdAsync(id, cancellationToken);
        return HandleServiceResult(result);
    }
}