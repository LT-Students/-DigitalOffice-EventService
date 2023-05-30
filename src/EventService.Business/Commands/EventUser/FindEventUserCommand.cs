using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DigitalOffice.Models.Broker.Models.User;
using LT.DigitalOffice.EventService.Broker.Requests.Interfaces;
using LT.DigitalOffice.EventService.Business.Commands.EventUser.Interfaces;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Mappers.Models.Interface;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Models;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventUser.Filter;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.EventService.Business.Commands.EventUser;

public class FindEventUserCommand : IFindEventUserCommand
{
  private readonly IEventUserRepository _eventUserRepository;
  private readonly IEventRepository _eventRepository;
  private readonly IUserService _userService;
  private readonly IEventUserInfoMapper _eventUserInfoMapper;
  private readonly IUserInfoMapper _userInfoMapper;
  private readonly IResponseCreator _responseCreator;

  public FindEventUserCommand(
    IEventUserRepository eventUserRepository,
    IEventRepository eventRepository,
    IUserService userService,
    IEventUserInfoMapper eventUserInfoMapper,
    IUserInfoMapper userInfoMapper,
    IResponseCreator responseCreator)
  {
    _eventUserRepository = eventUserRepository;
    _eventRepository = eventRepository;
    _userService = userService;
    _eventUserInfoMapper = eventUserInfoMapper;
    _userInfoMapper = userInfoMapper;
    _responseCreator = responseCreator;
  }

  public async Task<FindResultResponse<EventUserInfo>> ExecuteAsync(
    Guid eventId,
    FindEventUsersFilter filter,
    CancellationToken cancellationToken)
  {
    if (!await _eventRepository.DoesExistAsync(eventId, true))
    {
      return _responseCreator.CreateFailureFindResponse<EventUserInfo>(HttpStatusCode.BadRequest);
    }

    List<DbEventUser> eventUsers =
      await _eventUserRepository.FindAsync(eventId: eventId, filter: filter, cancellationToken: cancellationToken);

    if (eventUsers is null || !eventUsers.Any())
    {
      return new();
    }

    (List<UserData> usersData, int totalCount) = await _userService.FilteredUsersDataAsync(
      usersIds:eventUsers.Select(e => e.UserId).ToList(),
      skipCount: filter.SkipCount,
      takeCount: filter.TakeCount,
      ascendingSort: filter.IsAscendingSort,
      fullNameIncludeSubstring: filter.UserFullNameIncludeSubstring);

    return new FindResultResponse<EventUserInfo>(
      totalCount: totalCount,
      body: _eventUserInfoMapper.Map(
        userInfos: _userInfoMapper.Map(usersData),
        eventUsers: eventUsers));
  }
}
