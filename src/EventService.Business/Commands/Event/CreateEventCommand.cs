using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DigitalOffice.Models.Broker.Models.Image;
using DigitalOffice.Models.Broker.Models.User;
using FluentValidation.Results;
using LT.DigitalOffice.EventService.Broker.Requests.Interfaces;
using LT.DigitalOffice.EventService.Business.Commands.Event.Interfaces;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Mappers.Db.Interfaces;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Category;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Event;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventUser;
using LT.DigitalOffice.EventService.Validation.Event.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace LT.DigitalOffice.EventService.Business.Commands.Event;

public class CreateEventCommand : ICreateEventCommand
{
  private readonly IEventRepository _eventRepository;
  private readonly ICreateEventRequestValidator _validator;
  private readonly IDbEventMapper _eventMapper;
  private readonly IAccessValidator _accessValidator;
  private readonly IResponseCreator _responseCreator;
  private readonly IHttpContextAccessor _contextAccessor;
  private readonly IEmailService _emailService;
  private readonly IUserService _userService;
  private readonly IImageService _imageService;
  private readonly IDbCategoryMapper _categoryMapper;
  private readonly ICategoryRepository _categoryRepository;

  private const int ResizeMaxValue = 1000;
  private const int ConditionalWidth = 4;
  private const int ConditionalHeight = 3;

  private async Task SendInviteEmailsAsync(List<Guid> userIds, string eventName)
  {
    List<UserData> usersData = await _userService.GetUsersDataAsync(userIds);

    if (usersData is null || !usersData.Any())
    {
      return;
    }

    foreach (UserData user in usersData)
    {
      await _emailService.SendAsync(
        user.Email,
        "Invite to event",
        $"You have been invited to event {eventName}");
    }
  }

  public CreateEventCommand(
    IEventRepository repository,
    ICreateEventRequestValidator validator,
    IDbEventMapper eventMapper,
    IAccessValidator accessValidator,
    IResponseCreator responseCreator,
    IHttpContextAccessor contextAccessor,
    IUserService userService,
    IEmailService emailService,
    IImageService imageService,
    IDbCategoryMapper categoryMapper,
    ICategoryRepository categoryRepository)
  {
    _eventRepository = repository;
    _eventMapper = eventMapper;
    _validator = validator;
    _accessValidator = accessValidator;
    _responseCreator = responseCreator;
    _contextAccessor = contextAccessor;
    _userService = userService;
    _emailService = emailService;
    _imageService = imageService;
    _categoryMapper = categoryMapper;
    _categoryRepository = categoryRepository;
  }

  public async Task<OperationResultResponse<Guid?>> ExecuteAsync(CreateEventRequest request)
  {
    Guid senderId = _contextAccessor.HttpContext.GetUserId();

    if (!await _accessValidator.HasRightsAsync(senderId, Rights.AddEditRemoveUsers))
    {
      return _responseCreator.CreateFailureResponse<Guid?>(HttpStatusCode.Forbidden);
    }

    OperationResultResponse<Guid?> response = new();

    request.Users.Add(new UserRequest { UserId = senderId });
    request.Users = request.Users.Distinct().ToList();
    request.CategoriesIds = request.CategoriesIds?.Distinct().ToList();

    ValidationResult validationResult = await _validator.ValidateAsync(request);
    if (!validationResult.IsValid)
    {
      return _responseCreator.CreateFailureResponse<Guid?>(
        HttpStatusCode.BadRequest,
        validationResult.Errors.ConvertAll(er => er.ErrorMessage));
    }

    List<Guid> imagesIds = null;
    if (request.EventImages is not null && request.EventImages.Any())
    {
      imagesIds = await _imageService.CreateImagesAsync(
        request.EventImages,
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

    DbEvent dbEvent = _eventMapper.Map(request, senderId, imagesIds);

    response.Body = await _eventRepository.CreateAsync(dbEvent);

    await SendInviteEmailsAsync(dbEvent.Users.Select(x => x.UserId).ToList(), dbEvent.Name);

    List<DbCategory> dbCategories = new();
    if (response.Body is not null)
    {
      if (!request.CategoriesRequests.IsNullOrEmpty())
      {
        dbCategories.AddRange(request.CategoriesRequests.ConvertAll(_categoryMapper.Map));
        
        await _categoryRepository.CreateAsync(dbCategories);
      }

      _contextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;
    }
    else
    {
      _contextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }

    return response;
  }
}
