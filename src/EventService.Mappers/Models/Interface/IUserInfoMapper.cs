using System.Collections.Generic;
using DigitalOffice.Models.Broker.Models.User;
using LT.DigitalOffice.EventService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Mappers.Models.Interface;

[AutoInject]
public interface IUserInfoMapper
{
  List<UserInfo> Map(List<UserData> usersData);
}
