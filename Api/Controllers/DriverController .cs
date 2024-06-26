﻿using Service = Services.DriverService;
using ViewModel = Domain.Dtos.DriverViewModel;

namespace Api.Controllers;

[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class DriverController(IMapper mapper)
    : BaseController
{
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<List<ViewModel>>> GetAll([FromServices] Service driverService,
                                                            [FromQuery] FilteredPagination<DriverFilter> pagination,
                                                            CancellationToken cancellationToken)
    {
        var result = await driverService.GetAllAsync<ViewModel>(pagination, cancellationToken);
        return HandleServiceResult(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id}", Name = "DriverById")]
    public async Task<ActionResult<ViewModel>> GetById([FromServices] Service driverService,
                                                       Guid id,
                                                       CancellationToken cancellationToken)
    {
        var result = await driverService.GetByIdAsync<ViewModel>(id, cancellationToken);
        return HandleServiceResult(result);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<ViewModel>> Create([FromServices] Service driverService,
                                                      [FromBody] DriverDTO driver,
                                                      CancellationToken cancellationToken)
    {
        if (driver is null)
        {
            return BadRequest("Driver is null");
        }

        var result = await driverService.CreateAsync(driver, cancellationToken);

        if (result.IsSuccess)
        {
            return CreatedAtRoute<ViewModel>(mapper, "DriverById", result.Value!);
        }

        return HandleServiceResult(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromServices] Service driverService,
                                            Guid id,
                                            CancellationToken cancellationToken)
    {
        var result = await driverService.DeleteAsync(id, cancellationToken);
        return HandleServiceResult(result);
    }
}