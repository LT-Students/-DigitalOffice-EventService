using System.Collections.Generic;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Mappers.Db.Interfaces
{
  [AutoInject]
  public interface IEventUserInfoMapper
  {
    List<EventUserInfo> Map(List<UserInfo> userInfos, List<DbEventUser> eventUsers);
  }
}
