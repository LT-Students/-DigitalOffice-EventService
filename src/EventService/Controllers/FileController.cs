using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Business.Commands.File.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Models;
using LT.DigitalOffice.EventService.Models.Dto.Requests.File;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.EventService.Controllers;

[Route("[controller]")]
[ApiController]
public class FileController : ControllerBase
{
  [HttpGet("find")]
  public async Task<FindResultResponse<FileInfo>> FindAsync(
    [FromServices] IFindFilesCommand command,
    [FromQuery] FindFilesFilter findFilter)
  {
    return await command.ExecuteAsync(findFilter);
  }

  [HttpDelete("remove")]
  public async Task<OperationResultResponse<bool>> RemoveAsync(
    [FromServices] IRemoveFilesCommand command,
    [FromBody] RemoveFilesRequest request)
  {
    return await command.ExecuteAsync(request);
  }
}
