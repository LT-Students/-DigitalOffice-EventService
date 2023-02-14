using System;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.EventService.Models.Dto.Requests;

public class CreateEventCategoryRequest
{
  [Required]
  public Guid CategoryId { get; set; }
  [Required]
  public Guid EventId { get; set; }
}
