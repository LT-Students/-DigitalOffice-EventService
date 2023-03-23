using System;
using System.Collections.Generic;
using FluentValidation;
using LT.DigitalOffice.Kernel.Validators;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventUser;
using LT.DigitalOffice.EventService.Validation.EventUser.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Enums;
using LT.DigitalOffice.EventService.Data.Interfaces;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.JsonPatch;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using Microsoft.AspNetCore.Http;
using LT.DigitalOffice.Kernel.Extensions;

namespace LT.DigitalOffice.EventService.Validation.EventUser;

public class EditEventUserRequestValidator : ExtendedEditRequestValidator<Guid, EditEventUserRequest>, IEditEventUserRequestValidator
{
  private readonly IEventUserRepository _eventUserRepository;
  private readonly IEventRepository _eventRepository;
  private readonly IAccessValidator _accessValidator;
  private readonly IHttpContextAccessor _httpContextAccessor;

  private void HandleInternalPropertyValidationAsync(
    Operation<EditEventUserRequest> requestedOperation,
    DbEvent dbEvent,
    DbEventUser dbEventUser,
    bool isAddEditRemoveUsers,
    bool isUser,
    ValidationContext<(Guid,JsonPatchDocument<EditEventUserRequest>)> context)
  {
    Context = context;
    RequestedOperation = requestedOperation;

    #region paths

    AddСorrectPaths(
      new List<string>
      {
        nameof(EditEventUserRequest.Status),
        nameof(EditEventUserRequest.NotifyAtUtc),
      });

    AddСorrectOperations(nameof(EditEventUserRequest.Status), new List<OperationType> { OperationType.Replace });
    AddСorrectOperations(nameof(EditEventUserRequest.NotifyAtUtc), new List<OperationType> { OperationType.Replace });

    #endregion

    #region Status

    AddFailureForPropertyIf(
      nameof(EditEventUserRequest.Status),
      x => x == OperationType.Replace,
      new Dictionary<Func<Operation<EditEventUserRequest>, bool>, string>
      {
        {
        x => Enum.TryParse(typeof(EventUserStatus), x.value?.ToString(), out _) ?
        ((x.value.ToString().Trim()) == "Participant" &&
        (((dbEventUser.Status == EventUserStatus.Refused || dbEventUser.Status == EventUserStatus.Invited) && isUser)||
        (dbEventUser.Status == EventUserStatus.Discarded && isAddEditRemoveUsers)))||
        ((x.value.ToString().Trim()) == "Refused" &&
        ((dbEventUser.Status == EventUserStatus.Participant || dbEventUser.Status == EventUserStatus.Invited) && isUser))||
        ((x.value.ToString().Trim()) == "Discarded"  &&
        (dbEventUser.Status == EventUserStatus.Participant && isAddEditRemoveUsers))
        : false,
         "Uncorrect user status"
        }
      });

    #endregion

    #region NotifyAtUtc

    AddFailureForPropertyIf(
      nameof(EditEventUserRequest.NotifyAtUtc),
      x => x == OperationType.Replace,
      new Dictionary<Func<Operation<EditEventUserRequest>, bool>, string>
      {
        {
          x => string.IsNullOrEmpty(x.value?.ToString())? true :
          (DateTime.TryParse(x.value.ToString().Trim(), out DateTime date) &&
          DateTime.Parse(x.value.ToString().Trim()) < dbEvent.Date &&
          DateTime.Parse(x.value.ToString().Trim()) > DateTime.UtcNow),
          "Uncorrect notify"
        },
      });

    #endregion
  }

  public EditEventUserRequestValidator(
    IEventUserRepository eventUserRepository,
    IEventRepository eventRepository,
    IAccessValidator accessValidator,
    IHttpContextAccessor httpContextAccessor)
  {
    _eventUserRepository = eventUserRepository;
    _eventRepository = eventRepository;
    _accessValidator = accessValidator;
    _httpContextAccessor = httpContextAccessor;

    RuleFor(eventUserId => eventUserId.Item1)
      .MustAsync(async (eventUserId, _) => await _eventUserRepository.DoesExistAsync(eventUserId))
      .WithMessage("This Id doesn't exist.");

    RuleFor(paths => paths)
      .CustomAsync(async (paths, context, _) =>
      {
        DbEventUser dbEventUser = await _eventUserRepository.GetAsync(paths.Item1);
        DbEvent dbEvent = await _eventRepository.GetAsync(dbEventUser.EventId);
        bool isAddEditRemoveUsers = await _accessValidator.HasRightsAsync(Rights.AddEditRemoveUsers);
        bool isUser = _httpContextAccessor.HttpContext.GetUserId() == dbEventUser.UserId;

        foreach (var op in paths.Item2.Operations)
        {
          HandleInternalPropertyValidationAsync(
            requestedOperation: op,
            dbEvent: dbEvent,
            dbEventUser: dbEventUser,
            isAddEditRemoveUsers: isAddEditRemoveUsers,
            isUser: isUser,
            context: context);
        }
      });
  }
}
