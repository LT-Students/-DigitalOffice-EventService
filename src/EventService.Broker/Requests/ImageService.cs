using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigitalOffice.Models.Broker.Models.Image;
using LT.DigitalOffice.EventService.Broker.Requests.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.BrokerSupport.Helpers;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Models.Broker.Enums;
using LT.DigitalOffice.Models.Broker.Models.Image;
using LT.DigitalOffice.Models.Broker.Requests.Image;
using LT.DigitalOffice.Models.Broker.Responses.Image;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.EventService.Broker.Requests;

public class ImageService : IImageService
{
  private readonly ILogger<ImageService> _logger;
  private readonly IRequestClient<ICreateImagesRequest> _rcCreateImages;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public ImageService(
    ILogger<ImageService> logger,
    IRequestClient<ICreateImagesRequest> rcCreateImages,
    IHttpContextAccessor httpContextAccessor)
  {
    _logger = logger;
    _rcCreateImages = rcCreateImages;
    _httpContextAccessor = httpContextAccessor;
  }

  public async Task<List<Guid>> CreateImagesAsync(List<ImageContent> eventImages, ResizeParameters resizeParameters, List<string> errors = null)
  {
    return eventImages is null || !eventImages.Any()
      ? null
      : (await RequestHandler
        .ProcessRequest<ICreateImagesRequest, ICreateImagesResponse>(
          _rcCreateImages,
          ICreateImagesRequest.CreateObj(
            images: eventImages.Select(x => new CreateImageData(x.Name, x.Content, x.Extension, resizeParameters)).ToList(),
            imageSource: ImageSource.Event,
            createdBy: _httpContextAccessor.HttpContext.GetUserId()),
          errors,
          _logger)).ImagesIds;
  }
}
