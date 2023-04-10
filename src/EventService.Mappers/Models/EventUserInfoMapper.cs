using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.EventService.Mappers.Models.Interface;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Models;

namespace LT.DigitalOffice.EventService.Mappers.Models;

public class EventUserInfoMapper : IEventUserInfoMapper
{
  public List<EventUserInfo> Map(List<UserInfo> userInfos, List<DbEventUser> eventUsers)
  {
    return eventUsers?.Select(eu => new EventUserInfo
    {
      Id = eu.Id,
      Status = eu.Status.ToString(),
      NotifyAtUtc = eu.NotifyAtUtc,
      UserInfo = userInfos.Where(u => u.UserId == eu.UserId).ToList(),
    }).ToList();
  }
}
