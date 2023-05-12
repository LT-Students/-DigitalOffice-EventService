using System;

namespace LT.DigitalOffice.EventService.Models.Dto.Models;

public record UserBirthdayInfo
{
  public Guid UserId { get; set; }
  public DateTime DateOfBirth { get; set; }
}
