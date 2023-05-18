using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Broker.Requests.Interfaces;
using LT.DigitalOffice.EventService.Business.Commands.File.Interfaces;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Mappers.Models.Interfaces;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Models;
using LT.DigitalOffice.EventService.Models.Dto.Requests.File;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Models.File;

namespace LT.DigitalOffice.EventService.Business.Commands.File;

public class FindFilesCommand : IFindFilesCommand
{
  private readonly IEventFileRepository _repository;
  private readonly IFileService _fileService;
  private readonly IFileInfoMapper _fileMapper;

  public FindFilesCommand(
    IEventFileRepository repository,
    IFileService fileService,
    IFileInfoMapper fileMapper)
  {
    _repository = repository;
    _fileService = fileService;
    _fileMapper = fileMapper;
  }

  public async Task<FindResultResponse<FileInfo>> ExecuteAsync(FindEventFilesFilter findFilter)
  {
    (List<DbEventFile> dbFiles, int totalCount) = await _repository.FindAsync(findFilter);

    List<FileCharacteristicsData> files = await _fileService.GetFilesCharacteristicsAsync(dbFiles?.ConvertAll(file => file.FileId));

    return new FindResultResponse<FileInfo>(
      body: files?.ConvertAll(_fileMapper.Map),
      totalCount: totalCount);
  }
}
