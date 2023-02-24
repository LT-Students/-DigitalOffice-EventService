using System;
using LT.DigitalOffice.EventService.Models.Dto.Enums;

namespace LT.DigitalOffice.EventService.Models.Dto.Requests.Category
{
  public class CreateCategoryRequest
  {
    public string Name { get; set; }
    public Color Color { get; set; }
  }
}
