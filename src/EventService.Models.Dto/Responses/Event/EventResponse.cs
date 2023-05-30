using System;
using LT.DigitalOffice.EventService.Models.Dto.Enums;

namespace LT.DigitalOffice.EventService.Models.Dto.Responses.Event;

public class EventResponse
{
  public Guid Id { get; set; }
  public string Name { get; set; }
  public string Address { get; set; }
  public string Description { get; set; }
  public DateTime Date { get; set; }
  public FormatType Format { get; set; }
  public AccessType Access { get; set; }
  public DateTime CreatedAtUtc { get; set; }
}
