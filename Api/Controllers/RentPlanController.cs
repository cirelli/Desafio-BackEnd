using Microsoft.AspNetCore.Authorization;
using Service = Services.RentPlanService;
using ViewModel = Domain.Dtos.RentPlanViewModel;

namespace Api.Controllers;

[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class RentPlanController(IMapper mapper) : BaseController
{
    [Authorize(Roles = "Admin, Driver")]
    [HttpGet]
    public async Task<ActionResult<List<ViewModel>>> GetAll([FromServices] Service rentPlanService,
                                                            [FromQuery] Pagination pagination,
                                                            CancellationToken cancellationToken)
    {
        ServiceResult result = await rentPlanService.GetAllAsync<ViewModel>(pagination, cancellationToken);
        return HandleServiceResult(result);
    }

    [Authorize(Roles = "Admin, Driver")]
    [HttpGet("{id}", Name = "RentPlanById")]
    public async Task<ActionResult<ViewModel>> GetById([FromServices] Service rentPlanService,
                                                       Guid id,
                                                       CancellationToken cancellationToken)
    {
        ServiceResult result = await rentPlanService.GetByIdAsync<ViewModel>(id, cancellationToken);
        return HandleServiceResult(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<ViewModel>> Create([FromServices] Service rentPlanService,
                                                      [FromBody] RentPlanDTO rentPlan,
                                                      CancellationToken cancellationToken)
    {
        if (rentPlan is null)
        {
            return BadRequest("Rent Plan is null");
        }

        ServiceResult result = await rentPlanService.CreateAsync(rentPlan, cancellationToken);

        if (result is SuccessServiceResult<RentPlan> successResult)
        {
            result = new CreatedServiceResult<RentPlan>("RentPlanById", successResult);
        }

        return HandleServiceResult<ViewModel>(mapper, result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromServices] Service rentPlanService,
                                            Guid id,
                                            CancellationToken cancellationToken)
    {
        ServiceResult result = await rentPlanService.DeleteAsync(id, cancellationToken);
        return HandleServiceResult(result);
    }
}