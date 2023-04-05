using LT.DigitalOffice.EventService.Models.Dto.Enums;
using LT.DigitalOffice.Kernel.Requests;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.EventService.Models.Dto.Requests.Category;

public record FindCategoriesFilter : BaseFindFilter
{
  [FromQuery(Name = "userFullNameIncludeSubstring")]
  public string UserFullNameIncludeSubstring { get; set; }

  [FromQuery(Name = "color")]
  public CategoryColor Color { get; set; }
}

