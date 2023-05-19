using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Models.Dto.Models;
using LT.DigitalOffice.EventService.Models.Dto.Requests.File;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.EventService.Business.Commands.File.Interfaces;

[AutoInject]
public interface IFindFilesCommand
{
  Task<FindResultResponse<FileInfo>> ExecuteAsync(FindEventFilesFilter findFilter);
}
