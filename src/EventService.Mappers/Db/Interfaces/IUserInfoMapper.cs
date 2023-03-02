using System.Collections.Generic;
using LT.DigitalOffice.EventService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Responses.User;

namespace LT.DigitalOffice.EventService.Mappers.Db.Interfaces
{
  [AutoInject]
  public interface IUserInfoMapper
  {
    List<UserInfo> Map(IFilteredUsersDataResponse filteredUsersData);
  }
}
