using System;
using LT.DigitalOffice.Kernel.Requests;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.EventService.Models.Dto.Requests.UserBirthday;

public record FindUsersBirthdaysFilter : BaseFindFilter
{
  [FromQuery(Name = "StartTime")]
  public DateTime StartTime { get; set; }

  [FromQuery(Name = "EndTime")]
  public DateTime EndTime { get; set; }

}
