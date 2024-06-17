﻿using Microsoft.AspNetCore.Authorization;
using Service = Services.RentService;
using ViewModel = Domain.Dtos.RentViewModel;

namespace Api.Controllers;

[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class RentController(IMapper mapper) : BaseController
{
    [Authorize(Roles = "Admin, Driver")]
    [HttpGet]
    public async Task<ActionResult<List<ViewModel>>> GetAll([FromServices] Service service,
                                                            [FromQuery] Pagination pagination,
                                                            CancellationToken cancellationToken)
    {
        ServiceResult result = await service.GetAllAsync<ViewModel>(pagination, cancellationToken);
        return HandleServiceResult(result);
    }

    [Authorize(Roles = "Admin, Driver")]
    [HttpGet("{id}", Name = "RentById")]
    public async Task<ActionResult<ViewModel>> GetById([FromServices] Service service,
                                                       Guid id,
                                                       CancellationToken cancellationToken)
    {
        ServiceResult result = await service.GetByIdAsync<ViewModel>(id, cancellationToken);
        return HandleServiceResult(result);
    }

    [Authorize(Roles = "Driver")]
    [HttpPost]
    public async Task<ActionResult<ViewModel>> Create([FromServices] CurrentUser currentUser,
                                                      [FromServices] Service service,
                                                      [FromBody] RentDTO dto,
                                                      CancellationToken cancellationToken)
    {
        if (dto is null)
        {
            return BadRequest("Invalid body!");
        }

        ServiceResult result = await service.CreateAsync(currentUser.Id, dto, cancellationToken);

        if (result is SuccessServiceResult<Rent> successResult)
        {
            result = new CreatedServiceResult<Rent>("RentById", successResult);
        }

        return HandleServiceResult<ViewModel>(mapper, result);
    }

    [Authorize(Roles = "Driver")]
    [HttpPatch("{id}")]
    public async Task<ActionResult<ViewModel>> SetEndDate([FromServices] CurrentUser currentUser,
                                                          [FromServices] Service service,
                                                          Guid id,
                                                          [FromBody] RentPatchDTO dto,
                                                          CancellationToken cancellationToken)
    {
        if (dto is null)
        {
            return BadRequest("Invalid body!");
        }

        ServiceResult result = await service.SetEndDateAsync(currentUser.Id, id, dto, cancellationToken);
        return HandleServiceResult(result);
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