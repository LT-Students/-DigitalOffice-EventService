using System;
using System.Collections.Generic;
using LT.DigitalOffice.EventService.Models.Dto.Enums;

namespace LT.DigitalOffice.EventService.Models.Dto.Models
{
  public class EventUserInfo
  {
    public Guid Id { get; set; }
    public EventUserStatus Status { get; set; }
    public DateTime? NotifyAtUtc { get; set; }
    public List<UserInfo> UserInfo { get; set; }
  }
}
