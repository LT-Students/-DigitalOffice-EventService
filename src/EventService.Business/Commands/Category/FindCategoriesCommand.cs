using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Business.Commands.Category.Interfaces;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Mappers.Models.Interface;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Models;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Category;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.EventService.Business.Commands.Category;

public class FindCategoriesCommand : IFindCategoriesCommand
{
  private readonly ICategoryRepository _categoryRepository;
  private readonly ICategoryInfoMapper _mapper;

  public FindCategoriesCommand(
    ICategoryRepository categoryRepository,
    ICategoryInfoMapper mapper)
  {
    _categoryRepository = categoryRepository;
    _mapper = mapper;
  }
  
  public async Task<FindResultResponse<CategoryInfo>> ExecuteAsync(
    FindCategoriesFilter filter, 
    CancellationToken cancellationToken = default)
  {
    (List<DbCategory> dbCategories, int totalCount) = await _categoryRepository.FindAsync(filter, cancellationToken);

    return new FindResultResponse<CategoryInfo>(
      body: dbCategories.ConvertAll(_mapper.Map),
      totalCount: totalCount);
  }
}

