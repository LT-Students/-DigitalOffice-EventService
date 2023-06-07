using System;
using LT.DigitalOffice.Kernel.Requests;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.EventService.Models.Dto.Requests.File;

public record FindFilesFilter : BaseFindFilter
{
  [FromQuery(Name = "entityid")]
  public Guid EntityId { get; set; }
}
