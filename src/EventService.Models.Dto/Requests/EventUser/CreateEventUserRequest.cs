using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LT.DigitalOffice.EventService.Models.Dto.Enums;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventUser;

namespace LT.DigitalOffice.EventService.Models.Dto.Requests.EventsUsers;

  public record CreateEventUserRequest
  {
    [Required]
    public Guid EventId { get; set; }
    [Required]
    public List<UserRequest> Users { get; set; }
  }

