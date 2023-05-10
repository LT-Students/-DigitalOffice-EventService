using System.Threading;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Business.Commands.UserBirthday.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Models;
using LT.DigitalOffice.EventService.Models.Dto.Requests.UserBirthday;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.EventService.Controllers;

[Route("[controller]")]
[ApiController]
public class UserBirthdayController : ControllerBase
{
  [HttpGet("find")]
  public async Task<FindResultResponse<UserBirthdayInfo>> FindAsync(
    [FromServices] IFindUserBirthdayCommand command,
    [FromQuery] FindUsersBirthdaysFilter filter,
    CancellationToken cancellationToken)
  {
    return await command.ExecuteAsync(filter: filter, cancellationToken: cancellationToken);
  }
}
