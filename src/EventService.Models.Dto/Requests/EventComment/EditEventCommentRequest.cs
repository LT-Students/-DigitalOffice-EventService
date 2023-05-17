namespace LT.DigitalOffice.EventService.Models.Dto.Requests.EventComment;

public record EditEventCommentRequest
{
  public string Content { get; set; }
  public bool IsActive { get; set; }
}
