using LT.DigitalOffice.EventService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Models.File;

namespace LT.DigitalOffice.EventService.Mappers.Models.Interfaces;

[AutoInject]
public interface IFileInfoMapper
{
  FileInfo Map(FileCharacteristicsData file);
}
