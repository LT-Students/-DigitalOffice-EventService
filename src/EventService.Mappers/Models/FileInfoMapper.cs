using LT.DigitalOffice.EventService.Mappers.Models.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Models;
using LT.DigitalOffice.Models.Broker.Models.File;

namespace LT.DigitalOffice.EventService.Mappers.Models;

public class FileInfoMapper : IFileInfoMapper
{
  public FileInfo Map(FileCharacteristicsData file)
  {
    if (file is null)
    {
      return null;
    }

    return new FileInfo
    {
      Id = file.Id,
      Name = file.Name,
      Extension = file.Extension,
      Size = file.Size,
      CreatedAtUtc = file.CreatedAtUtc
    };
  }
}
