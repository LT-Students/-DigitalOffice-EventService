using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.EventService.Mappers.Db.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Models;
using LT.DigitalOffice.Models.Broker.Models;

namespace LT.DigitalOffice.EventService.Mappers.Db
{
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
}
