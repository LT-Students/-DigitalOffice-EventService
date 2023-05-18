using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Data.Provider;
using LT.DigitalOffice.EventService.Models.Db;

namespace LT.DigitalOffice.EventService.Data;

public class EventImageRepository : IEventImageRepository
{
  private readonly IDataProvider _provider;

  public EventImageRepository(
    IDataProvider provider)
  {
    _provider = provider;
  }

  public async Task<List<Guid>> CreateAsync(List<DbEventImage> images)
  {
    if (images == null)
    {
      return null;
    }

    _provider.EventImages.AddRange(images);
    await _provider.SaveAsync();

    return images.Select(x => x.ImageId).ToList();
  }

  public async Task<bool> RemoveAsync(List<Guid> imagesIds)
  {
    if (imagesIds == null)
    {
      return false;
    }

    IEnumerable<DbEventImage> images = _provider.EventImages
      .Where(x => imagesIds.Contains(x.ImageId));

    _provider.EventImages.RemoveRange(images);
    await _provider.SaveAsync();

    return true;
  }
}
