using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Category;
using LT.DigitalOffice.EventService.Models.Dto.Responses.Category;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Business.Commands.Category.Interfaces;

[AutoInject]
public interface IFindCategoryCommand
{
    public Task<FindCategoryResponse> ExecuteAsync(FindCategoryRequest request);
}
