using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Broker.Requests.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.Helpers;
using LT.DigitalOffice.Models.Broker.Requests.Email;
using MassTransit;

namespace LT.DigitalOffice.EventService.Broker.Requests;

  public class EmailService : IEmailService
  {
    private readonly IRequestClient<ISendEmailRequest> _rcSendEmail;

    public EmailService(IRequestClient<ISendEmailRequest> rcSendEmail)
    {
      _rcSendEmail = rcSendEmail;
    }

    public async Task SendAsync(string email, string subject, string text)
    {
      await _rcSendEmail.ProcessRequest<ISendEmailRequest, bool>(
        ISendEmailRequest.CreateObj(
          email, 
          subject, 
          text));
    }
  }

