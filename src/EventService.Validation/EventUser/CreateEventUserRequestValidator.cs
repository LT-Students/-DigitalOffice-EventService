using System;
using System.Collections.Generic;
using FluentValidation;
using LT.DigitalOffice.EventService.Broker.Requests.Interfaces;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Enums;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventsUsers;
using LT.DigitalOffice.EventService.Validation.EventUser.Interfaces;

namespace LT.DigitalOffice.EventService.Validation.EventUser
{
  public class CreateEventUserRequestValidator : AbstractValidator<CreateEventUserRequest>,
    ICreateEventUserRequestValidator
  {
    public CreateEventUserRequestValidator(IEventUserRepository eventUserRepository, IUserService userService,
      IEventRepository eventRepository)
    {
      RuleFor(request => request.UserId)
        .Cascade(CascadeMode.Stop)
        .MustAsync(async (userId, _) =>
          (await userService.CheckUsersExistenceAsync(new List<Guid> { userId }))?.Count == 1)
        .WithMessage("This user doesn't exist");

      RuleFor(request => request.EventId)
        .Cascade(CascadeMode.Stop)
        .MustAsync(async (eventId, _) => await eventRepository.IsEventExist(eventId))
        .WithMessage("This event doesn't exist");

      RuleFor(request => request)
        .Cascade(CascadeMode.Stop)
        .MustAsync(async (x, _) => !await eventUserRepository.IsUserAddedToEventAsync(x.UserId, x.EventId))
        .WithMessage("User is already added to event");

      WhenAsync(async (request, _) => request.UserStatus != EventUserStatus.Participant &&
                                      (await eventRepository.GetAsync(request.EventId)).Access == AccessType.Opened,
        () =>
        {
          RuleFor(request => request.UserStatus)
            .Cascade(CascadeMode.Stop)
            .Must(status => status != EventUserStatus.Discarded)
            .WithMessage("You can't add user with discarded status")
            .Must(status => status != EventUserStatus.Refused)
            .WithMessage("You can't add user with refused status")
            .Must(status => status != EventUserStatus.Invited)
            .WithMessage("You can't add user with invited status, choose participant status");
        });

      WhenAsync(async (request, _) => request.UserStatus != EventUserStatus.Invited &&
                                      (await eventRepository.GetAsync(request.EventId)).Access == AccessType.Closed,
        () =>
        {
          RuleFor(request => request.UserStatus)
            .Cascade(CascadeMode.Stop)
            .Must(status => status != EventUserStatus.Discarded)
            .WithMessage("You can't add user with discarded status")
            .Must(status => status != EventUserStatus.Refused)
            .WithMessage("You can't add user with refused status")
            .Must(status => status != EventUserStatus.Participant)
            .WithMessage("You must invite user to closed event");
        });

      When(request => request.NotifyAtUtc is not null, () =>
      {
        RuleFor(request => request)
          .Must(e => e.NotifyAtUtc > DateTime.UtcNow)
          .WithMessage(
            "Notification time is earlier than now")
          .MustAsync(async (e, _) => e.NotifyAtUtc < (await eventRepository.GetAsync(e.EventId)).Date)
          .WithMessage("Notification time can't be later than event date");
      });
    }
  }
}
