using LT.DigitalOffice.EventService.Mappers.Models.Interface;
using LT.DigitalOffice.EventService.Models.Dto.Models;
using LT.DigitalOffice.Models.Broker.Models.Image;

namespace LT.DigitalOffice.EventService.Mappers.Models;

public class ImageInfoMapper : IImageInfoMapper
{
  public ImageInfo Map(ImageData image)
  {
    return image is null
      ? default
      : new ImageInfo
      {
        Id = image.ImageId,
        ParentId = image.ParentId,
        Content = image.Content,
        Extension = image.Extension,
        Name = image.Name
      };
  }
}
