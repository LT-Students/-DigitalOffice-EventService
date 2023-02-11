using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Broker.Requests.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.Helpers;
using LT.DigitalOffice.Models.Broker.Requests.Email;
using MassTransit;

namespace LT.DigitalOffice.EventService.Broker.Requests;

  public class EmailService : IEmailService
  {
    private readonly IRequestClient<ISendEmailRequest> _requestClient;

    public EmailService(IRequestClient<ISendEmailRequest> requestClient)
    {
      _requestClient = requestClient;
    }

    public async Task SendAsync(string email, string subject, string text)
    {
      object request = ISendEmailRequest.CreateObj(email, subject, text);

      await _requestClient.ProcessRequest<ISendEmailRequest, bool>(request);
    }
  }

