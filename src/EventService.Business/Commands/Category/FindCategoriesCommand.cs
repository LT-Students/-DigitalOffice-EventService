using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Business.Commands.Category.Interfaces;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Mappers.Models.Interface;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Models;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Category;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Kernel.Validators.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.EventService.Business.Commands.Category;

public class FindCategoriesCommand : IFindCategoriesCommand
{
  private readonly ICategoryRepository _categoryRepository;
  private readonly IBaseFindFilterValidator _filterValidator;
  private readonly IHttpContextAccessor _contextAccessor;
  private readonly ICategoryInfoMapper _mapper;
  private readonly IResponseCreator _responseCreator;

  public FindCategoriesCommand(
  ICategoryRepository categoryRepository,
  IBaseFindFilterValidator filterValidator,
  IHttpContextAccessor contextAccessor,
  ICategoryInfoMapper mapper,
  IResponseCreator responseCreator)
  {
    _categoryRepository = categoryRepository;
    _filterValidator = filterValidator;
    _contextAccessor = contextAccessor;
    _mapper = mapper;
    _responseCreator = responseCreator;
  }
  
  
  public async Task<FindResultResponse<CategoryInfo>> ExecuteAsync(
    FindCategoriesFilter filter, 
    CancellationToken cancellationToken = default)
  {
    if (!_filterValidator.ValidateCustom(filter, out List<string> errors))
    {
      return _responseCreator.CreateFailureFindResponse<CategoryInfo>(
        HttpStatusCode.BadRequest, errors);
    }
    
    (List<DbCategory> dbCategories, int totalCount) = await _categoryRepository.FindAsync(filter, cancellationToken);

    return new FindResultResponse<CategoryInfo>(
      body: dbCategories.ConvertAll(_mapper.Map),
      totalCount: totalCount);
  }
}

