using LT.DigitalOffice.Kernel.BrokerSupport.Attributes;
using LT.DigitalOffice.Kernel.BrokerSupport.Configurations;
using LT.DigitalOffice.Models.Broker.Common;
using LT.DigitalOffice.Models.Broker.Requests.Email;
using LT.DigitalOffice.Models.Broker.Requests.User;

namespace LT.DigitalOffice.EventService.Models.Dto.Configurations;

public class RabbitMqConfig : BaseRabbitMqConfig
{
  [AutoInjectRequest(typeof(ICheckUsersExistence))]
  public string CheckUsersExistenceEndpoint { get; set; }

  [AutoInjectRequest(typeof(ISendEmailRequest))]
  public string SendEmailEndpoint { get; set; }

  [AutoInjectRequest(typeof(IGetUsersDataRequest))]
  public string GetUsersDataEndpoint { get; set; }

  [AutoInjectRequest(typeof(IFilteredUsersDataRequest))]
  public string FilteredUsersDataEndpoint { get; set; }
}
