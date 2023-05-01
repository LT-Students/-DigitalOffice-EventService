using System;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Data.Provider;
using LT.DigitalOffice.EventService.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.EventService.Data
{
  public class UserBirthdayRepository : IUserBirthdayRepository
  {
    private readonly IDataProvider _provider;

    public UserBirthdayRepository(IDataProvider provider)
    {
      _provider = provider;
    }

    public async Task UpdateUserBirthdatAsync(DbUserBirthday userBirthday)
    {
      DbUserBirthday birthday = await _provider.UsersBirthdays.FirstOrDefaultAsync(b => b.UserId == userBirthday.UserId);

      if (birthday is null && userBirthday.IsActive)
      {
        _provider.UsersBirthdays.Add(userBirthday);
      }
      else if (birthday is not null &&
        birthday.DateOfBirthday != userBirthday.DateOfBirthday &&
        userBirthday.IsActive)
      {
        birthday.DateOfBirthday = userBirthday.DateOfBirthday;
        birthday.ModifiedAtUtc = DateTime.UtcNow;
      }
      else if (birthday is not null && userBirthday.IsActive)
      {
        birthday.IsActive = userBirthday.IsActive;
        birthday.ModifiedAtUtc = DateTime.UtcNow;
      }

      await _provider.SaveAsync();
    }
  }
}
