using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Image;

namespace LT.DigitalOffice.EventService.Business.Commands.Image.Interfaces;

[AutoInject]
public interface IRemoveImageCommand
{
  Task<OperationResultResponse<bool>> ExecuteAsync(RemoveImageRequest request);
}
