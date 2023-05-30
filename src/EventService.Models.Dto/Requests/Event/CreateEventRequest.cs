using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LT.DigitalOffice.EventService.Models.Dto.Enums;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Category;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventUser;

namespace LT.DigitalOffice.EventService.Models.Dto.Requests.Event;

public record CreateEventRequest
{
  [Required]
  public string Name { get; set; }
  public string Address { get; set; }
  public string Description { get; set; }
  public DateTime Date { get; set; }
  public DateTime? EndDate { get; set; }
  public FormatType Format { get; set; }
  public AccessType Access { get; set; }
  [Required]
  public List<UserRequest> Users { get; set; }
  public List<Guid> CategoriesIds { get; set; }
  public List<CreateCategoryRequest> CategoriesRequests { get; set; }
  public List<ImageContent> EventImages { get; set; }
}
