using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.EventService.Models.Dto.Models;

public class CommentInfo
{
  public Guid Id { get; set; }
  public string Content { get; set; }
  public Guid UserId { get; set; }
  public Guid? ParentId { get; set; }
  public List<CommentInfo> Comment { get; set; }
}
