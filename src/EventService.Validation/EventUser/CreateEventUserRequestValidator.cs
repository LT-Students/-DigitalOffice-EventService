using System;
using System.Linq;
using FluentValidation;
using LT.DigitalOffice.EventService.Broker.Requests.Interfaces;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventUser;
using LT.DigitalOffice.EventService.Validation.EventUser.Interfaces;

namespace LT.DigitalOffice.EventService.Validation.EventUser;

	public class CreateEventUserRequestValidator : AbstractValidator<CreateEventUserRequest>, ICreateEventUserRequestValidator
	{
      public CreateEventUserRequestValidator(
        IEventUserRepository eventUserRepository, 
        IUserService userService, 
        IEventRepository eventRepository)
      {
        RuleFor(request => request.Users)
          .NotEmpty()
          .WithMessage("Empty user list.")
          .WithMessage("Some users doubled.")
          .MustAsync(async (users, _) =>
            (await userService.CheckUsersExistenceAsync(users.Select(users => users.UserId).ToList())).Count() == users.Count)
          .WithMessage("Some users doesn't exist.");

        RuleFor(request => request.EventId)
          .MustAsync(async (eventId, _) => await eventRepository.DoesExistAsync(eventId))
          .WithMessage("This event doesn't exist.");

        RuleFor(request => request)
          .MustAsync(async (x, _) =>
            !await eventUserRepository.DoesExistAsync(x.Users.Select(e => e.UserId).ToList(), x.EventId))
          .WithMessage("Some users have already been invited to the event or are participants in it.");

        When(request => request.Users.Select(r => r.NotifyAtUtc).ToList().Count > 0,
          () =>
        {
          RuleFor(request => request)
            .MustAsync(async (req, _) =>
            {
              DateTime evenTime = (await eventRepository.GetAsync(req.EventId)).Date;
              return !req.Users.Any(user =>
                user.NotifyAtUtc != null && (user.NotifyAtUtc < DateTime.UtcNow || user.NotifyAtUtc > evenTime));
            })
            .WithMessage("Some notification time is not valid, notification time mustn't be earlier than now and not later than event date.");
        });
      }
}

