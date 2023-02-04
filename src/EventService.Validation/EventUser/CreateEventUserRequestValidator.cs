using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using FluentValidation;
using LT.DigitalOffice.EventService.Broker.Requests.Interfaces;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventsUsers;
using LT.DigitalOffice.EventService.Validation.EventUser.Interfaces;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.EventService.Validation.EventUser;

	public class CreateEventUserRequestValidator : AbstractValidator<CreateEventUserRequest>, ICreateEventUserRequestValidator
	{
      public CreateEventUserRequestValidator(IEventUserRepository eventUserRepository, IUserService userService, IEventRepository eventRepository)
      {
        RuleFor(request => request.Users)
          //.Must(users =>
          //{
          //  List<Guid> usersIds = users.Select(r => r.UserId).ToList();
          //  return usersIds.Distinct().Count() == usersIds.Count();
          //})
          //.WithMessage("Some users doubled.")
          .MustAsync(async (users, _) =>
          {
            List<Guid> usersIds = users.Select(r => r.UserId).ToList();
            return (await userService.CheckUsersExistenceAsync(usersIds)).Count == usersIds.Count;
          })
          .WithMessage("Some users doesn't exist.");

        RuleFor(request => request.EventId)
          .MustAsync(async (eventId, _) => await eventRepository.DoesExistAsync(eventId))
          .WithMessage("This event doesn't exist.");

        RuleFor(request => request)
          .MustAsync(async (x, _) =>
          {
            foreach (var user in x.Users)
            {
              if (await eventUserRepository.DoesExistAsync(user.UserId, x.EventId))
              {
                return false;
              }
            }
            return true;
          })
          .WithMessage("This user doesn't exist");

        When(request => request.Users.Select(r => r.NotifyAtUtc).ToList().Count > 0, () =>
        {
          RuleFor(request => request)
            .MustAsync(async (req, _) =>
            {
              DateTime evenTime = (await eventRepository.GetAsync(req.EventId)).Date;
              return !req.Users.Any(user =>
                user.NotifyAtUtc != null && (user.NotifyAtUtc < DateTime.UtcNow || user.NotifyAtUtc > evenTime));
            })
            .WithMessage("Some notification time is not valid, notification time mustn't be earlier than now and not later than event date");
        });
      }
}

