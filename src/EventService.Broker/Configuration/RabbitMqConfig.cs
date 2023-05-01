using DigitalOffice.Kernel.BrokerSupport.Attributes;
using DigitalOffice.Models.Broker.Requests.User;
using LT.DigitalOffice.EventService.Broker.Consumers;
using LT.DigitalOffice.Kernel.BrokerSupport.Attributes;
using LT.DigitalOffice.Kernel.BrokerSupport.Configurations;
using LT.DigitalOffice.Models.Broker.Common;
using LT.DigitalOffice.Models.Broker.Requests.Email;
using LT.DigitalOffice.Models.Broker.Requests.User;

namespace LT.DigitalOffice.EventService.Broker.Configuration;

public class RabbitMqConfig : BaseRabbitMqConfig
{
  #region receive endpoints

  [MassTransitEndpoint(typeof(UpdateUserBirthdayConsumer))]
  public string UpdateUserBirthdayEndpoint { get; init; }

  #endregion

  // user

  [AutoInjectRequest(typeof(ICheckUsersExistence))]
  public string CheckUsersExistenceEndpoint { get; set; }

  [AutoInjectRequest(typeof(IGetUsersDataRequest))]
  public string GetUsersDataEndpoint { get; set; }

  [AutoInjectRequest(typeof(IFilteredUsersDataRequest))]
  public string FilteredUsersDataEndpoint { get; set; }

  [AutoInjectRequest(typeof(IGetUsersBirthdaysRequest))]
  public string GetUsersBirthdaysEndpoint { get; set; }

  //Email

  [AutoInjectRequest(typeof(ISendEmailRequest))]
  public string SendEmailEndpoint { get; set; }
}
