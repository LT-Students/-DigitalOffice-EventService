using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Broker.Requests.Interfaces;

  [AutoInject]
  public interface IEmailService
  {
    public Task SendAsync(string email, string subject, string text);
  }

