using System.Collections.Generic;

namespace LT.DigitalOffice.EventService.Models.Dto.Responses.Category;

public class FindCategoryResponse
{
    public List<CategoryInfo> CategoryInfo { get; set; }
    public bool IsSuccess { get; set; }
    public List<string> Errors { get; set; }
}
