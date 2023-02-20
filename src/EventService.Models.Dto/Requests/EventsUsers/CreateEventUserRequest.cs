using System;
using System.ComponentModel.DataAnnotations;
using LT.DigitalOffice.EventService.Models.Dto.Enums;

namespace LT.DigitalOffice.EventService.Models.Dto.Requests.EventsUsers
{
  public record CreateEventUserRequest
  {
    [Required] public Guid EventId { get; set; }

    [Required] public Guid UserId { get; set; }

    public EventUserStatus UserStatus { get; set; }
    public DateTime? NotifyAtUtc { get; set; }
  }
}
