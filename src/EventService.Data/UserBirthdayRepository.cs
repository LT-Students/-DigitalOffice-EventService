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

    public async Task UpdateUserBirthdayAsync(Guid userId, DateTime? dateOfBirth)
    {
      DbUserBirthday existingBirthday = await _provider.UsersBirthdays.FirstOrDefaultAsync(b => b.UserId == userId);

      if (existingBirthday is null && dateOfBirth.HasValue)
      {
        _provider.UsersBirthdays.Add(new DbUserBirthday
          {
            UserId = userId,
            DateOfBirth = dateOfBirth.Value,
            IsActive = true,
            CreatedAtUtc = DateTime.UtcNow
        });
      }
      else if (existingBirthday is not null &&
        existingBirthday.DateOfBirth != dateOfBirth &&
        dateOfBirth.HasValue)
      {
        existingBirthday.DateOfBirth = dateOfBirth.Value;
        existingBirthday.ModifiedAtUtc = DateTime.UtcNow;
      }
      else if (existingBirthday is not null && existingBirthday.IsActive != dateOfBirth.HasValue)
      {
        existingBirthday.IsActive = dateOfBirth.HasValue;
        existingBirthday.ModifiedAtUtc = DateTime.UtcNow;
      }

      await _provider.SaveAsync();
    }
  }
}
