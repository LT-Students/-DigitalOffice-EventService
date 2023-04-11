using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LT.DigitalOffice.EventService.Models.Dto.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum EventUserStatus
{
  Invited,
  Refused,
  Participant,
  Discarded
}
