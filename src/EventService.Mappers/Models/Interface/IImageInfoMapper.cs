using LT.DigitalOffice.EventService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Models.Image;

namespace LT.DigitalOffice.EventService.Mappers.Models.Interface;

[AutoInject]
public interface IImageInfoMapper
{
  ImageInfo Map(ImageData image);
}
