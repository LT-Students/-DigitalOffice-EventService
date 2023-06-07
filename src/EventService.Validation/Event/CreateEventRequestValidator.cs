using System;
using System.Linq;
using FluentValidation;
using LT.DigitalOffice.EventService.Broker.Requests.Interfaces;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Enums;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Event;
using LT.DigitalOffice.EventService.Validation.Category.Interfaces;
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
    ICategoryRepository categoryRepository,
    ICreateCategoryRequestValidator categoryValidator)
  {
    RuleFor(ev => ev.Name)
      .MaximumLength(150)
      .WithMessage("Name should not exceed maximum length of 150 symbols");

    When(ev => !string.IsNullOrWhiteSpace(ev.Description), () =>
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

    When(ev => ev.EndDate.HasValue, () =>
    {
      RuleFor(ev => ev)
        .Must(ev => ev.EndDate > ev.Date)
        .WithMessage("The end date must be later than the event date.");
    });

    RuleLevelCascadeMode = CascadeMode.Stop;
    RuleFor(ev => ev.Users)
      .NotEmpty()
      .WithMessage("User list must not be empty.")
      .Must((ev, users) =>
        users.Select(user => user.UserId).Contains(contextAccessor.HttpContext.GetUserId()))
      .WithMessage("Event organizer must be in list of participants.")
      .MustAsync(async (users, _) =>
        await userService.CheckUsersExistenceAsync(users.Select(userRequest => userRequest.UserId).ToList()))
      .WithMessage("Some users doesn't exist.");

    When(ev => ev.Access == AccessType.Closed, () =>
    {
      RuleFor(ev => ev.Users)
        .Must(users => users.Count > 1)
        .WithMessage("There should be at least one invited user in closed event");
    });

    RuleFor(ev => ev.Users)
      .Must((ev, users) => users.All(user => user.NotifyAtUtc is null || (user.NotifyAtUtc > DateTime.UtcNow && user.NotifyAtUtc < ev.Date)))
      .WithMessage("Some notification time is not valid, notification time mustn't be earlier than now or later than date of the event");

    When(ev => !ev.CategoriesRequests.IsNullOrEmpty(),
        () =>
          RuleForEach(request => request.CategoriesRequests)
            .SetValidator(categoryValidator));

    When(ev => !ev.CategoriesIds.IsNullOrEmpty() || !ev.CategoriesRequests.IsNullOrEmpty(), () =>
    {
      When(ev => !ev.CategoriesIds.IsNullOrEmpty(), () =>
      {
        RuleFor(ev => ev.CategoriesIds)
        .MustAsync((categories, _) => categoryRepository.DoExistAllAsync(categories))
        .WithMessage("Some of categories in the list doesn't exist.");
      });

      RuleFor(ev => ev)
        .Must((ev) =>
        {
          int countCategories = 0;

          if (!ev.CategoriesRequests.IsNullOrEmpty())
          {
            if (!ev.CategoriesIds.IsNullOrEmpty())
            {
              countCategories = ev.CategoriesIds.Count();
            }

            countCategories = countCategories + ev.CategoriesRequests.Count();
          }
          else
          {
            countCategories = ev.CategoriesIds.Count();
          };

          return countCategories < 2;
        })
        .WithMessage("Count of categories to event must be no more than 1.");
    });
  }
}
