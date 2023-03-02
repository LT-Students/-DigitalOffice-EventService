using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Broker.Requests.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.Helpers;
using LT.DigitalOffice.Models.Broker.Common;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.Models.Broker.Requests.User;
using LT.DigitalOffice.Models.Broker.Responses.User;
using MassTransit;

namespace LT.DigitalOffice.EventService.Broker.Requests;

public class UserService : IUserService
{
  private readonly IRequestClient<ICheckUsersExistence> _rcCheckUserExistence;
  private readonly IRequestClient<IGetUsersDataRequest> _rcGetUsersData;
  private readonly IRequestClient<IFilteredUsersDataRequest> _rcFilteredUsersData;

  public UserService(
    IRequestClient<ICheckUsersExistence> rcCheckUserExistence,
    IRequestClient<IGetUsersDataRequest> rcGetUsersData,
    IRequestClient<IFilteredUsersDataRequest> rcFilteredUsersData)
  {
    _rcCheckUserExistence = rcCheckUserExistence;
    _rcGetUsersData = rcGetUsersData;
    _rcFilteredUsersData = rcFilteredUsersData;
  }

  public async Task<bool> CheckUsersExistenceAsync(List<Guid> usersIds, List<string> errors = null)
  {
    if (usersIds is null || !usersIds.Any())
    {
      return false;
    }

    var existingUserIds = (await RequestHandler.ProcessRequest<ICheckUsersExistence, ICheckUsersExistence>(
        _rcCheckUserExistence,
        ICheckUsersExistence.CreateObj(usersIds),
        errors))
      ?.UserIds;
    return new HashSet<Guid>(usersIds.Distinct()).SetEquals(existingUserIds);
  }

  public async Task<List<UserData>> GetUsersDataAsync(List<Guid> usersIds)
  {
    if (usersIds is null || !usersIds.Any())
    {
      return null;
    }

    return (await _rcGetUsersData.ProcessRequest<IGetUsersDataRequest, IGetUsersDataResponse>(
      IGetUsersDataRequest.CreateObj(usersIds)))
      ?.UsersData;
  }

  public async Task<IFilteredUsersDataResponse> FilteredUsersDataAsync(
    List<Guid> usersIds, 
    int skipCount = 0, 
    int takeCount = 1, 
    bool? ascendingSort = null, 
    string fullNameIncludeSubstring = null)
  {
    return await _rcFilteredUsersData.ProcessRequest<IFilteredUsersDataRequest, IFilteredUsersDataResponse>(
      IFilteredUsersDataRequest.CreateObj(
        usersIds: usersIds, 
        skipCount: skipCount, 
        takeCount: takeCount, 
        ascendingSort: ascendingSort, 
        fullNameIncludeSubstring: fullNameIncludeSubstring));
  }
}
