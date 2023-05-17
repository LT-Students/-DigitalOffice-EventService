using System;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.EventService.Models.Dto.Requests.EventComment;

public record CreateEventCommentRequest
{
  [Required]
  [MaxLength(300)]
  public string Content { get; set; }
  public Guid UserId { get; set; }
  public Guid EventId { get; set; }
  public Guid? ParentId { get; set; }
}
