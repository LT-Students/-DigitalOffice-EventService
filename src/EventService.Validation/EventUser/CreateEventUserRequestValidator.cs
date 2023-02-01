using System;
using FluentValidation;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Enums;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventsUsers;
using LT.DigitalOffice.EventService.Validation.EventUser.Interfaces;

namespace LT.DigitalOffice.EventService.Validation.EventUser;

	public class CreateEventUserRequestValidator : AbstractValidator<CreateEventUserRequest>, ICreateEventUserRequestValidator
	{
      public CreateEventUserRequestValidator(IEventUserRepository repository)
      {
        RuleFor(request => request)
          .MustAsync(async (x, _) => !await repository.IsUserAddedToEventAsync(x.UserId, x.EventId))
          .WithMessage("User is already added to event");

        RuleFor(request => request.UserStatus)
          .Must(x => x != EventUserStatus.Discarded)
          .WithMessage("You can not add user with Discarded status")
          .Must(x => x != EventUserStatus.Refused)
          .WithMessage("You can not add user with Refused status");

        When(request => request.NotifyAtUtc is not null, () =>
        {
          RuleFor(request => request.NotifyAtUtc)
            .Must(x => x > DateTime.UtcNow)
            .WithMessage("Notification time is earlier than now");  // add validation that notification mustn't be later than event date
        });
      }
}

