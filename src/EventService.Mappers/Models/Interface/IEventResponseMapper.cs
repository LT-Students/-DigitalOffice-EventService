using System.Collections.Generic;
using DigitalOffice.Models.Broker.Models.User;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Models;
using LT.DigitalOffice.EventService.Models.Dto.Responses.Event;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Models.File;
using LT.DigitalOffice.Models.Broker.Models.Image;

namespace LT.DigitalOffice.EventService.Mappers.Models.Interface;

[AutoInject]
public interface IEventResponseMapper
{
  EventResponse Map(
    DbEvent dbEvent,
    List<UserData> usersData,
    List<ImageInfo> images,
    List<FileCharacteristicsData> files,
    List<CommentInfo> comments);
}
