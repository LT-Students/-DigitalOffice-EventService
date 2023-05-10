using System.Collections.Generic;
using LT.DigitalOffice.EventService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Models;

namespace LT.DigitalOffice.EventService.Mappers.Models.Interface;

[AutoInject]
public interface IUserInfoMapper
{
  List<UserInfo> Map(List<UserData> usersData);
}
