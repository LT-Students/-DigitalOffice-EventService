using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DigitalOffice.Models.Broker.Models.Image;
using FluentValidation.Results;
using LT.DigitalOffice.EventService.Broker.Requests.Interfaces;
using LT.DigitalOffice.EventService.Business.Commands.EventComment.Interfaces;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Mappers.Db.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventComment;
using LT.DigitalOffice.EventService.Validation.EventComment.Interfaces;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.EventService.Business.Commands.EventComment;

public class CreateEventCommentCommand : ICreateEventCommentCommand
{
  private readonly IEventCommentRepository _repository;
  private readonly ICreateEventCommentRequestValidator _validator;
  private readonly IDbEventCommentMapper _mapper;
  private readonly IResponseCreator _responseCreator;
  private readonly IHttpContextAccessor _contextAccessor;
  private readonly IImageService _imageService;

  private const int ResizeMaxValue = 1000;
  private const int ConditionalWidth = 4;
  private const int ConditionalHeight = 3;

  public CreateEventCommentCommand(
    IEventCommentRepository repository,
    ICreateEventCommentRequestValidator validator,
    IDbEventCommentMapper mapper,
    IResponseCreator responseCreator,
    IHttpContextAccessor contextAccessor,
    IImageService imageService)
  {
    _repository = repository;
    _mapper = mapper;
    _validator = validator;
    _responseCreator = responseCreator;
    _contextAccessor = contextAccessor;
    _imageService = imageService;
  }

  public async Task<OperationResultResponse<Guid?>> ExecuteAsync(CreateEventCommentRequest request)
  {
    ValidationResult validationResult = await _validator.ValidateAsync(request);
    if (!validationResult.IsValid)
    {
      return _responseCreator.CreateFailureResponse<Guid?>(
        HttpStatusCode.BadRequest,
        validationResult.Errors.ConvertAll(er => er.ErrorMessage));
    }

    OperationResultResponse<Guid?> response = new();

    List<Guid> imagesIds = null;
    if (request.CommentImages is not null && request.CommentImages.Any())
    {
      imagesIds = await _imageService.CreateImagesAsync(
        request.CommentImages,
        new ResizeParameters(
          maxResizeValue: ResizeMaxValue,
          maxSizeCompress: null,
          previewParameters: new PreviewParameters(
            conditionalWidth: ConditionalWidth,
            conditionalHeight: ConditionalHeight,
            resizeMaxValue: null,
            maxSizeCompress: null)),
        response.Errors);
    }
    
    response.Body = await _repository.CreateAsync(_mapper.Map(request, imagesIds));

    _contextAccessor.HttpContext.Response.StatusCode = response.Body is null
      ? (int)HttpStatusCode.BadRequest
      : (int)HttpStatusCode.Created;

    return response;
  }
}
