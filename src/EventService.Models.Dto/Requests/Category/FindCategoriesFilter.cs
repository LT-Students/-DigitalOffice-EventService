using LT.DigitalOffice.EventService.Models.Dto.Enums;
using LT.DigitalOffice.Kernel.Requests;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.EventService.Models.Dto.Requests.Category;

public record FindCategoriesFilter : BaseFindFilter
{
  [FromQuery(Name = "nameIncludeSubstring")]
  public string NameIncludeSubstring { get; set; }

  [FromQuery(Name = "color")]
  public CategoryColor? Color { get; set; }
  
  [FromQuery(Name = "includeDeactivated")]
  public bool IncludeDeactivated { get; set; }
}

