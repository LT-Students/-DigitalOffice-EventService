using System.Collections.Generic;
using FluentValidation;
using LT.DigitalOffice.EventService.Broker.Requests.Interfaces;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventComment;
using LT.DigitalOffice.EventService.Validation.EventComment.Interfaces;

namespace LT.DigitalOffice.EventService.Validation.EventComment;

public class CreateEventCommentRequestValidator : AbstractValidator<CreateEventCommentRequest>, ICreateEventCommentRequestValidator
{
  public CreateEventCommentRequestValidator(
    IEventRepository eventRepository,
    IEventCommentRepository commentRepository,
    IUserService userService)
  {
    RuleFor(x => x.EventId)
      .Cascade(CascadeMode.Stop)
      .NotEmpty()
      .WithMessage("Event id must be specified.")
      .MustAsync(async (eventId, _) => await eventRepository.DoesExistAsync(eventId))
      .WithMessage("This event doesn't exist.");

    When(x => x.ParentId.HasValue, () => 
    {
      RuleFor(x => x.ParentId)
        .MustAsync(async (parentId, _) => await commentRepository.DoesExistAsync(parentId.Value))
        .WithMessage("This parent id doesn't exist.");
    });

    RuleFor(x => x.UserId)
      .Cascade(CascadeMode.Stop)
      .NotEmpty()
      .WithMessage("User id must not be empty.") 
      .MustAsync(async (userId, _) => await userService.CheckUserExistenceAsync(userId, new List<string>()))
      .WithMessage("That user doesn't exist.");
  }
}
