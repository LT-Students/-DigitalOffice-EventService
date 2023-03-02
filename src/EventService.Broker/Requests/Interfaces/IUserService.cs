using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.Models.Broker.Responses.User;

namespace LT.DigitalOffice.EventService.Broker.Requests.Interfaces;

[AutoInject]
public interface IUserService
{
  Task<bool> CheckUsersExistenceAsync(List<Guid> usersIds, List<string> errors = null);
  Task<List<UserData>> GetUsersDataAsync(List<Guid> users);
  Task<IFilteredUsersDataResponse> FilteredUsersDataAsync(
    List<Guid> usersIds,
    int skipCount = 0,
    int takeCount = 1,
    bool? ascendingSort = null,
    string fullNameIncludeSubstring = null);
}
