using System.Collections.Generic;
using System.Linq;
using DigitalOffice.Models.Broker.Models.User;
using LT.DigitalOffice.EventService.Mappers.Models.Interface;
using LT.DigitalOffice.EventService.Mappers.Models.Interfaces;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Models;
using LT.DigitalOffice.EventService.Models.Dto.Responses.Event;
using LT.DigitalOffice.Models.Broker.Models.File;

namespace LT.DigitalOffice.EventService.Mappers.Models;

public class EventResponseMapper : IEventResponseMapper
{
  private readonly ICategoryInfoMapper _categoryInfoMapper;
  private readonly IUserInfoMapper _userInfoMapper;
  private readonly IFileInfoMapper _fileInfoMapper;

  public EventResponseMapper(
    ICategoryInfoMapper categoryInfoMapper,
    IUserInfoMapper userInfoMapper,
    IFileInfoMapper fileInfoMapper)
  {
    _categoryInfoMapper = categoryInfoMapper;
    _userInfoMapper = userInfoMapper;
    _fileInfoMapper = fileInfoMapper;
  }

  public EventResponse Map(
    DbEvent dbEvent,
    List<UserData> usersData,
    List<ImageInfo> images,
    List<FileCharacteristicsData> files,
    List<CommentInfo> comments)
  {
    return dbEvent is null
      ? null
      : new EventResponse
      {
        Id = dbEvent.Id,
        Name = dbEvent.Name,
        Address = dbEvent.Address,
        Description = dbEvent.Description,
        Date = dbEvent.Date,
        Format = dbEvent.Format,
        Access = dbEvent.Access,
        CreatedAtUtc = dbEvent.CreatedAtUtc,
        EventCategories = dbEvent.EventsCategories.Any()
          ? dbEvent.EventsCategories?.Select(ec => _categoryInfoMapper.Map(ec.Category)).ToList()
          : null,
        EventUsers = _userInfoMapper.Map(usersData),
        EventImages = images,
        EventFiles = files?.ConvertAll(_fileInfoMapper.Map),
        EventComments = comments
      };
  }
}
