using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Data.Provider;
using LT.DigitalOffice.EventService.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.EventService.Data;

public class ImageRepository : IImageRepository
{
  private readonly IDataProvider _provider;

  public ImageRepository(
    IDataProvider provider)
  {
    _provider = provider;
  }

  public async Task<List<Guid>> CreateAsync(List<DbImage> images)
  {
    if (images is null || !images.Any())
    {
      return null;
    }

    _provider.Images.AddRange(images);
    await _provider.SaveAsync();

    return images.ConvertAll(x => x.ImageId);
  }

  public async Task<bool> RemoveAsync(List<Guid> imagesIds)
  {
    if (imagesIds is null || !imagesIds.Any())
    {
      return false;
    }

    List<DbImage> images = await _provider.Images
      .Where(x => imagesIds.Contains(x.ImageId)).ToListAsync();

    _provider.Images.RemoveRange(images);

    await _provider.SaveAsync();

    return true;
  }
}
