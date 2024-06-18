using Service = Services.MotorbikeService;
using ViewModel = Domain.Dtos.MotorbikeViewModel;

namespace Api.Controllers;

[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class MotorbikeController(IMapper mapper)
    : BaseController
{
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<List<ViewModel>>> GetAll([FromServices] Service Service,
                                                            [FromQuery] FilteredPagination<BaseFilter> pagination,
                                                            CancellationToken cancellationToken)
    {
        var result = await Service.GetAllAsync<ViewModel>(pagination, cancellationToken);
        return HandleServiceResult(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id}", Name = "MotorbikeById")]
    public async Task<ActionResult<ViewModel>> GetById([FromServices] Service Service,
                                                       Guid id,
                                                       CancellationToken cancellationToken)
    {
        var result = await Service.GetByIdAsync<ViewModel>(id, cancellationToken);
        return HandleServiceResult(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<ViewModel>> Create([FromServices] Service Service,
                                                      [FromBody] MotorbikeDTO motorbike,
                                                      CancellationToken cancellationToken)
    {
        if (motorbike is null)
        {
            return BadRequest("Motorbike is null");
        }

        var result = await Service.CreateAsync(motorbike, cancellationToken);

        if (result.IsSuccess)
        {
            return CreatedAtRoute<ViewModel>(mapper, "MotorbikeById", result.Value!);
        }

        return HandleServiceResult(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("{id}")]
    public async Task<IActionResult> Update([FromServices] Service Service,
                                            Guid id,
                                            [FromBody] MotorbikePatchDTO motorbike,
                                            CancellationToken cancellationToken)
    {
        if (motorbike is null)
        {
            return BadRequest("Motorbike is null");
        }

        var result = await Service.UpdatePlateAsync(id, motorbike, cancellationToken);
        return HandleServiceResult(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromServices] Service Service,
                                            Guid id,
                                            CancellationToken cancellationToken)
    {
        var result = await Service.DeleteAsync(id, cancellationToken);
        return HandleServiceResult(result);
    }
}