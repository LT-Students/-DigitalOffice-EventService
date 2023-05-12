using System.Threading;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Models.Dto.Models;
using LT.DigitalOffice.EventService.Models.Dto.Requests.UserBirthday;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.EventService.Business.Commands.UserBirthday.Interfaces;

[AutoInject]
public interface IFindUserBirthdayCommand
{
  Task<FindResultResponse<UserBirthdayInfo>> ExecuteAsync(
    FindUsersBirthdaysFilter filter,
    CancellationToken cancellationToken = default);
}
