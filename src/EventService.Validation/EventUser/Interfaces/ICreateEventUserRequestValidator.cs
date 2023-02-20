using FluentValidation;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventsUsers;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Validation.EventUser.Interfaces
{
  [AutoInject]
  public interface ICreateEventUserRequestValidator : IValidator<CreateEventUserRequest>
  {
  }
}
