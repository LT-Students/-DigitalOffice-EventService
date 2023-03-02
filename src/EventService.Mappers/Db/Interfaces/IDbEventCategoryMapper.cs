using System.Collections.Generic;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventCategory;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Mappers.Db.Interfaces;

[AutoInject]
public interface IDbEventCategoryMapper
{
  List<DbEventCategory> Map(CreateEventCategoryRequest request);
}
