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

    public async Task UpdateUserBirthdayAsync(DbUserBirthday userBirthday)
    {
      DbUserBirthday birthdayInDb = await _provider.UsersBirthdays.FirstOrDefaultAsync(b => b.UserId == userBirthday.UserId);

      if (birthdayInDb is null && userBirthday.IsActive)
      {
        _provider.UsersBirthdays.Add(userBirthday);
      }
      else if (birthdayInDb is not null &&
        birthdayInDb.DateOfBirthday != userBirthday.DateOfBirthday &&
        userBirthday.IsActive)
      {
        birthdayInDb.DateOfBirthday = userBirthday.DateOfBirthday;
        birthdayInDb.ModifiedAtUtc = DateTime.UtcNow;
      }
      else if (birthdayInDb is not null && birthdayInDb.IsActive != userBirthday.IsActive)
      {
        birthdayInDb.IsActive = userBirthday.IsActive;
        birthdayInDb.ModifiedAtUtc = DateTime.UtcNow;
      }

      await _provider.SaveAsync();
    }
  }
}
