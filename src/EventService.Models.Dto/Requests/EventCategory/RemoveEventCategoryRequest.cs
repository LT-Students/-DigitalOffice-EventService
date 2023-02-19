using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.EventService.Models.Dto.Requests.EventCategory;

public class RemoveEventCategoryRequest
{
  public Guid EventId { get; set; }
  public List<Guid> EventCategoryIds { get; set; }
}
