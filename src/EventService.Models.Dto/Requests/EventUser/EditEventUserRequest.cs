using System;
using LT.DigitalOffice.EventService.Models.Dto.Enums;

namespace LT.DigitalOffice.EventService.Models.Dto.Requests.EventUser;

public class EditEventUserRequest
{
  public EventUserStatus Status { get; set; }
  public DateTime? NotifyAtUtc { get; set; }
}
