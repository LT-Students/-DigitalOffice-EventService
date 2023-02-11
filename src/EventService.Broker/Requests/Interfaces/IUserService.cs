using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Models;

namespace LT.DigitalOffice.EventService.Broker.Requests.Interfaces;

[AutoInject]
public interface IUserService
{
  public Task<List<Guid>> CheckUsersExistenceAsync(List<Guid> usersIds, List<string> errors = null);
  public Task<List<UserData>> GetUsersDataAsync(List<Guid> users);
}

