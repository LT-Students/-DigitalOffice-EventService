using System;
using LT.DigitalOffice.Kernel.Requests;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.EventService.Models.Dto.Requests.File;

public record FindEventFilesFilter : BaseFindFilter
{
  [FromQuery(Name = "eventid")]
  public Guid EventId { get; set; }
}
