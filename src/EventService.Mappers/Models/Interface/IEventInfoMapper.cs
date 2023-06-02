using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Mappers.Models.Interface;

[AutoInject]
public interface IEventInfoMapper
{
  EventInfo Map(DbEvent dbEvent);
}
