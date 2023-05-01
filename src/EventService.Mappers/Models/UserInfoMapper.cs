using System.Collections.Generic;
using System.Linq;
using DigitalOffice.Models.Broker.Models.User;
using LT.DigitalOffice.EventService.Mappers.Models.Interface;
using LT.DigitalOffice.EventService.Models.Dto.Models;

namespace LT.DigitalOffice.EventService.Mappers.Models;

public class UserInfoMapper : IUserInfoMapper
{
  public List<UserInfo> Map(List<UserData> usersData)
  {
    return usersData?.Select(u => new UserInfo
    {
      UserId = u.Id,
      FirstName = u.FirstName,
      LastName = u.LastName,
      MiddleName = u.MiddleName,
      ImageId = u.ImageId
    }).ToList();
  }
}
