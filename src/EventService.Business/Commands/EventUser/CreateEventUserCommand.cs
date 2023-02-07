using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using LT.DigitalOffice.EventService.Business.Commands.EventUser.Interfaces;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Mappers.Db.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Enums;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventUser;
using LT.DigitalOffice.EventService.Validation.EventUser.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.EventService.Business.Commands.EventUser;

	public class CreateEventUserCommand : ICreateEventUserCommand
  {
    private readonly IAccessValidator _accessValidator;
    private readonly IEventUserRepository _repository;
    private readonly IDbEventUserMapper _mapper;
    private readonly ICreateEventUserRequestValidator _validator;
    private readonly IResponseCreator _responseCreator;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IEventRepository _eventRepository;

    public CreateEventUserCommand(
      IAccessValidator accessValidator,
      IEventUserRepository repository,
      IDbEventUserMapper mapper,
      ICreateEventUserRequestValidator validator,
      IResponseCreator responseCreator,
      IHttpContextAccessor contextAccessor,
      IEventRepository eventRepository)
    {
      _accessValidator = accessValidator;
      _repository = repository;
      _mapper = mapper;
      _validator = validator;
      _responseCreator = responseCreator;
      _contextAccessor = contextAccessor;
      _eventRepository = eventRepository;
    }
    public async Task<OperationResultResponse<bool>> ExecuteAsync(CreateEventUserRequest request)
    {
      Guid senderId = _contextAccessor.HttpContext.GetUserId();
      AccessType eventType = (await _eventRepository.GetAsync(request.EventId)).Access;

      if (!await _accessValidator.HasRightsAsync(senderId, Rights.AddEditRemoveUsers) ||
          (eventType == AccessType.Closed && request.Users.Exists(x => x.UserId == senderId)))
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
      }

      if (request.Users.Exists(x => x.UserId == senderId) && request.Users.Count > 1)
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.BadRequest);
      }

      ValidationResult validationResult = await _validator.ValidateAsync(request);

      if (!validationResult.IsValid)
      {
        return _responseCreator.CreateFailureResponse<bool>(
          HttpStatusCode.BadRequest,
          validationResult.Errors.Select(er => er.ErrorMessage).ToList());
      }

      if (request.Users.Exists(x => x.UserId == senderId))
      {
        await _repository.CreateAsync(_mapper.Map(request, EventUserStatus.Participant));

        _contextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;
        return new OperationResultResponse<bool>
        {
          Body = true
        };
      }

      string error = null;
 
      if (request.Users.Distinct().Count() != request.Users.Count())
      {
        error = "Some users were doubled";
        request.Users = request.Users.Distinct().ToList();
      }

      await _repository.CreateAsync(_mapper.Map(request));
      _contextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;

      return new OperationResultResponse<bool> { Body = true, Errors = new List<string>() { error } };
    }
	}

