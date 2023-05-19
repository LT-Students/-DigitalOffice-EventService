using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.EventService.Models.Dto.Requests.Image;

public record RemoveImageRequest
{
  public Guid EventId { get; set; }
  public List<Guid> ImagesIds { get; set; }
}
