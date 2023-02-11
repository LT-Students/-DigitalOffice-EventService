using System;
using System.Collections.Generic;
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
  private readonly IRequestClient<ICheckUsersExistence> _checkUsersExistence;
  private readonly IRequestClient<IGetUsersDataRequest> _getUsersDataRequest;

  public UserService(
    IRequestClient<ICheckUsersExistence> checkUsersExistence, 
    IRequestClient<IGetUsersDataRequest> getUsersDataRequest)
  {
    _checkUsersExistence = checkUsersExistence;
    _getUsersDataRequest = getUsersDataRequest;
  }

  public async Task<List<Guid>> CheckUsersExistenceAsync(List<Guid> usersIds, List<string> errors = null)
  {
    return (await RequestHandler.ProcessRequest<ICheckUsersExistence, ICheckUsersExistence>(
        _checkUsersExistence,
        ICheckUsersExistence.CreateObj(usersIds),
        errors))
      ?.UserIds;

  }

  public async Task<List<UserData>> GetUsersDataAsync(List<Guid> users)
  {
    object request = IGetUsersDataRequest.CreateObj(users);

    return (await _getUsersDataRequest.ProcessRequest<IGetUsersDataRequest, IGetUsersDataResponse>(request))?.UsersData;
  }  
}

