using System;
using System.Collections.Generic;
using FluentValidation;
using LT.DigitalOffice.EventService.Broker.Requests.Interfaces;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventComment;
using LT.DigitalOffice.EventService.Validation.EventComment.Interfaces;
using LT.DigitalOffice.EventService.Validation.Image.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace LT.DigitalOffice.EventService.Validation.EventComment;

public class CreateEventCommentRequestValidator : AbstractValidator<CreateEventCommentRequest>, ICreateEventCommentRequestValidator
{
  public CreateEventCommentRequestValidator(
    IEventRepository eventRepository,
    IEventCommentRepository commentRepository,
    IUserService userService,
    IImageValidator imageValidator)
  {
    RuleLevelCascadeMode = CascadeMode.Stop;

    RuleFor(x => x.Content)
      .MaximumLength(300)
      .WithMessage("Content is too long.");

    RuleFor(x => x.EventId)
      .NotEmpty()
      .WithMessage("Event id must be specified.")
      .MustAsync((eventId, _) => eventRepository.DoesExistAsync(eventId, true))
      .WithMessage("This event doesn't exist.");

    When(x => x.ParentId.HasValue, () => 
    {
      RuleFor(x => x.ParentId)
        .MustAsync(async (parentId, _) => await commentRepository.DoesExistAsync(parentId.Value))
        .WithMessage("This parent id doesn't exist.");
    });

    RuleFor(x => x.UserId)
      .NotEmpty()
      .WithMessage("User id must not be empty.") 
      .MustAsync(async (userId, _) => await userService.CheckUsersExistenceAsync(new List<Guid>() { userId }, new List<string>()))
      .WithMessage("That user doesn't exist.");

    When(x => !x.CommentImages.IsNullOrEmpty(),
        () =>
          RuleForEach(request => request.CommentImages)
            .SetValidator(imageValidator));
  }
}
