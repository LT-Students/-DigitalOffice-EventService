using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using LT.DigitalOffice.EventService.Models.Dto.Requests;
using LT.DigitalOffice.EventService.Business.Commands.EventCategory.Interfaces;

namespace LT.DigitalOffice.EventService.Controllers;

[Route("[controller]")]
[ApiController]
public class EventCategoryController : ControllerBase
{
  [HttpPost("create")]
  public async Task<OperationResultResponse<Guid?>> CreateAsync(
    [FromServices] ICreateEventCategoryCommand command,
    [FromBody] CreateEventCategoryRequest request)
  {
    return await command.ExecuteAsync(request);
  }
}

