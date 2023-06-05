using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Event;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.EventService.Data.Interfaces;

[AutoInject]
public interface IEventRepository
{
  Task<Guid?> CreateAsync(DbEvent dbEvent);
  Task<bool> EditAsync(Guid eventId, JsonPatchDocument<DbEvent> request);
  Task<bool> DoesExistAsync(Guid eventId, bool? isActive);
  Task<bool> IsEventCompletedAsync(Guid eventId);
  Task<List<Guid>> GetExisting(List<Guid> eventsIds);
  Task<DbEvent> GetAsync(Guid eventId, GetEventFilter filter = null);
  Task<(List<DbEvent>, int totalCount)> FindAsync(
    FindEventsFilter filter,
    CancellationToken ct);
  Task<DbEvent> GetAsync(Guid eventId);
  Task<(List<Guid> filesIds, List<Guid> imagesIds)> RemoveImagesFilesAsync(Guid eventId);
}
