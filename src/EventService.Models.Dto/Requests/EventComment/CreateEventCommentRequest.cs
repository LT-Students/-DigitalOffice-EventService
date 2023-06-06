using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.EventService.Models.Dto.Requests.EventComment;

public record CreateEventCommentRequest
{
  [Required]
  public string Content { get; set; }
  public Guid UserId { get; set; }
  public Guid EventId { get; set; }
  public Guid? ParentId { get; set; }
  public List<ImageContent> CommentImages { get; set; }
}
