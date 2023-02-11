using System;
using System.Linq;
using FluentValidation;
using LT.DigitalOffice.EventService.Broker.Requests.Interfaces;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventUser;
using LT.DigitalOffice.EventService.Validation.EventUser.Interfaces;
using LT.DigitalOffice.Kernel.Extensions;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.EventService.Validation.EventUser;

	public class CreateEventUserRequestValidator : AbstractValidator<CreateEventUserRequest>, ICreateEventUserRequestValidator
	{
      public CreateEventUserRequestValidator(
        IEventUserRepository eventUserRepository, 
        IUserService userService, 
        IEventRepository eventRepository,
        IHttpContextAccessor contextAccessor)
      {
        RuleFor(request => request.Users)
          .NotEmpty()
          .WithMessage("User list must not be empty.")
          .MustAsync(async (users, _) =>
            (await userService.CheckUsersExistenceAsync(
              users.Select(userRequest => userRequest.UserId).ToList())).Count() == users.Count())
          .WithMessage("Some users doesn't exist.");

        RuleFor(request => request.EventId)
          .MustAsync(async (eventId, _) => await eventRepository.DoesExistAsync(eventId))
          .WithMessage("This event doesn't exist.");

        RuleFor(request => request)
          .MustAsync(async (x, _) =>
            !await eventUserRepository.DoesExistAsync(x.Users.Select(e => e.UserId).ToList(), x.EventId))
          .WithMessage("Some users have already been invited to the event or are participants in it.")
          .Must(x => !(x.Users.Count > 1 &&
                       x.Users.Exists(user => user.UserId == contextAccessor.HttpContext.GetUserId())))
          .WithMessage("User list must contains only your Id if you want to add yourself");

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
            .WithMessage("Notification time mustn't be earlier than now and not later than event date.");
        });
      }
}

