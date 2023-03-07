using System;
using LT.DigitalOffice.EventService.Models.Dto.Enums;

namespace LT.DigitalOffice.EventService.Models.Dto;

public class CategoryInfo
{
  public Guid Id { get; set; }
  public string Name { get; set; }
  public CategoryColor Color { get; set; }
}
