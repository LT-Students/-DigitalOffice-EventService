﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigitalOffice.Models.Broker.Models.User;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Broker.Requests.Interfaces;

[AutoInject]
public interface IUserService
{
  Task<bool> CheckUsersExistenceAsync(List<Guid> usersIds, List<string> errors = null);
  Task<List<UserData>> GetUsersDataAsync(List<Guid> users);
  Task<(List<UserData> usersData, int totalCount)> FilteredUsersDataAsync(
    List<Guid> usersIds,
    int skipCount = 0,
    int takeCount = 1,
    bool? ascendingSort = null,
    string fullNameIncludeSubstring = null);
  Task<List<UserBirthday>> GetUsersBirthdaysAsync();
}
