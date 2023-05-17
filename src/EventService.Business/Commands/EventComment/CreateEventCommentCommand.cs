using System;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using LT.DigitalOffice.EventService.Business.Commands.EventComment.Interfaces;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Mappers.Db.Interfaces;
using LT.DigitalOffice.EventService.Models.Db;
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

  public CreateEventCommentCommand(
    IEventCommentRepository repository,
    ICreateEventCommentRequestValidator validator,
    IDbEventCommentMapper mapper,
    IResponseCreator responseCreator,
    IHttpContextAccessor contextAccessor)
  {
    _repository = repository;
    _mapper = mapper;
    _validator = validator;
    _responseCreator = responseCreator;
    _contextAccessor = contextAccessor;
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

    DbEventComment dbEventComment = _mapper.Map(request);

    OperationResultResponse<Guid?> response = new()
    {
      Body = await _repository.CreateAsync(dbEventComment)
    };
    
    _contextAccessor.HttpContext.Response.StatusCode = response.Body is null
      ? (int)HttpStatusCode.BadRequest
      : (int)HttpStatusCode.Created;

    return response;
  }
}
