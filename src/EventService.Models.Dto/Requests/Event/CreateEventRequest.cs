using System;
using System.ComponentModel.DataAnnotations;
using LT.DigitalOffice.EventService.Models.Dto.Enums;

namespace LT.DigitalOffice.EventService.Models.Dto.Requests.Event;

public class CreateEventRequest
{
  [Required]
  public string Name { get; set; }
  [Required]
  public string Address { get; set; }
  public string Description { get; set; }
  [Required]
  public DateTime Date { get; set; }
  [Required]
  public FormatType Format { get; set; }
  [Required]
  public AccessType Access { get; set; }
}
