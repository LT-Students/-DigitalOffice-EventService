using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Business.Commands.EventsUsers.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventsUsers;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.EventService.Controllers;

  [Route("[controller]")]
  [ApiController]
  public class EventUserController : ControllerBase
  {
    [HttpPost("create")]
    public async Task<OperationResultResponse<List<Guid>>> CreateAsync(
      [FromServices] ICreateEventUserCommand command,
      [FromBody] CreateEventUserRequest request)
    {
      return await command.ExecuteAsync(request);
    }
  }

