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

  public UserService(
    IRequestClient<ICheckUsersExistence> rcCheckUserExistence, 
    IRequestClient<IGetUsersDataRequest> rcGetUsersData)
  {
    _rcCheckUserExistence = rcCheckUserExistence;
    _rcGetUsersData = rcGetUsersData;
  }

  public async Task<List<Guid>> CheckUsersExistenceAsync(List<Guid> usersIds, List<string> errors = null)
  {
    if (usersIds is null || !usersIds.Any())
    {
      return null;
    }
    return (await RequestHandler.ProcessRequest<ICheckUsersExistence, ICheckUsersExistence>(
        _rcCheckUserExistence,
        ICheckUsersExistence.CreateObj(usersIds),
        errors))
      ?.UserIds;
  }

  public async Task<List<UserData>> GetUsersDataAsync(List<Guid> usersIds)
  {
    if (usersIds is null || !usersIds.Any())
    {
      return null;
    }
    object request = IGetUsersDataRequest.CreateObj(usersIds);

    return (await _rcGetUsersData.ProcessRequest<IGetUsersDataRequest, IGetUsersDataResponse>(request))?.UsersData;
  }  
}

