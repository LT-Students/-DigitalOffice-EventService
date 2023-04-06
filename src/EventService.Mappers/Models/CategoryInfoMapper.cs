using LT.DigitalOffice.EventService.Mappers.Models.Interface;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Models;

namespace LT.DigitalOffice.EventService.Mappers.Models
{
  public class CategoryInfoMapper : ICategoryInfoMapper
  {
    public CategoryInfo Map(DbCategory dbCategory)
    {
      return new CategoryInfo
      {
        Id = dbCategory.Id, 
        Name = dbCategory.Name, 
        Color = dbCategory.Color, 
        IsActive = dbCategory.IsActive
      };
    }
  }
}
