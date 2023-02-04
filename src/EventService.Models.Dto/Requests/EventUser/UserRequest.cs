using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.EventService.Models.Dto.Requests.EventUser;

  public class UserRequest
  {
    [Required]
    public Guid UserId { get; set; }
    public DateTime? NotifyAtUtc { get; set; }
  }

