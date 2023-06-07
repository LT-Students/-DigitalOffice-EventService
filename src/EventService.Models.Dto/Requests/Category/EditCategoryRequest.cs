using LT.DigitalOffice.EventService.Models.Dto.Enums;

namespace LT.DigitalOffice.EventService.Models.Dto.Requests.Category;

public class EditCategoryRequest
{
  public string Name { get; set; }
  public CategoryColor Color { get; set; }
  public bool IsActive { get; set; }
}
