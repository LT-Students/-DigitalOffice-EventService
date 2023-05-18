using System;

namespace LT.DigitalOffice.EventService.Models.Dto.Models
{
  public record FileInfo
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Extension { get; set; }
    public long Size { get; set; }
    public DateTime CreatedAtUtc { get; set; }
  }
}
