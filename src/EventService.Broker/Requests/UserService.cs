using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Broker.Requests.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.Helpers;
using LT.DigitalOffice.Models.Broker.Common;
using MassTransit;

namespace LT.DigitalOffice.EventService.Broker.Requests;

public class UserService : IUserService
{
  private readonly IRequestClient<ICheckUsersExistence> _checkUsersExistence;

  public UserService(IRequestClient<ICheckUsersExistence> checkUsersExistence)
  {
    _checkUsersExistence = checkUsersExistence;
  }

  public async Task<List<Guid>> CheckUsersExistenceAsync(List<Guid> usersIds, List<string> errors = null)
  {
    return (await RequestHandler.ProcessRequest<ICheckUsersExistence, ICheckUsersExistence>(
        _checkUsersExistence,
        ICheckUsersExistence.CreateObj(usersIds),
        errors))
      ?.UserIds;

  }
}

