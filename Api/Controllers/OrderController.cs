using Service = Services.OrderService;
using ViewModel = Domain.Dtos.OrderViewModel;

namespace Api.Controllers;

[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class OrderController(IMapper mapper,
                             Service service)
    : BaseController
{
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<List<Message>>> GetAll([FromQuery] Pagination pagination,
                                                          CancellationToken cancellationToken)
    {
        var result = await service.GetAllAsync<ViewModel>(pagination, cancellationToken);
        return HandleServiceResult(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id}", Name = "OrderById")]
    public async Task<ActionResult<ViewModel>> GetById(Guid id,
                                                       CancellationToken cancellationToken)
    {
        var result = await service.GetByIdAsync<ViewModel>(id, cancellationToken);
        return HandleServiceResult(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<ViewModel>> Create([FromBody] OrderDTO dto,
                                                      CancellationToken cancellationToken)
    {
        if (dto is null)
        {
            return BadRequest("Invalid body!");
        }

        var result = await service.CreateAsync(dto, cancellationToken);

        if (result.IsSuccess)
        {
            return CreatedAtRoute<ViewModel>(mapper, "OrderById", result.Value!);
        }

        return HandleServiceResult(result);
    }

    [Authorize(Roles = "Driver")]
    [HttpPost("{id}/Accept")]
    public async Task<ActionResult> Accept([FromServices] CurrentUser currentUser,
                                           Guid id,
                                           CancellationToken cancellationToken)
    {
        var result = await service.AcceptAsync(currentUser.Id, id, cancellationToken);
        return HandleServiceResult(result);
    }

    [Authorize(Roles = "Driver")]
    [HttpPost("{id}/Deliver")]
    public async Task<ActionResult> Deliver([FromServices] CurrentUser currentUser,
                                            Guid id,
                                            CancellationToken cancellationToken)
    {
        var result = await service.DeliverAsync(currentUser.Id, id, cancellationToken);
        return HandleServiceResult(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id,
                                            CancellationToken cancellationToken)
    {
        var result = await service.DeleteAsync(id, cancellationToken);
        return HandleServiceResult(result);
    }
}