using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DigitalOffice.Models.Broker.Models.Image;
using FluentValidation.Results;
using LT.DigitalOffice.EventService.Broker.Requests.Interfaces;
using LT.DigitalOffice.EventService.Business.Commands.Image.Interfaces;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Mappers.Db.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Image;
using LT.DigitalOffice.EventService.Validation.Image.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.EventService.Business.Commands.Image;

public class CreateImageCommand : ICreateImageCommand
{
  private readonly IEventImageRepository _repository;
  private readonly IAccessValidator _accessValidator;
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IDbEventImageMapper _dbEventImageMapper;
  private readonly ICreateImagesRequestValidator _validator;
  private readonly IResponseCreator _responseCreator;
  private readonly IImageService _imageService;

  private const int ResizeMaxValue = 1000;
  private const int ConditionalWidth = 4;
  private const int ConditionalHeight = 3;

  public CreateImageCommand(
    IEventImageRepository repository,
    IAccessValidator accessValidator,
    IHttpContextAccessor httpContextAccessor,
    IDbEventImageMapper dbEventImageMapper,
    ICreateImagesRequestValidator validator,
    IResponseCreator responseCreator,
    IImageService imageService)
  {
    _repository = repository;
    _accessValidator = accessValidator;
    _httpContextAccessor = httpContextAccessor;
    _dbEventImageMapper = dbEventImageMapper;
    _validator = validator;
    _responseCreator = responseCreator;
    _imageService = imageService;
  }

  public async Task<OperationResultResponse<List<Guid>>> ExecuteAsync(CreateImagesRequest request)
  {
    if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveProjects))
    {
      return _responseCreator.CreateFailureResponse<List<Guid>>(HttpStatusCode.Forbidden);
    }

    ValidationResult validationResult = await _validator.ValidateAsync(request);
    if (!validationResult.IsValid)
    {
      return _responseCreator.CreateFailureResponse<List<Guid>>(
        HttpStatusCode.BadRequest,
        validationResult.Errors.Select(x => x.ErrorMessage).ToList());
    }

    OperationResultResponse<List<Guid>> response = new();

    List<Guid> imagesIds = await _imageService.CreateImagesAsync(
      request.Images,
      new ResizeParameters(
        maxResizeValue: ResizeMaxValue,
        maxSizeCompress: null,
        previewParameters: new PreviewParameters(
          conditionalWidth: ConditionalWidth,
          conditionalHeight: ConditionalHeight,
          resizeMaxValue: null,
          maxSizeCompress: null)),
      response.Errors);

    if (response.Errors.Any())
    {
      _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

      return response;
    }

    response.Body = await _repository.CreateAsync(imagesIds.Select(imageId =>
      _dbEventImageMapper.Map(
        imageId: imageId,
        eventId: request.EventId))
      .ToList());

    _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;

    return response;
  }
}
