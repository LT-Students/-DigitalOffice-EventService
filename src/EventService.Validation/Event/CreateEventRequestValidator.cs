using System;
using System.Linq;
using FluentValidation;
using LT.DigitalOffice.EventService.Broker.Requests.Interfaces;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Enums;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Event;
using LT.DigitalOffice.EventService.Validation.Event.Interfaces;
using LT.DigitalOffice.Kernel.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace LT.DigitalOffice.EventService.Validation.Event;

public class CreateEventRequestValidator : AbstractValidator<CreateEventRequest>, ICreateEventRequestValidator
{
  public CreateEventRequestValidator(
    IUserService userService,
    IHttpContextAccessor contextAccessor,
    ICategoryRepository categoryRepository)
  {
    RuleFor(ev => ev.Name)
      .MaximumLength(150)
      .WithMessage("Name should not exceed maximum length of 150 symbols");

    When(ev => !string.IsNullOrWhiteSpace(ev.Description),
      () =>
    {
      RuleFor(ev => ev.Description)
      .MaximumLength(500)
      .WithMessage("Description should not exceed maximum length of 500 symbols");
    });

    RuleFor(ev => ev.Address)
      .MaximumLength(400)
      .WithMessage("Address should not exceed maximum length of 400 symbols");

    RuleFor(ev => ev.Date)
      .Must(d => d > DateTime.UtcNow)
      .WithMessage("The event date must be later than the date the event was created");

    RuleFor(ev => ev.Users)
      .Cascade(CascadeMode.StopOnFirstFailure)
      .NotEmpty()
      .WithMessage("User list must not be empty.")
      .Must((x, _) =>
        x.Users.Select(u => u.UserId).Contains(contextAccessor.HttpContext.GetUserId()))
      .WithMessage("Event organizer must be in list of participants.")
      .MustAsync(async (users, _) =>
      await userService.CheckUsersExistenceAsync(users.Select(userRequest => userRequest.UserId).ToList()))
      .WithMessage("Some users doesn't exist.");

    When(ev => ev.Access == AccessType.Closed,
      () =>
      {
        RuleFor(ev => ev.Users)
        .Must(x => x.Count > 1)
        .WithMessage("There should be at least one invited user in closed event");
      });

    When(ev => ev.Users.Select(r => r.NotifyAtUtc).ToList().Count > 0,
      () =>
      {
        RuleFor(ev => ev)
        .Must(ev =>
        {
          return !ev.Users.Any(user =>
            user.NotifyAtUtc != null && (user.NotifyAtUtc < DateTime.UtcNow || user.NotifyAtUtc > ev.Date));
        })
          .WithMessage("Some notification time is not valid, notification time mustn't be earlier than now or later than date of the event");
      });

    When(ev => !ev.CategoriesIds.IsNullOrEmpty(),
      () =>
      {
        RuleFor(ev => ev.CategoriesIds)
        .Must(categoryRepository.DoesExistAllAsync)
        .WithMessage("Some of categories in the list doesn't exist.")
        .Must(cat => cat.Count < 6)
        .WithMessage("Count of categories to event must be no more than 5");
      });
  }
}
