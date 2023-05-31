using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigitalOffice.Models.Broker.Models.Image;
using LT.DigitalOffice.EventService.Models.Dto.Models;
using LT.DigitalOffice.EventService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Broker.Requests.Interfaces;

[AutoInject]
public interface IImageService
{
  Task<List<Guid>> CreateImagesAsync(List<ImageContent> eventImages, ResizeParameters resizeParameters, List<string> errors = null);
  Task<List<ImageInfo>> GetImagesAsync(List<Guid> imagesIds, List<string> errors = null);
}
