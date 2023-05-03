using System.Threading.Tasks;
using DigitalOffice.Models.Broker.Publishing.Subscriber.User;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Mappers.Db.Interfaces;
using MassTransit;

namespace LT.DigitalOffice.EventService.Broker.Consumers
{
  public class UpdateUserBirthdayConsumer : IConsumer<IUpdateUserBirthdayPublish>
  {
    private readonly IUserBirthdayRepository _userBirthdayRepository;

    private async Task UpdateUserBirthdayAsync(IUpdateUserBirthdayPublish publish)
    {
      if (publish is null)
      {
        return;
      }

      await _userBirthdayRepository.UpdateUserBirthdayAsync(publish.UserId, publish.DateOfBirth);
    }

    public UpdateUserBirthdayConsumer(
      IUserBirthdayRepository userBirthdayRepository)
    {
      _userBirthdayRepository = userBirthdayRepository;
    }

    public async Task Consume(ConsumeContext<IUpdateUserBirthdayPublish> context)
    {
      await UpdateUserBirthdayAsync(context.Message);
    }
  }
}
