using LT.DigitalOffice.EventService.Models.Dto.Enums;
using System.Collections.Generic;
using System;

namespace LT.DigitalOffice.EventService.Models.Dto.Models;

public class EventInfo
{
  public Guid Id { get; set; }
  public string Name { get; set; }
  public string Description { get; set; }
  public DateTime Date { get; set; }
  public List<CategoryInfo> EventsCategories { get; set; }
}
