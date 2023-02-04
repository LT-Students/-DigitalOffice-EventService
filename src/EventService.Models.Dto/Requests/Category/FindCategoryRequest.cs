using LT.DigitalOffice.EventService.Models.Dto.Enums;

namespace LT.DigitalOffice.EventService.Models.Dto.Requests.Category;

public class FindCategoryRequest
{
    public string Name { get; set; }
    [Newtonsoft.Json.JsonProperty("Color")]
    public Color Color { get; set; }
}
