using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Business.Commands.UserBirthday.Interfaces;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Mappers.Models.Interface;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Models;
using LT.DigitalOffice.EventService.Models.Dto.Requests.UserBirthday;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.EventService.Business.Commands.UserBirthday
{
  public class FindUserBirthdayCommand : IFindUserBirthdayCommand
  {
    private readonly IUserBirthdayRepository _userBirthdayRepository;
    private readonly IUserBirthdayInfoMapper _userBirthdayInfoMapper;

    public FindUserBirthdayCommand(
      IUserBirthdayRepository userBirthdayRepository,
      IUserBirthdayInfoMapper userBirthdayInfoMapper)
    {
      _userBirthdayRepository = userBirthdayRepository;
      _userBirthdayInfoMapper = userBirthdayInfoMapper;
    }

    public async Task<FindResultResponse<UserBirthdayInfo>> ExecuteAsync(
      FindUsersBirthdaysFilter filter,
      CancellationToken cancellationToken = default)
    {
      List<DbUserBirthday> usersBirthdays = await _userBirthdayRepository.FindAsync(cancellationToken);

      List<UserBirthdayInfo> usersBirthdaysInfo = new();

      if (filter.StartTime.Year == filter.EndTime.Year)
      {
        usersBirthdaysInfo = usersBirthdays.Where(ub =>
            (ub.DateOfBirth.Month > filter.StartTime.Month || (ub.DateOfBirth.Month == filter.StartTime.Month && ub.DateOfBirth.Day >= filter.StartTime.Day)) &&
            (ub.DateOfBirth.Month < filter.EndTime.Month || (ub.DateOfBirth.Month == filter.EndTime.Month && ub.DateOfBirth.Day <= filter.EndTime.Day)))
          .Select(ub => _userBirthdayInfoMapper.Map(ub, new DateTime(
            filter.StartTime.Year,
            ub.DateOfBirth.Month,
            ub.DateOfBirth.Day))).ToList();
      }
      else
      {
        for (int i = filter.StartTime.Year; i <= filter.EndTime.Year; i++)
        {
          if (i == filter.StartTime.Year)
          {
            usersBirthdaysInfo.AddRange(usersBirthdays.Where(ub =>
                ub.DateOfBirth.Month > filter.StartTime.Month || (ub.DateOfBirth.Month == filter.StartTime.Month && ub.DateOfBirth.Day >= filter.StartTime.Day))
              .Select(ub => _userBirthdayInfoMapper.Map(ub, new DateTime(i, ub.DateOfBirth.Month, ub.DateOfBirth.Day))));

            continue;
          }
          else if (i == filter.EndTime.Year)
          {
            usersBirthdaysInfo.AddRange(usersBirthdays.Where(ub =>
                ub.DateOfBirth.Month < filter.EndTime.Month || (ub.DateOfBirth.Month == filter.EndTime.Month && ub.DateOfBirth.Day <= filter.EndTime.Day))
              .Select(ub => _userBirthdayInfoMapper.Map(ub, new DateTime(i, ub.DateOfBirth.Month, ub.DateOfBirth.Day))));

            continue;
          }

          usersBirthdaysInfo.AddRange(
            usersBirthdays.Select(ub => _userBirthdayInfoMapper.Map(ub, new DateTime(i, ub.DateOfBirth.Month, ub.DateOfBirth.Day))));
        }
      }

      return new FindResultResponse<UserBirthdayInfo>(
        totalCount: usersBirthdaysInfo.Count,
        body: usersBirthdaysInfo.Skip(filter.SkipCount).Take(filter.TakeCount).ToList());
    }
  }
}
