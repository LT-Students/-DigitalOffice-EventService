using System;
using FluentValidation;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Event;
using LT.DigitalOffice.EventService.Validation.Event.Interfaces;

namespace LT.DigitalOffice.EventService.Validation.Event;

public class CreateEventRequestValidator : AbstractValidator<CreateEventRequest>, ICreateEventRequestValidator
{
  public CreateEventRequestValidator(
    IEventRepository repository)
  {
    RuleFor(ev => ev.Name)
      .MaximumLength(150)
      .WithMessage("Name should not exceed maximum length of 150 symbols");

    When(ev => !string.IsNullOrEmpty(ev.Description), 
      () =>
    {
      RuleFor(ev => ev.Description)
      .MaximumLength(500)
      .WithMessage("Description should not exceed maximum length of 500 symbols");
    });

    RuleFor(ev => ev.Address)
      .MaximumLength(150)
      .WithMessage("Address should not exceed maximum length of 400 symbols");

    RuleFor(ev => ev.Date)
      .Must(d => d >= DateTime.UtcNow)
      .WithMessage("The event date must be later than the date the event was created");
  }
}
