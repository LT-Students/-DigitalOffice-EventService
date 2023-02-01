using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using LT.DigitalOffice.EventService.Business.Commands.EventsUsers.Interfaces;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Mappers.Db.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventsUsers;
using LT.DigitalOffice.EventService.Validation.EventUser.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.EventService.Business.Commands.EventsUsers;

	public class CreateEventUserCommand : ICreateEventUserCommand
  {
    private readonly IAccessValidator _accessValidator;
    private readonly IEventUserRepository _repository;
    private readonly IDbEventUserMapper _mapper;
    private readonly ICreateEventUserRequestValidator _validator;
    private readonly IResponseCreator _responseCreator;
    private readonly IHttpContextAccessor _contextAccessor;

    public CreateEventUserCommand(
      IAccessValidator accessValidator,
      IEventUserRepository repository,
      IDbEventUserMapper mapper,
      ICreateEventUserRequestValidator validator,
      IResponseCreator responseCreator,
      IHttpContextAccessor contextAccessor)
    {
      _accessValidator = accessValidator;
      _repository = repository;
      _mapper = mapper;
      _validator = validator;
      _responseCreator = responseCreator;
      _contextAccessor = contextAccessor;
    }
    public async Task<OperationResultResponse<Guid?>> ExecuteAsync(CreateEventUserRequest request)
    {
      if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveUsers))
      {
        return _responseCreator.CreateFailureResponse<Guid?>(HttpStatusCode.Forbidden);
      }

      ValidationResult validationResult = _validator.Validate(request);

      if (!validationResult.IsValid)
      {
        return _responseCreator.CreateFailureResponse<Guid?>(
          HttpStatusCode.BadRequest,
          validationResult.Errors.Select(er => er.ErrorMessage).ToList());
      }

      OperationResultResponse<Guid?> response = new();

      response.Body = await _repository.CreateAsync(_mapper.Map(request));

      _contextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;

      return response;
    }
	}

