using System;

namespace LT.DigitalOffice.EventService.Models.Dto.Models;

public class UserInfo
{
  public Guid UserId { get; set; }
  public string FirstName { get; set; }
  public string MiddleName { get; set; }
  public string LastName { get; set; }
  public Guid? ImageId { get; set; }
}
